using System;
using System.Windows.Forms;
using HelpDesk.Desktop.Services;
using HelpDesk.Desktop.Utils;

namespace HelpDesk.Desktop.Forms
{
    public partial class FormDashboard : Form
    {
        public FormDashboard()
        {
            InitializeComponent();
            ConfigurarEstilo();
            CarregarDadosUsuario();
        }

        private void ConfigurarEstilo()
        {
            this.BackColor = AppStyles.ColorBackground;
        }

        private void CarregarDadosUsuario()
        {
            var usuario = AuthService.Instance.CurrentUser;
            if (usuario != null)
            {
                lblUsuario.Text = $"👤 {usuario.Nome}";
                lblPerfil.Text = usuario.Perfil;

                // Mostrar botão Usuários apenas para Admin
                if (!usuario.IsAdmin())
                {
                    btnUsuarios.Visible = false;
                }
            }
        }

        private void BtnTickets_Click(object sender, EventArgs e)
        {
            var formTickets = new FormTickets();
            formTickets.Show();
            this.Hide();
        }

        private void BtnNovoTicket_Click(object sender, EventArgs e)
        {
            var formNovoTicket = new FormNovoTicket();
            formNovoTicket.Show();
            this.Hide();
        }

        private void BtnUsuarios_Click(object sender, EventArgs e)
        {
            if (AuthService.Instance.IsAdmin())
            {
                var formUsuarios = new FormUsuarios();
                formUsuarios.Show();
                this.Hide();
            }
            else
            {
                AppStyles.ShowWarning("Acesso negado. Apenas administradores podem acessar esta área.");
            }
        }

        private void BtnCategorias_Click(object sender, EventArgs e)
        {
            if (AuthService.Instance.IsAdmin())
            {
                var formCategorias = new FormCategorias();
                formCategorias.Show();
                this.Hide();
            }
            else
            {
                AppStyles.ShowWarning("Acesso negado. Apenas administradores podem acessar esta área.");
            }
        }

        private void BtnSair_Click(object sender, EventArgs e)
        {
            var result = AppStyles.ShowConfirmation("Deseja realmente sair?", "Confirmar Saída");

            if (result == DialogResult.Yes)
            {
                AuthService.Instance.Logout();

                var formLogin = new FormLogin();
                formLogin.Show();
                this.Close();
            }
        }

        private void FormDashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Application.Exit();
            }
        }
    }
}