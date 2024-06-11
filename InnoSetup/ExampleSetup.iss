; https://github.com/DomGries/InnoDependencyInstaller


; requires netcorecheck.exe and netcorecheck_x64.exe (see CodeDependencies.iss)
#define public Dependency_Path_NetCoreCheck "dependencies\"

; requires dxwebsetup.exe (see CodeDependencies.iss)
;#define public Dependency_Path_DirectX "dependencies\"

#include "CodeDependencies.iss"

[Setup]
#define MyAppSetupName 'Smurftown'
#define MyAppVersion '2024.6.0'
#define MyAppPublisher 'ZrdJ'
#define MyAppCopyright 'Copyright © ZrdJ'
#define MyAppURL 'https://github.com/ZrdJ/smurftown'

AppName={#MyAppSetupName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppSetupName} {#MyAppVersion}
AppCopyright={#MyAppCopyright}
VersionInfoVersion={#MyAppVersion}
VersionInfoCompany={#MyAppPublisher}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
OutputBaseFilename={#MyAppSetupName}-{#MyAppVersion}
DefaultGroupName={#MyAppSetupName}
DefaultDirName={autopf}\{#MyAppSetupName}
UninstallDisplayIcon={app}\Smurftown.exe
OutputDir={#SourcePath}\bin
AllowNoIcons=yes
PrivilegesRequired=admin

; remove next line if you only deploy 32-bit binaries and dependencies
ArchitecturesInstallIn64BitMode=x64

[Languages]
Name: en; MessagesFile: "compiler:Default.isl"
Name: fr; MessagesFile: "compiler:Languages\French.isl"
Name: it; MessagesFile: "compiler:Languages\Italian.isl"
Name: de; MessagesFile: "compiler:Languages\German.isl"
Name: es; MessagesFile: "compiler:Languages\Spanish.isl"

[Files]
Source: "executables\win-x64\publish\Smurftown.exe"; DestDir: "{app}"; DestName: "Smurftown.exe"; Check: Dependency_IsX64; Flags: ignoreversion
Source: "executables\win-x86\publish\Smurftown.exe"; DestDir: "{app}"; Check: not Dependency_IsX64; Flags: ignoreversion

[Icons]
Name: "{group}\{#MyAppSetupName}"; Filename: "{app}\Smurftown.exe"
Name: "{group}\{cm:UninstallProgram,{#MyAppSetupName}}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#MyAppSetupName}"; Filename: "{app}\Smurftown.exe"; Tasks: desktopicon

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"

[Run]
Filename: "{app}\Smurftown.exe"; Description: "{cm:LaunchProgram,{#MyAppSetupName}}"; Flags: nowait postinstall skipifsilent

[Code]
function InitializeSetup: Boolean;
begin
  // comment out functions to disable installing them
  Dependency_AddDotNet80Desktop;
 
  Result := True;
end;
