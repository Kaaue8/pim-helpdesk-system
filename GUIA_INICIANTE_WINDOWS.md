# 🚀 Guia Completo: Como Criar o Site PIM 3 no Windows (Para Iniciantes)

Este guia foi criado especialmente para você! Vamos criar o site passo a passo, sem necessidade de experiência prévia em programação.

**Tempo estimado:** 30 minutos

---

## 📋 Pré-requisitos (O que você vai precisar)

Você vai instalar 3 coisas simples:

1. **Git** - Para clonar o projeto do GitHub
2. **Node.js** - Para rodar o projeto
3. **Visual Studio Code** - Para editar o código (opcional, mas recomendado)

---

## ✅ Passo 1: Instalar Git

Git é um programa que permite você baixar e gerenciar código do GitHub.

### Como instalar:

1. Acesse: https://git-scm.com/download/win
2. Clique em "Click here to download"
3. Execute o arquivo baixado (`.exe`)
4. Clique em "Next" várias vezes até terminar
5. Deixe as opções padrão selecionadas

### Verificar se instalou corretamente:

1. Abra o **Prompt de Comando** (Windows + R, digite `cmd`, Enter)
2. Digite: `git --version`
3. Você deve ver algo como: `git version 2.40.0`

✅ **Pronto! Git instalado!**

---

## ✅ Passo 2: Instalar Node.js

Node.js é um programa que permite rodar código JavaScript no seu computador.

### Como instalar:

1. Acesse: https://nodejs.org/
2. Clique no botão grande **"LTS"** (é a versão mais estável)
3. Execute o arquivo baixado (`.msi`)
4. Clique em "Next" várias vezes até terminar
5. **Deixe marcado:** "Add to PATH" (adicionar ao caminho)

### Verificar se instalou corretamente:

1. Abra o **Prompt de Comando** novamente
2. Digite: `node --version`
3. Você deve ver algo como: `v20.10.0`
4. Digite: `npm --version`
5. Você deve ver algo como: `10.2.0`

✅ **Pronto! Node.js instalado!**

---

## ✅ Passo 3: Instalar Visual Studio Code (Opcional)

VS Code é um editor de texto para editar código. É grátis e muito bom!

### Como instalar:

1. Acesse: https://code.visualstudio.com/
2. Clique em "Download for Windows"
3. Execute o arquivo baixado (`.exe`)
4. Clique em "Next" várias vezes até terminar

✅ **Pronto! VS Code instalado!**

---

## 🎯 Passo 4: Clonar o Projeto do GitHub

Agora vamos baixar o código do projeto.

### Como fazer:

1. Abra o **Prompt de Comando**
2. Navegue até a pasta onde quer salvar o projeto. Por exemplo:
   ```
   cd Desktop
   ```
   ou
   ```
   cd Documentos
   ```

3. Clone o repositório (copie e cole este comando):
   ```
   git clone https://github.com/seu-usuario/pim-3-layout.git
   ```

4. Entre na pasta do projeto:
   ```
   cd pim-3-layout
   ```

✅ **Pronto! Projeto clonado!**

---

## 🔧 Passo 5: Instalar Dependências

O projeto usa várias bibliotecas. Vamos instalá-las.

### Como fazer:

1. No **Prompt de Comando** (dentro da pasta `pim-3-layout`), digite:
   ```
   npm install
   ```

2. Aguarde até terminar (pode levar alguns minutos)

3. Você verá muitas linhas de texto. Quando terminar, verá algo como:
   ```
   added 500 packages in 2m
   ```

✅ **Pronto! Dependências instaladas!**

---

## 🚀 Passo 6: Rodar o Projeto Localmente

Agora vamos colocar o site rodando no seu computador!

### Como fazer:

1. No **Prompt de Comando** (dentro da pasta `pim-3-layout`), digite:
   ```
   npm run dev
   ```

2. Você verá algo como:
   ```
   ➜  Local:   http://localhost:3000/
   ```

3. Abra seu navegador (Chrome, Firefox, Edge, etc.)

4. Acesse: http://localhost:3000/

5. **Pronto! Seu site está rodando!** 🎉

### Para parar o servidor:

Pressione `Ctrl + C` no Prompt de Comando.

---

## 🎨 Passo 7: Customizar o Site

Agora vamos mudar cores, textos e imagens para deixar do seu jeito!

### Abrir no VS Code:

1. Abra o **Visual Studio Code**
2. Clique em "File" → "Open Folder"
3. Selecione a pasta `pim-3-layout`
4. Clique em "Select Folder"

### Estrutura de Arquivos:

```
pim-3-layout/
├── client/
│   ├── src/
│   │   ├── pages/          ← Páginas do site
│   │   │   ├── Home.tsx    ← Dashboard
│   │   │   ├── Profile.tsx ← Perfil
│   │   │   └── ...
│   │   ├── components/     ← Componentes reutilizáveis
│   │   ├── index.css       ← Estilos globais
│   │   └── App.tsx         ← Configuração de rotas
│   └── public/             ← Imagens e arquivos estáticos
└── README.md               ← Documentação
```

---

## 🎨 Customizações Comuns

### 1. Mudar Título do Site

**Arquivo:** `client/src/App.tsx`

Procure por:
```typescript
<ThemeProvider defaultTheme="light">
```

### 2. Mudar Cores Principais

**Arquivo:** `client/src/index.css`

Procure por:
```css
:root {
  --primary: 147 51 234; /* Roxo */
}
```

Mude os números para outras cores:
- Azul: `59 130 246`
- Verde: `34 197 94`
- Vermelho: `239 68 68`
- Rosa: `236 72 153`

### 3. Mudar Textos

Abra qualquer arquivo `.tsx` em `client/src/pages/` e mude o texto diretamente!

**Exemplo - Dashboard:**
```typescript
<h1 className="text-3xl font-bold text-gray-900">
  Bem-vindo ao PIM 3  ← MUDE AQUI
</h1>
```

### 4. Adicionar Imagens

1. Coloque a imagem em `client/public/`
2. Use no código:
```typescript
<img src="/nome-da-imagem.png" alt="Descrição" />
```

---

## 📝 Exemplo: Mudar Cor Roxo para Azul

### Passo a passo:

1. Abra `client/src/index.css`
2. Procure por `--primary: 147 51 234;`
3. Mude para `--primary: 59 130 246;`
4. Salve o arquivo (Ctrl + S)
5. Veja a mudança no navegador automaticamente!

---

## 🌐 Passo 8: Colocar Online (Deploy)

Quando estiver pronto, você pode colocar o site online gratuitamente!

### Opção 1: Vercel (Recomendado - Mais fácil)

1. Acesse: https://vercel.com/
2. Clique em "Sign Up"
3. Faça login com sua conta GitHub
4. Clique em "New Project"
5. Selecione o repositório `pim-3-layout`
6. Clique em "Deploy"
7. Pronto! Seu site está online!

### Opção 2: Netlify

1. Acesse: https://www.netlify.com/
2. Clique em "Sign up"
3. Faça login com sua conta GitHub
4. Clique em "New site from Git"
5. Selecione o repositório
6. Clique em "Deploy"

---

## 🐛 Troubleshooting (Se algo der errado)

### Erro: "npm: comando não encontrado"

**Solução:** Node.js não foi instalado corretamente.
- Desinstale e reinstale Node.js
- Reinicie o computador

### Erro: "git: comando não encontrado"

**Solução:** Git não foi instalado corretamente.
- Desinstale e reinstale Git
- Reinicie o computador

### Porta 3000 já está em uso

**Solução:** Outra aplicação está usando a porta 3000.
- Feche outras abas do navegador
- Ou use: `npm run dev -- --port 3001`

### Arquivo não salva as mudanças

**Solução:** Você precisa salvar o arquivo.
- Pressione `Ctrl + S` após fazer mudanças
- Ou ative "Auto Save" no VS Code

---

## 📚 Próximos Passos

Depois de dominar o básico, você pode:

1. **Aprender React** - Framework JavaScript para criar interfaces
2. **Aprender Tailwind CSS** - Framework CSS para estilizar
3. **Adicionar banco de dados** - Para salvar dados
4. **Adicionar autenticação** - Para login de usuários
5. **Publicar no GitHub** - Compartilhar seu código

---

## 🎓 Recursos Úteis

### Documentação Oficial

- **React:** https://react.dev
- **Tailwind CSS:** https://tailwindcss.com
- **Node.js:** https://nodejs.org/docs
- **Git:** https://git-scm.com/doc

### Tutoriais em Português

- **Curso React:** https://www.youtube.com/results?search_query=react+em+português
- **Tailwind CSS:** https://www.youtube.com/results?search_query=tailwind+css+português
- **Git e GitHub:** https://www.youtube.com/results?search_query=git+github+português

### Comunidades

- **Stack Overflow em Português:** https://pt.stackoverflow.com/
- **Dev.to:** https://dev.to/
- **GitHub Discussions:** Pergunte no repositório

---

## ✅ Checklist Final

Antes de considerar seu site "pronto", verifique:

- [ ] Site roda localmente sem erros
- [ ] Todas as páginas estão acessíveis
- [ ] Cores e textos foram customizados
- [ ] Imagens foram adicionadas
- [ ] Responsividade foi testada (abra em tamanho pequeno)
- [ ] Nenhum erro no console do navegador
- [ ] Código foi enviado para GitHub
- [ ] Site foi publicado online

---

## 🎉 Parabéns!

Você conseguiu criar seu próprio site! 🚀

Agora você pode:
- Compartilhar o link com amigos
- Continuar aprendendo e melhorando
- Adicionar novas funcionalidades
- Usar como portfólio

**Qualquer dúvida, volte neste guia ou procure na comunidade!**

---

**Última atualização:** Outubro 2025
**Versão:** 1.0
**Dificuldade:** ⭐ Iniciante

