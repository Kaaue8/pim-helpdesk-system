using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using HelpDesk.Api.Data;
using HelpDesk.Api.Services;

using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuração: Garante que as variáveis de ambiente sejam lidas
// O ASP.NET Core 8.0 já prioriza variáveis de ambiente, mas garantimos a leitura correta.
var configuration = builder.Configuration;

// 1. Configuração do Banco de Dados (Lendo da variável de ambiente ConnectionStrings__DefaultConnection)
var connectionString = configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    // Em um ambiente de produção, isso deve ser um erro fatal.
    // Para o Render, a variável de ambiente será 'ConnectionStrings__DefaultConnection'.
    throw new InvalidOperationException("Connection string 'DefaultConnection' not configured. Ensure 'ConnectionStrings__DefaultConnection' environment variable is set.");
}
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Configuração do JWT (Lendo das variáveis de ambiente Jwt__Key, Jwt__Issuer, Jwt__Audience)
var jwtKey = configuration["Jwt:Key"];
var jwtIssuer = configuration["Jwt:Issuer"];
var jwtAudience = configuration["Jwt:Audience"];

if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
{
    throw new InvalidOperationException("JWT configuration (Key, Issuer, or Audience) is missing. Ensure Jwt__Key, Jwt__Issuer, and Jwt__Audience environment variables are set.");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// Adicionar serviços ao contêiner.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configuração do Swagger/OpenAPI
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HelpDesk PIM API", Version = "v1" });

    // Adiciona a segurança JWT ao Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insira o token JWT no formato: Bearer {token}",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Adicionar serviços personalizados
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<TicketService>();

// Configuração do Semantic Kernel (Lendo da variável de ambiente OPENAI_API_KEY)
var openAiApiKey = configuration["OPENAI_API_KEY"];
if (string.IsNullOrEmpty(openAiApiKey))
{
    throw new InvalidOperationException("OPENAI_API_KEY environment variable is not set.");
}
builder.Services.AddSingleton<HoustonService>(new HoustonService(openAiApiKey));

// Configuração do CORS para permitir acesso do frontend (ajustar conforme o domínio do Render)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin() // Permitir qualquer origem (para testes iniciais)
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configurar o pipeline de requisição HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Aplicar o CORS
app.UseCors("AllowAll");

// app.UseHttpsRedirection(); // O Render lida com HTTPS no proxy

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Aplica as migrações do banco de dados automaticamente (opcional, mas útil para o primeiro deploy)
// Isso garante que o banco de dados seja criado/atualizado no primeiro deploy.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        // Logar o erro ou lidar com ele. Para o propósito do PIM, um log simples é suficiente.
        Console.WriteLine($"Erro ao aplicar migrações: {ex.Message}");
        // Em um ambiente real, você pode querer relançar a exceção ou parar a aplicação.
    }
}

app.Run();
