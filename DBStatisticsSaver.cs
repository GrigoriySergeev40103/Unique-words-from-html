using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;


namespace UniqueWordsFromHTML
{
    class DBStatisticsSaver : IStatisticsSaver
    {
        private readonly string connectionString =
                "server=127.0.0.1;uid=unique_words_app;pwd=Fkdm3oa04md4;database=unique_words_from_html";
        private MySqlConnection connection;
        private bool isConnected = false;
        private IErrorLogCreator errorLogCreator;
        public IErrorLogCreator ErrorLogCreator { get => errorLogCreator; set { errorLogCreator = value; } }

        public bool IsConnected => isConnected;

        public DBStatisticsSaver()
        {

        }

        public DBStatisticsSaver(IErrorLogCreator errorLogCreator)
        {
            this.errorLogCreator = errorLogCreator;
        }

        /// <summary>
        /// Пытается подключиться к базе данных
        /// </summary>
        /// <returns> True - если удалось подключиться без ошибок.
        ///           False - если во время подключения возникла ошибка</returns>
        private bool Connect()
        {
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                errorLogCreator.CreateErrorLog(e.Message);
                return false;
            }

            isConnected = true;
            return true;
        }

        /// <summary>
        /// Сохраняет статистику в базу данных
        /// </summary>
        /// <param name="statistics"></param>
        /// <returns> True - если удалось сохранить данные в базу данных без ошибок.
        ///           False - если не удалось сохранить данные в базу данных без ошибок</returns>
        public bool SaveStatistics(Dictionary<string, int> statistics)
        {
            if (!Connect())
                return false;

            // Запрос
            string sql;

            using (MySqlCommand sqlCommand = new MySqlCommand())
            {
                sqlCommand.Connection = connection;

                foreach (var wordCountPair in statistics)
                {
                    // Проверяем есть ли статистика по данному слову в базе данных
                    sql = $"SELECT " +
                          $"EXISTS(SELECT * " +
                          $"FROM unique_words_stats " +
                          $"WHERE word = '{ wordCountPair.Key}')";

                    sqlCommand.CommandText = sql;

                    try
                    {
                        // Если слова нет в базе данных
                        if ((long)sqlCommand.ExecuteScalar() == 0)
                        {
                            sql = "INSERT INTO unique_words_stats(word, count) " +
                                 $"VALUES('{ wordCountPair.Key}', '{wordCountPair.Value}')";
                        }
                        // Если есть
                        else
                        {
                            sql = $"UPDATE unique_words_stats " +
                                  $"SET count = count + {wordCountPair.Value} "
                                + $"WHERE word = '{wordCountPair.Key}'";
                        }

                        sqlCommand.CommandText = sql;
                        sqlCommand.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        CloseConnection();
                        errorLogCreator.CreateErrorLog(e.Message);
                        Console.WriteLine(e.Message);
                        return false;
                    }
                }
            }

            CloseConnection();
            return true;
        }

        private void CloseConnection()
        {
            connection.Close();
            isConnected = false;
        }
    }
}
