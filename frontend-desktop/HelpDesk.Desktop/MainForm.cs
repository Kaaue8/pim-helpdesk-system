using System;
using System.Drawing;
using System.Windows.Forms;
using HelpDesk.Desktop.Models;
using HelpDesk.Desktop.Services;

namespace HelpDesk.Desktop
{
    public partial class MainForm : Form
    {
        private readonly ApiService _apiService;
        private readonly Usuario _usuarioLogado;
        private Panel panelMenu;
        private Panel panelConteudo;
        private Label lblTitulo;
        private Label lblUsuario;
        private Button btnTickets;
        private Button btnUsuarios;
        private Button btnSetores;
        private Button btnSair;

        public MainForm(ApiService apiService, Usuario usuario)
        {
            _apiService = apiService;
            _usuarioLogado = usuario;

            InitializeComponent();
            ConfigurarInterface();
        }

        private void ConfigurarInterface()
        {
            // Configurações do Form
            this.ClientSize = new Size(1200, 700);
            this.Text = "HelpDesk - Sistema de Gerenciamento";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(249, 250, 251);

            // Painel de Menu Lateral
            panelMenu = new Panel
            {
                Size = new Size(250, 700),
                Location = new Point(0, 0),
                BackColor = Color.FromArgb(124, 58, 237),
                Dock = DockStyle.Left
            };

            // Título do Sistema
            lblTitulo = new Label
            {
                Text = "HelpDesk",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(50, 30)
            };

            // Nome do Usuário Logado
            lblUsuario = new Label
            {
                Text = $"Olá, {_usuarioLogado?.Nome ?? "Usuário"}",
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 80)
            };

            // Botão Tickets
            btnTickets = CriarBotaoMenu("Tickets", 150);
            btnTickets.Click += (s, e) => AbrirFormulario(new TicketsForm(_apiService, _usuarioLogado));

            // Botão Usuários
            btnUsuarios = CriarBotaoMenu("Usuários", 210);
            btnUsuarios.Click += (s, e) => AbrirFormulario(new UsuariosForm(_apiService));

            if (_usuarioLogado?.Perfil != "Admin")
            {
                btnUsuarios.Enabled = false;
                btnUsuarios.BackColor = Color.FromArgb(100, 40, 180);
            }

            // Botão Setores
            btnSetores = CriarBotaoMenu("Setores", 270);
            btnSetores.Click += (s, e) => AbrirFormulario(new SetoresForm(_apiService));

            if (_usuarioLogado?.Perfil != "Admin")
            {
                btnSetores.Enabled = false;
                btnSetores.BackColor = Color.FromArgb(100, 40, 180);
            }

            // Botão Sair
            btnSair = CriarBotaoMenu("Sair", 600);
            btnSair.BackColor = Color.FromArgb(220, 38, 38);
            btnSair.Click += BtnSair_Click;

            // Adicionar controles ao menu
            panelMenu.Controls.Add(lblTitulo);
            panelMenu.Controls.Add(lblUsuario);
            panelMenu.Controls.Add(btnTickets);
            panelMenu.Controls.Add(btnUsuarios);
            panelMenu.Controls.Add(btnSetores);
            panelMenu.Controls.Add(btnSair);

            // Painel de Conteúdo
            panelConteudo = new Panel
            {
                Location = new Point(250, 0),
                Size = new Size(950, 700),
                BackColor = Color.FromArgb(249, 250, 251),
                Dock = DockStyle.Fill
            };

            var lblBoasVindas = new Label
            {
                Text = "Bem-vindo ao Sistema HelpDesk",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(55, 65, 81),
                AutoSize = true,
                Location = new Point(50, 50)
            };

            var lblInstrucoes = new Label
            {
                Text = "Selecione uma opção no menu lateral para começar.",
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                ForeColor = Color.FromArgb(107, 114, 128),
                AutoSize = true,
                Location = new Point(50, 100)
            };

            panelConteudo.Controls.Add(lblBoasVindas);
            panelConteudo.Controls.Add(lblInstrucoes);

            this.Controls.Add(panelMenu);
            this.Controls.Add(panelConteudo);
        }

        private Button CriarBotaoMenu(string texto, int y)
        {
            var btn = new Button
            {
                Text = texto,
                Size = new Size(210, 45),
                Location = new Point(20, y),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(109, 40, 217),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0)
            };
            btn.FlatAppearance.BorderSize = 0;

            return btn;
        }

        private void AbrirFormulario(Form formulario)
        {
            panelConteudo.Controls.Clear();
            formulario.TopLevel = false;
            formulario.FormBorderStyle = FormBorderStyle.None;
            formulario.Dock = DockStyle.Fill;
            panelConteudo.Controls.Add(formulario);
            formulario.Show();
        }

        private void BtnSair_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Deseja realmente sair?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}