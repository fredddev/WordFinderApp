using System;
using System.Collections.Generic;

namespace WordFinderApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> matrix = new List<string>
            {
                "TREATJHTGO",
                "RSRCUVIXZK",
                "EIEPFFUSJB",
                "ASMARTKXNU",
                "TKHOJEPYYF",
                "OWOZKSMAGT",
                "FPNNIYPFHX",
                "BEYDXHYROM",
                "DGHOSTOZSJ",
                "JOGIGFJUTJ"
            };
            var wordstream = new List<string>
            {
                "TREAT",
                "SMART",
                "FABLE",
                "CRAVE",
                "GHOST"
            };

            var finder = new WordFinder(matrix);
            var result = finder.Find(wordstream);

            Console.WriteLine("Results:");
            foreach (var word in result)
            {
                Console.WriteLine(word);
            }
        }
    }
}
