using System;
using System.Drawing;
using System.Windows.Forms;
using HelpDesk.Desktop.Models;
using HelpDesk.Desktop.Services;

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
        private Panel panelLogin;

        public LoginForm()
        {
            // Inicializar API Service - ALTERE A URL PARA SUA API NO RENDER
            _apiService = new ApiService("https://pim-helpdesk-system.onrender.com/api/Login/Authenticate");

            InitializeComponent();
            ConfigurarInterface();
        }

        private void ConfigurarInterface()
        {
            // Configurações do Form
            this.ClientSize = new Size(400, 500);
            this.Text = "HelpDesk - Login";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(249, 250, 251);

            // Painel Principal
            panelLogin = new Panel
            {
                Size = new Size(340, 400),
                Location = new Point(30, 50),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };

            // Adicionar sombra simulada
            panelLogin.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, panelLogin.ClientRectangle,
                    Color.FromArgb(229, 231, 235), 1, ButtonBorderStyle.Solid,
                    Color.FromArgb(229, 231, 235), 1, ButtonBorderStyle.Solid,
                    Color.FromArgb(229, 231, 235), 1, ButtonBorderStyle.Solid,
                    Color.FromArgb(229, 231, 235), 1, ButtonBorderStyle.Solid);
            };

            // Título
            lblTitulo = new Label
            {
                Text = "HelpDesk Login",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(124, 58, 237),
                AutoSize = true,
                Location = new Point(60, 40)
            };

            // Label Email
            lblEmail = new Label
            {
                Text = "E-mail",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(55, 65, 81),
                AutoSize = true,
                Location = new Point(30, 120)
            };

            // TextBox Email
            txtEmail = new TextBox
            {
                Size = new Size(280, 35),
                Location = new Point(30, 145),
                Font = new Font("Segoe UI", 11),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Label Senha
            lblSenha = new Label
            {
                Text = "Senha",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(55, 65, 81),
                AutoSize = true,
                Location = new Point(30, 200)
            };

            // TextBox Senha
            txtSenha = new TextBox
            {
                Size = new Size(280, 35),
                Location = new Point(30, 225),
                Font = new Font("Segoe UI", 11),
                BorderStyle = BorderStyle.FixedSingle,
                PasswordChar = '●'
            };

            // Botão Login
            btnLogin = new Button
            {
                Text = "Entrar",
                Size = new Size(280, 45),
                Location = new Point(30, 290),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(124, 58, 237),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;

            // Adicionar controles ao painel
            panelLogin.Controls.Add(lblTitulo);
            panelLogin.Controls.Add(lblEmail);
            panelLogin.Controls.Add(txtEmail);
            panelLogin.Controls.Add(lblSenha);
            panelLogin.Controls.Add(txtSenha);
            panelLogin.Controls.Add(btnLogin);

            // Adicionar painel ao form
            this.Controls.Add(panelLogin);
        }

        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                MessageBox.Show("Por favor, preencha todos os campos.", "Aviso",
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

                var response = await _apiService.LoginAsync(loginRequest);

                if (response != null && !string.IsNullOrEmpty(response.Token))
                {
                    _apiService.SetToken(response.Token);

                    MessageBox.Show("Login realizado com sucesso!", "Sucesso",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Abrir formulário principal
                    var mainForm = new MainForm(_apiService, response.Usuario);
                    mainForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("E-mail ou senha inválidos.", "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao conectar com o servidor: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnLogin.Enabled = true;
                btnLogin.Text = "Entrar";
            }
        }
    }
}
