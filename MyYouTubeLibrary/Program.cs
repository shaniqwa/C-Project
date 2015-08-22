using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;



namespace MyYouTubeLibrary
{
    public class DataObject
    {
        public string Name { get; set; }
    }
    public class YouTubeDataServiceException:Exception
    {

    }
    public class YouTubeData
    {
        List<string> video;

        public YouTubeData()
        {
            
        }


    }
    public interface IyouTubeDataService
    {
         YouTubeData getYouTubeData(String DataType , String APIkey);
    }
    
   
    class RetriveMyUploads : IyouTubeDataService
    {
        public YouTubeData data;
        public String URL;
        public String urlParameters;
        //create an object of SingleObject
        private static RetriveMyUploads instance = new RetriveMyUploads();

        //make the constructor private so that this class cannot be instantiated
        private RetriveMyUploads() 
        {
            this.URL = "https://www.googleapis.com/youtube/v3/search";
        }

        //Get the only object availablea
        public static RetriveMyUploads getInstance()
        {
            return instance;
        }

        YouTubeData IyouTubeDataService.getYouTubeData(String DataType, String APIkey)
        {


            this.urlParameters = "?part=snippet&relatedToVideoId=ONqBDz_3wBQ&type=video&key=" + APIkey;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;  // Blocking call!
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(response);
                
                // Parse the response body. Blocking!
                try
                {
                    data = response.Content.ReadAsAsync<YouTubeData>().Result;
                    Console.WriteLine(data.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }  
            return data;
        }
    }

    public class YouTubeDataServiceFactory
    {
        //use getYouTubeData method to get object that implements IyouTubeDataService interface
        public IyouTubeDataService YouTubeService(String DataType, String APIkey)
        {
            if (DataType == null)
            {
                return null;
            }
            if (string.Equals(DataType, "MyUploads", StringComparison.OrdinalIgnoreCase))
            {
                RetriveMyUploads obj = RetriveMyUploads.getInstance();
                return obj;

            }

            return null;
        }
    }

    
    class Program
    {
        static void Main(string[] args)
        {
             YouTubeDataServiceFactory factory = new YouTubeDataServiceFactory();
             IyouTubeDataService func1 = factory.YouTubeService("MyUploads", "AIzaSyAs5EBJ96dISdxrgU1y5v93lYqIx-9Zbs0");
             func1.getYouTubeData("MyUploads", "AIzaSyBGKoPpk2OE3Z_-cfCwF035g7ljgZe25wo");
             Console.ReadLine();
        }
    }
}






   

