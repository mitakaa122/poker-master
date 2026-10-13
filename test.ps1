$ErrorActionPreference = 'Stop'
Push-Location $PSScriptRoot
try {
    $compiler = Join-Path $env:WINDIR 'Microsoft.NET\Framework64\v4.0.30319\csc.exe'
    New-Item -ItemType Directory -Force bin | Out-Null
    & $compiler /nologo /target:exe /out:bin\PokerTests.exe /r:System.Web.Extensions.dll Shared\Protocol.cs TexasHoldemServer\TexasHoldemServer\Game.cs TexasHoldemServer\TexasHoldemServer\HandRank.cs Tests\GameTests.cs
    if ($LASTEXITCODE) { throw 'Test build failed.' }
    & .\bin\PokerTests.exe
    if ($LASTEXITCODE) { throw 'Game tests failed.' }
} finally { Pop-Location }
