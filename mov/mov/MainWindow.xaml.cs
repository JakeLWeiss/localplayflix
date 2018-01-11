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
        bool isPlaying = true;
        double currentPosition;
        DispatcherTimer timer;
        public MainWindow()
        {
            InitializeComponent();
            mePlayer.Play();
            
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

    }
}
