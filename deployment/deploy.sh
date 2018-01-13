#/!bin/bash

apt install apache2 -y
apt install mysql-server -y
apt install mysql-client -y
apt install php7.0-cli -y
apt install libapache2-mod-php7.0 -y
apt install php-mysql -y
service apache2 restart

