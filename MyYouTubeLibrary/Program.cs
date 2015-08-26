using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;


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


    public class Item
    {
        string videoId { get; set; }
        string title { get; set; }
        string description { get; set; }

        public Item(string videoId) 
        {
            this.videoId = videoId;
        }
        public Item(string videoId, string title, string des)
        {
            this.videoId = videoId;
            this.title = title;
            this.description = des;
        }
        public override string ToString()
        {
            return "Title: " + title + "\nVideo ID: " + videoId + "\nDescription: " + description;
        }
    }
    //Data received from YouTube Web Service, parsed into data object
    public class YouTubeData
    {
        private List<Item> items;
        public YouTubeData() 
        {
            items = new List<Item>();
        }
        public void addItem(Item newItem)
        {
            items.Add(newItem);
        }
        public List<Item> Items
        {
           get
           {
               return items;
           }
           set
           {
               items = value;
           }
        }
    }


    public interface IyouTubeDataService
    {
         YouTubeData getYouTubeData(String DataType ,String videoID, String APIkey);
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
            this.data = new YouTubeData();
        }

        //Get the only object availablea
        public static VideoList getInstance()
        {
            return instance;
        }

        YouTubeData IyouTubeDataService.getYouTubeData(String DataType, String videoID ,String APIkey)
        {
            if (!string.Equals(DataType, "VideoList", StringComparison.OrdinalIgnoreCase))
            {
                throw new YouTubeDataServiceException("notValid");
            }
            
            //Set url query string + the APIkey of the user as parameter
            this.urlParameters = "?part=snippet&relatedToVideoId=" + videoID + "&type=video&key=" + APIkey;
            
            //Get json from youtube api
            var api = string.Format(URL + urlParameters);

            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString(api);
               
                //parse json
                JObject o = JObject.Parse(json);

                JArray items = (JArray)o["items"];
                int length = items.Count;

                for (int i = 0; i < items.Count; i++)
                {
                    var item = (JObject)items[i];
                    string title = (string)item["snippet"]["title"];
                    string id = (string)item["id"]["videoId"];
                    string des = (string)item["snippet"]["description"];
                    Item tmp = new Item(id, title,des);
                    data.addItem(tmp);
                }
            }
            return data;
        }
    }

    public class YouTubeDataServiceFactory
    {
        public const string argumetNotVaildMsg = "The argument is not valid. possible argument: VideoList";
        //use getYouTubeData method to get object that implements IyouTubeDataService interface
        public IyouTubeDataService YouTubeService(String DataType,String videoID ,String APIkey)
        {
            if (string.Equals(DataType, "VideoList", StringComparison.OrdinalIgnoreCase))
            {
                VideoList obj = VideoList.getInstance();
                return obj;
            }
            else
            {
                throw new YouTubeDataServiceException("notValid");
            }
        }
    }

    
    class Program
    {
        static void Main(string[] args)
        {
    
        }
    }
}






   

