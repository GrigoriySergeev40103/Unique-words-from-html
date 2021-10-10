using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Net;

namespace UniqueWordsFromHTML
{
    class HtmlFileHandler
    {
        /// <summary>
        /// Словарь содержащий статистику. Key - слово, Value - кол-во раз слово встретилось.
        /// </summary>
        private Dictionary<string, int> wordCountPairs = new Dictionary<string, int>();
        /// <summary>
        /// Путь до обрабатываемого файла
        /// </summary>
        private readonly string path;
        private IStatisticsSaver statsSaver;
        private IStatisticsWriter statsWriter;
        private IErrorLogCreator errorLogCreator;
        public IStatisticsSaver StatsSaver { get => statsSaver; set { statsSaver = value; } }
        public IStatisticsWriter StatsWriter { get => statsWriter; set { statsWriter = value; } }
        public IErrorLogCreator ErrorLogCreator { get => errorLogCreator; set { errorLogCreator = value; } }

        /// <summary>
        /// Конструктор, в случае если нет нужды в сохранении и выведении статистики
        /// </summary>
        /// <param name="filePath"></param>
        public HtmlFileHandler(string filePath)
        {
            path = filePath;
            statsSaver = null;
            statsWriter = null;
            errorLogCreator = null;
        }

        /// <summary>
        /// Конструктор в случае, если нужно только сохранить статистику, но не выводить
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="statisticsSaver"></param>
        public HtmlFileHandler(string filePath, IStatisticsSaver statisticsSaver)
        {
            path = filePath;
            statsSaver = statisticsSaver;
            statsWriter = null;
            errorLogCreator = null;
        }

        /// <summary>
        /// Конструктор в случае, если нужно только вывести статистику, но не сохранить
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="statisticsWriter"></param>
        public HtmlFileHandler(string filePath, IStatisticsWriter statisticsWriter)
        {
            path = filePath;
            statsWriter = statisticsWriter;
            statsSaver = null;
            errorLogCreator = null;
        }

        /// <summary>
        /// Конструктор в случае, если нужно и сохранить и вывести статистику
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="statisticsSaver"></param>
        /// <param name="statisticsWriter"></param>
        public HtmlFileHandler(string filePath, IStatisticsSaver statisticsSaver, IStatisticsWriter statisticsWriter)
        {
            path = filePath;
            statsSaver = statisticsSaver;
            statsWriter = statisticsWriter;
            errorLogCreator = null;
        }

        /// <summary>
        /// Подсчитывает статистику, выводит и сохраняет её, если были инициализированы нужные компоненты
        /// </summary>
        public void CalculateWordsStatistics()
        {
            try
            {
                // Заполняем словарь wordCountPairs статистикой
                using (StreamReader sr = File.OpenText(path))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        ProccessHTMLLine(line);
                    }
                }

                // Сортируем полученный словарь по убыванию, на основе того сколько раз слово встретилось
                var result = (from entry in wordCountPairs orderby entry.Value descending select entry)
                    .ToDictionary(pair => pair.Key, pair => pair.Value);

                if (statsSaver != null)
                    statsSaver.SaveStatistics(result);

                if (statsWriter != null)
                    statsWriter.WriteStatistics(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                errorLogCreator.CreateErrorLog(e.Message);
            }
            
        }


        /// <summary>
        /// Обновляет статистику на основе данной линии
        /// </summary>
        /// <param name="line"></param>
        private void ProccessHTMLLine(string line)
        {
            // Убираем все что заключенно в '<' '>'
            int startIndex = 0;
            int endIndex;

            while ((endIndex = line.IndexOf('>')) != -1)
            {

                startIndex = line.IndexOf('<');
                line = line.Remove(startIndex, endIndex - startIndex + 1);
            }
            //--------------------------------------

            // Убираем hmtl entities
            line = WebUtility.HtmlDecode(line);

            // Разделяем строку на слова
            string[] parts = line.Split(new char[] {' ', ',', '.', '!', '?','"', ';',
                ':', '[', ']', '(', ')', '\n', '\r','\t', '«', '»', '—', ' ', '-'},
                StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).ToArray();
            //--------------------------------------

            // Обновляем словарь на основе полученных слов
            AddToUniqueWords(parts);
        }

        /// <summary>
        /// Обновляет статистику на основе данных слов
        /// </summary>
        /// <param name="parts"></param>
        private void AddToUniqueWords(string[] parts)
        {
            string key;
            foreach (string word in parts)
            {
                // Каждое слово возводится в верхний регистр из соображений простоты чтения
                key = word.ToUpper();
                if (wordCountPairs.ContainsKey(key))
                    wordCountPairs[key]++;
                else
                    wordCountPairs.Add(key, 1);
            }
        }
    }
}