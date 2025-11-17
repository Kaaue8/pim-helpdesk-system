import { useState, useEffect } from "react";
import { Search, ChevronDown, X, Download } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Card } from "@/components/ui/card";
import Header from "@/components/Header";
import { useAuth } from "@/contexts/AuthContext";
import { toast } from "sonner";
import CriarChamadoModal from "@/components/CriarChamadoModal";

// Interfaces para tradução dos dados do backend
interface TicketBackend {
  id: number;
  titulo: string;
  descricao: string;
  status: string;
  prioridade: string;
  dataAbertura: string;
  dataFechamento: string | null;
  usuario: { id: number; nome: string; setor: { nomeSetor: string } | null };
  tecnico: { id: number; nome: string } | null;
}

// Interface anexo
interface Anexo {
  nome: string;
  tamanho: string;
  url: string;
}

//interface nova
interface ChamadoFrontend {
  id: string;
  tipo: string;
  tecnico: string;
  prioridade: string;
  sla: string;
  status: string;
  avaliacao: number;
  usuario: string;
  setor: string;
  descricao: string;
  solucao: string;
  abertohá: string;
  abertaEm: string;
  ultimaAtualizacao: string;
  anexos: Anexo[];
}

// Hook para buscar e adaptar os dados
// CORREÇÃO 1: O parâmetro foi renomeado de 'isLoadingAuth' para 'isAuthLoading' para clareza.
const useChamadosData = (token: string | null, isAuthLoading: boolean) => {
  const [chamados, setChamados] = useState<ChamadoFrontend[]>([]);
  const [tecnicos, setTecnicos] = useState<any[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchData = async () => {
      // Usa a nova variável 'isAuthLoading'
      if (isAuthLoading) return;
      if (!token && !isAuthLoading) {
        toast.error("Sessão expirada. Por favor, faça login novamente.");
        setIsLoading(false);
        return;
      }
      setIsLoading(true);
      try {
        const [chamadosResponse, usuariosResponse] = await Promise.all([
          fetch('http://localhost:5079/api/Tickets', { headers: { 'Authorization': `Bearer ${token}` } }  ),
          fetch('http://localhost:5079/api/Usuarios', { headers: { 'Authorization': `Bearer ${token}` } }  )
        ]);

        if (!chamadosResponse.ok || !usuariosResponse.ok) {
          throw new Error('Falha na comunicação com o servidor.');
        }
        
        const dadosDoBackend: TicketBackend[] = await chamadosResponse.json();
        const dadosAdaptados = dadosDoBackend.map((ticket) => {
          const formatarData = (d: string | null) => !d ? "-" : new Date(d).toLocaleString('pt-BR', { dateStyle: 'short', timeStyle: 'short' });
          return {
            id: `CH-${String(ticket.id).padStart(4, '0')}`,
            tipo: ticket.titulo,
            tecnico: ticket.tecnico?.nome || "Não atribuído",
            prioridade: ticket.prioridade,
            sla: "4h", // Mockado
            status: ticket.status,
            avaliacao: 0, // Mockado
            usuario: ticket.usuario.nome,
            setor: ticket.usuario.setor?.nomeSetor || "N/A",
            descricao: ticket.descricao,
            solucao: "", // Mockado
            abertohá: "-", // Mockado
            abertaEm: formatarData(ticket.dataAbertura),
            ultimaAtualizacao: formatarData(ticket.dataFechamento),
            anexos: [], // Mockado
          };
        });
        setChamados(dadosAdaptados);

        const todosUsuarios: any[] = await usuariosResponse.json();
        // Corrigido para buscar por 'perfil' que é o nome da propriedade vinda do backend
        setTecnicos(todosUsuarios.filter(u => u.perfil?.toLowerCase() === 'admin' || u.perfil?.toLowerCase() === 'analista'));
      } catch (error) {
        toast.error((error as Error).message);
      } finally {
        setIsLoading(false);
      }
    };
    fetchData();
  }, [token, isAuthLoading]); // Depende da nova variável

  return { chamados, tecnicos, isLoading };
};

export default function HistoricoChamados() {
  // CORREÇÃO 2: Trocado 'isLoadingAuth' por 'isLoading', que é o nome correto vindo do useAuth()
  const { userName, token, isLoading: isAuthLoading } = useAuth();
  
  // CORREÇÃO 3: Passando 'isAuthLoading' para o hook e renomeando o 'isLoading' retornado para 'isLoadingChamados' para evitar conflito.
  const { chamados, tecnicos, isLoading: isLoadingChamados } = useChamadosData(token, isAuthLoading);
  
  const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);
  const [searchTerm, setSearchTerm] = useState("");
  const [filterTipo, setFilterTipo] = useState("Todos");
  const [filterStatus, setFilterStatus] = useState("Todos");
  const [filterSLA, setFilterSLA] = useState("Todos");
  const [filterTecnico, setFilterTecnico] = useState("Todos");
  const [filterPrioridade, setFilterPrioridade] = useState("Todos");
  const [filterUsuario, setFilterUsuario] = useState("Todos");
  const [filterAvaliacao, setFilterAvaliacao] = useState("Todos");
  const [isDetalhesOpen, setIsDetalhesOpen] = useState(false);
  const [chamadoSelecionado, setChamadoSelecionado] = useState<ChamadoFrontend | null>(null);
  const [tecnicoEditado, setTecnicoEditado] = useState("");
  const [isEditingTecnico, setIsEditingTecnico] = useState(false);
  const [solucaoEditada, setSolucaoEditada] = useState("");
  const [isEditingSolucao, setIsEditingSolucao] = useState(false);

  const chamadosFiltrados = chamados.filter((chamado) => {
    if (chamado.status === "Aberto") return false;
    const matchSearch = chamado.id.toLowerCase().includes(searchTerm.toLowerCase()) || chamado.tipo.toLowerCase().includes(searchTerm.toLowerCase());
    const matchTipo = filterTipo === "Todos" || chamado.tipo === filterTipo;
    const matchStatus = filterStatus === "Todos" || chamado.status === filterStatus;
    const matchTecnico = filterTecnico === "Todos" || chamado.tecnico === filterTecnico;
    const matchPrioridade = filterPrioridade === "Todos" || chamado.prioridade === filterPrioridade;
    const matchUsuario = filterUsuario === "Todos" || chamado.usuario === filterUsuario;
    const matchAvaliacao = filterAvaliacao === "Todos" || (filterAvaliacao === "Avaliado" && chamado.avaliacao > 0) || (filterAvaliacao === "Pendente" && chamado.avaliacao === 0);
    return (matchSearch && matchTipo && matchStatus && matchTecnico && matchPrioridade && matchUsuario && matchAvaliacao);
  });

  const getStatusColor = (status: string) => {
    switch (status) {
      case "Resolvido": return "bg-green-100 text-green-800";
      case "Em andamento": return "bg-yellow-100 text-yellow-800";
      case "Aberto": return "bg-red-100 text-red-800";
      default: return "bg-gray-100 text-gray-800";
    }
  };

  const getSLAColor = (sla: string) => {
    const hours = parseInt(sla);
    if (hours <= 4) return "bg-green-100 text-green-800";
    if (hours <= 8) return "bg-yellow-100 text-yellow-800";
    return "bg-red-100 text-red-800";
  };

  const handleAbrirDetalhes = (chamado: ChamadoFrontend) => {
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

  const usuariosUnicos = Array.from(new Set(chamados.map((c) => c.usuario)));

  // O Header agora pode usar o 'userName' real vindo do contexto de autenticação
  return (
    <div className="min-h-screen bg-gray-50">
      <Header userName={userName} />

      <div className="max-w-7xl mx-auto px-4 py-8">
        <div className="flex justify-between items-center mb-8">
          <h1 className="text-4xl font-bold text-purple-900">HISTÓRICO DE CHAMADOS</h1>
          <Button
            onClick={() => setIsCreateModalOpen(true)}
            className="bg-purple-600 hover:bg-purple-700 text-white font-semibold py-3 px-6 rounded-full transition"
          >
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
                    {tecnicos.map((tecnico) => (
                      <option key={tecnico.id} value={tecnico.nome}>
                        {tecnico.nome}
                      </option>
                    ))}
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

        {/* Adicionado um estado de carregamento para a tabela */}
        {isLoadingChamados ? (
          <div className="text-center py-10">
            <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-purple-600 mx-auto mb-4"></div>
            <p className="text-gray-500">Carregando chamados...</p>
          </div>
        ) : (
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
        )}
      </div>

      {/* MODAL DE DETALHES */}
      {isDetalhesOpen && chamadoSelecionado && (
        <div className="fixed inset-0 bg-black/50 flex items-center justify-center p-4 z-50 overflow-y-auto">
          <Card className="bg-white rounded-2xl shadow-2xl max-w-4xl w-full my-8 max-h-[90vh] overflow-y-auto">
            <div className="sticky top-0 bg-white p-6 flex justify-between items-center border-b pb-4 rounded-t-2xl z-10">
              <h2 className="text-2xl font-bold text-purple-900">Chamado: {chamadoSelecionado.id}</h2>
              <button onClick={handleFecharDetalhes} className="text-gray-500 hover:text-gray-700 transition">
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
                        {chamadoSelecionado.anexos.map((anexo: Anexo, index: number) => (
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
                        <button onClick={() => setIsEditingTecnico(true)} className="text-purple-600 hover:text-purple-700 text-xs">
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
                          <option value="">Selecione um técnico...</option>
                          {tecnicos.map((tecnico) => (
                            <option key={tecnico.id} value={tecnico.nome}>
                              {tecnico.nome} ({tecnico.perfil})
                            </option>
                          ))}
                        </select>
                        <div className="flex gap-2">
                          <Button onClick={handleSalvarTecnico} className="flex-1 bg-purple-600 hover:bg-purple-700 text-white text-sm py-1 rounded transition">
                            Salvar
                          </Button>
                          <Button onClick={() => setIsEditingTecnico(false)} className="flex-1 bg-gray-300 hover:bg-gray-400 text-gray-900 text-sm py-1 rounded transition">
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
                        <button onClick={() => setIsEditingSolucao(true)} className="text-purple-600 hover:text-purple-700 text-xs">
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
                          <Button onClick={handleSalvarSolucao} className="flex-1 bg-purple-600 hover:bg-purple-700 text-white text-sm py-1 rounded transition">
                            Salvar
                          </Button>
                          <Button onClick={() => setIsEditingSolucao(false)} className="flex-1 bg-gray-300 hover:bg-gray-400 text-gray-900 text-sm py-1 rounded transition">
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
                    <Button onClick={handleEncerrarChamado} className="w-full bg-purple-600 hover:bg-purple-700 text-white font-semibold py-2 rounded transition">
                      ✓ Encerrar Chamado
                    </Button>
                  )}
                </div>
              </div>
            </div>
          </Card>
        </div>
      )}

      <CriarChamadoModal
        isOpen={isCreateModalOpen}
        onClose={() => setIsCreateModalOpen(false)}
      />
    </div>
  );
}
