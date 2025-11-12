import { useState } from "react";
import { Search, ChevronDown, CheckCircle } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Card } from "@/components/ui/card";
import Header from "@/components/Header";
import CriarChamadoModal from "@/components/CriarChamadoModal";

// Dados de exemplo - chamados do usuário
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
  },
];

export default function TodosChamados() {
  const [searchTerm, setSearchTerm] = useState("");
  const [filterTipo, setFilterTipo] = useState("Todos");
  const [filterStatus, setFilterStatus] = useState("Todos");
  const [filterSLA, setFilterSLA] = useState("Todos");
  const [filterTecnico, setFilterTecnico] = useState("Todos");
  const [filterPrioridade, setFilterPrioridade] = useState("Todos");
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isDetalhesOpen, setIsDetalhesOpen] = useState(false);
  const [isAvaliacaoSucesso, setIsAvaliacaoSucesso] = useState(false);
  const [chamadoSelecionado, setChamadoSelecionado] = useState<typeof CHAMADOS_EXEMPLO[0] | null>(null);
  const [avaliacaoText, setAvaliacaoText] = useState("");
  const [avaliacaoStars, setAvaliacaoStars] = useState(0);

  // Filtrar chamados
  const chamadosFiltrados = CHAMADOS_EXEMPLO.filter((chamado) => {
    const matchSearch =
      chamado.id.toLowerCase().includes(searchTerm.toLowerCase()) ||
      chamado.tipo.toLowerCase().includes(searchTerm.toLowerCase());

    const matchTipo = filterTipo === "Todos" || chamado.tipo === filterTipo;
    const matchStatus = filterStatus === "Todos" || chamado.status === filterStatus;
    const matchTecnico = filterTecnico === "Todos" || chamado.tecnico === filterTecnico;
    const matchPrioridade = filterPrioridade === "Todos" || chamado.prioridade === filterPrioridade;

    return matchSearch && matchTipo && matchStatus && matchTecnico && matchPrioridade;
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

  const handleEnviarAvaliacao = () => {
    if (avaliacaoStars > 0) {
      setIsAvaliacaoSucesso(true);
    }
  };

  const handleVoltarAosChamados = () => {
    setIsDetalhesOpen(false);
    setIsAvaliacaoSucesso(false);
  };

  const handleAbrirDetalhes = (chamado: typeof CHAMADOS_EXEMPLO[0]) => {
    setChamadoSelecionado(chamado);
    setAvaliacaoText("");
    setAvaliacaoStars(chamado.avaliacao || 0);
    setIsDetalhesOpen(true);
    setIsAvaliacaoSucesso(false);
  };

  return (
    <div className="min-h-screen bg-gray-50">
      <Header userName="User" />

      <div className="max-w-7xl mx-auto px-4 py-8">
        {/* Header Section */}
        <div className="flex justify-between items-center mb-8">
          <h1 className="text-4xl font-bold text-purple-900">TODOS OS CHAMADOS</h1>
          <Button
            onClick={() => setIsModalOpen(true)}
            className="bg-purple-600 hover:bg-purple-700 text-white font-semibold py-3 px-6 rounded-full transition"
          >
            + Novo Chamado
          </Button>
        </div>

        {/* Filtros e Busca */}
        <Card className="mb-6 p-6 bg-white">
          <div className="space-y-4">
            {/* Primeira linha de filtros */}
            <div className="flex flex-wrap gap-4 items-center">
              {/* Filtro Tipo */}
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

              {/* Filtro Status */}
              <div className="relative">
                <select
                  value={filterStatus}
                  onChange={(e) => setFilterStatus(e.target.value)}
                  className="appearance-none px-4 py-2 pr-8 bg-gray-100 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-purple-500 cursor-pointer"
                >
                  <option>Todos</option>
                  <option>Aberto</option>
                  <option>Em andamento</option>
                  <option>Resolvido</option>
                </select>
                <ChevronDown size={16} className="absolute right-2 top-3 text-gray-600 pointer-events-none" />
              </div>

              {/* Filtro SLA */}
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

              {/* Filtro Técnico */}
              <div className="relative">
                <select
                  value={filterTecnico}
                  onChange={(e) => setFilterTecnico(e.target.value)}
                  className="appearance-none px-4 py-2 pr-8 bg-gray-100 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-purple-500 cursor-pointer"
                >
                  <option>Todos</option>
                  <option>Loro José</option>
                  <option>João Alves</option>
                </select>
                <ChevronDown size={16} className="absolute right-2 top-3 text-gray-600 pointer-events-none" />
              </div>

              {/* Filtro Prioridade */}
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
            </div>

            {/* Busca */}
            <div className="flex items-center gap-2">
              <div className="flex-1 relative">
                <input
                  type="text"
                  placeholder="Busque pelo ID do chamado"
                  value={searchTerm}
                  onChange={(e) => setSearchTerm(e.target.value)}
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-purple-500"
                />
              </div>
              <button className="p-2 hover:bg-gray-100 rounded-lg transition">
                <Search size={20} className="text-gray-600" />
              </button>
            </div>
          </div>
        </Card>

        {/* Tabela de Chamados */}
        <Card className="bg-white overflow-hidden">
          <div className="overflow-x-auto">
            <table className="w-full">
              <thead>
                <tr className="bg-gray-100 border-b">
                  <th className="px-4 py-3 text-left text-sm font-semibold text-gray-900">Chamado</th>
                  <th className="px-4 py-3 text-left text-sm font-semibold text-gray-900">Tipo</th>
                  <th className="px-4 py-3 text-left text-sm font-semibold text-gray-900">Técnico</th>
                  <th className="px-4 py-3 text-left text-sm font-semibold text-gray-900">Prioridade</th>
                  <th className="px-4 py-3 text-left text-sm font-semibold text-gray-900">Aberto há</th>
                  <th className="px-4 py-3 text-left text-sm font-semibold text-gray-900">SLA Atual</th>
                  <th className="px-4 py-3 text-left text-sm font-semibold text-gray-900">Status</th>
                  <th className="px-4 py-3 text-left text-sm font-semibold text-gray-900">Avaliação</th>
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
                    {/* ⭐ COLUNA DE AVALIAÇÃO */}
                    <td className="px-4 py-3 text-sm">
                      {chamado.status === "Resolvido" ? (
                        chamado.avaliacao > 0 ? (
                          <CheckCircle size={20} className="text-purple-600" />
                        ) : (
                          <span className="text-gray-500 text-xs font-semibold">Pendente</span>
                        )
                      ) : (
                        <span className="text-gray-300 text-xs">-</span>
                      )}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </Card>
      </div>

      {/* Modal de Novo Chamado */}
      {isModalOpen && <CriarChamadoModal onClose={() => setIsModalOpen(false)} isOpen={false} />}

      {/* Modal de Detalhes do Chamado - RESPONSIVO 50/50 */}
      {isDetalhesOpen && chamadoSelecionado && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 p-4 overflow-y-auto">
          <Card className="w-full max-w-6xl my-8">
            {/* Header com título e botão fechar */}
            <div className="p-6 flex justify-between items-center border-b pb-4">
              <h2 className="text-2xl font-bold text-purple-900">Chamado: {chamadoSelecionado.id}</h2>
              <button
                onClick={() => setIsDetalhesOpen(false)}
                className="text-gray-600 hover:text-gray-900 p-2 rounded transition"
              >
                ✕
              </button>
            </div>

            {/* Topo - Datas e Status lado a lado */}
            <div className="p-6 grid grid-cols-1 lg:grid-cols-2 gap-6 border-b pb-4">
              {/* Lado esquerdo - Datas */}
              <div className="text-sm text-gray-700 space-y-1">
                <p>Chamado aberto em: {chamadoSelecionado.abertaEm}</p>
                <p>Última atualização: {chamadoSelecionado.ultimaAtualizacao}</p>
              </div>

              {/* Lado direito - Status, Aberto há e Técnico */}
              <div className="flex flex-col sm:flex-row gap-4 sm:gap-6 sm:justify-end">
                <div className="text-sm">
                  <p className="text-gray-600 mb-1"><strong>Status:</strong></p>
                  <p className={`font-bold ${chamadoSelecionado.status === "Resolvido" ? "text-green-600" : chamadoSelecionado.status === "Em andamento" ? "text-yellow-600" : "text-red-600"}`}>
                    {chamadoSelecionado.status}
                  </p>
                </div>
                <div className="text-sm">
                  <p className="text-gray-600 mb-1"><strong>Aberto há:</strong></p>
                  <p className="font-bold text-gray-900">{chamadoSelecionado.abertohá}</p>
                </div>
                <div className="text-sm">
                  <p className="text-gray-600 mb-1"><strong>Técnico responsável:</strong></p>
                  <p className="font-semibold text-gray-900">{chamadoSelecionado.tecnico}</p>
                </div>
              </div>
            </div>

            {/* Conteúdo principal - 50/50 */}
            <div className="p-6 grid grid-cols-1 lg:grid-cols-2 gap-6">
              {/* Lado esquerdo - 50% */}
              <div className="space-y-4">
                {/* Dados do usuário */}
                <div className="space-y-2">
                  <p className="font-semibold text-gray-900"><strong>Nome:</strong> {chamadoSelecionado.usuario}</p>
                  <p className="text-gray-600 text-sm"><strong>Setor:</strong> {chamadoSelecionado.setor}</p>
                </div>

                {/* Descrição */}
                <div>
                  <h3 className="font-bold text-purple-600 mb-2">Descrição do problema:</h3>
                  <p className="text-gray-700 bg-gray-50 p-4 rounded">{chamadoSelecionado.descricao}</p>
                </div>

                {/* Categoria */}
                <div>
                  <label className="block font-semibold text-gray-900 mb-2">Categoria:</label>
                  <div className="relative">
                    <select
                      disabled
                      value={chamadoSelecionado.tipo}
                      className="appearance-none w-full px-4 py-2 pr-8 bg-gray-100 border border-gray-300 rounded-lg cursor-not-allowed"
                    >
                      <option>{chamadoSelecionado.tipo}</option>
                    </select>
                    <ChevronDown size={16} className="absolute right-2 top-3 text-gray-600 pointer-events-none" />
                  </div>
                </div>

                {/* Info box */}
                <div className="bg-gray-100 p-4 rounded grid grid-cols-1 sm:grid-cols-2 gap-4 text-sm">
                  <div>
                    <p className="text-gray-600"><strong>ID Chamado:</strong> {chamadoSelecionado.id}</p>
                    <p className="text-gray-600"><strong>Nível de Suporte:</strong> N1</p>
                  </div>
                  <div>
                    <p className="text-gray-600"><strong>Prioridade:</strong> <span className="font-semibold">{chamadoSelecionado.prioridade}</span></p>
                    <p className="text-gray-600"><strong>SLA:</strong> {chamadoSelecionado.sla}</p>
                  </div>
                </div>

                {/* Detalhes Adicionais */}
                <div>
                  <h3 className="font-bold text-gray-900 mb-2">Detalhes Adicionais</h3>
                  <p className="text-gray-700 bg-gray-50 p-4 rounded">Minha máquina ficou localizada na sala 09, estou disponível na empresa durante o horário comercial. IP Máquina: 192.168.0.101</p>
                </div>

                {/* Anexo */}
                <div className="flex items-center gap-2 text-purple-600">
                  <span>📎</span>
                  <span>Print_Erro_Limpeg</span>
                </div>
              </div>

              {/* Lado direito - 50% */}
              <div className="space-y-4">
                {/* Solução */}
                <div>
                  <label className="block font-semibold text-gray-900 mb-2">Solução:</label>
                  <textarea
                    disabled
                    value={chamadoSelecionado.solucao}
                    className="w-full px-3 sm:px-4 py-2 bg-gray-100 border border-gray-300 rounded-lg h-20 sm:h-24 cursor-not-allowed resize-none text-sm sm:text-base"
                  />
                </div>
                {/* Avaliação - Somente se Resolvido E não foi avaliado */}
                {chamadoSelecionado.status === "Resolvido" && chamadoSelecionado.avaliacao === 0 && (
                  <div className="space-y-4 border-t pt-4">
                    <div>
                      <p className="text-xs sm:text-sm font-semibold text-gray-900 mb-2">Avalie seu atendimento:</p>
                      <div className="flex gap-2 mb-4">
                        {[1, 2, 3, 4, 5].map((star) => (
                          <button
                            key={star}
                            onClick={() => setAvaliacaoStars(star)}
                            className={`text-xl sm:text-2xl transition ${
                              star <= avaliacaoStars ? "text-yellow-400" : "text-gray-300"
                            }`}
                          >
                            ★
                          </button>
                        ))}
                      </div>
                    </div>
                    <textarea
                      placeholder="Deixe seu comentário (opcional)"
                      value={avaliacaoText}
                      onChange={(e) => setAvaliacaoText(e.target.value)}
                      className="w-full px-3 sm:px-4 py-2 border border-gray-300 rounded-lg h-16 sm:h-20 resize-none focus:outline-none focus:ring-2 focus:ring-purple-500 text-sm sm:text-base"
                    />
                    <Button
                      onClick={handleEnviarAvaliacao}
                      disabled={avaliacaoStars === 0}
                      className={`w-full py-2 sm:py-3 rounded-full font-semibold transition text-sm sm:text-base ${
                        avaliacaoStars > 0
                          ? "bg-purple-600 hover:bg-purple-700 text-white cursor-pointer"
                          : "bg-gray-300 text-gray-500 cursor-not-allowed"
                      }`}
                    >
                      Enviar Avaliação
                    </Button>
                  </div>
                )}
                {/* Avaliação já feita - Apenas leitura */}
                {chamadoSelecionado.status === "Resolvido" && chamadoSelecionado.avaliacao > 0 && (
                  <div className="space-y-4 border-t pt-4">
                    <div>
                      <p className="text-xs sm:text-sm font-semibold text-gray-900 mb-2">Sua avaliação:</p>
                      <div className="flex gap-2">
                        {[1, 2, 3, 4, 5].map((star) => (
                          <span
                            key={star}
                            className={`text-xl sm:text-2xl ${
                              star <= chamadoSelecionado.avaliacao ? "text-yellow-400" : "text-gray-300"
                            }`}
                          >
                            ★
                          </span>
                        ))}
                      </div>
                    </div>
                    <p className="text-xs sm:text-sm text-gray-600 italic">Obrigado pela sua avaliação!</p>
                  </div>
                )}
              </div>
            </div>
          </Card>
        </div>
      )}
      {/* Tela de Sucesso - Avaliação Enviada */}
      {isAvaliacaoSucesso && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 p-4">
          <Card className="w-full max-w-md">
            <div className="p-8 flex flex-col items-center text-center space-y-6">
              {/* Check roxo */}
              <div className="w-24 h-24 rounded-full bg-purple-600 flex items-center justify-center">
                <svg className="w-12 h-12 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={3} d="M5 13l4 4L19 7" />
                </svg>
              </div>
              {/* Título */}
              <h2 className="text-2xl sm:text-3xl font-bold text-purple-600">Avaliação enviada!</h2>
              {/* Mensagem */}
              <p className="text-gray-600 text-sm sm:text-base leading-relaxed">
                Obrigado pelo seu feedback! Seu comentário foi registrado e vai nos ajudar a melhorar nossas tratativas!
              </p>
              {/* Botão */}
              <Button
                onClick={handleVoltarAosChamados}
                className="w-full bg-purple-600 hover:bg-purple-700 text-white font-bold py-3 rounded-full transition text-sm sm:text-base"
              >
                Voltar aos chamados
              </Button>
            </div>
          </Card>
        </div>
      )}
    </div>
  );
}
