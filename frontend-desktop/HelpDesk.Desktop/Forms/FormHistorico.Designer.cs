namespace HelpDesk.Desktop.Forms
{
    partial class FormHistorico
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Button btnFechar;
        private System.Windows.Forms.Panel panelTimeline;
        private System.Windows.Forms.Label lblCarregando;

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
            this.btnFechar = new System.Windows.Forms.Button();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.panelTimeline = new System.Windows.Forms.Panel();
            this.lblCarregando = new System.Windows.Forms.Label();
            this.panelTop.SuspendLayout();
            this.panelTimeline.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.White;
            this.panelTop.Controls.Add(this.btnFechar);
            this.panelTop.Controls.Add(this.lblTitulo);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Padding = new System.Windows.Forms.Padding(20);
            this.panelTop.Size = new System.Drawing.Size(800, 80);
            this.panelTop.TabIndex = 0;
            // 
            // btnFechar
            // 
            this.btnFechar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFechar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.btnFechar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFechar.FlatAppearance.BorderSize = 0;
            this.btnFechar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFechar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnFechar.ForeColor = System.Drawing.Color.White;
            this.btnFechar.Location = new System.Drawing.Point(680, 20);
            this.btnFechar.Name = "btnFechar";
            this.btnFechar.Size = new System.Drawing.Size(100, 40);
            this.btnFechar.TabIndex = 1;
            this.btnFechar.Text = "✖ Fechar";
            this.btnFechar.UseVisualStyleBackColor = false;
            this.btnFechar.Click += new System.EventHandler(this.BtnFechar_Click);
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.lblTitulo.Location = new System.Drawing.Point(20, 25);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(265, 32);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "📜 Histórico do Ticket";
            // 
            // panelTimeline
            // 
            this.panelTimeline.AutoScroll = true;
            this.panelTimeline.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.panelTimeline.Controls.Add(this.lblCarregando);
            this.panelTimeline.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTimeline.Location = new System.Drawing.Point(0, 80);
            this.panelTimeline.Name = "panelTimeline";
            this.panelTimeline.Padding = new System.Windows.Forms.Padding(20);
            this.panelTimeline.Size = new System.Drawing.Size(800, 520);
            this.panelTimeline.TabIndex = 1;
            // 
            // lblCarregando
            // 
            this.lblCarregando.AutoSize = true;
            this.lblCarregando.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblCarregando.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lblCarregando.Location = new System.Drawing.Point(320, 200);
            this.lblCarregando.Name = "lblCarregando";
            this.lblCarregando.Size = new System.Drawing.Size(160, 21);
            this.lblCarregando.TabIndex = 0;
            this.lblCarregando.Text = "⏳ Carregando...";
            // 
            // FormHistorico
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.panelTimeline);
            this.Controls.Add(this.panelTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormHistorico";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "HelpDesk - Histórico do Ticket";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelTimeline.ResumeLayout(false);
            this.panelTimeline.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}