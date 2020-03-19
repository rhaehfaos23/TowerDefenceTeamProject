!include "MUI2.nsh"

Unicode true

Name "TowerDefence"
OutFile "install.exe"
InstallDir $PROGRAMFILES64\TowerDefence
RequestExecutionLevel admin

var StartMenuFolder

;----------Page
!insertmacro MUI_PAGE_WELCOME
!define MUI_STARTMENUPAGE_REGISTRY_ROOT "HKCU" 
!define MUI_STARTMENUPAGE_REGISTRY_KEY "Software\TowerDefence" 
!define MUI_STARTMENUPAGE_REGISTRY_VALUENAME "Start Menu Folder"
!insertmacro MUI_PAGE_STARTMENU Application $StartMenuFolder
!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES

!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
;--------------

!insertmacro MUI_LANGUAGE "Korean"

Section "Install" install
  SectionIn RO
  SetOutPath $INSTDIR
  File TD.exe
  File UnityCrashHandler64.exe
  File UnityPlayer.dll
  File ndp48-devpack-enu.exe
  File /nonfatal /a /r "TD_Data"
  File /nonfatal /a /r "MonoBleedingEdge"
  
  !insertmacro MUI_STARTMENU_WRITE_BEGIN Application
    CreateDirectory "$SMPROGRAMS\$StartMenuFolder"
    CreateShortCut "$SMPROGRAMS\$StartMenuFolder\Uninstall.lnk" "$INSTDIR\Uninstall.exe"
    CreateShortCut "$SMPROGRAMS\$StartMenuFolder\TD.lnk" "$INSTDIR\TD.exe"
  !insertmacro MUI_STARTMENU_WRITE_END
  
  Exec 'ndp48-devpack-enu.exe /q /passive /norestart'
  WriteUninstaller "Uninstall.exe"
  
  WriteRegStr HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\TowerDefence" "DisplayName" "TowerDefence"
  WriteRegStr HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\TowerDefence" "UninstallString" '"$INSTDIR\Uninstall.exe"'
  
SectionEnd

LangString DESC_install ${LANG_KOREAN} "게임 설치"
!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
  !insertmacro MUI_DESCRIPTION_TEXT ${install} $(DESC_install)
!insertmacro MUI_FUNCTION_DESCRIPTION_END


Section "Uninstall"
  ;실제파일 지우기
  Delete "$INSTDIR\TD.exe"
  Delete "$INSTDIR\Uninstall.exe"
  Delete "$INSTDIR\UnityCrashHandler64.exe"
  Delete "$INSTDIR\UnityPlayer.dll"
  Delete "$INSTDIR\ndp48-devpack-enu.exe"
  RMDir /r "$INSTDIR\TD_Data"
  RMDir /r "$INSTDIR\MonoBleedingEdge"
  RMDir "$INSTDIR"
  
  ;시작메뉴 바로가기 지우기
  !insertmacro MUI_STARTMENU_GETFOLDER Application $StartMenuFolder
  Delete "$SMPROGRAMS\$StartMenuFolder\TD.lnk"
  Delete "$SMPROGRAMS\$StartMenuFolder\Uninstall.lnk"
  RMDir "$SMPROGRAMS\$StartMenuFolder"
  
  
  DeleteRegKey HKLM 'Software\Microsoft\Windows\CurrentVersion\Uninstall\TowerDefence'
  DeleteRegKey HKCU 'Software\TowerDefence'
  
  
SectionEnd