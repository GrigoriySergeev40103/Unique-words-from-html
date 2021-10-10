using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueWordsFromHTML
{
    interface IErrorLogCreator
    {
        public void CreateErrorLog(string errorMessage);
    }
}
