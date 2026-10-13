# Texas Hold'em LAN

Two-player Windows poker: a console server, the restored Windows Forms table, and a console client usable by Codex. Play chips only. Each player starts with 2,000 chips; blinds are 10/20 and the dealer alternates.

## Build and test

Run from this folder in PowerShell:

```powershell
.\build.ps1
.\test.ps1
.\Tests\network-smoke.ps1
```

The build uses the Windows .NET Framework compiler already installed on this PC. No NuGet packages or .NET SDK are required. The script explicitly selects the active source files; old server model/controller files are retained for reference and are not compiled.

## Play

1. Run `bin\PokerServer.exe`. The default TCP port is 8805; a different port can be passed as its first argument.
2. Run `bin\PokerClient.exe`, enter your name, server address, and port, and click **Join table**.
3. A second player connects to the same server. The first hand starts when both seats are occupied.

Use `127.0.0.1` when client and server run on the same computer. On another computer in the LAN, use the server's LAN IPv4 address printed at startup. If Windows Firewall prompts, allow the server on your private network. No router port forwarding is needed for LAN play. Copy the `bin\PokerClient.exe` file to the other Windows PC; cards are drawn directly by the client.

**Check**, **Call**, and **Fold** perform the corresponding actions. **Raise to** means the total amount you will have put in on the current street, including any earlier bet or blind. The displayed maximum is the effective stack: the amount the other player can match. This avoids unmatched excess bets in heads-up play. An all-in that is below the ordinary minimum raise is allowed at this maximum. Both players click **Next hand** after a result; a player at zero chips ends the match.

The server validates turn order, amounts, and state versions. Opponent cards are omitted until showdown and stay hidden after a fold. Requests use bounded, newline-delimited UTF-8 JSON. A delayed or repeated action cannot silently apply to a different state.

## Reconnect and restart

Temporary connection failures retry automatically. Reopen the client with the same name, host and port to recover your seat; its session token is stored in `bin\sessions`. Seats remain reserved if a player closes the window, so the game waits for them to reconnect. There is no automatic fold timer or seat replacement yet. To start a new match, stop and restart the server; connected clients rejoin automatically. Game state is in memory and is lost on server shutdown.

## Console client

```powershell
.\bin\PokerCli.exe .\bin\codex.session.json join 127.0.0.1 8805 Codex
.\bin\PokerCli.exe .\bin\codex.session.json state
.\bin\PokerCli.exe .\bin\codex.session.json action 3 call
.\bin\PokerCli.exe .\bin\codex.session.json action 5 raise 100
.\bin\PokerCli.exe .\bin\codex.session.json ready 12
```

Replace example version numbers with `Version` from the latest state. The console client has the same server permissions and private view as the graphical client. Preserve its session file to reconnect. Codex chooses actions during an active conversation; the console client is not an autonomous bot.

## Current scope

One table, exactly two players, local/private network play. Internet hosting, spectators, accounts, and more than two seats are not implemented. LAN connectivity from a second physical PC must be verified on that PC; localhost tests do not establish firewall reachability.
