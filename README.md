### - Nome do projeto: DataVault

### - Obs: 
a pasta DataVault que contém a aplicação do Visual Studio(roxo)(mvc), na parte do formulário de feedback talvez precise da minha autorizção para enviar o formulário então na hora talvez de uma mensagem tipo check your email, para mim autorizar o ativamento, ai quando o senhor for corrigir pode me avisar?;
Precisa do Xampp e do nosso banco de dados para funcionar.

### - Descrição 
O projeto datavault é um gerenciador de arquivos com uma IA de fundo para criação de pastas de acordo com o conteúdo do arquivo, por exemplo se temos os arquivos Estados Unidos na segunda Guerra Mundial  e Bombas Atomicas, ela cria uma pasta chamada  Segunda Guerra Mundial. Sua finalidade é orgarnizar os arquivos das pessoas, organizando-os com pastas sobre seus assuntos, além de ter uma IA auxiliando. Principais funcionalidades são o carregamento e exibição de arquivos dentro das pastas criadas além da IA podendo fazer indicações e ajudas.

### 1. Pré-requisitos
- C#/Asp Net;
- Visual Studio(roxo) só para rodar;
- Tenha um xampp ou wamp para rodar o apache e o mysql;
- Baixe o nosso banco de dados SQL(BDtrabai.sql);
  <!-- Perdão pelo nome do arquivo --!>
- MySql Workbench
- Baixar a pasta DataVault_Certo
- ## Funcionalidade com IA Local
Este projeto inclui uma IA que organiza arquivos automaticamente com base no conteúdo.
### Para testar a IA:
1. Instale o Ollama: https://ollama.com/
2. Execute no terminal: `ollama pull phi3`
3. Rode a aplicação — a IA funcionará automaticamente ao fazer upload.

> Nota: A IA roda 100% localmente, sem envio de dados à nuvem.

### 2. Instalação
# Clone o repositório
git clone https://github.com/usuario/repositorio.git
# Acesse a pasta do projeto
cd DataVault

### 3. Como Executar
Para executar da melhor forma depois de ter baixado o projeto;
Abra-o no Visual Studio(roxo);
Com o Visual Studio(roxo) aberto clique em ferramentas, depois em gerenciar pacotes Nuget,
depois em gerenciar pacotes do Nuget para Solução, verifique se o pacote Microsoft.EntityFrameworkCore.Tools está instalado
e também o pacote Pomelo.EntityFrameworkCore.MySql está instalado, se não estiver vá em procurar
e coloque na barra de pesquisas esse nomes em específico e instale eles.
Abra no MySqlWorkbench o banco de dados baixado;
Abra o xampp e coloque o MySQl e o Apache para Iniciar.
Rode o programa no Visual Studio(roxo) clicando na setinha verde, com o http selecionado.

### 4. Acesso
- URL local: http://localhost:3306  
- Usuário padrão: root  
- Senha padrão: "" (vazia mesmo)


### - Integrantes do Grupo:
● Cauã Zanete Souza – 12303305
● Gabriel Cardoso Alvares de Oliveira – 12300411
● Gabriel Henrique de Lima Gonçalves– 12300713
● Pietro Barros Martins da Rocha – 12303313
● Rafael Augusto de Lima Gonçalves – 12300705
● Samuel Busatto Ribas – 12300918

 - Turma: 3B1

## Estrutura de Diretórios

    projeto/
    ├── DataVault/               # Código-fonte principal
    ├── Diagrama_Classes/        # Todos os diagramas de classes
    ├── Lista_Requisitos/        # Lista de requisitos
    ├── PDFS/                    # Toda a documentação em PDF
    ├── PITCH_BD/                # Tudo relacionado ao banco de dados do Pitch
    ├── Site/                    # Todo o FrontEnd utilizado no site
    ├── tentativa escola -certissiomo/test1 -Copia/   # Testes do front end e do github
    ├── README.md                # Arquivo de descrição do projeto
 
## Design Patterns Aplicados na Camada de Domínio
### 🔹 Singleton
-   **Uso**: Conexão única ao banco de dados.\
-   **Justificativa**: Evita múltiplas instâncias e consumo
    desnecessário de recursos.
### 🔹 Composite
-   **Uso**: Criação de pastas a partir de arquivos que não se encaixam em nenhuma pasta construida .\
-   **Justificativa**: Evita repetição de código além de cria uma melhor segurança e deixa mais eficaz a criação de pastas..
### 🔹 Decorator
-   **Uso**: Filtrar os arquivos das pessoas.\
-   **Justificativa**: Deixa a separação por tipo de arquivo mais fácil além de evitar repetição de código tendo um código geral que le o que foi selecionado e organiza assim.
### 🔹 Strategy
-   **Uso**: Leitura de conteúdo de arquivos.\
-   **Justificativa**: Ajuda na leitura de arquivos da IA e também separa a leitura de cada tipo de arquivo.
  
### - Vinte funcionalidades:
- 1. O sistema deve permitir o cadastro de usuários.[✔]
2. O usuário deve poder redefinir a senha.[✔]
3. O usuário deve poder personalizar as configurações da interface.[✔]
4. O sistema deve ter um sistema de busca eficiente de arquivos por meio de seus nomes e eles serão ordenados pelo tipo de arquivos que a pessoa escolher como preferencial.[✔]
5. O sistema deve ter um login bem sucedido.[✔]
6. Pesquisas por palavra chave de arquivos.[✔]
7. Ter avaliações mensais para feedbacks.[✔]
8. Ter menus intuitivos para o usuário poder manusear melhor o site[✔].
9. O sistema deve oferecer suporte a múltiplos idiomas.[✔]
10. Ter uma responsividade fluida em aparelhos diversos[✔]
11. A IA consegue criar pastas[✔]
12. Organização de arquivos dentro das pastas certas automátizado com IA[✔]
13. Análise de Conteúdo dos arquivos enviados[✔]
14. Os arquivos favoritados estão corretamente exibidos[✔]
15. Os arquivos compartilhados estão corretamente exibidos[✔]
16. Os arquivos movidos para a lixeira estão exibidos[✔]
17. Sugestão de Tags ou Palavras-Chave[✔]
18. Detecção de Conteúdo Sensível ou Confidencial[✔]
19. Quando clicar em pastas exibir todas as pastas do usuario[✔]
20. Resumo de Conteúdo dos arquivos enviados[✔]
