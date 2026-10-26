using System.Data.Common;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace HelpDesk.Api.Data;

/// <summary>
/// Toda vez que o EF Core abre uma conexão com o SQL Server, este interceptor
/// grava no SESSION_CONTEXT quem é o usuário logado e de qual IP ele veio.
///
/// O trigger dbo.trg_Chamados_Auditoria (pim_corrigido.sql) lê:
///   SESSION_CONTEXT(N'id_usuario')  -> LogsAuditoria.id_usuario_executor
///   SESSION_CONTEXT(N'ip_origem')   -> LogsAuditoria.ip_origem
///
/// Colocar em: Data/SessionContextInterceptor.cs
/// O SQL Server limpa o SESSION_CONTEXT quando a conexão volta ao pool,
/// então um usuário nunca "herda" a identidade de outro.
/// </summary>
public sealed class SessionContextInterceptor : DbConnectionInterceptor
{
    private readonly IHttpContextAccessor _http;

    public SessionContextInterceptor(IHttpContextAccessor http) => _http = http;

    public override void ConnectionOpened(DbConnection connection, ConnectionEndEventData eventData)
    {
        using var cmd = BuildCommand(connection);
        cmd?.ExecuteNonQuery();
    }

    public override async Task ConnectionOpenedAsync(
        DbConnection connection, ConnectionEndEventData eventData, CancellationToken cancellationToken = default)
    {
        await using var cmd = BuildCommand(connection);
        if (cmd is not null)
            await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    private DbCommand? BuildCommand(DbConnection connection)
    {
        var ctx = _http.HttpContext;
        if (ctx is null) return null; // ex.: migrations, jobs, testes sem requisição HTTP

        // JwtBearer converte "sub" em NameIdentifier por padrão; checamos os dois.
        var rawId = ctx.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? ctx.User.FindFirstValue("sub");
        int? userId = int.TryParse(rawId, out var parsed) ? parsed : null;
        var ip = ctx.Connection.RemoteIpAddress?.ToString();

        if (userId is null && ip is null) return null;

        var cmd = connection.CreateCommand();
        cmd.CommandText =
            "EXEC sp_set_session_context @key = N'id_usuario', @value = @uid; " +
            "EXEC sp_set_session_context @key = N'ip_origem',  @value = @ip;";

        var pUid = cmd.CreateParameter();
        pUid.ParameterName = "@uid";
        pUid.Value = userId.HasValue ? userId.Value : DBNull.Value;
        cmd.Parameters.Add(pUid);

        var pIp = cmd.CreateParameter();
        pIp.ParameterName = "@ip";
        pIp.Value = ip is not null ? ip : DBNull.Value;
        cmd.Parameters.Add(pIp);

        return cmd;
    }
}
