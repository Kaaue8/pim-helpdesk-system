import Layout from "@/components/Layout";
import { Card } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { useState } from "react";
import { Users, BarChart3, AlertCircle, Settings } from "lucide-react";

export default function Admin() {
  const [activeTab, setActiveTab] = useState("overview");

  const stats = [
    {
      label: "Total de Usuários",
      value: "1,234",
      change: "+12%",
      icon: Users,
      color: "bg-blue-50 text-blue-600",
    },
    {
      label: "Chamados Abertos",
      value: "89",
      change: "-5%",
      icon: AlertCircle,
      color: "bg-red-50 text-red-600",
    },
    {
      label: "Taxa de Resolução",
      value: "94%",
      change: "+3%",
      icon: BarChart3,
      color: "bg-green-50 text-green-600",
    },
    {
      label: "Tempo Médio",
      value: "4.2h",
      change: "-0.5h",
      icon: Settings,
      color: "bg-purple-50 text-purple-600",
    },
  ];

  const recentUsers = [
    {
      id: 1,
      name: "João Silva",
      email: "joao@example.com",
      status: "Ativo",
      joinDate: "2025-10-20",
    },
    {
      id: 2,
      name: "Maria Santos",
      email: "maria@example.com",
      status: "Ativo",
      joinDate: "2025-10-19",
    },
    {
      id: 3,
      name: "Pedro Costa",
      email: "pedro@example.com",
      status: "Inativo",
      joinDate: "2025-10-18",
    },
  ];

  const categories = [
    { id: 1, name: "Suporte Técnico", tickets: 45, color: "bg-blue-100" },
    { id: 2, name: "Bug Report", tickets: 23, color: "bg-red-100" },
    { id: 3, name: "Feature Request", tickets: 18, color: "bg-green-100" },
    { id: 4, name: "Dúvida", tickets: 12, color: "bg-yellow-100" },
  ];

  return (
    <Layout>
      <div className="space-y-8">
        <div>
          <h1 className="text-3xl font-bold text-gray-900">Painel Administrativo</h1>
          <p className="text-gray-600 mt-2">
            Gerencie usuários, chamados e configurações do sistema
          </p>
        </div>

        {/* Stats Grid */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
          {stats.map((stat) => {
            const Icon = stat.icon;
            return (
              <Card key={stat.label} className="p-6">
                <div className="flex items-start justify-between">
                  <div>
                    <p className="text-gray-600 text-sm font-medium">
                      {stat.label}
                    </p>
                    <p className="text-2xl font-bold text-gray-900 mt-2">
                      {stat.value}
                    </p>
                    <p className="text-xs text-green-600 mt-2">{stat.change}</p>
                  </div>
                  <div className={`p-3 rounded-lg ${stat.color}`}>
                    <Icon size={24} />
                  </div>
                </div>
              </Card>
            );
          })}
        </div>

        {/* Tabs */}
        <Card className="p-6">
          <div className="flex gap-4 border-b border-gray-200 mb-6">
            {[
              { id: "overview", label: "Visão Geral" },
              { id: "users", label: "Usuários" },
              { id: "categories", label: "Categorias" },
              { id: "settings", label: "Configurações" },
            ].map((tab) => (
              <button
                key={tab.id}
                onClick={() => setActiveTab(tab.id)}
                className={`px-4 py-2 font-medium border-b-2 transition ${
                  activeTab === tab.id
                    ? "border-purple-600 text-purple-600"
                    : "border-transparent text-gray-600 hover:text-gray-900"
                }`}
              >
                {tab.label}
              </button>
            ))}
          </div>

          {/* Overview Tab */}
          {activeTab === "overview" && (
            <div className="space-y-6">
              <div>
                <h3 className="font-bold text-gray-900 mb-4">
                  Distribuição de Chamados por Categoria
                </h3>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  {categories.map((cat) => (
                    <div
                      key={cat.id}
                      className={`p-4 rounded-lg ${cat.color} flex items-center justify-between`}
                    >
                      <div>
                        <p className="font-semibold text-gray-900">
                          {cat.name}
                        </p>
                        <p className="text-sm text-gray-600">
                          {cat.tickets} chamados
                        </p>
                      </div>
                      <div className="text-2xl font-bold text-gray-900">
                        {cat.tickets}
                      </div>
                    </div>
                  ))}
                </div>
              </div>
            </div>
          )}

          {/* Users Tab */}
          {activeTab === "users" && (
            <div className="space-y-4">
              <div className="flex justify-between items-center mb-4">
                <h3 className="font-bold text-gray-900">Usuários Recentes</h3>
                <Button className="bg-purple-600 hover:bg-purple-700 text-white">
                  Novo Usuário
                </Button>
              </div>
              <div className="overflow-x-auto">
                <table className="w-full">
                  <thead>
                    <tr className="border-b border-gray-200">
                      <th className="text-left py-3 px-4 font-semibold text-gray-700">
                        Nome
                      </th>
                      <th className="text-left py-3 px-4 font-semibold text-gray-700">
                        Email
                      </th>
                      <th className="text-left py-3 px-4 font-semibold text-gray-700">
                        Status
                      </th>
                      <th className="text-left py-3 px-4 font-semibold text-gray-700">
                        Data de Cadastro
                      </th>
                      <th className="text-left py-3 px-4 font-semibold text-gray-700">
                        Ações
                      </th>
                    </tr>
                  </thead>
                  <tbody>
                    {recentUsers.map((user) => (
                      <tr
                        key={user.id}
                        className="border-b border-gray-100 hover:bg-gray-50"
                      >
                        <td className="py-4 px-4 font-medium text-gray-900">
                          {user.name}
                        </td>
                        <td className="py-4 px-4 text-gray-600">{user.email}</td>
                        <td className="py-4 px-4">
                          <span
                            className={`px-3 py-1 rounded-full text-xs font-medium ${
                              user.status === "Ativo"
                                ? "bg-green-100 text-green-800"
                                : "bg-gray-100 text-gray-800"
                            }`}
                          >
                            {user.status}
                          </span>
                        </td>
                        <td className="py-4 px-4 text-gray-600">
                          {user.joinDate}
                        </td>
                        <td className="py-4 px-4">
                          <button className="text-purple-600 hover:text-purple-700 font-medium text-sm">
                            Editar
                          </button>
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            </div>
          )}

          {/* Categories Tab */}
          {activeTab === "categories" && (
            <div className="space-y-4">
              <div className="flex justify-between items-center mb-4">
                <h3 className="font-bold text-gray-900">Categorias de Chamados</h3>
                <Button className="bg-purple-600 hover:bg-purple-700 text-white">
                  Nova Categoria
                </Button>
              </div>
              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                {categories.map((cat) => (
                  <Card key={cat.id} className="p-4 flex items-center justify-between">
                    <div>
                      <p className="font-semibold text-gray-900">{cat.name}</p>
                      <p className="text-sm text-gray-600">
                        {cat.tickets} chamados ativos
                      </p>
                    </div>
                    <button className="text-purple-600 hover:text-purple-700">
                      Editar
                    </button>
                  </Card>
                ))}
              </div>
            </div>
          )}

          {/* Settings Tab */}
          {activeTab === "settings" && (
            <div className="space-y-6">
              <div>
                <h3 className="font-bold text-gray-900 mb-4">
                  Configurações do Sistema
                </h3>
                <div className="space-y-4">
                  <div className="flex items-center justify-between p-4 border border-gray-200 rounded-lg">
                    <div>
                      <p className="font-medium text-gray-900">
                        Notificações por Email
                      </p>
                      <p className="text-sm text-gray-600">
                        Enviar notificações automáticas aos usuários
                      </p>
                    </div>
                    <input type="checkbox" defaultChecked className="w-5 h-5" />
                  </div>
                  <div className="flex items-center justify-between p-4 border border-gray-200 rounded-lg">
                    <div>
                      <p className="font-medium text-gray-900">
                        Modo de Manutenção
                      </p>
                      <p className="text-sm text-gray-600">
                        Desabilitar acesso de usuários normais
                      </p>
                    </div>
                    <input type="checkbox" className="w-5 h-5" />
                  </div>
                  <div className="flex items-center justify-between p-4 border border-gray-200 rounded-lg">
                    <div>
                      <p className="font-medium text-gray-900">
                        Backup Automático
                      </p>
                      <p className="text-sm text-gray-600">
                        Realizar backup diário dos dados
                      </p>
                    </div>
                    <input type="checkbox" defaultChecked className="w-5 h-5" />
                  </div>
                </div>
              </div>
              <div className="flex gap-4 pt-4 border-t border-gray-200">
                <Button className="bg-purple-600 hover:bg-purple-700 text-white">
                  Salvar Alterações
                </Button>
                <Button variant="outline">Cancelar</Button>
              </div>
            </div>
          )}
        </Card>
      </div>
    </Layout>
  );
}

