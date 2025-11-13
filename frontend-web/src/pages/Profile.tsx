import { useState } from "react";
import { Button } from "@/components/ui/button";
import { Card } from "@/components/ui/card";
import { Edit2, LogOut, Eye, EyeOff } from "lucide-react";
import { toast } from "sonner";
import { useAuth } from "@/contexts/AuthContext";
import { useLocation } from "wouter";
import Header from "@/components/Header";

export default function Profile() {
  const { userName, userEmail, userType, logout } = useAuth();
  const [, setLocation] = useLocation();

  // Estados de edição
  const [isEditingNome, setIsEditingNome] = useState(false);
  const [isEditingEmail, setIsEditingEmail] = useState(false);
  const [isEditingCelular, setIsEditingCelular] = useState(false);
  const [isEditingSetor, setIsEditingSetor] = useState(false);

  // Estados de valores
  const [nome, setNome] = useState(userName || "");
  const [email, setEmail] = useState(userEmail || "");
  const [celular, setCelular] = useState("(11) 99999-9999");
  const [setor, setSetor] = useState("Financeiro");

  // Estados de modais
  const [isLGPDOpen, setIsLGPDOpen] = useState(false);
  const [showPassword, setShowPassword] = useState(false);

  const isAdmin = userType === "admin";

  const handleSalvar = () => {
    toast.success("Dados atualizados com sucesso!");
    setIsEditingNome(false);
    setIsEditingEmail(false);
    setIsEditingCelular(false);
    setIsEditingSetor(false);
  };

  const handleRedefinirSenha = () => {
    toast.success("Link de redefinição enviado para seu email!");
  };

  const handleLogout = () => {
    logout();
    setLocation("/login");
  };

  return (
    <div className="min-h-screen bg-gray-100">
      <Header userName={userName} />

      <div className="max-w-4xl mx-auto px-4 py-8">
        <h1 className="text-4xl font-bold text-purple-900 mb-8 text-center">
          Editar perfil
        </h1>

        {/* Formulário Principal */}
        <Card className={`p-8 mb-8 ${isAdmin ? "border-2 border-grey-500" : ""}`}>
          {/* Matrícula */}
          <div className="mb-6">
            <label className="block text-purple-900 font-semibold mb-2">
              Matrícula:
            </label>
            <input
              type="text"
              value="MAT-001"
              disabled
              className="w-full px-4 py-2 bg-gray-200 text-gray-700 rounded border border-gray-300"
            />
          </div>

          {/* Nome & Sobrenome */}
          <div className="mb-2">
            <label className="block text-purple-900 font-semibold mb-2">
              Nome & Sobrenome:
            </label>
            <div className="flex items-center gap-2">
              <input
                type="text"
                value={nome}
                onChange={(e) => setNome(e.target.value)}
                disabled={!isEditingNome || !isAdmin}
                className={`flex-1 px-4 py-2 rounded border border-gray-300 ${
                  isEditingNome && isAdmin
                    ? "bg-white text-gray-900"
                    : "bg-gray-200 text-gray-700"
                }`}
              />
              {isAdmin && (
                <button
                  onClick={() => setIsEditingNome(!isEditingNome)}
                  className="text-orange-500 hover:text-orange-700 p-2"
                >
                  <Edit2 size={20} className="text-purple-600 hover:text-purple-800" />
                </button>
              )}
            </div>
          </div>

          {/* Email Corporativo */}
          <div className="mb-2">
            <label className="block text-purple-900 font-semibold mb-2">
              Email corporativo:
            </label>
            <div className="flex items-center gap-2">
              <input
                type="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                disabled={!isEditingEmail || !isAdmin}
                className={`flex-1 px-4 py-2 rounded border border-gray-300 ${
                  isEditingEmail && isAdmin
                    ? "bg-white text-gray-900"
                    : "bg-gray-200 text-gray-700"
                }`}
              />
              {isAdmin && (
                <button
                  onClick={() => setIsEditingEmail(!isEditingEmail)}
                  className="text-orange-500 hover:text-orange-700 p-2"
                >
                  <Edit2 size={20} className="text-purple-600 hover:text-purple-800"/>
                </button>
              )}
            </div>
          </div>

          {/* Celular */}
          <div className="mb-2">
            <label className="block text-purple-900 font-semibold mb-2">
              Celular:
            </label>
            <div className="flex items-center gap-2">
              <input
                type="tel"
                value={celular}
                onChange={(e) => setCelular(e.target.value)}
                disabled={!isEditingCelular}
                className={`flex-1 px-4 py-2 rounded border border-gray-300 ${
                  isEditingCelular
                    ? "bg-white text-gray-900"
                    : "bg-gray-200 text-gray-700"
                }`}
              />
              <button
                onClick={() => setIsEditingCelular(!isEditingCelular)}
                className="text-orange-500 hover:text-orange-700 p-2"
              >
                <Edit2 size={20} className="text-purple-600 hover:text-purple-800"/>
              </button>
            </div>
          </div>

          {/* Setor */}
          <div className="mb-6">
            <label className="block text-purple-900 font-semibold mb-2">
              Setor:
            </label>
            {isAdmin && isEditingSetor ? (
              <div className="flex items-center gap-2">
                <select
                  value={setor}
                  onChange={(e) => setSetor(e.target.value)}
                  className="flex-1 px-4 py-2 border border-gray-300 rounded bg-white text-gray-900"
                >
                  <option>Financeiro</option>
                  <option>RH</option>
                  <option>TI</option>
                  <option>Suporte</option>
                </select>
                <button
                  onClick={() => setIsEditingSetor(!isEditingSetor)}
                  className="text-orange-500 hover:text-orange-700 p-2"
                >
                  <Edit2 size={20} className="text-purple-600 hover:text-purple-800" />
                </button>
              </div>
            ) : (
              <div className="flex items-center gap-2">
                <input
                  type="text"
                  value={setor}
                  disabled
                  className="flex-1 px-4 py-2 bg-gray-200 text-gray-700 rounded border border-gray-300"
                />
                {isAdmin && (
                  <button
                    onClick={() => setIsEditingSetor(!isEditingSetor)}
                    className="text-orange-500 hover:text-orange-700 p-2"
                  >
                    <Edit2 size={20} className="text-purple-600 hover:text-purple-800" />
                  </button>
                )}
              </div>
            )}
          </div>

          {/* Botão Salvar */}
          <div className="mb-6">
            <Button
              onClick={handleSalvar}
              className="w-full bg-purple-600 hover:bg-purple-700 text-white font-semibold py-2 rounded"
            >
              Salvar Alterações
            </Button>
          </div>

          {/* Links de Ação */}
          <div className="space-y-3 border-t pt-6">
            <button
              onClick={handleRedefinirSenha}
              className="block w-full text-left text-purple-600 hover:text-purple-800 font-semibold py-2 px-4 bg-purple-50 rounded hover:bg-purple-100 transition"
            >
              Redefinir Senha
            </button>

            <button
              onClick={() => setIsLGPDOpen(true)}
              className="block w-full text-left text-purple-600 hover:text-purple-800 font-semibold py-2 px-4 bg-purple-50 rounded hover:bg-purple-100 transition"
            >
              Ver termos de uso e consentimento (LGPD)
            </button>

            <button
              onClick={handleLogout}
              className="flex items-center gap-2 w-full text-left text-red-600 hover:text-red-800 font-semibold py-2 px-4 bg-red-50 rounded hover:bg-red-100 transition"
            >
              <LogOut size={18} />
              Sair da conta
            </button>
          </div>
        </Card>
      </div>

      {/* Modal LGPD */}
      {isLGPDOpen && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
          <Card className="w-full max-w-2xl p-8 max-h-96 overflow-y-auto mx-4">
            <div className="flex justify-between items-center mb-4">
              <h2 className="text-2xl font-bold text-purple-900">
                Termos de Uso e Consentimento
              </h2>
              <button
                onClick={() => setIsLGPDOpen(false)}
                className="text-gray-600 hover:text-gray-900 text-2xl"
              >
                ✕
              </button>
            </div>

            <div className="text-sm text-gray-700 space-y-4 mb-6">
              <p>
                <strong>Lei Geral de Proteção de Dados (LGPD)</strong>
              </p>
              <p>
                O sistema está totalmente em conformidade com a LGPD (Lei Geral de Proteção de Dados). Todos os dados pessoais fornecidos durante a abertura e o acompanhamento de chamados — como nome, e-mail, IP, setor e informações do problema — são armazenados de forma segura, criptografada e acessível apenas por usuários autorizados.
              </p>
              <p>
                A coleta de dados é feita exclusivamente para fins de atendimento técnico, análise de desempenho e melhoria do serviço. Nenhuma informação é compartilhada com terceiros sem consentimento.
              </p>
              <p>
                Além disso, mantemos políticas rígidas de segurança da informação, com controles de acesso, registros de auditoria e monitoramento constante para prevenir qualquer tipo de vazamento ou uso indevido.
              </p>
              <p>
                <strong>Transparência, sigilo e responsabilidade</strong> fazem parte da nossa política de proteção de dados.
              </p>
            </div>

            <Button
              onClick={() => setIsLGPDOpen(false)}
              className="w-full bg-purple-600 hover:bg-purple-700 text-white font-semibold py-2 rounded"
            >
              Entendi
            </Button>
          </Card>
        </div>
      )}
    </div>
  );
}
