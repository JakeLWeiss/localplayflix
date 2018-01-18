using Binding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;

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
            Movie m= (Movie)e.ClickedItem;
           writeLocalAsync(m);
            ResultTextBlock.Text = "You Selected--->>" + m.name + " with URI " + m.id;
        }


        private async void writeLocalAsync(Movie m) {

            /*
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter("e:\\gitprojects\\metadata.txt")) {
                
                file.WriteLine("{ \"movieid\": \"" + m.id + "\" , \"resumetime\": 0  }");
            }
            */
            Windows.Storage.StorageFolder storageFolder = await KnownFolders.DocumentsLibrary.CreateFolderAsync("metadata");
            Windows.Storage.StorageFile sampleFile = await storageFolder.CreateFileAsync("metadata.json", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            await Windows.Storage.FileIO.WriteTextAsync(sampleFile, "{ \"movieid\": \"" + m.id + "\" , \"resumetime\": 0  }");
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
                using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient()) {
                    using (System.Net.Http.HttpResponseMessage response = client.GetAsync("http://localhost/metadatafull.json").Result) {
                        using (HttpContent content = response.Content) {
                            var json = content.ReadAsStringAsync().Result;
                            List<JObject> jobs = JsonConvert.DeserializeObject<List<JObject>>(json);
                            foreach (JObject j in jobs) {
                                movies.Add(new Movie {
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
