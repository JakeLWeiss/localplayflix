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

print "<input type=\"text\" id=\"search\" onkeyup=\"myFunction()\" placeholder=\"Search for media item...\" title=\"search for media\">";

print "<script>\n";
print "function myFunction() {\n";
print "    var input, filter, ul, li, a, i;\n";
print "    input = document.getElementById(\"search\");\n";
print "    filter = input.value.toUpperCase();\n";
print "    ul = document.getElementById(\"MovieList\");\n";
print "    li = ul.getElementsByTagName(\"li\");\n";
print "    for (i = 0; i < li.length; i++) {\n";
print "        a = li[i].getElementsByTagName(\"a\")[0];\n";
print "        if (a.innerHTML.toUpperCase().indexOf(filter) > -1) {\n";
print "            li[i].style.display = \"\";\n";
print "        } else {\n";
print "            li[i].style.display = \"none\";\n";
print "\n";
print "        }\n";
print "    }\n";
print "}\n";
print "</script>";


echo '<ul id="MovieList">';
$data = $conn->select("SELECT * from movies limit ?,?", array(0, 10));
foreach($data as $entry){
		print('<li>'.PHP_EOL);
			print("<a href='" . "php/player.php?add=".$entry["filename"]. "'>");
			print("<img src='/image/".$entry["thumbnail"]."'> ");
			echo '<h3>' . $entry['name'] . '</h3>';
 			echo '<p>' . $entry['description'].'</p></a>';
		echo '</li>';
	//print_r($entry);
}
echo '</ul>'
?>
</pre>
<!--
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

	</script>-->
</div>
</body>
</html>
