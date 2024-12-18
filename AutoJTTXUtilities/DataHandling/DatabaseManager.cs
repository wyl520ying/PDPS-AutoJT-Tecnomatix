//using AutoJTTXCoreUtilities;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;

namespace AutoJTTXUtilities.DataHandling
{
    public class DatabaseManager<T> where T : IDataModel, new()
    {
        //连接字符串
        private readonly string _connectionString;
        //数据库路径
        private readonly string _databaseFilePath;

        public DatabaseManager(string databaseFilePath)
        {
            _databaseFilePath = databaseFilePath;
            _connectionString = $"Data Source={_databaseFilePath}";
            EnsureDatabaseExists();
        }

        //通用的检查并创建 *.db
        private void EnsureDatabaseExists()
        {
            //检查文件夹是否存在
            var path = Path.GetDirectoryName(_databaseFilePath);
            if (!Directory.Exists(path)) 
            {
                Directory.CreateDirectory(path);
            }

            if (!File.Exists(_databaseFilePath))
            {
                SQLiteConnection.CreateFile(_databaseFilePath);
                //Console.WriteLine("Database file created.");
            }
        }

        //通用的创建数据表
        public void CreateTable(string createTableQuery)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        //清空表
        public void ClearTable(string tableName)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string sql = $"DELETE FROM {tableName}";
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        //删除一条记录
        public void DeleteRecord(string deleteQuery, T data)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(deleteQuery, connection))
                {
                    data.AddParameters(command);
                    command.ExecuteNonQuery();
                }
            }
        }

        //通用的下载数据
        public ObservableCollection<T> LoadData(string selectQuery)
        {
            var data = new ObservableCollection<T>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = new T();
                            item.Load(reader);
                            data.Add(item);
                        }
                    }
                }
            }
            return data;
        }

        //通用的'添加或更新'一行数据
        public void InsertOrUpdateData(string insertOrUpdateQuery, T data)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(insertOrUpdateQuery, connection))
                {
                    data.AddParameters(command);
                    command.ExecuteNonQuery();
                }
            }
        }

        //通用的批量'添加或更新'数据
        public void InsertOrUpdateDataBatch(string insertOrUpdateQuery, ObservableCollection<T> dataList)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    foreach (var data in dataList)
                    {
                        using (var command = new SQLiteCommand(insertOrUpdateQuery, connection))
                        {
                            data.AddParameters(command);
                            command.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                }
            }
        }
    }
}