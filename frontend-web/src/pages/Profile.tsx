import Layout from "@/components/Layout";
import { Card } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Mail, Phone, MapPin, Edit2 } from "lucide-react";

export default function Profile() {
  const user = {
    name: "João Silva",
    email: "joao.silva@example.com",
    phone: "(11) 98765-4321",
    location: "São Paulo, SP",
    department: "Tecnologia",
    role: "Analista de Sistemas",
    joinDate: "2023-01-15",
    avatar: "JS",
  };

  return (
    <Layout>
      <div className="space-y-8">
        {/* Profile Header */}
        <div className="bg-gradient-to-r from-purple-600 to-purple-700 rounded-lg p-8 text-white">
          <div className="flex items-start justify-between">
            <div className="flex items-center gap-6">
              <div className="w-24 h-24 bg-white rounded-full flex items-center justify-center text-3xl font-bold text-purple-600">
                {user.avatar}
              </div>
              <div>
                <h1 className="text-3xl font-bold">{user.name}</h1>
                <p className="text-purple-100 mt-1">{user.role}</p>
                <p className="text-purple-100 text-sm">
                  Membro desde {user.joinDate}
                </p>
              </div>
            </div>
            <Button className="bg-white text-purple-600 hover:bg-purple-50 flex items-center gap-2">
              <Edit2 size={18} />
              Editar Perfil
            </Button>
          </div>
        </div>

        {/* Profile Information */}
        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
          {/* Contact Information */}
          <Card className="p-6">
            <h2 className="text-lg font-bold text-gray-900 mb-6">
              Informações de Contato
            </h2>
            <div className="space-y-4">
              <div className="flex items-center gap-4">
                <div className="p-3 bg-purple-100 rounded-lg">
                  <Mail className="text-purple-600" size={20} />
                </div>
                <div>
                  <p className="text-sm text-gray-600">Email</p>
                  <p className="text-gray-900 font-medium">{user.email}</p>
                </div>
              </div>
              <div className="flex items-center gap-4">
                <div className="p-3 bg-blue-100 rounded-lg">
                  <Phone className="text-blue-600" size={20} />
                </div>
                <div>
                  <p className="text-sm text-gray-600">Telefone</p>
                  <p className="text-gray-900 font-medium">{user.phone}</p>
                </div>
              </div>
              <div className="flex items-center gap-4">
                <div className="p-3 bg-green-100 rounded-lg">
                  <MapPin className="text-green-600" size={20} />
                </div>
                <div>
                  <p className="text-sm text-gray-600">Localização</p>
                  <p className="text-gray-900 font-medium">{user.location}</p>
                </div>
              </div>
            </div>
          </Card>

          {/* Professional Information */}
          <Card className="p-6">
            <h2 className="text-lg font-bold text-gray-900 mb-6">
              Informações Profissionais
            </h2>
            <div className="space-y-6">
              <div>
                <label className="text-sm text-gray-600 font-medium">
                  Departamento
                </label>
                <p className="text-gray-900 mt-1">{user.department}</p>
              </div>
              <div>
                <label className="text-sm text-gray-600 font-medium">
                  Cargo
                </label>
                <p className="text-gray-900 mt-1">{user.role}</p>
              </div>
              <div>
                <label className="text-sm text-gray-600 font-medium">
                  Status
                </label>
                <span className="inline-block mt-1 px-3 py-1 bg-green-100 text-green-800 rounded-full text-sm font-medium">
                  Ativo
                </span>
              </div>
            </div>
          </Card>
        </div>

        {/* Activity */}
        <Card className="p-6">
          <h2 className="text-lg font-bold text-gray-900 mb-6">
            Atividade Recente
          </h2>
          <div className="space-y-4">
            {[
              {
                action: "Abriu um novo chamado",
                description: "Problema de acesso ao sistema",
                date: "2025-10-25 14:30",
              },
              {
                action: "Atualizou seu perfil",
                description: "Alterou número de telefone",
                date: "2025-10-24 10:15",
              },
              {
                action: "Respondeu a um chamado",
                description: "Ticket TK-002",
                date: "2025-10-23 16:45",
              },
            ].map((activity, index) => (
              <div
                key={index}
                className="flex items-start gap-4 pb-4 border-b border-gray-100 last:border-0"
              >
                <div className="w-2 h-2 rounded-full bg-purple-600 mt-2"></div>
                <div>
                  <p className="font-medium text-gray-900">{activity.action}</p>
                  <p className="text-sm text-gray-600">{activity.description}</p>
                  <p className="text-xs text-gray-500 mt-1">{activity.date}</p>
                </div>
              </div>
            ))}
          </div>
        </Card>
      </div>
    </Layout>
  );
}

