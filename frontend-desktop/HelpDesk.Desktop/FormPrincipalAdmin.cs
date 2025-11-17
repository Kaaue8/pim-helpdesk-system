using System;
using System.Drawing;
using System.Windows.Forms;

namespace HelpDesk.Desktop
{
    public partial class FormPrincipalAdmin : Form
    {
        public FormPrincipalAdmin()
        {
            InitializeComponent();
            this.Text = "Helpcenter Apollo - Administração";
            this.WindowState = FormWindowState.Maximized;
            SetupLayout();

            // --- CORREÇÃO: CARREGA O DASHBOARD AO INICIAR ---
            LoadContentForm("Dashboard");
            // --- FIM DA CORREÇÃO ---
        }

        private void SetupLayout()
        {
            // ... (código do SetupLayout que você já tem)
            // ... (código do AddMenuItem que você já tem)
            // ... (código do MenuItem_Click que você já tem)
        }

        private void AddMenuItem(string text)
        {
            // ... (código do AddMenuItem que você já tem)
        }

        private void MenuItem_Click(object sender, EventArgs e)
        {
            // ... (código do MenuItem_Click que você já tem)
        }

        private void LoadContentForm(string formName)
        {
            // Limpa o painel de conteúdo
            pnlContent.Controls.Clear();

            Form contentForm = null;

            switch (formName)
            {
                case "Fila Chamados":
                    contentForm = new FormFilaChamados();
                    break;
                case "Histórico Chamados":
                    // ... (código do Histórico Chamados que você já tem)
                    return;
                case "Dashboard":
                    contentForm = new FormDashboard();
                    break;
                case "Usuários":
                    contentForm = new FormGestaoUsuarios();
                    break;
                case "FAQ":
                    // ... (código do FAQ que você já tem)
                    return;
                default:
                    // ... (código do default que você já tem)
                    return;
            }

            if (contentForm != null)
            {
                contentForm.TopLevel = false;
                contentForm.FormBorderStyle = FormBorderStyle.None;
                contentForm.Dock = DockStyle.Fill;
                pnlContent.Controls.Add(contentForm);
                contentForm.Show();
            }
        }
    }
}
