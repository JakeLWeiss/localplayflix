<?php
include 'networking.php';

$conn = new databaseConnection;
$conn->connect();

$result = $conn->pdo->query("select * from movies where name like '%" . $_POST['name'] . "%'")->fetch();

print_r($result);
?> 
