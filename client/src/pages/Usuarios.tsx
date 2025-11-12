import { useState } from "react";
import { X, Edit2 } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Card } from "@/components/ui/card";
import Header from "@/components/Header";
import { useAuth } from "@/contexts/AuthContext";
import { useLocation } from "wouter";
import { toast } from "sonner";

const USUARIOS_INICIAIS = [
  {
    id: "1",
    matricula: "001",
    nome: "Ratatoio Silva",
    email: "ratatoio.silva@empresalegal.com.br",
    setor: "RH",
    nivelSuporte: "User",
    permissoes: "User",
    status: "Ativo",
    celular: "(11) 98765-4321",
  },
  {
    id: "2",
    matricula: "002",
    nome: "Roberto Carlos",
    email: "roberto.carlos@empresalegal.com.br",
    setor: "TI",
    nivelSuporte: "User",
    permissoes: "User",
    status: "Ativo",
    celular: "(11) 99876-5432",
  },
  {
    id: "3",
    matricula: "003",
    nome: "Loro José",
    email: "loro.jose@empresalegal.com.br",
    setor: "Suporte",
    nivelSuporte: "Suporte N1",
    permissoes: "Suporte N1",
    status: "Ativo",
    celular: "(11) 97654-3210",
  },
  {
    id: "4",
    matricula: "004",
    nome: "João Alves",
    email: "joao.alves@empresalegal.com.br",
    setor: "Suporte",
    nivelSuporte: "Suporte N2/Admin",
    permissoes: "Suporte N2/Admin",
    status: "Ativo",
    celular: "(11) 96543-2109",
  },
];

export default function Usuarios() {
  const { userName, userType } = useAuth();
  const [, navigate] = useLocation();

  // Redirecionar user para página de usuários pública (se houver)
  if (userType === "user") {
    navigate("/");
    return null;
  }

  const [usuarios, setUsuarios] = useState(USUARIOS_INICIAIS);
  const [searchTerm, setSearchTerm] = useState("");
  const [filterSetor, setFilterSetor] = useState<string>("Todos");
  const [filterNivel, setFilterNivel] = useState<string>("Todos");

  // Usuário selecionado para edição
  const [selectedUsuario, setSelectedUsuario] = useState<typeof USUARIOS_INICIAIS[0] | null>(null);
  const [editingData, setEditingData] = useState<typeof USUARIOS_INICIAIS[0] | null>(null);

  const setores = ["Todos", ...Array.from(new Set(usuarios.map((u) => u.setor)))];
  const niveis = ["Todos", ...Array.from(new Set(usuarios.map((u) => u.nivelSuporte)))];

  const filteredUsuarios = usuarios.filter((usuario) => {
    const matchSearch =
      usuario.nome.toLowerCase().includes(searchTerm.toLowerCase()) ||
      usuario.email.toLowerCase().includes(searchTerm.toLowerCase());
    const matchSetor = filterSetor === "Todos" || usuario.setor === filterSetor;
    const matchNivel = filterNivel === "Todos" || usuario.nivelSuporte === filterNivel;
    return matchSearch && matchSetor && matchNivel;
  });

  const handleSelectUsuario = (usuario: typeof USUARIOS_INICIAIS[0]) => {
    setSelectedUsuario(usuario);
    setEditingData({ ...usuario });
  };

  const handleSaveUsuario = () => {
    if (!selectedUsuario || !editingData) return;

    if (
      !editingData.matricula.trim() ||
      !editingData.nome.trim() ||
      !editingData.email.trim() ||
      !editingData.setor.trim() ||
      !editingData.celular.trim()
    ) {
      toast.error("Preencha todos os campos obrigatórios!");
      return;
    }

    setUsuarios(
      usuarios.map((u) =>
        u.id === selectedUsuario.id ? editingData : u
      )
    );

    toast.success("Usuário atualizado com sucesso!");
    setSelectedUsuario(null);
    setEditingData(null);
  };

  const handleDeleteUsuario = () => {
    if (!selectedUsuario) return;

    if (confirm(`Tem certeza que deseja deletar o usuário: "${selectedUsuario.nome}"?`)) {
      setUsuarios(usuarios.filter((u) => u.id !== selectedUsuario.id));
      toast.success("Usuário deletado com sucesso!");
      setSelectedUsuario(null);
      setEditingData(null);
    }
  };

  const handleCreateNewUsuario = () => {
    const newId = String(Math.max(...usuarios.map((u) => parseInt(u.id)), 0) + 1);
    const newUsuario = {
      id: newId,
      matricula: "",
      nome: "",
      email: "",
      setor: "RH",
      nivelSuporte: "User",
      permissoes: "User",
      status: "Ativo",
      celular: "",
    };
    setSelectedUsuario(newUsuario);
    setEditingData(newUsuario);
  };

  const handleClearSelection = () => {
    setSelectedUsuario(null);
    setEditingData(null);
  };

  return (
    <div className="min-h-screen bg-gray-50">
      <Header userName={userName || "Admin"} />

      <div className="max-w-7xl mx-auto px-4 py-8">
        <div className="mb-8 flex justify-between items-center">
          <div>
            <h1 className="text-4xl font-bold text-purple-900">GESTÃO DE USUÁRIOS</h1>
          </div>
          <Button
            onClick={handleCreateNewUsuario}
            className="bg-purple-600 hover:bg-purple-700 text-white font-semibold py-2 px-6 rounded-full transition"
          >
            + Novo Usuário
          </Button>
        </div>

        <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
          {/* COLUNA ESQUERDA - LISTA DE USUÁRIOS */}
          <div className="lg:col-span-2 space-y-4">
            {/* Filtros */}
            <Card className="p-4 bg-white">
              <div className="space-y-3">
                <input
                  type="text"
                  placeholder="Buscar..."
                  value={searchTerm}
                  onChange={(e) => setSearchTerm(e.target.value)}
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-purple-500"
                />

                <div className="flex flex-wrap gap-2">
                  <select
                    value={filterSetor}
                    onChange={(e) => setFilterSetor(e.target.value)}
                    className="px-3 py-1 border border-gray-300 rounded text-sm bg-white focus:outline-none focus:ring-2 focus:ring-purple-500"
                  >
                    {setores.map((setor) => (
                      <option key={setor} value={setor}>
                        {setor === "Todos" ? "Setor" : setor}
                      </option>
                    ))}
                  </select>

                  <select
                    value={filterNivel}
                    onChange={(e) => setFilterNivel(e.target.value)}
                    className="px-3 py-1 border border-gray-300 rounded text-sm bg-white focus:outline-none focus:ring-2 focus:ring-purple-500"
                  >
                    {niveis.map((nivel) => (
                      <option key={nivel} value={nivel}>
                        {nivel === "Todos" ? "N. Suporte" : nivel}
                      </option>
                    ))}
                  </select>
                </div>
              </div>
            </Card>

            {/* Tabela de Usuários */}
            <Card className="overflow-hidden bg-white">
              <div className="overflow-x-auto">
                <table className="w-full text-sm">
                  <thead className="bg-gray-100 border-b">
                    <tr>
                      <th className="px-6 py-3 text-left font-semibold text-gray-700">Nome</th>
                      <th className="px-6 py-3 text-left font-semibold text-gray-700">Setor</th>
                      <th className="px-6 py-3 text-left font-semibold text-gray-700">Email</th>
                      <th className="px-6 py-3 text-left font-semibold text-gray-700">Permissões</th>
                      <th className="px-6 py-3 text-left font-semibold text-gray-700">Status</th>
                      <th className="px-6 py-3 text-center font-semibold text-gray-700">Ação</th>
                    </tr>
                  </thead>
                  <tbody>
                    {filteredUsuarios.length > 0 ? (
                      filteredUsuarios.map((usuario) => (
                        <tr
                          key={usuario.id}
                          className={`border-b hover:bg-gray-50 cursor-pointer transition ${
                            selectedUsuario?.id === usuario.id ? "bg-purple-50" : ""
                          }`}
                          onClick={() => handleSelectUsuario(usuario)}
                        >
                          <td className="px-6 py-4 text-gray-900 font-medium">{usuario.nome}</td>
                          <td className="px-6 py-4 text-gray-700">{usuario.setor}</td>
                          <td className="px-6 py-4 text-gray-700">{usuario.email}</td>
                          <td className="px-6 py-4 text-gray-700">{usuario.permissoes}</td>
                          <td className="px-6 py-4">
                            <span className="px-3 py-1 bg-green-100 text-green-700 rounded-full text-xs font-semibold">
                              {usuario.status}
                            </span>
                          </td>
                          <td className="px-6 py-4 text-center">
                            <button
                              onClick={(e) => {
                                e.stopPropagation();
                                handleSelectUsuario(usuario);
                              }}
                              className="text-purple-600 hover:text-purple-700 transition"
                            >
                              <Edit2 size={18} />
                            </button>
                          </td>
                        </tr>
                      ))
                    ) : (
                      <tr>
                        <td colSpan={6} className="px-6 py-8 text-center text-gray-500">
                          Nenhum usuário encontrado
                        </td>
                      </tr>
                    )}
                  </tbody>
                </table>
              </div>
            </Card>
          </div>

          {/* COLUNA DIREITA - PAINEL DE EDIÇÃO */}
          <div>
            {selectedUsuario && editingData ? (
              <Card className="p-6 bg-white sticky top-8">
                <div className="flex justify-between items-center mb-4">
                  <h2 className="text-lg font-bold text-purple-900">Editar/Criar perfil</h2>
                  <button
                    onClick={handleClearSelection}
                    className="text-gray-500 hover:text-gray-700"
                  >
                    <X size={20} />
                  </button>
                </div>

                <div className="space-y-4 max-h-[70vh] overflow-y-auto">
                  {/* Matrícula */}
                  <div>
                    <label className="block text-sm font-semibold text-purple-600 mb-2">
                      Matrícula:
                    </label>
                    <input
                      type="text"
                      value={editingData.matricula}
                      onChange={(e) =>
                        setEditingData({ ...editingData, matricula: e.target.value })
                      }
                      className="w-full px-3 py-2 border border-gray-300 rounded text-sm focus:outline-none focus:ring-2 focus:ring-purple-500 bg-gray-50"
                    />
                  </div>

                  {/* Nome & Sobrenome */}
                  <div>
                    <label className="block text-sm font-semibold text-purple-600 mb-2">
                      Nome & Sobrenome:
                    </label>
                    <input
                      type="text"
                      value={editingData.nome}
                      onChange={(e) =>
                        setEditingData({ ...editingData, nome: e.target.value })
                      }
                      className="w-full px-3 py-2 border border-gray-300 rounded text-sm focus:outline-none focus:ring-2 focus:ring-purple-500"
                    />
                  </div>

                  {/* Email corporativo */}
                  <div>
                    <label className="block text-sm font-semibold text-purple-600 mb-2">
                      Email corporativo:
                    </label>
                    <input
                      type="email"
                      value={editingData.email}
                      onChange={(e) =>
                        setEditingData({ ...editingData, email: e.target.value })
                      }
                      className="w-full px-3 py-2 border border-gray-300 rounded text-sm focus:outline-none focus:ring-2 focus:ring-purple-500"
                    />
                  </div>

                  {/* Celular */}
                  <div>
                    <label className="block text-sm font-semibold text-purple-600 mb-2">
                      Celular:
                    </label>
                    <input
                      type="tel"
                      value={editingData.celular}
                      onChange={(e) =>
                        setEditingData({ ...editingData, celular: e.target.value })
                      }
                      className="w-full px-3 py-2 border border-gray-300 rounded text-sm focus:outline-none focus:ring-2 focus:ring-purple-500"
                    />
                  </div>

                  {/* Setor */}
                  <div>
                    <label className="block text-sm font-semibold text-purple-600 mb-2">
                      Setor:
                    </label>
                    <select
                      value={editingData.setor}
                      onChange={(e) =>
                        setEditingData({ ...editingData, setor: e.target.value })
                      }
                      className="w-full px-3 py-2 border border-gray-300 rounded text-sm focus:outline-none focus:ring-2 focus:ring-purple-500 bg-white"
                    >
                      <option>RH</option>
                      <option>TI</option>
                      <option>Suporte</option>
                      <option>Financeiro</option>
                      <option>Administrativo</option>
                    </select>
                  </div>

                  {/* Nível de Suporte */}
                  <div>
                    <label className="block text-sm font-semibold text-purple-600 mb-2">
                      Nível de Suporte:
                    </label>
                    <select
                      value={editingData.nivelSuporte}
                      onChange={(e) =>
                        setEditingData({ ...editingData, nivelSuporte: e.target.value })
                      }
                      className="w-full px-3 py-2 border border-gray-300 rounded text-sm focus:outline-none focus:ring-2 focus:ring-purple-500 bg-white"
                    >
                      <option>User</option>
                      <option>Suporte N1</option>
                      <option>Suporte N2/Admin</option>
                    </select>
                  </div>

                  {/* Permissões */}
                  <div>
                    <label className="block text-sm font-semibold text-purple-600 mb-2">
                      Permissões:
                    </label>
                    <select
                      value={editingData.permissoes}
                      onChange={(e) =>
                        setEditingData({ ...editingData, permissoes: e.target.value })
                      }
                      className="w-full px-3 py-2 border border-gray-300 rounded text-sm focus:outline-none focus:ring-2 focus:ring-purple-500 bg-white"
                    >
                      <option>User</option>
                      <option>Suporte N1</option>
                      <option>Suporte N2/Admin</option>
                    </select>
                  </div>

                  {/* Status */}
                  <div>
                    <label className="block text-sm font-semibold text-purple-600 mb-2">
                      Status:
                    </label>
                    <select
                      value={editingData.status}
                      onChange={(e) =>
                        setEditingData({ ...editingData, status: e.target.value })
                      }
                      className="w-full px-3 py-2 border border-gray-300 rounded text-sm focus:outline-none focus:ring-2 focus:ring-purple-500 bg-white"
                    >
                      <option>Ativo</option>
                      <option>Inativo</option>
                    </select>
                  </div>

                  {/* Botões */}
                  <div className="space-y-2 pt-4">
                    <Button
                      onClick={handleDeleteUsuario}
                      className="w-full bg-red-600 hover:bg-red-700 text-white text-sm py-2 rounded transition"
                    >
                      Excluir Usuário
                    </Button>
                    <Button
                      onClick={handleSaveUsuario}
                      className="w-full bg-purple-600 hover:bg-purple-700 text-white text-sm py-2 rounded transition"
                    >
                      Salvar Edições
                    </Button>
                  </div>
                </div>
              </Card>
            ) : (
              <Card className="p-6 bg-white sticky top-8 text-center">
                <p className="text-gray-500 mb-4">Selecione um usuário para editar</p>
              </Card>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}