using lpflix;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace lpflix {
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
        public TimeSpan resumetime {
            get;
            set;
        }
    }
}

namespace Binding { 
    public class Movies : List<Movie> {
        public Movies() {

            try {
                using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient()) {
                    using (System.Net.Http.HttpResponseMessage response = client.GetAsync("http://65.24.246.246/json/metadatafull.json").Result) {
                        using (HttpContent content = response.Content) {
                            var json = content.ReadAsStringAsync().Result;
                            List<JObject> jobs = JsonConvert.DeserializeObject<List<JObject>>(json);
                            foreach (JObject j in jobs) {
                                this.Add(new Movie {
                                    id = ((string)j.GetValue("id")), //set parameters to watch the movie with
                                    name = (string)j.GetValue("name"),
                                    description = (string)j.GetValue("description"),
                                    thumbnail = (string)j.GetValue("thumbnail"),
                                    resumetime = (TimeSpan)j.GetValue("resumetime")
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
