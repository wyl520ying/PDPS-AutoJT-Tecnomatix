using AutoJTTXUtilities.Controls;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoJTTXUtilities.DataHandling.Customize
{
    public class IDNameValue_Model : AJTPropertyChanged, IDataModel
    {
        private string _id;
        private string _column1;
        private string _column2;
        private string _column3;

        public string Id { get => _id; set => SetPropNotify(ref _id, value); }
        public string Name { get => _column1; set => SetPropNotify(ref _column1, value); }
        public string Value { get => _column2; set => SetPropNotify(ref _column2, value); }
        public string ParentID { get => _column3; set => SetPropNotify(ref _column3, value); }



        public void Load(SQLiteDataReader reader)
        {
            Id = reader.GetString(0);
            Name = reader.GetString(1);
            Value = reader.GetString(2);
            ParentID = reader.GetString(3);
        }

        public void AddParameters(SQLiteCommand command)
        {
            command.Parameters.AddWithValue("@Id", Id);
            command.Parameters.AddWithValue("@Name", Name);
            command.Parameters.AddWithValue("@Value", Value);
            command.Parameters.AddWithValue("@ParentID", ParentID);
        }
    }
}
