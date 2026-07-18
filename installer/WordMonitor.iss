#define MyAppName "WordMonitor"
#define MyAppVersion "1.0.0"
#define MyAppPublisher "Felipe Xavier"
#define MyAppExeName "WordMonitor.Worker.exe"

[Setup]
AppId={{5C6B24A4-0B89-4F57-A0D6-9F9F3E7E5B17}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}

DefaultDirName={autopf}\WordMonitor
DefaultGroupName=WordMonitor

OutputDir=Output
OutputBaseFilename=WordMonitorSetup

Compression=lzma
SolidCompression=yes

ArchitecturesInstallIn64BitMode=x64
WizardStyle=modern

PrivilegesRequired=admin

UninstallDisplayIcon={app}\WordMonitor.Worker.exe

[Files]
Source: "..\src\WordMonitor.Worker\bin\Release\net8.0\win-x64\publish\*"; DestDir: "{app}"; Flags: recursesubdirs ignoreversion createallsubdirs

[Icons]
Name: "{group}\WordMonitor"; Filename: "{app}\WordMonitor.Worker.exe"

[Run]

Filename: "{sys}\sc.exe"; \
Parameters: "create WordMonitor binPath= ""{app}\WordMonitor.Worker.exe"" start= auto"; \
Flags: runhidden waituntilterminated

Filename: "{sys}\sc.exe"; \
Parameters: "start WordMonitor"; \
Flags: runhidden waituntilterminated

[UninstallRun]

Filename: "{sys}\sc.exe"; \
Parameters: "stop WordMonitor"; \
Flags: runhidden waituntilterminated

Filename: "{sys}\sc.exe"; \
Parameters: "delete WordMonitor"; \
Flags: runhidden waituntilterminated
