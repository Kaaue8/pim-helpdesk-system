import { useState } from "react";
import { Search, ChevronDown, X, Download } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Card } from "@/components/ui/card";
import Header from "@/components/Header";

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
    anexos: [
      { nome: "Print_Erro_1.jpeg", tamanho: "2.5 MB", url: "#" },
    ],
  },
  {
    id: "CH-0003",
    tipo: "Criação/Alteração de acessos",
    tecnico: "Loro José",
    prioridade: "2 - Média",
    sla: "9h",
    status: "Em andamento",
    avaliacao: 0,
    usuario: "Rafaela Silva",
    setor: "RH",
    descricao: "Preciso de acesso ao sistema de RH",
    solucao: "",
    abertohá: "09h15",
    abertaEm: "14/05/2025 - 08:00",
    ultimaAtualizacao: "14/05/2025 - 14:30",
    anexos: [],
  },
  {
    id: "CH-0002",
    tipo: "Solicitação de equipamento",
    tecnico: "João Alves",
    prioridade: "1 - Baixa",
    sla: "6h",
    status: "Em andamento",
    avaliacao: 0,
    usuario: "Yuri Alberto",
    setor: "TI",
    descricao: "Preciso de um mouse novo",
    solucao: "",
    abertohá: "6h22",
    abertaEm: "14/05/2025 - 08:15",
    ultimaAtualizacao: "14/05/2025 - 14:25",
    anexos: [],
  },
  {
    id: "CH-0001",
    tipo: "Criação/Alteração de acessos",
    tecnico: "João Alves",
    prioridade: "2 - Média",
    sla: "2h",
    status: "Resolvido",
    avaliacao: 0,
    usuario: "Carlos Souza",
    setor: "Financeiro",
    descricao: "Preciso de acesso ao sistema de financeiro",
    solucao: "Acesso criado com sucesso no sistema.",
    abertohá: "-",
    abertaEm: "13/05/2025 - 10:00",
    ultimaAtualizacao: "13/05/2025 - 11:30",
    anexos: [],
  },
];

const TECNICOS_DISPONIVEIS = ["Loro José", "João Alves", "Maria Silva"];

export default function HistoricoChamados() {
  const [searchTerm, setSearchTerm] = useState("");
  const [filterTipo, setFilterTipo] = useState("Todos");
  const [filterStatus, setFilterStatus] = useState("Todos");
  const [filterSLA, setFilterSLA] = useState("Todos");
  const [filterTecnico, setFilterTecnico] = useState("Todos");
  const [filterPrioridade, setFilterPrioridade] = useState("Todos");
  const [filterUsuario, setFilterUsuario] = useState("Todos");
  const [filterAvaliacao, setFilterAvaliacao] = useState("Todos");
  
  const [isDetalhesOpen, setIsDetalhesOpen] = useState(false);
  const [chamadoSelecionado, setChamadoSelecionado] = useState<typeof CHAMADOS_EXEMPLO[0] | null>(null);
  const [tecnicoEditado, setTecnicoEditado] = useState("");
  const [isEditingTecnico, setIsEditingTecnico] = useState(false);
  const [solucaoEditada, setSolucaoEditada] = useState("");
  const [isEditingSolucao, setIsEditingSolucao] = useState(false);

  const chamadosFiltrados = CHAMADOS_EXEMPLO.filter((chamado) => {
    if (chamado.status === "Aberto") return false;

    const matchSearch =
      chamado.id.toLowerCase().includes(searchTerm.toLowerCase()) ||
      chamado.tipo.toLowerCase().includes(searchTerm.toLowerCase());

    const matchTipo = filterTipo === "Todos" || chamado.tipo === filterTipo;
    const matchStatus = filterStatus === "Todos" || chamado.status === filterStatus;
    const matchTecnico = filterTecnico === "Todos" || chamado.tecnico === filterTecnico;
    const matchPrioridade = filterPrioridade === "Todos" || chamado.prioridade === filterPrioridade;
    const matchUsuario = filterUsuario === "Todos" || chamado.usuario === filterUsuario;
    const matchAvaliacao =
      filterAvaliacao === "Todos" ||
      (filterAvaliacao === "Avaliado" && chamado.avaliacao > 0) ||
      (filterAvaliacao === "Pendente" && chamado.avaliacao === 0);

    return (
      matchSearch &&
      matchTipo &&
      matchStatus &&
      matchTecnico &&
      matchPrioridade &&
      matchUsuario &&
      matchAvaliacao
    );
  });

  const getStatusColor = (status: string) => {
    switch (status) {
      case "Resolvido":
        return "bg-green-100 text-green-800";
      case "Em andamento":
        return "bg-yellow-100 text-yellow-800";
      case "Aberto":
        return "bg-red-100 text-red-800";
      default:
        return "bg-gray-100 text-gray-800";
    }
  };

  const getSLAColor = (sla: string) => {
    const hours = parseInt(sla);
    if (hours <= 4) return "bg-green-100 text-green-800";
    if (hours <= 8) return "bg-yellow-100 text-yellow-800";
    return "bg-red-100 text-red-800";
  };

  const handleAbrirDetalhes = (chamado: typeof CHAMADOS_EXEMPLO[0]) => {
    setChamadoSelecionado(chamado);
    setTecnicoEditado(chamado.tecnico);
    setSolucaoEditada(chamado.solucao);
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

  const handleEncerrarChamado = () => {
    if (chamadoSelecionado && chamadoSelecionado.status === "Em andamento") {
      chamadoSelecionado.status = "Resolvido";
      setChamadoSelecionado({ ...chamadoSelecionado });
    }
  };

  const usuariosUnicos = Array.from(new Set(CHAMADOS_EXEMPLO.map((c) => c.usuario)));

  return (
    <div className="min-h-screen bg-gray-50">
      <Header userName="Admin" />

      <div className="max-w-7xl mx-auto px-4 py-8">
        <div className="flex justify-between items-center mb-8">
          <h1 className="text-4xl font-bold text-purple-900">HISTÓRICO DE CHAMADOS</h1>
          <Button className="bg-purple-600 hover:bg-purple-700 text-white font-semibold py-3 px-6 rounded-full transition">
            + Novo Chamado
          </Button>
        </div>

        <Card className="mb-6 p-6 bg-white">
          <div className="space-y-4">
            <div className="grid grid-cols-2 sm:grid-cols-3 lg:grid-cols-5 gap-4">
              <div>
                <label className="block text-xs font-semibold text-gray-700 mb-2">Tipo</label>
                <div className="relative">
                  <select
                    value={filterTipo}
                    onChange={(e) => setFilterTipo(e.target.value)}
                    className="appearance-none w-full px-4 py-2 pr-8 bg-gray-100 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-purple-500 cursor-pointer"
                  >
                    <option>Todos</option>
                    <option>Sem conexão com a internet</option>
                    <option>Criação/Alteração de acessos</option>
                    <option>Solicitação de equipamento</option>
                  </select>
                  <ChevronDown size={16} className="absolute right-2 top-3 text-gray-600 pointer-events-none" />
                </div>
              </div>

              <div>
                <label className="block text-xs font-semibold text-gray-700 mb-2">Status</label>
                <div className="relative">
                  <select
                    value={filterStatus}
                    onChange={(e) => setFilterStatus(e.target.value)}
                    className="appearance-none w-full px-4 py-2 pr-8 bg-gray-100 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-purple-500 cursor-pointer"
                  >
                    <option>Todos</option>
                    <option>Em andamento</option>
                    <option>Resolvido</option>
                  </select>
                  <ChevronDown size={16} className="absolute right-2 top-3 text-gray-600 pointer-events-none" />
                </div>
              </div>

              <div>
                <label className="block text-xs font-semibold text-gray-700 mb-2">SLA Atual</label>
                <div className="relative">
                  <select
                    value={filterSLA}
                    onChange={(e) => setFilterSLA(e.target.value)}
                    className="appearance-none w-full px-4 py-2 pr-8 bg-gray-100 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-purple-500 cursor-pointer"
                  >
                    <option>Todos</option>
                    <option>2h</option>
                    <option>6h</option>
                    <option>9h</option>
                  </select>
                  <ChevronDown size={16} className="absolute right-2 top-3 text-gray-600 pointer-events-none" />
                </div>
              </div>

              <div>
                <label className="block text-xs font-semibold text-gray-700 mb-2">Técnico</label>
                <div className="relative">
                  <select
                    value={filterTecnico}
                    onChange={(e) => setFilterTecnico(e.target.value)}
                    className="appearance-none w-full px-4 py-2 pr-8 bg-gray-100 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-purple-500 cursor-pointer"
                  >
                    <option>Todos</option>
                    <option>Loro José</option>
                    <option>João Alves</option>
                  </select>
                  <ChevronDown size={16} className="absolute right-2 top-3 text-gray-600 pointer-events-none" />
                </div>
              </div>

              <div>
                <label className="block text-xs font-semibold text-gray-700 mb-2">Prioridade</label>
                <div className="relative">
                  <select
                    value={filterPrioridade}
                    onChange={(e) => setFilterPrioridade(e.target.value)}
                    className="appearance-none w-full px-4 py-2 pr-8 bg-gray-100 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-purple-500 cursor-pointer"
                  >
                    <option>Todos</option>
                    <option>1 - Baixa</option>
                    <option>2 - Média</option>
                    <option>3 - Alta</option>
                  </select>
                  <ChevronDown size={16} className="absolute right-2 top-3 text-gray-600 pointer-events-none" />
                </div>
              </div>
            </div>

            <div className="flex gap-2">
              <input
                type="text"
                placeholder="Busque pelo ID do chamado"
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                className="flex-1 px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-purple-500"
              />
              <Button className="bg-purple-600 hover:bg-purple-700 text-white px-6 rounded-lg transition">
                🔍
              </Button>
            </div>
          </div>
        </Card>

        <Card className="overflow-hidden">
          <div className="overflow-x-auto">
            <table className="w-full">
              <thead>
                <tr className="bg-gray-100 border-b">
                  <th className="px-4 py-3 text-left text-sm font-semibold text-gray-700">Chamado</th>
                  <th className="px-4 py-3 text-left text-sm font-semibold text-gray-700">Tipo</th>
                  <th className="px-4 py-3 text-left text-sm font-semibold text-gray-700">Técnico</th>
                  <th className="px-4 py-3 text-left text-sm font-semibold text-gray-700">Prioridade</th>
                  <th className="px-4 py-3 text-left text-sm font-semibold text-gray-700">Aberto há</th>
                  <th className="px-4 py-3 text-left text-sm font-semibold text-gray-700">SLA Atual</th>
                  <th className="px-4 py-3 text-left text-sm font-semibold text-gray-700">Status</th>
                  <th className="px-4 py-3 text-left text-sm font-semibold text-gray-700">Avaliação</th>
                </tr>
              </thead>
              <tbody>
                {chamadosFiltrados.map((chamado) => (
                  <tr
                    key={chamado.id}
                    onClick={() => handleAbrirDetalhes(chamado)}
                    className="border-b hover:bg-gray-50 cursor-pointer transition"
                  >
                    <td className="px-4 py-3 text-sm font-semibold text-purple-600">{chamado.id}</td>
                    <td className="px-4 py-3 text-sm text-gray-700">{chamado.tipo}</td>
                    <td className="px-4 py-3 text-sm text-gray-700">{chamado.tecnico}</td>
                    <td className="px-4 py-3 text-sm font-semibold text-gray-900">{chamado.prioridade}</td>
                    <td className="px-4 py-3 text-sm text-gray-700">{chamado.abertohá}</td>
                    <td className="px-4 py-3 text-sm">
                      <span className={`px-3 py-1 rounded-full text-xs font-semibold ${getSLAColor(chamado.sla)}`}>
                        {chamado.sla}
                      </span>
                    </td>
                    <td className="px-4 py-3 text-sm">
                      <span className={`px-3 py-1 rounded-full text-xs font-semibold ${getStatusColor(chamado.status)}`}>
                        {chamado.status}
                      </span>
                    </td>
                    <td className="px-4 py-3 text-sm">
                      {chamado.avaliacao > 0 ? (
                        <div className="flex gap-1">
                          {[1, 2, 3, 4, 5].map((star) => (
                            <span key={star} className={star <= chamado.avaliacao ? "text-yellow-400" : "text-gray-300"}>
                              ★
                            </span>
                          ))}
                        </div>
                      ) : (
                        <span className="text-gray-400 text-xs">Pendente</span>
                      )}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </Card>
      </div>

      {/* MODAL DE DETALHES */}
      {isDetalhesOpen && chamadoSelecionado && (
        <div className="fixed inset-0 bg-black/50 flex items-center justify-center p-4 z-50 overflow-y-auto">
          <Card className="bg-white rounded-2xl shadow-2xl max-w-4xl w-full my-8 max-h-[90vh] overflow-y-auto">
            {/* Header */}
            <div className="sticky top-0 bg-white p-6 flex justify-between items-center border-b pb-4 rounded-t-2xl z-10">
              <h2 className="text-2xl font-bold text-purple-900">Chamado: {chamadoSelecionado.id}</h2>
              <button
                onClick={handleFecharDetalhes}
                className="text-gray-500 hover:text-gray-700 transition"
              >
                <X size={24} />
              </button>
            </div>

            {/* Conteúdo */}
            <div className="p-6 space-y-6">
              {/* Datas */}
              <div className="text-sm text-gray-700 space-y-1">
                <p>Chamado aberto em: {chamadoSelecionado.abertaEm}</p>
                <p>Última atualização: {chamadoSelecionado.ultimaAtualizacao}</p>
              </div>

              {/* Status e Aberto há - NO TOPO, LADO A LADO */}
              <div className="flex justify-between items-start gap-8">
                <div>
                  <p className="text-sm text-gray-600 mb-1"><strong>Status:</strong></p>
                  <p className={`font-bold text-lg ${chamadoSelecionado.status === "Resolvido" ? "text-green-600" : "text-yellow-600"}`}>
                    {chamadoSelecionado.status}
                  </p>
                </div>
                <div>
                  <p className="text-sm text-gray-600 mb-1"><strong>Aberto há:</strong></p>
                  <p className="font-bold text-lg text-gray-900">{chamadoSelecionado.abertohá}</p>
                </div>
              </div>

              {/* Grid 2 colunas */}
              <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
                {/* COLUNA ESQUERDA */}
                <div className="space-y-4">
                  <div className="space-y-2">
                    <p className="text-sm"><strong>Nome:</strong> {chamadoSelecionado.usuario}</p>
                    <p className="text-sm"><strong>Setor:</strong> {chamadoSelecionado.setor}</p>
                  </div>

                  <div>
                    <p className="text-sm font-bold text-purple-600 mb-2">Descrição do problema:</p>
                    <div className="bg-gray-100 p-4 rounded text-sm text-gray-700">
                      {chamadoSelecionado.descricao}
                    </div>
                  </div>

                  <div>
                    <p className="text-sm font-bold text-purple-600 mb-2">Categoria:</p>
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
                      <p className="text-gray-600">Prioridade: <strong className="text-red-600">{chamadoSelecionado.prioridade}</strong></p>
                      <p className="text-gray-600">SLA: <strong>{chamadoSelecionado.sla}</strong></p>
                    </div>
                  </div>

                  {/* DETALHES ADICIONAIS - ANEXOS */}
                  {chamadoSelecionado.anexos && chamadoSelecionado.anexos.length > 0 && (
                    <div>
                      <p className="text-sm font-bold text-purple-600 mb-2">Detalhes Adicionais</p>
                      <div className="bg-gray-100 p-4 rounded space-y-2">
                        {chamadoSelecionado.anexos.map((anexo, index) => (
                          <div key={index} className="flex items-center justify-between gap-2">
                            <div className="flex items-center gap-2">
                              <span className="text-purple-600">📎</span>
                              <div className="text-sm">
                                <p className="text-gray-700 font-semibold">{anexo.nome}</p>
                                <p className="text-gray-500 text-xs">{anexo.tamanho}</p>
                              </div>
                            </div>
                            <button className="text-purple-600 hover:text-purple-700 transition">
                              <Download size={16} />
                            </button>
                          </div>
                        ))}
                      </div>
                    </div>
                  )}
                </div>

                {/* COLUNA DIREITA */}
                <div className="space-y-6">
                  {/* Técnico Responsável - EDITÁVEL SE EM ANDAMENTO */}
                  <div>
                    <div className="flex justify-between items-center mb-2">
                      <p className="text-sm text-gray-600"><strong>Técnico responsável:</strong></p>
                      {chamadoSelecionado.status === "Em andamento" && !isEditingTecnico && (
                        <button
                          onClick={() => setIsEditingTecnico(true)}
                          className="text-purple-600 hover:text-purple-700 text-xs"
                        >
                          ✏️ Editar
                        </button>
                      )}
                    </div>

                    {isEditingTecnico && chamadoSelecionado.status === "Em andamento" ? (
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

                  {/* Solução - EDITÁVEL SE EM ANDAMENTO */}
                  <div>
                    <div className="flex justify-between items-center mb-2">
                      <p className="text-sm text-gray-600"><strong>Solução:</strong></p>
                      {chamadoSelecionado.status === "Em andamento" && !isEditingSolucao && (
                        <button
                          onClick={() => setIsEditingSolucao(true)}
                          className="text-purple-600 hover:text-purple-700 text-xs"
                        >
                          ✏️ Editar
                        </button>
                      )}
                    </div>

                    {isEditingSolucao && chamadoSelecionado.status === "Em andamento" ? (
                      <div className="space-y-2">
                        <textarea
                          value={solucaoEditada}
                          onChange={(e) => setSolucaoEditada(e.target.value)}
                          className="w-full px-4 py-2 border border-gray-300 rounded text-sm text-gray-700 bg-white resize-none"
                          rows={6}
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

                  {/* Botão Encerrar Chamado - APENAS SE EM ANDAMENTO */}
                  {chamadoSelecionado.status === "Em andamento" && (
                    <Button
                      onClick={handleEncerrarChamado}
                      className="w-full bg-purple-600 hover:bg-purple-700 text-white font-semibold py-2 rounded transition"
                    >
                      ✓ Encerrar Chamado
                    </Button>
                  )}
                </div>
              </div>
            </div>
          </Card>
        </div>
      )}
    </div>
  );
}
