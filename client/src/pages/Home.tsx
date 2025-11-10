import { useState } from "react";
import { Card } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import Header from "@/components/Header";
import DonutChart from "@/components/DonutChart";
import CriarChamadoModal from "@/components/CriarChamadoModal";
import { useAuth } from "@/contexts/AuthContext";

export default function Home() {
  const { userType, userName } = useAuth();
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [modalTipo, setModalTipo] = useState<"problema" | "solicitacao">();

  // Mock data
  const ticketStats = {
    aberto: 1,
    andamento: 3,
    resolvido: 2,
    total: 6,
  };

  const recentTickets = [
    {
      id: "CH-0005",
      user: "Roberto Carlos",
      type: "Sem conexão com a internet",
      priority: "3 - Alta",
      openTime: "30min",
      sla: "4h",
    },
    {
      id: "CH-0004",
      user: "Maria Silva",
      type: "Problema com email",
      priority: "2 - Média",
      openTime: "2h",
      sla: "8h",
    },
    {
      id: "CH-0003",
      user: "João Santos",
      type: "Dúvida sobre sistema",
      priority: "1 - Baixa",
      openTime: "5h",
      sla: "24h",
    },
  ];

  const userTickets = [
    {
      id: "CH-0003",
      tipo: "Criação/alteração de acessos",
      status: "Em andamento",
    },
    {
      id: "CH-0002",
      tipo: "Solicitação de equipamento",
      status: "Em andamento",
    },
    {
      id: "CH-0001",
      tipo: "Criação/alteração de acessos",
      status: "Resolvido",
    },
  ];

  const handleAbrirChamado = (tipo: "problema" | "solicitacao") => {
    setModalTipo(tipo);
    setIsModalOpen(true);
  };

  // ADMIN VIEW
  if (userType === "admin") {
    return (
      <div className="min-h-screen bg-gray-100">
        {/* Header */}
        <Header userName={userName} />

        {/* Main Content */}
        <main className="max-w-7xl mx-auto px-4 py-8">
          {/* Welcome Title */}
          <div className="mb-8">
            <h1 className="text-4xl font-bold text-purple-900 text-center mb-2">
              BEM VINDO AO HELPCENTER
            </h1>
          </div>

          {/* Top Section - Gráfico + Legenda */}
          <Card className="bg-white rounded-2xl shadow-md p-6 mb-8">
            <div className="grid grid-cols-1 lg:grid-cols-2 gap-8 items-center">
              {/* Gráfico */}
              <div className="flex justify-center">
                <div className="w-56 h-56">
                  <DonutChart data={ticketStats} />
                </div>
              </div>

              {/* Legenda */}
              <div className="bg-purple-100 rounded-xl p-6">
                <h3 className="text-lg font-bold text-purple-900 mb-4">
                  Resumo de Chamados
                </h3>
                <div className="space-y-3">
                  <div className="flex items-center gap-2">
                    <span className="text-sm font-medium text-gray-900">
                      Total de chamados: {ticketStats.total}
                    </span>
                  </div>
                  <div className="flex items-center gap-2">
                    <div className="w-4 h-4 bg-purple-300 rounded"></div>
                    <span className="text-sm text-gray-700">
                      Em aberto: {ticketStats.aberto} ({Math.round((ticketStats.aberto / ticketStats.total) * 100)}%)
                    </span>
                  </div>
                  <div className="flex items-center gap-2">
                    <div className="w-4 h-4 bg-purple-600 rounded"></div>
                    <span className="text-sm text-gray-700">
                      Em andamento: {ticketStats.andamento} ({Math.round((ticketStats.andamento / ticketStats.total) * 100)}%)
                    </span>
                  </div>
                  <div className="flex items-center gap-2">
                    <div className="w-4 h-4 bg-purple-900 rounded"></div>
                    <span className="text-sm text-gray-700">
                      Resolvido: {ticketStats.resolvido} ({Math.round((ticketStats.resolvido / ticketStats.total) * 100)}%)
                    </span>
                  </div>
                </div>
              </div>
            </div>
          </Card>

          {/* Fila de Chamados Table */}
          <Card className="bg-white rounded-2xl shadow-md overflow-hidden">
            <div className="p-6">
              <h2 className="text-lg font-bold text-gray-900 mb-6">
                Acesso Rápido - Fila de Chamados
              </h2>

              {/* Table */}
              <div className="overflow-x-auto">
                <table className="w-full">
                  <thead>
                    <tr className="border-b-2 border-gray-300">
                      <th className="text-left py-3 px-4 font-semibold text-gray-900">
                        Chamado
                      </th>
                      <th className="text-left py-3 px-4 font-semibold text-gray-900">
                        Usuário
                      </th>
                      <th className="text-left py-3 px-4 font-semibold text-gray-900">
                        Tipo
                      </th>
                      <th className="text-left py-3 px-4 font-semibold text-gray-900">
                        Prioridade
                      </th>
                      <th className="text-left py-3 px-4 font-semibold text-gray-900">
                        Aberto há
                      </th>
                      <th className="text-left py-3 px-4 font-semibold text-gray-900">
                        SLA
                      </th>
                    </tr>
                  </thead>
                  <tbody>
                    {recentTickets.map((ticket, index) => (
                      <tr
                        key={ticket.id}
                        className={`border-b border-gray-200 hover:bg-gray-50 transition ${
                          index === 0 ? "bg-gray-100" : ""
                        }`}
                      >
                        <td className="py-4 px-4 font-semibold text-gray-900">
                          {ticket.id}
                        </td>
                        <td className="py-4 px-4 text-gray-700">{ticket.user}</td>
                        <td className="py-4 px-4 text-gray-700">{ticket.type}</td>
                        <td className="py-4 px-4 text-gray-700">
                          {ticket.priority}
                        </td>
                        <td className="py-4 px-4 text-gray-700">{ticket.openTime}</td>
                        <td className="py-4 px-4 text-gray-700">
                          {ticket.sla}
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            </div>
          </Card>
        </main>
      </div>
    );
  }

  // USER VIEW
  return (
    <div className="min-h-screen bg-gray-100">
      {/* Header */}
      <Header userName={userName} />

      {/* Main Content */}
      <main className="max-w-7xl mx-auto px-4 py-8">
        {/* Welcome Title */}
        <div className="mb-8">
          <h1 className="text-4xl font-bold text-purple-900 text-center mb-2">
            BEM VINDO AO HELPCENTER
          </h1>
          <p className="text-center text-gray-600">
            Como podemos ajudá-lo hoje?
          </p>
        </div>

        {/* White Container */}
        <div className="bg-white rounded-2xl shadow-md p-8 space-y-8">
          {/* Acesso Rápido Section */}
          <div>
            <h2 className="text-lg font-bold text-gray-900 mb-6">
              Acesso rápido
            </h2>

            {/* Quick Access Blocks */}
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              {/* Block 1: Algo não funciona */}
              <Card 
                onClick={() => handleAbrirChamado("problema")}
                className="bg-gray-100 p-8 rounded-xl shadow-sm hover:shadow-md transition cursor-pointer border border-gray-200"
              >
                <div className="flex items-start gap-4">
                  <div className="text-2xl">🔧</div>
                  <div>
                    <h3 className="text-lg font-bold text-gray-900 mb-2">
                      Algo não funciona
                    </h3>
                    <p className="text-sm text-gray-700">
                      Problemas que estão causando interrupção em seu ambiente
                    </p>
                    <p className="text-xs text-gray-600 mt-2">
                      Ex: Internet, cabeamento.
                    </p>
                  </div>
                </div>
              </Card>

              {/* Block 2: Solicite Algo */}
              <Card 
                onClick={() => handleAbrirChamado("solicitacao")}
                className="bg-gray-100 p-8 rounded-xl shadow-sm hover:shadow-md transition cursor-pointer border border-gray-200"
              >
                <div className="flex items-start gap-4">
                  <div className="text-2xl">📋</div>
                  <div>
                    <h3 className="text-lg font-bold text-gray-900 mb-2">
                      Solicite Algo
                    </h3>
                    <p className="text-sm text-gray-700">
                      Criação ou alteração de acessos, reset de senhas, configurações de e-mail, problemas sistêmicos.
                    </p>
                  </div>
                </div>
              </Card>
            </div>
          </div>

          {/* Chamados Recentes Section */}
          <div className="border-t border-gray-200 pt-8">
            <h2 className="text-lg font-bold text-gray-900 mb-6">
              CHAMADOS RECENTES
            </h2>

            {/* Table */}
            <div className="overflow-x-auto">
              <table className="w-full">
                <thead>
                  <tr className="border-b border-gray-300">
                    <th className="text-left py-3 px-4 font-semibold text-gray-900">
                      Chamado
                    </th>
                    <th className="text-left py-3 px-4 font-semibold text-gray-900">
                      Tipo
                    </th>
                    <th className="text-left py-3 px-4 font-semibold text-gray-900">
                      Status
                    </th>
                  </tr>
                </thead>
                <tbody>
                  {userTickets.map((ticket) => (
                    <tr
                      key={ticket.id}
                      className="border-b border-gray-200 hover:bg-gray-50 transition"
                    >
                      <td className="py-4 px-4 font-medium text-gray-900">
                        {ticket.id}
                      </td>
                      <td className="py-4 px-4 text-gray-700">
                        {ticket.tipo}
                      </td>
                      <td className="py-4 px-4 text-gray-700">
                        {ticket.status}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        </div>
      </main>

      {/* Modal */}
      <CriarChamadoModal 
        isOpen={isModalOpen} 
        onClose={() => setIsModalOpen(false)}
        tipo={modalTipo}
      />
    </div>
  );
}
