#!/bin/bash

apt-get update
apt-get install apache2 -y
apt-get install mysql-server -y
apt-get install mysql-client -y
apt-get install php7.0-cli -y
apt-get install libapache2-mod-php7.0 -y
apt-get install php-mysql -y
service apache2 restart
