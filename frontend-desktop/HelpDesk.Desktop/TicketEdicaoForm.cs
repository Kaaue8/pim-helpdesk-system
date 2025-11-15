using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using HelpDesk.Desktop.Models;
using HelpDesk.Desktop.Services;

namespace HelpDesk.Desktop
{
    public partial class TicketEdicaoForm : Form
    {
        private readonly ApiService _apiService;
        private readonly Ticket _ticket;
        private readonly Usuario _usuarioLogado;
        private TextBox txtTitulo;
        private TextBox txtDescricao;
        private ComboBox cmbStatus;
        private ComboBox cmbPrioridade;
        private ComboBox cmbSetor;
        private Button btnSalvar;
        private Button btnCancelar;
        private Label lblTitulo;
        private List<Setor> _setores;

        public TicketEdicaoForm(ApiService apiService, Ticket ticket, Usuario usuario)
        {
            _apiService = apiService;
            _ticket = ticket;
            _usuarioLogado = usuario;
            _setores = new List<Setor>();

            InitializeComponent();
            ConfigurarInterface();
            CarregarSetores();

            if (_ticket != null)
            {
                PreencherDados();
            }
        }

        private void ConfigurarInterface()
        {
            this.ClientSize = new Size(500, 600);
            this.Text = _ticket == null ? "Novo Ticket" : "Editar Ticket";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(249, 250, 251);

            lblTitulo = new Label
            {
                Text = _ticket == null ? "Novo Ticket" : "Editar Ticket",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(55, 65, 81),
                AutoSize = true,
                Location = new Point(30, 30)
            };

            var lblTituloField = new Label
            {
                Text = "Título:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(55, 65, 81),
                AutoSize = true,
                Location = new Point(30, 90)
            };

            txtTitulo = new TextBox
            {
                Size = new Size(440, 30),
                Location = new Point(30, 115),
                Font = new Font("Segoe UI", 10)
            };

            var lblDescricaoField = new Label
            {
                Text = "Descrição:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(55, 65, 81),
                AutoSize = true,
                Location = new Point(30, 160)
            };

            txtDescricao = new TextBox
            {
                Size = new Size(440, 100),
                Location = new Point(30, 185),
                Font = new Font("Segoe UI", 10),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };

            var lblStatusField = new Label
            {
                Text = "Status:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(55, 65, 81),
                AutoSize = true,
                Location = new Point(30, 300)
            };

            cmbStatus = new ComboBox
            {
                Size = new Size(200, 30),
                Location = new Point(30, 325),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbStatus.Items.AddRange(new object[] { "Aberto", "Em Andamento", "Resolvido", "Fechado" });
            cmbStatus.SelectedIndex = 0;

            var lblPrioridadeField = new Label
            {
                Text = "Prioridade:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(55, 65, 81),
                AutoSize = true,
                Location = new Point(270, 300)
            };

            cmbPrioridade = new ComboBox
            {
                Size = new Size(200, 30),
                Location = new Point(270, 325),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbPrioridade.Items.AddRange(new object[] { "Baixa", "Média", "Alta", "Urgente" });
            cmbPrioridade.SelectedIndex = 0;

            var lblSetorField = new Label
            {
                Text = "Setor:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(55, 65, 81),
                AutoSize = true,
                Location = new Point(30, 370)
            };

            cmbSetor = new ComboBox
            {
                Size = new Size(440, 30),
                Location = new Point(30, 395),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            btnSalvar = new Button
            {
                Text = "Salvar",
                Size = new Size(200, 40),
                Location = new Point(30, 480),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(124, 58, 237),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSalvar.FlatAppearance.BorderSize = 0;
            btnSalvar.Click += BtnSalvar_Click;

            btnCancelar = new Button
            {
                Text = "Cancelar",
                Size = new Size(200, 40),
                Location = new Point(270, 480),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(156, 163, 175),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            this.Controls.Add(lblTitulo);
            this.Controls.Add(lblTituloField);
            this.Controls.Add(txtTitulo);
            this.Controls.Add(lblDescricaoField);
            this.Controls.Add(txtDescricao);
            this.Controls.Add(lblStatusField);
            this.Controls.Add(cmbStatus);
            this.Controls.Add(lblPrioridadeField);
            this.Controls.Add(cmbPrioridade);
            this.Controls.Add(lblSetorField);
            this.Controls.Add(cmbSetor);
            this.Controls.Add(btnSalvar);
            this.Controls.Add(btnCancelar);
        }

        private async void CarregarSetores()
        {
            try
            {
                _setores = await _apiService.GetSetoresAsync();

                if (_setores != null)
                {
                    cmbSetor.DisplayMember = "Nome";
                    cmbSetor.ValueMember = "Id";
                    cmbSetor.DataSource = _setores;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar setores: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PreencherDados()
        {
            if (_ticket != null)
            {
                txtTitulo.Text = _ticket.Titulo;
                txtDescricao.Text = _ticket.Descricao;

                cmbStatus.SelectedItem = _ticket.Status;
                cmbPrioridade.SelectedItem = _ticket.Prioridade;

                if (_setores != null)
                {
                    var setor = _setores.Find(s => s.Id == _ticket.SetorId);
                    if (setor != null)
                    {
                        cmbSetor.SelectedItem = setor;
                    }
                }
            }
        }

        private async void BtnSalvar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitulo.Text))
            {
                MessageBox.Show("Por favor, preencha o título.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbSetor.SelectedItem == null)
            {
                MessageBox.Show("Por favor, selecione um setor.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnSalvar.Enabled = false;
            btnSalvar.Text = "Salvando...";

            try
            {
                var ticket = new Ticket
                {
                    Id = _ticket?.Id ?? 0,
                    Titulo = txtTitulo.Text,
                    Descricao = txtDescricao.Text,
                    Status = cmbStatus.SelectedItem?.ToString() ?? "Aberto",
                    Prioridade = cmbPrioridade.SelectedItem?.ToString() ?? "Média",
                    SetorId = ((Setor)cmbSetor.SelectedItem).Id,
                    UsuarioId = _usuarioLogado?.Id ?? 0,
                    Categoria = "Geral"
                };

                if (_ticket == null)
                {
                    var resultado = await _apiService.CreateTicketAsync(ticket);

                    if (resultado != null)
                    {
                        MessageBox.Show("Ticket criado com sucesso!", "Sucesso",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show("Erro ao criar ticket.", "Erro",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    var sucesso = await _apiService.UpdateTicketAsync(_ticket.Id, ticket);

                    if (sucesso)
                    {
                        MessageBox.Show("Ticket atualizado com sucesso!", "Sucesso",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show("Erro ao atualizar ticket.", "Erro",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}