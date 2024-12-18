//using AutoJTTXCoreUtilities;
using System.Data.SQLite;

namespace AutoJTTXUtilities.DataHandling
{
    public interface IDataModel
    {
        void Load(SQLiteDataReader reader);
        void AddParameters(SQLiteCommand command);
    }
}