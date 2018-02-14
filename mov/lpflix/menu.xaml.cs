
using Binding;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace lpflix {
    
    public partial class MainWindow : Window {

        Movie m; //global movie selected
        List<Movie> movs = new Movies(); //full list of the movies from auto generated file

        public MainWindow() {
            InitializeComponent();            
        }

        private void writeFile(string path, object t) { //write metadata into json
            
            using (StreamWriter file = File.CreateText(path)) {
                JsonSerializer serializer = new JsonSerializer();
                //serialize object directly into file stream
                serializer.Serialize(file, t);
            }
        }

        private void dg_KeyDown(object sender, KeyEventArgs e) {//I just love keyboard shortcuts
            m =(Movie) dg.SelectedItem; //select movie
            if (e.Key == Key.Space) { //allow for spacebar to be used to play the video
                loadScreen();
            }
        }

        private void play_Click(object sender, RoutedEventArgs e) { //make use of the play button
            m = (Movie)dg.SelectedItem; //select and play the movie

            loadScreen();
        }

        private void loadScreen() { //play when selected
            writeFile(@"/html/metadata.json", m); //update to proper metadata
            player p = new player(); 
            p.Show();
        }

       
    }


}




    
    