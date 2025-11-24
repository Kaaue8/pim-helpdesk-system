using Microsoft.EntityFrameworkCore;
using HelpDesk.Api.Models;

namespace HelpDesk.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // ========================
        //       DBSETS
        // ========================
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Setor> Setores { get; set; }
        public DbSet<LogAuditoria> LogsAuditoria { get; set; }
        public DbSet<CategoriasChamados> CategoriasChamados { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ============================================================
            // LogAuditoria <-> Usuario (Afetado / Executor)
            // ============================================================
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

            // ============================================================
            // Ticket <-> Usuario (Solicitante)
            // ============================================================
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Usuario)
                .WithMany(u => u.TicketsAbertos)
                .HasForeignKey(t => t.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // ============================================================
            // Ticket <-> Usuario (Técnico)
            // ============================================================
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Tecnico)
                .WithMany(u => u.TicketsAtribuidos)
                .HasForeignKey(t => t.TecnicoId)
                .OnDelete(DeleteBehavior.Restrict);

            // ============================================================
            // Setor <-> Usuario
            // ============================================================
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Setor)
                .WithMany(s => s.Usuarios)
                .HasForeignKey(u => u.SetorIdSetor)
                .IsRequired(false)                      // Navegação pode ser null
                .OnDelete(DeleteBehavior.Restrict);     // Evita deleções em cascata

            base.OnModelCreating(modelBuilder);
        }
    }
}
