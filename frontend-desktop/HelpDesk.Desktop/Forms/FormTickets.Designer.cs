namespace HelpDesk.Desktop.Forms
{
    partial class FormTickets
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Button btnVoltar;
        private System.Windows.Forms.Button btnNovoTicket;
        private System.Windows.Forms.Button btnAtualizar;
        private System.Windows.Forms.Button btnHistorico;
        private System.Windows.Forms.Panel panelFiltros;
        private System.Windows.Forms.Label lblFiltroStatus;
        private System.Windows.Forms.ComboBox cmbFiltroStatus;
        private System.Windows.Forms.Label lblBusca;
        private System.Windows.Forms.TextBox txtBusca;
        private System.Windows.Forms.DataGridView dgvTickets;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelTop = new System.Windows.Forms.Panel();
            this.btnHistorico = new System.Windows.Forms.Button();
            this.btnAtualizar = new System.Windows.Forms.Button();
            this.btnNovoTicket = new System.Windows.Forms.Button();
            this.btnVoltar = new System.Windows.Forms.Button();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.panelFiltros = new System.Windows.Forms.Panel();
            this.txtBusca = new System.Windows.Forms.TextBox();
            this.lblBusca = new System.Windows.Forms.Label();
            this.cmbFiltroStatus = new System.Windows.Forms.ComboBox();
            this.lblFiltroStatus = new System.Windows.Forms.Label();
            this.dgvTickets = new System.Windows.Forms.DataGridView();
            this.panelTop.SuspendLayout();
            this.panelFiltros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTickets)).BeginInit();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.White;
            this.panelTop.Controls.Add(this.btnHistorico);
            this.panelTop.Controls.Add(this.btnAtualizar);
            this.panelTop.Controls.Add(this.btnNovoTicket);
            this.panelTop.Controls.Add(this.btnVoltar);
            this.panelTop.Controls.Add(this.lblTitulo);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Padding = new System.Windows.Forms.Padding(20);
            this.panelTop.Size = new System.Drawing.Size(1200, 80);
            this.panelTop.TabIndex = 0;
            // 
            // btnHistorico
            // 
            this.btnHistorico.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHistorico.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(130)))), ((int)(((byte)(246)))));
            this.btnHistorico.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHistorico.FlatAppearance.BorderSize = 0;
            this.btnHistorico.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHistorico.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnHistorico.ForeColor = System.Drawing.Color.White;
            this.btnHistorico.Location = new System.Drawing.Point(870, 20);
            this.btnHistorico.Name = "btnHistorico";
            this.btnHistorico.Size = new System.Drawing.Size(120, 40);
            this.btnHistorico.TabIndex = 4;
            this.btnHistorico.Text = "📜 Histórico";
            this.btnHistorico.UseVisualStyleBackColor = false;
            this.btnHistorico.Click += new System.EventHandler(this.BtnHistorico_Click);
            // 
            // btnAtualizar
            // 
            this.btnAtualizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAtualizar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.btnAtualizar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAtualizar.FlatAppearance.BorderSize = 0;
            this.btnAtualizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAtualizar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAtualizar.ForeColor = System.Drawing.Color.White;
            this.btnAtualizar.Location = new System.Drawing.Point(1000, 20);
            this.btnAtualizar.Name = "btnAtualizar";
            this.btnAtualizar.Size = new System.Drawing.Size(120, 40);
            this.btnAtualizar.TabIndex = 3;
            this.btnAtualizar.Text = "🔄 Atualizar";
            this.btnAtualizar.UseVisualStyleBackColor = false;
            this.btnAtualizar.Click += new System.EventHandler(this.BtnAtualizar_Click);
            // 
            // btnNovoTicket
            // 
            this.btnNovoTicket.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNovoTicket.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(58)))), ((int)(((byte)(237)))));
            this.btnNovoTicket.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNovoTicket.FlatAppearance.BorderSize = 0;
            this.btnNovoTicket.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNovoTicket.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnNovoTicket.ForeColor = System.Drawing.Color.White;
            this.btnNovoTicket.Location = new System.Drawing.Point(730, 20);
            this.btnNovoTicket.Name = "btnNovoTicket";
            this.btnNovoTicket.Size = new System.Drawing.Size(130, 40);
            this.btnNovoTicket.TabIndex = 2;
            this.btnNovoTicket.Text = "➕ Novo Ticket";
            this.btnNovoTicket.UseVisualStyleBackColor = false;
            this.btnNovoTicket.Click += new System.EventHandler(this.BtnNovoTicket_Click);
            // 
            // btnVoltar
            // 
            this.btnVoltar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.btnVoltar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVoltar.FlatAppearance.BorderSize = 0;
            this.btnVoltar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVoltar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnVoltar.ForeColor = System.Drawing.Color.White;
            this.btnVoltar.Location = new System.Drawing.Point(20, 20);
            this.btnVoltar.Name = "btnVoltar";
            this.btnVoltar.Size = new System.Drawing.Size(100, 40);
            this.btnVoltar.TabIndex = 1;
            this.btnVoltar.Text = "← Voltar";
            this.btnVoltar.UseVisualStyleBackColor = false;
            this.btnVoltar.Click += new System.EventHandler(this.BtnVoltar_Click);
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblTitulo.Location = new System.Drawing.Point(130, 25);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(183, 32);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "🎫 Meus Tickets";
            // 
            // panelFiltros
            // 
            this.panelFiltros.BackColor = System.Drawing.Color.White;
            this.panelFiltros.Controls.Add(this.txtBusca);
            this.panelFiltros.Controls.Add(this.lblBusca);
            this.panelFiltros.Controls.Add(this.cmbFiltroStatus);
            this.panelFiltros.Controls.Add(this.lblFiltroStatus);
            this.panelFiltros.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFiltros.Location = new System.Drawing.Point(0, 80);
            this.panelFiltros.Name = "panelFiltros";
            this.panelFiltros.Padding = new System.Windows.Forms.Padding(20);
            this.panelFiltros.Size = new System.Drawing.Size(1200, 70);
            this.panelFiltros.TabIndex = 1;
            // 
            // txtBusca
            // 
            this.txtBusca.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBusca.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtBusca.Location = new System.Drawing.Point(350, 25);
            this.txtBusca.Name = "txtBusca";
            this.txtBusca.Size = new System.Drawing.Size(300, 25);
            this.txtBusca.TabIndex = 3;
            this.txtBusca.TextChanged += new System.EventHandler(this.TxtBusca_TextChanged);
            // 
            // lblBusca
            // 
            this.lblBusca.AutoSize = true;
            this.lblBusca.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblBusca.Location = new System.Drawing.Point(350, 5);
            this.lblBusca.Name = "lblBusca";
            this.lblBusca.Size = new System.Drawing.Size(42, 15);
            this.lblBusca.TabIndex = 2;
            this.lblBusca.Text = "Buscar";
            // 
            // cmbFiltroStatus
            // 
            this.cmbFiltroStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFiltroStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbFiltroStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbFiltroStatus.FormattingEnabled = true;
            this.cmbFiltroStatus.Items.AddRange(new object[] {
            "Todos",
            "Aberto",
            "Em Andamento",
            "Fechado"});
            this.cmbFiltroStatus.Location = new System.Drawing.Point(20, 25);
            this.cmbFiltroStatus.Name = "cmbFiltroStatus";
            this.cmbFiltroStatus.Size = new System.Drawing.Size(200, 25);
            this.cmbFiltroStatus.TabIndex = 1;
            this.cmbFiltroStatus.SelectedIndexChanged += new System.EventHandler(this.CmbFiltroStatus_SelectedIndexChanged);
            // 
            // lblFiltroStatus
            // 
            this.lblFiltroStatus.AutoSize = true;
            this.lblFiltroStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblFiltroStatus.Location = new System.Drawing.Point(20, 5);
            this.lblFiltroStatus.Name = "lblFiltroStatus";
            this.lblFiltroStatus.Size = new System.Drawing.Size(39, 15);
            this.lblFiltroStatus.TabIndex = 0;
            this.lblFiltroStatus.Text = "Status";
            // 
            // dgvTickets
            // 
            this.dgvTickets.AllowUserToAddRows = false;
            this.dgvTickets.AllowUserToDeleteRows = false;
            this.dgvTickets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTickets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTickets.Location = new System.Drawing.Point(0, 150);
            this.dgvTickets.Name = "dgvTickets";
            this.dgvTickets.ReadOnly = true;
            this.dgvTickets.RowTemplate.Height = 25;
            this.dgvTickets.Size = new System.Drawing.Size(1200, 550);
            this.dgvTickets.TabIndex = 2;
            this.dgvTickets.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvTickets_CellDoubleClick);
            // 
            // FormTickets
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Controls.Add(this.dgvTickets);
            this.Controls.Add(this.panelFiltros);
            this.Controls.Add(this.panelTop);
            this.MinimumSize = new System.Drawing.Size(1200, 700);
            this.Name = "FormTickets";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HelpDesk - Tickets";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormTickets_FormClosing);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelFiltros.ResumeLayout(false);
            this.panelFiltros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTickets)).EndInit();
            this.ResumeLayout(false);
        }
    }
}