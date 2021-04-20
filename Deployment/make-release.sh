cd ..
#!/bin/bash

# build project
cd ..
cd ServerSightPostScript
dotnet publish -r linux-x64 /p:PublishSingleFile=true
cd Deployment

rm -r release/ || true
mkdir release/ || true

# copy to release folder
cp setup.sh release/
cp serversight-post.service release/
cp ../ServerSightPostScript/bin/Debug/net5.0/linux-x64/publish/ServerSightPostScript release
