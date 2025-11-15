using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HelpDeskDesktop.Services;

namespace HelpDeskDesktop
{
    public partial class TicketsForm : Form
    {
        private readonly ApiService _apiService;

        private DataGridView dgvTickets;
        private Button btnNovo;
        private Button btnEditar;
        private Button btnExcluir;
        private Button btnAtualizar;
        private TextBox txtBusca;
        private Label lblTitulo;
        private Panel panelHeader;
        private Panel panelAcoes;

        private List<Ticket> _todosTickets;

        public TicketsForm(ApiService apiService)
        {
            _apiService = apiService;
            InitializeComponent();
            ConfigurarInterface();
            CarregarTickets();
        }

        private void ConfigurarInterface()
        {
            // Configurações do Form
            this.Text = "Gestão de Tickets";
            this.Size = new Size(1000, 600);
            this.BackColor = Color.FromArgb(243, 244, 246);

            // Panel Header
            panelHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.White
            };

            lblTitulo = new Label