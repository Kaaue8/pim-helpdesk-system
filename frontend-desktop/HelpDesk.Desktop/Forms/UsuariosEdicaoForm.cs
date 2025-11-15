Criação / Edição de Usuários)
using System;
using System.Drawing;
using System.Windows.Forms;
using HelpDeskDesktop.Services;

namespace HelpDeskDesktop
{
    public partial class UsuarioEdicaoForm : Form
    {
        private readonly ApiService _apiService;
        private readonly Usuario _usuarioExistente;
        private readonly bool _modoEdicao;

        private TextBox txtNome;
        private TextBox txtEmail;
        private TextBox txtSenha;
        private ComboBox cmbPerfil;
        private ComboBox cmbSetor;
        private Button btnSalvar;
        private Button btnCancelar;
        private Label lblNome;
        private Label lblEmail;
        private Label lblSenha;
        private Label lblPerfil;
        private Label lblSetor;
        private Label lblFormTitulo;

        public UsuarioEdicaoForm(ApiService apiService, Usuario usuarioExistente)
        {
            _apiService = apiService;
            _usuarioExistente = usuarioExistente;
            _modoEdicao = usuarioExistente != null;

            InitializeComponent();
            ConfigurarInterface();
            CarregarDados();
        }

        private void ConfigurarInterface()
        {
            // Configurações do Form
            this.Text = _modoEdicao ? "Editar Usuário" : "Novo Usuário";
            this.Size = new Size(500, 550);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(243, 244, 246);

            // Título do formulário
            lblFormTitulo = new Label
            {
                Text = _modoEdicao ? "Editar Usuário" : "Novo Usuário",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(124, 58, 237),
                AutoSize = true,
                Location = new Point(30, 20)
            };

            // Label Nome
            lblNome = new Label
            {
                Text = "Nome:",
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

            // Label Email
            lblEmail = new Label
            {
                Text = "Email:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(31, 41, 55),
                AutoSize = true,
                Location = new Point(30, 135)
            };

            // TextBox Email
            txtEmail = new TextBox
            {
                Size = new Size(420, 30),
                Location = new Point(30, 160),
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Label Senha
            lblSenha = new Label
            {
                Text = "Senha:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(31, 41, 55),
                AutoSize = true,
                Location = new Point(30, 200)
            };

            // TextBox Senha
            txtSenha = new TextBox
            {
                Size = new Size(420, 30),
                Location = new Point(30, 225),
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle,
                PasswordChar = '●'
            };

            // Label Perfil
            lblPerfil = new Label
            {
                Text = "Perfil:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(31, 41, 55),
                AutoSize = true,
                Location = new Point(30, 265)
            };

            // ComboBox Perfil
            cmbPerfil = new ComboBox
            {
                Size = new Size(200, 30),
                Location = new Point(30, 290),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbPerfil.Items.AddRange(new object[] { "Admin", "Analista", "Usuario" });
            cmbPerfil.SelectedIndex = 2; // Default: Usuario

            // Label Setor
            lblSetor = new Label
            {
                Text = "Setor:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(31, 41, 55),
                AutoSize = true,
                Location = new Point(250, 265)
            };

            // ComboBox Setor
            cmbSetor = new ComboBox
            {
                Size = new Size(200, 30),
                Location = new Point(250, 290),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Botão Salvar
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

            // Botão Cancelar
            btnCancelar = new Button
            {
                Text = "Cancelar",
                Size = new Size(200, 40),
                Location = new Point(250, 350),
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
            this.Controls.Add(lblEmail);
            this.Controls.Add(txtEmail);
            this.Controls.Add(lblSenha);
            this.Controls.Add(txtSenha);
            this.Controls.Add(lblPerfil);
            this.Controls.Add(cmbPerfil);
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
                if (_modoEdicao && _usuarioExistente != null)
                {
                    txtNome.Text = _usuarioExistente.Nome;
                    txtEmail.Text = _usuarioExistente.Email;
                    txtSenha.Text = ""; // Não preencher a senha por segurança
                    cmbPerfil.SelectedItem = _usuarioExistente.Perfil;
                    if (_usuarioExistente.SetorId.HasValue)
                    {
                        cmbSetor.SelectedValue = _usuarioExistente.SetorId.Value;
                    }
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
            if (string.IsNullOrWhiteSpace(txtNome.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                (!_modoEdicao && string.IsNullOrWhiteSpace(txtSenha.Text)))
            {
                MessageBox.Show("Por favor, preencha todos os campos obrigatórios.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnSalvar.Enabled = false;
            btnSalvar.Text = "Salvando...";

            try
            {
                var usuario = new Usuario
                {
                    Nome = txtNome.Text,
                    Email = txtEmail.Text,
                    Perfil = cmbPerfil.SelectedItem.ToString(),
                    SetorId = cmbSetor.SelectedValue != null ? (int?)cmbSetor.SelectedValue : null
                };

                // Só atualiza a senha se foi preenchida
                if (!string.IsNullOrWhiteSpace(txtSenha.Text))
                {
                    usuario.Senha = txtSenha.Text;
                }

                if (_modoEdicao)
                {
                    usuario.Id = _usuarioExistente.Id;
                    var sucesso = await _apiService.UpdateUsuarioAsync(usuario.Id, usuario);
                    if (sucesso)
                    {
                        MessageBox.Show("Usuário atualizado com sucesso!", "Sucesso",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                    }
                }
                else
                {
                    var usuarioCriado = await _apiService.CreateUsuarioAsync(usuario);
                    if (usuarioCriado != null)
                    {
                        MessageBox.Show("Usuário criado com sucesso!", "Sucesso",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
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

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new Size(500, 550);
            this.Name = "UsuarioEdicaoForm";
            this.ResumeLayout(false);
        }
    }
}