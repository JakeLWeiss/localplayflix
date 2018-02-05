<?php
include "networking.php";

$conn = new databaseConnection;
$conn->connect();

$stmt = $conn->pdo->prepare("update movies set currenttime=? where filename=?");
$stmt->execute(array($_POST['time'], $_POST['filename']));
 echo json_encode($conn->select("Select currenttime from movies where filename=?", array($_POST['filename']))[0]);

?>
