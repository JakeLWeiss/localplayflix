<?php

include "networking.php";

echo "entire post request" . PHP_EOL;
print_r($_POST);
$keys = array_keys($_POST);
$values = array_values($_POST);

echo PHP_EOL . "print each json as object" . PHP_EOL;
foreach($keys as $key){
	print_r(json_decode($_POST[$key]));
}



//e79a2ce31204e7510c3155c4bc8f1a0cbc20d62a5263783dfa9ca6075d891513ba15b7f8a8b5a96259cd1fe216b13e871ca1


print_r()
//print_r(json_decode($_POST["thing"]));
?>
