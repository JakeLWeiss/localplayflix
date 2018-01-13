using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;


namespace mov {

    public partial class MainWindow : Window {
        
        //vars for cheking the scrubbing state, window state, and the play state
        bool isDragging = false;
        bool isPlaying = true;
        bool fullScreen = false;
        double volHist; //remembering the volume for mute toggle

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
                PlayImg.Source = new BitmapImage(new Uri("Resources/pause.png", UriKind.Relative));
                isPlaying = true;
            } else {
                mePlayer.Pause();
                PlayImg.Source = new BitmapImage(new Uri("Resources/play.png", UriKind.Relative));
                isPlaying = false;
            }
        }

        public MainWindow() {
            InitializeComponent();
            
            mePlayer.Play();//auto play the video
            DispatcherTimer timer = new DispatcherTimer(); //dispatch timer in order to update the scrubbing
            timer.Interval = TimeSpan.FromSeconds(1); //update the scrub bar every second and tick the timer
            timer.Tick += timer_Tick;
            timer.Start();

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

            //volume and scrubbing conroll with arrow keys
            if (e.Key == Key.Up && mePlayer.Volume < 1) mePlayer.Volume += .1;
            if (e.Key == Key.Down && mePlayer.Volume > 0) mePlayer.Volume -= .1;
            if (e.Key == Key.Left) mePlayer.Position -= TimeSpan.FromSeconds(5);
            if (e.Key == Key.Right) mePlayer.Position += TimeSpan.FromSeconds(5);

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
