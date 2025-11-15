using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HelpDesk.Desktop.Models;
using HelpDesk.Desktop.Services;

namespace HelpDesk.Desktop
{
    public partial class SetoresForm : Form
    {
        private readonly ApiService _apiService;
        private DataGridView dgvSetores;
        private Button btnNovo;
        private Button btnEditar;
        private Button btnExcluir;
        private Button btnAtualizar;
        private TextBox txtBuscar;
        private Label lblTitulo;
        private List<Setor> _todosSetores;

        public SetoresForm(ApiService apiService)
        {
            _apiService = apiService;
            _todosSetores = new List<Setor>();

            InitializeComponent();
            ConfigurarInterface();
            CarregarSetores();
        }

        private void ConfigurarInterface()
        {
            this.Size = new Size(950, 700);
            this.BackColor = Color.FromArgb(249, 250, 251);

            lblTitulo = new Label
            {
                Text = "Gerenciamento de Setores",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(55, 65, 81),
                AutoSize = true,
                Location = new Point(30, 30)
            };

            txtBuscar = new TextBox
            {
                Size = new Size(400, 30),
                Location = new Point(30, 80),
                Font = new Font("Segoe UI", 10),
                PlaceholderText = "Buscar por nome..."
            };
            txtBuscar.TextChanged += (s, e) => FiltrarSetores();

            btnNovo = new Button
            {
                Text = "+ Novo Setor",
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
            btnAtualizar.Click += (s, e) => CarregarSetores();

            dgvSetores = new DataGridView
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
            dgvSetores.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(124, 58, 237);
            dgvSetores.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvSetores.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvSetores.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 251);

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
            this.Controls.Add(btnNovo);
            this.Controls.Add(btnAtualizar);
            this.Controls.Add(dgvSetores);
            this.Controls.Add(btnEditar);
            this.Controls.Add(btnExcluir);
        }

        private async void CarregarSetores()
        {
            try
            {
                var setores = await _apiService.GetSetoresAsync();

                if (setores != null)
                {
                    _todosSetores = setores;
                    dgvSetores.DataSource = setores;
                    ConfigurarColunas();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar setores: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarColunas()
        {
            if (dgvSetores.Columns.Count > 0)
            {
                dgvSetores.Columns["Id"].HeaderText = "ID";
                dgvSetores.Columns["Id"].Width = 50;
                dgvSetores.Columns["Nome"].HeaderText = "Nome";
                dgvSetores.Columns["Descricao"].HeaderText = "Descrição";
                dgvSetores.Columns["DataCriacao"].HeaderText = "Data Criação";
                dgvSetores.Columns["DataCriacao"].Width = 150;
            }
        }

        private void FiltrarSetores()
        {
            var setoresFiltrados = _todosSetores.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(txtBuscar.Text))
            {
                setoresFiltrados = setoresFiltrados.Where(s =>
                    s.Nome.Contains(txtBuscar.Text, StringComparison.OrdinalIgnoreCase));
            }

            dgvSetores.DataSource = setoresFiltrados.ToList();
        }

        private void BtnNovo_Click(object sender, EventArgs e)
        {
            var form = new SetorEdicaoForm(_apiService, null);
            if (form.ShowDialog() == DialogResult.OK)
            {
                CarregarSetores();
            }
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (dgvSetores.SelectedRows.Count > 0)
            {
                var setor = dgvSetores.SelectedRows[0].DataBoundItem as Setor;
                var form = new SetorEdicaoForm(_apiService, setor);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    CarregarSetores();
                }
            }
            else
            {
                MessageBox.Show("Selecione um setor para editar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void BtnExcluir_Click(object sender, EventArgs e)
        {
            if (dgvSetores.SelectedRows.Count > 0)
            {
                var setor = dgvSetores.SelectedRows[0].DataBoundItem as Setor;

                var result = MessageBox.Show($"Deseja realmente excluir o setor '{setor?.Nome}'?",
                    "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes && setor != null)
                {
                    try
                    {
                        var sucesso = await _apiService.DeleteSetorAsync(setor.Id);

                        if (sucesso)
                        {
                            MessageBox.Show("Setor excluído com sucesso!", "Sucesso",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CarregarSetores();
                        }
                        else
                        {
                            MessageBox.Show("Erro ao excluir setor.", "Erro",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro ao excluir setor: {ex.Message}", "Erro",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecione um setor para excluir.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}