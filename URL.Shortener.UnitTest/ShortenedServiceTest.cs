using Moq;
using URL.Shortener.Common;
using URL.Shortener.Interface;
using URL.Shortener.Model.ViewModels;

namespace URL.Shortener.UnitTest
{
    [TestFixture]
    public class ShortenedServiceTest
    {
        private IQueryable<ShortenedUrlView> shortenedUrls;

        [SetUp]
        public void Setup()
        {
            shortenedUrls = new List<ShortenedUrlView>() {
                new() { LongUrl = "https://LongUrl1.com.au" },
                new() { LongUrl = "https://LongUrl2.com.au" },
                new() { LongUrl = "https://LongUrl3.com.au" }
            }.AsQueryable();
        }

        [Test]
        public async Task GetAllAsync_Success()
        {
            // Arrange
            var mockService = new Mock<IShortenedUrlService>();
            mockService.Setup(x => x.GetAllAsync()).Returns(async () =>
            {
                await Task.Yield();
                return shortenedUrls;
            });

            // Act
            var actual = await mockService.Object.GetAllAsync();

            // Assert
            Equals(shortenedUrls.Count(), actual.Count());
        }

        [Test]
        public async Task GetAsync_Success()
        {
            // Arrange
            var url = shortenedUrls.FirstOrDefault();

            var mockService = new Mock<IShortenedUrlService>();
            mockService.Setup(x => x.GetAsync(url.Code)).Returns(async () => {
                await Task.Yield();
                return url;
            });

            // Act
            var entity = new ShortenedUrlView { Code = url.Code, LongUrl = url.LongUrl };
            await mockService.Object.CreateAsync(entity.LongUrl);
            var actual = await mockService.Object.GetAsync(url.Code);

            // Assert
            Equals(url, actual);
        }

        [Test]
        public async Task GetAsync_NotFound_Success()
        {
            // Arrange
            var code = "gHLLDKxB";
            var mockService = new Mock<IShortenedUrlService>();

            mockService.Setup(x => x.GetAsync(code)).Returns(async () => {
                await Task.Yield();
                return null;
            });

            // Act
            var actual = await mockService.Object.GetAsync(code);

            // Assert
            mockService.Verify(m => m.GetAsync(code), Times.AtLeastOnce());
            Equals(null, actual);
        }

        [Test]
        public void SaveAsync_IsNull_Failure_Throws()
        {
            string errorMessage = "LongUrl cannot be empty";

            // Arrange
            var url = It.IsAny<string>();

            // Act and Assert
            Assert.That(async () =>
                await SaveAsync_ThrowException(url, errorMessage),
                Throws.Exception.TypeOf<ServiceException>().And.Message.EqualTo(errorMessage));
        }

        private static async Task SaveAsync_ThrowException(string url, string errorMessage)
        {
            var mockService = new Mock<IShortenedUrlService>();
            await mockService.Object.CreateAsync(url).ConfigureAwait(false);
            throw new ServiceException(errorMessage);
        }
    }
}