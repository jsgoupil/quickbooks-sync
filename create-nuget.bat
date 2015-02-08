rd /S /Q .\publish

call build.bat
if %errorlevel% neq 0 exit /b %errorlevel%

md .\publish

tools\nuget\nuget.exe pack .\src\QbXml\QbXml.csproj -OutputDirectory .\publish
if %errorlevel% neq 0 exit /b %errorlevel%