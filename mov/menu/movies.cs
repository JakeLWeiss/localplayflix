using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Binding {
    public class Movies {
        public int MoviesId {
            get;
            set;
        }
        public string Category {
            get;
            set;
        }
        public string Model {
            get;
            set;
        }
        public string Image {
            get;
            set;
        }
    }
    public class MoviesManager {
        public static List<Movies> GetMovies() {
            var movies = new List<Movies>();
            movies.Add(new Movies {
                Category = "Honda", Model = "2014", Image = "Assets/StoreLogo.png"
            });
            movies.Add(new Movies {
                Category = "City", Model = "2008", Image = "Assets/StoreLogo.png"
            });
            movies.Add(new Movies {
                Category = "Ferrari", Model = "2010", Image = "Assets/StoreLogo.png"
            });
            movies.Add(new Movies {
                Category = "Toyota", Model = "2011", Image = "Assets/StoreLogo.png"
            });
            movies.Add(new Movies {
                Category = "Mehran", Model = "2009", Image = "Assets/StoreLogo.png"
            });
            return movies;
        }
    }
}