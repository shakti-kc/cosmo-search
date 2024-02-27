using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
namespace Cosmos{
public class Cosmos{

public class SearchData
{
    public string Word { get; set; }
    public string Title { get; set; }
    public string Url { get; set; }
}

public class SearchEngine
{
    private List<SearchData> _searchContent;

    public SearchEngine(string filePath)
    {
        _searchContent = new List<SearchData>();
        ParseDataStructure(filePath);
    }

    private void ParseDataStructure(string path)
    {
        string page = null;
        string title = null;

        foreach (var line in File.ReadLines(path))
        {
            SearchData searchData = new SearchData();

            if (line.StartsWith("*PAGE:"))
            {
                page = line.Substring(6);
                title = null;
            }
            else
            {
                if (title == null)
                {
                    title = line.Trim();
                }
            }

            if (page != null && title != null)
            {
                searchData.Url = page;
                searchData.Title = title;
                searchData.Word = line.Trim();

                _searchContent.Add(searchData);
            }
        }
    }

      public List<SearchData> FindWord(string word)
    {
       Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();
        var results = _searchContent.Where(sd => string.Equals(sd.Word, word, StringComparison.CurrentCultureIgnoreCase)).ToList();
        stopWatch.Stop();
        TimeSpan ts = stopWatch.Elapsed;
        var elapsedSeconds = stopWatch.Elapsed;;
        Console.WriteLine($"Found {results.Count} results in {ts.ToString("ss\\.fffffff")} seconds.");
        return results;   
    }
}

static void Main(string[] args)
{
    string filePath = "./data.txt"; // Replace with your data file path

    try
    {
        var searchEngine = new SearchEngine(filePath);

        while (true)
        {
            Console.WriteLine("Enter a word to search (or 'exit' to quit): ");
            var word = Console.ReadLine();

            if (word.ToLower() == "exit")
            {
                break;
            }

            var results = searchEngine.FindWord(word);

            if (!results.Any())
            {
                Console.WriteLine("No results found.");
            }
            else
            {
                Console.WriteLine($"Found {results.Count} results:");
                // foreach (var result in results)
                // {
                //     Console.WriteLine($"- {result.Title} ({result.Url})");
                // }
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error: " + ex.Message);
    }
}
}
}
