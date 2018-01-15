using Binding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace menu {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private List<Movie> movies;

        public MainPage() {
            this.InitializeComponent();
            movies = MoviesManager.GetMovies();
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e) {
            var movie= (Movie)e.ClickedItem;
            ResultTextBlock.Text = "You Selected--->>" + movie.name + " with URI " + movie.id;
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
}

    public class MoviesManager {

        public static List<Movie> GetMovies() {
            var movies = new List<Movie>();
            
            List<Movie> movs = new List<Movie>();
            try {
                using (HttpClient client = new HttpClient()) {
                    using (HttpResponseMessage response = client.GetAsync("http://192.168.0.137/metadatafull.json").Result) {
                        using (HttpContent content = response.Content) {
                            var json = content.ReadAsStringAsync().Result;
                            List<JObject> jobs = JsonConvert.DeserializeObject<List<JObject>>(json);
                            foreach (JObject j in jobs) { 
                                movies.Add(new Movie { 
                                    id = new Uri((string)j.GetValue("movieid")), //set parameters to watch the movie with
                                    name = (string)j.GetValue("description"),
                                    description = (string)j.GetValue("description"),
                                    thumbnail = (string)j.GetValue("thumbnail"),
                                });
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
