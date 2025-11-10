import { useState } from "react";
import { Menu, X, LogOut } from "lucide-react";
import { Link, useLocation } from "wouter";
import { useAuth } from "@/contexts/AuthContext";

interface HeaderProps {
  userName?: string;
}

export default function Header({ userName = "Usuário" }: HeaderProps) {
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const auth = useAuth();
  const [, setLocation] = useLocation();

  const handleLogout = () => {
    auth.logout();
    setLocation("/login");
  };

  // Menu items variam conforme o tipo de usuário
  const menuItems = auth.userType === "admin" 
    ? [
        { label: "Fila Chamados", href: "/fila-chamados" },
        { label: "Histórico Chamados", href: "/historico-chamados" },
        { label: "Dashboard", href: "/" },
        { label: "Usuários", href: "/admin" },
        { label: "FAQ", href: "/faq" },
        { label: "Categorias", href: "/admin" },
      ]
    : [
        { label: "Todos os Chamados", href: "/chamados" },
        { label: "FAQ", href: "/faq" },
      ];

  return (
    <header className="bg-gradient-to-r from-purple-700 to-purple-600 text-white shadow-lg">
      <div className="w-full px-4 py-4">
        <div className="flex items-center justify-between gap-8">
          {/* Logo/Brand */}
          <Link href="/" className="text-xl font-bold hover:text-purple-100 transition whitespace-nowrap">
            Helpcenter Apollo
          </Link>

          {/* Menu - Desktop */}
          <nav className="hidden md:flex items-center gap-6 flex-1">
            {menuItems.map((item) => (
              <Link
                key={item.href}
                href={item.href}
                className="text-sm font-medium hover:text-purple-100 transition whitespace-nowrap"
              >
                {item.label}
              </Link>
            ))}
          </nav>

          {/* User Profile - Desktop */}
          <div className="hidden md:flex items-center gap-4 whitespace-nowrap">
            <div className="flex items-center gap-2">
              <div className="w-10 h-10 bg-white/20 rounded-full flex items-center justify-center border-2 border-white">
                <span className="text-sm font-bold">{userName.charAt(0).toUpperCase()}</span>
              </div>
              <span className="text-sm font-medium">{userName}</span>
            </div>
            <button 
              onClick={handleLogout}
              className="p-2 hover:bg-white/10 rounded-lg transition"
              title="Sair"
            >
              <LogOut size={18} />
            </button>
          </div>

          {/* Mobile Menu Button */}
          <button
            onClick={() => setIsMenuOpen(!isMenuOpen)}
            className="md:hidden p-2 hover:bg-white/10 rounded-lg transition"
          >
            {isMenuOpen ? <X size={24} /> : <Menu size={24} />}
          </button>
        </div>

        {/* Mobile Menu */}
        {isMenuOpen && (
          <nav className="md:hidden mt-4 space-y-2 pb-4">
            {menuItems.map((item) => (
              <Link
                key={item.href}
                href={item.href}
                onClick={() => setIsMenuOpen(false)}
                className="block px-4 py-2 text-sm font-medium hover:bg-white/10 rounded-lg transition"
              >
                {item.label}
              </Link>
            ))}
            <div className="px-4 py-2 border-t border-white/20 mt-2 pt-2">
              <div className="flex items-center gap-2 mb-2">
                <div className="w-8 h-8 bg-white/20 rounded-full flex items-center justify-center border-2 border-white">
                  <span className="text-xs font-bold">{userName.charAt(0).toUpperCase()}</span>
                </div>
                <span className="text-sm font-medium">{userName}</span>
              </div>
              <button 
                onClick={handleLogout}
                className="w-full flex items-center gap-2 px-4 py-2 text-sm hover:bg-white/10 rounded-lg transition"
              >
                <LogOut size={16} />
                Sair
              </button>
            </div>
          </nav>
        )}
      </div>
    </header>
  );
}
