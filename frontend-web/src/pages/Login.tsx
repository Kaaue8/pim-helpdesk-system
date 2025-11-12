import { useState, useEffect } from "react";
import { Eye, EyeOff } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Card } from "@/components/ui/card";
import { useAuth } from "@/contexts/AuthContext";
import { useLocation } from "wouter";

export default function Login() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [showPassword, setShowPassword] = useState(false);
  const [agreedToTerms, setAgreedToTerms] = useState(false);
  const [loginError, setLoginError] = useState("");
  
  // Modal States
  const [showTerms, setShowTerms] = useState(false);
  const [showForgotPassword, setShowForgotPassword] = useState(false);
  const [showLinkSent, setShowLinkSent] = useState(false);
  const [forgotEmail, setForgotEmail] = useState("");

  const { login, isLoggedIn } = useAuth();
  const [, setLocation] = useLocation();

  // Se já está logado, redireciona para home
  useEffect(() => {
    if (isLoggedIn) {
      setLocation("/");
    }
  }, [isLoggedIn, setLocation]);

  const handleLogin = (e: React.FormEvent) => {
    e.preventDefault();
    setLoginError("");

    if (!agreedToTerms) {
      setLoginError("Você precisa concordar com os Termos de Uso e Política de Privacidade");
      return;
    }

    if (!email || !password) {
      setLoginError("Por favor, preencha email e senha");
      return;
    }

    const result = login(email, password);
    if (!result.success) {
      setLoginError(result.error || "Erro ao fazer login");
    }
  };

  const handleForgotPassword = (e: React.FormEvent) => {
    e.preventDefault();
    // TODO: Integrar com backend
    console.log("Recuperar senha para:", forgotEmail);
    setShowForgotPassword(false);
    setShowLinkSent(true);
  };

  return (
    <div 
      className="min-h-screen flex items-center justify-center p-4 relative overflow-hidden"
      style={{
        backgroundImage: "url('/fundo-tela-login.png')",
        backgroundSize: "cover",
        backgroundPosition: "center",
        backgroundAttachment: "fixed"
      }}
    >
      {/* Main Container - Card + Houston */}
      <div className="relative z-10 w-full max-w-5xl flex items-center justify-center gap-8">
        
        {/* Login Card */}
        <div className="w-full max-w-md">
          <Card className="bg-white/70 backdrop-blur-md rounded-3xl shadow-2xl p-5 border border-white/20">
            {/* Logo/Title */}
            <div className="text-center mb-6">
              <h1 className="text-2xl font-bold text-purple-900">HELPCENTER APOLLO</h1>
              <p className="text-sm text-purple-700 mt-2">
                Bem-vindo ao helpcenter, efetue seu login abaixo!
              </p>
            </div>

            {/* Login Form */}
            <form onSubmit={handleLogin} className="space-y-3">
              {/* Email Input */}
              <div>
                <label className="block text-sm font-medium text-purple-900 mb-2">
                  Email
                </label>
                <input
                  type="email"
                  value={email}
                  onChange={(e) => {
                    setEmail(e.target.value);
                    setLoginError("");
                  }}
                  placeholder="seu-email@example.com"
                  className="w-full px-4 py-2 rounded-lg border border-purple-200 bg-white/80 focus:outline-none focus:ring-2 focus:ring-purple-600 focus:border-transparent transition text-sm"
                  required
                />
              </div>

              {/* Password Input */}
              <div>
                <label className="block text-sm font-medium text-purple-900 mb-2">
                  Senha
                </label>
                <div className="relative">
                  <input
                    type={showPassword ? "text" : "password"}
                    value={password}
                    onChange={(e) => {
                      setPassword(e.target.value);
                      setLoginError("");
                    }}
                    placeholder="••••••••"
                    className="w-full px-4 py-2 rounded-lg border border-purple-200 bg-white/80 focus:outline-none focus:ring-2 focus:ring-purple-600 focus:border-transparent transition text-sm"
                    required
                  />
                  <button
                    type="button"
                    onClick={() => setShowPassword(!showPassword)}
                    className="absolute right-3 top-1/2 -translate-y-1/2 text-purple-600 hover:text-purple-700"
                  >
                    {showPassword ? <EyeOff size={18} /> : <Eye size={18} />}
                  </button>
                </div>
              </div>

              {/* Error Message */}
              {loginError && (
                <div className="p-3 bg-red-100 border border-red-300 rounded-lg text-sm text-red-800">
                  {loginError}
                </div>
              )}

              {/* Checkbox - Terms Agreement */}
              <div className="flex items-start gap-2 mt-4">
                <input
                  type="checkbox"
                  id="terms"
                  checked={agreedToTerms}
                  onChange={(e) => setAgreedToTerms(e.target.checked)}
                  className="mt-1 w-4 h-4 rounded border-purple-300 text-purple-600 focus:ring-purple-500 cursor-pointer"
                />
                <label htmlFor="terms" className="text-xs text-purple-900 cursor-pointer leading-tight">
                  Li e concordo com os{" "}
                  <button
                    type="button"
                    onClick={() => setShowTerms(true)}
                    className="text-purple-600 hover:text-purple-700 underline font-medium"
                  >
                    Termos de Uso
                  </button>
                  {" "}e a{" "}
                  <button
                    type="button"
                    onClick={() => setShowTerms(true)}
                    className="text-purple-600 hover:text-purple-700 underline font-medium"
                  >
                    Política de Privacidade
                  </button>
                  {" "}(LGPD)
                </label>
              </div>

              {/* Login Button */}
              <Button
                type="submit"
                disabled={!agreedToTerms}
                className={`w-full font-semibold py-2 rounded-lg transition mt-4 text-sm ${
                  agreedToTerms
                    ? "bg-purple-600 hover:bg-purple-700 text-white cursor-pointer"
                    : "bg-purple-300 text-purple-600 cursor-not-allowed opacity-60"
                }`}
              >
                Acessar
              </Button>
            </form>

            {/* Forgot Password Link */}
            <div className="mt-3 text-center">
              <button
                type="button"
                onClick={() => setShowForgotPassword(true)}
                className="text-xs text-purple-600 hover:text-purple-700 font-medium underline"
              >
                Esqueci minha senha
              </button>
            </div>
          </Card>
        </div>

        {/* Houston Mascot - Ao lado do card */}
        <div className="hidden lg:flex flex-col items-center justify-center">
          <img
            src="/houston.png"
            alt="Houston - Mascote"
            className="w-56 h-56 object-contain drop-shadow-lg"
          />
          <p className="text-sm text-white font-medium text-center mt-4 max-w-xs drop-shadow-md">
            Esse aqui é o Houston, ele sempre te ajudará com seus chamados!
          </p>
        </div>
      </div>

      {/* MODAL: Terms of Service */}
      {showTerms && (
        <div className="fixed inset-0 bg-black/50 flex items-center justify-center p-4 z-50">
          <Card className="bg-white rounded-2xl shadow-2xl max-w-2xl max-h-[80vh] overflow-y-auto">
            <div className="p-8">
              <div className="flex justify-between items-center mb-6">
                <h2 className="text-2xl font-bold text-gray-900">
                  Termos de Uso e Política de Privacidade
                </h2>
                <button
                  onClick={() => setShowTerms(false)}
                  className="text-gray-500 hover:text-gray-700 text-2xl"
                >
                  ✕
                </button>
              </div>

              <div className="space-y-4 text-sm text-gray-700 leading-relaxed">
                <div>
                  <h3 className="font-semibold text-gray-900 mb-2">1. Finalidade do Aplicativo</h3>
                  <p>
                    Este aplicativo tem como objetivo permitir que usuários registrem chamados de suporte técnico e acompanhem o andamento das solicitações de forma prática e segura.
                  </p>
                </div>

                <div>
                  <h3 className="font-semibold text-gray-900 mb-2">2. Coleta de Dados Pessoais</h3>
                  <p className="mb-2">Coletamos os seguintes dados pessoais:</p>
                  <ul className="list-disc list-inside space-y-1 ml-2">
                    <li>Nome completo</li>
                    <li>E-mail</li>
                    <li>Telefone (se fornecido)</li>
                    <li>Dados do dispositivo (para suporte técnico)</li>
                    <li>Informações inseridas nos chamados</li>
                  </ul>
                  <p className="mt-2">Esses dados são utilizados exclusivamente para:</p>
                  <ul className="list-disc list-inside space-y-1 ml-2">
                    <li>Identificar o usuário</li>
                    <li>Processar e responder chamados</li>
                    <li>Oferecer sugestões automáticas com base em IA</li>
                    <li>Melhorar continuamente o serviço</li>
                  </ul>
                </div>

                <div>
                  <h3 className="font-semibold text-gray-900 mb-2">3. Base Legal para o Tratamento de Dados</h3>
                  <p>
                    O tratamento dos dados pessoais ocorre com base no seu consentimento, conforme exigido pela Lei Geral de Proteção de Dados Pessoais (LGPD - Lei nº 13.709/2018).
                  </p>
                </div>

                <div>
                  <h3 className="font-semibold text-gray-900 mb-2">4. Compartilhamento de Dados</h3>
                  <p>
                    Seus dados não serão compartilhados com terceiros, exceto quando exigido por lei ou ordem judicial.
                  </p>
                </div>

                <div>
                  <h3 className="font-semibold text-gray-900 mb-2">5. Segurança dos Dados</h3>
                  <p>
                    Adotamos medidas técnicas e organizacionais adequadas para proteger seus dados pessoais contra acesso não autorizado, vazamento ou uso indevido.
                  </p>
                </div>

                <div>
                  <h3 className="font-semibold text-gray-900 mb-2">6. Seus Direitos (Art. 18 da LGPD)</h3>
                  <p className="mb-2">Você tem o direito de:</p>
                  <ul className="list-disc list-inside space-y-1 ml-2">
                    <li>Acessar seus dados pessoais</li>
                    <li>Corrigir dados incompletos ou desatualizados</li>
                    <li>Revogar seu consentimento a qualquer momento</li>
                    <li>Solicitar a exclusão dos dados</li>
                    <li>Obter informações sobre o uso de seus dados</li>
                  </ul>
                  <p className="mt-2">Essas ações podem ser realizadas na seção Configurações de Privacidade do aplicativo.</p>
                </div>

                <div>
                  <h3 className="font-semibold text-gray-900 mb-2">7. Uso de Inteligência Artificial</h3>
                  <p>
                    Utilizamos inteligência artificial para sugerir soluções automáticas e classificar chamados. As sugestões são auxiliares e não substituem a análise humana.
                  </p>
                </div>

                <div>
                  <h3 className="font-semibold text-gray-900 mb-2">8. Alterações</h3>
                  <p>
                    Este documento poderá ser atualizado para refletir melhorias no sistema ou alterações legais. A versão mais recente estará sempre disponível no aplicativo.
                  </p>
                </div>
              </div>
            </div>
          </Card>
        </div>
      )}

      {/* MODAL: Forgot Password */}
      {showForgotPassword && (
        <div className="fixed inset-0 bg-black/50 flex items-center justify-center p-4 z-50">
          <Card className="bg-white rounded-2xl shadow-2xl max-w-md">
            <div className="p-8">
              <div className="flex justify-between items-center mb-6">
                <h2 className="text-2xl font-bold text-gray-900">
                  Recuperar Senha
                </h2>
                <button
                  onClick={() => setShowForgotPassword(false)}
                  className="text-gray-500 hover:text-gray-700 text-2xl"
                >
                  ✕
                </button>
              </div>

              <form onSubmit={handleForgotPassword} className="space-y-4">
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-2">
                    Email
                  </label>
                  <input
                    type="email"
                    value={forgotEmail}
                    onChange={(e) => setForgotEmail(e.target.value)}
                    placeholder="seu-email@example.com"
                    className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-transparent outline-none transition"
                    required
                  />
                </div>

                <Button
                  type="submit"
                  className="w-full bg-purple-600 hover:bg-purple-700 text-white"
                >
                  Enviar Link de Recuperação
                </Button>
              </form>
            </div>
          </Card>
        </div>
      )}

      {/* MODAL: Link Sent */}
      {showLinkSent && (
        <div className="fixed inset-0 bg-black/50 flex items-center justify-center p-4 z-50">
          <Card className="bg-white rounded-2xl shadow-2xl max-w-md">
            <div className="p-8 text-center">
              <div className="mb-4">
                <div className="w-16 h-16 bg-green-100 rounded-full flex items-center justify-center mx-auto">
                  <svg className="w-8 h-8 text-green-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 13l4 4L19 7" />
                  </svg>
                </div>
              </div>

              <h2 className="text-2xl font-bold text-gray-900 mb-2">
                Email Enviado!
              </h2>
              <p className="text-gray-600 mb-6">
                Verifique seu email para o link de recuperação de senha.
              </p>

              <Button
                onClick={() => {
                  setShowLinkSent(false);
                  setShowForgotPassword(false);
                  setForgotEmail("");
                }}
                className="w-full bg-purple-600 hover:bg-purple-700 text-white"
              >
                Voltar ao Login
              </Button>
            </div>
          </Card>
        </div>
      )}
    </div>
  );
}
