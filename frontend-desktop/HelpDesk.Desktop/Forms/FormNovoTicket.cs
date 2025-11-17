using System;
using System.Windows.Forms;
using HelpDesk.Desktop.Models;
using HelpDesk.Desktop.Services;
using HelpDesk.Desktop.Utils;

namespace HelpDesk.Desktop.Forms
{
    public partial class FormNovoTicket : Form
    {
        private Ticket? _ticketEdicao;
        private bool _modoEdicao;

        public FormNovoTicket(Ticket? ticket = null)
        {
            InitializeComponent();
            _ticketEdicao = ticket;
            _modoEdicao = ticket != null;
            ConfigurarEstilo();
            CarregarCombos();

            if (_modoEdicao)
            {
                PreencherDados();
            }
        }

        private void ConfigurarEstilo()
        {
            this.BackColor = AppStyles.ColorBackground;
            lblTitulo.Text = _modoEdicao ? "✏️ Editar Ticket" : "➕ Novo Ticket";
            btnSalvar.Text = _modoEdicao ? "💾 Salvar Alterações" : "✅ Criar Ticket";
        }

        private async void CarregarCombos()
        {
            try
            {
                // Carregar categorias
                var categorias = await ApiService.Instance.GetCategoriasAsync();
                cmbCategoria.DataSource = categorias;
                cmbCategoria.DisplayMember = "NomeCategoria";
                cmbCategoria.ValueMember = "ID_Categoria";

                // Adicionar opções de status
                cmbStatus.Items.AddRange(new string[] { "Aberto", "Em Andamento", "Fechado" });
                cmbStatus.SelectedIndex = 0;

                // Adicionar opções de prioridade
                cmbPrioridade.Items.AddRange(new string[] { "Baixa", "Média", "Alta", "Urgente" });
                cmbPrioridade.SelectedIndex = 1;
            }
            catch (Exception ex)
            {
                AppStyles.ShowError($"Erro ao carregar dados: {ex.Message}");
            }
        }

        private void PreencherDados()
        {
            if (_ticketEdicao != null)
            {
                txtTitulo.Text = _ticketEdicao.Titulo;
                txtDescricao.Text = _ticketEdicao.Descricao;
                cmbStatus.SelectedItem = _ticketEdicao.Status;
                cmbPrioridade.SelectedItem = _ticketEdicao.Prioridade;

                if (_ticketEdicao.ID_Categoria.HasValue)
                {
                    cmbCategoria.SelectedValue = _ticketEdicao.ID_Categoria.Value;
                }
            }
        }

        private async void BtnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validações
                if (string.IsNullOrWhiteSpace(txtTitulo.Text))
                {
                    AppStyles.ShowWarning("Por favor, informe o título do ticket.");
                    txtTitulo.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtDescricao.Text))
                {
                    AppStyles.ShowWarning("Por favor, informe a descrição do ticket.");
                    txtDescricao.Focus();
                    return;
                }

                btnSalvar.Enabled = false;
                btnSalvar.Text = "Salvando...";

                var ticket = new Ticket
                {
                    Titulo = txtTitulo.Text.Trim(),
                    Descricao = txtDescricao.Text.Trim(),
                    Status = cmbStatus.SelectedItem?.ToString() ?? "Aberto",
                    Prioridade = cmbPrioridade.SelectedItem?.ToString() ?? "Média",
                    ID_Categoria = cmbCategoria.SelectedValue != null ? (int)cmbCategoria.SelectedValue : null,
                    SolicitanteId = AuthService.Instance.CurrentUser!.ID_Usuario
                };

                if (_modoEdicao && _ticketEdicao != null)
                {
                    ticket.ID_Ticket = _ticketEdicao.ID_Ticket;
                    await ApiService.Instance.UpdateTicketAsync(ticket.ID_Ticket, ticket);
                    AppStyles.ShowSuccess("Ticket atualizado com sucesso!");
                }
                else
                {
                    await ApiService.Instance.CreateTicketAsync(ticket);
                    AppStyles.ShowSuccess("Ticket criado com sucesso!");
                }

                VoltarParaTickets();
            }
            catch (Exception ex)
            {
                AppStyles.ShowError($"Erro ao salvar ticket: {ex.Message}");
                btnSalvar.Enabled = true;
                btnSalvar.Text = _modoEdicao ? "💾 Salvar Alterações" : "✅ Criar Ticket";
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            VoltarParaTickets();
        }

        private void VoltarParaTickets()
        {
            var formTickets = new FormTickets();
            formTickets.Show();
            this.Close();
        }

        private void FormNovoTicket_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Application.Exit();
            }
        }
    }
}