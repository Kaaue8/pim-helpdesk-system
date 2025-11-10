# 🎨 Guia de Customização Rápida - PIM 3

Neste guia você vai aprender a fazer as customizações mais comuns de forma rápida e fácil!

---

## 🎯 Customizações Mais Comuns

### 1️⃣ Mudar o Nome do Site

**Onde:** `client/src/components/Layout.tsx`

**Encontre:**
```typescript
{sidebarOpen && <h1 className="text-xl font-bold">PIM 3</h1>}
```

**Mude para:**
```typescript
{sidebarOpen && <h1 className="text-xl font-bold">Seu Nome Aqui</h1>}
```

**Salve:** Ctrl + S

---

### 2️⃣ Mudar Cor Roxo para Outra Cor

**Onde:** `client/src/index.css`

**Encontre a seção `:root {` e procure por:**
```css
--primary: 147 51 234; /* Roxo */
```

**Mude para uma destas opções:**

| Cor | Código |
|-----|--------|
| 🔵 Azul | `59 130 246` |
| 🟢 Verde | `34 197 94` |
| 🔴 Vermelho | `239 68 68` |
| 🟡 Amarelo | `234 179 8` |
| 🩷 Rosa | `236 72 153` |
| 🟣 Roxo Claro | `168 85 247` |
| 🟦 Ciano | `34 211 238` |

**Exemplo - Mudar para Azul:**
```css
--primary: 59 130 246; /* Azul */
```

**Salve:** Ctrl + S

---

### 3️⃣ Mudar Textos da Home (Dashboard)

**Onde:** `client/src/pages/Home.tsx`

**Encontre:**
```typescript
<h1 className="text-3xl font-bold text-gray-900">
  Bem-vindo ao PIM 3
</h1>
<p className="text-gray-600 mt-2">
  Gerencie seus chamados e perfil em um único lugar
</p>
```

**Mude para seus textos:**
```typescript
<h1 className="text-3xl font-bold text-gray-900">
  Bem-vindo ao Meu Site
</h1>
<p className="text-gray-600 mt-2">
  Descrição do meu site aqui
</p>
```

**Salve:** Ctrl + S

---

### 4️⃣ Mudar Textos do Menu (Sidebar)

**Onde:** `client/src/components/Layout.tsx`

**Encontre:**
```typescript
const navItems = [
  { label: "Dashboard", path: "/" },
  { label: "Meu Perfil", path: "/perfil" },
  { label: "Abrir Chamado", path: "/chamado" },
  { label: "Meus Chamados", path: "/chamados" },
  { label: "FAQ", path: "/faq" },
  { label: "Admin", path: "/admin" },
];
```

**Mude os `label` para seus itens de menu:**
```typescript
const navItems = [
  { label: "Home", path: "/" },
  { label: "Sobre", path: "/perfil" },
  { label: "Serviços", path: "/chamado" },
  { label: "Contato", path: "/chamados" },
  { label: "Blog", path: "/faq" },
  { label: "Admin", path: "/admin" },
];
```

**Salve:** Ctrl + S

---

### 5️⃣ Adicionar Imagem/Logo

**Passo 1:** Coloque a imagem em `client/public/`

**Passo 2:** Abra `client/index.html`

**Encontre:**
```html
<link rel="icon" type="image/svg+xml" href="/vite.svg" />
```

**Mude para:**
```html
<link rel="icon" type="image/png" href="/seu-logo.png" />
```

**Passo 3:** Se quiser mostrar a logo no site, abra `client/src/components/Layout.tsx`

**Encontre:**
```typescript
<h1 className="text-xl font-bold">PIM 3</h1>
```

**Mude para:**
```typescript
<img src="/seu-logo.png" alt="Logo" className="h-8 w-auto" />
```

---

### 6️⃣ Mudar Cores dos Botões

**Onde:** Qualquer arquivo `.tsx` que tenha botões

**Encontre:**
```typescript
<button className="px-6 py-2 bg-purple-600 text-white rounded-lg">
  Clique aqui
</button>
```

**Mude a cor:**
```typescript
<button className="px-6 py-2 bg-blue-600 text-white rounded-lg">
  Clique aqui
</button>
```

**Cores disponíveis:**
- `bg-blue-600` - Azul
- `bg-green-600` - Verde
- `bg-red-600` - Vermelho
- `bg-yellow-600` - Amarelo
- `bg-pink-600` - Rosa
- `bg-purple-600` - Roxo

---

### 7️⃣ Mudar Fonte/Tipografia

**Onde:** `client/index.html`

**Encontre:**
```html
<link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap" rel="stylesheet">
```

**Mude para outra fonte do Google Fonts:**
```html
<link href="https://fonts.googleapis.com/css2?family=Roboto:wght@400;500;700&display=swap" rel="stylesheet">
```

**Depois, em `client/src/index.css`, mude:**
```css
@layer base {
  body {
    @apply font-sans;
    font-family: "Inter", sans-serif;
  }
}
```

**Para:**
```css
@layer base {
  body {
    @apply font-sans;
    font-family: "Roboto", sans-serif;
  }
}
```

---

## 📝 Exemplo Completo: Customizar Home

Vamos fazer uma customização completa da página Home!

### Arquivo: `client/src/pages/Home.tsx`

**Antes:**
```typescript
export default function Home() {
  return (
    <Layout>
      <div className="space-y-8">
        <div>
          <h1 className="text-3xl font-bold text-gray-900">
            Bem-vindo ao PIM 3
          </h1>
          <p className="text-gray-600 mt-2">
            Gerencie seus chamados e perfil em um único lugar
          </p>
        </div>
        {/* ... resto do código ... */}
      </div>
    </Layout>
  );
}
```

**Depois:**
```typescript
export default function Home() {
  return (
    <Layout>
      <div className="space-y-8">
        <div>
          <h1 className="text-3xl font-bold text-gray-900">
            Bem-vindo à Minha Loja
          </h1>
          <p className="text-gray-600 mt-2">
            Encontre os melhores produtos com os melhores preços
          </p>
        </div>
        {/* ... resto do código ... */}
      </div>
    </Layout>
  );
}
```

---

## 🎨 Tabela de Cores Tailwind

| Classe | Cor |
|--------|-----|
| `bg-slate-600` | Cinza |
| `bg-gray-600` | Cinza Escuro |
| `bg-zinc-600` | Cinza Neutro |
| `bg-neutral-600` | Neutro |
| `bg-stone-600` | Bege |
| `bg-red-600` | Vermelho |
| `bg-orange-600` | Laranja |
| `bg-amber-600` | Âmbar |
| `bg-yellow-600` | Amarelo |
| `bg-lime-600` | Lima |
| `bg-green-600` | Verde |
| `bg-emerald-600` | Esmeralda |
| `bg-teal-600` | Azul-Verde |
| `bg-cyan-600` | Ciano |
| `bg-sky-600` | Azul Claro |
| `bg-blue-600` | Azul |
| `bg-indigo-600` | Índigo |
| `bg-violet-600` | Violeta |
| `bg-purple-600` | Roxo |
| `bg-fuchsia-600` | Fúcsia |
| `bg-pink-600` | Rosa |
| `bg-rose-600` | Rosa Escuro |

---

## 🔄 Fluxo de Customização

1. **Abra o arquivo** em VS Code
2. **Encontre o texto/cor** que quer mudar
3. **Mude para o novo valor**
4. **Salve** (Ctrl + S)
5. **Veja a mudança** no navegador (atualiza automaticamente)
6. **Se não gostar**, desfaça (Ctrl + Z) e tente novamente

---

## ⚠️ Dicas Importantes

### ✅ Faça:
- Salve sempre após fazer mudanças
- Teste no navegador para ver o resultado
- Faça uma mudança de cada vez
- Use nomes descritivos para suas customizações

### ❌ Não faça:
- Não delete chaves `{}` ou parênteses `()`
- Não mude nomes de variáveis sem saber o que está fazendo
- Não mude a estrutura das pastas
- Não delete linhas de código importantes

---

## 🆘 Se Algo Quebrar

Se você acidentalmente quebrou algo:

1. **Abra o arquivo** que editou
2. **Pressione Ctrl + Z** várias vezes para desfazer
3. **Ou copie o código original** do GitHub

---

## 📚 Próximas Customizações

Depois de dominar o básico, você pode:

1. **Adicionar novas páginas** - Copie uma página existente
2. **Mudar layout** - Edite a estrutura HTML
3. **Adicionar animações** - Use Tailwind CSS animations
4. **Integrar com banco de dados** - Salve dados reais
5. **Adicionar login** - Implemente autenticação

---

**Qualquer dúvida, consulte o README.md ou DEVELOPMENT.md!**

Boa sorte! 🚀

