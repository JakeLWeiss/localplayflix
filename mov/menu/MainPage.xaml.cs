using Binding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace menu
{
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
            ResultTextBlock.Text = "You Selected--->>" + movie.name + "--->>description_ " + movie.description;
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
        public string id {
            get;
            set;
        }
        public string thumbnail {
            get;
            set;
        }
    }
    public class MoviesManager {
        public static List<Movie> GetMovies() {
            var movies = new List<Movie>();
            movies.Add(new Movie {
                name = "1", description = "was actually gonna use dustins fb profile but meh", thumbnail = "https://vignette.wikia.nocookie.net/inazuma-eleven/images/4/48/Tenma_and_Shuu_in_GO_Movie_HQ.PNG/revision/latest?cb=20160715140309"
            });
            movies.Add(new Movie {
                name = "2", description = "description of movie", thumbnail = "https://vignette.wikia.nocookie.net/inazuma-eleven/images/4/48/Tenma_and_Shuu_in_GO_Movie_HQ.PNG/revision/latest?cb=20160715140309"
            });
            movies.Add(new Movie {
                name = "3", description = "description of movie", thumbnail = "https://vignette.wikia.nocookie.net/inazuma-eleven/images/4/48/Tenma_and_Shuu_in_GO_Movie_HQ.PNG/revision/latest?cb=20160715140309"
            });
            movies.Add(new Movie {
                name = "4", description = "description of movie", thumbnail = "https://vignette.wikia.nocookie.net/inazuma-eleven/images/4/48/Tenma_and_Shuu_in_GO_Movie_HQ.PNG/revision/latest?cb=20160715140309"
            });
            movies.Add(new Movie {
                name = "5", description = "description of movie", thumbnail = "https://vignette.wikia.nocookie.net/inazuma-eleven/images/4/48/Tenma_and_Shuu_in_GO_Movie_HQ.PNG/revision/latest?cb=20160715140309"
            });
            return movies;
        }
    }
}