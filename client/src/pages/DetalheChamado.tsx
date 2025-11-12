import { useState } from "react";
import { Star } from "lucide-react";
import { Button } from "@/components/ui/button";
import Header from "@/components/Header";
import { useLocation } from "wouter";

const CHAMADOS_EXEMPLO = [
  {
    id: "CH-0004",
    tipo: "Sem conexão com a internet",
    tecnico: "Loro José",
    prioridade: "3 - Alta",
    sla: "2h",
    status: "Resolvido",
    avaliacao: 5,
    usuario: "Rafaela Silva",
    setor: "RH",
    descricao: "Minha internet está caindo e voltando desde cedo, uso através do wi-fi mas não sei o que está acontecendo.",
    solucao: "Verificado que o driver da placa de rede estava desatualizado. Foi feita a atualização e reiniciado o equipamento. Usuária testou conexão e está normalizado por enquanto.",
    abertohá: "-",
    abertaEm: "14/05/2025 - 12:32",
    ultimaAtualizacao: "14/05/2025 - 14:40",
    detalhesAdicionais: "Minha máquina ficou localizada na sala 09, estou disponível na empresa durante o horário comercial. IP Máquina: 192.168.0.101",
    anexos: ["Print_Erro_Limpeg"],
  },
];

export default function DetalheChamado() {
  const [, setLocation] = useLocation();
  const [avaliacaoHover, setAvaliacaoHover] = useState(0);

  const chamado = CHAMADOS_EXEMPLO[0];

  if (!chamado) {
    return (
      <div className="min-h-screen bg-gray-50">
        <Header userName="Admin" />
        <div className="max-w-7xl mx-auto px-4 py-8">
          <p className="text-center text-gray-600">Chamado não encontrado</p>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-100">
      <Header userName="Admin" />

      <div className="max-w-7xl mx-auto px-4 py-8">
        <div className="bg-white rounded-3xl p-8 shadow-lg">
          <h1 className="text-4xl font-bold italic text-purple-600 mb-8">Chamado: {chamado.id}</h1>

          <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
            {/* COLUNA ESQUERDA */}
            <div className="space-y-6">
              <div className="text-sm text-gray-700 space-y-1">
                <p>Chamado aberto em: {chamado.abertaEm}</p>
                <p>Última atualização: {chamado.ultimaAtualizacao}</p>
              </div>

              <div className="space-y-2">
                <p className="text-sm"><strong>Nome:</strong> {chamado.usuario}</p>
                <p className="text-sm"><strong>Setor:</strong> {chamado.setor}</p>
              </div>

              <div>
                <p className="text-sm font-bold text-purple-600 mb-2">Descrição do problema:</p>
                <div className="bg-gray-100 p-4 rounded text-sm text-gray-700">
                  {chamado.descricao}
                </div>
              </div>

              <div className="bg-yellow-50 border-l-4 border-yellow-400 p-3 text-xs text-gray-600">
                ⚠️ O campo de categoria foi preenchido automaticamente usando inteligência artificial, mas pode ser editado.
              </div>

              <div>
                <p className="text-sm font-bold text-purple-600 mb-2">Categoria:</p>
                <select
                  defaultValue={chamado.tipo}
                  className="w-full px-4 py-2 bg-yellow-50 border-2 border-yellow-300 rounded text-sm text-gray-900 cursor-pointer"
                >
                  <option>{chamado.tipo}</option>
                </select>
              </div>

              <div className="bg-gray-100 p-4 rounded grid grid-cols-2 gap-4 text-sm">
                <div>
                  <p className="text-gray-600">ID Chamado: <strong>{chamado.id}</strong></p>
                  <p className="text-gray-600">Nível de Suporte: <strong>N1</strong></p>
                </div>
                <div>
                  <p className="text-gray-600">Prioridade: <strong className="text-red-600">{chamado.prioridade}</strong></p>
                  <p className="text-gray-600">SLA: <strong>{chamado.sla}</strong></p>
                </div>
              </div>

              <div>
                <p className="text-sm font-bold text-purple-600 mb-2">Detalhes Adicionais</p>
                <div className="bg-gray-100 p-4 rounded text-sm text-gray-700">
                  {chamado.detalhesAdicionais}
                </div>
              </div>

              {chamado.anexos && chamado.anexos.length > 0 && (
                <div className="flex items-center gap-2">
                  <span className="text-purple-600">📎</span>
                  <span className="text-sm text-purple-600 bg-purple-100 px-3 py-1 rounded">
                    {chamado.anexos[0]}
                  </span>
                </div>
              )}

              <Button
                onClick={() => setLocation("/historico-chamados")}
                className="bg-purple-600 hover:bg-purple-700 text-white font-bold py-2 px-6 rounded-full transition mt-4"
              >
                Voltar aos Chamados
              </Button>
            </div>

            {/* COLUNA DIREITA */}
            <div className="space-y-6">
              <div className="flex justify-between items-start gap-4">
                <div>
                  <p className="text-sm text-gray-600 mb-1"><strong>Status:</strong></p>
                  <p className={`font-bold text-lg ${chamado.status === "Resolvido" ? "text-green-600" : "text-yellow-600"}`}>
                    {chamado.status}
                  </p>
                </div>
                <div>
                  <p className="text-sm text-gray-600 mb-1"><strong>Aberto há:</strong></p>
                  <p className="font-bold text-lg text-gray-900">{chamado.abertohá}</p>
                </div>
              </div>

              <div>
                <p className="text-sm text-gray-600 mb-2"><strong>Técnico responsável:</strong></p>
                <input
                  type="text"
                  defaultValue={chamado.tecnico}
                  className="w-full px-4 py-2 border border-gray-300 rounded text-sm text-gray-900 bg-white"
                />
              </div>

              <div>
                <p className="text-sm text-gray-600 mb-2"><strong>Solução:</strong></p>
                <textarea
                  defaultValue={chamado.solucao}
                  className="w-full px-4 py-2 border border-gray-300 rounded text-sm text-gray-700 bg-cyan-50 resize-none"
                  rows={6}
                />
              </div>

              <div>
                <p className="text-sm font-bold text-gray-900 mb-3">Avalie seu atendimento:</p>
                <div className="flex gap-2 mb-3">
                  {[1, 2, 3, 4, 5].map((star) => (
                    <Star
                      key={star}
                      size={24}
                      className={`cursor-pointer transition ${
                        star <= (avaliacaoHover || chamado.avaliacao)
                          ? "fill-yellow-400 text-yellow-400"
                          : "text-gray-300"
                      }`}
                      onMouseEnter={() => setAvaliacaoHover(star)}
                      onMouseLeave={() => setAvaliacaoHover(0)}
                    />
                  ))}
                </div>
                <p className="text-xs text-gray-600 italic bg-gray-50 p-3 rounded">
                  Muito bom! Me chamaram via teams, rapidamente fizeram testes comigo e identificaram que precisava de uma atualização na máquina. Agora não estou mais com problemas.
                </p>
              </div>

              <Button className="w-full bg-purple-600 hover:bg-purple-700 text-white font-bold py-2 rounded-full transition">
                Enviar Avaliação
              </Button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
