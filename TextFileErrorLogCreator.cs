using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace UniqueWordsFromHTML
{
    class TextFileErrorLogCreator : IErrorLogCreator
    {
        public void CreateErrorLog(string errorMessege)
        {
            try
            {
                File.WriteAllText(DateTime.Now.ToString().Replace(':', '.') + ".txt", errorMessege);
            }
            catch (Exception e)
            {
                Console.WriteLine("Не удалось создать лог об ошибке: ");
                Console.WriteLine(e.Message);
            }
            
        }
    }
}
