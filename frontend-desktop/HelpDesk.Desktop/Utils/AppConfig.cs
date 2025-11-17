namespace HelpDesk.Desktop.Utils
{
    /// <summary>
    /// Configurações da aplicação Desktop HelpDesk
    /// </summary>
    public static class AppConfig
    {
        /// <summary>
        /// URL base da API HelpDesk hospedada no Render
        /// </summary>
        public static string ApiBaseUrl => "https://pim-helpdesk-system.onrender.com/api";

        /// <summary>
        /// Timeout para requisições HTTP (em segundos)
        /// </summary>
        public static int ApiTimeout => 30;

        /// <summary>
        /// Nome da aplicação
        /// </summary>
        public static string AppName => "HelpCenter Apollo";

        /// <summary>
        /// Versão da aplicação
        /// </summary>
        public static string AppVersion => "1.0.0";
    }
}

