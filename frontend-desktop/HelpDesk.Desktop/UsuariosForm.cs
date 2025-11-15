using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HelpDesk.Desktop.Models;
using HelpDesk.Desktop.Services;

namespace HelpDesk.Desktop
{
    public partial class UsuariosForm : Form
    {
        private readonly ApiService _apiService;
        private DataGridView dgvUsuarios;
        private Button btnNovo;
        private Button btnEditar;
        private Button btnExcluir;
        private Button btnAtualizar;
        private TextBox txtBuscar;
        private ComboBox cmbFiltroPerfil;
        private Label lblTitulo;
        private List<Usuario> _todosUsuarios;

        public UsuariosForm(ApiService apiService)
        {
            _apiService = apiService;
            _todosUsuarios = new List<Usuario>();

            InitializeComponent();
            ConfigurarInterface();
            CarregarUsuarios();
        }

        private void ConfigurarInterface()
        {
            this.Size = new Size(950, 700);
            this.BackColor = Color.FromArgb(249, 250, 251);

            lblTitulo = new Label
            {
                Text = "Gerenciamento de Usuários",
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
                PlaceholderText = "Buscar por nome ou e-mail..."
            };
            txtBuscar.TextChanged += (s, e) => FiltrarUsuarios();

            cmbFiltroPerfil = new ComboBox
            {
                Size = new Size(150, 30),
                Location = new Point(350, 80),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbFiltroPerfil.Items.AddRange(new object[] { "Todos", "Admin", "Analista", "Usuario" });
            cmbFiltroPerfil.SelectedIndex = 0;
            cmbFiltroPerfil.SelectedIndexChanged += (s, e) => FiltrarUsuarios();

            btnNovo = new Button
            {
                Text = "+ Novo Usuário",
                Size = new Size(140, 35),
                Location = new Point(540, 78),
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
            btnAtualizar.Click += (s, e) => CarregarUsuarios();

            dgvUsuarios = new DataGridView
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
            dgvUsuarios.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(124, 58, 237);
            dgvUsuarios.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvUsuarios.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvUsuarios.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 251);

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
            this.Controls.Add(cmbFiltroPerfil);
            this.Controls.Add(btnNovo);
            this.Controls.Add(btnAtualizar);
            this.Controls.Add(dgvUsuarios);
            this.Controls.Add(btnEditar);
            this.Controls.Add(btnExcluir);
        }

        private async void CarregarUsuarios()
        {
            try
            {
                var usuarios = await _apiService.GetUsuariosAsync();

                if (usuarios != null)
                {
                    _todosUsuarios = usuarios;
                    dgvUsuarios.DataSource = usuarios;
                    ConfigurarColunas();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar usuários: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarColunas()
        {
            if (dgvUsuarios.Columns.Count > 0)
            {
                dgvUsuarios.Columns["Id"].HeaderText = "ID";
                dgvUsuarios.Columns["Id"].Width = 50;
                dgvUsuarios.Columns["Nome"].HeaderText = "Nome";
                dgvUsuarios.Columns["Email"].HeaderText = "E-mail";
                dgvUsuarios.Columns["Perfil"].HeaderText = "Perfil";
                dgvUsuarios.Columns["Perfil"].Width = 100;
                dgvUsuarios.Columns["DataCriacao"].HeaderText = "Data Criação";
                dgvUsuarios.Columns["DataCriacao"].Width = 150;

                if (dgvUsuarios.Columns["Senha"] != null) dgvUsuarios.Columns["Senha"].Visible = false;
                if (dgvUsuarios.Columns["Setor"] != null) dgvUsuarios.Columns["Setor"].Visible = false;
                if (dgvUsuarios.Columns["SetorId"] != null) dgvUsuarios.Columns["SetorId"].Visible = false;
            }
        }

        private void FiltrarUsuarios()
        {
            var usuariosFiltrados = _todosUsuarios.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(txtBuscar.Text))
            {
                usuariosFiltrados = usuariosFiltrados.Where(u =>
                    u.Nome.Contains(txtBuscar.Text, StringComparison.OrdinalIgnoreCase) ||
                    u.Email.Contains(txtBuscar.Text, StringComparison.OrdinalIgnoreCase));
            }

            if (cmbFiltroPerfil.SelectedIndex > 0)
            {
                var perfil = cmbFiltroPerfil.SelectedItem?.ToString();
                usuariosFiltrados = usuariosFiltrados.Where(u => u.Perfil == perfil);
            }

            dgvUsuarios.DataSource = usuariosFiltrados.ToList();
        }

        private void BtnNovo_Click(object sender, EventArgs e)
        {
            var form = new UsuarioEdicaoForm(_apiService, null);
            if (form.ShowDialog() == DialogResult.OK)
            {
                CarregarUsuarios();
            }
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count > 0)
            {
                var usuario = dgvUsuarios.SelectedRows[0].DataBoundItem as Usuario;
                var form = new UsuarioEdicaoForm(_apiService, usuario);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    CarregarUsuarios();
                }
            }
            else
            {
                MessageBox.Show("Selecione um usuário para editar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void BtnExcluir_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count > 0)
            {
                var usuario = dgvUsuarios.SelectedRows[0].DataBoundItem as Usuario;

                var result = MessageBox.Show($"Deseja realmente excluir o usuário '{usuario?.Nome}'?",
                    "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes && usuario != null)
                {
                    try
                    {
                        var sucesso = await _apiService.DeleteUsuarioAsync(usuario.Id);

                        if (sucesso)
                        {
                            MessageBox.Show("Usuário excluído com sucesso!", "Sucesso",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CarregarUsuarios();
                        }
                        else
                        {
                            MessageBox.Show("Erro ao excluir usuário.", "Erro",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro ao excluir usuário: {ex.Message}", "Erro",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecione um usuário para excluir.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}