#!/bin/bash

sudo mkdir -p /var/lib/serversight/ || true
sudo apt update
sudo apt install sysstat || true

# getting latest versoin
releaseversion=$( curl -s https://api.github.com/repos/lukasvdberk/ServerSightPostScript/releases/latest | grep browser_download_url | cut -d '"' -f 4 | cut -d'/' -f8- | curl -s https://api.github.com/repos/lukasvdberk/ServerSightPostScript/releases/latest | grep browser_download_url | cut -d '"' -f 4 | cut -d'/' -f8- | cut -f1 -d"/" | head -n 1)
sudo touch /var/lib/serversight/serversight-post-script
sudo chmod 777 /var/lib/serversight/serversight-post-script
sudo wget -c https://github.com/lukasvdberk/ServerSightPostScript/releases/download/${releaseversion}/ServerSightPostScript -P /var/lib/serversight/
sudo wget -c https://github.com/lukasvdberk/ServerSightPostScript/releases/download/${releaseversion}/serversight-post.service -P /etc/systemd/system/
sudo chmod 644 /etc/systemd/system/serversight-post.service

sudo chmod 777 /var/lib/serversight/ServerSightPostScript

echo 'What is the id of the server? (you can find this on the server page)'
read serverSightId

echo 'What is your API key? (you can generate one on the api key management page)'
read apiKey

sudo touch /var/lib/serversight/.env
sudo chmod 777 /var/lib/serversight/.env
sudo printf "SERVER_SIGHT_API_KEY=$apiKey\nSERVER_SIGHT_SERVER_ID=$serverSightId" > /var/lib/serversight/.env
sudo chmod 740 /var/lib/serversight/.env

echo 'Written to /var/lib/serversight/.env'

sudo systemctl enable serversight-post.service
sudo systemctl start serversight-post.service