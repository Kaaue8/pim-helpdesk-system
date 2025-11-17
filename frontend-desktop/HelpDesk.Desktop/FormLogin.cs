using System;
using System.Windows.Forms;
using HelpDesk.Desktop.Services;

namespace HelpDesk.Desktop
{
    public partial class FormLogin : Form
    {
        private readonly ApiService _apiService;

        public FormLogin()
        {
            InitializeComponent();
            _apiService = new ApiService();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtSenha.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Por favor, preencha o e-mail e a senha.", "Erro de Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnLogin.Enabled = false;

            // --- SIMULAÇÃO DE SUCESSO DE LOGIN PARA AVANÇAR NO FRONTEND ---
            string result = "token_simulado";
            // --- FIM DA SIMULAÇÃO ---

            if (result.StartsWith("Erro"))
            {
                MessageBox.Show(result, "Erro de Autenticação", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnLogin.Enabled = true; // Reabilita o botão em caso de erro
            }
            else
            {
                // Sucesso na autenticação
                MessageBox.Show("Login realizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Abre a tela principal e fecha a de login
                FormPrincipalAdmin principalForm = new FormPrincipalAdmin();
                principalForm.Show();
                this.Hide();
            }
        }
    }
}
