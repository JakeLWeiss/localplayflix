<?php
include 'networking.php';
$conn = new databaseConnection;
$conn->connect();

//$data = $conn->select("Select user, host from mysql.user", array());

$data = $conn->select("Select user, host from mysql.user where user = ?", array("dustin"));
//print_r($data);
foreach($data as $entry){
	print_r($entry);
	echo '<br>';
}
?>
