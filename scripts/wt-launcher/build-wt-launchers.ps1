$ErrorActionPreference = 'Stop'

$repoRoot = Resolve-Path (Join-Path $PSScriptRoot '..\..')
$source = Join-Path $PSScriptRoot 'EchoBirdWtLauncher.cs'
$outDir = Join-Path $env:USERPROFILE '.echobird\wrappers'
$candidates = @(
    "$env:WINDIR\Microsoft.NET\Framework64\v4.0.30319\csc.exe",
    "$env:WINDIR\Microsoft.NET\Framework\v4.0.30319\csc.exe"
)

$csc = $candidates | Where-Object { Test-Path -LiteralPath $_ } | Select-Object -First 1
if (-not $csc) {
    throw 'Could not find csc.exe from .NET Framework 4.x.'
}

New-Item -ItemType Directory -Force -Path $outDir | Out-Null

& $csc /nologo /target:winexe /optimize+ /out:"$outDir\echobird-wt-claudecode.exe" $source
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

& $csc /nologo /target:winexe /optimize+ /out:"$outDir\echobird-wt-codex.exe" $source
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Host "Built Windows Terminal launchers in $outDir"
