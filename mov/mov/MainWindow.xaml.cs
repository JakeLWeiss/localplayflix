using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;


namespace mov
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        bool isDragging = false;
        bool isPlaying = true;
        bool fullScreen = false;
        
        

        public MainWindow()
        {
            InitializeComponent();
            mePlayer.Play();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if ((mePlayer.Source != null) && (mePlayer.NaturalDuration.HasTimeSpan) && (!isDragging))
            {
                scrub.Minimum = 0;
                scrub.Maximum = mePlayer.NaturalDuration.TimeSpan.TotalSeconds;
                scrub.Value = mePlayer.Position.TotalSeconds;
            }
        }

        private void scrub_DragStarted(object sender, EventArgs e)
        {
            isDragging = true;
        }


        private void scrub_DragCompleted(object sender, EventArgs e)
        {
            isDragging = false;
            mePlayer.Position = TimeSpan.FromSeconds(scrub.Value);
        }


        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (!isPlaying)
            {
                mePlayer.Play();
                btnPlay.Content = "||";
                isPlaying = true;
            }
            else
            {
                mePlayer.Pause();
                btnPlay.Content = ">";
                isPlaying = false;
            }
        }

        private void btnRW_Click(object sender, RoutedEventArgs e)
        {
            mePlayer.Position -= TimeSpan.FromSeconds(10);
        }

        private void btnFF_Click(object sender, RoutedEventArgs e)
        {
            mePlayer.Position += TimeSpan.FromSeconds(10);
        }

        private void keys(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Space)
            {
                if (isPlaying)
                {
                    mePlayer.Pause();
                }
                else
                {
                    mePlayer.Play();
                }
                isPlaying = !isPlaying;
            }

            if (e.Key == Key.F)
            {
                if (!fullScreen)
                {
                    this.WindowStyle = WindowStyle.None;
                    this.WindowState = WindowState.Maximized;
                }
                else
                {
                    this.WindowStyle = WindowStyle.SingleBorderWindow;
                    this.WindowState = WindowState.Normal;
                }

                fullScreen = !fullScreen;
            }

            if(fullScreen && e.Key == Key.Escape)
            {
                fullScreen = false;
                this.WindowState = WindowState.Normal;
            }
        }


        private void hidCont(object sender, MouseEventArgs e)
        {

            contP.Opacity = 0;
            scrub.Opacity = 0;

        }

        private void showCont(object sender, MouseEventArgs e)
        {

            
            scrub.Opacity = 100;
            contP.Opacity = 100;
        }
    }
}
