using RichardSzalay.MockHttp;
using SmartReader;
using Xunit;

namespace SmartReaderTests
{
    public class PerformanceTests
    {
        [Fact]
        public void TestGetArticleDoesNotHang1()
        {
            // setting up mocking HttpClient
            var mockHttp = new MockHttpMessageHandler();
            var sourceContent = File.ReadAllText(Path.Combine("..", "..", "..", "test-performance", @"testFile1.html"));
            mockHttp.When("https://localhost/article")
                .Respond("text/html", sourceContent);

            var reader = new Reader("https://localhost/article");

            reader.SetHttpClientHandler(mockHttp);
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Article article = reader.GetArticle();                       
            
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            Assert.True(article.Completed);
            Assert.True(elapsedMs < 10000);
        }

        [Fact]
        public void TestGetArticleDoesNotHang2()
        {
            // setting up mocking HttpClient
            var mockHttp = new MockHttpMessageHandler();
            var sourceContent = File.ReadAllText(Path.Combine("..", "..", "..", "test-performance", @"testFile2.html"));

            string cleanedHtml = sourceContent;
            mockHttp.When("https://localhost/article")
                .Respond("text/html", cleanedHtml);

            var reader = new Reader("https://localhost/article");
            reader.PreCleanPage = true;

            reader.SetHttpClientHandler(mockHttp);
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Article article = reader.GetArticle();                       
            
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            Assert.True(article.Completed);
            Assert.True(elapsedMs < 10000);
        }

        [Fact]
        public async Task TestGetArticleIsCancelled()
        {
            // setting up mocking HttpClient
            var mockHttp = new MockHttpMessageHandler();
            var sourceContent = File.ReadAllText(Path.Combine("..", "..", "..", "test-performance", @"testFile2.html"));

            string cleanedHtml = sourceContent;
            mockHttp.When("https://localhost/article")
                .Respond("text/html", cleanedHtml);

            var reader = new Reader("https://localhost/article");            

            reader.SetHttpClientHandler(mockHttp);
            var watch = System.Diagnostics.Stopwatch.StartNew();
            CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(8000));
            
            Article article = await reader.GetArticleAsync(cts.Token);

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            Assert.False(article.Completed);
            Assert.True(elapsedMs < 10000);
        }
    }
}
