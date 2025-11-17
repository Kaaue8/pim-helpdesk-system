using Microsoft.EntityFrameworkCore;
using HelpDesk.Api.Models;
using BCrypt.Net;
using System; // Adicionado para DateTime

namespace HelpDesk.Api.Data
{
    public class AppDbContext : DbContext
    {
        // Adicionado para suprimir o erro de modelo não determinístico
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        }
        // 1. CONSTRUTOR SEM PARÂMETROS (Para o Add-Migration)
        public AppDbContext()
        {
        }

        // 2. CONSTRUTOR COM PARÂMETROS (Para a Aplicação)
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // 3. DBSETS (As tabelas)
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Setor> Setores { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<LogAuditoria> LogsAuditoria { get; set; }

        // 4. ONMODELCREATING (Configurações avançadas e Data Seeding)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // --- Configuração de Relacionamentos (Para desambiguar avisos do EF Core) ---

            // Relacionamentos LogAuditoria <-> Usuario (Múltiplos relacionamentos)
            modelBuilder.Entity<LogAuditoria>()
                .HasOne(l => l.UsuarioAfetado)
                .WithMany(u => u.LogsAfetados) // Propriedade de navegação no Usuario.cs
                .HasForeignKey(l => l.IdUsuarioAfetado)
                .OnDelete(DeleteBehavior.Restrict); // Evita exclusão em cascata

            modelBuilder.Entity<LogAuditoria>()
                .HasOne(l => l.UsuarioExecutor)
                .WithMany(u => u.LogsExecutados) // Propriedade de navegação no Usuario.cs
                .HasForeignKey(l => l.IdUsuarioExecutor)
                .OnDelete(DeleteBehavior.Restrict); // Evita exclusão em cascata

            // Relacionamentos Ticket <-> Usuario (Múltiplos relacionamentos)
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Usuario) // O usuário que abriu o ticket
                .WithMany(u => u.TicketsAbertos) // Propriedade de navegação no Usuario.cs
                .HasForeignKey(t => t.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict); // Evita exclusão em cascata

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Tecnico) // O técnico que atende o ticket
                .WithMany(u => u.TicketsAtribuidos) // Propriedade de navegação no Usuario.cs
                .HasForeignKey(t => t.TecnicoId)
                .OnDelete(DeleteBehavior.Restrict); // Evita exclusão em cascata

            // --- Fim da Configuração de Relacionamentos ---

            // --- Data Seeding (Dados Iniciais) ---

            // 1. Setores
            modelBuilder.Entity<Setor>().HasData(
                new Setor { IdSetor = 1, Nome = "Financeiro" },
                new Setor { IdSetor = 2, Nome = "TI" },
                new Setor { IdSetor = 3, Nome = "RH" },
                new Setor { IdSetor = 4, Nome = "Administrativo" }
            );

            // 2. Categorias
            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { IdCategoria = 1, Nome = "Hardware" },
                new Categoria { IdCategoria = 2, Nome = "Software" },
                new Categoria { IdCategoria = 3, Nome = "Acesso" },
                new Categoria { IdCategoria = 4, Nome = "Rede" },
                new Categoria { IdCategoria = 5, Nome = "Sistema" }
            );

            // 3. Usuários (Todos com a senha "123456" hasheada)
            string senhaHash = BCrypt.Net.BCrypt.HashPassword("123456");

            // Usando uma data estática para evitar o erro de modelo não determinístico
            var dataEstatica = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<Usuario>().HasData(
                // Admin 1
                new Usuario
                {
                    IdUsuario = 1,
                    Nome = "Kaue",
                    Email = "kaue@empresa.com",
                    SenhaHash = senhaHash,
                    Perfil = "Admin",
                    DataCriacao = dataEstatica.AddDays(-10), // Data estática
                    SetorId = 1 // Financeiro
                },
                // Admin 2
                new Usuario
                {
                    IdUsuario = 2,
                    Nome = "Julia Castro",
                    Email = "julia.castro@empresa.com",
                    SenhaHash = senhaHash,
                    Perfil = "Admin",
                    DataCriacao = dataEstatica.AddDays(-9), // Data estática
                    SetorId = 1 // Financeiro
                },
                // Usuario Comum 1
                new Usuario
                {
                    IdUsuario = 3,
                    Nome = "Vinicius Wayna",
                    Email = "vinicius.wayna@empresa.com",
                    SenhaHash = senhaHash,
                    Perfil = "Usuario",
                    DataCriacao = dataEstatica.AddDays(-8), // Data estática
                    SetorId = 3 // RH
                },
                // Admin
                new Usuario
                {
                    IdUsuario = 6,
                    Nome = "Vinicius Macedo",
                    Email = "vinicius.macedo@empresa.com",
                    SenhaHash = senhaHash,
                    Perfil = "Admin",
                    DataCriacao = dataEstatica.AddDays(-7), // Data estática
                    SetorId = 4 // Administrativo
                },
                // Usuario Comum 2
                new Usuario
                {
                    IdUsuario = 7,
                    Nome = "Irineu Julio",
                    Email = "irineu.julio@empresa.com",
                    SenhaHash = senhaHash,
                    Perfil = "Usuario",
                    DataCriacao = dataEstatica.AddDays(-6), // Data estática
                    SetorId = 2 // TI
                },
                // Usuario Comum 3
                new Usuario
                {
                    IdUsuario = 8,
                    Nome = "Samuel NObRe",
                    Email = "samuel.nobre@empresa.com",
                    SenhaHash = senhaHash,
                    Perfil = "Usuario",
                    DataCriacao = dataEstatica.AddDays(-5), // Data estática
                    SetorId = 3 // RH
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}