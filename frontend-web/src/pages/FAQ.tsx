import { useState } from "react";
import { ChevronDown, X } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Card } from "@/components/ui/card";
import Header from "@/components/Header";
import { useAuth } from "@/contexts/AuthContext";
import { toast } from "sonner";

const FAQS_INICIAIS = [
  {
    id: "1",
    question: "O que é o PIM 3?",
    answer:
      "PIM 3 é um sistema de gestão de perfis e chamados que permite aos usuários gerenciar suas informações pessoais e acompanhar solicitações de suporte de forma centralizada.",
  },
  {
    id: "2",
    question: "Como faço para criar uma conta?",
    answer:
      "Você pode criar uma conta clicando no botão 'Registrar' na página inicial. Preencha seus dados pessoais e siga as instruções de confirmação por email.",
  },
  {
    id: "3",
    question: "Como abro um novo chamado?",
    answer:
      "Clique em 'Novo Chamado' no menu lateral, preencha o formulário com os detalhes do seu problema e clique em 'Enviar Chamado'. Você receberá um número de ID para acompanhar.",
  },
  {
    id: "4",
    question: "Qual é o tempo médio de resposta?",
    answer:
      "Nosso tempo médio de resposta é de 24 horas para chamados com prioridade média. Chamados críticos são atendidos em até 4 horas.",
  },
  {
    id: "5",
    question: "Posso anexar arquivos ao meu chamado?",
    answer:
      "Sim! Você pode anexar arquivos de até 10MB ao criar ou responder a um chamado. Formatos aceitos: PDF, PNG, JPG, DOC, DOCX.",
  },
  {
    id: "6",
    question: "Como atualizo meu perfil?",
    answer:
      "Acesse a seção 'Meu Perfil' no menu lateral, clique em 'Editar Perfil' e faça as alterações desejadas. Não esqueça de salvar as mudanças.",
  },
  {
    id: "7",
    question: "Posso alterar meu email?",
    answer:
      "Sim, você pode alterar seu email na seção de perfil. Após a alteração, você receberá um email de confirmação para validar o novo endereço.",
  },
  {
    id: "8",
    question: "Meus dados estão seguros?",
    answer:
      "Sim! Utilizamos criptografia de ponta a ponta e seguimos as melhores práticas de segurança da indústria para proteger seus dados.",
  },
  {
    id: "9",
    question: "Como faço para resetar minha senha?",
    answer:
      "Clique em 'Esqueci minha senha' na página de login. Você receberá um email com um link para redefinir sua senha.",
  },
  {
    id: "10",
    question: "Como faço para entrar em contato com o suporte?",
    answer:
      "Você pode abrir um chamado diretamente na plataforma ou enviar um email para suporte@pim3.com. Também temos um chat de suporte disponível das 8h às 18h.",
  },
];

export default function FAQ() {
  const { userName, userType } = useAuth();
  const isAdmin = userType === "admin";

  const [faqs, setFaqs] = useState(FAQS_INICIAIS);
  const [expandedId, setExpandedId] = useState<string | null>(null);
  const [searchTerm, setSearchTerm] = useState("");
  const [selectedFaq, setSelectedFaq] = useState<typeof FAQS_INICIAIS[0] | null>(null);
  const [editingTitle, setEditingTitle] = useState("");
  const [editingAnswer, setEditingAnswer] = useState("");

  const filteredFaqs = faqs.filter((faq) => {
    const matchSearch =
      faq.question.toLowerCase().includes(searchTerm.toLowerCase()) ||
      faq.answer.toLowerCase().includes(searchTerm.toLowerCase());
    return matchSearch;
  });

  const handleSelectFaq = (faq: typeof FAQS_INICIAIS[0]) => {
    setSelectedFaq(faq);
    setEditingTitle(faq.question);
    setEditingAnswer(faq.answer);
  };

  const handleSaveFaq = () => {
    if (!selectedFaq) return;

    if (!editingTitle.trim() || !editingAnswer.trim()) {
      toast.error("Preencha todos os campos!");
      return;
    }

    setFaqs(
      faqs.map((faq) =>
        faq.id === selectedFaq.id
          ? {
              ...faq,
              question: editingTitle,
              answer: editingAnswer,
            }
          : faq
      )
    );

    toast.success("FAQ atualizada com sucesso!");
    setSelectedFaq(null);
    setEditingTitle("");
    setEditingAnswer("");
  };

  const handleDeleteFaq = () => {
    if (!selectedFaq) return;

    if (confirm(`Tem certeza que deseja deletar a FAQ: "${selectedFaq.question}"?`)) {
      setFaqs(faqs.filter((faq) => faq.id !== selectedFaq.id));
      toast.success("FAQ deletada com sucesso!");
      setSelectedFaq(null);
      setEditingTitle("");
      setEditingAnswer("");
    }
  };

  const handleCreateNewFaq = () => {
    const newId = String(Math.max(...faqs.map((f) => parseInt(f.id)), 0) + 1);
    const newFaq = {
      id: newId,
      question: "Nova pergunta",
      answer: "Nova resposta",
    };
    setFaqs([...faqs, newFaq]);
    handleSelectFaq(newFaq);
    toast.success("Nova FAQ criada!");
  };

  const handleClearSelection = () => {
    setSelectedFaq(null);
    setEditingTitle("");
    setEditingAnswer("");
  };

  return (
    <div className="min-h-screen bg-gray-50">
      <Header userName={userName || "Usuário"} />

      <div className={`max-w-7xl mx-auto px-4 py-8 ${isAdmin ? "" : "max-w-4xl"}`}>
        <div className="mb-8">
          <h1 className="text-4xl font-bold text-purple-900">FAQ - Dúvidas Frequentes</h1>
          <p className="text-gray-600 mt-2">
            {isAdmin ? "Gerencie as perguntas frequentes do sistema" : "Encontre respostas para suas dúvidas"}
          </p>
        </div>

        {/* LAYOUT ADMIN - COM EDIÇÃO */}
        {isAdmin ? (
          <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
            {/* COLUNA ESQUERDA - LISTA DE FAQs */}
            <div className="lg:col-span-2 space-y-4">
              {/* Filtros */}
              <Card className="p-4 bg-white">
                <div className="space-y-3">
                  <input
                    type="text"
                    placeholder="Buscar..."
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                    className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-purple-500"
                  />
                </div>
              </Card>

              {/* Lista de FAQs */}
              <div className="space-y-3">
                {filteredFaqs.length > 0 ? (
                  filteredFaqs.map((faq) => (
                    <Card
                      key={faq.id}
                      className={`overflow-hidden cursor-pointer transition ${
                        selectedFaq?.id === faq.id
                          ? "ring-2 ring-purple-600 shadow-md"
                          : "hover:shadow-md"
                      }`}
                      onClick={() => handleSelectFaq(faq)}
                    >
                      <button
                        onClick={(e) => {
                          e.stopPropagation();
                          setExpandedId(expandedId === faq.id ? null : faq.id);
                        }}
                        className="w-full p-4 text-left flex items-start justify-between gap-3 hover:bg-gray-50 transition"
                      >
                        <div className="flex-1">
                          <h3 className="text-sm font-semibold text-gray-900">{faq.question}</h3>
                        </div>
                        <ChevronDown
                          size={20}
                          className={`text-gray-400 flex-shrink-0 transition ${
                            expandedId === faq.id ? "rotate-180" : ""
                          }`}
                        />
                      </button>

                      {expandedId === faq.id && (
                        <div className="px-4 pb-4 border-t border-gray-100 bg-gray-50">
                          <p className="text-sm text-gray-700 leading-relaxed">{faq.answer}</p>
                        </div>
                      )}
                    </Card>
                  ))
                ) : (
                  <Card className="p-6 text-center text-gray-500">
                    Nenhuma FAQ encontrada
                  </Card>
                )}
              </div>
            </div>

            {/* COLUNA DIREITA - PAINEL DE EDIÇÃO */}
            <div>
              {selectedFaq ? (
                <Card className="p-6 bg-white sticky top-8">
                  <div className="flex justify-between items-center mb-4">
                    <h2 className="text-lg font-bold text-purple-900">Editar FAQ</h2>
                    <button
                      onClick={handleClearSelection}
                      className="text-gray-500 hover:text-gray-700"
                    >
                      <X size={20} />
                    </button>
                  </div>

                  <div className="space-y-4">
                    {/* Título */}
                    <div>
                      <label className="block text-sm font-semibold text-gray-700 mb-2">
                        Título:
                      </label>
                      <input
                        type="text"
                        value={editingTitle}
                        onChange={(e) => setEditingTitle(e.target.value)}
                        className="w-full px-3 py-2 border border-gray-300 rounded text-sm focus:outline-none focus:ring-2 focus:ring-purple-500"
                      />
                    </div>

                    {/* Artigo */}
                    <div>
                      <label className="block text-sm font-semibold text-gray-700 mb-2">
                        Artigo:
                      </label>
                      <textarea
                        value={editingAnswer}
                        onChange={(e) => setEditingAnswer(e.target.value)}
                        className="w-full px-3 py-2 border border-gray-300 rounded text-sm focus:outline-none focus:ring-2 focus:ring-purple-500 resize-none"
                        rows={8}
                      />
                    </div>

                    {/* Botões */}
                    <div className="flex gap-2">
                      <Button
                        onClick={handleDeleteFaq}
                        className="flex-1 bg-red-600 hover:bg-red-700 text-white text-sm py-2 rounded transition"
                      >
                        Excluir FAQ
                      </Button>
                      <Button
                        onClick={handleSaveFaq}
                        className="flex-1 bg-purple-600 hover:bg-purple-700 text-white text-sm py-2 rounded transition"
                      >
                        Salvar Alterações
                      </Button>
                    </div>
                  </div>
                </Card>
              ) : (
                <Card className="p-6 bg-white sticky top-8 text-center">
                  <p className="text-gray-500 mb-4">Selecione uma FAQ para editar</p>
                  <Button
                    onClick={handleCreateNewFaq}
                    className="w-full bg-purple-600 hover:bg-purple-700 text-white font-semibold py-2 rounded transition"
                  >
                    + Nova FAQ
                  </Button>
                </Card>
              )}
            </div>
          </div>
        ) : (
          /* LAYOUT USER - APENAS LEITURA */
          <div className="space-y-6">
            {/* Busca */}
            <Card className="p-4 bg-white">
              <input
                type="text"
                placeholder="Buscar..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-purple-500"
              />
            </Card>

            {/* Lista de FAQs */}
            <div className="space-y-3">
              {filteredFaqs.length > 0 ? (
                filteredFaqs.map((faq) => (
                  <Card
                    key={faq.id}
                    className="overflow-hidden hover:shadow-md transition"
                  >
                    <button
                      onClick={() =>
                        setExpandedId(expandedId === faq.id ? null : faq.id)
                      }
                      className="w-full p-4 text-left flex items-start justify-between gap-3 hover:bg-gray-50 transition"
                    >
                      <div className="flex-1">
                        <h3 className="text-sm font-semibold text-gray-900">{faq.question}</h3>
                      </div>
                      <ChevronDown
                        size={20}
                        className={`text-gray-400 flex-shrink-0 transition ${
                          expandedId === faq.id ? "rotate-180" : ""
                        }`}
                      />
                    </button>

                    {expandedId === faq.id && (
                      <div className="px-4 pb-4 border-t border-gray-100 bg-gray-50">
                        <p className="text-sm text-gray-700 leading-relaxed">{faq.answer}</p>
                      </div>
                    )}
                  </Card>
                ))
              ) : (
                <Card className="p-6 text-center text-gray-500">
                  Nenhuma FAQ encontrada
                </Card>
              )}
            </div>

            {/* Seção de Contato */}
            <Card className="p-8 bg-gradient-to-r from-purple-600 to-purple-700 text-white">
              <div className="text-center">
                <h2 className="text-2xl font-bold mb-3">Não encontrou sua resposta?</h2>
                <p className="text-purple-100 mb-6">
                  Nossa equipe de suporte está pronta para ajudá-lo
                </p>
                <div className="flex flex-col sm:flex-row gap-4 justify-center">
                  <button className="px-6 py-2 bg-white text-purple-600 font-semibold rounded-lg hover:bg-purple-50 transition">
                    Abrir Chamado
                  </button>
                  <button className="px-6 py-2 border-2 border-white text-white font-semibold rounded-lg hover:bg-purple-600 transition">
                    Enviar Email
                  </button>
                </div>
              </div>
            </Card>
          </div>
        )}
      </div>
    </div>
  );
}
