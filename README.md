# <img src="https://bionicbeanie.me/McStatBot.png" alt="drawing" width="30"/> McStatBot 

Discord bot to show server status and player profile. 

![.NET](https://github.com/cool-mist/McStatBot/workflows/.NET/badge.svg?branch=main) 

Add bot to your discord server using this [invite](https://discord.com/api/oauth2/authorize?client_id=716535144344977469&permissions=84992&scope=bot) 

## Commands

Name|Description|Usage
---|---|---
!help|Show help text| !help
!status <server-name>| Show minecraft server status| !status 2b2t.org
!show | Show sever status of the minecraft server registered in the guild | !show
!register <server-name>| Registers a minecraft server for the guid | !register 2b2t.org
!profile <player-name>| Show player profile information | !profile PotatoChips3814 

## Self hosting

- Requires [.NET Core 3+](https://github.com/dotnet/runtime)
- Set token as the environment variable `TOKEN` for the process

```sh
$> dotnet restore
$> dotnet build --no-restore
$> dotnet publish -c Release
...
<Go to releases folder>
...
$> dotnet McStatBot.dll
```
