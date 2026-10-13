$ErrorActionPreference = 'Stop'
$compiler = Join-Path $env:WINDIR 'Microsoft.NET\Framework64\v4.0.30319\csc.exe'
if (!(Test-Path -LiteralPath $compiler)) { throw 'The Windows .NET Framework compiler was not found.' }
Push-Location $PSScriptRoot
try {
    New-Item -ItemType Directory -Force bin | Out-Null
    $shared = 'Shared\Protocol.cs'
    $server = @('TexasHoldemServer\TexasHoldemServer\ServerMain.cs', 'TexasHoldemServer\TexasHoldemServer\Game.cs', 'TexasHoldemServer\TexasHoldemServer\HandRank.cs')
    & $compiler /nologo /target:exe /out:bin\PokerServer.exe /r:System.Web.Extensions.dll $shared @server
    if ($LASTEXITCODE) { throw 'Server build failed.' }
    $client = @('TexasHoldemClient\TexasHoldemClient\Program.cs', 'TexasHoldemClient\TexasHoldemClient\LoginForm.cs', 'TexasHoldemClient\TexasHoldemClient\LoginForm.Designer.cs', 'TexasHoldemClient\TexasHoldemClient\PokerWindow.cs', 'TexasHoldemClient\TexasHoldemClient\PokerWindow.Designer.cs')
    & $compiler /nologo /target:winexe /out:bin\PokerClient.exe /r:System.Web.Extensions.dll /r:System.Windows.Forms.dll /r:System.Drawing.dll $shared @client
    if ($LASTEXITCODE) { throw 'Client build failed.' }
    & $compiler /nologo /target:exe /out:bin\PokerCli.exe /r:System.Web.Extensions.dll $shared ConsoleClient\Program.cs
    if ($LASTEXITCODE) { throw 'Console client build failed.' }
    Write-Host 'Built bin\PokerServer.exe, bin\PokerClient.exe and bin\PokerCli.exe'
} finally { Pop-Location }
