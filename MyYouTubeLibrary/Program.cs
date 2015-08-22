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
    //Exceptin Class
    public class YouTubeDataServiceException:Exception
    {
        public YouTubeDataServiceException(String msg)
        {
            if (string.Equals(msg, "notValid", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("The argument is not valid. possible argument: VideoList");
            }
        }
    }

    //Data received from YouTube Web Service, parsed into data object
    public class YouTubeData
    {
        public YouTubeData()
        {
            
        }
    }


    public interface IyouTubeDataService
    {
         YouTubeData getYouTubeData(String DataType , String APIkey);
    }
    
   
    class VideoList : IyouTubeDataService
    {
        public YouTubeData data;
        public String URL;
        public String urlParameters;

        //create an object of SingleObject
        private static VideoList instance = new VideoList();

        //make the constructor private so that this class cannot be instantiated
        private VideoList() 
        {
            this.URL = "https://www.googleapis.com/youtube/v3/search";
        }

        //Get the only object availablea
        public static VideoList getInstance()
        {
            return instance;
        }

        YouTubeData IyouTubeDataService.getYouTubeData(String DataType, String APIkey)
        {
            if (!string.Equals(DataType, "VideoList", StringComparison.OrdinalIgnoreCase))
            {
                throw new YouTubeDataServiceException("notValid");
            }
            
            this.urlParameters = "?part=snippet&relatedToVideoId=06_ng5caZ78&type=video&key=" + APIkey;
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

            if (string.Equals(DataType, "VideoList", StringComparison.OrdinalIgnoreCase))
            {
                VideoList obj = VideoList.getInstance();
                return obj;
            }

            if (!string.Equals(DataType, "VideoList", StringComparison.OrdinalIgnoreCase))
            {
                throw new YouTubeDataServiceException("notValid");
            }

            return null;   
            
            
        }
    }

    
    class Program
    {
        static void Main(string[] args)
        {
             YouTubeDataServiceFactory factory = new YouTubeDataServiceFactory();
             IyouTubeDataService func1 = factory.YouTubeService("VideoList", "AIzaSyAs5EBJ96dISdxrgU1y5v93lYqIx-9Zbs0");
             func1.getYouTubeData("VideoList", "AIzaSyBGKoPpk2OE3Z_-cfCwF035g7ljgZe25wo");
             Console.ReadLine();
        }
    }
}






   

