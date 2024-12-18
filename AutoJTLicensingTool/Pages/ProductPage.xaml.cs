using AutoJTLicensingTool.Messages;
using AutoJTLicensingTool.PageModel;
using AutoJTTXCoreUtilities;
using AutoJTTXServiceUtilities;
using AutoJTTXUtilities.Controls;
using CommunityToolkit.Mvvm.Messaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AutoJTLicensingTool.Pages
{
    /// <summary>
    /// ProductPage.xaml 的交互逻辑
    /// </summary>
    public partial class ProductPage : Page
        , IRecipient<QueryProductAllListSuccessMessage>, IRecipient<TabControlAddItemViewMessage>
    {
        Window m_MainWindow { get; set; }
        ProductPageModel _app;

        //用户点击的功能id
        string s_fun_id;

        public ProductPage(Window mainWindow, string fun_id)
        {
            InitializeComponent();

            //用户点击的功能id
            this.GetVersion4ID(fun_id);

            this.DataContext = this._app = new ProductPageModel();
            this.dataGrid.Visibility = Visibility.Hidden;
            this.textbox2.Visibility = Visibility.Hidden;

            WeakReferenceMessenger.Default.Register<QueryProductAllListSuccessMessage>(this);
            WeakReferenceMessenger.Default.Register<TabControlAddItemViewMessage>(this);

            this.m_MainWindow = mainWindow;
        }
        public override void BeginInit()
        {
            WeakReferenceMessenger.Default.Send("ProductPage_Init");
            base.BeginInit();
        }

        //功能特权的列
        public async void Receive(QueryProductAllListSuccessMessage message)
        {
            try
            {
                this.dataGrid.Visibility = Visibility.Visible;
                this.textbox2.Visibility = Visibility.Visible;

                dataGrid.ItemsSource = message.GridDataSource.DefaultView;
                for (int i = 0; i < message.ProductNameList.Count; i++)
                {
                    DataGridTextColumn textColumn = new DataGridTextColumn()
                    {
                        Header = message.ProductNameList[i] == "试用版" ? "试用版(7天)" : message.ProductNameList[i],
                        Binding = new Binding()
                        {
                            Path = new PropertyPath($"[{i + 1}]"),
                            Mode = BindingMode.OneWay,
                        },
                        FontSize = 14.0,
                    };
                    dataGrid.Columns.Add(textColumn);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"未知异常,{ex.Message}");
            }

            //2024年5月14日 - 按要求取消二次登录验证
            //await OpenCheckLoginUsersMethod();
        }

        //弹出二次确认账号对话框
        async Task OpenCheckLoginUsersMethod()
        {
            #region 弹出确认账号对话框

            AutoJTLicensingTool.Views.AJTLicenseDialog aJTConfirmBoxDialog = new AutoJTLicensingTool.Views.AJTLicenseDialog();

            WindowHelper.SetOwner(aJTConfirmBoxDialog, this.m_MainWindow);

            aJTConfirmBoxDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            //用户是否重新扫码登录过
            bool isReLogin = false;

            aJTConfirmBoxDialog.IsReLoginUsers = (isrelogin) =>
            {
                isReLogin = isrelogin;
            };

            aJTConfirmBoxDialog.ShowDialog();

            //用户重新登录过
            if (isReLogin)
            {
                //重新刷新产品列表 
                await this._app.RefreshProductListAsync(true);

                //更新标题栏
                this.m_MainWindow.Title = $"订阅管理 - {GlobalClass.NickName}";
            }

            #endregion
        }

        //tab1初始化
        public void Receive(TabControlAddItemViewMessage message)
        {
            try
            {
                //用于切换对应的版本
                TabItem _verdesc_target = null;

                foreach (string item in message.ProductNameList)
                {
                    //添加item
                    TabItem newItem1 = new TabItem
                    {
                        Header = item,
                        Name = item,
                        Style = (Style)this.FindResource("TabItemExWithUnderLineStyle"),
                        Cursor = Cursors.Hand,
                        FontSize = 22,
                        Height = 38,
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(0),
                        Tag = item,
                    };
                    //添加tab单击事件 PreviewMouseLeftButtonDown
                    newItem1.AddHandler(UIElement.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(this.TextBlock_MouseLeftButtonDown), true);



                    #region 在item中创建一个stackpanel

                    //新建stackpanel
                    StackPanel stackPanel = new StackPanel() { Orientation = Orientation.Horizontal };

                    #region 在stackpanle中创建一个 itemscontrol

                    //创建一个virtualizingstackpanel
                    FrameworkElementFactory virtualizingstackpanel = new FrameworkElementFactory(typeof(VirtualizingStackPanel));
                    virtualizingstackpanel.SetValue(VirtualizingStackPanel.OrientationProperty, Orientation.Horizontal);

                    //新建itemscontrol并绑定数据源
                    ItemsControl itemsControl = new ItemsControl();
                    itemsControl.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("ProductShowModels"));
                    itemsControl.SetValue(ItemsControl.ItemsPanelProperty, new ItemsPanelTemplate() { VisualTree = virtualizingstackpanel });

                    //在itemsControl中创建一个 dataTemplate
                    DataTemplate dataTemplate = new DataTemplate();//typeof(Product)
                    itemsControl.SetValue(ItemsControl.ItemTemplateProperty, dataTemplate);

                    #region 在DataTemplate中创建一个 border

                    //创建一个Border
                    FrameworkElementFactory border_套餐 = new FrameworkElementFactory(typeof(Border));
                    border_套餐.SetValue(Border.BorderBrushProperty, (Brush)new BrushConverter().ConvertFrom("#fad37d"));
                    border_套餐.SetValue(Border.HeightProperty, 155.0);
                    border_套餐.SetValue(Border.CornerRadiusProperty, new CornerRadius(10));
                    border_套餐.SetValue(Border.WidthProperty, 150.0);
                    border_套餐.SetValue(Border.BorderThicknessProperty, new Thickness(2));
                    border_套餐.SetValue(Border.MarginProperty, new Thickness(7,15,3,15));
                    //给 border 绑定一个tag
                    border_套餐.SetBinding(StackPanel.TagProperty, new Binding("套餐ID"));
                    
                    //给 border 设置单击事件
                    //border_套餐.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(StackPanel_MouseLeftButtonDown), true);
                    border_套餐.AddHandler(MouseLeftButtonUpEvent, new MouseButtonEventHandler(StackPanel_MouseLeftButtonDown), true);

                    #region 在border内创建一个 stackpanel1

                    //在border内创建一个stackpanel
                    FrameworkElementFactory stackPanel1 = new FrameworkElementFactory(typeof(StackPanel));
                    stackPanel1.SetValue(StackPanel.BackgroundProperty, Brushes.Transparent);

                    #region 在stackpanel中创建一个 border1

                    //在stackpanel中创建一个border
                    FrameworkElementFactory border1 = new FrameworkElementFactory(typeof(Border));
                    border1.SetValue(Border.CornerRadiusProperty, new CornerRadius(10, 0, 8, 0));
                    border1.SetValue(Border.HorizontalAlignmentProperty, HorizontalAlignment.Left);
                    border1.SetBinding(Border.VisibilityProperty, new Binding("显示新人专享价符号"));
                    border1.SetValue(Border.BorderBrushProperty, Brushes.Transparent);
                    border1.SetValue(Border.BackgroundProperty, (Brush)new BrushConverter().ConvertFrom("#E64C32"));
                    border1.SetValue(Border.MarginProperty, new Thickness(-1, -1, 2, 2));


                    #region 在border1中添加一个 textblock1

                    FrameworkElementFactory textblock1 = new FrameworkElementFactory(typeof(TextBlock));
                    textblock1.SetValue(TextBlock.PaddingProperty, new Thickness(5, 0, 5, 0));
                    textblock1.SetBinding(TextBlock.TextProperty, new Binding("标头"));
                    textblock1.SetValue(TextBlock.FontSizeProperty, 14.0);
                    textblock1.SetValue(TextBlock.HeightProperty, 20.0);
                    textblock1.SetValue(TextBlock.FontWeightProperty, FontWeights.Normal);
                    textblock1.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Left);
                    textblock1.SetValue(TextBlock.BackgroundProperty, Brushes.Transparent);
                    textblock1.SetValue(TextBlock.ForegroundProperty, Brushes.White);

                    #endregion

                    border1.AppendChild(textblock1);

                    #endregion

                    stackPanel1.AppendChild(border1);

                    #region 在 stackpanel1 中创建一个 TextBlock2

                    FrameworkElementFactory textblock2 = new FrameworkElementFactory(typeof(TextBlock));
                    textblock2.SetBinding(TextBlock.TextProperty, new Binding("套餐时长"));
                    textblock2.SetValue(TextBlock.MarginProperty, new Thickness(0, 5, 0, 0));
                    textblock2.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Center);
                    textblock2.SetValue(TextBlock.FontSizeProperty, 16.0);
                    textblock2.SetValue(TextBlock.ForegroundProperty, (Brush)new BrushConverter().ConvertFrom("#6B2D00"));

                    #endregion

                    stackPanel1.AppendChild(textblock2);

                    #region 在 stackpanel1 中添加一个 stackpanel2

                    FrameworkElementFactory stackPanel2 = new FrameworkElementFactory(typeof(StackPanel));
                    stackPanel2.SetValue(StackPanel.OrientationProperty, Orientation.Vertical);
                    stackPanel2.SetValue(StackPanel.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                    stackPanel2.SetValue(StackPanel.MarginProperty, new Thickness(0));


                    #region 在 stackpanel2 中添加4个 TextBlock11 12 13 14

                    FrameworkElementFactory TextBlock11 = new FrameworkElementFactory(typeof(TextBlock));
                    TextBlock11.SetValue(TextBlock.MarginProperty, new Thickness(5, 5, 5, 0));
                    TextBlock11.SetBinding(TextBlock.VisibilityProperty, new Binding("显示实付价"));
                    TextBlock11.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Center);
                    TextBlock11.SetValue(TextBlock.FontSizeProperty, 22.0);
                    TextBlock11.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                    TextBlock11.SetValue(TextBlock.ForegroundProperty, (Brush)new BrushConverter().ConvertFrom("#FF6600"));

                    var bold = new FrameworkElementFactory(typeof(Bold));
                    var run = new FrameworkElementFactory(typeof(Run));
                    run.SetValue(Run.TextProperty, "￥");
                    bold.AppendChild(run);
                    TextBlock11.AppendChild(bold);

                    var normal = new FrameworkElementFactory(typeof(Run));
                    //normal.SetValue(Run.FontWeightProperty, FontWeights.Bold);
                    normal.SetBinding(Run.TextProperty, new Binding("实付价格"));
                    TextBlock11.AppendChild(normal);


                    FrameworkElementFactory TextBlock12 = new FrameworkElementFactory(typeof(TextBlock));
                    TextBlock12.SetValue(TextBlock.MarginProperty, new Thickness(5, 5, 5, 0));
                    TextBlock12.SetBinding(TextBlock.VisibilityProperty, new Binding("显示新人专享价"));
                    TextBlock12.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Center);
                    TextBlock12.SetValue(TextBlock.FontSizeProperty, 22.0);
                    TextBlock12.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                    TextBlock12.SetValue(TextBlock.ForegroundProperty, (Brush)new BrushConverter().ConvertFrom("#FF6600"));

                    var bold1 = new FrameworkElementFactory(typeof(Bold));
                    var run1 = new FrameworkElementFactory(typeof(Run));
                    run1.SetValue(Run.TextProperty, "￥");
                    bold1.AppendChild(run1);
                    TextBlock12.AppendChild(bold1);

                    var normal1 = new FrameworkElementFactory(typeof(Run));
                    //normal1.SetValue(Run.FontWeightProperty, FontWeights.Bold);
                    normal1.SetBinding(Run.TextProperty, new Binding("新人专享价"));
                    TextBlock12.AppendChild(normal1);

                    FrameworkElementFactory TextBlock13 = new FrameworkElementFactory(typeof(TextBlock));
                    TextBlock13.SetValue(TextBlock.MarginProperty, new Thickness(5, 5, 5, 0));
                    TextBlock13.SetValue(TextBlock.TextDecorationsProperty, TextDecorations.Strikethrough);
                    TextBlock13.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Center);
                    TextBlock13.SetValue(TextBlock.FontSizeProperty, 14.0);
                    TextBlock13.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Bottom);
                    TextBlock13.SetValue(TextBlock.ForegroundProperty, (Brush)new BrushConverter().ConvertFrom("#7B7B7B"));

                    var run2 = new FrameworkElementFactory(typeof(Run));
                    run2.SetValue(Run.FontWeightProperty, FontWeights.Normal);
                    run2.SetValue(Run.TextProperty, "￥");
                    TextBlock13.AppendChild(run2);

                    var normal2 = new FrameworkElementFactory(typeof(Run));
                    normal2.SetValue(Run.FontWeightProperty, FontWeights.Normal);
                    normal2.SetBinding(Run.TextProperty, new Binding("原价"));
                    TextBlock13.AppendChild(normal2);


                    FrameworkElementFactory TextBlock14 = new FrameworkElementFactory(typeof(TextBlock));
                    TextBlock14.SetValue(TextBlock.MarginProperty, new Thickness(5, 5, 5, 0));
                    TextBlock14.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Center);
                    TextBlock14.SetValue(TextBlock.FontSizeProperty, 12.0);
                    TextBlock14.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Bottom);
                    TextBlock14.SetValue(TextBlock.ForegroundProperty, (Brush)new BrushConverter().ConvertFrom("#A67D5D"));

                    var run21 = new FrameworkElementFactory(typeof(Run));
                    run21.SetValue(Run.FontWeightProperty, FontWeights.Normal);
                    run21.SetValue(Run.TextProperty, "约￥");//低至
                    TextBlock14.AppendChild(run21);

                    var normal21 = new FrameworkElementFactory(typeof(Run));
                    normal21.SetValue(Run.FontWeightProperty, FontWeights.Normal);
                    normal21.SetBinding(Run.TextProperty, new Binding("每天价格"));
                    TextBlock14.AppendChild(normal21);

                    var normal211 = new FrameworkElementFactory(typeof(Run));
                    normal211.SetValue(Run.FontWeightProperty, FontWeights.Normal);
                    normal211.SetValue(Run.TextProperty, "/天");
                    TextBlock14.AppendChild(normal211);

                    #endregion

                    stackPanel2.AppendChild(TextBlock11);
                    stackPanel2.AppendChild(TextBlock12);
                    stackPanel2.AppendChild(TextBlock13);
                    stackPanel2.AppendChild(TextBlock14);

                    #endregion

                    stackPanel1.AppendChild(stackPanel2);

                    #region 在 stackpanel1 中添加一个 grid1

                    /*

                     <Grid>
                        <Path  Stroke="#CA942D" StrokeThickness="5"  Fill="#CA942D" Data="M 125,155 L 150,130 L 150,145 A10,10 90 0 1 140 155 L 125,155 Z"/>
                        <Path Stroke="White" StrokeThickness="2" Data="M 135.27,147.55 L 139.51,151.8 L 148.35,142.96"/>
                     </Grid>

                     */
                    FrameworkElementFactory grid1 = new FrameworkElementFactory(typeof(Grid));
                    grid1.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Right);
                    grid1.SetValue(Grid.VerticalAlignmentProperty, VerticalAlignment.Bottom);
                    grid1.SetValue(MarginProperty, new Thickness(0, 16, -1, 0));
                    grid1.SetBinding(Grid.VisibilityProperty, new Binding("显示选中符号"));

                    #region 在 grid1 中添加2个path

                    FrameworkElementFactory paht1 = new FrameworkElementFactory(typeof(Path));
                    paht1.SetValue(VerticalAlignmentProperty, VerticalAlignment.Bottom);
                    paht1.SetValue(Path.StrokeProperty, (Brush)new BrushConverter().ConvertFrom("#FAD37D"));
                    paht1.SetValue(Path.StrokeThicknessProperty, 3.0);
                    paht1.SetValue(Path.FillProperty, (Brush)new BrushConverter().ConvertFrom("#FAD37D"));
                    paht1.SetValue(Path.DataProperty, TypeDescriptor.GetConverter(typeof(Geometry)).ConvertFrom("M 0,13 L 25,-8 L 25,3 A8,8 90 0 1 15 13 L 0,13 Z"));

                    FrameworkElementFactory paht2 = new FrameworkElementFactory(typeof(Path));
                    paht2.SetValue(Path.StrokeProperty, Brushes.White);
                    paht2.SetValue(Path.StrokeThicknessProperty, 2.0);
                    paht2.SetValue(Path.DataProperty, TypeDescriptor.GetConverter(typeof(Geometry)).ConvertFrom("M 10.27,5.55 L 14.51,9.8 L 23.35,0.96"));

                    #endregion

                    grid1.AppendChild(paht1);
                    grid1.AppendChild(paht2);

                    #endregion

                    stackPanel1.AppendChild(grid1);

                    #endregion

                    //添加到botder
                    border_套餐.AppendChild(stackPanel1);

                    #endregion

                    //将border作为DataTemplate的VisualTree属性
                    dataTemplate.VisualTree = border_套餐;

                    #endregion

                    stackPanel.Children.Add(itemsControl);

                    #endregion

                    #region 将stackpanel加入到item中

                    //将StackPanel作为TabItem的内容
                    newItem1.Content = new ScrollViewer
                    {
                        VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                        HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                        Content = stackPanel
                    };

                    #endregion

                    //将item加入到tab
                    this.Tab1.Items.Add(newItem1);

                    //用于切换对应的版本
                    if (this.s_fun_id?.Equals(item) == true)
                    {
                        _verdesc_target = newItem1;
                    }
                }

                //切换对应的版本
                if (_verdesc_target != null)
                {
                    try
                    {
                        this.Tab1.SelectedItem = _verdesc_target;
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"未知异常,{ex.Message}");
            }
        }

        //用户切换tab
        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                System.Windows.Controls.TabItem tabitem = sender as System.Windows.Controls.TabItem;
                if (!string.IsNullOrEmpty(tabitem.Tag.ToString()))
                {
                    WeakReferenceMessenger.Default.Send(new ProductPageSwitchNameMessage() { Name = tabitem.Tag.ToString() });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"未知异常,{ex.Message}");
            }
        }

        //用户选择套餐
        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Border targetControl = sender as Border;

                /*try
                {
                    if (targetControl != null)
                    {
                        // Create a diagonal linear gradient with four stops.
                        LinearGradientBrush myLinearGradientBrush =
                            new LinearGradientBrush();
                        myLinearGradientBrush.StartPoint = new System.Windows.Point(0, 0.5);
                        myLinearGradientBrush.EndPoint = new Point(1.0, 0.5);
                        myLinearGradientBrush.GradientStops.Add(
                            new GradientStop(Color.FromRgb(243, 214, 157), 0.0));
                        myLinearGradientBrush.GradientStops.Add(
                        new GradientStop(Color.FromRgb(251, 231, 194), 1.0));

                        targetControl.Background = myLinearGradientBrush;
                    }
                }
                catch
                {
                }*/


                if (targetControl.Tag != null)
                {
                    WeakReferenceMessenger.Default.Send(new ProductPageSelectedMessage() { Id = targetControl.Tag.ToString() });
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"未知异常,{ex.Message}");
            }
        }

        //查询功能对应的版本描述
        async void GetVersion4ID(string id)
        {
#if EXTERNAL
            try
            {
                string s_json_desc = await AJTDatabaseOperation.GetModelIDVersionDesc(id);
                if (!string.IsNullOrEmpty(s_json_desc))
                {
                    DataTable myJObject = JsonConvert.DeserializeObject<DataTable>(s_json_desc);

                    if (myJObject != null && myJObject.Rows.Count > 0)
                    {
                        this.s_fun_id = myJObject.Rows[0]["version_desc"].ToString();
                    }
                }
            }
            catch
            {
            }
#endif
        }
    }
}