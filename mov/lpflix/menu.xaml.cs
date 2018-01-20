using Binding;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace lpflix {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        Movie m;

        public MainWindow() {

            InitializeComponent();

            List < Movie > movies = new Movies();
            TextBlock t = new TextBlock();
            
        }

        private void writeFile() {
            string path = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);


            using (StreamWriter file = File.CreateText(path + "/../../Resources/metadata.json")) {
                JsonSerializer serializer = new JsonSerializer();
                //serialize object directly into file stream
                serializer.Serialize(file, m);
            }


            try {
                //create WebClient object
                WebClient client = new WebClient();

                client.Credentials = CredentialCache.DefaultCredentials;

                String s = JsonConvert.SerializeObject(m);
                Console.WriteLine(s);

                // new WebClient().UploadFile("http://localhost/metadata2.json", "POST", @"c:/lpflix/metadata2.json");
                //client.Dispose();



            } catch (Exception err) {
                MessageBox.Show(err.Message);
            }
        }

        private void dg_KeyDown(object sender, KeyEventArgs e) {

            m =(Movie) dg.SelectedItem;
            if (e.Key == Key.Space) { //toggle pause on space press
                                      //TODO add file write

                writeFile();
                loadScreen();
               
            }

        }

        private void dg_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
            m = (Movie)dg.SelectedItem;
            writeFile();
            Console.WriteLine("Selected: "+m.name);
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



namespace Binding {
    public class Movie {

        public string name {
            get;
            set;
        }
        public string description {
            get;
            set;
        }
        public Uri id {
            get;
            set;
        }
        public string thumbnail {
            get;
            set;
        }
        public int resumetime {
            get;
            set;
        }

    }

    public class Movies : List<Movie> {
        public Movies() {

            try {
                using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient()) {
                    using (System.Net.Http.HttpResponseMessage response = client.GetAsync("http://localhost/metadatafull.json").Result) {
                        using (HttpContent content = response.Content) {
                            var json = content.ReadAsStringAsync().Result;
                            List<JObject> jobs = JsonConvert.DeserializeObject<List<JObject>>(json);
                            foreach (JObject j in jobs) {
                                this.Add(new Movie {
                                    id = new Uri((string)j.GetValue("id")), //set parameters to watch the movie with
                                    name = (string)j.GetValue("description"),
                                    description = (string)j.GetValue("description"),
                                    thumbnail = (string)j.GetValue("thumbnail"),
                                    resumetime = (int)j.GetValue("resumetime")
                                });
                            }
                        }
                    }
                }
            } catch {
                Console.WriteLine("Connection to apache server failed. Test media loaded");
            }

        }
    }
}