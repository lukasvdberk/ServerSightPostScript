#!/bin/bash

sudo mkdir -p /var/lib/serversight/ || true
sudo apt update
sudo apt install sysstat || true

sudo touch /var/lib/serversight/serversight-post-script
sudo chmod 777 /var/lib/serversight/serversight-post-script
sudo wget -c https://github.com/lukasvdberk/ServerSightPostScript/releases/download/0.1/ServerSightPostScript -P /var/lib/serversight/
sudo wget -c https://github.com/lukasvdberk/ServerSightPostScript/releases/download/0.1/serversight-post.service -P /etc/systemd/system/
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

# TODO download release of binary file
# TODO get systemd file
# TODO copy binary file
