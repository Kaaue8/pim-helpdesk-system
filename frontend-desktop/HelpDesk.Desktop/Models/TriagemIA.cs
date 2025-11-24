using System;

namespace HelpDesk.Desktop.Models
{
    public class TriagemIA
    {
        public int ID_Triagem { get; set; }
        public int? ID_Ticket { get; set; }
        public string TituloOriginal { get; set; } = string.Empty;
        public string DescricaoOriginal { get; set; } = string.Empty;
        public string? CategoriaSugerida { get; set; }
        public string? PrioridadeSugerida { get; set; }
        public string? SetorSugerido { get; set; }
        public string? ResumoTriagem { get; set; }
        public string? SolucaoSugerida { get; set; }
        public int? ID_SolucaoComum { get; set; }
        public int NivelConfianca { get; set; } = 0;
        public bool UsuarioAceitou { get; set; } = false;
        public string? FeedbackUsuario { get; set; }
        public DateTime DataTriagem { get; set; } = DateTime.Now;
        public int TempoProcessamento { get; set; } = 0;

        public bool IsAltaConfianca() => NivelConfianca >= 80;
        public bool IsMediaConfianca() => NivelConfianca >= 50 && NivelConfianca < 80;
        public bool IsBaixaConfianca() => NivelConfianca < 50;

        public string GetNivelConfiancaTexto()
        {
            if (IsAltaConfianca()) return "Alta";
            if (IsMediaConfianca()) return "Média";
            return "Baixa";
        }

        public void RegistrarFeedback(bool aceitou, string? feedback = null)
        {
            UsuarioAceitou = aceitou;
            FeedbackUsuario = feedback;
        }
    }
}