import { useState, useMemo } from "react";
import Header from "@/components/Header";
import { useAuth } from "@/contexts/AuthContext";
import DashboardStatusCard from "@/components/DashboardStatusCard";
import DashboardLineChart from "@/components/DashboardLineChart";
import DonutChart from "@/components/DonutChart";
import DashboardBarChart from "@/components/DashboardBarChart";
import DashboardTable from "@/components/DashboardTable";
import { Card } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { BarChart3, TrendingUp, Clock, CheckCircle } from "lucide-react";

// Dados dos gráficos
const chamadosPorDiaCompleto = [
  { dia: "01/10", chamados: 2 },
  { dia: "02/10", chamados: 1 },
  { dia: "03/10", chamados: 3 },
  { dia: "04/10", chamados: 2 },
  { dia: "05/10", chamados: 4 },
  { dia: "06/10", chamados: 3 },
  { dia: "07/10", chamados: 5 },
  { dia: "08/10", chamados: 4 },
  { dia: "09/10", chamados: 2 },
  { dia: "10/10", chamados: 3 },
  { dia: "11/10", chamados: 4 },
  { dia: "12/10", chamados: 2 },
  { dia: "13/11", chamados: 3 },
  { dia: "14/11", chamados: 1 },
  { dia: "15/11", chamados: 2 },
  { dia: "16/11", chamados: 3 },
];

const statusDataCompleto = {
  aberto: 5,
  andamento: 8,
  resolvido: 12,
  total: 25,
};

const volumetriaPorTipoCompleto = [
  { tipo: "Criação/Alteração de acessos", volume: 10 },
  { tipo: "Sem conexão com a internet", volume: 8 },
  { tipo: "Solicitação de equipamento", volume: 7 },
];

const entregaNominalCompleto = [
  { tecnico: "Loro José", tickets: 8, sla: "5h", avaliacao: "4 Estrelas" },
  { tecnico: "João Alves", tickets: 7, sla: "4h", avaliacao: "3 Estrelas" },
  { tecnico: "Maria Silva", tickets: 10, sla: "6h", avaliacao: "5 Estrelas" },
];

// Dados por técnico
const dadosPorTecnico = {
  loro: {
    chamadosPorDia: chamadosPorDiaCompleto.filter((_, i) => i % 2 === 0),
    status: { aberto: 2, andamento: 3, resolvido: 3, total: 8 },
    volumetria: [
      { tipo: "Criação/Alteração de acessos", volume: 4 },
      { tipo: "Sem conexão com a internet", volume: 2 },
      { tipo: "Solicitação de equipamento", volume: 2 },
    ],
  },
  joao: {
    chamadosPorDia: chamadosPorDiaCompleto.filter((_, i) => i % 2 === 1),
    status: { aberto: 3, andamento: 5, resolvido: 9, total: 17 },
    volumetria: [
      { tipo: "Criação/Alteração de acessos", volume: 6 },
      { tipo: "Sem conexão com a internet", volume: 6 },
      { tipo: "Solicitação de equipamento", volume: 5 },
    ],
  },
};

// Dados por tipo
const dadosPorTipo = {
  acesso: {
    chamadosPorDia: chamadosPorDiaCompleto.map((d) => ({ ...d, chamados: Math.floor(Math.random() * 5) + 1 })),
    status: { aberto: 2, andamento: 3, resolvido: 5, total: 10 },
  },
  internet: {
    chamadosPorDia: chamadosPorDiaCompleto.map((d) => ({ ...d, chamados: Math.floor(Math.random() * 4) + 1 })),
    status: { aberto: 1, andamento: 2, resolvido: 5, total: 8 },
  },
  equipamento: {
    chamadosPorDia: chamadosPorDiaCompleto.map((d) => ({ ...d, chamados: Math.floor(Math.random() * 3) + 1 })),
    status: { aberto: 2, andamento: 3, resolvido: 2, total: 7 },
  },
};

export default function Dashboard() {
  const { userName } = useAuth();
  const [filtroTecnico, setFiltroTecnico] = useState("");
  const [filtroData, setFiltroData] = useState("");
  const [filtroTipo, setFiltroTipo] = useState("");

  // Dados filtrados
  const dadosFiltrados = useMemo(() => {
    let chamados = chamadosPorDiaCompleto;
    let status = statusDataCompleto;
    let volumetria = volumetriaPorTipoCompleto;
    let entrega = entregaNominalCompleto;

    // Filtro por técnico
    if (filtroTecnico) {
      chamados = dadosPorTecnico[filtroTecnico as keyof typeof dadosPorTecnico]?.chamadosPorDia || chamados;
      status = dadosPorTecnico[filtroTecnico as keyof typeof dadosPorTecnico]?.status || status;
      volumetria = dadosPorTecnico[filtroTecnico as keyof typeof dadosPorTecnico]?.volumetria || volumetria;
      entrega = entregaNominalCompleto.filter((e) => {
        if (filtroTecnico === "loro") return e.tecnico === "Loro José";
        if (filtroTecnico === "joao") return e.tecnico === "João Alves";
        return true;
      });
    }

    // Filtro por tipo
    if (filtroTipo) {
      const dadosTipo = dadosPorTipo[filtroTipo as keyof typeof dadosPorTipo];
      chamados = dadosTipo?.chamadosPorDia || chamados;
      status = dadosTipo?.status || status;
    }

    // Filtro por data (filtra a partir da data selecionada até hoje)
    if (filtroData) {
      chamados = chamados.filter((d) => {
        const data = new Date(d.dia.split("/").reverse().join("-"));
        const dataFiltro = new Date(filtroData);
        const hoje = new Date();
        return data >= dataFiltro && data <= hoje;
      });
    }

    return { chamados, status, volumetria, entrega };
  }, [filtroTecnico, filtroData, filtroTipo]);

  const statusCards = [
    {
      label: "Total",
      value: dadosFiltrados.status.total,
      icon: CheckCircle,
      colorClass: "from-purple-200 to-purple-100 text-purple-900",
    },
    {
      label: "Em aberto",
      value: dadosFiltrados.status.aberto,
      icon: BarChart3,
      colorClass: "from-orange-200 to-orange-100 text-orange-900",
    },
    {
      label: "Andamento",
      value: dadosFiltrados.status.andamento,
      icon: TrendingUp,
      colorClass: "from-yellow-200 to-yellow-100 text-yellow-900",
    },
    {
      label: "Resolvido",
      value: dadosFiltrados.status.resolvido,
      icon: CheckCircle,
      colorClass: "from-green-200 to-green-100 text-green-900",
    },
  ];

  const handleFiltrar = () => {
    console.log("Filtros aplicados:", { filtroTecnico, filtroData, filtroTipo });
  };


  return (
    <div className="min-h-screen bg-gray-100">
      <Header userName={userName} />

      <div className="max-w-7xl mx-auto px-4 py-8">
        {/* Título */}
        <h1 className="text-4xl font-bold text-purple-900 text-center mb-4">
          DASHBOARD CHAMADOS
        </h1>

        {/* Cards de Status + Filtros */}
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-4 mb-6">
          {/* Cards de Status - Lado Esquerdo */}
          <div className="lg:col-span-2">
            <Card className="p-4 bg-white border-2 border-gray-300 rounded-xl">
              <h3 className="text-base font-bold text-gray-800 mb-3">
                Chamados por status
              </h3>
              <div className="grid grid-cols-2 md:grid-cols-4 gap-3">
                {statusCards.map((card, index) => (
                  <div key={index} className="flex justify-center">
                    <div className="w-full">
                      <DashboardStatusCard
                        label={card.label}
                        value={card.value}
                        icon={card.icon}
                        colorClass={card.colorClass}
                      />
                    </div>
                  </div>
                ))}
              </div>
            </Card>
          </div>

          {/* Filtros - Lado Direito */}
          <div>
            <Card className="p-4 bg-gray-200 rounded-xl h-full">
              <div className="space-y-3">
                <div>
                  <label className="block text-xs font-semibold text-gray-700 mb-1">
                    Técnico
                  </label>
                  <select
                    value={filtroTecnico}
                    onChange={(e) => setFiltroTecnico(e.target.value)}
                    className="w-full px-2 py-1 border border-gray-300 rounded-lg bg-white text-gray-700 text-xs cursor-pointer"
                  >
                    <option value="">Todos</option>
                    <option value="loro">Loro José</option>
                    <option value="joao">João Alves</option>
                  </select>
                </div>

                <div>
                  <label className="block text-xs font-semibold text-gray-700 mb-1">
                    Data
                  </label>
                  <input
                    type="date"
                    value={filtroData}
                    onChange={(e) => setFiltroData(e.target.value)}
                    className="w-full px-2 py-1 border border-gray-300 rounded-lg bg-white text-gray-700 text-xs cursor-pointer"
                  />
                </div>

                <div>
                  <label className="block text-xs font-semibold text-gray-700 mb-1">
                    Tipo
                  </label>
                  <select
                    value={filtroTipo}
                    onChange={(e) => setFiltroTipo(e.target.value)}
                    className="w-full px-2 py-1 border border-gray-300 rounded-lg bg-white text-gray-700 text-xs cursor-pointer"
                  >
                    <option value="">Todos</option>
                    <option value="acesso">Criação/Alteração de acessos</option>
                    <option value="internet">Sem conexão com a internet</option>
                    <option value="equipamento">Solicitação de equipamento</option>
                  </select>
                </div>

              </div>
            </Card>
          </div>
        </div>

        {/* Gráficos e Tabela */}
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
          {/* Chamados por dia */}
          <DashboardLineChart title="Chamados por dia" data={dadosFiltrados.chamados} />

          {/* Chamados por status (Rosca) */}
          <div className="bg-gray-200 p-6 rounded-2xl">
            <Card className="p-2 bg-white rounded-xl h-full flex flex-col items-center justify-center">
              <h2 className="text-xl font-bold text-gray-800 mb-4 w-full">
                Chamados por status
              </h2>
              <DonutChart data={dadosFiltrados.status} />
              <div className="mt-4 space-y-2 text-sm">
                <div className="flex items-center gap-2">
                  <div
                    className="w-3 h-3 rounded-full"
                    style={{ backgroundColor: "#e9d5ff" }}
                  ></div>
                  <span className="text-gray-700">
                    Em aberto: {dadosFiltrados.status.aberto} ({Math.round((dadosFiltrados.status.aberto / dadosFiltrados.status.total) * 100)}%)
                  </span>
                </div>
                <div className="flex items-center gap-2">
                  <div
                    className="w-3 h-3 rounded-full"
                    style={{ backgroundColor: "#a855f7" }}
                  ></div>
                  <span className="text-gray-700">
                    Andamento: {dadosFiltrados.status.andamento} ({Math.round((dadosFiltrados.status.andamento / dadosFiltrados.status.total) * 100)}%)
                  </span>
                </div>
                <div className="flex items-center gap-2">
                  <div
                    className="w-3 h-3 rounded-full"
                    style={{ backgroundColor: "#6b21a8" }}
                  ></div>
                  <span className="text-gray-700">
                    Resolvido: {dadosFiltrados.status.resolvido} ({Math.round((dadosFiltrados.status.resolvido / dadosFiltrados.status.total) * 100)}%)
                  </span>
                </div>
              </div>
            </Card>
          </div>

          {/* Volumetria por tipo */}
          <DashboardBarChart
            title="Volumetria por tipo"
            data={dadosFiltrados.volumetria}
          />

          {/* Entrega Nominal - Média */}
          <DashboardTable
            title="Entrega Nominal – Média"
            data={dadosFiltrados.entrega}
          />
        </div>
      </div>
    </div>
  );
}
