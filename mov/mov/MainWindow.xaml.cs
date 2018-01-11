using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace mov
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        bool isDragging = false;
        bool isPlaying = false;
        DispatcherTimer timer;
        public MainWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(2000);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
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

        

     
        void timer_Tick(object sender, EventArgs e)
        {
            if (!isDragging) seekBar.Value = mePlayer.Position.TotalSeconds;
        }

        private void seekBar_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            isDragging = true;

        }

        private void seekBar_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            isDragging = false;
            mePlayer.Position = TimeSpan.FromSeconds(seekBar.Value);
        }
    }
}
