$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
$vswhere = Join-Path ${env:ProgramFiles(x86)} "Microsoft Visual Studio\Installer\vswhere.exe"
$msbuild = & $vswhere -latest -products * -requires Microsoft.Component.MSBuild -find "MSBuild\**\Bin\MSBuild.exe" | Select-Object -First 1
if (-not $msbuild) { throw "MSBuild not found" }
$csproj = Join-Path $root "JClientBot.csproj"
& $msbuild $csproj /m /p:Configuration=Debug /p:Platform=AnyCPU
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
Write-Host "Build succeeded: $root\bin\Debug\JClientBot.exe"
