using LibraNovel.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LibraNovel.Infrastructure.Data.Context
{
    public class LibraNovelContextSeed
    {
        public static async Task SeedAsync(LibraNovelContext libraNovelContext, ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retryForAvailability = retry == null ? 0 : retry.Value;

            try
            {

                // NOTE : Only run this if using a real database
                libraNovelContext.Database.Migrate();
                libraNovelContext.Database.EnsureCreated();

                await SeedNovelsAsync(libraNovelContext);

            }
            catch (Exception exception)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<LibraNovelContext>();
                    log.LogError(exception.Message);
                    await SeedAsync(libraNovelContext, loggerFactory, retryForAvailability);
                }
                throw;
            }
        }

        private static async Task SeedNovelsAsync(LibraNovelContext libraNovelContext)
        {
            if (libraNovelContext.Novels.Any())
                return;

            var novels = new List<Novel>()
            {
                new Novel()
                {
                    Title = "Sự trở lại của thiên thần",
                    Description = "Cuốn tiểu thuyết này là phần thứ tư của loạt truyện \"Hắc Ma Pháp Sư\" (The Vampire Diaries) của tác giả." +
                                  "Cuốn sách kể về cuộc sống của Elena Gilbert, một cô gái trẻ đang sống ở thị trấn Fell's Church, Virginia. Sau khi trải qua nhiều cuộc phiêu lưu và gặp gỡ những thách thức, Elena phát hiện ra rằng cô có một số sức mạnh siêu nhiên và là một phần của một thế giới bí ẩn với ma cà rồng, phù thủy và những sinh vật siêu nhiên khác." +
                                  "Trong \"Sự Trở Lại Của Thiên Thần\", Elena và những người bạn của cô phải đối mặt với các thử thách mới và đối đầu với các thế lực tà ác từ thế giới bóng tối. Câu chuyện tập trung vào mối quan hệ phức tạp giữa các nhân vật, sự đấu tranh giữa ánh sáng và bóng tối, cũng như sức mạnh của tình yêu và lòng kiên nhẫn.",
                    TotalPages = 10,
                    PublishedDate = DateTime.Now,
                },

                new Novel()
                {
                    Title = "Cuối chân trời",
                    Description = "\"Cuối Chân Trời\" là một tiểu thuyết của nhà văn nổi tiếng Nicholas Sparks, xuất bản lần đầu vào năm 1998. Cuốn sách này kể về câu chuyện tình yêu đầy cảm động giữa hai nhân vật chính là Dawson Cole và Amanda Collier." +
                                  "Câu chuyện bắt đầu khi hai nhân vật chính gặp lại nhau sau một thời gian xa cách. Dawson và Amanda từng là bạn bè thân thiết ở tuổi trẻ, nhưng cuộc sống đã kéo họ xa nhau. Sự gặp lại này đánh thức những kí ức cũ và những cảm xúc dày vò từ quá khứ.",
                    TotalPages = 18,
                    PublishedDate = DateTime.Now,
                },

                new Novel()
                {
                    Title = "Luật của kẻ mạnh",
                    Description = "\"Luật của Kẻ Mạnh\" là một tiểu thuyết hành động và phiêu lưu của tác giả Wilbur Smith, được xuất bản lần đầu vào năm 1964. Cuốn sách này kể về câu chuyện ly kỳ và đầy hấp dẫn về cuộc chiến giữa các nhân vật mạnh mẽ trong một môi trường hoang dã và đầy thách thức." +
                                  "Cuốn sách mở đầu bằng việc giới thiệu các nhân vật chính, bao gồm Sean Courtney, một người đàn ông can đảm và thông minh, và Mark Anders, một lính thủy đánh bộ người Mỹ. Cả hai đều sống ở Nam Phi vào những năm 1860, thời kỳ của sự đổ bộ và mở cửa của người châu Âu vào lục địa châu Phi.",
                    TotalPages = 8,
                    PublishedDate = DateTime.Now,
                },
            };

            libraNovelContext.Novels.AddRange(novels);
            await libraNovelContext.SaveChangesAsync();
        }
    }
}
