import { useState } from "react";
import { Search, ChevronDown } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Card } from "@/components/ui/card";
import Header from "@/components/Header";
import CriarChamadoModal from "@/components/CriarChamadoModal";
import { useAuth } from "@/contexts/AuthContext";
import { toast } from "sonner";

const CHAMADOS_FILA = [
  {
    id: "CH-0008",
    usuario: "Roberto Carlos",
    tipo: "Sem conexão com a internet",
    prioridade: "3 - Alta",
    abertohá: "30min",
    sla: "4h",
    status: "Aberto",
  },
  {
    id: "CH-0009",
    usuario: "Pedro Oliveira",
    tipo: "Problema com email",
    prioridade: "2 - Média",
    abertohá: "1h45",
    sla: "8h",
    status: "Aberto",
  },
  {
    id: "CH-0010",
    usuario: "Ana Costa",
    tipo: "Dúvida sobre sistema",
    prioridade: "1 - Baixa",
    abertohá: "2h15",
    sla: "24h",
    status: "Aberto",
  },
];

export default function FilaChamados() {
  const { userName } = useAuth();

  const [searchTerm, setSearchTerm] = useState("");
  const [filterTipo, setFilterTipo] = useState("Todos");
  const [filterStatus, setFilterStatus] = useState("Todos");
  const [filterPrioridade, setFilterPrioridade] = useState("Todos");
  const [filterSLA, setFilterSLA] = useState("Todos");
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [chamados, setChamados] = useState(CHAMADOS_FILA);

  const chamadosFiltrados = chamados.filter((chamado) => {
    const matchSearch =
      chamado.id.toLowerCase().includes(searchTerm.toLowerCase()) ||
      chamado.usuario.toLowerCase().includes(searchTerm.toLowerCase()) ||
      chamado.tipo.toLowerCase().includes(searchTerm.toLowerCase());

    const matchTipo = filterTipo === "Todos" || chamado.tipo === filterTipo;
    const matchStatus = filterStatus === "Todos" || chamado.status === filterStatus;
    const matchPrioridade = filterPrioridade === "Todos" || chamado.prioridade === filterPrioridade;

    return matchSearch && matchTipo && matchStatus && matchPrioridade;
  });

  const handleAssumir = (id: string) => {
    setChamados(chamados.filter((c) => c.id !== id));
    toast.success(`Chamado ${id} assumido com sucesso!`);
  };

  const getSLAColor = (sla: string) => {
    const hours = parseInt(sla);
    if (hours <= 4) return "bg-green-100 text-green-800";
    if (hours <= 8) return "bg-yellow-100 text-yellow-800";
    return "bg-red-100 text-red-800";
  };

  return (
    <div className="min-h-screen bg-gray-50">
      <Header userName={userName} />

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
            {/* Filtros com rótulos */}
            <div className="grid grid-cols-2 md:grid-cols-5 gap-4 items-end">
              <div>
                <label className="block text-sm font-semibold text-gray-700 mb-2">Tipo</label>
                <div className="relative">
                  <select
                    value={filterTipo}
                    onChange={(e) => setFilterTipo(e.target.value)}
                    className="appearance-none w-full px-4 py-2 pr-8 bg-gray-100 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-purple-500 cursor-pointer"
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
              </div>

              <div>
                <label className="block text-sm font-semibold text-gray-700 mb-2">Status</label>
                <div className="relative">
                  <select
                    value={filterStatus}
                    onChange={(e) => setFilterStatus(e.target.value)}
                    className="appearance-none w-full px-4 py-2 pr-8 bg-gray-100 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-purple-500 cursor-pointer"
                  >
                    <option>Todos</option>
                    <option>Aberto</option>
                  </select>
                  <ChevronDown size={16} className="absolute right-2 top-3 text-gray-600 pointer-events-none" />
                </div>
              </div>

              <div>
                <label className="block text-sm font-semibold text-gray-700 mb-2">SLA</label>
                <div className="relative">
                  <select
                    value={filterSLA}
                    onChange={(e) => setFilterSLA(e.target.value)}
                    className="appearance-none w-full px-4 py-2 pr-8 bg-gray-100 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-purple-500 cursor-pointer"
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
              </div>

              <div>
                <label className="block text-sm font-semibold text-gray-700 mb-2">Prioridade</label>
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

              <div className="md:col-span-1 col-span-2">
                <label className="block text-sm font-semibold text-gray-700 mb-2">Buscar</label>
                <div className="flex items-center gap-2">
                  <input
                    type="text"
                    placeholder="ID, Usuário..."
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                    className="flex-1 px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-purple-500"
                  />
                  <button className="p-2 hover:bg-gray-100 rounded-lg transition">
                    <Search size={20} className="text-gray-600" />
                  </button>
                </div>
              </div>
            </div>

            {/* Resumo */}
            <div className="bg-gray-100 px-4 py-3 rounded-lg">
              <p className="text-sm font-semibold text-gray-700">
                Chamados em aberto: <span className="text-purple-600">{String(chamadosFiltrados.length).padStart(2, "0")}</span>
              </p>
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
                  <th className="px-6 py-3 text-left text-sm font-semibold text-gray-900">SLA Atual</th>
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
                        <span className="px-3 py-1 rounded-full text-xs font-semibold bg-orange-100 text-orange-800">
                          {chamado.status}
                        </span>
                      </td>
                      <td className="px-6 py-4">
                        <Button
                          onClick={() => handleAssumir(chamado.id)}
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

      <CriarChamadoModal isOpen={isModalOpen} onClose={() => setIsModalOpen(false)} />
    </div>
  );
}
