using MySampleApp.Core.Entities;
using MySampleApp.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
namespace MySampleApp.Infrastructure.Services
{
    public class UrlAnalyzerService : IUrlAnalyzer
    {

        ///<summary>
        /// We can also use some 3rd party library
        /// Need to explore further
        /// </summary>
        string[] stopWords = new string[] { "the", "of", "and", "a", "an", "in", "to", "for", "is", "it", "on", "with", "as", "at", "by" };
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
            var browser = new ScrapingBrowser();
            var page = await browser.NavigateToPageAsync(new Uri(url));

            var textNodes = page.Html
                .DescendantsAndSelf()
                .Where(node => node.NodeType == HtmlNodeType.Text && !string.IsNullOrWhiteSpace(node.InnerText) && node.ParentNode.Name != "script");

            var wordCount = new Dictionary<string, int>();
            foreach (var node in textNodes)
            {
                string[] words = Regex.Matches(node.InnerText, @"\b(?!(?:\d+\.?)+\b)\w+\b")
                    .Cast<Match>()
                    .Select(m => m.Value.ToLower().Trim())
                    .Where(word => !string.IsNullOrEmpty(word) && !stopWords.Contains(word) && !word.Equals("width"))
                    .ToArray();

                foreach (string word in words)
                {
                    if (wordCount.ContainsKey(word))
                        wordCount[word]++;
                    else
                        wordCount[word] = 1;
                }
            }

            var result = new WordCountResult
            {
                TotalCount = wordCount.Sum(kv => kv.Value),
                WordCounts = wordCount.OrderByDescending(kv => kv.Value).Select(kv => new WordCount { Word = kv.Key, Count = kv.Value }).ToList()
            };

            return result;
        }
    
    }
}
