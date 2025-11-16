using Microsoft.EntityFrameworkCore;
using HelpDesk.Api.Models;

namespace HelpDesk.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Setor> Setores { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketHistorico> TicketHistoricos { get; set; } // ✅ Histórico de tickets
        public DbSet<LogAuditoria> LogsAuditoria { get; set; } // ✅ Auditoria geral

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===== RELACIONAMENTOS DE TICKET =====
            
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Solicitante)
                .WithMany(u => u.TicketsAbertos)
                .HasForeignKey(t => t.SolicitanteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Responsavel)
                .WithMany(u => u.TicketsAtribuidos)
                .HasForeignKey(t => t.ResponsavelId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Categoria)
                .WithMany(c => c.Tickets)
                .HasForeignKey(t => t.CategoriaId)
                .OnDelete(DeleteBehavior.SetNull);

            // ===== RELACIONAMENTOS DE USUARIO =====
            
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Setor)
                .WithMany(s => s.Usuarios)
                .HasForeignKey(u => u.SetorId)
                .OnDelete(DeleteBehavior.SetNull);

            // ===== RELACIONAMENTOS DE HISTORICO =====
            
            modelBuilder.Entity<TicketHistorico>()
                .HasOne(h => h.Ticket)
                .WithMany(t => t.Historico)
                .HasForeignKey(h => h.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TicketHistorico>()
                .HasOne(h => h.Usuario)
                .WithMany()
                .HasForeignKey(h => h.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // ===== RELACIONAMENTOS DE AUDITORIA =====
            
            modelBuilder.Entity<LogAuditoria>()
                .HasOne(l => l.UsuarioAfetado)
                .WithMany(u => u.LogsAfetados)
                .HasForeignKey(l => l.IdUsuarioAfetado)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LogAuditoria>()
                .HasOne(l => l.UsuarioExecutor)
                .WithMany(u => u.LogsExecutados)
                .HasForeignKey(l => l.IdUsuarioExecutor)
                .OnDelete(DeleteBehavior.Restrict);

            // ===== ÍNDICES PARA PERFORMANCE =====
            
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Ticket>()
                .HasIndex(t => t.Status);

            modelBuilder.Entity<Ticket>()
                .HasIndex(t => t.Prioridade);

            modelBuilder.Entity<TicketHistorico>()
                .HasIndex(h => h.TicketId);

            modelBuilder.Entity<TicketHistorico>()
                .HasIndex(h => h.DataHora);
        }
    }
}

