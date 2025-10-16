# pim-helpdesk-system
PIM - Sistema de Help Desk

Este repositório contém o Projeto Integrado Multidisciplinar (PIM) para um sistema de Help Desk, desenvolvido para a Universidade Paulista. O projeto abrange as especificações, protótipos e modelagem de um sistema de gestão de chamados com recursos de Inteligência Artificial para triagem automática e sugestão de soluções, com implementações planejadas para plataformas Web, Mobile e Desktop.

Estrutura do Projeto

Este projeto é organizado em módulos para facilitar o desenvolvimento e a colaboração. Abaixo está a estrutura de diretórios e uma breve descrição de cada um:

•
.github/: Contém as configurações para automações do GitHub, como os workflows de Integração Contínua (CI) e Entrega Contínua (CD) através do GitHub Actions. Leia mais sobre o Workflow CI.

•
backend/: Código-fonte do servidor (API) e lógica de negócios, desenvolvido em C# com ASP.NET Core. Veja o README do Backend.

•
banco de dados/: Scripts SQL para modelagem do banco de dados e migrações, utilizando MS SQL Server. Veja o README do Banco de Dados.

•
documentos/: Documentação completa do projeto, incluindo o trabalho final em PDF e outras especificações. Veja o README da Documentação.

•
frontend-desktop/: Aplicação desktop para sistemas operacionais, desenvolvida em C# com WPF/WinForms ou .NET MAUI. Veja o README do Frontend Desktop.

•
frontend-mobile/: Aplicação mobile para dispositivos iOS e Android, desenvolvida em C# com Xamarin ou .NET MAUI. Veja o README do Frontend Mobile.

•
frontend-web/: Aplicação web para acesso via navegador, desenvolvida com HTML, CSS e JavaScript. Veja o README do Frontend Web.

•
.gitignore: Arquivo que lista os padrões de arquivos e diretórios que o Git deve ignorar e não rastrear. Leia mais sobre .gitignore e .gitkeep.

•
LICENÇA: O arquivo de licença do projeto (MIT License).

•
LEIA-ME.md: Este arquivo, com informações gerais sobre o projeto.

Fluxo de Trabalho de Integração Contínua (CI)

Um workflow de Integração Contínua (CI) foi configurado usando GitHub Actions para garantir a qualidade e a integridade do código. Este workflow é executado automaticamente em cada push (envio de código) e pull request (solicitação de revisão de código) para as branches main ou master.

O que o Workflow Faz:

•
Verifica o código: Garante que o código enviado esteja de acordo com as regras de formatação e padrões.

•
Instala dependências: Baixa todas as bibliotecas e pacotes necessários para cada parte do projeto.

•
Compila o código: Para projetos C# (Backend, Mobile, Desktop) e o Frontend Web, verifica se o código pode ser construído sem erros.

•
Roda testes (opcional, mas recomendado): Se configurado, executa testes automatizados para garantir que as novas mudanças não introduzam bugs.

O objetivo é simples: Detectar problemas no código o mais cedo possível, antes que eles se tornem maiores e mais difíceis de corrigir. Se o workflow falhar, significa que há algo que precisa ser ajustado no código antes que ele possa ser integrado à branch principal.

Entendendo .gitignore e .gitkeep

Esses dois arquivos são essenciais para o controle de versão com Git, especialmente em projetos grandes com diversas tecnologias.

.gitignore

•
Função: O arquivo .gitignore informa ao Git quais arquivos e pastas não devem ser rastreados e, portanto, não devem ser incluídos no repositório. Isso é crucial para evitar que arquivos desnecessários (como dependências de pacotes, arquivos temporários de build, configurações locais, etc.) sejam versionados.

•
Necessidade:

•
Reduzir o tamanho do repositório: Evita que arquivos grandes e gerados automaticamente sejam armazenados no histórico do Git.

•
Evitar conflitos: Impede que arquivos de configuração específicos de cada desenvolvedor ou ambiente sejam enviados, causando conflitos.

•
Segurança: Garante que informações sensíveis (como chaves de API, senhas) não sejam acidentalmente commitadas.



•
No seu projeto: O .gitignore foi configurado com o template .NET, que já ignora padrões comuns de projetos C# (ex: bin/, obj/, node_modules/, etc.). Você não precisa adicionar nada a ele a menos que encontre um novo tipo de arquivo que o Git esteja rastreando indevidamente.

•
Como Usar: Não é necessário alterá-lo diretamente, a menos que haja uma necessidade específica. Ele já está funcionando para você.

.gitkeep

•
Função: O Git não rastreia pastas vazias. Se você criar uma pasta (como backend/src/) e ela estiver vazia, o Git a ignorará e ela não será incluída no repositório. O .gitkeep é um arquivo vazio (ou com um comentário) que é colocado dentro de uma pasta vazia para forçar o Git a rastrear essa pasta.

•
Necessidade: Garante que a estrutura de diretórios planejada seja mantida no repositório, mesmo que algumas pastas ainda não contenham arquivos de código. Isso é útil para padronizar a estrutura para a equipe.

•
No seu projeto: Você já tem arquivos .gitkeep nas subpastas que foram criadas vazias (ex: backend/src/, frontend-web/src/).

•
Como Usar: Uma vez que você adicione arquivos de código reais a uma pasta que contém um .gitkeep, você pode remover o arquivo .gitkeep dessa pasta, pois o Git passará a rastrear a pasta devido aos novos arquivos.




Como Começar a Trabalhar (Para a Equipe)

1. Clonar o Repositório

•
Abra o terminal (ou Git Bash no Windows).

•
Navegue até a pasta onde deseja guardar seus projetos:

•
Copie a URL do repositório: Na página do GitHub, clique no botão verde "<> Code" e copie a URL HTTPS.

•
Clone o repositório:

•
Entre na pasta do projeto:

2. Configuração Inicial do Git (Uma única vez)

•
No terminal, dentro da pasta do projeto, execute:

3. Ciclo de Trabalho Diário

A. Antes de Começar a Codificar (Sempre!)

Sempre puxe as últimas alterações do repositório para garantir que sua cópia local esteja atualizada com o trabalho dos outros:

Bash


git pull origin main # Ou 'git pull origin master' se sua branch principal for master


B. Fazendo Suas Alterações

•
Trabalhe nos arquivos da sua parte do projeto (ex: frontend-web/src/).

•
Salve suas alterações regularmente.

C. Salvando Suas Alterações (Commit)

Quando terminar uma parte significativa da sua tarefa:

1.
Verifique quais arquivos foram alterados:

2.
Adicione os arquivos alterados para o "staging area" (área de preparação):

3.
Faça um commit (salve a alteração com uma mensagem descritiva):

D. Enviando Suas Alterações para o GitHub (Push)

Depois de fazer um ou mais commits, envie suas alterações para o repositório no GitHub:

Bash


git push origin main # Ou 'git push origin master'


4. Monitorando o Workflow de CI

Após cada git push, o GitHub Actions executará o workflow de CI. Você pode verificar o status na aba "Actions" do repositório no GitHub. Se o workflow falhar, clique nele para ver os detalhes do erro e corrija o problema no seu código antes de tentar um novo push.




Este README.md principal, juntamente com os README.md específicos de cada subdiretório, servirá como um guia completo para sua equipe. Agora, vamos criar os README.md para cada diretório, o que será feito na próxima fase do nosso plano.


