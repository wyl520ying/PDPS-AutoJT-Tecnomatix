using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoJTTXUtilities.Controls.InvitationWindow.ViewModels
{
    public class InvitationDialogViewModel : AJTPropertyChanged
    {
        string _invitationCode;
        public string InvitationCode { get => _invitationCode; set => SetPropNotify(ref _invitationCode, value); }

        string _downloadLink;

        public InvitationDialogViewModel(string code,string downloadLink)
        {
            this.InvitationCode = code;
            this._downloadLink = downloadLink;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("每位新用户输入推荐码并成功登录后，推荐人即可增加一个月AutoJT_TX专业版使用时长(重新登录后查看)。AutoJT包含2D尺寸标注功能，也可以分享给设计同事。");
            sb.AppendLine("仿真插件: 导插枪、焊点清单、焊点参照分配、焊点截图等十几项功能，试用期一个月。软件右上角有分享功能");
            sb.AppendLine("设计插件: 组孔公差尺寸批量标注，最低版本要求AutoCAD2018，安装后输入JRH命令，按提示操作。此功能目前免费。");
            sb.Append("AutoJT_CAD插件介绍: ");
            sb.AppendLine("https://mp.weixin.qq.com/s?__biz=Mzg2NjEwODUyNQ==&mid=2247483928&idx=1&sn=7c469303956aa9b0d49de4bfb543d3a4&chksm=ce4e971af9391e0c44f2f1b6f89fae60387f1288fac22ce4fb709316004c505e177939dc14bb#rd");
            sb.Append("软件下载地址: ");
            sb.AppendLine(this._downloadLink);
            sb.Append("我的邀请码: ");
            sb.AppendLine(this._invitationCode);

            return sb.ToString();
        }
    }
}
