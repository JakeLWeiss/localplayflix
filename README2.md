# localplayflix
A local network video streaming player

Some notes about setup. There are some configurations to change in mysql, apache2.conf, default-000.conf, and php.ini 

in mysql create an account with a password and not unix_socket as the authentication method, and use this in the networking.php Database class.

/etc/apache2/apache2.conf :on the directory that includes /var/www/ change to the parent directory of the html folder

/etc/apache2/sites-available/000-default.conf : change Document root to /path/to/html (including html)

/etc/php/7.0/apache2/php.ini change display_errors to On

all of this needs to be done as root. 
