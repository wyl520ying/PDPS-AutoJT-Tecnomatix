using AutoJTTXUtilities.Controls;
using System.Data.SQLite;

namespace AutoJTTXUtilities.DataHandling.Customize
{
    //车型和操作的接口
    public interface VehicleModel_Base 
    {
        string ID {  get; set; }
        //string Name { get; set; }
    }
      
    //车型额操作的实现类
    public class VehicleModel_NodeRelations_Model : VehicleModel_Base, IDataModel
    {
        string _ID;
        string _Name;

        //所属的车型ID
        string _ParentID;

        public string ID { get => _ID; set => _ID = value; }
        public string Name { get => _Name; set => _Name = value; }
        public string ParentID { get => _ParentID; set => _ParentID = value; }

        public void AddParameters(SQLiteCommand command)
        {
            command.Parameters.AddWithValue("@ID", ID);

            if (!string.IsNullOrWhiteSpace(Name))
            {
                command.Parameters.AddWithValue("@Name", Name);
            }

            if (!string.IsNullOrWhiteSpace(ParentID))
            {
                //所属的车型ID
                command.Parameters.AddWithValue("@ParentID", ParentID);
            }      
        }

        public void Load(SQLiteDataReader reader)
        {
            ID = reader.GetString(0);

            if (reader.FieldCount == 3)
            {
                Name = reader.GetString(1);
                //所属的车型ID
                ParentID = reader.GetString(2);
            }
        }
    }
}