using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;


namespace mov {

    public partial class MainWindow : Window {

        //mini class to store metadata in a container sor safty
        [DataContract]
        internal class Movie {
            [DataMember]
            internal Uri movieid;
            [DataMember]
            internal int resumeTime;
        }

        //vars for cheking the scrubbing state, window state, and the play state
        bool isDragging = false;
        bool isPlaying = true;
        bool fullScreen = false;
        double volHist; //remembering the volume for mute toggle
        Movie m = new Movie(); //metadata for the movie

        private Movie parseJSON() { //parses the metadata json from apache server
            try {
                using (WebClient wc = new WebClient()) {
                    var json = wc.DownloadString("http://192.168.0.137/metadata.json"); //downloads the metadata file
                    JObject j = (JObject)JsonConvert.DeserializeObject(json); 
                    JObject o = JObject.Parse(json); //read in and parse json
                 
                    m.movieid = new Uri((string)j.GetValue("movieid")); //set parameters to watch the movie with
                    m.resumeTime = (int)j.GetValue("resumeTime");
                   
                }
            } catch {
                Console.WriteLine("Connection to apache server failed. Test media loaded");
            }
            return m;
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

        private void loadMedia() {
            parseJSON();    //collects metadata
            if (m.movieid != null) { //makes sure fail isnt a crash
                mePlayer.Source = m.movieid;
                mePlayer.Position = TimeSpan.FromSeconds(m.resumeTime); //gives off stopped time
            }
            mePlayer.Play();//auto play the video
        }

        public MainWindow() {
            InitializeComponent();
      
            loadMedia();//loads media to play
                                  
            DispatcherTimer timer = new DispatcherTimer(); //dispatch timer in order to update the scrubbing
            timer.Interval = TimeSpan.FromSeconds(1); //update the scrub bar every second and tick the timer
            timer.Tick += timer_Tick;
            timer.Start();
            scrub.ApplyTemplate(); //apply template so that the thumb can be acessed as its own object
            Thumb thumb = (scrub.Template.FindName("PART_Track", scrub) as Track).Thumb;
            thumb.MouseEnter += new MouseEventHandler(thumb_MouseEnter);//bind thumb action to scrub bar

        }

        private void timer_Tick(object sender, EventArgs e) {
            //scrub the video bar and controll it based on the total time of the video being played
            if ((mePlayer.Source != null) && (mePlayer.NaturalDuration.HasTimeSpan) && (!isDragging)) {
                scrub.Minimum = 0; //set the minimum position
                scrub.Maximum = mePlayer.NaturalDuration.TimeSpan.TotalSeconds;
                scrub.Value = mePlayer.Position.TotalSeconds; //set it at the time value
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

        private void keys(object sender, KeyEventArgs e){ //keydown logic for shortcuts
       
            if (e.Key == Key.Space){ //toggle pause on space press
                pauseToggle();
            }

            if (e.Key == Key.F){ //toggle fullscreen view on f press
            
                if (!fullScreen) {
                    this.WindowStyle = WindowStyle.None;
                    this.WindowState = WindowState.Maximized;
                } else {
                    this.WindowStyle = WindowStyle.SingleBorderWindow;
                    this.WindowState = WindowState.Normal;
                }

                fullScreen = !fullScreen;
            }

            if (fullScreen && e.Key == Key.Escape){ //exit fullscreen
                fullScreen = false;
                this.WindowState = WindowState.Normal;
            }

            //volume and scrubbing conroll with wasd keys
            if (e.Key == Key.W && mePlayer.Volume < 1) mePlayer.Volume += .1;
            if (e.Key == Key.S && mePlayer.Volume > 0) mePlayer.Volume -= .1;
            if (e.Key == Key.A) mePlayer.Position -= TimeSpan.FromSeconds(5);
            if (e.Key == Key.D) mePlayer.Position += TimeSpan.FromSeconds(5);

            if (e.Key == Key.M){ //mute on m press
                muteToggle();
            }
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e){//allow for volume scrolling with mouse wheel
            mePlayer.Volume += (e.Delta > 0) ? 0.1 : -0.1;
        }

        private void hideCont(object sender, MouseEventArgs e){ //hits the controller bar on mouse leave
            contP.Opacity = 0;
        }

        private void showCont(object sender, MouseEventArgs e){ //shows the controller bar on mouse enter
            contP.Opacity = 100;
        }

        private void btnMute_Click(object sender, RoutedEventArgs e){ //event handler for mute button
            muteToggle();
        }

        private void btnFF_Click(object sender, RoutedEventArgs e){ //event handler for fast forward
            mePlayer.Position += TimeSpan.FromSeconds(5);
        }

        private void btnRW_Click(object sender, RoutedEventArgs e){ //event handler for rewind
            mePlayer.Position -= TimeSpan.FromSeconds(5);
        }
    }
}
