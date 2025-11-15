using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using HelpDesk.Desktop.Models;
using HelpDesk.Desktop.Services;

namespace HelpDesk.Desktop
{
    public partial class UsuarioEdicaoForm : Form
    {
        private readonly ApiService _apiService;
        private readonly Usuario _usuario;
        private TextBox txtNome;
        private TextBox txtEmail;
        private TextBox txtSenha;
        private ComboBox cmbPerfil;
        private ComboBox cmbSetor;
        private Button btnSalvar;
        private Button btnCancelar;
        private Label lblTitulo;
        private List<Setor> _setores;

        public UsuarioEdicaoForm(ApiService apiService, Usuario usuario)
        {
            _apiService = apiService;
            _usuario = usuario;
            _setores = new List<Setor>();

            InitializeComponent();
            ConfigurarInterface();
            CarregarSetores();

            if (_usuario != null)
            {
                PreencherDados();
            }
        }

        private void ConfigurarInterface()
        {
            this.ClientSize = new Size(500, 600);
            this.Text = _usuario == null ? "Novo Usuário" : "Editar Usuário";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(249, 250, 251);

            lblTitulo = new Label
            {
                Text = _usuario == null ? "Novo Usuário" : "Editar Usuário",
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

            var lblEmailField = new Label
            {
                Text = "E-mail:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(55, 65, 81),
                AutoSize = true,
                Location = new Point(30, 160)
            };

            txtEmail = new TextBox
            {
                Size = new Size(440, 30),
                Location = new Point(30, 185),
                Font = new Font("Segoe UI", 10)
            };

            var lblSenhaField = new Label
            {
                Text = _usuario == null ? "Senha:" : "Senha (deixe em branco para não alterar):",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(55, 65, 81),
                AutoSize = true,
                Location = new Point(30, 230)
            };

            txtSenha = new TextBox
            {
                Size = new Size(440, 30),
                Location = new Point(30, 255),
                Font = new Font("Segoe UI", 10),
                PasswordChar = '●'
            };

            var lblPerfilField = new Label
            {
                Text = "Perfil:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(55, 65, 81),
                AutoSize = true,
                Location = new Point(30, 300)
            };

            cmbPerfil = new ComboBox
            {
                Size = new Size(200, 30),
                Location = new Point(30, 325),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbPerfil.Items.AddRange(new object[] { "Admin", "Analista", "Usuario" });
            cmbPerfil.SelectedIndex = 2;

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
            this.Controls.Add(lblNomeField);
            this.Controls.Add(txtNome);
            this.Controls.Add(lblEmailField);
            this.Controls.Add(txtEmail);
            this.Controls.Add(lblSenhaField);
            this.Controls.Add(txtSenha);
            this.Controls.Add(lblPerfilField);
            this.Controls.Add(cmbPerfil);
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
            if (_usuario != null)
            {
                txtNome.Text = _usuario.Nome;
                txtEmail.Text = _usuario.Email;
                cmbPerfil.SelectedItem = _usuario.Perfil;

                if (_setores != null && _usuario.SetorId.HasValue)
                {
                    var setor = _setores.Find(s => s.Id == _usuario.SetorId.Value);
                    if (setor != null)
                    {
                        cmbSetor.SelectedItem = setor;
                    }
                }
            }
        }

        private async void BtnSalvar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Por favor, preencha o nome.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Por favor, preencha o e-mail.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_usuario == null && string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                MessageBox.Show("Por favor, preencha a senha.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnSalvar.Enabled = false;
            btnSalvar.Text = "Salvando...";

            try
            {
                var usuario = new Usuario
                {
                    Id = _usuario?.Id ?? 0,
                    Nome = txtNome.Text,
                    Email = txtEmail.Text,
                    Perfil = cmbPerfil.SelectedItem?.ToString() ?? "Usuario",
                    SetorId = cmbSetor.SelectedItem != null ? ((Setor)cmbSetor.SelectedItem).Id : (int?)null
                };

                if (!string.IsNullOrWhiteSpace(txtSenha.Text))
                {
                    usuario.Senha = txtSenha.Text;
                }
                else if (_usuario != null)
                {
                    usuario.Senha = _usuario.Senha;
                }

                if (_usuario == null)
                {
                    var resultado = await _apiService.CreateUsuarioAsync(usuario);

                    if (resultado != null)
                    {
                        MessageBox.Show("Usuário criado com sucesso!", "Sucesso",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show("Erro ao criar usuário.", "Erro",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    var sucesso = await _apiService.UpdateUsuarioAsync(_usuario.Id, usuario);

                    if (sucesso)
                    {
                        MessageBox.Show("Usuário atualizado com sucesso!", "Sucesso",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show("Erro ao atualizar usuário.", "Erro",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar usuário: {ex.Message}", "Erro",
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