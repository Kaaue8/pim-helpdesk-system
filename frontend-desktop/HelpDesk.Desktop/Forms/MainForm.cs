using HelpDesk.Desktop.Models;
using HelpDesk.Desktop.Services;
using HelpDeskDesktop;
using HelpDeskDesktop.Services;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace HelpDesk.Desktop
{
    public partial class MainForm : Form
    {
        private readonly ApiService _apiService;
        private readonly Usuario _usuarioLogado;

        private Panel panelHeader;
        private Panel panelMenu;
        private Panel panelConteudo;
        private Label lblBemVindo;
        private Button menuPrincipal;
        private Button menuTickets;
        private Button menuUsuarios;
        private Button menuSetores;
        private Button menuSair;

        public MainForm(ApiService apiService, Usuario usuarioLogado)
        {
            _apiService = apiService;
            _usuarioLogado = usuarioLogado;

            InitializeComponent();
            ConfigurarInterface();
        }

        private void ConfigurarInterface()
        {
            // Configurações do Form
            this.Text = "HelpDesk PIM - Dashboard";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.IsMdiContainer = true;
            this.BackColor = Color.FromArgb(243, 244, 246);

            // Panel Header (Roxo)
            panelHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.FromArgb(124, 58, 237)
            };

            lblBemVindo = new Label
            {
                Text = $"Bem-vindo, {_usuarioLogado.Nome}!",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 20)
            };

            panelHeader.Controls.Add(lblBemVindo);

            // Panel Menu Lateral (Roxo)
            panelMenu = new Panel
            {
                Dock = DockStyle.Left,
                Width = 200,
                BackColor = Color.FromArgb(124, 58, 237)
            };

            // Botões do Menu
            menuPrincipal = CriarBotaoMenu("Dashboard", 20);
            menuPrincipal.Click += MenuPrincipal_Click;

            menuTickets = CriarBotaoMenu("Tickets", 80);
            menuTickets.Click += MenuTickets_Click;

            menuUsuarios = CriarBotaoMenu("Usuários", 140);
            menuUsuarios.Click += MenuUsuarios_Click;

            menuSetores = CriarBotaoMenu("Setores", 200);
            menuSetores.Click += MenuSetores_Click;

            menuSair = CriarBotaoMenu("Sair", 260);
            menuSair.Click += MenuSair_Click;

            panelMenu.Controls.Add(menuPrincipal);
            panelMenu.Controls.Add(menuTickets);
            panelMenu.Controls.Add(menuUsuarios);
            panelMenu.Controls.Add(menuSetores);
            panelMenu.Controls.Add(menuSair);

            // Panel de Conteúdo
            panelConteudo = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(243, 244, 246)
            };

            // Adicionar controles ao form
            this.Controls.Add(panelConteudo);
            this.Controls.Add(panelMenu);
            this.Controls.Add(panelHeader);
        }

        private Button CriarBotaoMenu(string texto, int y)
        {
            return new Button
            {
                Text = texto,
                Size = new Size(180, 45),
                Location = new Point(10, y),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(109, 40, 217),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0)
            };
        }

        private void MenuPrincipal_Click(object sender, EventArgs e)
        {
            LimparConteudo();
            var lblDashboard = new Label
            {
                Text = "Dashboard Principal",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(124, 58, 237),
                AutoSize = true,
                Location = new Point(30, 30)
            };
            panelConteudo.Controls.Add(lblDashboard);
        }

        private void MenuTickets_Click(object sender, EventArgs e)
        {
            AbrirFormNoConteudo(new TicketsForm(_apiService));
        }

        private void MenuUsuarios_Click(object sender, EventArgs e)
        {
            AbrirFormNoConteudo(new UsuariosForm(_apiService));
        }

        private void MenuSetores_Click(object sender, EventArgs e)
        {
            AbrirFormNoConteudo(new SetoresForm(_apiService));
        }

        private void MenuSair_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Deseja realmente sair?", "Confirmar Saída",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void AbrirFormNoConteudo(Form form)
        {
            LimparConteudo();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            panelConteudo.Controls.Add(form);
            form.Show();
        }

        private void LimparConteudo()
        {
            panelConteudo.Controls.Clear();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new Size(1200, 700);
            this.Name = "MainForm";
            this.ResumeLayout(false);
        }
    }
}