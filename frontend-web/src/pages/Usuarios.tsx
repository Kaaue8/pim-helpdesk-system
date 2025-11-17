import { useState, useEffect } from "react";
import { Button } from "@/components/ui/button";
import { Card } from "@/components/ui/card";
import { Edit2, Trash2, Search } from "lucide-react";
import { toast } from "sonner";
import { useAuth } from "../contexts/AuthContext";
import Header from "@/components/Header";

// Interfaces para o frontend e backend
interface UsuarioBackend {
  id: number;
  nome: string;
  email: string;
  perfil: string;
  [key: string]: any;
}
interface UsuarioFrontend {
  id: number;
  matricula: string;
  nome: string;
  email: string;
  celular: string;
  setor: string;
  nivelSuporte: string;
  permissoes: string;
  status: "ativo" | "inativo";
  backendData: UsuarioBackend;
}

export default function Usuarios() {
  const { userName, token } = useAuth();
  const [usuarios, setUsuarios] = useState<UsuarioFrontend[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [busca, setBusca] = useState("");
  const [filtroSetor, setFiltroSetor] = useState("");
  const [filtroNivel, setFiltroNivel] = useState("");
  const [usuarioSelecionado, setUsuarioSelecionado] = useState<UsuarioFrontend | null>(null);
  const [isEditando, setIsEditando] = useState(false);
  const [showConfirmDelete, setShowConfirmDelete] = useState(false);

  // Estados de edição
  const [matricula, setMatricula] = useState("");
  const [nome, setNome] = useState("");
  const [email, setEmail] = useState("");
  const [celular, setCelular] = useState("");
  const [setor, setSetor] = useState("");
  const [nivelSuporte, setNivelSuporte] = useState("");
  const [permissoes, setPermissoes] = useState("");
  const [status, setStatus] = useState<"ativo" | "inativo">("ativo");

  // Busca inicial de dados
  useEffect(() => {
    const fetchUsuarios = async () => {
      if (!token) { setIsLoading(false); return; }
      try {
        const response = await fetch('http://localhost:5079/api/Usuarios', {
          method: 'GET',
          headers: { 'Authorization': `Bearer ${token}` }
        } );
        if (!response.ok) throw new Error('Falha ao carregar usuários.');
        const dataFromBackend: UsuarioBackend[] = await response.json();
        const dadosParaFrontend: UsuarioFrontend[] = dataFromBackend.map(user => ({
          id: user.id,
          nome: user.nome,
          email: user.email,
          permissoes: user.perfil,
          matricula: `MAT-${String(user.id).padStart(3, "0")}`,
          celular: "(00) 00000-0000",
          setor: user.setor?.nomeSetor || "N/A",
          nivelSuporte: "",
          status: "ativo",
          backendData: user,
        }));
        setUsuarios(dadosParaFrontend);
      } catch (error) {
        toast.error((error as Error).message);
      } finally {
        setIsLoading(false);
      }
    };
    fetchUsuarios();
  }, [token]);

  // Lógica de filtragem
  const usuariosFiltrados = usuarios.filter(user => {
    const matchBusca = user.nome.toLowerCase().includes(busca.toLowerCase()) ||
                       user.email.toLowerCase().includes(busca.toLowerCase());
    const matchSetor = !filtroSetor || user.setor === filtroSetor;
    const matchNivel = !filtroNivel || user.nivelSuporte === filtroNivel;
    return matchBusca && matchSetor && matchNivel;
  });

  const setoresUnicos = Array.from(new Set(usuarios.map(u => u.setor)));
  const niveisUnicos = Array.from(new Set(usuarios.map(u => u.nivelSuporte)));

  // Funções de UI
  const handleNovoUsuario = () => {
    setUsuarioSelecionado(null);
    setMatricula(""); setNome(""); setEmail(""); setCelular("");
    setSetor(""); setNivelSuporte(""); setPermissoes(""); setStatus("ativo");
    setIsEditando(true);
  };

  const handleEditar = (usuario: UsuarioFrontend) => {
    setUsuarioSelecionado(usuario);
    setMatricula(usuario.matricula); setNome(usuario.nome); setEmail(usuario.email);
    setCelular(usuario.celular); setSetor(usuario.setor);
    setNivelSuporte(usuario.nivelSuporte); setPermissoes(usuario.permissoes);
    setStatus(usuario.status);
    setIsEditando(true);
  };

  const handleCancelar = () => {
    setIsEditando(false);
    setUsuarioSelecionado(null);
    setShowConfirmDelete(false);
  };

  // ==================================================================
  // FUNÇÃO DE SALVAR (EDIÇÃO E CRIAÇÃO)
  // ==================================================================
  const handleSalvar = async () => {
    // --- LÓGICA DE EDIÇÃO ---
    if (usuarioSelecionado) {
      const mapaSetores: { [key: string]: number } = { "Financeiro": 1, "TI": 2, "RH": 3, "Administrativo": 4, "Suporte": 4 };
      const novoSetorId = mapaSetores[setor] || usuarioSelecionado.backendData.setorIdSetor;
      const pacoteParaBackend = {
        ...usuarioSelecionado.backendData,
        nome: nome,
        email: email,
        perfil: permissoes,
        setorIdSetor: novoSetorId,
        senhaHash: usuarioSelecionado.backendData.senhaHash || "placeholder-para-nao-dar-erro",
      };
      try {
        const response = await fetch(`http://localhost:5079/api/Usuarios/${usuarioSelecionado.id}`, {
          method: 'PUT',
          headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${token}` },
          body: JSON.stringify(pacoteParaBackend )
        });
        if (response.status !== 204) throw new Error("Falha ao atualizar o usuário.");
        toast.success("Usuário atualizado com sucesso!");
        const usuarioAtualizadoUI = { ...usuarioSelecionado, nome, email, permissoes, setor, backendData: pacoteParaBackend };
        setUsuarios(usuarios.map(u => u.id === usuarioSelecionado.id ? usuarioAtualizadoUI : u));
        handleCancelar();
      } catch (error) {
        console.error("Erro ao salvar edição:", error);
        toast.error((error as Error).message);
      }
    // --- LÓGICA DE CRIAÇÃO ---
    } else {
      if (!nome || !email || !permissoes) {
        toast.error("Nome, Email e Permissões são obrigatórios!");
        return;
      }
      const mapaSetores: { [key: string]: number } = { "Financeiro": 1, "TI": 2, "RH": 3, "Administrativo": 4, "Suporte": 4 };
      const novoSetorId = mapaSetores[setor] || 1;
      const novoUsuarioDto = {
        Nome: nome,
        Email: email,
        Senha: "senhaforte123",
        Perfil: permissoes,
        SetorIdSetor: novoSetorId,
      };
      try {
        const response = await fetch('http://localhost:5079/api/Login/Register', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${token}` },
          body: JSON.stringify(novoUsuarioDto )
        });
        if (!response.ok) throw new Error("Falha ao criar o novo usuário.");
        const usuarioCriado = await response.json();
        toast.success("Usuário criado com sucesso!");
        const novoUsuarioFrontend: UsuarioFrontend = {
          id: usuarioCriado.id,
          nome: usuarioCriado.nome,
          email: usuarioCriado.email,
          permissoes: usuarioCriado.perfil,
          matricula: `MAT-${String(usuarioCriado.id).padStart(3, "0")}`,
          celular: "(00) 00000-0000",
          setor: setor || "N/A",
          nivelSuporte: nivelSuporte,
          status: "ativo",
          backendData: usuarioCriado,
        };
        setUsuarios([...usuarios, novoUsuarioFrontend]);
        handleCancelar();
      } catch (error) {
        console.error("Erro ao criar usuário:", error);
        toast.error((error as Error).message);
      }
    }
  };

  // ==================================================================
  // FUNÇÃO DE EXCLUIR (AGORA CONECTADA)
  // ==================================================================
  const handleExcluir = async () => {
    if (!usuarioSelecionado) {
      toast.error("Nenhum usuário selecionado para exclusão.");
      return;
    }
    try {
      const response = await fetch(`http://localhost:5079/api/Usuarios/${usuarioSelecionado.id}`, {
        method: 'DELETE',
        headers: { 'Authorization': `Bearer ${token}` }
      } );
      if (response.status !== 204) {
        throw new Error("Falha ao excluir o usuário.");
      }
      toast.success("Usuário excluído com sucesso!");
      setUsuarios(usuarios.filter(user => user.id !== usuarioSelecionado.id));
      handleCancelar();
    } catch (error) {
      console.error("Erro ao excluir usuário:", error);
      toast.error((error as Error).message);
    }
  };

  // Funções de UI
  const atualizarPermissoes = (novoSetor: string, novoNivel: string) => { let novaPermissao = "User"; if (novoSetor === "Suporte") { if (novoNivel === "N1") novaPermissao = "Suporte N1"; else if (novoNivel === "N2") novaPermissao = "Suporte N2/Admin"; else if (novoNivel === "N3") novaPermissao = "Admin"; } else if (novoSetor === "T.I" && novoNivel) { novaPermissao = "Admin"; } setPermissoes(novaPermissao); };
  const handleSetorChange = (novoSetor: string) => { setSetor(novoSetor); if (novoSetor !== "T.I") setNivelSuporte(""); atualizarPermissoes(novoSetor, nivelSuporte); };
  const handleNivelSuporteChange = (novoNivel: string) => { setNivelSuporte(novoNivel); atualizarPermissoes(setor, novoNivel); };
  const handleToggleStatus = () => setStatus(status === "ativo" ? "inativo" : "ativo");

  return (
    <div className="min-h-screen bg-gray-100">
      <Header userName={userName} />
      <div className="max-w-7xl mx-auto px-4 py-8">
        {isLoading ? (
          <div className="flex items-center justify-center py-16">
            <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-purple-600"></div>
            <p className="ml-4 text-gray-600">Carregando usuários...</p>
          </div>
        ) : !isEditando ? (
          <>
            <div className="flex justify-between items-center mb-8">
              <h1 className="text-4xl font-bold text-purple-900">GESTÃO DE USUÁRIOS</h1>
              <Button onClick={handleNovoUsuario} className="bg-purple-600 hover:bg-purple-700 text-white font-semibold py-2 px-6 rounded-full">
                + Novo Usuário
              </Button>
            </div>
            <Card className="p-4 mb-8 bg-gray-200">
              <div className="flex gap-4 items-center flex-wrap">
                <select value={filtroSetor} onChange={(e) => setFiltroSetor(e.target.value)} className="px-4 py-2 border border-gray-300 rounded-lg bg-white text-gray-700 cursor-pointer">
                  <option value="">Todos os Setores</option>
                  {setoresUnicos.map(s => <option key={s} value={s}>{s}</option>)}
                </select>
                <select value={filtroNivel} onChange={(e) => setFiltroNivel(e.target.value)} className="px-4 py-2 border border-gray-300 rounded-lg bg-white text-gray-700 cursor-pointer">
                  <option value="">Todos os Níveis</option>
                  {niveisUnicos.map(n => <option key={n} value={n}>{n}</option>)}
                </select>
                <div className="flex-1 flex items-center gap-2 bg-white rounded-lg px-4 py-2 border border-gray-300">
                  <input type="text" placeholder="Buscar..." value={busca} onChange={(e) => setBusca(e.target.value)} className="flex-1 bg-transparent text-gray-700 focus:outline-none" />
                  <Search size={20} className="text-gray-400" />
                </div>
              </div>
            </Card>
            <Card className="p-6 overflow-x-auto">
              <table className="w-full text-sm">
                <thead>
                  <tr className="border-b-2 border-gray-300 bg-gray-100">
                    <th className="text-left py-3 px-2 text-purple-900 font-semibold">Nome</th>
                    <th className="text-left py-3 px-2 text-purple-900 font-semibold">Setor</th>
                    <th className="text-left py-3 px-2 text-purple-900 font-semibold">Email</th>
                    <th className="text-left py-3 px-2 text-purple-900 font-semibold">Permissões</th>
                    <th className="text-left py-3 px-2 text-purple-900 font-semibold">Status</th>
                    <th className="text-center py-3 px-2 text-purple-900 font-semibold">Ações</th>
                  </tr>
                </thead>
                <tbody>
                  {usuariosFiltrados.map((usuario, index) => (
                    <tr key={usuario.id} className={index % 2 === 0 ? "bg-gray-100" : "bg-white"}>
                      <td className="py-3 px-2 text-gray-700">{usuario.nome}</td>
                      <td className="py-3 px-2 text-gray-700">{usuario.setor}</td>
                      <td className="py-3 px-2 text-gray-700 text-xs">{usuario.email}</td>
                      <td className="py-3 px-2 text-gray-700">{usuario.permissoes}</td>
                      <td className="py-3 px-2 text-gray-700"><span className="text-gray-600 font-medium">{usuario.status === "ativo" ? "Ativo" : "Inativo"}</span></td>
                      <td className="py-3 px-2 text-center">
                        <button onClick={() => handleEditar(usuario)} className="text-purple-600 hover:text-purple-800"><Edit2 size={18} /></button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </Card>
          </>
        ) : (
          <>
            <div className="max-w-2xl mx-auto">
              <h1 className="text-4xl font-bold text-purple-900 mb-8 text-center">{usuarioSelecionado ? "Editar" : "Criar"} Usuário</h1>
              <Card className="p-8">
                <div className="mb-6"><label className="block text-purple-900 font-semibold mb-2">Matrícula:</label><input type="text" value={matricula} onChange={(e) => setMatricula(e.target.value)} disabled={!!usuarioSelecionado} className="w-full px-4 py-2 border border-gray-300 rounded bg-gray-200 text-gray-700" /></div>
                <div className="mb-6"><label className="block text-purple-900 font-semibold mb-2">Nome & Sobrenome*:</label><input type="text" value={nome} onChange={(e) => setNome(e.target.value)} className="w-full px-4 py-2 border border-gray-300 rounded bg-white text-gray-900" /></div>
                <div className="mb-6"><label className="block text-purple-900 font-semibold mb-2">Email corporativo*:</label><input type="email" value={email} onChange={(e) => setEmail(e.target.value)} className="w-full px-4 py-2 border border-gray-300 rounded bg-white text-gray-900" /></div>
                <div className="mb-6"><label className="block text-purple-900 font-semibold mb-2">Celular:</label><input type="tel" value={celular} onChange={(e) => setCelular(e.target.value)} className="w-full px-4 py-2 border border-gray-300 rounded bg-white text-gray-900" /></div>
                <div className="mb-6"><label className="block text-purple-900 font-semibold mb-2">Setor*:</label><select value={setor} onChange={(e) => handleSetorChange(e.target.value)} className="w-full px-4 py-2 border border-gray-300 rounded bg-white text-gray-900"><option value="">Selecione...</option><option value="Financeiro">Financeiro</option><option value="RH">RH</option><option value="T.I">T.I</option><option value="Suporte">Suporte</option></select></div>
                {setor === "T.I" ? (<div className="mb-6"><label className="block text-purple-900 font-semibold mb-2">Nível de Suporte:</label><select value={nivelSuporte} onChange={(e) => handleNivelSuporteChange(e.target.value)} className="w-full px-4 py-2 border border-gray-300 rounded bg-white text-gray-900"><option value="">Selecione...</option><option value="N1">N1</option><option value="N2">N2</option><option value="N3">N3</option></select></div>) : (<div className="mb-6"><label className="block text-purple-900 font-semibold mb-2 text-gray-500">Nível de Suporte: (Apenas para T.I)</label><input type="text" disabled className="w-full px-4 py-2 border border-gray-300 rounded bg-gray-200 text-gray-500" placeholder="Disponível apenas para setor T.I" /></div>)}
                <div className="mb-8"><label className="block text-purple-900 font-semibold mb-2">Permissões*: (Preenchida automaticamente)</label><input type="text" value={permissoes} disabled className="w-full px-4 py-2 border border-gray-300 rounded bg-gray-200 text-gray-700 font-semibold" /></div>
                <div className="mb-8 flex gap-4"><button onClick={handleToggleStatus} className={`px-6 py-2 rounded font-semibold transition ${status === "ativo" ? "text-purple-600 border-b-2 border-purple-600" : "text-gray-400 border-b-2 border-transparent"}`}>Perfil Ativo</button><span className="text-gray-400">|</span><button onClick={handleToggleStatus} className={`px-6 py-2 rounded font-semibold transition ${status === "inativo" ? "text-purple-600 border-b-2 border-purple-600" : "text-gray-400 border-b-2 border-transparent"}`}>Desativar Perfil</button></div>
                <div className="flex gap-4"><Button onClick={() => setShowConfirmDelete(true)} className="flex-1 bg-red-600 hover:bg-red-700 text-white font-semibold py-2 rounded"><Trash2 size={18} className="mr-2" />Excluir Usuário</Button><Button onClick={handleCancelar} className="flex-1 bg-gray-400 hover:bg-gray-500 text-white font-semibold py-2 rounded">Cancelar</Button><Button onClick={handleSalvar} className="flex-1 bg-purple-600 hover:bg-purple-700 text-white font-semibold py-2 rounded">Salvar Edições</Button></div>
              </Card>
            </div>
          </>
        )}
      </div>
      {showConfirmDelete && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
          <Card className="p-8 max-w-md">
            <h2 className="text-2xl font-bold text-purple-900 mb-4">Tem certeza disso?</h2>
            <p className="text-gray-700 mb-6">Você está prestes a excluir o usuário <strong>{nome}</strong>. Esta ação não pode ser desfeita.</p>
            <div className="flex gap-4"><Button onClick={handleCancelar} className="flex-1 bg-gray-400 hover:bg-gray-500 text-white font-semibold py-2 rounded">Cancelar</Button><Button onClick={handleExcluir} className="flex-1 bg-red-600 hover:bg-red-700 text-white font-semibold py-2 rounded">Excluir</Button></div>
          </Card>
        </div>
      )}
    </div>
  );
}
