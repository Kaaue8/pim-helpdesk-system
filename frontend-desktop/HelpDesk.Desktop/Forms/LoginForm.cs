using HelpDesk.Desktop.Models;
using HelpDesk.Desktop.Services;
using HelpDeskDesktop;
using HelpDeskDesktop.Models;
using HelpDeskDesktop.Services;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace HelpDesk.Desktop
{
    public partial class LoginForm : Form
    {
        private readonly ApiService _apiService;

        private TextBox txtEmail;
        private TextBox txtSenha;
        private Button btnLogin;
        private Label lblTitulo;
        private Label lblEmail;
        private Label lblSenha;
        private Panel panelMain;

        public LoginForm()
        {
            _apiService = new ApiService();
            InitializeComponent();
            ConfigurarInterface();
        }

        private void ConfigurarInterface()
        {
            // Configurações do Form
            this.Text = "HelpDesk PIM - Login";
            this.Size = new Size(400, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(124, 58, 237); // Roxo

            // Panel Principal
            panelMain = new Panel
            {
                Size = new Size(350, 400),
                Location = new Point(25, 50),
                BackColor = Color.White
            };

            // Título
            lblTitulo = new Label
            {
                Text = "HelpDesk PIM",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(124, 58, 237),
                AutoSize = true,
                Location = new Point(70, 30)
            };

            // Label Email
            lblEmail = new Label
            {
                Text = "Email:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(31, 41, 55),
                AutoSize = true,
                Location = new Point(30, 120)
            };

            // TextBox Email
            txtEmail = new TextBox
            {
                Size = new Size(290, 30),
                Location = new Point(30, 145),
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
                Location = new Point(30, 190)
            };

            // TextBox Senha
            txtSenha = new TextBox
            {
                Size = new Size(290, 30),
                Location = new Point(30, 215),
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle,
                PasswordChar = '●'
            };

            // Botão Login
            btnLogin = new Button
            {
                Text = "Entrar",
                Size = new Size(290, 45),
                Location = new Point(30, 280),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(124, 58, 237),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;

            // Adicionar controles ao panel
            panelMain.Controls.Add(lblTitulo);
            panelMain.Controls.Add(lblEmail);
            panelMain.Controls.Add(txtEmail);
            panelMain.Controls.Add(lblSenha);
            panelMain.Controls.Add(txtSenha);
            panelMain.Controls.Add(btnLogin);

            // Adicionar panel ao form
            this.Controls.Add(panelMain);
        }

        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            // Validação básica
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                MessageBox.Show("Por favor, preencha todos os campos.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnLogin.Enabled = false;
            btnLogin.Text = "Entrando...";

            try
            {
                var loginRequest = new LoginRequest
                {
                    Email = txtEmail.Text,
                    Senha = txtSenha.Text
                };

                var loginResponse = await _apiService.LoginAsync(loginRequest);

                if (loginResponse != null && loginResponse.Usuario != null)
                {
                    // Verificar se é Admin
                    if (loginResponse.Usuario.Perfil != "Admin")
                    {
                        MessageBox.Show("Acesso negado. Apenas administradores podem acessar este sistema.",
                            "Acesso Negado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Salvar token
                    _apiService.SetToken(loginResponse.Token);

                    // Abrir o MainForm
                    this.Hide();
                    var mainForm = new MainForm(_apiService, loginResponse.Usuario);
                    mainForm.FormClosed += (s, args) => this.Close();
                    mainForm.Show();
                }
                else
                {
                    MessageBox.Show("Email ou senha inválidos.", "Erro de Login",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao fazer login: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnLogin.Enabled = true;
                btnLogin.Text = "Entrar";
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new Size(400, 500);
            this.Name = "LoginForm";
            this.ResumeLayout(false);
        }
    }
}