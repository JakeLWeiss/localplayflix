
using Binding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace lpflix {

    public partial class player : Window {

        //vars for cheking the scrubbing state, window state, and the play state
        bool isDragging = false;
        bool isPlaying = true;
        bool fullScreen = false;
        double volHist; //remembering the volume for mute toggle
        Movie m = new Movie(); //metadata for the movie
        Movies movies = new Movies(); //list of the movies
        DispatcherTimer hide = new DispatcherTimer(); //time to hide the controll bar (global in order to be used in action listener)
        
        public player() {
            InitializeComponent();         

            loadMedia();//loads media to play

            DispatcherTimer timer = new DispatcherTimer(); //dispatch timer in order to update the scrubbing
            timer.Interval = TimeSpan.FromSeconds(1); //update the scrub bar every second and tick the timer
            timer.Tick += timer_Tick;
            timer.Start();
            scrub.ApplyTemplate(); //apply template so that the thumb can be acessed as its own object
            Thumb thumb = (scrub.Template.FindName("PART_Track", scrub) as Track).Thumb;
            thumb.MouseEnter += new MouseEventHandler(thumb_MouseEnter);//bind thumb action to scrub bar

            //timer in order to write to json
            DispatcherTimer udjson = new DispatcherTimer();
            udjson.Interval = TimeSpan.FromSeconds(1);
            udjson.Tick += Udjson_Tick;
            udjson.Start();
        }
        
        private void phpShit() {


            WebRequest request = WebRequest.Create("http://localhost/response.php");
            request.Method = "POST";

            //data to encode.

            string postData = "Data="+JsonConvert.SerializeObject(m);
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            //this is up to you as to how you want to encode this. there are a ton of encoding methods. 
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.  
            string responseFromServer = reader.ReadToEnd();
            // Display the content.  
            Console.WriteLine(responseFromServer);
            // Clean up the streams.  
            reader.Close();
            dataStream.Close();
            response.Close();

            Console.ReadLine();
        }

        private void updateList() { //updates the full metadata for server
            movies.RemoveAll(movie => movie.name.Equals(m.name));
            movies.Add(m);
            writeFile(@"/html/metadatafull.json", movies);

        }

        private void Udjson_Tick(object sender, EventArgs e) { // sets the resume time of the video
            m.resumetime = mePlayer.Position;
            writeFile(@"/html/metadata.json", m);
            updateList();
            phpShit();
        }

        private void writeFile(string path, object t) { // writes to the document server root
            using (StreamWriter file = File.CreateText(path)) {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, t);
            }
        }

        private Movie parseJSON() { //parses the metadata json from apache server

            WebClient client = new WebClient();
            Stream stream = client.OpenRead("http://localhost/metadata.json");
            StreamReader reader = new StreamReader(stream);
            string json = reader.ReadToEnd();
            JObject j = (JObject)JsonConvert.DeserializeObject(json);
            JObject o = JObject.Parse(json); //read in and parse json

            m.id = new Uri(((string)j.GetValue("id"))); //set parameters to watch the movie with
            m.resumetime = (TimeSpan)j.GetValue("resumetime");
            m.thumbnail = ((string)j.GetValue("thumbnail"));
            m.description = ((string)j.GetValue("description"));
            m.name = ((string)j.GetValue("name"));

            return m;
        }

        private void loadMedia() {
            parseJSON();    //collects metadata
            if (m.id != null) { //makes sure fail isnt a crash
                mePlayer.Source = m.id;
                mePlayer.Position = TimeSpan.FromSeconds((int)m.resumetime.TotalSeconds); //gives off stopped time
            }
            mePlayer.Play();//auto play the video
        }

        private void muteToggle() { //toggles mute control
            if (mePlayer.Volume != 0) {
                volHist = mePlayer.Volume;  //save the last volume setting for reaccess
                mePlayer.Volume = 0;
            } else {
                mePlayer.Volume = volHist; //restore from mute to previous volume
            }
        }

        private void pauseToggle() {//toggles pause control
            if (!isPlaying) {
                mePlayer.Play();
                PlayImg.Source = new BitmapImage(new Uri("Resources/pause.png", UriKind.Relative)); //toggles pause image
                isPlaying = true;
            } else {
                mePlayer.Pause();
                PlayImg.Source = new BitmapImage(new Uri("Resources/play.png", UriKind.Relative)); //toggles play image
                isPlaying = false;
            }
        }

        private void timer_Tick(object sender, EventArgs e) {
            //scrub the video bar and controll it based on the total time of the video being played
            if ((mePlayer.Source != null) && (mePlayer.NaturalDuration.HasTimeSpan) && (!isDragging)) {
                scrub.Minimum = 0; //set the minimum position
                scrub.Maximum = mePlayer.NaturalDuration.TimeSpan.TotalSeconds;
                scrub.Value = mePlayer.Position.TotalSeconds; //set it at the time value
                playTime.Text = String.Format("{0} / {1}", mePlayer.Position.ToString(@"mm\:ss"), mePlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
            }
        }

        private void scrub_DragStarted(object sender, EventArgs e) {
            isDragging = true;
        }

        private void scrub_DragCompleted(object sender, EventArgs e) {
            isDragging = false;
            mePlayer.Position = TimeSpan.FromSeconds(scrub.Value); //change time to scrubbed value 
        }

        private void thumb_MouseEnter(object sender, MouseEventArgs e) { //action listener for the track bar and scrubbing use click 
            if (e.LeftButton == MouseButtonState.Pressed && e.MouseDevice.Captured == null) {
                MouseButtonEventArgs args = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, MouseButton.Left);
                args.RoutedEvent = MouseLeftButtonDownEvent;
                (sender as Thumb).RaiseEvent(args);

            }
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e) {
            //play the video or pause based on the controller, will be replaced with icon
            pauseToggle();
        }

        private void keys(object sender, KeyEventArgs e) { //keydown logic for shortcuts

            if (e.Key == Key.Space) { //toggle pause on space press
                pauseToggle();
            }

            if (e.Key == Key.F) { //toggle fullscreen view on f press

                if (!fullScreen) {
                    this.WindowStyle = WindowStyle.None;
                    this.WindowState = WindowState.Maximized;
                } else {
                    this.WindowStyle = WindowStyle.SingleBorderWindow;
                    this.WindowState = WindowState.Normal;
                }

                fullScreen = !fullScreen;
            }

            if (fullScreen && e.Key == Key.Escape) { //exit fullscreen
                fullScreen = false;
                this.WindowState = WindowState.Normal;
            }

            //volume and scrubbing conroll with wasd keys
            if (e.Key == Key.W && mePlayer.Volume < 1) mePlayer.Volume += .1;
            if (e.Key == Key.S && mePlayer.Volume > 0) mePlayer.Volume -= .1;
            if (e.Key == Key.A) mePlayer.Position -= TimeSpan.FromSeconds(5);
            if (e.Key == Key.D) mePlayer.Position += TimeSpan.FromSeconds(5);

            if (e.Key == Key.M) { //mute on m press
                muteToggle();
            }
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e) {//allow for volume scrolling with mouse wheel
            mePlayer.Volume += (e.Delta > 0) ? 0.1 : -0.1;
        }

        private void hideCont(object sender, MouseEventArgs e) { //hides the controller bar on mouse leave
            hide.Interval = TimeSpan.FromSeconds(2); //give 2 seconds till leave
            hide.Tick += hideTick;
            hide.Start();
        }

        private void hideTick(object sender, EventArgs e) { //hide the bar and stop the timer untill reactivated
            contP.Opacity = 0;
            hide.Stop();
        }

        private void showCont(object sender, MouseEventArgs e) { //shows the controller bar on mouse enter
            contP.Opacity = 100;
        }

        private void btnMute_Click(object sender, RoutedEventArgs e) { //event handler for mute button
            muteToggle();
        }

        private void btnFF_Click(object sender, RoutedEventArgs e) { //event handler for fast forward
            mePlayer.Position += TimeSpan.FromSeconds(5);
        }

        private void btnRW_Click(object sender, RoutedEventArgs e) { //event handler for rewind
            mePlayer.Position -= TimeSpan.FromSeconds(5);
        }
    }
}
