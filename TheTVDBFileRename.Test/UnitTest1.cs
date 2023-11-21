using FluentAssertions;
using TheTVDBFileRename.Shared;

namespace TheTVDBFileRename.Test
{
    public class UnitTest1
    {
        [Fact]
        public async void TheTVDBFileRename_ReadHTML_ReturnDataTable()
        {
            try
            {
                //Arrange 

                var wc = new WebCalls();
                string[] urls =
                {
                    "https://thetvdb.com/series/dragon-ball-absalon/seasons/official/1"
                };


                //Act
                var dt = await wc.ReadHTML(urls);

                //Assert
                dt.Should().NotBeNull();
                //dt.Should().HaveRowCount(15);
            }
            catch (Exception)
            {

                throw;
            }
        }


        [Theory]
        [InlineData("https://thetvdb.com/series/dragon-ball-absalon/seasons/official/1", 15)]
        public async void TheTVDBFileRename_ReadHTML_ReturnInt(string url, int expected)
        {
            try
            {
                //Arrange 

                var wc = new WebCalls();
                string[] urls =
                {
                   url
                };


                //Act
                var dt = await wc.ReadHTML(urls);

                //Assert
                dt.Should().NotBeNull();
                dt.Should().HaveRowCount(expected);
            }
            catch (Exception)
            {

                throw;
            }
        }


        [Theory]
        [InlineData(10.5,4.5, 15)]
        [InlineData(10,4, 14)]
        public void TheTVDBFileRename_Add2Nos_ReturnInt(double a, double b, double expected)
        {
            try
            {
                //Arrange 

                var wc = new WebCalls();
               

                //Act
                double result = wc.Add2Nos(a,b);

                //Assert
                result.Should().Be(expected);
                
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}