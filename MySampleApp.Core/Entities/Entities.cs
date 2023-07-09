namespace MySampleApp.Core.Entities
{
    public class ImageExt
    {
        public string Url { get; set; }
    }

    public class WordCount
    {
        public string Word { get; set; }
        public int Count { get; set; }
    }

    public class WordCountResult
    {
        public int TotalCount { get; set; }
        public List<WordCount> WordCounts { get; set; }
    }
}