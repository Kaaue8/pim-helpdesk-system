using System;
using System.Windows.Forms;

namespace HelpDesk.Desktop
{
    static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // A linha abaixo inicia o FormLogin
            Application.Run(new FormLogin());
        }
    }
}
