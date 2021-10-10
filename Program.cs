using System;
using System.IO;
using System.Collections.Generic;
using MySql.Data.MySqlClient;


namespace UniqueWordsFromHTML
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                // Спрашиваем пользователя путь до файла.
                string path = ConsoleInterface.AskUserForPath();

                // Подсчёт и вывод статистики
                HtmlFileHandler fileHandler = new HtmlFileHandler(path, new DBStatisticsSaver(new TextFileErrorLogCreator()),
                    new ConsoleStatisticsWriter())
                {
                    ErrorLogCreator = new TextFileErrorLogCreator()
                };
                fileHandler.CalculateWordsStatistics();
            } while (ConsoleInterface.AskUserIfWantsToContinue() != false);

        }
    }
}

