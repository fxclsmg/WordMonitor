--mostra árvore a partir raiz do projeto
tree .\ /F

--mostra arvore do sub projeto
tree .\src\WordMonitor.Configurator /F

--mostra arvore do sub projeto
tree .\src\WordMonitor.Core /F

--mostra arvore do sub projeto
tree .\src\WordMonitor.Worker /F

-- Bibliotecas utilizadas 
dotnet add package DocumentFormat.OpenXml
dotnet add package MailKit
dotnet add package Microsoft.Extensions.Configuration.Json

-- cria solução
dotnet new sln -n WordMonitor

-- em src/
dotnet new worker -n WordMonitor.Worker
dotnet new classlib -n WordMonitor.Core
dotnet new winforms -n WordMonitor.Configurator

-- na raiz
dotnet sln add .\src\WordMonitor.Core\WordMonitor.Core.csproj
dotnet sln add .\src\WordMonitor.Worker\WordMonitor.Worker.csproj
dotnet sln add .\src\WordMonitor.Configurator\WordMonitor.Configurator.csproj

-- lista sln
dotnet sln list

-- limpa temp
dotnet clean

-- build do core
dotnet build .\src\WordMonitor.Core\WordMonitor.Core.csproj

-- build do worker
dotnet build .\src\WordMonitor.Worker\WordMonitor.Worker.csproj

-- rodar worker
dotnet run --project .\src\WordMonitor.Worker

-- refenrecia projeto core no worker
dotnet add .\src\WordMonitor.Worker\WordMonitor.Worker.csproj reference .\src\WordMonitor.Core\WordMonitor.Core.csproj

-- lista referencias
dotnet list .\src\WordMonitor.Worker\WordMonitor.Worker.csproj reference

-- próximos passos:
-- conseguir gerar instalador
-- fazer arquivo de configuração externo à aplcação
-- fazer programa de configuraões 

-- ainda não utilizadas
dotnet add package Microsoft.Data.Sqlite
dotnet add package Dapper

