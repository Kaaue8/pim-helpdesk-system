using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HelpDesk.Desktop.Models;
using HelpDesk.Desktop.Services;

namespace HelpDesk.Desktop
{
    public partial class TicketsForm : Form
    {
        private readonly ApiService _apiService;
        private readonly Usuario _usuarioLogado;
        private DataGridView dgvTickets;
        private Button btnNovo;
        private Button btnEditar;
        private Button btnExcluir;
        private Button btnAtualizar;
        private TextBox txtBuscar;
        private ComboBox cmbFiltroStatus;
        private Label lblTitulo;
        private List<Ticket> _todosTickets;

        public TicketsForm(ApiService apiService, Usuario usuario)
        {
            _apiService = apiService;
            _usuarioLogado = usuario;
            _todosTickets = new List<Ticket>();

            InitializeComponent();
            ConfigurarInterface();
            CarregarTickets();
        }

        private void ConfigurarInterface()
        {
            this.Size = new Size(950, 700);
            this.BackColor = Color.FromArgb(249, 250, 251);

            lblTitulo = new Label
            {
                Text = "Gerenciamento de Tickets",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(55, 65, 81),
                AutoSize = true,
                Location = new Point(30, 30)
            };

            txtBuscar = new TextBox
            {
                Size = new Size(300, 30),
                Location = new Point(30, 80),
                Font = new Font("Segoe UI", 10),
                PlaceholderText = "Buscar por título..."
            };
            txtBuscar.TextChanged += (s, e) => FiltrarTickets();

            cmbFiltroStatus = new ComboBox
            {
                Size = new Size(150, 30),
                Location = new Point(350, 80),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbFiltroStatus.Items.AddRange(new object[] { "Todos", "Aberto", "Em Andamento", "Resolvido", "Fechado" });
            cmbFiltroStatus.SelectedIndex = 0;
            cmbFiltroStatus.SelectedIndexChanged += (s, e) => FiltrarTickets();

            btnNovo = new Button
            {
                Text = "+ Novo Ticket",
                Size = new Size(130, 35),
                Location = new Point(550, 78),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(124, 58, 237),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnNovo.FlatAppearance.BorderSize = 0;
            btnNovo.Click += BtnNovo_Click;

            btnAtualizar = new Button
            {
                Text = "Atualizar",
                Size = new Size(100, 35),
                Location = new Point(700, 78),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAtualizar.FlatAppearance.BorderSize = 0;
            btnAtualizar.Click += (s, e) => CarregarTickets();

            dgvTickets = new DataGridView
            {
                Size = new Size(890, 450),
                Location = new Point(30, 130),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                Font = new Font("Segoe UI", 9)
            };
            dgvTickets.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(124, 58, 237);
            dgvTickets.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvTickets.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvTickets.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 251);

            btnEditar = new Button
            {
                Text = "Editar",
                Size = new Size(100, 35),
                Location = new Point(30, 600),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(251, 146, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnEditar.FlatAppearance.BorderSize = 0;
            btnEditar.Click += BtnEditar_Click;

            btnExcluir = new Button
            {
                Text = "Excluir",
                Size = new Size(100, 35),
                Location = new Point(150, 600),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(220, 38, 38),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnExcluir.FlatAppearance.BorderSize = 0;
            btnExcluir.Click += BtnExcluir_Click;

            this.Controls.Add(lblTitulo);
            this.Controls.Add(txtBuscar);
            this.Controls.Add(cmbFiltroStatus);
            this.Controls.Add(btnNovo);
            this.Controls.Add(btnAtualizar);
            this.Controls.Add(dgvTickets);
            this.Controls.Add(btnEditar);
            this.Controls.Add(btnExcluir);
        }

        private async void CarregarTickets()
        {
            try
            {
                var tickets = await _apiService.GetTicketsAsync();

                if (tickets != null)
                {
                    _todosTickets = tickets;
                    dgvTickets.DataSource = tickets;
                    ConfigurarColunas();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar tickets: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarColunas()
        {
            if (dgvTickets.Columns.Count > 0)
            {
                dgvTickets.Columns["Id"].HeaderText = "ID";
                dgvTickets.Columns["Id"].Width = 50;
                dgvTickets.Columns["Titulo"].HeaderText = "Título";
                dgvTickets.Columns["Status"].HeaderText = "Status";
                dgvTickets.Columns["Status"].Width = 120;
                dgvTickets.Columns["Prioridade"].HeaderText = "Prioridade";
                dgvTickets.Columns["Prioridade"].Width = 100;
                dgvTickets.Columns["DataCriacao"].HeaderText = "Data Criação";
                dgvTickets.Columns["DataCriacao"].Width = 150;

                if (dgvTickets.Columns["Descricao"] != null) dgvTickets.Columns["Descricao"].Visible = false;
                if (dgvTickets.Columns["Usuario"] != null) dgvTickets.Columns["Usuario"].Visible = false;
                if (dgvTickets.Columns["Analista"] != null) dgvTickets.Columns["Analista"].Visible = false;
                if (dgvTickets.Columns["Setor"] != null) dgvTickets.Columns["Setor"].Visible = false;
                if (dgvTickets.Columns["UsuarioId"] != null) dgvTickets.Columns["UsuarioId"].Visible = false;
                if (dgvTickets.Columns["AnalistaId"] != null) dgvTickets.Columns["AnalistaId"].Visible = false;
                if (dgvTickets.Columns["SetorId"] != null) dgvTickets.Columns["SetorId"].Visible = false;
                if (dgvTickets.Columns["DataAtualizacao"] != null) dgvTickets.Columns["DataAtualizacao"].Visible = false;
                if (dgvTickets.Columns["Categoria"] != null) dgvTickets.Columns["Categoria"].Visible = false;
            }
        }

        private void FiltrarTickets()
        {
            var ticketsFiltrados = _todosTickets.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(txtBuscar.Text))
            {
                ticketsFiltrados = ticketsFiltrados.Where(t =>
                    t.Titulo.Contains(txtBuscar.Text, StringComparison.OrdinalIgnoreCase));
            }

            if (cmbFiltroStatus.SelectedIndex > 0)
            {
                var status = cmbFiltroStatus.SelectedItem?.ToString();
                ticketsFiltrados = ticketsFiltrados.Where(t => t.Status == status);
            }

            dgvTickets.DataSource = ticketsFiltrados.ToList();
        }

        private void BtnNovo_Click(object sender, EventArgs e)
        {
            var form = new TicketEdicaoForm(_apiService, null, _usuarioLogado);
            if (form.ShowDialog() == DialogResult.OK)
            {
                CarregarTickets();
            }
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (dgvTickets.SelectedRows.Count > 0)
            {
                var ticket = dgvTickets.SelectedRows[0].DataBoundItem as Ticket;
                var form = new TicketEdicaoForm(_apiService, ticket, _usuarioLogado);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    CarregarTickets();
                }
            }
            else
            {
                MessageBox.Show("Selecione um ticket para editar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void BtnExcluir_Click(object sender, EventArgs e)
        {
            if (dgvTickets.SelectedRows.Count > 0)
            {
                var ticket = dgvTickets.SelectedRows[0].DataBoundItem as Ticket;

                var result = MessageBox.Show($"Deseja realmente excluir o ticket '{ticket?.Titulo}'?",
                    "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes && ticket != null)
                {
                    try
                    {
                        var sucesso = await _apiService.DeleteTicketAsync(ticket.Id);

                        if (sucesso)
                        {
                            MessageBox.Show("Ticket excluído com sucesso!", "Sucesso",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CarregarTickets();
                        }
                        else
                        {
                            MessageBox.Show("Erro ao excluir ticket.", "Erro",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro ao excluir ticket: {ex.Message}", "Erro",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecione um ticket para excluir.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}