using Microsoft.EntityFrameworkCore;
using HelpDesk.Api.Models;
using BCrypt.Net;
using System;

namespace HelpDesk.Api.Data
{
    /// <summary>
    /// Contexto do banco de dados - AppDbContext
    /// ✅ CORRIGIDO para refletir o schema real do Azure SQL Database
    /// </summary>
    public class AppDbContext : DbContext
    {
        // Suprime avisos de modelo não determinístico
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(w => 
                w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        }

        // 1. CONSTRUTOR SEM PARÂMETROS (Para migrations)
        public AppDbContext()
        {
        }

        // 2. CONSTRUTOR COM PARÂMETROS (Para a aplicação)
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // 3. DBSETS (Tabelas do banco de dados)
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Setor> Setores { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<LogAuditoria> LogsAuditoria { get; set; }
        public DbSet<TicketHistorico> TicketHistorico { get; set; }

        // 4. ONMODELCREATING (Configurações de relacionamentos e seed data)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ========================================
            // CONFIGURAÇÃO DE RELACIONAMENTOS
            // ========================================

            // Relacionamento: LogAuditoria -> Usuario (UsuarioAfetado)
            modelBuilder.Entity<LogAuditoria>()
                .HasOne(l => l.UsuarioAfetado)
                .WithMany(u => u.LogsAfetados)
                .HasForeignKey(l => l.IdUsuarioAfetado)
                .OnDelete(DeleteBehavior.Restrict); // Evita exclusão em cascata

            // Relacionamento: LogAuditoria -> Usuario (UsuarioExecutor)
            modelBuilder.Entity<LogAuditoria>()
                .HasOne(l => l.UsuarioExecutor)
                .WithMany(u => u.LogsExecutados)
                .HasForeignKey(l => l.IdUsuarioExecutor)
                .OnDelete(DeleteBehavior.Restrict); // Evita exclusão em cascata

            // Relacionamento: Ticket -> Usuario (Solicitante)
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Solicitante)
                .WithMany(u => u.TicketsAbertos)
                .HasForeignKey(t => t.SolicitanteId)
                .OnDelete(DeleteBehavior.Restrict); // Evita exclusão em cascata

            // Relacionamento: Ticket -> Usuario (Responsavel)
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Responsavel)
                .WithMany(u => u.TicketsAtribuidos)
                .HasForeignKey(t => t.ResponsavelId)
                .OnDelete(DeleteBehavior.Restrict); // Evita exclusão em cascata

            // Relacionamento: Ticket -> Categoria
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Categoria)
                .WithMany(c => c.Tickets)
                .HasForeignKey(t => t.CategoriaId)
                .OnDelete(DeleteBehavior.SetNull); // Se categoria for deletada, FK vira NULL

            // Relacionamento: Usuario -> Setor
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Setor)
                .WithMany(s => s.Usuarios)
                .HasForeignKey(u => u.SetorId)
                .OnDelete(DeleteBehavior.Restrict); // Evita exclusão em cascata

            // Relacionamento: TicketHistorico -> Ticket
            modelBuilder.Entity<TicketHistorico>()
                .HasOne(h => h.Ticket)
                .WithMany(t => t.Historico)
                .HasForeignKey(h => h.TicketId)
                .OnDelete(DeleteBehavior.Cascade); // Se ticket for deletado, histórico também é

            // Relacionamento: TicketHistorico -> Usuario
            modelBuilder.Entity<TicketHistorico>()
                .HasOne(h => h.Usuario)
                .WithMany()
                .HasForeignKey(h => h.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict); // Evita exclusão em cascata

            // ========================================
            // DATA SEEDING (Dados Iniciais)
            // ========================================

            // 1. SETORES
            // ✅ Corrigido: usando propriedade "Nome" em vez de "NomeSetor"
            modelBuilder.Entity<Setor>().HasData(
                new Setor { IdSetor = 1, Nome = "Financeiro", Descricao = "Setor Financeiro", Ativo = true },
                new Setor { IdSetor = 2, Nome = "TI", Descricao = "Tecnologia da Informação", Ativo = true },
                new Setor { IdSetor = 3, Nome = "RH", Descricao = "Recursos Humanos", Ativo = true },
                new Setor { IdSetor = 4, Nome = "Administrativo", Descricao = "Setor Administrativo", Ativo = true }
            );

            // 2. CATEGORIAS
            // ✅ Corrigido: usando propriedade "Nome" em vez de "NomeCategoria"
            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { IdCategoria = 1, Nome = "Hardware", Descricao = "Problemas de hardware", Ativo = true },
                new Categoria { IdCategoria = 2, Nome = "Software", Descricao = "Problemas de software", Ativo = true },
                new Categoria { IdCategoria = 3, Nome = "Acesso", Descricao = "Problemas de acesso", Ativo = true },
                new Categoria { IdCategoria = 4, Nome = "Rede", Descricao = "Problemas de rede", Ativo = true },
                new Categoria { IdCategoria = 5, Nome = "Sistema", Descricao = "Problemas de sistema", Ativo = true }
            );

            // 3. USUÁRIOS
            // ✅ Corrigido: usando "IdUsuario" em vez de "Id" e "SetorId" em vez de "SetorIdSetor"
            // Senha padrão para todos: "123456"
            string senhaHash = BCrypt.Net.BCrypt.HashPassword("123456");

            // Data estática para evitar erro de modelo não determinístico
            var dataEstatica = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<Usuario>().HasData(
                // Administrador 1
                new Usuario
                {
                    IdUsuario = 1,
                    Nome = "Kaue",
                    Email = "kaue@empresa.com",
                    SenhaHash = senhaHash,
                    Perfil = "Admin",
                    DataCriacao = dataEstatica.AddDays(-10),
                    SetorId = 1, // Financeiro
                    Ativo = true
                },
                // Administrador 2
                new Usuario
                {
                    IdUsuario = 2,
                    Nome = "Julia Castro",
                    Email = "julia.castro@empresa.com",
                    SenhaHash = senhaHash,
                    Perfil = "Admin",
                    DataCriacao = dataEstatica.AddDays(-9),
                    SetorId = 1, // Financeiro
                    Ativo = true
                },
                // Usuário Comum 1
                new Usuario
                {
                    IdUsuario = 3,
                    Nome = "Vinicius Wayna",
                    Email = "vinicius.wayna@empresa.com",
                    SenhaHash = senhaHash,
                    Perfil = "Usuario",
                    DataCriacao = dataEstatica.AddDays(-8),
                    SetorId = 3, // RH
                    Ativo = true
                },
                // Administrador 3
                new Usuario
                {
                    IdUsuario = 6,
                    Nome = "Vinicius Macedo",
                    Email = "vinicius.macedo@empresa.com",
                    SenhaHash = senhaHash,
                    Perfil = "Admin",
                    DataCriacao = dataEstatica.AddDays(-7),
                    SetorId = 4, // Administrativo
                    Ativo = true
                },
                // Usuário Comum 2
                new Usuario
                {
                    IdUsuario = 7,
                    Nome = "Irineu Julio",
                    Email = "irineu.julio@empresa.com",
                    SenhaHash = senhaHash,
                    Perfil = "Usuario",
                    DataCriacao = dataEstatica.AddDays(-6),
                    SetorId = 2, // TI
                    Ativo = true
                },
                // Usuário Comum 3
                new Usuario
                {
                    IdUsuario = 8,
                    Nome = "Samuel Nobre",
                    Email = "samuel.nobre@empresa.com",
                    SenhaHash = senhaHash,
                    Perfil = "Usuario",
                    DataCriacao = dataEstatica.AddDays(-5),
                    SetorId = 3, // RH
                    Ativo = true
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}

