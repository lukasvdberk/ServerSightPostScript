# Server sight post script
This script will post statistics/information about your server to the server sight api.

### Use for your server
If you have a debian based system you can run the following script follow the following steps.

-  Go to releases and execute the setup.sh from the latest release.

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