import { useAuth } from "@/contexts/AuthContext";
import { useLocation } from "wouter";
import { useEffect } from "react";

interface ProtectedRouteProps {
  children: React.ReactNode;
  requiredRole?: "admin" | "user";
}

export default function ProtectedRoute({
  children,
  requiredRole,
}: ProtectedRouteProps) {
  const { isLoggedIn, userType } = useAuth();
  const [, setLocation] = useLocation();

  useEffect(() => {
    // Se não está logado, redireciona para login
    if (!isLoggedIn) {
      setLocation("/login");
      return;
    }

    // Se requer um role específico e o usuário não tem, redireciona para home
    if (requiredRole && userType !== requiredRole) {
      setLocation("/");
      return;
    }
  }, [isLoggedIn, userType, requiredRole, setLocation]);

  // Se não está logado, não renderiza nada (o efeito vai redirecionar)
  if (!isLoggedIn) {
    return null;
  }

  // Se requer um role específico e não tem, não renderiza
  if (requiredRole && userType !== requiredRole) {
    return null;
  }

  return <>{children}</>;
}
