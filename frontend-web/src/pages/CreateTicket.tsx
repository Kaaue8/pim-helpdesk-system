import Layout from "@/components/Layout";
import { Card } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { useState } from "react";
import { useAuth } from "@/contexts/AuthContext"; 
import { useNavigate } from "react-router-dom"; 
import { toast } from "sonner"; 

export default function CreateTicket() {
  const { token } = useAuth();
  const navigate = useNavigate();
  const [isSubmitting, setIsSubmitting] = useState(false); // Desabilita o botão durante o envio
  const [formData, setFormData] = useState({
    title: "",
    category: "",
    priority: "media",
    description: "",
    attachment: null as string | null,
  });

const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
  if (!token) {
    toast.error("Sua sessão expirou. Por favor, faça login novamente.");
    return;
  }

  setIsSubmitting(true);

  // Objeto que será enviado para a API
  const ticketData = {
    titulo: formData.title,
    descricao: formData.description,
    prioridade: formData.priority,
    // Status inicial é sempre 'Aberto'
    status: "Aberto", 
    // O ID do solicitante virá do token no backend, não precisamos enviar
  };

  try {
    const response = await fetch('http://localhost:5079/api/Tickets', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`,
      },
      body: JSON.stringify(ticketData ),
    });

    if (!response.ok) {
      
      const errorResult = await response.json().catch(() => null);
      throw new Error(errorResult?.message || "Falha ao criar o chamado.");
    }

    toast.success("Chamado criado com sucesso!");
    
    // Após o sucesso, redireciona o usuário para a fila de chamados para ver o ticket novo
    navigate("/fila-chamados");

  } catch (error) {
    toast.error((error as Error).message);
  } finally {
    setIsSubmitting(false);
  }
  };

  const handleChange = (
    e: React.ChangeEvent<
      HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement
    >
  ) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  return (
    <Layout>
      <div className="space-y-8">
        <div>
          <h1 className="text-3xl font-bold text-gray-900">Abrir Novo Chamado</h1>
          <p className="text-gray-600 mt-2">
            Descreva seu problema ou solicitação para que nossa equipe possa
            ajudá-lo
          </p>
        </div>

        <Card className="p-8">
          <form onSubmit={handleSubmit} className="space-y-6">
            {/* Title */}
            <div>
              <label className="block text-sm font-semibold text-gray-700 mb-2">
                Título do Chamado *
              </label>
              <input
                type="text"
                name="title"
                value={formData.title}
                onChange={handleChange}
                placeholder="Ex: Problema ao fazer login"
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-transparent outline-none transition"
                required
              />
            </div>

            {/* Category */}
            <div>
              <label className="block text-sm font-semibold text-gray-700 mb-2">
                Categoria *
              </label>
              <select
                name="category"
                value={formData.category}
                onChange={handleChange}
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-transparent outline-none transition"
                required
              >
                <option value="">Selecione uma categoria</option>
                <option value="tecnico">Suporte Técnico</option>
                <option value="funcionalidade">Solicitação de Funcionalidade</option>
                <option value="bug">Relato de Bug</option>
                <option value="duvida">Dúvida</option>
                <option value="outro">Outro</option>
              </select>
            </div>

            {/* Priority */}
            <div>
              <label className="block text-sm font-semibold text-gray-700 mb-2">
                Prioridade
              </label>
              <div className="flex gap-4">
                {[
                  { value: "baixa", label: "Baixa" },
                  { value: "media", label: "Média" },
                  { value: "alta", label: "Alta" },
                  { value: "critica", label: "Crítica" },
                ].map((option) => (
                  <label key={option.value} className="flex items-center gap-2">
                    <input
                      type="radio"
                      name="priority"
                      value={option.value}
                      checked={formData.priority === option.value}
                      onChange={handleChange}
                      className="w-4 h-4 text-purple-600"
                    />
                    <span className="text-gray-700">{option.label}</span>
                  </label>
                ))}
              </div>
            </div>

            {/* Description */}
            <div>
              <label className="block text-sm font-semibold text-gray-700 mb-2">
                Descrição Detalhada *
              </label>
              <textarea
                name="description"
                value={formData.description}
                onChange={handleChange}
                placeholder="Descreva seu problema em detalhes. Inclua passos para reproduzir o problema, mensagens de erro, etc."
                rows={6}
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-transparent outline-none transition resize-none"
                required
              />
            </div>

            {/* Attachment */}
            <div>
              <label className="block text-sm font-semibold text-gray-700 mb-2">
                Anexar Arquivo (Opcional)
              </label>
              <div className="border-2 border-dashed border-gray-300 rounded-lg p-6 text-center hover:border-purple-500 transition cursor-pointer">
                <input
                  type="file"
                  className="hidden"
                  id="file-input"
                  onChange={(e) => {
                    const file = e.target.files?.[0];
                    if (file) {
                      setFormData((prev) => ({
                        ...prev,
                        attachment: file.name,
                      }));
                    }
                  }}
                />
                <label htmlFor="file-input" className="cursor-pointer">
                  <svg
                    className="mx-auto h-12 w-12 text-gray-400"
                    stroke="currentColor"
                    fill="none"
                    viewBox="0 0 48 48"
                  >
                    <path
                      d="M28 8H12a4 4 0 00-4 4v20a4 4 0 004 4h24a4 4 0 004-4V20"
                      strokeWidth={2}
                      strokeLinecap="round"
                      strokeLinejoin="round"
                    />
                    <path
                      d="M32 4v12m0 0l-4-4m4 4l4-4"
                      strokeWidth={2}
                      strokeLinecap="round"
                      strokeLinejoin="round"
                    />
                  </svg>
                  <p className="mt-2 text-sm font-medium text-gray-700">
                    Clique para selecionar ou arraste um arquivo
                  </p>
                  <p className="text-xs text-gray-500 mt-1">
                    PNG, JPG, PDF até 10MB
                  </p>
                </label>
                {formData.attachment && (
                  <p className="mt-2 text-sm text-green-600 font-medium">
                    ✓ {formData.attachment}
                  </p>
                )}
              </div>
            </div>

            {/* Terms */}
            <div className="flex items-start gap-3 p-4 bg-blue-50 rounded-lg">
              <input
                type="checkbox"
                id="terms"
                className="mt-1 w-4 h-4 text-blue-600"
                required
              />
              <label htmlFor="terms" className="text-sm text-gray-700">
                Concordo que meu chamado será analisado e respondido conforme
                nossa política de atendimento.
              </label>
            </div>

            {/* Buttons */}
            <div className="flex gap-4 pt-6 border-t border-gray-200">
              <Button
                type="submit"
                className="bg-purple-600 hover:bg-purple-700 text-white flex-1"
                disabled={isSubmitting} 
              >
                {isSubmitting ? "Enviando..." : "Enviar Chamado"}
              </Button>
              <Button
                type="button"
                variant="outline"
                className="flex-1"
                onClick={() => {
                  setFormData({
                    title: "",
                    category: "",
                    priority: "media",
                    description: "",
                    attachment: null,
                  });
                }}
              >
                Limpar
              </Button>
            </div>
          </form>
        </Card>

        {/* Help Section */}
        <Card className="p-6 bg-gradient-to-r from-purple-50 to-blue-50 border-purple-200">
          <h3 className="font-bold text-gray-900 mb-3">Dicas para um melhor atendimento:</h3>
          <ul className="space-y-2 text-sm text-gray-700">
            <li>✓ Seja específico e detalhado na descrição do problema</li>
            <li>✓ Inclua prints ou arquivos que ajudem a entender o problema</li>
            <li>✓ Mencione quando o problema começou</li>
            <li>✓ Descreva os passos que levaram ao erro</li>
          </ul>
        </Card>
      </div>
    </Layout>
  );
}

