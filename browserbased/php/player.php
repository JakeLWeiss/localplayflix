<html>
  <head>
    <title>LocalPlayFlix b/c localflix was taken</title>
   
    <meta content="">
    <style>
    *{
		font-family: Courier;
		color: white;
    }
    
    #vid{
		position: absolute;
		z-index: -1;
		top: 0;
		left: 0;
		width: 100%; 
		max-height: 100%;
		max-width:100%
		object-fit: cover;
    }
    
    </style>
    <script   src="http://code.jquery.com/jquery-3.3.1.min.js"
			  integrity="sha256-FgpCb/KJQlLNfOu91ta32o/NMZxltwRo8QtmkMRdAu8="
			  crossorigin="anonymous"></script>
    <script>
		$(document).ready(function(){
			var vid = $('#vid');
			console.log($('#vid'));
			vid.ready(function(){
				setInterval(update, 1000)
			});
		});
		
		
		function update(){
			if(!vid.paused){
				$.post("updatetime.php", { time: vid.currentTime})
				.done(function( data ) {
					console.log(JSON.parse(data));
				});
			}
			
		}
		
    </script>
  </head>
	<body bgcolor="#000">

		<video id='vid' controls>
			<source src="" type="video/mp4">
			fuck you 
		</video>


	</body>

</html>
