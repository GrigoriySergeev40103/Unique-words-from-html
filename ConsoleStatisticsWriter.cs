using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueWordsFromHTML
{
    class ConsoleStatisticsWriter : IStatisticsWriter
    {
        public void WriteStatistics(Dictionary<string, int> wordCountPairs)
        {
            foreach (var wordCountPair in wordCountPairs)
            {
                Console.WriteLine($"{wordCountPair.Key,-20} - {wordCountPair.Value}");
            }
        }
    }
}
