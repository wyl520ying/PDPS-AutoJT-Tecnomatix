using System.Collections.ObjectModel;

namespace AutoJTTXUtilities.Controls.OperationsView.Models
{
    // File: Models/TreeNode.cs
    public interface TreeNode
    {
        //tree的name
        string Name { get; set; }
        //tree的图标
        string IconPath { get; }    
        
        string ExternalID { get; set; }
    }

    //有cheildren的父类
    public interface NodeColletion
    {
        //child
        ObservableCollection<TreeNode> Children { get; set; } 
    }

    // Derived models for specific node types
    public class OperationNode : TreeNode, NodeColletion
    {
        //child
        public ObservableCollection<TreeNode> Children { get; set; } = new ObservableCollection<TreeNode>();
        public string Name { get; set; } 
        public string IconPath => "pack://application:,,,/AutoJTTXUtilities;component/Controls/OperationsView/Icons/operation.png";
        public string ExternalID { get; set; }
    }

    //焊接操作节点
    public class WeldOpNode : TreeNode
    {
        public string Name { get; set; } 
        public string IconPath => "pack://application:,,,/AutoJTTXUtilities;component/Controls/OperationsView/Icons/weldop.png";
        public string ExternalID { get; set; }      
    }

    //品节点
    public class CompOpNode : TreeNode, NodeColletion
    {
        //child
        public ObservableCollection<TreeNode> Children { get; set; } = new ObservableCollection<TreeNode>();
        public string Name { get; set; } 
        public string IconPath => "pack://application:,,,/AutoJTTXUtilities;component/Controls/OperationsView/Icons/compop.png";
        public string ExternalID { get; set; }
    }

    //其他节点
    public class OthersNode : TreeNode, NodeColletion
    {
        public ObservableCollection<TreeNode> Children { get; set; } = new ObservableCollection<TreeNode>();
        public string Name { get; set; } 
        public string IconPath => "pack://application:,,,/AutoJTTXUtilities;component/Controls/OperationsView/Icons/default.png";
        public string ExternalID { get; set; }
    }
}