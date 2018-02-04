<?php
include "networking.php";

$conn = new databaseConnection;
$conn->connect();

$stmt = $conn->pdo->prepare("update movies set currenttime=? where movieid=1");
$stmt->execute(array($_POST['time']));
 echo json_encode($conn->select("Select currenttime from movies where movieid=1", array())[0]);

?>
