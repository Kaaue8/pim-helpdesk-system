using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HelpDesk.Api.Data;
using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization; // Adicionado

// Controllers/RelatoriosController.cs

[Authorize] // Adicionado para proteger o Controller
[ApiController]
[Route("api/[controller]")]
public class RelatoriosController : ControllerBase
{
    private readonly AppDbContext _context;

    public RelatoriosController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Relatorios/Status
    // Retorna a contagem de tickets por status (Aberto, Em Análise, Encerrado)
    [HttpGet("Status")]
    public async Task<ActionResult> GetTicketsPorStatus()
    {
        var ticketsPorStatus = await _context.Tickets
            .GroupBy(t => t.Status)
            .Select(g => new
            {
                Status = g.Key,
                Contagem = g.Count()
            })
            .ToListAsync();

        return Ok(ticketsPorStatus);
    }

    // GET: api/Relatorios/Setor
    // Retorna a contagem de tickets por setor
    [HttpGet("Setor")]
    public async Task<ActionResult> GetTicketsPorSetor()
    {
        // Usando o Join para ligar Tickets -> Usuários -> Setores
        var ticketsPorSetor = await _context.Tickets
            .Join(_context.Usuarios, // 1. Junta Tickets com Usuários
                  ticket => ticket.UsuarioId,
                  usuario => usuario.Id,
                  (ticket, usuario) => new { ticket, usuario })
            .Join(_context.Setores, // 2. Junta o resultado anterior com Setores
                  t => t.usuario.SetorIdSetor, // Chave estrangeira no Usuário (assumindo SetorIdSetor)
                  setor => setor.IdSetor, // Chave primária no Setor
                  (t, setor) => new { t.ticket, setor }) // Projeta para incluir o Ticket e o Setor
            .GroupBy(t => t.setor.NomeSetor) // Agrupa pelo Nome do Setor
            .Select(g => new
            {
                Setor = g.Key,
                Contagem = g.Count()
            })
            .ToListAsync();

        return Ok(ticketsPorSetor);
    }
}