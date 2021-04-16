# TODO download release of binary file
# TODO get systemd file
# TODO copy binary file

sudo mkdir -p /var/lib/serversight/ || true

echo 'What is the id of the server? (you can find this on the server page)'
read serverSightId

echo 'What is your API key? (you can generate one on the api key management page)'
read apiKey

sudo touch /var/lib/serversight/.env
sudo chmod 777 /var/lib/serversight/.env
sudo printf "SERVER_SIGHT_API_KEY=$apiKey; \n SERVER_SIGHT_SERVER_ID=$serverSightId;" > /var/lib/serversight/.env
sudo chmod 740 /var/lib/serversight/.env

echo 'Written to /var/lib/serversight/.env'
# /var/lib/serversight/serversight-post-script
# /var/lib/serversight/.env

# sudo cp serversight-post.service /etc/systemd/system/serversight-post.service
# sudo chmod 644 /etc/systemd/system/serversight-post.service

# sudo systemctl enable serversight-post.service
# sudo systemctl start serversight-post.service