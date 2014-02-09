using System.Configuration;
using System.Data.SQLite;

namespace GameData.Classes
{
    public static class DataModel
    {
        #region Fields
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["SQLiteConnectionString"].ConnectionString;
        #endregion


        #region Constructor
        static DataModel()
        {
        } 
        #endregion


        #region Public Methods
        public static void GetHistoryByPlayerName()
        {
            using (var sqlConn = new SQLiteConnection(ConnectionString))
            {
                sqlConn.Open();
                SQLiteCommand command = sqlConn.CreateCommand();
                command.CommandText = "SELECT";

                command.ExecuteNonQuery();
            }
        }
        #endregion
    }
}
