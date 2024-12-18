//using AutoJTTXCoreUtilities;
using AutoJTTXUtilities.Controls;
using System.Data.SQLite;

namespace AutoJTTXUtilities.DataHandling.Customize
{
    public class RoboMgrDataModel : AJTPropertyChanged, IDataModel
    {
        private string _id;
        private string _column1;
        private string _column2;

        public string Id { get => _id; set => SetPropNotify(ref _id, value); }
        public string Column1 { get => _column1; set => SetPropNotify(ref _column1, value); }
        public string Column2 { get => _column2; set => SetPropNotify(ref _column2, value); }


        public void Load(SQLiteDataReader reader)
        {            
            if (reader.FieldCount == 3)
            {
                Id = reader.GetString(0);
                Column1 = reader.GetString(1);
                Column2 = reader.GetString(2);
            }
            else if (reader.FieldCount == 1)
            {
                Id = reader.GetString(0);
            }
        }

        public void AddParameters(SQLiteCommand command)
        {
            command.Parameters.AddWithValue("@Id", Id);
            command.Parameters.AddWithValue("@Column1", Column1);
            command.Parameters.AddWithValue("@Column2", Column2);
        }
    }
}