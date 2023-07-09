using MySampleApp.Core.Entities;
using static System.Net.Mime.MediaTypeNames;

namespace MySampleApp.Web.Models
{
    public class AnalyzeViewModel
    {
        public List<ImageExt> ImagesXP { get; set; }
        public int TotalWords { get; set; }
        public List<WordCount> TopWords { get; set; }
    }
}
