# PIM 3 - Sistema de Gestão de Perfis e Chamados

Um sistema web moderno e responsivo para gerenciamento de perfis de usuários e chamados/tickets de suporte, desenvolvido com React 19, Tailwind CSS 4 e shadcn/ui.

## 🎯 Características Principais

- **Dashboard Intuitivo** - Visão geral de chamados e estatísticas
- **Gestão de Perfil** - Edição de informações pessoais e profissionais
- **Sistema de Chamados** - Criar, acompanhar e gerenciar tickets
- **FAQ Interativo** - Perguntas frequentes com categorias
- **Painel Administrativo** - Gerenciamento de usuários e configurações
- **Design Responsivo** - Funciona perfeitamente em desktop, tablet e mobile
- **Interface Moderna** - Paleta de cores roxo/púrpura com componentes elegantes

## 🚀 Tecnologias Utilizadas

- **React 19** - Framework JavaScript moderno
- **TypeScript** - Tipagem estática para maior segurança
- **Tailwind CSS 4** - Framework CSS utilitário
- **shadcn/ui** - Componentes UI de alta qualidade
- **Wouter** - Roteamento leve para React
- **Vite** - Build tool rápido e moderno
- **Lucide Icons** - Ícones SVG escaláveis

## 📁 Estrutura do Projeto

```
client/
├── src/
│   ├── pages/
│   │   ├── Home.tsx              # Dashboard principal
│   │   ├── Profile.tsx           # Página de perfil do usuário
│   │   ├── CreateTicket.tsx      # Formulário para abrir chamado
│   │   ├── Tickets.tsx           # Lista de chamados
│   │   ├── FAQ.tsx               # Perguntas frequentes
│   │   ├── Admin.tsx             # Painel administrativo
│   │   └── NotFound.tsx          # Página 404
│   ├── components/
│   │   ├── Layout.tsx            # Layout principal (Sidebar + Header)
│   │   └── ui/                   # Componentes shadcn/ui
│   ├── contexts/
│   │   └── ThemeContext.tsx      # Contexto de tema
│   ├── hooks/                    # Custom React hooks
│   ├── lib/                      # Utilitários e helpers
│   ├── App.tsx                   # Configuração de rotas
│   ├── main.tsx                  # Entry point
│   └── index.css                 # Estilos globais
├── public/                       # Arquivos estáticos
└── index.html                    # HTML principal
```

## 🎨 Paleta de Cores

- **Primária**: Roxo/Púrpura (`#7C3AED`, `#6D28D9`)
- **Secundária**: Branco/Off-white (`#FFFFFF`, `#F9FAFB`)
- **Acentos**: Vermelho, Verde, Azul, Amarelo
- **Neutros**: Cinzas para textos secundários

## 📱 Responsividade

O projeto é totalmente responsivo com breakpoints:
- **Mobile**: até 640px
- **Tablet**: 641px a 1024px
- **Desktop**: acima de 1024px

## 🔧 Como Usar

### Instalação

```bash
# Instalar dependências
pnpm install

# Iniciar servidor de desenvolvimento
pnpm dev

# Build para produção
pnpm build

# Preview da build
pnpm preview
```

### Navegação

A aplicação possui as seguintes rotas:

- `/` - Dashboard (Home)
- `/perfil` - Perfil do usuário
- `/chamado` - Abrir novo chamado
- `/chamados` - Lista de chamados
- `/faq` - Perguntas frequentes
- `/admin` - Painel administrativo

## 🎯 Funcionalidades Implementadas

### Dashboard
- Cards com estatísticas de chamados
- Tabela de chamados recentes
- Links rápidos para outras seções
- Cards informativos

### Perfil
- Exibição de informações pessoais
- Informações profissionais
- Histórico de atividades
- Botão para editar perfil

### Abrir Chamado
- Formulário completo com validação
- Seleção de categoria
- Definição de prioridade
- Upload de arquivo
- Termos de serviço

### Meus Chamados
- Listagem com busca
- Filtros por status
- Indicadores visuais de prioridade
- Estatísticas resumidas

### FAQ
- Perguntas organizadas por categoria
- Busca e filtros
- Acordeão expansível
- Seção de contato

### Admin
- Visão geral com estatísticas
- Gerenciamento de usuários
- Gerenciamento de categorias
- Configurações do sistema

## 🎨 Customização

### Alterar Cores

Edite as variáveis CSS em `client/src/index.css`:

```css
@layer base {
  :root {
    --primary: 147 51 234; /* Roxo */
    --secondary: 59 130 246; /* Azul */
    /* ... mais cores */
  }
}
```

### Alterar Tipografia

Modifique as fontes em `client/index.html` e `client/src/index.css`.

## 📊 Performance

- Lazy loading de componentes
- Otimização de imagens
- Code splitting automático
- Caching inteligente

## 🔒 Segurança

- Validação de formulários no cliente
- Proteção contra XSS
- CORS configurado
- Headers de segurança

## 📝 Licença

Este projeto é fornecido como está para uso pessoal e comercial.

## 👨‍💻 Desenvolvimento

### Adicionar Nova Página

1. Crie um arquivo em `client/src/pages/NovaPagina.tsx`
2. Importe em `client/src/App.tsx`
3. Adicione a rota no componente `Router`

### Adicionar Novo Componente

1. Crie um arquivo em `client/src/components/MeuComponente.tsx`
2. Importe onde necessário

## 🐛 Troubleshooting

### O servidor não inicia

```bash
# Limpe o cache
rm -rf node_modules/.vite

# Reinstale dependências
pnpm install

# Inicie novamente
pnpm dev
```

### Erros de TypeScript

```bash
# Verifique tipos
pnpm tsc --noEmit
```

## 📞 Suporte

Para dúvidas ou problemas, consulte a seção FAQ da aplicação ou abra um chamado no painel.

---

**Desenvolvido com ❤️ usando React, Tailwind CSS e shadcn/ui**

