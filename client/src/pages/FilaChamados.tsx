import { useState } from "react";
import { Search, ChevronDown, X } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Card } from "@/components/ui/card";
import Header from "@/components/Header";
import CriarChamadoModal from "@/components/CriarChamadoModal";
import { useAuth } from "@/contexts/AuthContext";
import { useLocation } from "wouter";
import { toast } from "sonner";

const CHAMADOS_FILA = [
  {
    id: "CH-0010",
    usuario: "Marina Costa",
    tipo: "Problema com email",
    prioridade: "2 - Média",
    abertohá: "2h30",
    sla: "8h",
    status: "Aberto",
    setor: "Financeiro",
    descricao: "Não consigo enviar emails. Recebo mensagem de erro ao tentar.",
    abertaEm: "14/05/2025 - 13:00",
    ultimaAtualizacao: "14/05/2025 - 14:45",
    anexos: [],
    tecnico: "",
    solucao: "",
  },
  {
    id: "CH-0009",
    usuario: "Pedro Santos",
    tipo: "Sem conexão com a internet",
    prioridade: "3 - Alta",
    abertohá: "1h15",
    sla: "4h",
    status: "Aberto",
    setor: "Vendas",
    descricao: "Meu computador não consegue conectar à internet desde esta manhã.",
    abertaEm: "14/05/2025 - 14:30",
    ultimaAtualizacao: "14/05/2025 - 14:35",
    anexos: [],
    tecnico: "",
    solucao: "",
  },
  {
    id: "CH-0008",
    usuario: "Ana Silva",
    tipo: "Dúvida sobre sistema",
    prioridade: "1 - Baixa",
    abertohá: "45m",
    sla: "24h",
    status: "Aberto",
    setor: "RH",
    descricao: "Como faço para gerar um relatório no novo sistema?",
    abertaEm: "14/05/2025 - 15:00",
    ultimaAtualizacao: "14/05/2025 - 15:05",
    anexos: [],
    tecnico: "",
    solucao: "",
  },
  {
    id: "CH-0007",
    usuario: "Lucas Oliveira",
    tipo: "Solicitação de equipamento",
    prioridade: "1 - Baixa",
    abertohá: "30m",
    sla: "48h",
    status: "Aberto",
    setor: "TI",
    descricao: "Preciso de um mouse novo para meu computador.",
    abertaEm: "14/05/2025 - 15:15",
    ultimaAtualizacao: "14/05/2025 - 15:20",
    anexos: [],
    tecnico: "",
    solucao: "",
  },
];

const TECNICOS_DISPONIVEIS = ["Loro José", "João Alves", "Maria Silva"];

export default function FilaChamados() {
  const { userName, userType } = useAuth();
  const [, navigate] = useLocation();

  if (userType === "user") {
    navigate("/chamados");
    return null;
  }

  const [searchTerm, setSearchTerm] = useState("");
  const [filterUsuario, setFilterUsuario] = useState("Todos");
  const [filterTipo, setFilterTipo] = useState("Todos");
  const [filterPrioridade, setFilterPrioridade] = useState("Todos");
  const [filterSLA, setFilterSLA] = useState("Todos");
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [chamados, setChamados] = useState(CHAMADOS_FILA);

  // Estados do modal de detalhes
  const [isDetalhesOpen, setIsDetalhesOpen] = useState(false);
  const [chamadoSelecionado, setChamadoSelecionado] = useState<typeof CHAMADOS_FILA[0] | null>(null);
  const [tecnicoEditado, setTecnicoEditado] = useState("");
  const [isEditingTecnico, setIsEditingTecnico] = useState(false);
  const [solucaoEditada, setSolucaoEditada] = useState("");
  const [isEditingSolucao, setIsEditingSolucao] = useState(false);

  const chamadosFiltrados = chamados.filter((chamado) => {
    const matchSearch =
      chamado.id.toLowerCase().includes(searchTerm.toLowerCase()) ||
      chamado.usuario.toLowerCase().includes(searchTerm.toLowerCase()) ||
      chamado.tipo.toLowerCase().includes(searchTerm.toLowerCase());

    const matchUsuario = filterUsuario === "Todos" || chamado.usuario === filterUsuario;
    const matchTipo = filterTipo === "Todos" || chamado.tipo === filterTipo;
    const matchPrioridade = filterPrioridade === "Todos" || chamado.prioridade === filterPrioridade;
    const matchSLA = filterSLA === "Todos" || chamado.sla === filterSLA;

    return matchSearch && matchUsuario && matchTipo && matchPrioridade && matchSLA;
  });

  const handleAssumir = (chamado: typeof CHAMADOS_FILA[0]) => {
    setChamadoSelecionado(chamado);
    setTecnicoEditado(userName || "");
    setSolucaoEditada("");
    setIsEditingTecnico(false);
    setIsEditingSolucao(false);
    setIsDetalhesOpen(true);
  };

  const handleFecharDetalhes = () => {
    setIsDetalhesOpen(false);
    setChamadoSelecionado(null);
  };

  const handleSalvarTecnico = () => {
    if (chamadoSelecionado) {
      chamadoSelecionado.tecnico = tecnicoEditado;
      setIsEditingTecnico(false);
    }
  };

  const handleSalvarSolucao = () => {
    if (chamadoSelecionado) {
      chamadoSelecionado.solucao = solucaoEditada;
      setIsEditingSolucao(false);
    }
  };

  const handleConfirmarAssumicao = () => {
    if (chamadoSelecionado) {
      setChamados(chamados.filter((c) => c.id !== chamadoSelecionado.id));
      toast.success(`Chamado ${chamadoSelecionado.id} assumido com sucesso!`);
      handleFecharDetalhes();
    }
  };

  const getSLAColor = (sla: string) => {
    const hours = parseInt(sla);
    if (hours <= 4) return "bg-green-100 text-green-800";
    if (hours <= 8) return "bg-yellow-100 text-yellow-800";
    return "bg-red-100 text-red-800";
  };

  return (
    <div className="min-h-screen bg-gray-50">
      <Header userName={userName || "Admin"} />

      <div className="max-w-7xl mx-auto px-4 py-8">
        <div className="flex justify-between items-center mb-8">
          <h1 className="text-4xl font-bold text-purple-900">FILA DE CHAMADOS</h1>
          <Button
            onClick={() => setIsModalOpen(true)}
            className="bg-purple-600 hover:bg-purple-700 text-white font-semibold py-3 px-6 rounded-full transition"
          >
            + Novo Chamado
          </Button>
        </div>

        <Card className="mb-6 p-6 bg-white">
          <div className="space-y-4">
            <div className="flex flex-wrap gap-4 items-center">
              <div className="relative">
                <select
                  value={filterUsuario}
                  onChange={(e) => setFilterUsuario(e.target.value)}
                  className="appearance-none px-4 py-2 pr-8 bg-gray-100 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-purple-500 cursor-pointer"
                >
                  <option>Todos</option>
                  <option>Marina Costa</option>
                  <option>Pedro Santos</option>
                  <option>Ana Silva</option>
                  <option>Lucas Oliveira</option>
                </select>
                <ChevronDown size={16} className="absolute right-2 top-3 text-gray-600 pointer-events-none" />
              </div>

              <div className="relative">
                <select
                  value={filterTipo}
                  onChange={(e) => setFilterTipo(e.target.value)}
                  className="appearance-none px-4 py-2 pr-8 bg-gray-100 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-purple-500 cursor-pointer"
                >
                  <option>Todos</option>
                  <option>Sem conexão com a internet</option>
                  <option>Problema com email</option>
                  <option>Criação/Alteração de acessos</option>
                  <option>Solicitação de equipamento</option>
                  <option>Dúvida sobre sistema</option>
                </select>
                <ChevronDown size={16} className="absolute right-2 top-3 text-gray-600 pointer-events-none" />
              </div>

              <div className="relative">
                <select
                  value={filterPrioridade}
                  onChange={(e) => setFilterPrioridade(e.target.value)}
                  className="appearance-none px-4 py-2 pr-8 bg-gray-100 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-purple-500 cursor-pointer"
                >
                  <option>Todos</option>
                  <option>1 - Baixa</option>
                  <option>2 - Média</option>
                  <option>3 - Alta</option>
                </select>
                <ChevronDown size={16} className="absolute right-2 top-3 text-gray-600 pointer-events-none" />
              </div>

              <div className="relative">
                <select
                  value={filterSLA}
                  onChange={(e) => setFilterSLA(e.target.value)}
                  className="appearance-none px-4 py-2 pr-8 bg-gray-100 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-purple-500 cursor-pointer"
                >
                  <option>Todos</option>
                  <option>2h</option>
                  <option>4h</option>
                  <option>8h</option>
                  <option>24h</option>
                  <option>48h</option>
                </select>
                <ChevronDown size={16} className="absolute right-2 top-3 text-gray-600 pointer-events-none" />
              </div>

              <div className="flex-1 flex items-center gap-2">
                <input
                  type="text"
                  placeholder="Busque pelo ID do chamado"
                  value={searchTerm}
                  onChange={(e) => setSearchTerm(e.target.value)}
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-purple-500"
                />
                <button className="p-2 hover:bg-gray-100 rounded-lg transition">
                  <Search size={20} className="text-gray-600" />
                </button>
              </div>
            </div>
          </div>
        </Card>

        <Card className="bg-white overflow-hidden">
          <div className="overflow-x-auto">
            <table className="w-full">
              <thead className="bg-gray-100 border-b border-gray-300">
                <tr>
                  <th className="px-6 py-3 text-left text-sm font-semibold text-gray-900">Chamado</th>
                  <th className="px-6 py-3 text-left text-sm font-semibold text-gray-900">Usuário</th>
                  <th className="px-6 py-3 text-left text-sm font-semibold text-gray-900">Tipo</th>
                  <th className="px-6 py-3 text-left text-sm font-semibold text-gray-900">Prioridade</th>
                  <th className="px-6 py-3 text-left text-sm font-semibold text-gray-900">Aberto há</th>
                  <th className="px-6 py-3 text-left text-sm font-semibold text-gray-900">SLA</th>
                  <th className="px-6 py-3 text-left text-sm font-semibold text-gray-900">Status</th>
                  <th className="px-6 py-3 text-left text-sm font-semibold text-gray-900">Ação</th>
                </tr>
              </thead>
              <tbody>
                {chamadosFiltrados.length > 0 ? (
                  chamadosFiltrados.map((chamado, index) => (
                    <tr
                      key={index}
                      className="border-b border-gray-200 hover:bg-gray-50 transition"
                    >
                      <td className="px-6 py-4 text-sm font-semibold text-purple-600">{chamado.id}</td>
                      <td className="px-6 py-4 text-sm text-gray-700">{chamado.usuario}</td>
                      <td className="px-6 py-4 text-sm text-gray-700">{chamado.tipo}</td>
                      <td className="px-6 py-4 text-sm text-gray-700">{chamado.prioridade}</td>
                      <td className="px-6 py-4 text-sm text-gray-700">{chamado.abertohá}</td>
                      <td className="px-6 py-4">
                        <span className={`px-3 py-1 rounded-full text-xs font-semibold ${getSLAColor(chamado.sla)}`}>
                          {chamado.sla}
                        </span>
                      </td>
                      <td className="px-6 py-4">
                        <span className="px-3 py-1 rounded-full text-xs font-semibold bg-red-100 text-red-800">
                          {chamado.status}
                        </span>
                      </td>
                      <td className="px-6 py-4">
                        <Button
                          onClick={() => handleAssumir(chamado)}
                          className="bg-gray-400 hover:bg-gray-500 text-white font-semibold py-2 px-4 rounded transition text-xs"
                        >
                          ASSUMIR
                        </Button>
                      </td>
                    </tr>
                  ))
                ) : (
                  <tr>
                    <td colSpan={8} className="px-6 py-8 text-center text-gray-500">
                      Nenhum chamado encontrado
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
        </Card>
      </div>

      {/* MODAL DE DETALHES */}
      {isDetalhesOpen && chamadoSelecionado && (
        <div className="fixed inset-0 bg-black/50 flex items-center justify-center p-4 z-50 overflow-y-auto">
          <Card className="bg-white rounded-2xl shadow-2xl max-w-4xl w-full my-8 max-h-[90vh] overflow-y-auto">
            <div className="sticky top-0 bg-white p-6 flex justify-between items-center border-b pb-4 rounded-t-2xl z-10">
              <h2 className="text-2xl font-bold text-purple-900">Chamado: {chamadoSelecionado.id}</h2>
              <button
                onClick={handleFecharDetalhes}
                className="text-gray-500 hover:text-gray-700 transition"
              >
                <X size={24} />
              </button>
            </div>

            <div className="p-6 space-y-6">
              <div className="text-sm text-gray-700 space-y-1">
                <p>Chamado aberto em: {chamadoSelecionado.abertaEm}</p>
                <p>Última atualização: {chamadoSelecionado.ultimaAtualizacao}</p>
              </div>

              <div className="flex justify-between items-start gap-8">
                <div>
                  <p className="text-sm text-gray-600 mb-1"><strong>Prioridade:</strong></p>
                  <p className="font-bold text-lg text-red-600">
                    {chamadoSelecionado.prioridade}
                  </p>
                </div>
                <div>
                  <p className="text-sm text-gray-600 mb-1"><strong>Aberto há:</strong></p>
                  <p className="font-bold text-lg text-gray-900">{chamadoSelecionado.abertohá}</p>
                </div>
              </div>

              <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
                <div className="space-y-4">
                  <div className="space-y-2">
                    <p className="text-sm"><strong>Usuário:</strong> {chamadoSelecionado.usuario}</p>
                    <p className="text-sm"><strong>Setor:</strong> {chamadoSelecionado.setor}</p>
                  </div>

                  <div>
                    <p className="text-sm font-bold text-purple-600 mb-2">Descrição do problema:</p>
                    <div className="bg-gray-100 p-4 rounded text-sm text-gray-700">
                      {chamadoSelecionado.descricao}
                    </div>
                  </div>

                  <div>
                    <p className="text-sm font-bold text-purple-600 mb-2">Tipo:</p>
                    <div className="bg-gray-100 p-4 rounded text-sm text-gray-700">
                      {chamadoSelecionado.tipo}
                    </div>
                  </div>

                  <div className="bg-gray-100 p-4 rounded grid grid-cols-2 gap-4 text-sm">
                    <div>
                      <p className="text-gray-600">ID Chamado: <strong>{chamadoSelecionado.id}</strong></p>
                      <p className="text-gray-600">Nível de Suporte: <strong>N1</strong></p>
                    </div>
                    <div>
                      <p className="text-gray-600">SLA: <strong>{chamadoSelecionado.sla}</strong></p>
                    </div>
                  </div>
                </div>

                <div className="space-y-6">
                  <div>
                    <div className="flex justify-between items-center mb-2">
                      <p className="text-sm text-gray-600"><strong>Técnico responsável:</strong></p>
                      {!isEditingTecnico && (
                        <button
                          onClick={() => setIsEditingTecnico(true)}
                          className="text-purple-600 hover:text-purple-700 text-xs"
                        >
                          ✏️ Editar
                        </button>
                      )}
                    </div>

                    {isEditingTecnico ? (
                      <div className="space-y-2">
                        <select
                          value={tecnicoEditado}
                          onChange={(e) => setTecnicoEditado(e.target.value)}
                          className="w-full px-4 py-2 border border-gray-300 rounded text-sm text-gray-900 bg-white"
                        >
                          {TECNICOS_DISPONIVEIS.map((tecnico) => (
                            <option key={tecnico} value={tecnico}>
                              {tecnico}
                            </option>
                          ))}
                        </select>
                        <div className="flex gap-2">
                          <Button
                            onClick={handleSalvarTecnico}
                            className="flex-1 bg-purple-600 hover:bg-purple-700 text-white text-sm py-1 rounded transition"
                          >
                            Salvar
                          </Button>
                          <Button
                            onClick={() => setIsEditingTecnico(false)}
                            className="flex-1 bg-gray-300 hover:bg-gray-400 text-gray-900 text-sm py-1 rounded transition"
                          >
                            Cancelar
                          </Button>
                        </div>
                      </div>
                    ) : (
                      <input
                        type="text"
                        value={tecnicoEditado}
                        disabled
                        className="w-full px-4 py-2 border border-gray-300 rounded text-sm text-gray-900 bg-gray-50"
                      />
                    )}
                  </div>

                  <div>
                    <div className="flex justify-between items-center mb-2">
                      <p className="text-sm text-gray-600"><strong>Solução:</strong></p>
                      {!isEditingSolucao && (
                        <button
                          onClick={() => setIsEditingSolucao(true)}
                          className="text-purple-600 hover:text-purple-700 text-xs"
                        >
                          ✏️ Editar
                        </button>
                      )}
                    </div>

                    {isEditingSolucao ? (
                      <div className="space-y-2">
                        <textarea
                          value={solucaoEditada}
                          onChange={(e) => setSolucaoEditada(e.target.value)}
                          className="w-full px-4 py-2 border border-gray-300 rounded text-sm text-gray-700 bg-white resize-none"
                          rows={6}
                          placeholder="Descreva a solução do problema..."
                        />
                        <div className="flex gap-2">
                          <Button
                            onClick={handleSalvarSolucao}
                            className="flex-1 bg-purple-600 hover:bg-purple-700 text-white text-sm py-1 rounded transition"
                          >
                            Salvar
                          </Button>
                          <Button
                            onClick={() => setIsEditingSolucao(false)}
                            className="flex-1 bg-gray-300 hover:bg-gray-400 text-gray-900 text-sm py-1 rounded transition"
                          >
                            Cancelar
                          </Button>
                        </div>
                      </div>
                    ) : (
                      <textarea
                        value={solucaoEditada}
                        disabled
                        className="w-full px-4 py-2 border border-gray-300 rounded text-sm text-gray-700 bg-gray-50 resize-none"
                        rows={6}
                      />
                    )}
                  </div>

                  <Button
                    onClick={handleConfirmarAssumicao}
                    className="w-full bg-purple-600 hover:bg-purple-700 text-white font-semibold py-2 rounded transition"
                  >
                    ✓ Assumir Chamado
                  </Button>
                </div>
              </div>
            </div>
          </Card>
        </div>
      )}

      <CriarChamadoModal isOpen={isModalOpen} onClose={() => setIsModalOpen(false)} />
    </div>
  );
}
