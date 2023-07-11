using MySampleApp.Core.Entities;
using MySampleApp.Core.Interfaces;
using OpenQA.Selenium.Chrome;
using System.Text.RegularExpressions;

namespace MySampleApp.Infrastructure.Services
{
    public class UrlAnalyzerService : IUrlAnalyzer
    {

        ///<summary>
        /// We can also use some 3rd party library
        /// Need to explore further
        /// </summary>
        string[] stopWords = new string[] { "the", "of", "and", "a", "an", "in", "to", "for", "is", "it", "on", "with", "as", "at", "by","you","your" };
        public async Task<List<MySampleApp.Core.Entities.ImageExt>> GetImagesFromUrl(string url)
        {
            using (var httpClient = new HttpClient())
            {
                var pageContent = await httpClient.GetStringAsync(url);

                var imageUrls = Regex.Matches(pageContent, "<img.+?src=[\"'](.+?)[\"'].*?>")
                    .Select(match => match.Groups[1].Value)
                    .ToList();

                var images = imageUrls.Select(imageUrl => new ImageExt { Url = imageUrl }).ToList();
                return images;
            }
        }
        public async Task<WordCountResult> GetWordCount(string url)
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("--headless"); // Run Chrome in headless mode
                                                      // Provide the path to the ChromeDriver executable
            var chromeDriverPath = @"..\"; // Update with the actual path
            using (var driver = new ChromeDriver(chromeDriverPath, chromeOptions))
            {
                driver.Navigate().GoToUrl(url);
                await Task.Delay(2000); // Delay to allow JavaScript to execute (adjust as needed)

                var textContent = driver.FindElement(OpenQA.Selenium.By.TagName("body")).Text;

                var words = Regex.Matches(textContent, @"\b(?!(?:\d+\.?)+\b)\w+\b")
                    .Cast<Match>()
                    .Select(m => m.Value.ToLower().Trim())
                    .Where(word =>
                        !string.IsNullOrEmpty(word) &&
                        !stopWords.Contains(word) &&
                        !IsCssRelated(word) &&
                        !IsJavaScriptCode(word) &&
                        !IsNonContentText(word) &&
                        !stopWords.Contains(word)
                    )
                    .ToArray();

                var wordCount = words
                    .GroupBy(word => word)
                    .ToDictionary(group => group.Key, group => group.Count());

                var result = new WordCountResult
                {
                    TotalCount = wordCount.Sum(kv => kv.Value),
                    WordCounts = wordCount.OrderByDescending(kv => kv.Value)
                        .Select(kv => new WordCount { Word = kv.Key, Count = kv.Value })
                        .ToList()
                };

                return result;
            }
        }

        private bool IsCssRelated(string word)
        {
            return Regex.IsMatch(word, @"^(webkit|moz|ms|o)-[a-z]+(-[a-z]+)?$");
        }

        private bool IsJavaScriptCode(string word)
        {
            return word.Equals("function") || word.Equals("var") || word.Equals("const") || word.Equals("let");
        }

        private bool IsNonContentText(string word)
        {
            return word.Length <= 3;
        }
    }
}
