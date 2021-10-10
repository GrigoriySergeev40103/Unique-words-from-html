using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueWordsFromHTML
{
    interface IStatisticsSaver
    {
        /// <summary>
        /// Сохраняет статистику
        /// </summary>
        /// <param name="statistics"></param>
        /// <returns> True - если удалось сохранить.
        ///           False - если не удалось</returns>
        public bool SaveStatistics(Dictionary<string, int> statistics);

    }
}
