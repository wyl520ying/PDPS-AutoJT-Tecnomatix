using AutoJTTXCoreUtilities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace AutoJTLicensingTool.ViewModels
{
    internal class PaymentWindowModel : ObservableObject
    {
        private string tip = "";
        public string Tip
        {
            get { return tip; }
            set { SetProperty(ref tip, value); }
        }

        private string titleName = "";
        public string TitleName
        {
            get { return GlobalClass.NickName; }
            set { SetProperty(ref titleName, value); }
        }

        public PaymentWindowModel()
        {
            WeakReferenceMessenger.Default.Register<string>("MainWindow_Init", async
                (sender, message) =>
            {
                try
                {
                    await InitAsync();

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"未知错误,{ex.Message}");
                }
            });
        }

        private Task InitAsync()
        {
            return Task.CompletedTask;
        }
    }
}