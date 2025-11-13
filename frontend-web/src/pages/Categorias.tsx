import { useState } from "react";
import { Button } from "@/components/ui/button";
import { Card } from "@/components/ui/card";
import { Edit2, Trash2, Search } from "lucide-react";
import { toast } from "sonner";
import { useAuth } from "@/contexts/AuthContext";
import Header from "@/components/Header";

interface Categoria {
  id: number;
  tipo: string;
  sla: string;
  nivelSuporte: string;
  prioridade: string;
  palavrasChave: string;
  sugestaoSolucao: string;
}

const CATEGORIAS_INICIAIS: Categoria[] = [
  {
    id: 1,
    tipo: "Sem conexão com a internet",
    sla: "4h",
    nivelSuporte: "N1",
    prioridade: "Alta",
    palavrasChave: "sem internet, rede caiu, offline",
    sugestaoSolucao: "Tente desligar o aparelho e ligar novamente. Verifique os cabos de conexão estão em conformidade. Tente desconectar e conectar novamente na rede."
  },
  {
    id: 2,
    tipo: "Impressora com defeito",
    sla: "12h",
    nivelSuporte: "N1",
    prioridade: "Média",
    palavrasChave: "impressora, não imprime, erro impressão",
    sugestaoSolucao: "Tente desligar o aparelho e ligar novamente. Verifique se há mensagens de erro na tela"
  },
  {
    id: 3,
    tipo: "Sistema fora do ar / Erro ao acessar",
    sla: "4h",
    nivelSuporte: "N2",
    prioridade: "Alta",
    palavrasChave: "sistema fora, erro login, não acessa sistema",
    sugestaoSolucao: "Nosso equipe vai investigar o problema de acesso e restaurar o sistema o mais rápido possível."
  },
  {
    id: 4,
    tipo: "Cabeamento com defeito",
    sla: "8h",
    nivelSuporte: "N2",
    prioridade: "Média",
    palavrasChave: "cabo, rede, desconectado, rompido",
    sugestaoSolucao: "Vamos verificar os cabos e substituir se for detectado algum dano."
  },
  {
    id: 5,
    tipo: "Equipamento com defeito",
    sla: "24h",
    nivelSuporte: "N2",
    prioridade: "Baixa",
    palavrasChave: "hd, formatação, restauração, travado",
    sugestaoSolucao: "Será necessário restaurar ou formatar o sistema. Cuidaremos disso para você."
  }
];

export default function Categorias() {
  const { userName } = useAuth();
  const [categorias, setCategorias] = useState<Categoria[]>(CATEGORIAS_INICIAIS);
  const [busca, setBusca] = useState("");
  const [categoriaSelecionada, setCategoriaSelecionada] = useState<Categoria | null>(null);
  const [isEditando, setIsEditando] = useState(false);

  // Estados de edição
  const [tipo, setTipo] = useState("");
  const [sla, setSla] = useState("");
  const [nivelSuporte, setNivelSuporte] = useState("");
  const [prioridade, setPrioridade] = useState("");
  const [palavrasChave, setPalavrasChave] = useState("");
  const [sugestaoSolucao, setSugestaoSolucao] = useState("");

  const categoriasFiltradas = categorias.filter(cat =>
    cat.tipo.toLowerCase().includes(busca.toLowerCase()) ||
    cat.palavrasChave.toLowerCase().includes(busca.toLowerCase())
  );

  const handleNovaCategoria = () => {
    setCategoriaSelecionada(null);
    setTipo("");
    setSla("");
    setNivelSuporte("");
    setPrioridade("");
    setPalavrasChave("");
    setSugestaoSolucao("");
    setIsEditando(true);
  };

  const handleEditar = (categoria: Categoria) => {
    setCategoriaSelecionada(categoria);
    setTipo(categoria.tipo);
    setSla(categoria.sla);
    setNivelSuporte(categoria.nivelSuporte);
    setPrioridade(categoria.prioridade);
    setPalavrasChave(categoria.palavrasChave);
    setSugestaoSolucao(categoria.sugestaoSolucao);
    setIsEditando(true);
  };

  const handleSalvar = () => {
    if (!tipo || !sla || !nivelSuporte || !prioridade) {
      toast.error("Preencha todos os campos obrigatórios!");
      return;
    }

    if (categoriaSelecionada) {
      // Editar categoria existente
      setCategorias(categorias.map(cat =>
        cat.id === categoriaSelecionada.id
          ? {
              ...cat,
              tipo,
              sla,
              nivelSuporte,
              prioridade,
              palavrasChave,
              sugestaoSolucao
            }
          : cat
      ));
      toast.success("Categoria atualizada com sucesso!");
    } else {
      // Criar nova categoria
      const novaCategoria: Categoria = {
        id: Math.max(...categorias.map(c => c.id), 0) + 1,
        tipo,
        sla,
        nivelSuporte,
        prioridade,
        palavrasChave,
        sugestaoSolucao
      };
      setCategorias([...categorias, novaCategoria]);
      toast.success("Categoria criada com sucesso!");
    }
    setIsEditando(false);
  };

  const handleExcluir = () => {
    if (categoriaSelecionada) {
      setCategorias(categorias.filter(cat => cat.id !== categoriaSelecionada.id));
      toast.success("Categoria excluída com sucesso!");
      setIsEditando(false);
      setCategoriaSelecionada(null);
    }
  };

  const handleCancelar = () => {
    setIsEditando(false);
    setCategoriaSelecionada(null);
  };

  return (
    <div className="min-h-screen bg-gray-100">
      <Header userName={userName} />

      <div className="max-w-7xl mx-auto px-4 py-8">
        {!isEditando ? (
          <>
            {/* Cabeçalho */}
            <div className="flex justify-between items-center mb-8">
              <h1 className="text-4xl font-bold text-purple-900">
                GESTÃO DE CATEGORIAS
              </h1>
              <Button
                onClick={handleNovaCategoria}
                className="bg-purple-600 hover:bg-purple-700 text-white font-semibold py-2 px-6 rounded-full"
              >
                + Nova Categoria
              </Button>
            </div>

            {/* Busca */}
            <Card className="p-2 mb-6 border-1 border-grey-500">
              <div className="flex items-center gap-2">
                <input
                  type="text"
                  placeholder="Buscar Categoria..."
                  value={busca}
                  onChange={(e) => setBusca(e.target.value)}
                  className="flex-1 px-4 py-2 border border-gray-300 rounded-full bg-white text-gray-700 focus:outline-none"
                />
                <Search size={20} className="text-gray-400" />
              </div>
            </Card>

            {/* Tabela */}
            <Card className="p-2 overflow-x-auto">
              <table className="w-full text-sm">
                <thead>
                  <tr className="border-b-2 border-gray-300">
                    <th className="text-left py-3 px-2 text-purple-900 font-semibold">ID</th>
                    <th className="text-left py-3 px-2 text-purple-900 font-semibold">Tipo</th>
                    <th className="text-left py-3 px-2 text-purple-900 font-semibold">SLA</th>
                    <th className="text-left py-3 px-2 text-purple-900 font-semibold">N. Suporte</th>
                    <th className="text-left py-3 px-2 text-purple-900 font-semibold">Prioridade</th>
                    <th className="text-left py-3 px-2 text-purple-900 font-semibold">Palavras-chave</th>
                    <th className="text-left py-3 px-2 text-purple-900 font-semibold">Sugestão de solução</th>
                    <th className="text-center py-3 px-2 text-purple-900 font-semibold"></th>
                  </tr>
                </thead>
                <tbody>
                  {categoriasFiltradas.map((categoria, index) => (
                    <tr key={categoria.id} className={index % 2 === 0 ? "bg-gray-100" : "bg-white"}>
                      <td className="py-3 px-2 text-gray-700">{categoria.id}</td>
                      <td className="py-3 px-2 text-gray-700">{categoria.tipo}</td>
                      <td className="py-3 px-2 text-gray-700">{categoria.sla}</td>
                      <td className="py-3 px-2 text-gray-700">{categoria.nivelSuporte}</td>
                      <td className="py-3 px-2 text-gray-700">{categoria.prioridade}</td>
                      <td className="py-3 px-2 text-gray-700 text-xs">{categoria.palavrasChave}</td>
                      <td className="py-3 px-2 text-gray-700 text-xs">{categoria.sugestaoSolucao.substring(0, 50)}...</td>
                      <td className="py-3 px-2 text-center">
                        <button
                          onClick={() => handleEditar(categoria)}
                          className="text-purple-600 hover:text-purple-800"
                        >
                          <Edit2 size={18} />
                        </button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </Card>
          </>
        ) : (
          /* Formulário de Edição */
          <div className="max-w-2xl mx-auto">
            <h1 className="text-4xl font-bold text-purple-900 mb-8 text-center">
              {categoriaSelecionada ? "Editar" : "Criar"} Categoria
            </h1>

            <Card className="p-8">
              {/* Tipo */}
              <div className="mb-6">
                <label className="block text-purple-900 font-semibold mb-2">
                  Tipo (Nome Categoria):
                </label>
                <input
                  type="text"
                  value={tipo}
                  onChange={(e) => setTipo(e.target.value)}
                  className="w-full px-4 py-2 border border-gray-300 rounded bg-white text-gray-900"
                />
              </div>

              {/* SLA */}
              <div className="mb-6">
                <label className="block text-purple-900 font-semibold mb-2">
                  SLA (de Tratativa):
                </label>
                <input
                  type="text"
                  placeholder="Ex: 4h, 8h, 12h, 24h"
                  value={sla}
                  onChange={(e) => setSla(e.target.value)}
                  className="w-full px-4 py-2 border border-gray-300 rounded bg-white text-gray-900"
                />
              </div>

              {/* Nível de Suporte */}
              <div className="mb-6">
                <label className="block text-purple-900 font-semibold mb-2">
                  Nível de Suporte:
                </label>
                <select
                  value={nivelSuporte}
                  onChange={(e) => setNivelSuporte(e.target.value)}
                  className="w-full px-4 py-2 border border-gray-300 rounded bg-white text-gray-900"
                >
                  <option value="">Selecione...</option>
                  <option value="N1">N1</option>
                  <option value="N2">N2</option>
                  <option value="N3">N3</option>
                </select>
              </div>

              {/* Prioridade */}
              <div className="mb-6">
                <label className="block text-purple-900 font-semibold mb-2">
                  Prioridade:
                </label>
                <select
                  value={prioridade}
                  onChange={(e) => setPrioridade(e.target.value)}
                  className="w-full px-4 py-2 border border-gray-300 rounded bg-white text-gray-900"
                >
                  <option value="">Selecione...</option>
                  <option value="Baixa">Baixa</option>
                  <option value="Média">Média</option>
                  <option value="Alta">Alta</option>
                </select>
              </div>

              {/* Palavras-chave */}
              <div className="mb-6">
                <label className="block text-purple-900 font-semibold mb-2">
                  Palavras-Chave (IA):
                </label>
                <textarea
                  value={palavrasChave}
                  onChange={(e) => setPalavrasChave(e.target.value)}
                  placeholder="Ex: internet, rede, conexão"
                  className="w-full px-4 py-2 border border-gray-300 rounded bg-white text-gray-900 resize-none"
                  rows={3}
                />
              </div>

              {/* Sugestão de Solução */}
              <div className="mb-8">
                <label className="block text-purple-900 font-semibold mb-2">
                  Sugestão de Solução (IA):
                </label>
                <textarea
                  value={sugestaoSolucao}
                  onChange={(e) => setSugestaoSolucao(e.target.value)}
                  placeholder="Descreva a solução sugerida..."
                  className="w-full px-4 py-2 border border-gray-300 rounded bg-white text-gray-900 resize-none"
                  rows={5}
                />
              </div>

              {/* Botões */}
              <div className="flex gap-4">
                {categoriaSelecionada && (
                  <Button
                    onClick={handleExcluir}
                    className="flex-1 bg-red-600 hover:bg-red-700 text-white font-semibold py-2 rounded"
                  >
                    <Trash2 size={18} className="mr-2" />
                    Excluir Categoria
                  </Button>
                )}
                <Button
                  onClick={handleCancelar}
                  className="flex-1 bg-gray-400 hover:bg-gray-500 text-white font-semibold py-2 rounded"
                >
                  Cancelar
                </Button>
                <Button
                  onClick={handleSalvar}
                  className="flex-1 bg-purple-600 hover:bg-purple-700 text-white font-semibold py-2 rounded"
                >
                  Salvar Edições
                </Button>
              </div>
            </Card>
          </div>
        )}
      </div>
    </div>
  );
}
