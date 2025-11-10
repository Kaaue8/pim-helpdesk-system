# 🐙 Como Publicar seu Projeto no GitHub (Para Iniciantes)

Neste guia você vai aprender a colocar seu projeto no GitHub e depois publicar online!

---

## 📋 O que é GitHub?

GitHub é um serviço online onde você pode:
- **Guardar seu código** na nuvem
- **Compartilhar com outras pessoas**
- **Controlar versões** (histórico de mudanças)
- **Publicar seu site** gratuitamente

---

## ✅ Passo 1: Criar Conta no GitHub

1. Acesse: https://github.com/
2. Clique em "Sign up"
3. Preencha com:
   - Email
   - Senha
   - Nome de usuário (ex: `seu-nome-123`)
4. Clique em "Create account"
5. Confirme seu email

✅ **Pronto! Conta criada!**

---

## ✅ Passo 2: Criar um Novo Repositório

Um repositório é como uma pasta no GitHub onde seu código fica guardado.

### Como criar:

1. Faça login no GitHub
2. Clique no ícone **+** no canto superior direito
3. Selecione **"New repository"**
4. Preencha:
   - **Repository name:** `pim-3-layout` (ou outro nome)
   - **Description:** "Sistema de gestão de perfis e chamados" (opcional)
   - **Public:** Deixe marcado (para ser público)
   - **Initialize this repository with:** Deixe em branco

5. Clique em **"Create repository"**

✅ **Pronto! Repositório criado!**

---

## ✅ Passo 3: Enviar seu Código para GitHub

Agora vamos enviar o código do seu computador para o GitHub.

### No Prompt de Comando:

1. Abra o **Prompt de Comando**
2. Navegue até a pasta do projeto:
   ```
   cd Desktop\pim-3-layout
   ```

3. Configure seu nome e email (primeira vez apenas):
   ```
   git config --global user.name "Seu Nome"
   git config --global user.email "seu-email@gmail.com"
   ```

4. Inicialize o repositório:
   ```
   git init
   ```

5. Adicione todos os arquivos:
   ```
   git add .
   ```

6. Crie um "commit" (salve as mudanças):
   ```
   git commit -m "Primeiro commit - Projeto PIM 3"
   ```

7. Adicione o repositório remoto (mude `seu-usuario` para seu usuário do GitHub):
   ```
   git remote add origin https://github.com/seu-usuario/pim-3-layout.git
   ```

8. Envie para o GitHub:
   ```
   git branch -M main
   git push -u origin main
   ```

9. Digite seu **nome de usuário** e **token de acesso pessoal**

✅ **Pronto! Código enviado para GitHub!**

---

## 🔐 Criar Token de Acesso (Se Pedir)

Se o GitHub pedir um "token" em vez de senha:

1. Acesse: https://github.com/settings/tokens
2. Clique em **"Generate new token"**
3. Dê um nome: "Git Push"
4. Selecione **"repo"** (para acesso ao repositório)
5. Clique em **"Generate token"**
6. **Copie o token** (você vai usar uma única vez)
7. Cole no Prompt de Comando quando pedir

---

## ✅ Passo 4: Verificar no GitHub

1. Acesse seu repositório no GitHub
2. Você deve ver todos os seus arquivos lá!
3. Veja o histórico de commits clicando em "Commits"

✅ **Pronto! Código no GitHub!**

---

## 🚀 Passo 5: Publicar Online com Vercel

Agora vamos colocar seu site online gratuitamente!

### Como fazer:

1. Acesse: https://vercel.com/
2. Clique em **"Sign Up"**
3. Clique em **"Continue with GitHub"**
4. Autorize o Vercel a acessar sua conta GitHub
5. Clique em **"New Project"**
6. Selecione o repositório **`pim-3-layout`**
7. Clique em **"Import"**
8. Deixe as configurações padrão
9. Clique em **"Deploy"**
10. Aguarde alguns minutos...

✅ **Pronto! Seu site está online!**

### Acessar seu site:

Quando terminar, você verá um link como:
```
https://pim-3-layout-abc123.vercel.app/
```

Compartilhe este link com qualquer pessoa!

---

## 🔄 Atualizar seu Site (Depois de Mudanças)

Depois que você faz mudanças no seu computador, siga estes passos para atualizar o site online:

### No Prompt de Comando:

1. Navegue até a pasta do projeto:
   ```
   cd Desktop\pim-3-layout
   ```

2. Adicione as mudanças:
   ```
   git add .
   ```

3. Crie um commit:
   ```
   git commit -m "Descrição das mudanças"
   ```

4. Envie para GitHub:
   ```
   git push
   ```

5. **Vercel atualizará automaticamente!** (espere 1-2 minutos)

---

## 📝 Exemplo de Fluxo Completo

### Dia 1: Criar e publicar
```
1. Criar repositório no GitHub
2. Enviar código com git push
3. Publicar com Vercel
4. Compartilhar link
```

### Dia 2: Fazer mudanças
```
1. Editar arquivo no VS Code
2. Salvar (Ctrl + S)
3. Testar localmente (npm run dev)
4. git add .
5. git commit -m "Mudei a cor para azul"
6. git push
7. Vercel atualiza automaticamente
```

---

## 🎯 Comandos Git Mais Usados

| Comando | O que faz |
|---------|-----------|
| `git status` | Mostra arquivos modificados |
| `git add .` | Adiciona todos os arquivos |
| `git commit -m "mensagem"` | Salva as mudanças |
| `git push` | Envia para GitHub |
| `git pull` | Baixa mudanças do GitHub |
| `git log` | Mostra histórico de commits |
| `git diff` | Mostra diferenças nos arquivos |

---

## 🆘 Problemas Comuns

### Erro: "fatal: not a git repository"

**Solução:** Você não está na pasta correta.
```
cd Desktop\pim-3-layout
```

### Erro: "fatal: 'origin' does not appear to be a 'git' repository"

**Solução:** Você não configurou o repositório remoto.
```
git remote add origin https://github.com/seu-usuario/pim-3-layout.git
```

### Erro: "Authentication failed"

**Solução:** Seu token expirou ou está errado.
- Crie um novo token em: https://github.com/settings/tokens
- Use o novo token ao fazer push

### Vercel não atualiza

**Solução:** Aguarde 2-3 minutos. Se não funcionar:
1. Acesse seu projeto no Vercel
2. Clique em "Deployments"
3. Clique em "Redeploy" no último deploy

---

## 📚 Próximos Passos

Depois de dominar GitHub e Vercel, você pode:

1. **Aprender Git avançado** - Branches, merge, etc.
2. **Usar GitHub Actions** - Automatizar testes
3. **Colaborar com outros** - Pull requests
4. **Usar GitHub Pages** - Publicar diretamente do GitHub
5. **Adicionar CI/CD** - Deploy automático

---

## 🔗 Links Úteis

- **GitHub Docs:** https://docs.github.com/
- **Git Tutorial:** https://git-scm.com/book/pt-BR/v2
- **Vercel Docs:** https://vercel.com/docs
- **GitHub Learning Lab:** https://lab.github.com/

---

## ✅ Checklist

- [ ] Conta GitHub criada
- [ ] Repositório criado
- [ ] Código enviado com git push
- [ ] Vercel configurado
- [ ] Site publicado online
- [ ] Link compartilhado

---

**Parabéns! Seu site está online!** 🎉

Agora você pode:
- Compartilhar o link com amigos
- Continuar atualizando o site
- Aprender mais sobre desenvolvimento
- Usar como portfólio

**Boa sorte!** 🚀

