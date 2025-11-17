using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using HelpDesk.Desktop.Models;
using HelpDesk.Desktop.Services;
using HelpDesk.Desktop.Utils;

namespace HelpDesk.Desktop.Forms
{
    public partial class FormLogin : Form
    {
        private PictureBox picBackground;
        private Panel pnlCard;
        private Label lblTitulo;
        private Label lblSubtitulo;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblSenha;
        private TextBox txtSenha;
        private Button btnAcessar;
        private CheckBox chkLGPD;
        private LinkLabel lnkEsqueciSenha;
        private PictureBox picAstronauta;
        private Label lblHouston;

        public FormLogin()
        {
            InitializeComponent();
            ConfigurarComponentes();
        }

        private void ConfigurarComponentes()
        {
            // Configurações do Form
            this.Text = "HelpDesk - Login";
            this.Size = new Size(1300, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(45, 0, 85);

            // Imagem de Fundo
            picBackground = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.FromArgb(45, 0, 85)
            };

            // Tentar carregar imagem de fundo
            string backgroundPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "background.jpg");
            if (File.Exists(backgroundPath))
            {
                try
                {
                    picBackground.Image = Image.FromFile(backgroundPath);
                }
                catch { }
            }

            this.Controls.Add(picBackground);

            // Card Principal
            pnlCard = new Panel
            {
                Size = new Size(450, 600),
                Location = new Point((this.ClientSize.Width - 450) / 2, (this.ClientSize.Height - 600) / 2),
                BackColor = Color.FromArgb(200, 195, 205), // #C8C3CD
            };

            // Bordas arredondadas
            pnlCard.Paint += (s, e) =>
            {
                var path = new System.Drawing.Drawing2D.GraphicsPath();
                int radius = 20;
                path.AddArc(0, 0, radius, radius, 180, 90);
                path.AddArc(pnlCard.Width - radius, 0, radius, radius, 270, 90);
                path.AddArc(pnlCard.Width - radius, pnlCard.Height - radius, radius, radius, 0, 90);
                path.AddArc(0, pnlCard.Height - radius, radius, radius, 90, 90);
                path.CloseFigure();
                pnlCard.Region = new Region(path);
            };

            picBackground.Controls.Add(pnlCard);

            // Título
            lblTitulo = new Label
            {
                Text = "HELPCENTER APOLLO",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 0, 85),
                Location = new Point(50, 40),
                Size = new Size(350, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlCard.Controls.Add(lblTitulo);

            // Subtítulo
            lblSubtitulo = new Label
            {
                Text = "Bem vindo ao helpcenter, efetue seu login abaixo!",
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                ForeColor = Color.FromArgb(60, 60, 60),
                Location = new Point(50, 85),
                Size = new Size(350, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlCard.Controls.Add(lblSubtitulo);

            // Label Email
            lblEmail = new Label
            {
                Text = "Email",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 0, 150),
                Location = new Point(75, 140),
                Size = new Size(300, 20)
            };
            pnlCard.Controls.Add(lblEmail);

            // TextBox Email
            txtEmail = new TextBox
            {
                Font = new Font("Segoe UI", 11F),
                Location = new Point(75, 165),
                Size = new Size(300, 30),
                BorderStyle = BorderStyle.FixedSingle
            };
            pnlCard.Controls.Add(txtEmail);

            // Label Senha
            lblSenha = new Label
            {
                Text = "Senha",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 0, 150),
                Location = new Point(75, 210),
                Size = new Size(300, 20)
            };
            pnlCard.Controls.Add(lblSenha);

            // TextBox Senha
            txtSenha = new TextBox
            {
                Font = new Font("Segoe UI", 11F),
                Location = new Point(75, 235),
                Size = new Size(300, 30),
                PasswordChar = '●',
                BorderStyle = BorderStyle.FixedSingle
            };
            txtSenha.KeyPress += TxtSenha_KeyPress;
            pnlCard.Controls.Add(txtSenha);

            // Botão Acessar
            btnAcessar = new Button
            {
                Text = "Acessar",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Location = new Point(75, 280),
                Size = new Size(300, 45),
                BackColor = Color.FromArgb(124, 58, 237),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAcessar.FlatAppearance.BorderSize = 0;
            btnAcessar.Click += BtnAcessar_Click;
            pnlCard.Controls.Add(btnAcessar);

            // Checkbox LGPD
            chkLGPD = new CheckBox
            {
                Text = "Li e concordo com os Termos de Uso e a Política de Privacidade (LGPD)",
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.FromArgb(100, 0, 150),
                Location = new Point(75, 340),
                Size = new Size(300, 40),
                Checked = false
            };
            pnlCard.Controls.Add(chkLGPD);

            // Link Esqueci Senha
            lnkEsqueciSenha = new LinkLabel
            {
                Text = "Esqueci minha senha",
                Font = new Font("Segoe UI", 9F, FontStyle.Underline),
                LinkColor = Color.FromArgb(100, 0, 150),
                Location = new Point(150, 390),
                Size = new Size(150, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };
            lnkEsqueciSenha.Click += LnkEsqueciSenha_Click;
            pnlCard.Controls.Add(lnkEsqueciSenha);

            // Texto Houston
            lblHouston = new Label
            {
                Text = "Esse aqui é o Houston, ele sempre\nte ajudará com seus chamados!",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(60, 60, 60),
                Location = new Point(50, 450),
                Size = new Size(260, 80),
                TextAlign = ContentAlignment.MiddleLeft
            };
            pnlCard.Controls.Add(lblHouston);

            // Astronauta
            picAstronauta = new PictureBox
            {
                Location = new Point(320, 440),
                Size = new Size(100, 100),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };

            // Tentar carregar imagem do astronauta
            string astronautPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "astronaut.png");
            if (File.Exists(astronautPath))
            {
                try
                {
                    picAstronauta.Image = Image.FromFile(astronautPath);
                }
                catch { }
            }

            pnlCard.Controls.Add(picAstronauta);
        }

        private async void BtnAcessar_Click(object sender, EventArgs e)
        {
            // Validar LGPD
            if (!chkLGPD.Checked)
            {
                AppStyles.ShowWarning("Você precisa concordar com os Termos de Uso e Política de Privacidade (LGPD) para continuar.");
                return;
            }

            // Validações
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                AppStyles.ShowWarning("Por favor, informe o e-mail.");
                txtEmail.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                AppStyles.ShowWarning("Por favor, informe a senha.");
                txtSenha.Focus();
                return;
            }

            try
            {
                btnAcessar.Enabled = false;
                btnAcessar.Text = "Entrando...";

                // Tentar fazer login
                var response = await ApiService.Instance.LoginAsync(txtEmail.Text, txtSenha.Text);

                if (response != null && !string.IsNullOrEmpty(response.Token) && response.Usuario != null)
                {
                    // Salvar autenticação
                    AuthService.Instance.SetAuthentication(response.Usuario, response.Token);

                    // Abrir Dashboard
                    this.Hide();
                    var dashboard = new FormDashboard();
                    dashboard.FormClosed += (s, args) => this.Close();
                    dashboard.Show();
                }
                else
                {
                    AppStyles.ShowError("Credenciais inválidas. Verifique seu e-mail e senha.");
                    btnAcessar.Enabled = true;
                    btnAcessar.Text = "Acessar";
                }
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                btnAcessar.Enabled = true;
                btnAcessar.Text = "Acessar";

                AppStyles.ShowError($"Erro de conexão com o servidor.\n\n" +
                    $"Verifique:\n" +
                    $"1. Se a URL da API está correta em AppConfig.cs\n" +
                    $"2. Se a API está online e acessível\n" +
                    $"3. Sua conexão com a internet\n\n" +
                    $"URL atual: {AppConfig.ApiBaseUrl}\n\n" +
                    $"Detalhes: {ex.Message}");
            }
            catch (Exception ex)
            {
                btnAcessar.Enabled = true;
                btnAcessar.Text = "Acessar";

                AppStyles.ShowError($"Erro ao fazer login:\n{ex.Message}");
            }
        }

        private void LnkEsqueciSenha_Click(object sender, EventArgs e)
        {
            AppStyles.ShowInfo("Funcionalidade de recuperação de senha em desenvolvimento.\n\n" +
                "Entre em contato com o administrador do sistema.");
        }

        private void TxtSenha_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                BtnAcessar_Click(sender, e);
            }
        }
    }
}

