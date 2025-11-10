# Guia de Desenvolvimento - PIM 3

Este documento fornece informações detalhadas para desenvolvedores que desejam contribuir ou estender o projeto PIM 3.

## 🏗️ Arquitetura

### Estrutura de Componentes

O projeto segue a arquitetura de componentes React com as seguintes camadas:

1. **Pages** - Componentes de página de alto nível
2. **Components** - Componentes reutilizáveis
3. **UI Components** - Componentes base do shadcn/ui
4. **Hooks** - Lógica reutilizável
5. **Contexts** - Estado global

### Padrões de Código

#### Componentes Funcionais

Todos os componentes devem ser funcionais com hooks:

```typescript
import { useState } from "react";

export default function MeuComponente() {
  const [state, setState] = useState("");

  return <div>{state}</div>;
}
```

#### Tipagem TypeScript

Sempre use tipos explícitos:

```typescript
interface Props {
  title: string;
  onClick: (id: number) => void;
}

export default function Botao({ title, onClick }: Props) {
  return <button onClick={() => onClick(1)}>{title}</button>;
}
```

## 🎨 Styling

### Tailwind CSS

Use classes Tailwind para styling:

```tsx
<div className="p-4 bg-white rounded-lg shadow-md hover:shadow-lg transition">
  Conteúdo
</div>
```

### Variáveis CSS

Utilize variáveis CSS definidas em `index.css`:

```css
.meu-elemento {
  background-color: hsl(var(--primary));
  color: hsl(var(--foreground));
}
```

### Componentes shadcn/ui

Reutilize componentes do shadcn/ui:

```tsx
import { Button } from "@/components/ui/button";
import { Card } from "@/components/ui/card";

export default function Exemplo() {
  return (
    <Card>
      <Button>Clique aqui</Button>
    </Card>
  );
}
```

## 🔄 Estado e Contexto

### Hooks Locais

Para estado local de componente:

```typescript
const [isOpen, setIsOpen] = useState(false);
```

### Context API

Para estado global, use o ThemeContext como referência:

```typescript
import { useTheme } from "@/contexts/ThemeContext";

export default function Componente() {
  const { theme, toggleTheme } = useTheme();
  return <button onClick={toggleTheme}>Alternar tema</button>;
}
```

## 🛣️ Roteamento

### Adicionar Nova Rota

1. Crie a página em `pages/`:

```typescript
// pages/NovaPage.tsx
import Layout from "@/components/Layout";

export default function NovaPage() {
  return <Layout>Conteúdo da página</Layout>;
}
```

2. Importe em `App.tsx`:

```typescript
import NovaPage from "./pages/NovaPage";
```

3. Adicione a rota:

```typescript
<Route path={"/nova"} component={NovaPage} />
```

### Navegação

Use o componente Link do wouter:

```typescript
import { Link } from "wouter";

<Link href="/perfil">
  <a>Ir para perfil</a>
</Link>
```

## 📦 Componentes Reutilizáveis

### Criar Novo Componente

```typescript
// components/MeuComponente.tsx
interface Props {
  title: string;
  variant?: "primary" | "secondary";
}

export default function MeuComponente({ title, variant = "primary" }: Props) {
  return (
    <div className={`p-4 rounded-lg ${variant === "primary" ? "bg-purple-600" : "bg-gray-200"}`}>
      {title}
    </div>
  );
}
```

### Usar em Múltiplas Páginas

```typescript
import MeuComponente from "@/components/MeuComponente";

export default function Home() {
  return <MeuComponente title="Bem-vindo" />;
}
```

## 🧪 Boas Práticas

### Performance

1. **Lazy Loading**
   ```typescript
   const ComponenteLazyLoaded = lazy(() => import("./Componente"));
   ```

2. **Memoization**
   ```typescript
   const MeuComponente = memo(function Componente(props) {
     return <div>{props.children}</div>;
   });
   ```

3. **useCallback**
   ```typescript
   const handleClick = useCallback(() => {
     console.log("Clicado");
   }, []);
   ```

### Acessibilidade

1. Use atributos ARIA:
   ```tsx
   <button aria-label="Fechar menu">×</button>
   ```

2. Mantenha foco visível:
   ```css
   button:focus {
     outline: 2px solid var(--primary);
   }
   ```

3. Suporte a teclado:
   ```typescript
   const handleKeyDown = (e: React.KeyboardEvent) => {
     if (e.key === "Escape") closeModal();
   };
   ```

### Responsividade

Sempre teste em mobile:

```tsx
<div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
  {/* Cards responsivos */}
</div>
```

## 🔍 Debugging

### Console

```typescript
console.log("Debug:", variavel);
console.error("Erro:", erro);
console.warn("Aviso:", aviso);
```

### React DevTools

Instale a extensão React DevTools no navegador para inspecionar componentes.

### TypeScript Checking

```bash
pnpm tsc --noEmit
```

## 📝 Convenções de Código

### Nomes de Arquivos

- Componentes: `PascalCase` (ex: `MeuComponente.tsx`)
- Páginas: `PascalCase` (ex: `Home.tsx`)
- Utilitários: `camelCase` (ex: `formatDate.ts`)

### Nomes de Variáveis

```typescript
// ✅ Bom
const isLoading = true;
const userData = { name: "João" };

// ❌ Ruim
const loading = true;
const data = { name: "João" };
```

### Imports

```typescript
// ✅ Bom - Organizado
import { useState } from "react";
import { Card } from "@/components/ui/card";
import Layout from "@/components/Layout";
import { formatDate } from "@/lib/formatDate";

// ❌ Ruim - Desorganizado
import Layout from "@/components/Layout";
import { useState } from "react";
import { formatDate } from "@/lib/formatDate";
import { Card } from "@/components/ui/card";
```

## 🚀 Build e Deploy

### Build Local

```bash
pnpm build
```

### Preview da Build

```bash
pnpm preview
```

### Otimizações

1. **Code Splitting**: Automático com Vite
2. **Tree Shaking**: Remove código não utilizado
3. **Minificação**: Automática na build
4. **Image Optimization**: Coloque imagens em `public/`

## 📚 Recursos Úteis

- [React Docs](https://react.dev)
- [TypeScript Handbook](https://www.typescriptlang.org/docs/)
- [Tailwind CSS Docs](https://tailwindcss.com/docs)
- [shadcn/ui Components](https://ui.shadcn.com)
- [Wouter Docs](https://github.com/molefrog/wouter)

## 🤝 Contribuindo

1. Crie uma branch para sua feature
2. Siga as convenções de código
3. Teste em múltiplos navegadores
4. Faça commit com mensagens claras
5. Abra um pull request

## 📋 Checklist de Desenvolvimento

Antes de fazer commit:

- [ ] Código segue as convenções
- [ ] TypeScript sem erros
- [ ] Componentes são reutilizáveis
- [ ] Responsividade testada
- [ ] Acessibilidade considerada
- [ ] Performance otimizada
- [ ] Sem console.log em produção

---

**Última atualização**: Outubro 2025

