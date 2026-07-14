WordMonitor/
│
├── Program.cs
├── appsettings.json
├── appsettings.Local.json
│
├── Models/
│   ├── DocumentoInfo.cs
│   ├── Notificacao.cs
│   └── StatusDocumento.cs
│
├── Notifications/
│   ├── EmialNotifier.cs
│   ├── INotifier.cs
│   └── LogNotifier.cs
│
├── Properties/
│   └── launchSettings.json
│
├── Services/
│   ├── DocumentMonitorService.cs
│   ├── DocumentScanner.cs
│   ├── ExpirationService.cs
│   ├── ExpirationService.cs
│   ├── NotificationBuilder.cs
│   ├── ParserService.cs
│   ├── ValidityChecker.cs
│   └── WordReader.cs
│
└── Utils/
    └── Utils\FileHelper.cs

-- Bibliotecas utilizadas 
dotnet add package DocumentFormat.OpenXml
dotnet add package MailKit
dotnet add package Microsoft.Extensions.Configuration.Json

-- ainda não utilizadas
dotnet new worker
dotnet add package Microsoft.Data.Sqlite
dotnet add package Dapper

