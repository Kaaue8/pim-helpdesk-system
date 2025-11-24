using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using HelpDesk.Desktop.Models;
using HelpDesk.Desktop.Services;
using HelpDesk.Desktop.Utils;

namespace HelpDesk.Desktop.Forms
{
    public partial class FormTickets : Form
    {
        private List<Ticket> _todosTickets = new List<Ticket>();

        public FormTickets()
        {
            InitializeComponent();
            ConfigurarEstilo();
            CarregarTickets();
        }

        private void ConfigurarEstilo()
        {
            this.BackColor = AppStyles.ColorBackground;
            AppStyles.StyleDataGridView(dgvTickets);
        }

        private async void CarregarTickets()
        {
            try
            {
                btnAtualizar.Enabled = false;
                btnAtualizar.Text = "Carregando...";

                _todosTickets = await ApiService.Instance.GetTicketsAsync();
                AplicarFiltros();
            }
            catch (Exception ex)
            {
                AppStyles.ShowError($"Erro ao carregar tickets: {ex.Message}");
            }
            finally
            {
                btnAtualizar.Enabled = true;
                btnAtualizar.Text = "🔄 Atualizar";
            }
        }

        private void AplicarFiltros()
        {
            var ticketsFiltrados = _todosTickets.AsEnumerable();

            // Filtro por status
            if (cmbFiltroStatus.SelectedIndex > 0)
            {
                string statusSelecionado = cmbFiltroStatus.SelectedItem?.ToString() ?? "";
                ticketsFiltrados = ticketsFiltrados.Where(t => t.Status == statusSelecionado);
            }

            // Filtro por busca
            string termoBusca = txtBusca.Text.Trim();
            if (!string.IsNullOrEmpty(termoBusca))
            {
                ticketsFiltrados = ticketsFiltrados.Where(t =>
                    t.Titulo.Contains(termoBusca, StringComparison.OrdinalIgnoreCase) ||
                    t.Descricao.Contains(termoBusca, StringComparison.OrdinalIgnoreCase));
            }

            dgvTickets.DataSource = ticketsFiltrados.ToList();
            ConfigurarColunas();
        }

        private void ConfigurarColunas()
        {
            if (dgvTickets.Columns.Count > 0)
            {
                dgvTickets.Columns["ID_Ticket"].HeaderText = "ID";
                dgvTickets.Columns["ID_Ticket"].Width = 60;
                dgvTickets.Columns["Titulo"].HeaderText = "Título";
                dgvTickets.Columns["Status"].HeaderText = "Status";
                dgvTickets.Columns["Status"].Width = 120;
                dgvTickets.Columns["Prioridade"].HeaderText = "Prioridade";
                dgvTickets.Columns["Prioridade"].Width = 100;
                dgvTickets.Columns["DataAbertura"].HeaderText = "Data Abertura";
                dgvTickets.Columns["DataAbertura"].Width = 150;

                // Ocultar colunas desnecessárias
                if (dgvTickets.Columns.Contains("Descricao"))
                    dgvTickets.Columns["Descricao"].Visible = false;
                if (dgvTickets.Columns.Contains("SolicitanteId"))
                    dgvTickets.Columns["SolicitanteId"].Visible = false;
                if (dgvTickets.Columns.Contains("ResponsavelId"))
                    dgvTickets.Columns["ResponsavelId"].Visible = false;
                if (dgvTickets.Columns.Contains("ID_Categoria"))
                    dgvTickets.Columns["ID_Categoria"].Visible = false;
                if (dgvTickets.Columns.Contains("DataFechamento"))
                    dgvTickets.Columns["DataFechamento"].Visible = false;
                if (dgvTickets.Columns.Contains("ResumoTriagem"))
                    dgvTickets.Columns["ResumoTriagem"].Visible = false;
                if (dgvTickets.Columns.Contains("SetorRecomendado"))
                    dgvTickets.Columns["SetorRecomendado"].Visible = false;
                if (dgvTickets.Columns.Contains("SolucaoSugerida"))
                    dgvTickets.Columns["SolucaoSugerida"].Visible = false;
            }
        }

        private void BtnNovoTicket_Click(object sender, EventArgs e)
        {
            var formNovoTicket = new FormNovoTicket();
            formNovoTicket.Show();
            this.Close();
        }

        private void BtnAtualizar_Click(object sender, EventArgs e)
        {
            CarregarTickets();
        }

        private void BtnVoltar_Click(object sender, EventArgs e)
        {
            var formDashboard = new FormDashboard();
            formDashboard.Show();
            this.Close();
        }

        private void CmbFiltroStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        private void TxtBusca_TextChanged(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        private void DgvTickets_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var ticket = (Ticket)dgvTickets.Rows[e.RowIndex].DataBoundItem;
                var formNovoTicket = new FormNovoTicket(ticket);
                formNovoTicket.Show();
                this.Close();
            }
        }

        private void BtnHistorico_Click(object sender, EventArgs e)
        {
            if (dgvTickets.SelectedRows.Count > 0)
            {
                var ticket = (Ticket)dgvTickets.SelectedRows[0].DataBoundItem;
                var formHistorico = new FormHistorico(ticket.ID_Ticket);
                formHistorico.ShowDialog();
            }
            else
            {
                AppStyles.ShowWarning("Selecione um ticket para ver o histórico.");
            }
        }

        private void FormTickets_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Application.Exit();
            }
        }
    }
}