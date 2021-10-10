using System;
using System.IO;
using System.Collections.Generic;

namespace UniqueWordsFromHTML
{
    static class ConsoleInterface
    {
        public static string AskUserForPath()
        {
            string path;

            // Спрашиваем путь, пока пользователь не введёт коректный
            while (true)
            {
                Console.WriteLine("Введите путь до обрабатываемого файла: ");
                path = Console.ReadLine();

                if (!File.Exists(path))
                    Console.WriteLine("Не существует файла по заданному пути!");
                else
                    break;
            }

            return path;
        }

        public static bool AskUserIfWantsToContinue()
        {
            // Спрашиваем пользователя, хочет ли он продолжить, пока не введёт коректный ответ
            while (true)
            {
                Console.WriteLine("Хотите продолжить?(Y/N) :");
                string answer = Console.ReadLine();
                if (answer == "Y")
                    return true;
                else if (answer == "N")
                    return false;
                else
                    Console.WriteLine("Неккоректный ввод, повторите.");
            }
        }
    }
}
