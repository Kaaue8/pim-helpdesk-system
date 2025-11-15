using HelpDesk.Desktop.Forms;
using HelpDesk.Desktop.Models;
using HelpDesk.Desktop.Services;
using HelpDeskDesktop.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace HelpDeskDesktop
{
    public partial class SetoresForm : Form
    {
        private readonly ApiService _apiService;

        private DataGridView dgvSetores;
        private Button btnNovo;
        private Button btnEditar;
        private Button btnExcluir;
        private Button btnAtualizar;
        private TextBox txtBusca;
        private Label lblTitulo;
        private Panel panelHeader;
        private Panel panelAcoes;

        private List<Setor> _todosSetores;

        public SetoresForm(ApiService apiService)
        {
            _apiService = apiService;
            InitializeComponent();
            ConfigurarInterface();
            CarregarSetores();
        }

        private void ConfigurarInterface()
        {
            // Configurações do Form
            this.Text = "Gestão de Setores";
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
            {
                Text = "Gestão de Setores",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(124, 58, 237),
                AutoSize = true,
                Location = new Point(20, 20)
            };

            txtBusca = new TextBox
            {
                Size = new Size(300, 30),
                Location = new Point(20, 50),
                Font = new Font("Segoe UI", 10),
                PlaceholderText = "Buscar por nome..."
            };
            txtBusca.TextChanged += TxtBusca_TextChanged;

            panelHeader.Controls.Add(lblTitulo);
            panelHeader.Controls.Add(txtBusca);

            // Panel de Ações
            panelAcoes = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = Color.White
            };

            btnNovo = CriarBotao("Novo Setor", 20, Color.FromArgb(124, 58, 237));
            btnNovo.Click += BtnNovo_Click;

            btnEditar = CriarBotao("Editar", 140, Color.FromArgb(59, 130, 246));
            btnEditar.Click += BtnEditar_Click;

            btnExcluir = CriarBotao("Excluir", 260, Color.FromArgb(239, 68, 68));
            btnExcluir.Click += BtnExcluir_Click;

            btnAtualizar = CriarBotao("Atualizar", 380, Color.FromArgb(34, 197, 94));
            btnAtualizar.Click += (s, e) => CarregarSetores();

            panelAcoes.Controls.Add(btnNovo);
            panelAcoes.Controls.Add(btnEditar);
            panelAcoes.Controls.Add(btnExcluir);
            panelAcoes.Controls.Add(btnAtualizar);

            // DataGridView
            dgvSetores = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                Font = new Font("Segoe UI", 9)
            };

            // Adicionar controles ao form
            this.Controls.Add(dgvSetores);
            this.Controls.Add(panelHeader);
            this.Controls.Add(panelAcoes);
        }

        private Button CriarBotao(string texto, int x, Color cor)
        {
            return new Button
            {
                Text = texto,
                Size = new Size(110, 35),
                Location = new Point(x, 12),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = cor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
        }

        private async void CarregarSetores()
        {
            try
            {
                btnAtualizar.Enabled = false;
                btnAtualizar.Text = "Carregando...";

                _todosSetores = await _apiService.GetSetoresAsync();
                AtualizarGrid(_todosSetores);

                MessageBox.Show($"{_todosSetores.Count} setores carregados.", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar setores: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnAtualizar.Enabled = true;
                btnAtualizar.Text = "Atualizar";
            }
        }

        private void AtualizarGrid(List<Setor> setores)
        {
            dgvSetores.DataSource = null;
            dgvSetores.DataSource = setores;

            // Configurar colunas
            if (dgvSetores.Columns.Count > 0)
            {
                dgvSetores.Columns["Id"].HeaderText = "ID";
                dgvSetores.Columns["Id"].Width = 80;
                dgvSetores.Columns["Nome"].HeaderText = "Nome do Setor";
            }
        }

        private void TxtBusca_TextChanged(object sender, EventArgs e)
        {
            if (_todosSetores == null) return;

            var termoBusca = txtBusca.Text.ToLower();
            var setoresFiltrados = _todosSetores.Where(s =>
                s.Nome.ToLower().Contains(termoBusca)
            ).ToList();

            AtualizarGrid(setoresFiltrados);
        }

        private void BtnNovo_Click(object sender, EventArgs e)
        {
            var formEdicao = new SetorEdicaoForm(_apiService, null);
            if (formEdicao.ShowDialog() == DialogResult.OK)
            {
                CarregarSetores();
            }
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (dgvSetores.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um setor para editar.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var setorSelecionado = (Setor)dgvSetores.SelectedRows[0].DataBoundItem;
            var formEdicao = new SetorEdicaoForm(_apiService, setorSelecionado);
            if (formEdicao.ShowDialog() == DialogResult.OK)
            {
                CarregarSetores();
            }
        }

        private async void BtnExcluir_Click(object sender, EventArgs e)
        {
            if (dgvSetores.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um setor para excluir.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var setorSelecionado = (Setor)dgvSetores.SelectedRows[0].DataBoundItem;
            var result = MessageBox.Show($"Deseja realmente excluir o setor '{setorSelecionado.Nome}'?",
                "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var sucesso = await _apiService.DeleteSetorAsync(setorSelecionado.Id);
                    if (sucesso)
                    {
                        MessageBox.Show("Setor excluído com sucesso!", "Sucesso",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CarregarSetores();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao excluir setor: {ex.Message}", "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new Size(1000, 600);
            this.Name = "SetoresForm";
            this.ResumeLayout(false);
        }
    }
}