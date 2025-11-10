import { useState } from "react";
import { RefreshCw, Paperclip } from "lucide-react";
import { Card } from "@/components/ui/card";
import { Button } from "@/components/ui/button";

interface CriarChamadoModalProps {
  isOpen: boolean;
  onClose: () => void;
  tipo?: string;
}

// Dados de categorias e sugestões de solução
const CATEGORIAS_DATA = [
  {
    id: 1,
    nome: "Sem conexão com a internet",
    palavrasChave: ["internet", "conexão", "wifi", "rede", "online"],
    solucao: "1. Verifique se o cabo de rede está conectado\n2. Reinicie o modem\n3. Reinicie seu computador\n4. Contate o suporte se o problema persistir",
    prioridade: "3 - Alta",
    sla: "4h",
  },
  {
    id: 2,
    nome: "Problema com email",
    palavrasChave: ["email", "outlook", "gmail", "mail", "mensagem"],
    solucao: "1. Verifique sua conexão com a internet\n2. Limpe o cache do navegador\n3. Tente acessar via webmail\n4. Verifique sua senha",
    prioridade: "2 - Média",
    sla: "8h",
  },
  {
    id: 3,
    nome: "Criação/alteração de acessos",
    palavrasChave: ["acesso", "permissão", "usuário", "senha", "login"],
    solucao: "Sua solicitação será processada em até 24 horas. Você receberá um email com as novas credenciais.",
    prioridade: "2 - Média",
    sla: "24h",
  },
  {
    id: 4,
    nome: "Solicitação de equipamento",
    palavrasChave: ["equipamento", "mouse", "teclado", "monitor", "notebook"],
    solucao: "Sua solicitação será analisada pelo departamento de TI. Você receberá uma resposta em até 48 horas.",
    prioridade: "1 - Baixa",
    sla: "48h",
  },
  {
    id: 5,
    nome: "Dúvida sobre sistema",
    palavrasChave: ["dúvida", "como", "sistema", "software", "aplicação"],
    solucao: "Consulte a documentação disponível no portal. Se a dúvida persistir, abra um chamado para suporte.",
    prioridade: "1 - Baixa",
    sla: "24h",
  },
];

export default function CriarChamadoModal({
  isOpen,
  onClose,
  tipo,
}: CriarChamadoModalProps) {
  const [step, setStep] = useState<"descricao" | "detalhes" | "sucesso" | "registrado">(
    "descricao"
  );
  const [descricao, setDescricao] = useState("");
  const [categoriaIdentificada, setCategoriaIdentificada] = useState<
    (typeof CATEGORIAS_DATA)[0] | null
  >(null);
  const [categoriaSelecionada, setCategoriaSelecionada] = useState<
    (typeof CATEGORIAS_DATA)[0] | null
  >(null);
  const [detalhesAdicionais, setDetalhesAdicionais] = useState("");
  const [arquivo, setArquivo] = useState<string | null>(null);
  const [iaCarregando, setIaCarregando] = useState(false);

  // Simular identificação com IA
  const identificarComIA = () => {
    if (!descricao.trim()) {
      alert("Por favor, descreva o problema");
      return;
    }

    setIaCarregando(true);

    // Simular delay de processamento
    setTimeout(() => {
      const descricaoLower = descricao.toLowerCase();
      let melhorMatch = null;
      let maiorScore = 0;

      // Buscar melhor correspondência
      CATEGORIAS_DATA.forEach((categoria) => {
        const score = categoria.palavrasChave.filter((palavra) =>
          descricaoLower.includes(palavra)
        ).length;

        if (score > maiorScore) {
          maiorScore = score;
          melhorMatch = categoria;
        }
      });

      // Se não encontrou match, usar a primeira categoria como padrão
      const categoriaEncontrada = melhorMatch || CATEGORIAS_DATA[0];
      setCategoriaIdentificada(categoriaEncontrada);
      setCategoriaSelecionada(categoriaEncontrada);
      setIaCarregando(false);
    }, 1500);
  };

  const recarregarIA = () => {
    setCategoriaIdentificada(null);
    setCategoriaSelecionada(null);
    identificarComIA();
  };

  const handleProblemaResolvido = () => {
    // TODO: Enviar feedback para IA
    console.log("Problema resolvido com IA:", {
      descricao,
      categoria: categoriaSelecionada?.nome,
    });
    setStep("sucesso");
  };

  const handleAbrirChamado = () => {
    // TODO: Enviar dados para backend
    console.log("Abrindo chamado:", {
      descricao,
      categoria: categoriaSelecionada?.nome,
      detalhesAdicionais,
      arquivo,
    });
    setStep("registrado");
  };

  const handleFechar = () => {
    setStep("descricao");
    setDescricao("");
    setCategoriaIdentificada(null);
    setCategoriaSelecionada(null);
    setDetalhesAdicionais("");
    setArquivo(null);
    onClose();
  };

  if (!isOpen) return null;

  // STEP 1: Descrição e Identificação com IA
  if (step === "descricao") {
    return (
      <div className="fixed inset-0 bg-black/50 flex items-center justify-center p-4 z-50">
        <Card className="bg-white rounded-2xl shadow-2xl max-w-2xl w-full max-h-[90vh] overflow-y-auto">
          <div className="p-8">
            {/* Header - Centralizado */}
            <div className="mb-8 text-center">
              <h2 className="text-2xl font-bold text-gray-900">
                Abertura de chamado
              </h2>
              <p className="text-sm text-purple-600 mt-2">
                Algo não está funcionando? Vamos ver como podemos ajudar!
              </p>
            </div>

            {/* Descrição */}
            <div className="mb-4">
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Descrição do problema *
              </label>
              <textarea
                value={descricao}
                onChange={(e) => setDescricao(e.target.value)}
                placeholder="Descreva o problema que está enfrentando..."
                rows={5}
                className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-transparent outline-none transition resize-none"
              />
            </div>

            {/* Botão Identificar com IA - Logo abaixo da descrição */}
            {!categoriaIdentificada && (
              <Button
                onClick={identificarComIA}
                disabled={iaCarregando || !descricao.trim()}
                className="w-full mb-6 bg-purple-600 hover:bg-purple-700 text-white font-semibold py-2 rounded-lg transition disabled:opacity-50"
              >
                {iaCarregando ? "Identificando..." : "Identificar com IA"}
              </Button>
            )}

            {/* Info IA */}
            {categoriaIdentificada && (
              <div className="mb-6 p-3 bg-yellow-50 border border-yellow-200 rounded-lg text-xs text-yellow-800">
                ✨ O campo de categoria foi preenchido automaticamente usando
                inteligência artificial, mas pode ser editado
              </div>
            )}

            {/* Categoria e Solução - Mesmo retângulo */}
            {categoriaIdentificada && (
              <Card className="mb-6 p-6 bg-gray-50 border border-gray-200">
                {/* Categoria */}
                <div className="mb-6">
                  <div className="flex items-center gap-3 mb-2">
                    <label className="block text-sm font-medium text-purple-900">
                      Categoria:
                    </label>
                    <button
                      onClick={recarregarIA}
                      disabled={iaCarregando}
                      className="text-purple-600 hover:text-purple-700 disabled:opacity-50"
                      title="Recarregar categoria"
                    >
                      <RefreshCw size={16} />
                    </button>
                  </div>
                  <select
                    value={categoriaSelecionada?.id || ""}
                    onChange={(e) => {
                      const categoria = CATEGORIAS_DATA.find(
                        (c) => c.id === parseInt(e.target.value)
                      );
                      setCategoriaSelecionada(categoria || null);
                    }}
                    className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-transparent outline-none transition"
                  >
                    {CATEGORIAS_DATA.map((cat) => (
                      <option key={cat.id} value={cat.id}>
                        {cat.nome}
                      </option>
                    ))}
                  </select>
                </div>

                {/* Info Box */}
                {categoriaSelecionada && (
                  <div className="mb-6 p-4 bg-white rounded-lg space-y-2 text-sm border border-gray-200">
                    <div className="flex justify-between">
                      <span className="font-medium text-gray-900">
                        ID Chamado: CH-0004
                      </span>
                      <span className="font-medium text-gray-900">
                        Prioridade: {categoriaSelecionada.prioridade}
                      </span>
                    </div>
                    <div className="flex justify-between">
                      <span className="font-medium text-gray-900">
                        Nível de Suporte: N1
                      </span>
                      <span className="font-medium text-gray-900">
                        SLA: {categoriaSelecionada.sla}
                      </span>
                    </div>
                  </div>
                )}

                {/* Sugestão de Solução */}
                {categoriaSelecionada && (
                  <div className="p-4 bg-white rounded-lg border border-gray-200">
                    <h3 className="font-semibold text-purple-900 mb-2">
                      Sugestão de Solução:
                    </h3>
                    <p className="text-sm text-gray-700 whitespace-pre-line">
                      {categoriaSelecionada.solucao}
                    </p>
                  </div>
                )}
              </Card>
            )}

            {/* Buttons - Abaixo do retângulo de categoria e solução */}
            {categoriaSelecionada && (
              <div className="flex gap-4 pt-4 border-t border-gray-200">
                <Button
                  onClick={handleProblemaResolvido}
                  className="flex-1 bg-purple-600 hover:bg-purple-700 text-white font-semibold py-2 rounded-lg transition"
                >
                  Problema Resolvido com IA
                </Button>
                <Button
                  onClick={() => setStep("detalhes")}
                  className="flex-1 bg-gray-600 hover:bg-gray-700 text-white font-semibold py-2 rounded-lg transition"
                >
                  Seguir com Chamado
                </Button>
              </div>
            )}
          </div>
        </Card>
      </div>
    );
  }

  // STEP 2: Detalhes Adicionais
  if (step === "detalhes") {
    return (
      <div className="fixed inset-0 bg-black/50 flex items-center justify-center p-4 z-50">
        <Card className="bg-white rounded-2xl shadow-2xl max-w-2xl w-full max-h-[90vh] overflow-y-auto">
          <div className="p-8">
            {/* Header */}
            <div className="flex justify-between items-center mb-6">
              <div>
                <h2 className="text-2xl font-bold text-gray-900">
                  Detalhes do Chamado
                </h2>
                <p className="text-sm text-purple-600 mt-1">
                  Categoria: {categoriaSelecionada?.nome}
                </p>
              </div>
              <button
                onClick={handleFechar}
                className="text-gray-500 hover:text-gray-700 text-2xl"
              >
                ✕
              </button>
            </div>

            {/* Descrição (read-only) */}
            <div className="mb-6">
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Descrição do Problema
              </label>
              <div className="w-full px-4 py-3 border border-gray-300 rounded-lg bg-gray-50 text-gray-700 text-sm">
                {descricao}
              </div>
            </div>

            {/* Detalhes Adicionais */}
            <div className="mb-6">
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Detalhes Adicionais
              </label>
              <textarea
                value={detalhesAdicionais}
                onChange={(e) => setDetalhesAdicionais(e.target.value)}
                placeholder="Ex: nome do equipamento, localização da máquina, mensagens de erro..."
                rows={4}
                className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-transparent outline-none transition resize-none"
              />
            </div>

            {/* Anexo */}
            <div className="mb-6">
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Anexar Arquivo (Opcional)
              </label>
              <div className="flex items-center gap-3">
                <input
                  type="file"
                  id="file-input"
                  className="hidden"
                  onChange={(e) => {
                    const file = e.target.files?.[0];
                    if (file) {
                      setArquivo(file.name);
                    }
                  }}
                />
                <label
                  htmlFor="file-input"
                  className="flex items-center gap-2 px-4 py-2 bg-purple-100 text-purple-700 rounded-lg cursor-pointer hover:bg-purple-200 transition"
                >
                  <Paperclip size={16} />
                  Escolher arquivo
                </label>
                {arquivo && (
                  <span className="text-sm text-green-600">✓ {arquivo}</span>
                )}
              </div>
            </div>

            {/* Info Box */}
            {categoriaSelecionada && (
              <div className="mb-6 p-4 bg-gray-100 rounded-lg space-y-2 text-sm">
                <div className="flex justify-between">
                  <span className="font-medium text-gray-900">
                    ID Chamado: CH-0004
                  </span>
                  <span className="font-medium text-gray-900">
                    Prioridade: {categoriaSelecionada.prioridade}
                  </span>
                </div>
                <div className="flex justify-between">
                  <span className="font-medium text-gray-900">
                    Nível de Suporte: N1
                  </span>
                  <span className="font-medium text-gray-900">
                    SLA: {categoriaSelecionada.sla}
                  </span>
                </div>
              </div>
            )}

            {/* Buttons */}
            <div className="flex gap-4 pt-4 border-t border-gray-200">
              <Button
                onClick={handleAbrirChamado}
                className="flex-1 bg-purple-600 hover:bg-purple-700 text-white font-semibold py-2 rounded-lg transition"
              >
                Abrir chamado
              </Button>
              <Button
                onClick={() => setStep("descricao")}
                variant="outline"
                className="flex-1"
              >
                Voltar
              </Button>
            </div>
          </div>
        </Card>
      </div>
    );
  }

  // STEP 3: Sucesso (Problema Resolvido com IA)
  if (step === "sucesso") {
    return (
      <div className="fixed inset-0 bg-black/50 flex items-center justify-center p-4 z-50">
        <Card className="bg-white rounded-2xl shadow-2xl max-w-md w-full">
          <div className="p-8 text-center">
            {/* Ícone Houston */}
            <div className="mb-6 flex justify-center">
              <img src="/houston-agradece.png" alt="Houston" className="w-32 h-32 object-contain" />
            </div>

            {/* Título */}
            <h2 className="text-2xl font-bold text-purple-900 mb-4">
              Parece que o Houston já te ajudou!
            </h2>

            {/* Mensagem */}
            <p className="text-gray-700 mb-8 text-sm leading-relaxed">
              Salvamos essa informação e estamos felizes com isso! Agora é só ir
              para tela inicial. Caso seu problema não tenha sido resolvido
              ainda, clique em voltar e avance com a abertura de chamado!
            </p>

            {/* Buttons */}
            <div className="flex gap-4">
              <Button
                onClick={() => {
                  handleFechar();
                  // TODO: Redirecionar para home
                }}
                className="flex-1 bg-purple-600 hover:bg-purple-700 text-white font-semibold py-2 rounded-lg transition"
              >
                Ir para tela inicial
              </Button>
              <Button
                onClick={() => setStep("descricao")}
                variant="outline"
                className="flex-1"
              >
                Voltar
              </Button>
            </div>
          </div>
        </Card>
      </div>
    );
  }

  // STEP 4: Chamado Registrado
  if (step === "registrado") {
    return (
      <div className="fixed inset-0 bg-black/50 flex items-center justify-center p-4 z-50">
        <Card className="bg-white rounded-2xl shadow-2xl max-w-md w-full">
          <div className="p-8">
            {/* Botão Fechar (X) */}
            <div className="flex justify-end mb-4">
              <button
                onClick={handleFechar}
                className="text-gray-500 hover:text-gray-700 text-2xl font-light"
                title="Fechar"
              >
                ✕
              </button>
            </div>

            {/* Conteúdo Centralizado */}
            <div className="text-center">
              {/* Ícone Avião de Papel */}
              <div className="mb-0 flex justify-center">
                <img src="/aviao-papel.png" alt="Avião" className="w-8 h-8 object-contain" />
              </div>

              {/* Título */}
              <h2 className="text-2xl font-bold text-purple-900 mb-4">
                Chamado registrado!
              </h2>

              {/* Mensagem */}
              <p className="text-gray-700 mb-4 text-sm leading-relaxed">
                Seu chamado foi enviado com sucesso! Vamos analisar e entrar em
                contato através de seus dados cadastrados.
              </p>

              {/* Info Box */}
              <div className="mb-4 p-4 bg-purple-50 rounded-lg space-y-2 text-sm border border-purple-200">
                <div className="font-medium text-purple-900">
                  Número do chamado: CH-0004
                </div>
                <div className="font-medium text-purple-900">
                  SLA: {categoriaSelecionada?.sla}
                </div>
              </div>

              {/* Button */}
              <Button
                onClick={() => {
                  handleFechar();
                  // Redirecionar para página de chamados
                  window.location.href = "/chamados";
                }}
                className="w-full bg-purple-600 hover:bg-purple-700 text-white font-semibold py-3 rounded-lg transition"
              >
                Voltar aos chamados
              </Button>
            </div>
          </div>
        </Card>
      </div>
    );
  }

  return null;
}
