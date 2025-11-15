using System;
using System.Drawing;
using System.Windows.Forms;
using HelpDeskDesktop.Services;

namespace HelpDeskDesktop
{
    public partial class TicketEdicaoForm : Form
    {
        private readonly ApiService _apiService;
        private readonly Ticket _ticketExistente;
        private readonly bool _modoEdicao;

        private TextBox txtTitulo;
        private TextBox txtDescricao;
        private ComboBox cmbStatus;
        private ComboBox cmbPrioridade;
        private ComboBox cmbSetor;
        private Button btnSalvar;
        private Button btnCancelar;
        private Label lblTitulo;
        private Label lblDescricao;
        private Label lblStatus;
        private Label lblPrioridade;
        private Label lblSetor;
        private Label lblFormTitulo;

        public TicketEdicaoForm(ApiService apiService, Ticket ticketExistente)
        {
            _apiService = apiService;
            _ticketExistente = ticketExistente;
            _modoEdicao = ticketExistente != null;

            InitializeComponent();
            ConfigurarInterface();
            CarregarDados();
        }

        private void ConfigurarInterface()
        {
            // Configurações do Form
            this.Text = _modoEdicao ? "Editar Ticket" : "Novo Ticket";
            this.Size = new Size(500, 550);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(243, 244, 246);

            // Título do formulário
            lblFormTitulo = new Label
            {
                Text = _modoEdicao ? "Editar Ticket" : "Novo Ticket",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(124, 58, 237),
                AutoSize = true,
                Location = new Point(30, 20)
            };

            // Label Título
            lblTitulo = new Label
            {
                Text = "Título:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(31, 41, 55),
                AutoSize = true,
                Location = new Point(30, 70)
            };

            // TextBox Título
            txtTitulo = new TextBox
            {
                Size = new Size(420, 30),
                Location = new Point(30, 95),
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Label Descrição
            lblDescricao = new Label
            {
                Text = "Descrição:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(31, 41, 55),
                AutoSize = true,
                Location = new Point(30, 135)
            };

            // TextBox Descrição
            txtDescricao = new TextBox
            {
                Size = new Size(420, 80),
                Location = new Point(30, 160),
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };

            // Label Status
            lblStatus = new Label
            {
                Text = "Status:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(31, 41, 55),
                AutoSize = true,
                Location = new Point(30, 255)
            };

            // ComboBox Status
            cmbStatus = new ComboBox
            {
                Size = new Size(200, 30),
                Location = new Point(30, 280),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbStatus.Items.AddRange(new object[] { "Aberto", "Em Andamento", "Fechado" });
            cmbStatus.SelectedIndex = 0;

            // Label Prioridade
            lblPrioridade = new Label
            {
                Text = "Prioridade:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(31, 41, 55),
                AutoSize = true,
                Location = new Point(250, 255)
            };

            // ComboBox Prioridade
            cmbPrioridade = new ComboBox
            {
                Size = new Size(200, 30),
                Location = new Point(250, 280),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbPrioridade.Items.AddRange(new object[] { "Baixa", "Media", "Alta" });
            cmbPrioridade.SelectedIndex = 1;

            // Label Setor
            lblSetor = new Label
            {
                Text = "Setor:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(31, 41, 55),
                AutoSize = true,
                Location = new Point(30, 325)
            };

            // ComboBox Setor
            cmbSetor = new ComboBox
            {
                Size = new Size(420, 30),
                Location = new Point(30, 350),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Botão Salvar
            btnSalvar = new Button
            {
                Text = "Salvar",
                Size = new Size(200, 40),
                Location = new Point(30, 410),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(124, 58, 237),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSalvar.FlatAppearance.BorderSize = 0;
            btnSalvar.Click += BtnSalvar_Click;

            // Botão Cancelar
            btnCancelar = new Button
            {
                Text = "Cancelar",
                Size = new Size(200, 40),
                Location = new Point(250, 410),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(107, 114, 128),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            // Adicionar controles ao form
            this.Controls.Add(lblFormTitulo);
            this.Controls.Add(lblTitulo);
            this.Controls.Add(txtTitulo);
            this.Controls.Add(lblDescricao);
            this.Controls.Add(txtDescricao);
            this.Controls.Add(lblStatus);
            this.Controls.Add(cmbStatus);
            this.Controls.Add(lblPrioridade);
            this.Controls.Add(cmbPrioridade);
            this.Controls.Add(lblSetor);
            this.Controls.Add(cmbSetor);
            this.Controls.Add(btnSalvar);
            this.Controls.Add(btnCancelar);
        }

        private async void CarregarDados()
        {
            try
            {
                // Carregar setores
                var setores = await _apiService.GetSetoresAsync();
                cmbSetor.DataSource = setores;
                cmbSetor.DisplayMember = "Nome";
                cmbSetor.ValueMember = "Id";

                // Se estiver em modo de edição, preencher os campos
                if (_modoEdicao && _ticketExistente != null)
                {
                    txtTitulo.Text = _ticketExistente.Titulo;
                    txtDescricao.Text = _ticketExistente.Descricao;
                    cmbStatus.SelectedItem = _ticketExistente.Status;
                    cmbPrioridade.SelectedItem = _ticketExistente.Prioridade;
                    cmbSetor.SelectedValue = _ticketExistente.SetorId;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnSalvar_Click(object sender, EventArgs e)
        {
            // Validação
            if (string.IsNullOrWhiteSpace(txtTitulo.Text) ||
                string.IsNullOrWhiteSpace(txtDescricao.Text) ||
                cmbSetor.SelectedValue == null)
            {
                MessageBox.Show("Por favor, preencha todos os campos obrigatórios.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnSalvar.Enabled = false;
            btnSalvar.Text = "Salvando...";

            try
            {
                var ticket = new Ticket
                {
                    Titulo = txtTitulo.Text,
                    Descricao = txtDescricao.Text,
                    Status = cmbStatus.SelectedItem.ToString(),
                    Prioridade = cmbPrioridade.SelectedItem.ToString(),
                    SetorId = (int)cmbSetor.SelectedValue,
                    DataAbertura = _modoEdicao ? _ticketExistente.DataAbertura : DateTime.Now,
                    UsuarioId = _modoEdicao ? _ticketExistente.UsuarioId : 1 // Ajustar conforme necessário
                };

                if (_modoEdicao)
                {
                    ticket.Id = _ticketExistente.Id;
                    var sucesso = await _apiService.UpdateTicketAsync(ticket.Id, ticket);
                    if (sucesso)
                    {
                        MessageBox.Show("Ticket atualizado com sucesso!", "Sucesso",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                    }
                }
                else
                {
                    var ticketCriado = await _apiService.CreateTicketAsync(ticket);
                    if (ticketCriado != null)
                    {
                        MessageBox.Show("Ticket criado com sucesso!", "Sucesso",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar ticket: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSalvar.Enabled = true;
                btnSalvar.Text = "Salvar";
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new Size(500, 550);
            this.Name = "TicketEdicaoForm";
            this.ResumeLayout(false);
        }
    }
}