using Microsoft.Win32;
using System;
using System.IO;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.Implementation.ModelObjects;
using Tecnomatix.Engineering.Implementation.Options;

namespace AutoJTTXCoreUtilities
{
    public class AJTTxApplicationUtilities
    {
        public static Version GetVersion()
        {
            Version result;
            try
            {
                result = Version.Parse(GetVersionName());
            }
            catch
            {
                result = new Version(0, 0, 0, 0);
            }
            return result;
        }

        public static string GetVersionName()
        {
            string result;
            try
            {
                result = TxApplicationImpl.the().GetVersionName();
            }
            catch
            {
                result = "0.0.0.0";
            }
            return result;
        }

        public static string[] GetLocalization()
        {
            return AJTRegistryKeys.GetSubKeys(Registry.LocalMachine, "SOFTWARE\\Tecnomatix\\eMPower\\InstalledProducts\\Localization");
        }

        public static string GetInstallationDirectory_dotNetCmd()
        {
            return Path.Combine(TxApplication.InstallationDirectory, "DotNetCommands");
        }
        public static string GetInstallationDirectory()
        {
            return TxApplication.InstallationDirectory;
        }

        public static void HideNavigationCubeAndFrame()
        {
            TxOptionsImpl.the().ShouldShowNavigationCube = false;
            TxOptionsImpl.the().ShouldShowNavigationFrame = false;
        }
        public static void ShowNavigationCubeAndFrame()
        {
            TxOptionsImpl.the().ShouldShowNavigationCube = true;
            TxOptionsImpl.the().ShouldShowNavigationFrame = true;
        }

        //public static void HideNavigationCubeAndFrameHeightVer()
        //{
        //    //隐藏框架                
        //    TxGraphicViewer.ShowNavigationCube = false;
        //    TxGraphicViewer.ShowNavigationFrame = false;
        //}
        //public static void ShowNavigationCubeAndFrameHeightVer()
        //{
        //    //显示框架
        //    TxGraphicViewer.ShowNavigationCube = true;
        //    TxGraphicViewer.ShowNavigationFrame = true;
        //}

        public static void ClearAllTransactions()
        {
            TxApplication.ActiveDocument.UndoManager.ClearAllTransactions();
        }

        public static void StartUndoTransaction()
        {
            TxApplication.ActiveUndoManager.StartTransaction();
        }

        public static void EndUndoTransaction()
        {
            TxApplication.ActiveUndoManager.EndTransaction();
        }
    }
}
