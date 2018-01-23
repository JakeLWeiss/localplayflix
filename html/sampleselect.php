<?php
include 'networking.php';


$conn = new databaseConnection;
$conn->connect();

$result = $conn->pdo->query("select * from movies where name like 'Mirai%'")->fetch();

print_r($result);
?> 
