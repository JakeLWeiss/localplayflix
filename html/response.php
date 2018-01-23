<?php
echo "entire post request" . PHP_EOL;
print_r($_POST);
$keys = array_keys($_POST);

echo PHP_EOL . "print each json as object" . PHP_EOL;
foreach($keys as $key){
	print_r(json_decode($_POST[$key]));
}
//print_r(json_decode($_POST["thing"]));
?>
