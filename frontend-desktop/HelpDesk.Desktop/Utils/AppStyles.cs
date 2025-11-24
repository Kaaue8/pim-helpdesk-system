using System;
using System.Drawing;
using System.Windows.Forms;

namespace HelpDesk.Desktop.Utils
{
    /// <summary>
    /// Design System centralizado com cores, fontes e componentes padronizados
    /// </summary>
    public static class AppStyles
    {
        #region Cores

        // Cores Primárias
        public static Color ColorPrimary { get; } = ColorTranslator.FromHtml("#7C3AED");
        public static Color ColorPrimaryDark { get; } = ColorTranslator.FromHtml("#6D28D9");
        public static Color ColorPrimaryLight { get; } = ColorTranslator.FromHtml("#A78BFA");

        // Cores de Feedback
        public static Color ColorSuccess { get; } = ColorTranslator.FromHtml("#10B981");
        public static Color ColorError { get; } = ColorTranslator.FromHtml("#EF4444");
        public static Color ColorWarning { get; } = ColorTranslator.FromHtml("#F59E0B");
        public static Color ColorInfo { get; } = ColorTranslator.FromHtml("#3B82F6");

        // Cores de Background
        public static Color ColorBackground { get; } = ColorTranslator.FromHtml("#F9FAFB");
        public static Color ColorBackgroundLight { get; } = Color.White;
        public static Color ColorBackgroundDark { get; } = ColorTranslator.FromHtml("#1F2937");

        // Cores de Texto
        public static Color ColorText { get; } = ColorTranslator.FromHtml("#1F2937");
        public static Color ColorTextSecondary { get; } = ColorTranslator.FromHtml("#6B7280");
        public static Color ColorTextLight { get; } = Color.White;
        public static Color ColorTextPrimary { get; } = ColorTranslator.FromHtml("#1F2937");

        // Cores de Borda
        public static Color ColorBorder { get; } = ColorTranslator.FromHtml("#E5E7EB");
        public static Color ColorBorderDark { get; } = ColorTranslator.FromHtml("#D1D5DB");

        #endregion

        #region Fontes

        public static Font FontTitle { get; } = new Font("Segoe UI", 18F, FontStyle.Bold);
        public static Font FontSubtitle { get; } = new Font("Segoe UI", 14F, FontStyle.Bold);
        public static Font FontBody { get; } = new Font("Segoe UI", 10F, FontStyle.Regular);
        public static Font FontBodyBold { get; } = new Font("Segoe UI", 10F, FontStyle.Bold);
        public static Font FontSmall { get; } = new Font("Segoe UI", 9F, FontStyle.Regular);
        public static Font FontButton { get; } = new Font("Segoe UI", 10F, FontStyle.Bold);
        public static Font FontLabel { get; } = new Font("Segoe UI", 9F, FontStyle.Regular);

        #endregion

        #region Botões

        public static Button CreateButton(string text, EventHandler? onClick = null)
        {
            var button = new Button
            {
                Text = text,
                Font = FontButton,
                BackColor = ColorPrimary,
                ForeColor = ColorTextLight,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Height = 40,
                Width = 120
            };

            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = ColorPrimaryDark;

            if (onClick != null)
                button.Click += onClick;

            return button;
        }

        public static Button CreatePrimaryButton(string text, EventHandler? onClick = null)
        {
            return CreateButton(text, onClick);
        }

        public static Button CreateSecondaryButton(string text, EventHandler? onClick = null)
        {
            var button = CreateButton(text, onClick);
            button.BackColor = ColorBackgroundLight;
            button.ForeColor = ColorPrimary;
            button.FlatAppearance.BorderColor = ColorPrimary;
            button.FlatAppearance.BorderSize = 2;
            button.FlatAppearance.MouseOverBackColor = ColorPrimaryLight;
            return button;
        }

        public static Button CreateSuccessButton(string text, EventHandler? onClick = null)
        {
            var button = CreateButton(text, onClick);
            button.BackColor = ColorSuccess;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(14, 159, 110);
            return button;
        }

        public static Button CreateDangerButton(string text, EventHandler? onClick = null)
        {
            var button = CreateButton(text, onClick);
            button.BackColor = ColorError;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 38, 38);
            return button;
        }

        #endregion

        #region TextBox

        public static TextBox CreateTextBox(string placeholder = "")
        {
            var textBox = new TextBox
            {
                Font = FontBody,
                ForeColor = ColorText,
                BorderStyle = BorderStyle.FixedSingle,
                Height = 30
            };

            if (!string.IsNullOrEmpty(placeholder))
            {
                textBox.Text = placeholder;
                textBox.ForeColor = ColorTextSecondary;

                textBox.Enter += (s, e) =>
                {
                    if (textBox.Text == placeholder)
                    {
                        textBox.Text = "";
                        textBox.ForeColor = ColorText;
                    }
                };

                textBox.Leave += (s, e) =>
                {
                    if (string.IsNullOrWhiteSpace(textBox.Text))
                    {
                        textBox.Text = placeholder;
                        textBox.ForeColor = ColorTextSecondary;
                    }
                };
            }

            return textBox;
        }

        #endregion

        #region ComboBox

        public static ComboBox CreateComboBox()
        {
            return new ComboBox
            {
                Font = FontBody,
                ForeColor = ColorText,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Height = 30
            };
        }

        #endregion

        #region Labels

        public static Label CreateLabel(string text, Font? font = null, Color? color = null)
        {
            return new Label
            {
                Text = text,
                Font = font ?? FontBody,
                ForeColor = color ?? ColorText,
                AutoSize = true
            };
        }

        public static Label CreateTitleLabel(string text)
        {
            return CreateLabel(text, FontTitle, ColorText);
        }

        public static Label CreateSubtitleLabel(string text)
        {
            return CreateLabel(text, FontSubtitle, ColorText);
        }

        #endregion

        #region DataGridView

        public static void StyleDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor = ColorBackgroundLight;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.EnableHeadersVisualStyles = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.ReadOnly = true;
            dgv.RowHeadersVisible = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Estilo do cabeçalho
            dgv.ColumnHeadersDefaultCellStyle.BackColor = ColorPrimary;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = ColorTextLight;
            dgv.ColumnHeadersDefaultCellStyle.Font = FontBodyBold;
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(10);
            dgv.ColumnHeadersHeight = 40;

            // Estilo das células
            dgv.DefaultCellStyle.BackColor = ColorBackgroundLight;
            dgv.DefaultCellStyle.ForeColor = ColorText;
            dgv.DefaultCellStyle.Font = FontBody;
            dgv.DefaultCellStyle.SelectionBackColor = ColorPrimaryLight;
            dgv.DefaultCellStyle.SelectionForeColor = ColorText;
            dgv.DefaultCellStyle.Padding = new Padding(10, 5, 10, 5);

            // Estilo das linhas alternadas
            dgv.AlternatingRowsDefaultCellStyle.BackColor = ColorBackground;

            dgv.RowTemplate.Height = 40;
            dgv.GridColor = ColorBorder;
        }

        #endregion

        #region Panels e Cards

        public static Panel CreateCard(int padding = 20)
        {
            return new Panel
            {
                BackColor = ColorBackgroundLight,
                Padding = new Padding(padding)
            };
        }

        public static Panel CreateSidebar()
        {
            return new Panel
            {
                BackColor = ColorPrimary,
                Dock = DockStyle.Left,
                Width = 250
            };
        }

        #endregion

        #region Mensagens

        public static void ShowSuccess(string message, string title = "Sucesso")
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void ShowError(string message, string title = "Erro")
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ShowWarning(string message, string title = "Atenção")
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static DialogResult ShowConfirmation(string message, string title = "Confirmação")
        {
            return MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        public static void ShowInfo(string message, string title = "Informação")
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion
    }
}

