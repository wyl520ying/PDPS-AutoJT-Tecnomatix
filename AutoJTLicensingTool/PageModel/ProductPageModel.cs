using AutoJTL.SDK.Strandard.Request;
using AutoJTL.SDK.Strandard.Response;
using AutoJTLicensingTool.Common;
using AutoJTLicensingTool.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Contrib.Hub;
using QRCoder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;
using AutoJTTXCoreUtilities;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections;
using AutoJTL.SDK.Strandard;
using System.Threading;
using System.Windows.Threading;
using System.Data;
using System.Text.RegularExpressions;

namespace AutoJTLicensingTool.PageModel
{
    public class ProductPageModel
        : ObservableObject,
        IRecipient<ProductPageSwitchNameMessage>,
        IRecipient<ProductPageSelectedMessage>,
        IRecipient<AliPaySuccess>, IContext
    {
        #region 用于将UI Dispatcher传递给ViewModel

        private readonly Dispatcher _dispatcher;
        public bool IsSynchronized
        {
            get
            {
                return this._dispatcher.Thread == Thread.CurrentThread;
            }
        }
        public void Invoke(Action action)
        {
            Debug.Assert(action != null);

            this._dispatcher.Invoke(action);
        }

        public void BeginInvoke(Action action)
        {
            Debug.Assert(action != null);

            this._dispatcher.BeginInvoke(action);
        }

        public ProductPageModel(Dispatcher dispatcher)
        {
            Debug.Assert(dispatcher != null);

            this._dispatcher = dispatcher;
        }

        #endregion

        #region 属性

        //功能列表
        private DataTable m_DataSource;
        public DataTable DataSource
        {
            get { return m_DataSource; }
            set
            {
                if (m_DataSource != value)
                {
                    m_DataSource = value;
                    SetProperty(ref m_DataSource, value);
                }
            }
        }
        //所有的套餐列表
        public List<Product> products = new List<Product>();
        public List<Product> Products
        {
            get { return products; }
            set { SetProperty(ref products, value); }
        }
        //功能权限
        public List<ProductPermissions> permissions = new List<ProductPermissions>();
        public List<ProductPermissions> Permissions
        {
            get { return permissions; }
            set { SetProperty(ref permissions, value); }
        }
        //用户信息
        public List<ProductUserInfo> userInfos = new List<ProductUserInfo>();
        public List<ProductUserInfo> UserInfos
        {
            get { return userInfos; }
            set { SetProperty(ref userInfos, value); }
        }

        public string tip = "";
        public string Tip
        {
            get { return tip; }
            set
            {
                MessageBox.Show(value, "AutoJT");
                SetProperty(ref tip, value);
            }
        }

        //版本列表
        public List<string> productNameList;
        public List<string> ProductNameList
        {
            get { return productNameList; }
            set { SetProperty(ref productNameList, value); }
        }
        //套餐价格列表
        public List<ProductItemShowModel> productShowModels;
        public List<ProductItemShowModel> ProductShowModels
        {
            get { return productShowModels; }
            set { SetProperty(ref productShowModels, value); }
        }

        //支付宝panel
        public Visibility visibilitySelectedProduct = Visibility.Hidden;
        public Visibility VisibilitySelectedProduct
        {
            get { return visibilitySelectedProduct; }
            set { SetProperty(ref visibilitySelectedProduct, value); }
        }

        //支付宝二维码
        public BitmapImage payQRCode;
        public BitmapImage PayQRCode
        {
            get { return payQRCode; }
            set { SetProperty(ref payQRCode, value); }
        }

        //显示支付价格
        public PayShowModel payShow;
        public PayShowModel PayShow
        {
            get { return payShow; }
            set
            {
                SetProperty(ref payShow, value);
                VisibilitySelectedProduct = payShow != null ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        public RelayCommand<string> SelectProductNameCommand { get; set; }

        public RelayCommand<string> SelectProductCommand { get; set; }

        public CommunityToolkit.Mvvm.Input.RelayCommand SelectProductNameTestCommand { get; set; }

        #endregion

        #region Constructor

        public ProductPageModel() : this(Dispatcher.CurrentDispatcher)
        {
            //页面切换
            WeakReferenceMessenger.Default.Register<ProductPageSwitchNameMessage>(this);
            //用户选择套餐
            WeakReferenceMessenger.Default.Register<ProductPageSelectedMessage>(this);
            //支付成功
            WeakReferenceMessenger.Default.Register<AliPaySuccess>(this);

            SelectProductNameCommand = new RelayCommand<string>(value =>
            {

            });

            SelectProductCommand = new RelayCommand<string>(value =>
            {

            });
            SelectProductNameTestCommand = new CommunityToolkit.Mvvm.Input.RelayCommand(() =>
            {

            });

            WeakReferenceMessenger.Default.Register<string>("ProductPage_Init", async
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

        private async Task InitAsync()
        {
            await RefreshProductListAsync();
        }

        #endregion
        //重复的次数
        int irecNum = 0;

        //刷新列表
        public async Task RefreshProductListAsync(bool isReLogin = false)
        {
            try
            {
                irecNum++;

                //检查网络
                if (!await this.TestNetMethod())
                {
                    throw new Exception("网络连接失败");
                }

                //重建hub链接
                if (string.IsNullOrEmpty(GlobalClass.LoginId))
                {
                    await this.InitHubAsync();
                }

                var response = await AppSetting.AutoJTLClient.ExecuteAsync(new ProductAllListReqeust()
                {
                    LoginId = GlobalClass.LoginId,
                    //Uuid = GlobalClass.user.strUsrId,
                    //Category = GlobalClass.Category,
                });

                if (response == null
                    || !response.Code.Equals(0)
                    || response.Data == null)
                {
                    Tip = $"查询产品列表失败,{response?.Msg}";
                    return;
                }

                Products = response.Data.Products;
                Permissions = response.Data.Permissions;
                UserInfos = response.Data.UserInfos;

                DataSource = DataSource ?? new DataTable();

                var 描述列表GroupBy = Permissions.GroupBy(x => x.套餐描述);

                List<string> nameList = 描述列表GroupBy.Select(x => x.Key).ToList();//OrderBy(x => x).ToList()

                List<string> functionList = Permissions.Select(x => x.功能特权描述.Trim()).Distinct().ToList();



                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("功能特权", typeof(string)));
                foreach (var item in nameList)
                {
                    dt.Columns.Add(new DataColumn(item, typeof(string)));
                }

                //添加功能特权的数据
                foreach (string item in functionList)
                {
                    DataRow dataRow1 = dt.NewRow();
                    dataRow1["功能特权"] = item;

                    foreach (string name in nameList)
                    {
                        string nameValue = Permissions.FirstOrDefault(x => x.套餐描述 == name && x.功能特权描述.Trim() == item)?.功能可用状态;
                        if (string.IsNullOrEmpty(nameValue))
                        {
                            dataRow1[name] = "-";
                        }
                        else
                        {
                            string item1 = nameValue.Equals("1") ? "支持" : "-";
                            dataRow1[name] = item1;
                        }
                    }
                    dt.Rows.Add(dataRow1);
                }


                //功能特权排序
                //排序字符串
                string strSort = string.Empty;
                for (int i = nameList.Count - 1; i >= 0; i--)
                {
                    string item = nameList[i];
                    if (string.IsNullOrEmpty(strSort))
                    {
                        strSort = $"[{item}] desc";
                    }
                    else
                    {
                        strSort += $", [{item}] desc";
                    }
                }
                DataView dv = dt.DefaultView;
                dv.Sort = strSort;
                this.DataSource = dv.ToTable();



                //找到所有的版本
                ProductNameList = nameList;

                //0是试用版, 1是免费版, 2开始是收费版本
                if (nameList.Count > 0)
                    ProductShowModels = Products.Where(x => x.套餐描述 == nameList[2]).Select(x => new ProductItemShowModel(x)).ToList();


                //进程是否是重新登录的进程
                if (!isReLogin)
                {
                    //显示tab
                    WeakReferenceMessenger.Default.Send(new TabControlAddItemViewMessage()
                    {
                        //0是试用版, 1是免费版, 2开始是收费版本
                        ProductNameList = this.ProductNameList.Where(a => a != "试用版" && a != "免费版").ToList(),
                        ProductShowModels = this.ProductShowModels,
                    });


                    //添加特权的grid列
                    WeakReferenceMessenger.Default.Send(new QueryProductAllListSuccessMessage()
                    {
                        ProductNameList = nameList,
                        GridDataSource = this.DataSource,
                    });
                }

                //切换到tab1                
                //this.Receive(new ProductPageSwitchNameMessage() { Name = this.ProductNameList[2] });

                PayShow = null;
            }
            catch (Exception ex)
            {
                //超过5次
                if (irecNum > 5)
                {
                    Tip = $"查询产品列表异常,{ex.Message}";
                }
                else
                {
                    await InitAsync();
                }
            }
        }

        //用户切换tab
        public void Receive(ProductPageSwitchNameMessage message)
        {
            try
            {
                ProductShowModels = Products.Where(x => x.套餐描述 == message.Name).Select(x => new ProductItemShowModel(x)).ToList();
                PayQRCode = null;
                PayShow = null;
            }
            catch (Exception ex)
            {
                Tip = $"未知异常,{ex.Message}";
            }
        }
        //用户选中套餐
        public async void Receive(ProductPageSelectedMessage message)
        {
            try
            {

                //检查网络   
                int itestNet = default;
                do
                {
                    itestNet++;

                    //检查网络连接
                    if (await this.TestNetMethod())
                    {
                        //网络畅通
                        break;
                    }
                    else
                    {
                        if (itestNet == 5)
                        {
                            Tip = "网络连接失败";
                            return;
                        }
                    }

                } while (itestNet < 5);



                //检查用户是否可以购买这个版本
                bool bl1 = AutoJTTXServiceUtilities.AJTDatabaseOperation.CheckUserOwnerVersion(GlobalClass.user.strUsrId, message.Id, out int? ownerVer);
                if (!bl1)
                {
                    Tip = $"您已经拥有更高的版本, 不可以重复选择低版本";
                    return;
                }


                PayQRCode = null;


                if (this.ProductShowModels != null)
                {
                    List<ProductItemShowModel> itemShowModels = new List<ProductItemShowModel>();

                    foreach (var item in this.ProductShowModels)
                    {
                        if (item.套餐ID == message.Id)
                        {
                            item.显示选中符号 = Visibility.Visible;
                        }
                        itemShowModels.Add(item);
                    }

                    this.ProductShowModels = itemShowModels;
                }


                //ProductItemShowModel showproduct = this.ProductShowModels.FirstOrDefault(x =>
                //{
                //    return x.套餐ID == message.Id;
                //});

                Product product = Products.FirstOrDefault(x => x.套餐ID == message.Id);
                if (product == null)//|| showproduct == null)
                {
                    Tip = "选择错误";
                    return;
                }
                PayShow = new PayShowModel(product);
                var response = AppSetting.AutoJTLClient.ExecuteAsync(new QueryProductPayLinkRequest()
                {
                    LoginId = GlobalClass.LoginId,
                    Money = PayShow.实付,
                    ProductId = product.套餐ID,
                    //Uuid = GlobalClass.user.strUsrId
                }).Result;

                if (response == null
                        || !response.Code.Equals(0)
                       || response.Data == null)
                {
                    Tip = $"获取二维码失败,{response?.Msg}";
                    return;
                }

                using (var qrGenerator = new QRCodeGenerator())
                using (var qrCodeData = qrGenerator.CreateQrCode(response.Data.QrCode, QRCodeGenerator.ECCLevel.L))
                using (var pngByteQrCode = new PngByteQRCode(qrCodeData))
                {
                    var pngBytes = pngByteQrCode.GetGraphic(20, false);
                    PayQRCode = ByteToImage(pngBytes);
                };
            }
            catch (Exception ex)
            {
                Tip = $"未知异常,{ex.Message}";
            }
        }

        public BitmapImage ByteToImage(byte[] imageData)
        {
            BitmapImage biImg = new BitmapImage();
            try
            {
                using (MemoryStream ms = new MemoryStream(imageData))
                {
                    biImg.BeginInit();
                    biImg.StreamSource = ms;
                    biImg.CacheOption = BitmapCacheOption.OnLoad;
                    biImg.EndInit();
                    biImg.Freeze();
                }
            }
            catch (Exception ex)
            {
                Tip = $"转换图片失败:: {ex.Message}";
            }
            return biImg;
        }

        private HashSet<string> alipayTradeNos = new HashSet<string>();
        public void Receive(AliPaySuccess message)
        {
            if (!alipayTradeNos.Contains(message.AlipayTradeNo))
            {
                alipayTradeNos.Add(message.AlipayTradeNo);
                this.Invoke(new Action(() =>
                {
                    MessageBox.Show("支付成功!");
                }));

                PayQRCode = null;
                PayShow = null;

                //关闭窗口重新登录
                WeakReferenceMessenger.Default.Send(new PaySuccessMessage());
            }
        }

        //重建hub链接
        Task InitHubAsync()
        {
            return Task.Run(async () =>
            {
                try
                {
                    if (!await Global.InitHubConnectionAsync())
                    {
                        throw new Exception("初始化失败");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"未知异常{ex.Message}");
                }
            });
        }
        //检查网络
        async Task<bool> TestNetMethod()
        {
            bool result = false;

            if (!await Global.TestNetworkConnectionAsync())
            {
                result = false;
            }
            else
            {
                result = true;
            }

            return result;
        }
    }

    public sealed class ProductItemShowModel
    {

        public ProductItemShowModel(Product product)
        {
            this.套餐ID = product.套餐ID;
            this.套餐描述 = product.套餐描述;
            this.套餐时长 = product.套餐时长;
            this.实付价格 = product.实付价格;
            this.原价 = product.原价;
            this.新人专享价 = product.新人专享价;

            显示新人专享价 = Visibility.Collapsed;
            显示新人专享价符号 = Visibility.Visible;
            显示实付价 = Visibility.Collapsed;
            显示原价 = Visibility.Collapsed;

            this.显示选中符号 = Visibility.Hidden;

            if (!string.IsNullOrEmpty(product.新人专享价))
            {
                this.显示新人专享价 = Visibility.Visible;
                this.显示新人专享价符号 = Visibility.Visible;
                this.标头 = "超值";//"新人专享价";


                try
                {
                    //天
                    if (this.套餐时长.IndexOf("天") != -1)
                    {
                        //天数
                        string strDays = Regex.Replace(this.套餐时长, @"[\u4e00-\u9fa5]", "");
                        if (int.TryParse(strDays, out int idays))
                        {
                            if (double.TryParse(this.新人专享价, out double newUserPrice))
                            {
                                this.每天价格 = Math.Round(newUserPrice / idays, 2).ToString();
                            }
                        }
                    }
                    else if (this.套餐时长.IndexOf("月") != -1)
                    {
                        //月数
                        string strmouth = Regex.Replace(this.套餐时长, @"[\u4e00-\u9fa5]", "");
                        if (int.TryParse(strmouth, out int imouth))
                        {
                            if (double.TryParse(this.新人专享价, out double newUserPrice))
                            {
                                this.每天价格 = Math.Round(newUserPrice / imouth / 31, 2).ToString();
                            }
                        }
                    }
                    else if (this.套餐时长.IndexOf("年") != -1)
                    {
                        //年数
                        string stryear = Regex.Replace(this.套餐时长, @"[\u4e00-\u9fa5]", "");
                        if (int.TryParse(stryear, out int iyear))
                        {
                            if (double.TryParse(this.新人专享价, out double newUserPrice))
                            {
                                this.每天价格 = Math.Round(newUserPrice / iyear / 366, 2).ToString();
                            }
                        }
                    }
                    else
                    {
                        this.每天价格 = "0.01";
                    }
                }
                catch
                {
                }
            }
            else
            {
                this.显示实付价 = Visibility.Visible;
                this.显示原价 = Visibility.Visible;
                this.标头 = "超值";


                try
                {
                    //天
                    if (this.套餐时长.IndexOf("天") != -1)
                    {
                        //天数
                        string strDays = Regex.Replace(this.套餐时长, @"[\u4e00-\u9fa5]", "");
                        if (int.TryParse(strDays, out int idays))
                        {
                            if (double.TryParse(this.实付价格, out double newUserPrice))
                            {
                                this.每天价格 = Math.Round(newUserPrice / idays, 2).ToString();
                            }
                        }
                    }
                    else if (this.套餐时长.IndexOf("月") != -1)
                    {
                        //月数
                        string strmouth = Regex.Replace(this.套餐时长, @"[\u4e00-\u9fa5]", "");
                        if (int.TryParse(strmouth, out int imouth))
                        {
                            if (double.TryParse(this.实付价格, out double newUserPrice))
                            {
                                this.每天价格 = Math.Round(newUserPrice / imouth / 31, 2).ToString();
                            }
                        }
                    }
                    else if (this.套餐时长.IndexOf("年") != -1)
                    {
                        //年数
                        string stryear = Regex.Replace(this.套餐时长, @"[\u4e00-\u9fa5]", "");
                        if (int.TryParse(stryear, out int iyear))
                        {
                            if (double.TryParse(this.实付价格, out double newUserPrice))
                            {
                                this.每天价格 = Math.Round(newUserPrice / iyear / 366, 2).ToString();
                            }
                        }
                    }
                    else
                    {
                        this.每天价格 = "0.01";
                    }
                }
                catch
                {
                }
            }
        }

        public string 套餐ID { get; set; }

        public string 套餐描述 { get; set; }

        public string 套餐时长 { get; set; }

        public string 实付价格 { get; set; }

        public string 原价 { get; set; }

        public string 新人专享价 { get; set; }
        public string 标头 { get; set; }
        public string 每天价格 { get; set; }

        public Visibility 显示新人专享价 { get; set; }
        public Visibility 显示新人专享价符号 { get; set; }
        public Visibility 显示实付价 { get; set; }

        public Visibility 显示原价 { get; set; }

        public Visibility 显示选中符号 { get; set; }
    }

    public class PayShowModel
    {
        public PayShowModel(Product product)
        {
            this.描述 = product.套餐描述;
            this.时长 = product.套餐时长;
            if (!string.IsNullOrEmpty(product.新人专享价))
            {
                实付 = product.新人专享价;
                try
                {
                    优惠 = (Convert.ToDecimal(product.原价) - Convert.ToDecimal(product.新人专享价)).ToString();
                }
                catch { }
            }
            else
            {
                实付 = product.实付价格;
                try
                {
                    优惠 = (Convert.ToDecimal(product.原价) - Convert.ToDecimal(product.实付价格)).ToString();
                }
                catch { }
            }
        }

        public string 描述 { get; set; }

        public string 时长 { get; set; }

        public string 实付 { get; set; }

        public string 优惠 { get; set; }
    }
}
