tools\nuget\nuget.exe restore QuickbooksSync.sln

set MSBUILD="C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe"

rd /S /Q .\src\QbXml\bin\Debug
rd /S /Q .\src\QbXml\bin\Release

%MSBUILD% QuickbooksSync.sln /p:Configuration="Debug"
if %errorlevel% neq 0 exit /b %errorlevel%

%MSBUILD% QuickbooksSync.sln /p:Configuration="Release"
if %errorlevel% neq 0 exit /b %errorlevel%