using HelpDesk.Desktop.Models;
using HelpDesk.Desktop.Services;
using HelpDeskDesktop.Services;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace HelpDeskDesktop
{
    public partial class SetorEdicaoForm : Form
    {
        private readonly ApiService _apiService;
        private readonly Setor _setorExistente;
        private readonly bool _modoEdicao;

        private TextBox txtNome;
        private Button btnSalvar;
        private Button btnCancelar;
        private Label lblNome;
        private Label lblFormTitulo;

        public SetorEdicaoForm(ApiService apiService, Setor setorExistente)
        {
            _apiService = apiService;
            _setorExistente = setorExistente;
            _modoEdicao = setorExistente != null;

            InitializeComponent();
            ConfigurarInterface();
            CarregarDados();
        }

        private void ConfigurarInterface()
        {
            // Configurações do Form
            this.Text = _modoEdicao ? "Editar Setor" : "Novo Setor";
            this.Size = new Size(500, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(243, 244, 246);

            // Título do formulário
            lblFormTitulo = new Label
            {
                Text = _modoEdicao ? "Editar Setor" : "Novo Setor",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(124, 58, 237),
                AutoSize = true,
                Location = new Point(30, 20)
            };

            // Label Nome
            lblNome = new Label
            {
                Text = "Nome do Setor:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(31, 41, 55),
                AutoSize = true,
                Location = new Point(30, 70)
            };

            // TextBox Nome
            txtNome = new TextBox
            {
                Size = new Size(420, 30),
                Location = new Point(30, 95),
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Botão Salvar
            btnSalvar = new Button
            {
                Text = "Salvar",
                Size = new Size(200, 40),
                Location = new Point(30, 150),
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
                Location = new Point(250, 150),
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
            this.Controls.Add(lblNome);
            this.Controls.Add(txtNome);
            this.Controls.Add(btnSalvar);
            this.Controls.Add(btnCancelar);
        }

        private void CarregarDados()
        {
            // Se estiver em modo de edição, preencher o campo
            if (_modoEdicao && _setorExistente != null)
            {
                txtNome.Text = _setorExistente.Nome;
            }
        }

        private async void BtnSalvar_Click(object sender, EventArgs e)
        {
            // Validação
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Por favor, preencha o nome do setor.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnSalvar.Enabled = false;
            btnSalvar.Text = "Salvando...";

            try
            {
                var setor = new Setor
                {
                    Nome = txtNome.Text
                };

                if (_modoEdicao)
                {
                    setor.Id = _setorExistente.Id;
                    var sucesso = await _apiService.UpdateSetorAsync(setor.Id, setor);
                    if (sucesso)
                    {
                        MessageBox.Show("Setor atualizado com sucesso!", "Sucesso",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                    }
                }
                else
                {
                    var setorCriado = await _apiService.CreateSetorAsync(setor);
                    if (setorCriado != null)
                    {
                        MessageBox.Show("Setor criado com sucesso!", "Sucesso",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
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

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new Size(500, 300);
            this.Name = "SetorEdicaoForm";
            this.ResumeLayout(false);
        }
    }
}