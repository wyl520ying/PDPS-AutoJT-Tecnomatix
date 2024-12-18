using System.Collections.Generic;

namespace AutoJTTXCoreUtilities
{
    public class GlobalClass
    {
        public struct User
        {
            public string strUsrId;
            public string strUsrName;
            public int trialDate;
            public Authority authority;//administrators users            
        };

        //用户的权限
        public enum Authority
        {
            administrators, users
        }

        #region public Field

        //网络测试的地址
        public static string TestNetworkIP
        {
            get
            {
#if DEBUG
                return "localhost";
#else
                return "www.autojt.com";
#endif

            }
        }

        //loginID,用于通信
        public static string LoginId { get; set; }

        //用户
        public static User user;

        //宿主软件的版本
        public static string SoftWareHostVersion { get; set; }

        //当前插件版本号
        public static string CurrentVersion { get; set; }
        //最新版本号
        public static string LastVersion { get; set; }
        //新版本下载链接
        public static string LastVersionDownloadLink { get; set; }

        //昵称
        public static string NickName { get; set; }

        //注册ID
        public static string RegId { get; set; }

        //插件的类别
        public static string Category
        {
            get
            {
                return "Tecnomatix";
            }
        }

        //机器码
        public static string DeviceId { get; set; }
        //用户电脑信息
        public static string UserPCInfos { get; set; }


        //版本可用的模块ID集合
        public static List<string> EditionAbilityModules { get; set; }
        //许可到期日
        public static System.DateTime ExpireDate;
        //版本描述
        public static string VersionDesc { get; set; }
        //内部版本用户名
        public static string Internal_tag { get; set; }
        //是否是内部版本
        public static bool? IsInternal { get; set; }

        //邀请码
        public static string InvitationCode { get; set; }

        #endregion

        #region Public static Method

        //清空数据(缓存)
        public static void ClearCache()
        {
            GlobalClass.LoginId = null;

            GlobalClass.user.strUsrId = null;
            GlobalClass.user.strUsrName = null;
            GlobalClass.user.trialDate = 0;

            GlobalClass.SoftWareHostVersion = null;
            GlobalClass.CurrentVersion = null;
            GlobalClass.LastVersion = null;

            GlobalClass.NickName = null;
            GlobalClass.RegId = null;

            //GlobalClass.Category = null;
            GlobalClass.DeviceId = null;
            GlobalClass.UserPCInfos = null;

            GlobalClass.EditionAbilityModules = null;
            ExpireDate = default;
            VersionDesc = null;
            Internal_tag = null;
            IsInternal = null;

        }

        //内外版本
        public static void ParseModels(bool? isinter)
        {
            //是否是内部版
            GlobalClass.IsInternal = isinter;
        }

        //检查版本是否可用 
        public static bool? CheckModuleIsAvailable(string doduleCode)
        {
            return true;
            bool? result = null;

            try
            {
                result = GlobalClass.EditionAbilityModules?.Contains(doduleCode) ?? false || (GlobalClass.IsInternal == true && !string.IsNullOrEmpty(GlobalClass.Internal_tag));
            }
            catch
            {
                result = null;
            }

            return result;
        }

        /*
        000	资源重命名
        001	焊点和过渡点重命名
        002	焊点截图
        003	导插枪
        004	导焊点清单
        005	工装机构快速定义
        006	新JT导入
        007	焊点参照分配
        0071	焊点参照分配包含过渡点
        008	焊点一键可达
        009	创建进出枪点
        010	焊点查重查漏
        011	快速换焊枪
        012	快速选择
        013	焊点对比
        014	CATIA焊点球提取
        015	产品导入
        016	修改OLPCommand
        017	Resource一键坐标回原点
        018	快速添加Kuka程序号_压力_板厚
         */

        #endregion
    }
}