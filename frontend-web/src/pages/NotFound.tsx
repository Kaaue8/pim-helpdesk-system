import { Button } from "@/components/ui/button";
import { Link } from "wouter";

export default function NotFound() {

  return (
    <div className="min-h-screen bg-gradient-to-br from-purple-600 to-purple-700 flex items-center justify-center p-4">
      <div className="text-center">
        <div className="mb-8">
          <h1 className="text-9xl font-bold text-white mb-4">404</h1>
          <h2 className="text-3xl font-bold text-purple-100 mb-2">
            Página não encontrada
          </h2>
          <p className="text-purple-200 mb-8">
            Desculpe, a página que você está procurando não existe ou foi removida.
          </p>
        </div>

        <div className="flex flex-col sm:flex-row gap-4 justify-center">
          <Link href="/">
            <Button className="bg-white text-purple-600 hover:bg-purple-50 px-8">
              Voltar para página inicial
            </Button>
          </Link>
          <Link href="/faq">
            <Button
              variant="outline"
              className="border-white text-white hover:bg-purple-600 px-8"
            >
              Ver FAQ
            </Button>
          </Link>
        </div>

        <div className="mt-12 text-purple-200 text-sm">
          <p>Código de erro: 404</p>
        </div>
      </div>
    </div>
  );
}
