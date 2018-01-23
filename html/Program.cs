using System;
using System.IO;
using System.Net;
using System.Text;

namespace Examples.System.Net
{
    public class WebRequestPostExample
    {
        public static void Main()
        {
			//just a little sample bit of code that, in conjunction with response.php, will do a complete post request and return decoded json data. 
            WebRequest request = WebRequest.Create("localhost/response.php");
            request.Method = "POST";
            
            //data to encode. at the moment, the encoding is in url encoding mode, and the string is a JSON encoded string (JSON.stringify in a browser)
            string postData = "thing={\"name\":\"Mirai Nikki Episode 2\",\"description\":\"Episode 2 of the Future Diary (Dubbed)\",\"id\":\"http://localhost/mnikki2.mp4\",\"thumbnail\":\"http://localhost/mnikkithumb.png\",\"resumetime\":357}";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            //this is up to you as to how you want to encode this. there are a ton of encoding methods. 
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.  
            string responseFromServer = reader.ReadToEnd();
            // Display the content.  
            Console.WriteLine(responseFromServer);
            // Clean up the streams.  
            reader.Close();
            dataStream.Close();
            response.Close();

            Console.ReadLine();
        }
    }
}
