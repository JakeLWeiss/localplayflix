using Binding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace menu1 {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        public List<Movie> movies;

        public MainWindow() {

            movies = new Movies();
            foreach (Movie m in movies) Console.WriteLine(m.name);
            
            InitializeComponent();

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

    }

    public class Movies : List<Movie> {
        public Movies() {
            this.Add(new Movie() {
                id = (new Uri("http://localhost/mnikki2.mp4")),
                name = "mnikki2.mp4",
                thumbnail = "/Resources/Play.png",
                description = " blah"
            });
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
                                });
                                Console.WriteLine((string)j.GetValue("description"));
                            }
                        }
                    }
                }
            } catch {
                Console.WriteLine("Connection to apache server failed. Test media loaded");
            }
            
        }
    }

    public class MoviesManager : List<Movie> {

        
        public static List<Movie> GetMovies() {
            var movies = new List<Movie>();

            List<Movie> movs = new List<Movie>();
            movs.Add(new Movie() {
                id = (new Uri("http://localhost/mnikki2.mp4")),
                name = "mnikki2.mp4",
                thumbnail = "/Resources/Play.png",
                description = " blah"
            });
            try {
                using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient()) {
                    using (System.Net.Http.HttpResponseMessage response = client.GetAsync("http://localhost/metadatafull.json").Result) {
                        using (HttpContent content = response.Content) {
                            var json = content.ReadAsStringAsync().Result;
                            List<JObject> jobs = JsonConvert.DeserializeObject<List<JObject>>(json);
                            foreach (JObject j in jobs) {
                                movs.Add(new Movie {
                                    id = new Uri((string)j.GetValue("id")), //set parameters to watch the movie with
                                    name = (string)j.GetValue("description"),
                                    description = (string)j.GetValue("description"),
                                    thumbnail = (string)j.GetValue("thumbnail"),
                                });
                                Console.WriteLine((string)j.GetValue("description"));
                            }
                        }
                    }
                }
            } catch {
                Console.WriteLine("Connection to apache server failed. Test media loaded");
            }
            return movies;
        }
    }
}