import React, { createContext, useContext, useState, useEffect } from "react";

export type UserType = "admin" | "user" | null;

// CORREÇÃO 1: Adicionado 'isLoading' à interface
interface AuthContextType {
  isLoggedIn: boolean;
  userType: UserType;
  userName: string;
  userEmail: string;
  token: string | null;
  isLoading: boolean; // Adicionado para que outros componentes saibam o estado de carregamento
  login: (email: string, password: string) => Promise<{ success: boolean; error?: string }>;
  logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

const API_BASE_URL = "http://localhost:5079/api";

export function AuthProvider({ children }: { children: React.ReactNode }  ) {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [userType, setUserType] = useState<UserType>(null);
  const [userName, setUserName] = useState("");
  const [userEmail, setUserEmail] = useState("");
  const [token, setToken] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(true); // Esta variável já existia, agora vamos exportá-la

  useEffect(() => {
    try {
      const savedAuth = localStorage.getItem("auth");
      const savedToken = localStorage.getItem("token");
      
      if (savedAuth && savedToken) {
        const auth = JSON.parse(savedAuth);
        setIsLoggedIn(true);
        setUserType(auth.userType);
        setUserName(auth.userName);
        setUserEmail(auth.userEmail);
        setToken(savedToken);
      }
    } catch (error) {
      console.error("Erro ao carregar autenticação:", error);
      localStorage.removeItem("auth");
      localStorage.removeItem("token");
    } finally {
      setIsLoading(false);
    }
  }, []);

  const login = async (emailParam: string, passwordParam: string) => {
    try {
      setIsLoading(true);

      const response: Response = await fetch(`${API_BASE_URL}/Login/Authenticate`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          email: emailParam,
          Senha: passwordParam,
        }),
      });

      if (!response.ok) {
        const errorData = await response.json().catch(() => ({}));
        return {
          success: false,
          error: errorData.message || "Email ou senha incorretos",
        };
      }

      const data: any = await response.json();
      const { token: jwtToken, nome, email: emailFromResponse, perfil } = data;

      if (!jwtToken || !emailFromResponse) {
        return {
          success: false,
          error: "Resposta inválida do servidor",
        };
      }

      const perfilNormalizado = perfil.toLowerCase();
      const userTypeFromBackend = (perfilNormalizado === "admin" || perfilNormalizado === "analista") ? "admin" : "user";

      setIsLoggedIn(true);
      setUserType(userTypeFromBackend);
      setUserName(nome || "Usuário");
      setUserEmail(emailFromResponse);
      setToken(jwtToken);

      localStorage.setItem(
        "auth",
        JSON.stringify({
          userType: userTypeFromBackend,
          userName: nome,
          userEmail: emailFromResponse,
        })
      );
      localStorage.setItem("token", jwtToken);

      return { success: true };
    } catch (error) {
      console.error("Erro ao fazer login:", error);
      return {
        success: false,
        error: "Erro ao conectar ao servidor. Tente novamente.",
      };
    } finally {
      setIsLoading(false);
    }
  };

  const logout = () => {
    setIsLoggedIn(false);
    setUserType(null);
    setUserName("");
    setUserEmail("");
    setToken(null);
    localStorage.removeItem("auth");
    localStorage.removeItem("token");
  };

  if (isLoading) {
    return (
      <div className="flex items-center justify-center min-h-screen">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-purple-600 mx-auto mb-4"></div>
          <p className="text-gray-600">Carregando...</p>
        </div>
      </div>
    );
  }

  return (
    <AuthContext.Provider
      // CORREÇÃO 2: Adicionado 'isLoading' ao objeto de valor compartilhado
      value={{
        isLoggedIn,
        userType,
        userName,
        userEmail,
        token,
        isLoading, // Agora 'isLoading' é acessível por qualquer componente que use 'useAuth'
        login,
        logout,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
}

// CORREÇÃO 3: Adicionado 'isLoading' ao objeto padrão
const DEFAULT_AUTH: AuthContextType = {
  isLoggedIn: false,
  userType: null,
  userName: "User",
  userEmail: "",
  token: null,
  isLoading: true, // Inicia como 'true' por segurança
  login: async () => ({ success: false, error: "AuthProvider não disponível" }),
  logout: () => {},
};

export function useAuth() {
  const context = useContext(AuthContext);
  return context || DEFAULT_AUTH;
}
