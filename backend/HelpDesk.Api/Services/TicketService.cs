using Microsoft.EntityFrameworkCore;
using HelpDesk.Api.Data;
using HelpDesk.Api.Models;

namespace HelpDesk.Api.Services
{
    public class TicketService
    {
        private readonly AppDbContext _context;

        public TicketService(AppDbContext context)
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
            else if (perfil == "Admin")
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

        public async Task<Ticket> CreateTicketAsync(Ticket ticket, int usuarioId = 0, string ipOrigem = "")
        {
            ticket.DataAbertura = DateTime.Now;
            ticket.DataAtualizacao = DateTime.Now;
            ticket.Status = "Aberto";

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            // ✅ Registrar histórico
            if (usuarioId > 0)
            {
                await RegistrarHistorico(ticket.IdTicket, usuarioId, "Criado",
                    $"Ticket criado: {ticket.Titulo}", null, "Aberto", null, ticket.Prioridade, null, null, ipOrigem);
            }

            return await GetTicketByIdAsync(ticket.IdTicket) ?? ticket;
        }

        public async Task<Ticket?> UpdateTicketAsync(int id, Ticket ticketAtualizado, int usuarioId = 0, string ipOrigem = "")
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return null;
            }

            var statusAnterior = ticket.Status;
            var prioridadeAnterior = ticket.Prioridade;
            var responsavelAnteriorId = ticket.ResponsavelId;

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

            // ✅ Registrar histórico de mudanças
            if (usuarioId > 0)
            {
                if (statusAnterior != ticketAtualizado.Status)
                {
                    await RegistrarHistorico(id, usuarioId, "Status Alterado",
                        $"Status alterado de '{statusAnterior}' para '{ticketAtualizado.Status}'",
                        statusAnterior, ticketAtualizado.Status, null, null, null, null, ipOrigem);
                }

                if (prioridadeAnterior != ticketAtualizado.Prioridade)
                {
                    await RegistrarHistorico(id, usuarioId, "Prioridade Alterada",
                        $"Prioridade alterada de '{prioridadeAnterior}' para '{ticketAtualizado.Prioridade}'",
                        null, null, prioridadeAnterior, ticketAtualizado.Prioridade, null, null, ipOrigem);
                }

                if (responsavelAnteriorId != ticketAtualizado.ResponsavelId)
                {
                    await RegistrarHistorico(id, usuarioId, "Atribuído",
                        "Ticket foi reatribuído",
                        null, null, null, null, responsavelAnteriorId, ticketAtualizado.ResponsavelId, ipOrigem);
                }
            }

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

        // ✅ Método para registrar histórico
        private async Task RegistrarHistorico(int ticketId, int usuarioId, string acao, string descricao,
            string? statusAnterior, string? statusNovo, string? prioridadeAnterior, string? prioridadeNova,
            int? responsavelAnteriorId, int? responsavelNovoId, string? ipOrigem)
        {
            var historico = new TicketHistorico
            {
                TicketId = ticketId,
                UsuarioId = usuarioId,
                Acao = acao,
                Descricao = descricao,
                StatusAnterior = statusAnterior,
                StatusNovo = statusNovo,
                PrioridadeAnterior = prioridadeAnterior,
                PrioridadeNova = prioridadeNova,
                ResponsavelAnteriorId = responsavelAnteriorId,
                ResponsavelNovoId = responsavelNovoId,
                DataHora = DateTime.Now,
                IpOrigem = ipOrigem
            };

            _context.TicketHistoricos.Add(historico);
            await _context.SaveChangesAsync();
        }

        // ✅ Obter histórico de um ticket
        public async Task<List<TicketHistorico>> ObterHistoricoAsync(int ticketId)
        {
            return await _context.TicketHistoricos
                .Where(h => h.TicketId == ticketId)
                .Include(h => h.Usuario)
                .OrderByDescending(h => h.DataHora)
                .ToListAsync();
        }
    }
}

