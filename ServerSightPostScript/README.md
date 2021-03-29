# Server sight post script
This script will post information about a specific server sight server.
It will gather the following information.
- CPU usage
- RAM usage
- Hard Disks
- Open ports
- Network adapters 
  - And its ip's

# Installation
Standard dotnet project. There is one other requirement for linux cpu usage and that is the following:
```bash
sudo apt install syssstat
```
Make use mpstat is available in your bash.
