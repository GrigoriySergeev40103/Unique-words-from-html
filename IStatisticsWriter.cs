using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueWordsFromHTML
{
    interface IStatisticsWriter
    {

        public void WriteStatistics(Dictionary<string, int> wordCountPairs);
    }
}
