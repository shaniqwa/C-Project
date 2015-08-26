using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyYouTubeLibrary;
using NUnit.Framework;
namespace MyYouTubeLibrary.Tests
{
    [TestFixture()]
    public class YouTubeDataServiceFactoryTests
    {
        [Test()]
        public void YouTubeServiceTest()
        {
            // arrange
            YouTubeDataServiceFactory factory = new YouTubeDataServiceFactory();
            try
            {
                // act
                IyouTubeDataService func1 = factory.YouTubeService("VideofdfdsList", "_bdOTUocn5w", "AIzaSyAs5EBJ96dISdxrgU1y5v93lYqIx-9Zbs0");
                YouTubeData data = func1.getYouTubeData("VideodsdsList", "_bdOTUocn5w", "AIzaSyBGKoPpk2OE3Z_-cfCwF035g7ljgZe25wo");
            }
           catch (YouTubeDataServiceException e)
            {
                // assert
                StringAssert.Contains(e.Message, YouTubeDataServiceFactory.argumetNotVaildMsg);
                return;
            }
            Assert.Fail("No exception was thrown.");
        }   
    }
}
