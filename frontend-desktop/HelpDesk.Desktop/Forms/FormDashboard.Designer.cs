namespace HelpDesk.Desktop.Forms
{
    partial class FormDashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblUsuario = new System.Windows.Forms.Label();
            this.lblPerfil = new System.Windows.Forms.Label();
            this.btnTickets = new System.Windows.Forms.Button();
            this.btnNovoTicket = new System.Windows.Forms.Button();
            this.btnUsuarios = new System.Windows.Forms.Button();
            this.btnCategorias = new System.Windows.Forms.Button();
            this.btnSair = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblUsuario
            // 
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblUsuario.Location = new System.Drawing.Point(20, 20);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(73, 21);
            this.lblUsuario.TabIndex = 0;
            this.lblUsuario.Text = "Usuário:";
            // 
            // lblPerfil
            // 
            this.lblPerfil.AutoSize = true;
            this.lblPerfil.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblPerfil.Location = new System.Drawing.Point(20, 50);
            this.lblPerfil.Name = "lblPerfil";
            this.lblPerfil.Size = new System.Drawing.Size(42, 19);
            this.lblPerfil.TabIndex = 1;
            this.lblPerfil.Text = "Perfil:";
            // 
            // btnTickets
            // 
            this.btnTickets.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnTickets.Location = new System.Drawing.Point(50, 100);
            this.btnTickets.Name = "btnTickets";
            this.btnTickets.Size = new System.Drawing.Size(200, 50);
            this.btnTickets.TabIndex = 2;
            this.btnTickets.Text = "📋 Meus Tickets";
            this.btnTickets.UseVisualStyleBackColor = true;
            this.btnTickets.Click += new System.EventHandler(this.BtnTickets_Click);
            // 
            // btnNovoTicket
            // 
            this.btnNovoTicket.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnNovoTicket.Location = new System.Drawing.Point(50, 170);
            this.btnNovoTicket.Name = "btnNovoTicket";
            this.btnNovoTicket.Size = new System.Drawing.Size(200, 50);
            this.btnNovoTicket.TabIndex = 3;
            this.btnNovoTicket.Text = "➕ Novo Ticket";
            this.btnNovoTicket.UseVisualStyleBackColor = true;
            this.btnNovoTicket.Click += new System.EventHandler(this.BtnNovoTicket_Click);
            // 
            // btnUsuarios
            // 
            this.btnUsuarios.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnUsuarios.Location = new System.Drawing.Point(50, 240);
            this.btnUsuarios.Name = "btnUsuarios";
            this.btnUsuarios.Size = new System.Drawing.Size(200, 50);
            this.btnUsuarios.TabIndex = 4;
            this.btnUsuarios.Text = "👥 Usuários";
            this.btnUsuarios.UseVisualStyleBackColor = true;
            this.btnUsuarios.Click += new System.EventHandler(this.BtnUsuarios_Click);
            // 
            // btnCategorias
            // 
            this.btnCategorias.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnCategorias.Location = new System.Drawing.Point(50, 310);
            this.btnCategorias.Name = "btnCategorias";
            this.btnCategorias.Size = new System.Drawing.Size(200, 50);
            this.btnCategorias.TabIndex = 5;
            this.btnCategorias.Text = "📂 Categorias";
            this.btnCategorias.UseVisualStyleBackColor = true;
            this.btnCategorias.Click += new System.EventHandler(this.BtnCategorias_Click);
            // 
            // btnSair
            // 
            this.btnSair.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnSair.Location = new System.Drawing.Point(50, 380);
            this.btnSair.Name = "btnSair";
            this.btnSair.Size = new System.Drawing.Size(200, 50);
            this.btnSair.TabIndex = 6;
            this.btnSair.Text = "🚪 Sair";
            this.btnSair.UseVisualStyleBackColor = true;
            this.btnSair.Click += new System.EventHandler(this.BtnSair_Click);
            // 
            // FormDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Controls.Add(this.btnSair);
            this.Controls.Add(this.btnCategorias);
            this.Controls.Add(this.btnUsuarios);
            this.Controls.Add(this.btnNovoTicket);
            this.Controls.Add(this.btnTickets);
            this.Controls.Add(this.lblPerfil);
            this.Controls.Add(this.lblUsuario);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dashboard - HelpDesk";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormDashboard_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.Label lblPerfil;
        private System.Windows.Forms.Button btnTickets;
        private System.Windows.Forms.Button btnNovoTicket;
        private System.Windows.Forms.Button btnUsuarios;
        private System.Windows.Forms.Button btnCategorias;
        private System.Windows.Forms.Button btnSair;
    }
}
