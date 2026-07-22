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

-- projeto do serviço do windows
dotnet new worker -n src/WordMonitor.Worker
dotnet sln add .\src\WordMonitor.Worker\WordMonitor.Worker.csproj

-- projeto com a regra de negócio
dotnet new classlib -n src/WordMonitor.Core
dotnet sln add .\src\WordMonitor.Core\WordMonitor.Core.csproj

-- projeto para configurar 
dotnet new winforms -n src/WordMonitor.Configurator
dotnet sln add .\src\WordMonitor.Configurator\WordMonitor.Configurator.csproj

-- projeto para ícone da bandeija
dotnet new winforms -n src/WordMonitor.Tray
dotnet sln add src\WordMonitor.Tray\WordMonitor.Tray.csproj

-- projeto para rodar comandos com privilégos
dotnet new console -n src/WordMonitor.ServiceTool
dotnet sln add src/WordMonitor.ServiceTool/WordMonitor.ServiceTool.csproj

-- refenrecia projeto core no worker
dotnet add .\src\WordMonitor.Worker\WordMonitor.Worker.csproj reference .\src\WordMonitor.Core\WordMonitor.Core.csproj

-- pacote de servico do windows
dotnet add .\src\WordMonitor.Worker package Microsoft.Extensions.Hosting.WindowsServices --version 8.0.1

-- pacote de controle de processos
dotnet add package System.ServiceProcess.ServiceController

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

-- lista referencias do projeto worker
dotnet list .\src\WordMonitor.Worker\WordMonitor.Worker.csproj reference

-- comando para publicar projeto auto contido da configuração:
dotnet publish -c Release

-- inicia o serviço
sc start WordMonitor

-- para o serviço
sc stop WordMonitor

-- deleta o serviço
sc delete WordMonitor

-- caso trave o serviço remover o registro em
HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WordMonitor


-- instalador gerado com inosetup (automatixar?)


-- ainda não utilizadas
dotnet add package Microsoft.Data.Sqlite
dotnet add package Dapper

