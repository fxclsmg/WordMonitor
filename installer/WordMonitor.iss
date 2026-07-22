#define MyAppName "WordMonitor"
#define MyAppVersion "1.0.0"
#define MyAppPublisher "Felipe Xavier"


[Setup]

AppId={{5C6B24A4-0B89-4F57-A0D6-9F9F3E7E5B17}}

AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}

DefaultDirName={autopf}\WordMonitor
DefaultGroupName=WordMonitor

LanguageDetectionMethod=none
ShowLanguageDialog=auto

OutputDir=Output
OutputBaseFilename=WordMonitorSetup

Compression=lzma
SolidCompression=yes

ArchitecturesInstallIn64BitMode=x64

WizardStyle=modern

PrivilegesRequired=admin

SetupIconFile=WordMonitor.ico

UninstallDisplayIcon={app}\WordMonitor.ico


[Languages]

Name: "brazilianportuguese"; MessagesFile: "compiler:Languages\BrazilianPortuguese.isl"



[Files]


; ==========================
; Serviço Windows
; ==========================

Source: "..\src\WordMonitor.Worker\bin\Release\net8.0\win-x64\publish\*"; \
DestDir: "{app}"; \
Flags: recursesubdirs ignoreversion createallsubdirs



; ==========================
; Configurator
; ==========================

Source: "..\src\WordMonitor.Configurator\bin\Release\net8.0-windows\win-x64\publish\*"; \
DestDir: "{app}"; \
Flags: recursesubdirs ignoreversion createallsubdirs



; ==========================
; Tray
; ==========================

Source: "..\src\WordMonitor.Tray\bin\Release\net8.0-windows\win-x64\publish\*"; \
DestDir: "{app}"; \
Flags: recursesubdirs ignoreversion createallsubdirs



; ==========================
; Service tool
; ==========================

Source: "..\src\WordMonitor.ServiceTool\bin\Release\net8.0-windows\win-x64\publish\*"; \
DestDir: "{app}"; \
Flags: recursesubdirs ignoreversion createallsubdirs


; ==========================
; Template configuração
; ==========================

Source: "..\src\WordMonitor.Worker\appsettings.Local.template.json"; \
DestDir: "{app}"; \
Flags: ignoreversion



; ==========================
; Ícone
; ==========================

Source: "WordMonitor.ico"; \
DestDir: "{app}"



[Icons]


; Atalho configurador

Name: "{group}\Configurar WordMonitor"; \
Filename: "{app}\WordMonitor.Configurator.exe"; \
IconFilename: "{app}\WordMonitor.ico"

; Atalho Tray

Name: "{group}\Status WordMonitor"; \
Filename: "{app}\WordMonitor.Tray.exe"; \
IconFilename: "{app}\WordMonitor.ico"


; Desinstalar

Name: "{group}\Desinstalar WordMonitor"; \
Filename: "{uninstallexe}"


[Registry]

Root: HKCU; Subkey: "Software\Microsoft\Windows\CurrentVersion\Run"; \
ValueType: string; ValueName: "WordMonitor"; \
ValueData: "{app}\WordMonitor.Tray.exe"; \
Flags: uninsdeletevalue

[Code]


procedure CriarConfiguracaoInicial;

begin

  if not FileExists(
    ExpandConstant('{app}\appsettings.Local.json')
  ) then

  begin

    FileCopy(
      ExpandConstant('{app}\appsettings.Local.template.json'),
      ExpandConstant('{app}\appsettings.Local.json'),
      False
    );

  end;

end;



procedure CurStepChanged(
  CurStep: TSetupStep
);

begin

  if CurStep = ssPostInstall then
  begin

    CriarConfiguracaoInicial;

  end;

end;



[Run]


; inicia o Tray depois que o configurador fechar

Filename: "{app}\WordMonitor.Tray.exe"; \
Flags: postinstall nowait skipifsilent




[UninstallRun]


Filename: "{sys}\sc.exe"; \
Parameters: "stop WordMonitor"; \
Flags: runhidden waituntilterminated



Filename: "{sys}\sc.exe"; \
Parameters: "delete WordMonitor"; \
Flags: runhidden waituntilterminated

[Code]

procedure CurUninstallStepChanged(
  CurUninstallStep: TUninstallStep
);

begin

  if CurUninstallStep = usUninstall then
  begin

    RegDeleteValue(
      HKCU,
      'Software\Microsoft\Windows\CurrentVersion\Run',
      'WordMonitor'
    );

  end;

end;
