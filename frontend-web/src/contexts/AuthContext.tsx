import React, { createContext, useContext, useState, useEffect } from "react";

export type UserType = "admin" | "user" | null;

interface AuthContextType {
  isLoggedIn: boolean;
  userType: UserType;
  userName: string;
  userEmail: string;
  login: (email: string, password: string ) => { success: boolean; error?: string };
  logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

// Dados de teste
const VALID_USERS = {
  admin: {
    email: "ju.vc1999@gmail.com",
    password: "Testandoapollo456@",
    name: "Loro José",
    type: "admin" as UserType,
  },
  user: {
    email: "testeapollo@gmail.com",
    password: "Testandoapollo456@",
    name: "Usuário Teste",
    type: "user" as UserType,
  },
};

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [userType, setUserType] = useState<UserType>(null);
  const [userName, setUserName] = useState("");
  const [userEmail, setUserEmail] = useState("");
  const [isLoading, setIsLoading] = useState(true);

  // Carregar dados do localStorage ao montar o componente
  useEffect(() => {
    try {
      const savedAuth = localStorage.getItem("auth");
      if (savedAuth) {
        const auth = JSON.parse(savedAuth);
        setIsLoggedIn(true);
        setUserType(auth.userType);
        setUserName(auth.userName);
        setUserEmail(auth.userEmail);
      }
    } catch (error) {
      console.error("Erro ao carregar autenticação:", error);
      localStorage.removeItem("auth");
    } finally {
      setIsLoading(false);
    }
  }, []);

  const login = (email: string, password: string) => {
    // Validar credenciais
    for (const [key, user] of Object.entries(VALID_USERS)) {
      if (user.email === email && user.password === password) {
        setIsLoggedIn(true);
        setUserType(user.type);
        setUserName(user.name);
        setUserEmail(user.email);

        // Salvar no localStorage
        localStorage.setItem(
          "auth",
          JSON.stringify({
            userType: user.type,
            userName: user.name,
            userEmail: user.email,
          })
        );

        return { success: true };
      }
    }

    // Se credenciais inválidas
    return {
      success: false,
      error: "Email ou senha incorretos",
    };
  };

  const logout = () => {
    setIsLoggedIn(false);
    setUserType(null);
    setUserName("");
    setUserEmail("");
    localStorage.removeItem("auth");
  };

  // Não renderizar até carregar o estado do localStorage
  if (isLoading) {
    return <div>Carregando...</div>;
  }

  return (
    <AuthContext.Provider
      value={{
        isLoggedIn,
        userType,
        userName,
        userEmail,
        login,
        logout,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
}

// Valor padrão seguro para quando o contexto não está disponível
const DEFAULT_AUTH: AuthContextType = {
  isLoggedIn: false,
  userType: null,
  userName: "User",
  userEmail: "",
  login: () => ({ success: false, error: "AuthProvider não disponível" }),
  logout: () => {},
};

export function useAuth() {
  const context = useContext(AuthContext);
  // Retornar valor padrão em vez de lançar erro
  return context || DEFAULT_AUTH;
}
