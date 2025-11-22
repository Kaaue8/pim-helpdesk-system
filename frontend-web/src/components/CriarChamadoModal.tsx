import { useState, useEffect } from "react";
import { X } from "lucide-react";
import { Button } from "@/components/ui/button";
import { apiCall } from "@/lib/api";

interface CategoriasChamados {
  idCategoria: number;
  categoria: string;
  sla: string;
  nivel: string;
  prioridade: string;
  palavrasChave?: string;
  termosNegativos?: string;
  exemploDescricao?: string;
  sugestaoSolucao?: string;
  passosSolucao?: string;
  artigoBaseConhecimento?: string;
}

interface CriarChamadoModalProps {
  isOpen: boolean;
  onClose: () => void;
  onChamadoCreated?: () => void;
}

export default function CriarChamadoModal({
  isOpen,
  onClose,
  onChamadoCreated,
}: CriarChamadoModalProps) {
  const [step, setStep] = useState(1);
  const [categorias, setCategorias] = useState<CategoriasChamados[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  // Form data
  const [selectedCategoria, setSelectedCategoria] = useState<number | null>(null);
  const [descricao, setDescricao] = useState("");
  const [arquivo, setArquivo] = useState<File | null>(null);
  const [nomeArquivo, setNomeArquivo] = useState("");

  // Fetch categories on mount
  useEffect(() => {
    if (isOpen && categorias.length === 0) {
      fetchCategorias();
    }
  }, [isOpen]);

  const fetchCategorias = async () => {
    try {
      setLoading(true);
      setError("");
      const data = await apiCall("/CategoriasChamados");
      setCategorias(data);
    } catch (err) {
      setError("Erro ao carregar categorias");
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file) {
      setArquivo(file);
      setNomeArquivo(file.name);
    }
  };

  const handleAvancar = () => {
    if (!selectedCategoria || !descricao.trim()) {
      setError("Selecione uma categoria e descreva o problema");
      return;
    }
    setError("");
    setStep(2);
  };

  const handleConfirmar = async () => {
    try {
      setLoading(true);
      setError("");

      const formData = new FormData();
      formData.append("categoriaId", selectedCategoria?.toString() || "");
      formData.append("descricao", descricao);
      if (arquivo) {
        formData.append("arquivo", arquivo);
      }

      const response = await fetch("/api/Tickets", {
        method: "POST",
        headers: {
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
        body: formData,
      });

      if (!response.ok) {
        throw new Error("Erro ao criar chamado");
      }

      // Success - show confirmation screen
      setStep(3);
      setTimeout(() => {
        onChamadoCreated?.();
        handleClose();
      }, 2000);
    } catch (err) {
      setError("Erro ao criar chamado. Tente novamente.");
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleClose = () => {
    setStep(1);
    setSelectedCategoria(null);
    setDescricao("");
    setArquivo(null);
    setNomeArquivo("");
    setError("");
    onClose();
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50">
      <div className="bg-white rounded-lg shadow-lg w-full max-w-md max-h-[90vh] overflow-y-auto">
        {/* Header com X */}
        <div className="flex justify-end p-4 border-b">
          <button
            onClick={handleClose}
            className="text-gray-500 hover:text-gray-700"
          >
            <X size={24} />
          </button>
        </div>

        {/* Step 1: Category Selection */}
        {step === 1 && (
          <div className="p-6 space-y-4">
            <h2 className="text-xl font-bold">Abrir Novo Chamado</h2>

            {error && (
              <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-2 rounded">
                {error}
              </div>
            )}

            {loading ? (
              <div className="text-center py-8">Carregando categorias...</div>
            ) : (
              <>
                <div>
                  <label className="block text-sm font-medium mb-2">
                    Categoria do Chamado
                  </label>
                  <select
                    value={selectedCategoria || ""}
                    onChange={(e) => setSelectedCategoria(Number(e.target.value))}
                    className="w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
                  >
                    <option value="">Selecione uma categoria...</option>
                    {categorias.map((cat) => (
                      <option key={cat.idCategoria} value={cat.idCategoria}>
                        {cat.categoria}
                      </option>
                    ))}
                  </select>
                </div>

                <div>
                  <label className="block text-sm font-medium mb-2">
                    Descrição do Problema
                  </label>
                  <textarea
                    value={descricao}
                    onChange={(e) => setDescricao(e.target.value)}
                    placeholder="Descreva detalhadamente o problema..."
                    className="w-full border rounded px-3 py-2 h-32 focus:outline-none focus:ring-2 focus:ring-blue-500"
                  />
                </div>

                <Button
                  onClick={handleAvancar}
                  className="w-full bg-blue-600 hover:bg-blue-700 text-white"
                >
                  Avançar para abrir chamado
                </Button>
              </>
            )}
          </div>
        )}

        {/* Step 2: Additional Details */}
        {step === 2 && (
          <div className="p-6 space-y-4">
            <h2 className="text-xl font-bold">Detalhes Adicionais</h2>

            {error && (
              <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-2 rounded">
                {error}
              </div>
            )}

            <div>
              <label className="block text-sm font-medium mb-2">
                Anexar Arquivo (Opcional)
              </label>
              <input
                type="file"
                onChange={handleFileChange}
                className="w-full border rounded px-3 py-2"
              />
              {nomeArquivo && (
                <p className="text-sm text-gray-600 mt-2">
                  Arquivo: {nomeArquivo}
                </p>
              )}
            </div>

            <div className="bg-blue-50 border border-blue-200 rounded p-4">
              <p className="text-sm text-gray-700">
                <strong>Categoria:</strong>{" "}
                {categorias.find((c) => c.idCategoria === selectedCategoria)
                  ?.categoria || ""}
              </p>
              <p className="text-sm text-gray-700 mt-2">
                <strong>Descrição:</strong> {descricao}
              </p>
            </div>

            <div className="flex gap-3">
              <Button
                onClick={() => setStep(1)}
                variant="outline"
                className="flex-1"
              >
                Voltar
              </Button>
              <Button
                onClick={handleConfirmar}
                disabled={loading}
                className="flex-1 bg-green-600 hover:bg-green-700 text-white"
              >
                {loading ? "Criando..." : "Confirmar"}
              </Button>
            </div>
          </div>
        )}

        {/* Step 3: Confirmation */}
        {step === 3 && (
          <div className="p-6 text-center space-y-4">
            <div className="text-5xl">✅</div>
            <h2 className="text-xl font-bold">Chamado Criado com Sucesso!</h2>
            <p className="text-gray-600">
              Seu chamado foi registrado e será analisado em breve.
            </p>
          </div>
        )}
      </div>
    </div>
  );
}
