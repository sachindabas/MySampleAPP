using MySampleApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MySampleApp.Core.Interfaces
{
    public interface IUrlAnalyzer
    {
        Task<List<ImageExt>> GetImagesFromUrl(string url);
        Task<WordCountResult> GetWordCount(string url);
    }
}