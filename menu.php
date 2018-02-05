<?php
include 'php/networking.php';
$conn = new databaseConnection;
$conn->connect();
?>
<!DOCTYPE html>
<head>
	<meta charset="UTF-8">
	<title> lpflix </title>
	<link rel="stylesheet" type="text/css" href="../css/menustyle.css">
	<script
  src="https://code.jquery.com/jquery-3.3.1.min.js"
  integrity="sha256-FgpCb/KJQlLNfOu91ta32o/NMZxltwRo8QtmkMRdAu8="
  crossorigin="anonymous"></script>
	
	
		
</head>		
<body>	

<pre>
<?php

$data = $conn->select("SELECT * from movies limit ?,?", array(0, 10));
foreach($data as $entry){
		print('<li>'.PHP_EOL);
			print("<a href='" . "php/player.php?add=".$entry["filename"]. "'>");
			echo '<img src="">';
			echo '<h3>' . $entry['name'] . '</h3>';
			echo '<p>' . $entry['description'].'</p></a>';
		echo '</li>';
	//print_r($entry);
}
?>
</pre>

	<ul class="medialist"></ul>
	<script type='text/javascript'>
	
	$(document).ready(function() {
		
		var requestURL = 'json/metadatafull.json';
		var request = new XMLHttpRequest();
		request.open('GET', requestURL, true);
		request.responseType = 'json';

		request.send();

		request.onload = function() {
			var jsonresp = request.response;
  			populate(jsonresp);
		}
		
		function populate(jsonObj) {
  			
			for (var i = 0; i < jsonObj.length; i++){
				
  				var obj = jsonObj[i];
  				$('.medialist').append(
  					$('<li>').append(
  						$('<a>').attr('href', 'php/player.php?add='+obj['id']).append(
  							$('<img>').attr('src', obj['thumbnail']),
  							$('<h3>').append(obj['name']),
  							$('<p>').append(obj['description'])
  						)
  					)
  				);

  			}

  			
		}
	});

	</script>
</div>
</body>
</html>
