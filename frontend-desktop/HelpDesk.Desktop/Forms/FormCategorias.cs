using System;
using System.Collections.Generic;
using System.Windows.Forms;
using HelpDesk.Desktop.Models;
using HelpDesk.Desktop.Services;
using HelpDesk.Desktop.Utils;

namespace HelpDesk.Desktop.Forms
{
    public partial class FormCategorias : Form
    {
        public FormCategorias()
        {
            InitializeComponent();
            ConfigurarEstilo();
            CarregarCategorias();
        }

        private void ConfigurarEstilo()
        {
            this.BackColor = AppStyles.ColorBackground;
            AppStyles.StyleDataGridView(dgvCategorias);
        }

        private async void CarregarCategorias()
        {
            try
            {
                btnAtualizar.Enabled = false;
                btnAtualizar.Text = "Carregando...";

                var categorias = await ApiService.Instance.GetCategoriasAsync();
                dgvCategorias.DataSource = categorias;
                ConfigurarColunas();
            }
            catch (Exception ex)
            {
                AppStyles.ShowError($"Erro ao carregar categorias: {ex.Message}");
            }
            finally
            {
                btnAtualizar.Enabled = true;
                btnAtualizar.Text = "🔄 Atualizar";
            }
        }

        private void ConfigurarColunas()
        {
            if (dgvCategorias.Columns.Count > 0)
            {
                dgvCategorias.Columns["ID_Categoria"].HeaderText = "ID";
                dgvCategorias.Columns["ID_Categoria"].Width = 80;
                dgvCategorias.Columns["ID_Categoria"].ReadOnly = true;
                dgvCategorias.Columns["NomeCategoria"].HeaderText = "Nome da Categoria";
                dgvCategorias.Columns["NomeCategoria"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private async void BtnAdicionar_Click(object sender, EventArgs e)
        {
            try
            {
                string nomeCategoria = txtNome.Text.Trim();

                if (string.IsNullOrWhiteSpace(nomeCategoria))
                {
                    AppStyles.ShowWarning("Por favor, informe o nome da categoria.");
                    txtNome.Focus();
                    return;
                }

                btnAdicionar.Enabled = false;
                btnAdicionar.Text = "Adicionando...";

                var categoria = new Categoria
                {
                    NomeCategoria = nomeCategoria
                };

                await ApiService.Instance.CreateCategoriaAsync(categoria);
                AppStyles.ShowSuccess("Categoria adicionada com sucesso!");

                txtNome.Clear();
                txtNome.Focus();
                CarregarCategorias();
            }
            catch (Exception ex)
            {
                AppStyles.ShowError($"Erro ao adicionar categoria: {ex.Message}");
            }
            finally
            {
                btnAdicionar.Enabled = true;
                btnAdicionar.Text = "➕ Adicionar";
            }
        }

        private void TxtNome_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                BtnAdicionar_Click(sender, e);
            }
        }

        private void BtnAtualizar_Click(object sender, EventArgs e)
        {
            CarregarCategorias();
        }

        private void BtnVoltar_Click(object sender, EventArgs e)
        {
            var formDashboard = new FormDashboard();
            formDashboard.Show();
            this.Close();
        }

        private async void DgvCategorias_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == dgvCategorias.Columns["NomeCategoria"].Index)
                {
                    var categoria = (Categoria)dgvCategorias.Rows[e.RowIndex].DataBoundItem;
                    await ApiService.Instance.UpdateCategoriaAsync(categoria.ID_Categoria, categoria);
                    AppStyles.ShowSuccess("Categoria atualizada com sucesso!");
                }
            }
            catch (Exception ex)
            {
                AppStyles.ShowError($"Erro ao atualizar categoria: {ex.Message}");
                CarregarCategorias();
            }
        }

        private void FormCategorias_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Application.Exit();
            }
        }
    }
}