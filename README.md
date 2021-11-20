# Server sight post script
This script will post statistics/information about your server to the server sight api.

For the other repositories that belong to this collection of repos:

- https://github.com/lukasvdberk/ServerSightFrontend
- https://github.com/lukasvdberk/ServerSightAPI

Currently runs at [this](https://serversight.lukas.sh) url.


### Use for your server
If you have a debian based system you can run the following script follow the following steps.

-  Go to releases and execute the setup.sh from the latest release.
-  Or use this simple one liner `sh <(curl -LJ https://github.com/lukasvdberk/ServerSightPostScript/releases/download/1.0/setup.sh) -y`

### Development 
Make sure you have the lastest version of dotnet installed

Then you can run in the root folder of the project.
```bash
dotnet run
```

If you want to make a new release run
```bash
chmod +x Deployment/make-release.sh
./Deployment/make-release.sh
```
