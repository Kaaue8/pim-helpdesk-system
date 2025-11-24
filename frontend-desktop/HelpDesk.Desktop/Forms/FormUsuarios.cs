using System;
using System.Collections.Generic;
using System.Windows.Forms;
using HelpDesk.Desktop.Models;
using HelpDesk.Desktop.Services;
using HelpDesk.Desktop.Utils;

namespace HelpDesk.Desktop.Forms
{
    public partial class FormUsuarios : Form
    {
        public FormUsuarios()
        {
            InitializeComponent();
            ConfigurarEstilo();
            CarregarUsuarios();
        }

        private void ConfigurarEstilo()
        {
            this.BackColor = AppStyles.ColorBackground;
            AppStyles.StyleDataGridView(dgvUsuarios);
        }

        private async void CarregarUsuarios()
        {
            try
            {
                btnAtualizar.Enabled = false;
                btnAtualizar.Text = "Carregando...";

                var usuarios = await ApiService.Instance.GetUsuariosAsync();
                dgvUsuarios.DataSource = usuarios;
                ConfigurarColunas();
            }
            catch (Exception ex)
            {
                AppStyles.ShowError($"Erro ao carregar usuários: {ex.Message}");
            }
            finally
            {
                btnAtualizar.Enabled = true;
                btnAtualizar.Text = "🔄 Atualizar";
            }
        }

        private void ConfigurarColunas()
        {
            if (dgvUsuarios.Columns.Count > 0)
            {
                dgvUsuarios.Columns["ID_Usuario"].HeaderText = "ID";
                dgvUsuarios.Columns["ID_Usuario"].Width = 60;
                dgvUsuarios.Columns["Nome"].HeaderText = "Nome";
                dgvUsuarios.Columns["Nome"].Width = 200;
                dgvUsuarios.Columns["Email"].HeaderText = "Email";
                dgvUsuarios.Columns["Email"].Width = 250;
                dgvUsuarios.Columns["Perfil"].HeaderText = "Perfil";
                dgvUsuarios.Columns["Perfil"].Width = 120;
                dgvUsuarios.Columns["DataCriacao"].HeaderText = "Data Criação";
                dgvUsuarios.Columns["DataCriacao"].Width = 150;

                // Ocultar colunas sensíveis
                if (dgvUsuarios.Columns.Contains("SenhaHash"))
                    dgvUsuarios.Columns["SenhaHash"].Visible = false;
                if (dgvUsuarios.Columns.Contains("ID_Setor"))
                    dgvUsuarios.Columns["ID_Setor"].Visible = false;
            }
        }

        private void BtnAtualizar_Click(object sender, EventArgs e)
        {
            CarregarUsuarios();
        }

        private void BtnVoltar_Click(object sender, EventArgs e)
        {
            var formDashboard = new FormDashboard();
            formDashboard.Show();
            this.Close();
        }

        private void FormUsuarios_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Application.Exit();
            }
        }
    }
}