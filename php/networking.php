<?php

//for the initial connection as well as data retrieval

class databaseConnection{
	public $pdo;
	
	function __construct(){
	
	}
	
	//mysql initial connection
	function connect(){
		$host = 'localhost';
		$db = 'localplayflix';
		$user = 'localplayflix';
		$pass = 'changeme';
		$charset = 'utf8';

		$dsn = "mysql:host=$host;dbname=$db;charset=$charset";
		$opt = [
			PDO::ATTR_ERRMODE => PDO::ERRMODE_EXCEPTION,
			PDO::ATTR_DEFAULT_FETCH_MODE => PDO::FETCH_ASSOC,
			PDO::ATTR_EMULATE_PREPARES => false,
		];

		$this->pdo = new PDO($dsn, $user, $pass, $opt);
	}
	
	//generic insert, by 2 arrays of names and values and then the table name
	function insert(array $names, array $values, string $table){
		$names = implode(", ", $names);
		$qs = "?";
		for($i = 1; $i < count($values); $i++){
			$qs .= ", ?";
		}
		//echo $qs . PHP_EOL;
		$query = "INSERT INTO $table ($names) values ($qs)";
		echo $query . PHP_EOL;
		$stmt = $this->pdo->prepare($query);
		print_r($stmt);
		$stmt->execute($values);
	}
	
	//this will be ideal for submitting metadata, since it can be single tuple with an array of binary value tuples 
	function insertMultiple(array $names, array $valueArray, string $table){
		foreach($valueArray as $entry){
			$this->insert($names, $entry, $table);
		}
	}
	
	//takes a PDO query and executes it with the arguments; returns an object of all the data returned.
	function select($query, array $executeArguments){
		$stmt = $this->pdo->prepare($query);
		$stmt->execute($executeArguments);
		$result = [];
		while($row = $stmt->fetch()){
			$result[] = $row;
		}
		return $result;
	}
	
	//TODO make this actually work with the current setup. currently just a straight import from old stuff.
	function selectMetaByTypeID($type, $ID){
		$stmt1 = $conn->pdo->query("SELECT * from posts Order by metakey desc");
		while($row = $stmt1->fetch()){

			echo "<br>Metakey: " . $row["metakey"] . PHP_EOL . "<br>";
			echo "Title: " . $row["title"] . "<br>";
			echo "Content: " . $row["content"] . "<br>";
			echo "Author: " . $row["author"] . "<br>";
			echo "Post date: " . $row["postdate"] . "<br>";
			echo "Last edited: " . $row["editdate"] . "<br>";
			$stmt2 = $conn->pdo->prepare("SELECT * FROM meta WHERE metakey = ?");
			$stmt2->execute([$row["metakey"]]);
			
			echo "<br>Metadata: <br>";
			while($val = $stmt2->fetch()){
				echo "<br>" . $val["v"];
			}
			
			echo "<br>" . "<br>";
		}
	}
	
	function update(){
	
	}
	
	
	//lookup section
	
	function retrievePost(){
	
	}
	
	function retrievePostsById(){
	
	}
	
	function retrievePostsByDate(){
	
	}
	
	function retrievePostsByAuthor(){
	
	}
	
	function retrievePostsByMeta(){
	
	}
	
	//gets the current maxid in meta (this could be done with autoincrement, but i dont want to trust that at the moment)
	function getCurrentMetaID(){
		return $this->pdo->query("SELECT MAX(metaid) as 'maxmetaid' FROM meta")->fetch()["maxmetaid"];
	}
	
	//gets the maximum property from a certain table; also works in lexicographic order
	function getCurrentMax($table, $property){
		return $this->pdo->query("SELECT MAX($property) as 'max' FROM $table")->fetch()["max"];
	}


	
	
	
	
}

class Media{
	public $dbc;
	public $title;
	public $contentLocation;
	public $description;
	private $owner;
	private $ownerid;
	public $metakey;
	public $metadata;
	
	//pass dbc by reference to save on performance
	function __construct(&$dbc, $meta){
		$this->title = $title;
		$this->content = $content;
		$this->dbc = $dbc;
	}
	
	
	
	
	function createPost(){
		if($_SESSION["admin"] === 1){
		
			//TODO get autoincrement
			$this->metakey = $this->dbc->getCurrentMetaID() + 1; //TODO do this earlier, as constructor, allows for meta creation
			//$m = $metakey->fetch()["maxmetakey"];
			//TODO add user security check
			
			
			$this->dbc->insert(array("title", "content", "author", "authorid", "metakey", "postdate"), array($this->title, $this->content, $this->author, $this->authorid, $this->metakey, date("Y-m-d H:i:s")), "posts");
			//TODO meta insert thing, gotta get a better way of architecting that. I have a headache rn, Ill do that later
			
			
		}else{
			echo "wrong " . $_SESSION['admin'] . "<br>";
		}
		
	}
	
	function updatePost(){
	
	}
	
	function deletePost(){
	
	}
}

class Meta{
	public $dbc;
	public $entries;
	public $metaID;
	public $type;
	public $typeID;
	//TODO restructure 
	
	function __construct(&$dbc, $metaID, $type, $typeID){
		$this->dbc = $dbc;
		$this->metaID = $metaID;
		$this->entries = array();
		$this->type = $type;
		
		$this->metaID = $dbc->getCurrentMax("meta", "metaid") + 1;
		$this->typeID = $typeID;
	}
	
	//adds the extra stuff for the multiple meta instert. modular and all
	function addMeta($k, $v){
		$this->entries[] = [$k, $v];
	}
	
	function createMeta($k, $v){
		$this->dbc->insert(array("metaid", "metakey", "metavalue", $this->type."id"), array($this->metaID, $k, $v,  $this->typeID), "meta");
	}
	
	function createMultipleMeta($entries){
		foreach($entries as $entry){
			$this->createMeta(...$entry);
			$this->metaID++;
		}
		//$this->dbc->insertMultiple(array( "metaid", "metakey", "metavalue", "$this->type", "$this->typeID"), array($this->entries), "meta");
	}
	
	function updateMeta(){
	
	}
	
	function removeMeta(){
	
	}
	

}

class UserManagement{
	public $dbc;
	
	function createUser(){
	}
	
	function deleteUser(){
	}	
}

class User{
	//TODO UNSECURE AS HELL DO NOT LAUNCH TODO TODO TODO
	public $dbc;
	
	function __construct(&$dbc){
		$this->dbc = $dbc;
	}
	
	function login($username, $userid, $password){
		$stmt = $this->dbc->pdo->prepare("SELECT * from users where userid = ?");
		$stmt->execute([$userid]);
		$result = $stmt->fetch();
		if($password === $result["password"]){
			session_start();
			$_SESSION = $result;
			
			//$_SESSION["username"] = $username;
			//$_SESSION["userid"] = $userid;
			var_dump($_SESSION);
		}else{
			echo "wrong<br>";
		}
		
	}
	
	function logout(){
		session_unset();
		session_destroy();
	}

}
/*
$conn = new databaseConnection;
$conn->connect();

//$data = $conn->select("Select user, host from mysql.user", array());

$data = $conn->select("Select user, host from mysql.user where user = ?", array("dustin"));
//print_r($data);
foreach($data as $entry){
	print_r($entry);
	echo '<br>';
}
//var_dump($data->fetch());

/*
$conn = new databaseConnection;
$conn->connect();
//echo phpinfo() . PHP_EOL;


$stmt1 = $conn->pdo->query("Select * from movies limit 0,5");
//var_dump($stmt1);
while($row = $stmt1->fetch()){
	print_r($row);
}*/
//generate a ton of random data to populate database
// for($i = 0; $i < 1000; $i++){
// 	$conn->insert(array("name", "description", "location"), array(bin2hex(random_bytes(50)), bin2hex(random_bytes(100)), 	bin2hex(random_bytes(100))), "movies");
// }
//print_r(array(bin2hex(random_bytes(50)), bin2hex(random_bytes(1000)), bin2hex(random_bytes(100))));
?>

