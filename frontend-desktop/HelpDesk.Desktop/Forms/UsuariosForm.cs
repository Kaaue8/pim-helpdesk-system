using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HelpDeskDesktop.Services;

namespace HelpDeskDesktop
{
    public partial class UsuariosForm : Form
    {
        private readonly ApiService _apiService;

        private DataGridView dgvUsuarios;
        private Button btnNovo;
        private Button btnEditar;
        private Button btnExcluir;
        private Button btnAtualizar;
        private TextBox txtBusca;
        private Label lblTitulo;
        private Panel panelHeader;
        private Panel panelAcoes;

        private List<Usuario> _todosUsuarios;

        public UsuariosForm(ApiService apiService)
        {
            _apiService = apiService;
            InitializeComponent();
            ConfigurarInterface();
            CarregarUsuarios();
        }

        private void ConfigurarInterface()
        {
            // Configurações do Form
            this.Text = "Gestão de Usuários";
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
                Text = "Gestão de Usuários",
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
                PlaceholderText = "Buscar por nome ou email..."
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

            btnNovo = CriarBotao("Novo Usuário", 20, Color.FromArgb(124, 58, 237));
            btnNovo.Click += BtnNovo_Click;

            btnEditar = CriarBotao("Editar", 150, Color.FromArgb(59, 130, 246));
            btnEditar.Click += BtnEditar_Click;

            btnExcluir = CriarBotao("Excluir", 270, Color.FromArgb(239, 68, 68));
            btnExcluir.Click += BtnExcluir_Click;

            btnAtualizar = CriarBotao("Atualizar", 390, Color.FromArgb(34, 197, 94));
            btnAtualizar.Click += (s, e) => CarregarUsuarios();

            panelAcoes.Controls.Add(btnNovo);
            panelAcoes.Controls.Add(btnEditar);
            panelAcoes.Controls.Add(btnExcluir);
            panelAcoes.Controls.Add(btnAtualizar);

            // DataGridView
            dgvUsuarios = new DataGridView
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
            this.Controls.Add(dgvUsuarios);
            this.Controls.Add(panelHeader);
            this.Controls.Add(panelAcoes);
        }

        private Button CriarBotao(string texto, int x, Color cor)
        {
            return new Button
            {
                Text = texto,
                Size = new Size(120, 35),
                Location = new Point(x, 12),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = cor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
        }

        private async void CarregarUsuarios()
        {
            try
            {
                btnAtualizar.Enabled = false;
                btnAtualizar.Text = "Carregando...";

                _todosUsuarios = await _apiService.GetUsuariosAsync();
                AtualizarGrid(_todosUsuarios);

                MessageBox.Show($"{_todosUsuarios.Count} usuários carregados.", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar usuários: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnAtualizar.Enabled = true;
                btnAtualizar.Text = "Atualizar";
            }
        }

        private void AtualizarGrid(List<Usuario> usuarios)
        {
            dgvUsuarios.DataSource = null;
            dgvUsuarios.DataSource = usuarios;

            // Configurar colunas
            if (dgvUsuarios.Columns.Count > 0)
            {
                dgvUsuarios.Columns["Id"].HeaderText = "ID";
                dgvUsuarios.Columns["Id"].Width = 50;
                dgvUsuarios.Columns["Nome"].HeaderText = "Nome";
                dgvUsuarios.Columns["Email"].HeaderText = "Email";
                dgvUsuarios.Columns["Perfil"].HeaderText = "Perfil";
                dgvUsuarios.Columns["Perfil"].Width = 100;
                dgvUsuarios.Columns["SetorId"].HeaderText = "Setor ID";
                dgvUsuarios.Columns["SetorId"].Width = 80;

                // Ocultar a coluna de senha
                if (dgvUsuarios.Columns.Contains("Senha"))
                {
                    dgvUsuarios.Columns["Senha"].Visible = false;
                }
            }
        }

        private void TxtBusca_TextChanged(object sender, EventArgs e)
        {
            if (_todosUsuarios == null) return;

            var termoBusca = txtBusca.Text.ToLower();
            var usuariosFiltrados = _todosUsuarios.Where(u =>
                u.Nome.ToLower().Contains(termoBusca) ||
                u.Email.ToLower().Contains(termoBusca)
            ).ToList();

            AtualizarGrid(usuariosFiltrados);
        }

        private void BtnNovo_Click(object sender, EventArgs e)
        {
            var formEdicao = new UsuarioEdicaoForm(_apiService, null);
            if (formEdicao.ShowDialog() == DialogResult.OK)
            {
                CarregarUsuarios();
            }
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um usuário para editar.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var usuarioSelecionado = (Usuario)dgvUsuarios.SelectedRows[0].DataBoundItem;
            var formEdicao = new UsuarioEdicaoForm(_apiService, usuarioSelecionado);
            if (formEdicao.ShowDialog() == DialogResult.OK)
            {
                CarregarUsuarios();
            }
        }

        private async void BtnExcluir_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um usuário para excluir.", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var usuarioSelecionado = (Usuario)dgvUsuarios.SelectedRows[0].DataBoundItem;
            var result = MessageBox.Show($"Deseja realmente excluir o usuário '{usuarioSelecionado.Nome}'?",
                "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var sucesso = await _apiService.DeleteUsuarioAsync(usuarioSelecionado.Id);
                    if (sucesso)
                    {
                        MessageBox.Show("Usuário excluído com sucesso!", "Sucesso",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CarregarUsuarios();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao excluir usuário: {ex.Message}", "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new Size(1000, 600);
            this.Name = "UsuariosForm";
            this.ResumeLayout(false);
        }
    }
}