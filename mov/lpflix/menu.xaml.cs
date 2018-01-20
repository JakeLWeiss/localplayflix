
using Binding;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace lpflix {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        Movie m;
        List<Movie> movs = new Movies();

        public MainWindow() {
            InitializeComponent();            
        }

        private void writeFile(string path, object t) {
            
            using (StreamWriter file = File.CreateText(path)) {
                JsonSerializer serializer = new JsonSerializer();
                //serialize object directly into file stream
                serializer.Serialize(file, t);
            }
        }

        private void dg_KeyDown(object sender, KeyEventArgs e) {

            m =(Movie) dg.SelectedItem;
            if (e.Key == Key.Space) { //toggle pause on space press
                writeFile(@"/html/metadata.json",m);
                loadScreen();
               
            }

        }

        private void dg_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
            m = (Movie)dg.SelectedItem;
            writeFile(@"/html/metadata.json",m);
            
        }

        private void play_Click(object sender, RoutedEventArgs e) { 
            m = (Movie)dg.SelectedItem;
            loadScreen();
        }

        private void loadScreen() {
            player p = new player();
            p.Show();
        }
    }


}




    
    