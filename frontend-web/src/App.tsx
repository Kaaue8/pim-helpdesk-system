import { Toaster } from "@/components/ui/sonner";
import { TooltipProvider } from "@/components/ui/tooltip";
import { Route, Switch } from "wouter";
import ErrorBoundary from "./components/ErrorBoundary";
import { ThemeProvider } from "./contexts/ThemeContext";
import { AuthProvider } from "./contexts/AuthContext";
import ProtectedRoute from "./components/ProtectedRoute";

// Páginas
import Login from "./pages/Login";
import Home from "./pages/Home";
import TodosChamados from "./pages/TodosChamados";
import HistoricoChamados from "./pages/HistoricoChamados";
import FAQ from "./pages/FAQ";
import FilaChamados from "./pages/FilaChamados";
import Usuarios from "./pages/Usuarios";
import Admin from "./pages/Admin";
import NotFound from "./pages/NotFound";

function Router() {
  return (
    <Switch>
      {/* Rota pública */}
      <Route path="/login" component={Login} />

      {/* Rotas protegidas */}
      <Route path="/">
        <ProtectedRoute>
          <Home />
        </ProtectedRoute>
      </Route>

      <Route path="/chamados">
        <ProtectedRoute requiredRole="user">
          <TodosChamados />
        </ProtectedRoute>
      </Route>

      <Route path="/historico-chamados">
        <ProtectedRoute requiredRole="admin">
          <HistoricoChamados />
        </ProtectedRoute>
      </Route>

      <Route path="/fila-chamados">
        <ProtectedRoute requiredRole="admin">
          <FilaChamados />
        </ProtectedRoute>
      </Route>

      <Route path="/usuarios">
        <ProtectedRoute requiredRole="admin">
          <Usuarios />
        </ProtectedRoute>
      </Route>

      <Route path="/faq">
        <ProtectedRoute>
          <FAQ />
        </ProtectedRoute>
      </Route>

      <Route path="/admin">
        <ProtectedRoute requiredRole="admin">
          <Admin />
        </ProtectedRoute>
      </Route>

      {/* Rota 404 */}
      <Route component={NotFound} />
    </Switch>
  );
}

function App() {
  return (
    <ErrorBoundary>
      <AuthProvider>
        <ThemeProvider defaultTheme="light">
          <TooltipProvider>
            <Toaster />
            <Router />
          </TooltipProvider>
        </ThemeProvider>
      </AuthProvider>
    </ErrorBoundary>
  );
}

export default App;