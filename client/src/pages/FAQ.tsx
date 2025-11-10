import Layout from "@/components/Layout";
import { Card } from "@/components/ui/card";
import { useState } from "react";
import { ChevronDown } from "lucide-react";

export default function FAQ() {
  const [expandedId, setExpandedId] = useState<string | null>(null);

  const faqs = [
    {
      id: "1",
      category: "Geral",
      question: "O que é o PIM 3?",
      answer:
        "PIM 3 é um sistema de gestão de perfis e chamados que permite aos usuários gerenciar suas informações pessoais e acompanhar solicitações de suporte de forma centralizada.",
    },
    {
      id: "2",
      category: "Geral",
      question: "Como faço para criar uma conta?",
      answer:
        "Você pode criar uma conta clicando no botão 'Registrar' na página inicial. Preencha seus dados pessoais e siga as instruções de confirmação por email.",
    },
    {
      id: "3",
      category: "Chamados",
      question: "Como abro um novo chamado?",
      answer:
        "Clique em 'Novo Chamado' no menu lateral, preencha o formulário com os detalhes do seu problema e clique em 'Enviar Chamado'. Você receberá um número de ID para acompanhar.",
    },
    {
      id: "4",
      category: "Chamados",
      question: "Qual é o tempo médio de resposta?",
      answer:
        "Nosso tempo médio de resposta é de 24 horas para chamados com prioridade média. Chamados críticos são atendidos em até 4 horas.",
    },
    {
      id: "5",
      category: "Chamados",
      question: "Posso anexar arquivos ao meu chamado?",
      answer:
        "Sim! Você pode anexar arquivos de até 10MB ao criar ou responder a um chamado. Formatos aceitos: PDF, PNG, JPG, DOC, DOCX.",
    },
    {
      id: "6",
      category: "Perfil",
      question: "Como atualizo meu perfil?",
      answer:
        "Acesse a seção 'Meu Perfil' no menu lateral, clique em 'Editar Perfil' e faça as alterações desejadas. Não esqueça de salvar as mudanças.",
    },
    {
      id: "7",
      category: "Perfil",
      question: "Posso alterar meu email?",
      answer:
        "Sim, você pode alterar seu email na seção de perfil. Após a alteração, você receberá um email de confirmação para validar o novo endereço.",
    },
    {
      id: "8",
      category: "Segurança",
      question: "Meus dados estão seguros?",
      answer:
        "Sim! Utilizamos criptografia de ponta a ponta e seguimos as melhores práticas de segurança da indústria para proteger seus dados.",
    },
    {
      id: "9",
      category: "Segurança",
      question: "Como faço para resetar minha senha?",
      answer:
        "Clique em 'Esqueci minha senha' na página de login. Você receberá um email com um link para redefinir sua senha.",
    },
    {
      id: "10",
      category: "Suporte",
      question: "Como faço para entrar em contato com o suporte?",
      answer:
        "Você pode abrir um chamado diretamente na plataforma ou enviar um email para suporte@pim3.com. Também temos um chat de suporte disponível das 8h às 18h.",
    },
  ];

  const categories = ["Todos", ...Array.from(new Set(faqs.map((faq) => faq.category)))];

  const [selectedCategory, setSelectedCategory] = useState<string>("Todos");

  const filteredFaqs =
    selectedCategory === "Todos"
      ? faqs
      : faqs.filter((faq) => faq.category === selectedCategory);

  return (
    <Layout>
      <div className="space-y-8">
        <div>
          <h1 className="text-3xl font-bold text-gray-900">
            Perguntas Frequentes
          </h1>
          <p className="text-gray-600 mt-2">
            Encontre respostas para as dúvidas mais comuns
          </p>
        </div>

        {/* Category Filter */}
        <Card className="p-6">
          <div className="flex flex-wrap gap-2">
            {categories.map((category) => (
              <button
                key={category}
                onClick={() => setSelectedCategory(category)}
                className={`px-4 py-2 rounded-lg font-medium transition ${
                  selectedCategory === category
                    ? "bg-purple-600 text-white"
                    : "bg-gray-100 text-gray-700 hover:bg-gray-200"
                }`}
              >
                {category}
              </button>
            ))}
          </div>
        </Card>

        {/* FAQs */}
        <div className="space-y-4">
          {filteredFaqs.map((faq) => (
            <Card
              key={faq.id}
              className="overflow-hidden hover:shadow-md transition"
            >
              <button
                onClick={() =>
                  setExpandedId(expandedId === faq.id ? null : faq.id)
                }
                className="w-full p-6 text-left flex items-start justify-between gap-4 hover:bg-gray-50 transition"
              >
                <div className="flex-1">
                  <div className="flex items-center gap-3 mb-2">
                    <span className="text-xs font-semibold text-purple-600 bg-purple-50 px-2 py-1 rounded">
                      {faq.category}
                    </span>
                  </div>
                  <h3 className="text-lg font-semibold text-gray-900">
                    {faq.question}
                  </h3>
                </div>
                <ChevronDown
                  size={24}
                  className={`text-gray-400 flex-shrink-0 transition ${
                    expandedId === faq.id ? "rotate-180" : ""
                  }`}
                />
              </button>

              {expandedId === faq.id && (
                <div className="px-6 pb-6 border-t border-gray-100 bg-gray-50">
                  <p className="text-gray-700 leading-relaxed">{faq.answer}</p>
                </div>
              )}
            </Card>
          ))}
        </div>

        {/* Contact Section */}
        <Card className="p-8 bg-gradient-to-r from-purple-600 to-purple-700 text-white">
          <div className="text-center">
            <h2 className="text-2xl font-bold mb-3">
              Não encontrou sua resposta?
            </h2>
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
    </Layout>
  );
}

