using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HelpDesk.Desktop.Models;
using HelpDesk.Desktop.Services;
using HelpDesk.Desktop.Utils;

namespace HelpDesk.Desktop.Forms
{
    public partial class FormHistorico : Form
    {
        private int _ticketId;

        public FormHistorico(int ticketId)
        {
            InitializeComponent();
            _ticketId = ticketId;
            ConfigurarEstilo();
            CarregarHistorico();
        }

        private void ConfigurarEstilo()
        {
            this.BackColor = AppStyles.ColorBackground;
        }

        private async void CarregarHistorico()
        {
            try
            {
                lblCarregando.Visible = true;
                panelTimeline.Controls.Clear();

                var historicos = await ApiService.Instance.GetHistoricoTicketAsync(_ticketId);

                if (historicos == null || historicos.Count == 0)
                {
                    MostrarMensagemVazia();
                    return;
                }

                // Ordenar por data (mais recente primeiro)
                historicos = historicos.OrderByDescending(h => h.DataHora).ToList();

                int yPosition = 20;
                foreach (var historico in historicos)
                {
                    var card = CriarCardHistorico(historico);
                    card.Location = new Point(20, yPosition);
                    panelTimeline.Controls.Add(card);
                    yPosition += card.Height + 15;
                }

                lblCarregando.Visible = false;
            }
            catch (Exception ex)
            {
                lblCarregando.Visible = false;
                AppStyles.ShowError($"Erro ao carregar histórico: {ex.Message}");
            }
        }

        private Panel CriarCardHistorico(TicketHistorico historico)
        {
            var card = new Panel
            {
                Width = panelTimeline.Width - 60,
                Height = 100,
                BackColor = Color.White,
                Padding = new Padding(15)
            };

            // Indicador colorido (barra lateral)
            var indicador = new Panel
            {
                Width = 5,
                Height = card.Height,
                Location = new Point(0, 0),
                BackColor = ObterCorPorAcao(historico.Acao)
            };
            card.Controls.Add(indicador);

            // Ação
            var lblAcao = new Label
            {
                Text = $"🔔 {historico.Acao}",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = AppStyles.ColorTextPrimary,
                Location = new Point(20, 10),
                AutoSize = true
            };
            card.Controls.Add(lblAcao);

            // Data e Hora
            var lblDataHora = new Label
            {
                Text = historico.DataHora.ToString("dd/MM/yyyy HH:mm:ss"),
                Font = new Font("Segoe UI", 8F),
                ForeColor = AppStyles.ColorTextSecondary,
                Location = new Point(20, 35),
                AutoSize = true
            };
            card.Controls.Add(lblDataHora);

            // Status Anterior → Novo
            if (!string.IsNullOrEmpty(historico.StatusAnterior) && !string.IsNullOrEmpty(historico.StatusNovo))
            {
                var lblStatus = new Label
                {
                    Text = $"📊 {historico.StatusAnterior} → {historico.StatusNovo}",
                    Font = new Font("Segoe UI", 9F),
                    ForeColor = AppStyles.ColorPrimary,
                    Location = new Point(20, 60),
                    AutoSize = true
                };
                card.Controls.Add(lblStatus);
            }

            // IP Origem
            if (!string.IsNullOrEmpty(historico.IpOrigem))
            {
                var lblIp = new Label
                {
                    Text = $"🌐 {historico.IpOrigem}",
                    Font = new Font("Segoe UI", 8F),
                    ForeColor = AppStyles.ColorTextSecondary,
                    Location = new Point(card.Width - 150, 10),
                    AutoSize = true
                };
                card.Controls.Add(lblIp);
            }

            return card;
        }

        private Color ObterCorPorAcao(string acao)
        {
            if (acao.Contains("Criado") || acao.Contains("Aberto"))
                return AppStyles.ColorSuccess;
            else if (acao.Contains("Atualizado") || acao.Contains("Andamento"))
                return AppStyles.ColorWarning;
            else if (acao.Contains("Fechado") || acao.Contains("Concluído"))
                return AppStyles.ColorPrimary;
            else
                return AppStyles.ColorTextSecondary;
        }

        private void MostrarMensagemVazia()
        {
            var lblVazio = new Label
            {
                Text = "📭 Nenhum histórico encontrado para este ticket.",
                Font = new Font("Segoe UI", 12F),
                ForeColor = AppStyles.ColorTextSecondary,
                AutoSize = true,
                Location = new Point(50, 50)
            };
            panelTimeline.Controls.Add(lblVazio);
            lblCarregando.Visible = false;
        }

        private void BtnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

