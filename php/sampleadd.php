<?php
include 'networking.php';


$obj = json_decode($_POST["thing"]);
print_r($obj);
$conn = new databaseConnection;
$conn->connect();

$conn->insert(array("name", "description", "location"), array($obj->name, $obj->description, $obj->id) , 'movies');
?>
