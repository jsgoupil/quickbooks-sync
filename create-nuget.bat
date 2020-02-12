rd /S /Q .\publish

call build.bat
if %errorlevel% neq 0 exit /b %errorlevel%

md .\publish

dotnet pack .\src\QbXml\QbXml.csproj -c Release --include-symbols --no-build -o .\publish
if %errorlevel% neq 0 exit /b %errorlevel%

dotnet pack .\src\WebConnector\WebConnector.csproj -c Release --include-symbols --no-build -o .\publish
if %errorlevel% neq 0 exit /b %errorlevel%

dotnet pack .\src\WebConnector.AspNetCore\WebConnector.AspNetCore.csproj -c Release --include-symbols --no-build -o .\publish
if %errorlevel% neq 0 exit /b %errorlevel%