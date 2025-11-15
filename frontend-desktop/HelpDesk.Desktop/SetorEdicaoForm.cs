using System;
using System.Drawing;
using System.Windows.Forms;
using HelpDesk.Desktop.Models;
using HelpDesk.Desktop.Services;

namespace HelpDesk.Desktop
{
    public partial class SetorEdicaoForm : Form
    {
        private readonly ApiService _apiService;
        private readonly Setor _setor;
        private TextBox txtNome;
        private TextBox txtDescricao;
        private Button btnSalvar;
        private Button btnCancelar;
        private Label lblTitulo;

        public SetorEdicaoForm(ApiService apiService, Setor setor)
        {
            _apiService = apiService;
            _setor = setor;

            InitializeComponent();
            ConfigurarInterface();

            if (_setor != null)
            {
                PreencherDados();
            }
        }

        private void ConfigurarInterface()
        {
            this.ClientSize = new Size(500, 450);
            this.Text = _setor == null ? "Novo Setor" : "Editar Setor";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(249, 250, 251);

            lblTitulo = new Label
            {
                Text = _setor == null ? "Novo Setor" : "Editar Setor",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(55, 65, 81),
                AutoSize = true,
                Location = new Point(30, 30)
            };

            var lblNomeField = new Label
            {
                Text = "Nome:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(55, 65, 81),
                AutoSize = true,
                Location = new Point(30, 90)
            };

            txtNome = new TextBox
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
                Size = new Size(440, 120),
                Location = new Point(30, 185),
                Font = new Font("Segoe UI", 10),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };

            btnSalvar = new Button
            {
                Text = "Salvar",
                Size = new Size(200, 40),
                Location = new Point(30, 350),
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
                Location = new Point(270, 350),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(156, 163, 175),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            this.Controls.Add(lblTitulo);
            this.Controls.Add(lblNomeField);
            this.Controls.Add(txtNome);
            this.Controls.Add(lblDescricaoField);
            this.Controls.Add(txtDescricao);
            this.Controls.Add(btnSalvar);
            this.Controls.Add(btnCancelar);
        }

        private void PreencherDados()
        {
            if (_setor != null)
            {
                txtNome.Text = _setor.Nome;
                txtDescricao.Text = _setor.Descricao;
            }
        }

        private async void BtnSalvar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Por favor, preencha o nome do setor.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnSalvar.Enabled = false;
            btnSalvar.Text = "Salvando...";

            try
            {
                var setor = new Setor
                {
                    Id = _setor?.Id ?? 0,
                    Nome = txtNome.Text,
                    Descricao = txtDescricao.Text
                };

                if (_setor == null)
                {
                    var resultado = await _apiService.CreateSetorAsync(setor);

                    if (resultado != null)
                    {
                        MessageBox.Show("Setor criado com sucesso!", "Sucesso",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show("Erro ao criar setor.", "Erro",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    var sucesso = await _apiService.UpdateSetorAsync(_setor.Id, setor);

                    if (sucesso)
                    {
                        MessageBox.Show("Setor atualizado com sucesso!", "Sucesso",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show("Erro ao atualizar setor.", "Erro",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar setor: {ex.Message}", "Erro",
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