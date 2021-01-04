#define MyUnInstallerName "ATI.PlayerMax Offer Export Tool"
#define MyAppName "ATI.PlayerMax Offer Export Tool"
#define MyAppVerName "ATI.PlayerMax Offer Export Tool"
#define MyAppPublisher "Aristocrat Technologies, Inc"
#define MyInstallerExeName "PlayerMaxOfferExportToolInstaller"
#define MyDirectory "PlayerMaxOfferExportTool"
#define ProgramGroup "PlayerMaxOfferExportTool"
#define ProgramFileName "OfferExport.exe"
#define ProgramDescription "PlayerMax Offer Export Tool"
#define ServiceConfigFileName "appSettings.json"
#define ProgramReleaseDir "..\bin\Release\netcoreapp3.1"
#define ModuleName "PlayerMaxOfferExportTool"
#define ServiceEncryptBatchFile "encrypt.bat"

#ifndef VERSION
  #define VERSION "0.0.0.0"
#endif

[Setup]
AppName={#MyAppName}
AppVerName={#MyAppVerName}
AppPublisher={#MyAppPublisher}
DefaultDirName={pf}\Aristocrat\PlayerMax\{#MyAppName}
DefaultGroupName={#MyAppName}
OutputBaseFilename={#MyInstallerExeName}
OutputDir=SetupFiles\SeparateInstallers
Compression=lzma
SolidCompression=true
CreateAppDir=true
UninstallDisplayName={#MyUnInstallerName}
AppVersion={#VERSION}
VersionInfoVersion={#VERSION}
DisableStartupPrompt=true
DisableDirPage=yes
DisableProgramGroupPage=yes
DisableReadyPage=yes

[Files]
// Be sure to publish from Visual Studio
Source: ..\published\*.*; DestDir: {app}; Excludes: *.pdb, *vshost*; Flags: ignoreversion replacesameversion recursesubdirs createallsubdirs touch
Source: {#ProgramReleaseDir}\Queries\*; DestDir: "{app}\Queries"; Flags: ignoreversion

[Icons]
Name: {group}\Offer Export Tool; Filename: {app}\{#ProgramFileName}; WorkingDir: {app}; IconFilename: {app}\{#ProgramFileName}; IconIndex: 0
Name: {commondesktop}\PlayerMax Offer Export Tool; Filename: {app}\{#ProgramFileName}; WorkingDir: {app}; IconFilename: {app}\{#ProgramFileName}; IconIndex: 0
