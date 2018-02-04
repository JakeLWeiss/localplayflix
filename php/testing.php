<?php
include 'networking.php';
$conn = new databaseConnection;
$conn->connect();

//$data = $conn->select("Select user, host from mysql.user", array());

//$data = $conn->select("Select * from mysql.user where user = ?", array("dustin"));
$data = $conn->select("Select * from mysql.user where user = ?", array($_POST['name']));



echo json_encode($data);

/*
foreach($data as $entry){
	print_r($entry);
	echo '<br>';
}

echo $conn->getCurrentMax("movies", 'name');


//adventures in adding and inserting a movie, with metadata
$meta = new Meta($conn, 1, "movie", 1);
$meta->addMeta("testkey", "testval");
$meta->addMeta("testkey2", "testval2");

$meta->createMultipleMeta($meta->entries);

*/

?>
