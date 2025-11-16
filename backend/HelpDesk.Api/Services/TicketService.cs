using Microsoft.EntityFrameworkCore;
using HelpDesk.Api.Data;
using HelpDesk.Api.Models;

namespace HelpDesk.Api.Services
{
    public class TicketService
    {
        private readonly ApplicationDbContext _context;

        public TicketService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Ticket>> GetAllTicketsAsync()
        {
            return await _context.Tickets
                .Include(t => t.Solicitante)
                .Include(t => t.Responsavel)
                .Include(t => t.Categoria)
                .OrderByDescending(t => t.DataAbertura)
                .ToListAsync();
        }

        public async Task<List<Ticket>> GetTicketsByUsuarioAsync(int usuarioId, string perfil)
        {
            if (perfil == "Admin")
            {
                return await GetAllTicketsAsync();
            }
            else if (perfil == "Analista")
            {
                return await _context.Tickets
                    .Include(t => t.Solicitante)
                    .Include(t => t.Responsavel)
                    .Include(t => t.Categoria)
                    .Where(t => t.ResponsavelId == usuarioId || t.ResponsavelId == null)
                    .OrderByDescending(t => t.DataAbertura)
                    .ToListAsync();
            }
            else
            {
                return await _context.Tickets
                    .Include(t => t.Solicitante)
                    .Include(t => t.Responsavel)
                    .Include(t => t.Categoria)
                    .Where(t => t.SolicitanteId == usuarioId)
                    .OrderByDescending(t => t.DataAbertura)
                    .ToListAsync();
            }
        }

        public async Task<Ticket?> GetTicketByIdAsync(int id)
        {
            return await _context.Tickets
                .Include(t => t.Solicitante)
                .Include(t => t.Responsavel)
                .Include(t => t.Categoria)
                .FirstOrDefaultAsync(t => t.IdTicket == id);
        }

        public async Task<Ticket> CreateTicketAsync(Ticket ticket)
        {
            ticket.DataAbertura = DateTime.Now;
            ticket.DataAtualizacao = DateTime.Now;
            ticket.Status = "Aberto";

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return await GetTicketByIdAsync(ticket.IdTicket) ?? ticket;
        }

        public async Task<Ticket?> UpdateTicketAsync(int id, Ticket ticketAtualizado)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return null;
            }

            ticket.Titulo = ticketAtualizado.Titulo;
            ticket.Descricao = ticketAtualizado.Descricao;
            ticket.Prioridade = ticketAtualizado.Prioridade;
            ticket.Status = ticketAtualizado.Status;
            ticket.CategoriaId = ticketAtualizado.CategoriaId;
            ticket.ResponsavelId = ticketAtualizado.ResponsavelId;
            ticket.Solucao = ticketAtualizado.Solucao;
            ticket.DataAtualizacao = DateTime.Now;

            if (ticketAtualizado.Status == "Fechado" || ticketAtualizado.Status == "Resolvido")
            {
                ticket.DataFechamento = DateTime.Now;
            }

            await _context.SaveChangesAsync();

            return await GetTicketByIdAsync(id);
        }

        public async Task<bool> DeleteTicketAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return false;
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}

