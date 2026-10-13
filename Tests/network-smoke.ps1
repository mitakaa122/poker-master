$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
$portProbe = [System.Net.Sockets.TcpListener]::new([System.Net.IPAddress]::Loopback, 0)
$portProbe.Start()
$port = $portProbe.LocalEndpoint.Port
$portProbe.Stop()
$server = Start-Process -FilePath "$root\bin\PokerServer.exe" -ArgumentList $port -WindowStyle Hidden -PassThru -RedirectStandardOutput "$root\bin\smoke-server.log" -RedirectStandardError "$root\bin\smoke-server-error.log"
function Send-Request($request, [switch]$Fragmented) {
    $client = [System.Net.Sockets.TcpClient]::new('127.0.0.1', $port)
    try {
        $client.ReceiveTimeout = 4000
        $stream = $client.GetStream()
        $bytes = [Text.Encoding]::UTF8.GetBytes(($request | ConvertTo-Json -Compress) + "`n")
        if ($Fragmented) {
            foreach ($b in $bytes) { $stream.WriteByte($b) }
        } else { $stream.Write($bytes, 0, $bytes.Length) }
        $reader = [IO.StreamReader]::new($stream)
        $reply = $reader.ReadLine() | ConvertFrom-Json
        if ($reply.Error) { throw $reply.Error }
        return $reply
    } finally { $client.Dispose() }
}
try {
    $connected = $false
    for ($i = 0; $i -lt 30; $i++) {
        try { $probe = [Net.Sockets.TcpClient]::new('127.0.0.1', $port); $probe.Dispose(); $connected = $true; break } catch { Start-Sleep -Milliseconds 100 }
    }
    if (!$connected) { throw 'Test server did not start.' }
    $tokens = @([Guid]::NewGuid().ToString('N'), [Guid]::NewGuid().ToString('N'))
    # A client that sends nothing must not prevent other players from connecting.
    $slow = [Net.Sockets.TcpClient]::new('127.0.0.1', $port)
    for ($i = 0; $i -lt 2; $i++) { $state = Send-Request @{ Command = 'join'; Name = "Smoke$i"; Token = $tokens[$i] } -Fragmented }
    $slow.Dispose()
    $actions = 0
    while ($state.Stage -ne 'Finished' -and $actions -lt 30) {
        $token = $tokens[$state.Turn]
        $state = Send-Request @{ Command = 'state'; Token = $token }
        $action = if ($state.ToCall -gt 0) { 'call' } else { 'check' }
        $state = Send-Request @{ Command = 'action'; Action = $action; Token = $token; Version = $state.Version }
        $actions++
    }
    if ($state.Stage -ne 'Finished' -or $state.Board.Count -ne 5) { throw 'Network hand did not finish.' }
    if (($state.Players | Measure-Object Chips -Sum).Sum -ne 4000) { throw 'Network chip total mismatch.' }
    Write-Host "PASS: two TCP clients completed a hand in $actions actions; fragmented requests and a stalled peer handled."
} finally {
    if ($slow) { $slow.Dispose() }
    if (!$server.HasExited) { Stop-Process -Id $server.Id }
}
