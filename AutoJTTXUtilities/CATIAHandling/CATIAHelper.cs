using AutoJTMathUtilities;
using AutoJTTXUtilities.PathHandling;
using HybridShapeTypeLib;
using INFITF;
using MECMOD;
using ProductStructureTypeLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AutoJTTXUtilities.CATIAHandling
{
    public struct ProductCollection
    {
        public string Name { get; set; }
        public Product iProduct { get; set; }

        public AJTMatrix iPosition { get; set; }
    }
    public class CATIAHelper
    {
        #region Properties

        //单例模式之Lazy
        private static readonly Lazy<CATIAHelper> lazy = new Lazy<CATIAHelper>(() => new CATIAHelper());



        //catia application
        private INFITF.Application cATIA = null;
        public INFITF.Application CATIA { get => cATIA; set => cATIA = value; }


        //三大件
        private ProductStructureTypeLib.ProductDocument rOOTProductDocument;
        private ProductStructureTypeLib.Product rootProduct;
        private INFITF.Selection selectoion_1;
        //三大件
        public ProductDocument ROOTProductDocument { get => rOOTProductDocument; set => rOOTProductDocument = value; }
        public Product RootProduct { get => rootProduct; set => rootProduct = value; }
        public INFITF.Selection Selectoion_1 { get => selectoion_1; set => selectoion_1 = value; }


        //root product doc fullpath
        public string m_root_product_doc_fullpath { get; private set; }
        //root product doc name
        public string m_root_product_doc_name { get; private set; }




        //所有的节点的集合
        int m_IcountAll;
        //所有得product集合
        List<ProductCollection> productCollection;
        //所有得product集合用于恢复显示
        List<Product> productList_all;
        //所有得product 重复项 list集合
        List<string> productNameList;
        //所有的零件和组件的  重复项 list集合
        List<string> productNameList_all;
        //所有得错误集合
        List<string> errorList;

        //读取得cgr数据结构
        Compound_Part m_Compound_Part;

        #endregion



        #region plan part

        PartDocument partDocument;
        Part part1;
        HybridShapeFactory hybridShapeFactory1;

        public PartDocument m_PartDocument { get => partDocument; set => partDocument = value; }
        public Part m_Part1 { get => part1; set => part1 = value; }
        public HybridShapeFactory m_HybridShapeFactory1 { get => hybridShapeFactory1; set => hybridShapeFactory1 = value; }

        //初始化 part 三大件
        public Task InitializeCATIA_PartDocument(bool isCreatePart = false)
        {
            return Task.Run(() =>
            {           
            if (this.CATIA == null)
            {
                return;
            }

            //创建part
            if (isCreatePart)
            {
                Documents documents1 = this.cATIA.Documents;
                this.m_PartDocument = (PartDocument)documents1.Add("Part");
                this.m_root_product_doc_name = Path.GetFileNameWithoutExtension(m_PartDocument.get_Name());
                this.m_Part1 = m_PartDocument.Part;
                this.m_HybridShapeFactory1 = (HybridShapeFactory)m_Part1.HybridShapeFactory;
                this.Selectoion_1 = m_PartDocument.Selection;
            }
            else
            {
                try
                {
                    this.m_PartDocument = (PartDocument)this.cATIA.ActiveDocument;
                    this.m_root_product_doc_name = Path.GetFileNameWithoutExtension(m_PartDocument.get_Name());
                    this.m_Part1 = m_PartDocument.Part;
                    this.m_HybridShapeFactory1 = (HybridShapeFactory)m_Part1.HybridShapeFactory;
                    this.Selectoion_1 = m_PartDocument.Selection;
                }
                catch
                {
                    throw new Exception("请打开Part");
                }
            }
            });
        }


        #endregion

        #region Constructor

        //plan B
        public CATIAHelper(bool flag1 = false)
        {
            try
            {
                this.CATIA = System.Runtime.InteropServices.Marshal.GetActiveObject("CATIA.Application") as INFITF.Application;
            }
            catch
            {
                try
                {
                    this.CATIA = System.Runtime.InteropServices.Marshal.GetActiveObject("DELMIA.Application") as INFITF.Application;
                }
                catch
                {
                    throw new Exception("请打开CATIA或DELMIA");
                }
            }
        }
        public CATIAHelper()
        {
            try
            {
                //所有得product集合
                this.productCollection = null;
                this.productList_all = null;
                //所有得product 重复项 list集合
                this.productNameList = null;
                this.productNameList_all = null;
                //所有得错误集合
                this.errorList = null;

                //读取得cgr数据结构
                this.m_Compound_Part = null;
                this.m_IcountAll = 0;

                this.CATIA = System.Runtime.InteropServices.Marshal.GetActiveObject("CATIA.Application") as INFITF.Application;                              
            }
            catch
            {
                try
                {
                    this.CATIA = System.Runtime.InteropServices.Marshal.GetActiveObject("DELMIA.Application") as INFITF.Application;
                }
                catch 
                {
                    throw new Exception("请打开CATIA或DELMIA");
                    //Type oType = System.Type.GetTypeFromProgID("CATIA.Application");
                    //CATIA = (INFITF.Application)Activator.CreateInstance(oType);
                    //CATIA.Visible = true;
                }
            }
        }

        //单实例
        private static CATIAHelper GetInstance
        {
            get { return lazy.Value; }
        }


        #endregion

        #region public method

        /// <summary>
        /// 初始化catia ROOTProductDocument  RootProduct Selectoion_1
        /// doc没问题就进入设计模式, 装配模式
        /// </summary>
        public void InitializeCATIA_ProductDocument()
        {
            if (this.CATIA == null)
            {
                return;
            }

            try
            {
                this.ROOTProductDocument = this.CATIA.ActiveDocument as ProductStructureTypeLib.ProductDocument;
                if (this.ROOTProductDocument == null)
                {
                    m_root_product_doc_fullpath = null;
                    m_root_product_doc_name = null;
                    Selectoion_1 = null;
                    RootProduct = null;

                    return;
                }
                this.RootProduct = this.ROOTProductDocument.Product;

                try
                {
                    this.m_root_product_doc_fullpath = System.IO.Path.GetDirectoryName(this.ROOTProductDocument.FullName);

                    //this.m_root_product_doc_name = Path.GetFileNameWithoutExtension(m_root_product_doc_fullpath);
                    this.m_root_product_doc_name = AJTPath.GetFileNameNoNamingRules(this.RootProduct.get_PartNumber(), true);
                }
                catch
                {

                }
            }
            catch { }

            if (this.RootProduct == null)
            {
                return;
            }

            try
            {
                this.RootProduct.Update();

                this.Selectoion_1 = this.ROOTProductDocument.Selection;

                CATIAHelper.StartAssembly(this.CATIA, this.RootProduct, this.Selectoion_1);
                CATIAHelper.Apply_DESIN_MODE(this.RootProduct);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #region 导出cgr

        public Compound_Part Convert2CGR(string ouputPath, out int icount, out List<string> error, out int _icountAll)
        {
            //所有的节点对象数量
            _icountAll = 0;
            //导出得数量
            icount = 0;
            //错误得对象
            error = null;

            //检查文件夹是否存在
            try
            {
                if (!System.IO.Directory.Exists(ouputPath))
                {
                    System.IO.Directory.CreateDirectory(ouputPath);
                }
            }
            catch
            {
                return null;
            }

            try
            {
                productCollection = new List<ProductCollection>();
                productList_all = new List<Product>();
                productNameList = new List<string>();
                productNameList_all = new List<string>();
                errorList = new List<string>();

                //root comp
                this.m_Compound_Part = new Compound_Part()
                {
                    m_Name = this.m_root_product_doc_name,
                    m_NameWithoutExtension = Path.GetFileNameWithoutExtension(this.m_root_product_doc_name),
                    m_Matrix = new AJTMatrix()
                };

                //避免弹出覆盖对话框
                this.CATIA.DisplayFileAlerts = false;

                //递归Component, 记录location和装配结构
                this.SetComponent(this.RootProduct, this.m_Compound_Part);

                this.SetComponent(this.RootProduct);

                //遍历product list 导出cgr
                foreach (ProductCollection item in this.productCollection)
                {
                    try
                    {
                        //parent组件相对于世界的逆矩阵
                        //AJTMatrix aFrame_Inverse = AJTMatrix.Inverse(item.iPosition);                     
                        //object[] array = AJTMatrix.ToDoubleArrayCAT(aFrame_Inverse);

                        //检查姿态 
                        //item.iProduct.Position.SetComponents(array);

                        //单独显示
                        this.selectoion_1.Clear();
                        this.selectoion_1.Add(item.iProduct);
                        this.selectoion_1.VisProperties.SetShow(CatVisPropertyShow.catVisPropertyShowAttr);

                        string partNumber = item.Name;
                        //cgr文件名
                        partNumber = System.IO.Path.Combine(ouputPath, partNumber + ".cgr");

                        //删除已经存在得文件
                        try
                        {
                            if (System.IO.File.Exists(partNumber))
                            {
                                try
                                {
                                    System.IO.File.Delete(partNumber);
                                }
                                catch
                                {
                                }
                            }
                        }
                        catch
                        {
                        }


                        //导出cgr
                        try
                        {
                            this.rOOTProductDocument.ExportData(ref partNumber, "cgr");
                            icount++;
                        }
                        catch
                        {
                            //错误列表
                            this.errorList.Add(item.Name);
                        }

                        //if (this.SaveAsCGRMethod(item.iProduct, partNumber))
                        //{
                        //    icount++;
                        //}
                        //else
                        //{
                        //    //错误列表
                        //    this.errorList.Add(item.Name);
                        //}

                    }
                    catch
                    {
                        //错误列表
                        this.errorList.Add(item.Name);
                    }
                    finally
                    {
                        //隐藏已经的product
                        selectoion_1.VisProperties.SetShow(CatVisPropertyShow.catVisPropertyNoShowAttr);
                    }
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                //if (productList_all != null)
                //{
                //    //恢复显示
                //    foreach (Product item in this.productList_all)
                //    {
                //        try
                //        {
                //            SetProeuctDisplayStuats(this.cATIA, item, this.Selectoion_1, CatVisPropertyShow.catVisPropertyShowAttr);
                //        }
                //        catch
                //        {
                //        }
                //    }
                //}

                //显示所有零件
                //this.SearchPartMethod(out int _izxc, true);
                try
                {
                    this.ROOTProductDocument.Close();
                }
                catch
                {
                }
            }

            _icountAll = this.m_IcountAll;
            error = this.errorList;
            return this.m_Compound_Part;
        }

        //搜索所有part
        public bool SearchPartMethod(out int icount, bool isShow = false)
        {
            bool bl989 = false;
            icount = 0;

            if (this.selectoion_1 == null)
            {
                return false;
            }

            this.selectoion_1.Clear();
            INFITF.Selection selection2 = selectoion_1;
            //显示所有的装配体, 防止转出的jt为空
            string searchText_product = "((((((((((CATProductSearch.Assembly + CATLndSearch.Assembly) + CATHvaSearch.Assembly) + CATAsmSearch.Assembly) + CATWguSearch.Assembly) + CATHvuSearch.Assembly) + CATSsrSearch.Assembly) + CATPcsSearch.Assembly) + CATTbuSearch.Assembly) + CATPslSearch.Assembly) + CATPiuSearch.Assembly),all";
            selection2.Search(ref searchText_product);
            //没有装配体
            if (selectoion_1.Count2 != 0)
            {
                //show所有零件
                this.selectoion_1.VisProperties.SetShow(CatVisPropertyShow.catVisPropertyShowAttr);
            }


            //搜索目标
            string searchText = "(((((((((((((((CATProductSearch.Part + CATStFreeStyleSearch.PartFeature) + CATLndSearch.Part) + CATHvaSearch.Part) + CATAsmSearch.Part) + CATWguSearch.Part) + CATHvuSearch.Part) + CATPrtSearch.PartFeature) + CATSsrSearch.Part) + CATGmoSearch.PartFeature) + CATSpdSearch.PartFeature) + CATPcsSearch.Part) + CATTbuSearch.Part) + CATPslSearch.Part) + CATPiuSearch.Part)),all";// & Visibility=Shown

            this.selectoion_1.Clear();

            selection2 = selectoion_1;
            selection2.Search(ref searchText);

            //没有零件
            if (selectoion_1.Count2 == 0)
            {
                return false;
            }
            else
            {
                if (isShow)
                {
                    //show所有零件
                    this.selectoion_1.VisProperties.SetShow(CatVisPropertyShow.catVisPropertyShowAttr);
                }
                else
                {
                    //隐藏所有零件
                    this.selectoion_1.VisProperties.SetShow(CatVisPropertyShow.catVisPropertyNoShowAttr);
                }
            }

            icount = selectoion_1.Count2;
            bl989 = true;

            return bl989;
        }

        //递归遍历所有part
        void SetComponent(Product rootProduct, Compound_Part compound)
        {
            //parent products
            Products products = rootProduct.Products;
            //遍历
            foreach (Product item in products)
            {
                //零件号
                string partNumber = string.Empty;
                try
                {
                    partNumber = item.get_PartNumber().Trim();

                    partNumber = AJTPath.GetFileNameNoNamingRules(partNumber, true);
                }
                catch
                {
                    //掉链接
                    continue;
                }

                try
                {
                    //组件
                    if (item.Products.Count > 0 && !item.HasAMasterShapeRepresentation())
                    {
                        //更新所有的组件零件数量
                        this.m_IcountAll++;

                        //加入装配体集合
                        Compound_Part compound_Part1 = new Compound_Part()
                        {
                            m_Name = partNumber + ".cgr",
                            m_NameWithoutExtension = partNumber,
                            m_Matrix = new AJTMatrix()
                        };
                        compound.m_List.Add(compound_Part1);



                        //递归
                        this.SetComponent(item, compound_Part1);
                    }
                    //零件
                    else
                    {
                        //更新所有的组件零件数量
                        this.m_IcountAll++;

                        //获取姿态, 相对于世界坐标系
                        AJTMatrix pos12 = FindAbsolutePoseOfComponent(item, out AJTMatrix aJTMatrix_parent_first);

                        //加入装配体集合
                        Part_Prototype part_Prototype1 = new Part_Prototype()
                        {
                            m_Name = partNumber + ".cgr",
                            m_NameWithoutExtension = partNumber,
                            m_Matrix = pos12
                        };
                        compound.m_List.Add(part_Prototype1);


                        //全部的poroduct集合
                        this.productList_all.Add(item);

                        //排除重复项
                        if (!this.productNameList.Contains(partNumber))
                        {
                            //加入到重复项集合
                            this.productNameList.Add(partNumber);
                            //加入到product 集合
                            this.productCollection.Add(new ProductCollection() { Name = partNumber, iProduct = item, iPosition = aJTMatrix_parent_first });
                        }
                    }
                }
                catch
                {
                    //加入到错误集合
                    this.errorList.Add(partNumber);
                    continue;
                }
            }
        }
        void SetComponent(Product rootProduct)
        {
            //parent products
            Products products = rootProduct.Products;
            //遍历
            foreach (Product item in products)
            {
                try
                {
                    //零件号
                    string partNumber = string.Empty;
                    try
                    {
                        partNumber = item.get_PartNumber().Trim();

                        partNumber = AJTPath.GetFileNameNoNamingRules(partNumber, true);
                    }
                    catch
                    {
                        //掉链接
                        continue;
                    }

                    try
                    {
                        if (!this.productNameList_all.Contains(partNumber))
                        {
                            item.Position.SetComponents(AJTMatrix.ToDoubleArrayCAT(new AJTMatrix()));
                            this.productNameList_all.Add(partNumber);
                        }
                    }
                    catch
                    {

                    }

                    //组件
                    if (item.Products.Count > 0 && !item.HasAMasterShapeRepresentation())
                    {
                        //递归
                        this.SetComponent(item);
                    }
                }
                catch
                {
                    continue;
                }
            }
        }

        //另存为cgr
        bool SaveAsCGRMethod(Product product32, string cgrfullName)
        {
            bool result = false;

            try
            {
                //待转换的document
                Document convertDocument = product32.ReferenceProduct.Parent as Document;
                convertDocument.ExportData(ref cgrfullName, "cgr");

                result = true;
            }
            catch
            {
                result = false;
            }

            return result;
        }


        #endregion



        //创建小球
        public static bool CreateShape(CATIAHelper cATIAHelper,double x,double y ,double z,double dia,string name)
        {
            bool result = false;

            try
            {
                //创建point
                HybridShapePointCoord hybridShapePointCoord = cATIAHelper.m_HybridShapeFactory1.AddNewPointCoord(x, y, z);

                //定义工作对象
                cATIAHelper.m_Part1.InWorkObject = hybridShapePointCoord;

                //创建ref
                Reference reference1 = cATIAHelper.m_Part1.CreateReferenceFromObject(hybridShapePointCoord);


                //创建shere
                var hybridShapeSphere1 = cATIAHelper.m_HybridShapeFactory1.AddNewSphere(reference1, null, dia / 2, -45.0, -45.0, 0, 180.0);
                //整个球体
                hybridShapeSphere1.Limitation = 1;

                //name
                hybridShapeSphere1.set_Name(name);

                //插入body
                Body body1 = cATIAHelper.m_Part1.MainBody;
                body1.InsertHybridShape(hybridShapeSphere1);

                //定义工作对象
                cATIAHelper.m_Part1.InWorkObject = hybridShapeSphere1;
                result = true;
            }
            catch (Exception)
            {
                throw;
            }
           
            return result;
        }

        #endregion

        #region static method

        #region 设计模式 Workbench

        /// <summary>
        /// 进入装配空间
        /// </summary>
        public static void StartAssembly(INFITF.Application CATIA, Product oRootProduct, INFITF.Selection oSel)
        {
            try
            {
                string strWorkbenchID = CATIA.Application.GetWorkbenchId();

                string WorkbenchId = "Assembly";
                if (strWorkbenchID != WorkbenchId)
                {
                    oSel.Clear();
                    oSel.Add(oRootProduct);

                    CATIA.Application.StartWorkbench("Assembly");
                }
            }
            catch { }
        }

        /// <summary>
        /// 进入设计模式
        /// </summary>
        public static void Apply_DESIN_MODE(Product product1)
        {
            product1.ApplyWorkMode(CatWorkModeType.DESIGN_MODE);
        }

        #endregion

        #region 检查catia进程

        //查找caitia进程
        public static void GetCNEXTProcess(out System.Diagnostics.Process FirstCATapp)
        {
            FirstCATapp = null;

            System.Diagnostics.Process[] CATapp = System.Diagnostics.Process.GetProcessesByName("CNEXT");//查找catia进程            

            if (CATapp.Length == 1)
            {
                FirstCATapp = CATapp.FirstOrDefault();
                return;
            }

            int i = 0;
            foreach (System.Diagnostics.Process item in CATapp)
            {
                if (i == 0)
                {
                    i = item.Id;
                }
                else if (i > item.Id)
                {
                    i = item.Id;
                    FirstCATapp = item;
                }
            }
        }

        /// <summary>
        /// 检查catia进程数量
        /// </summary>
        /// <returns></returns>
        public static bool GetCNEXTProcessNum(out int catProcessNum)
        {
            catProcessNum = 0;
            try
            {
                System.Diagnostics.Process[] CATapp1 = System.Diagnostics.Process.GetProcessesByName("CNEXT");
                catProcessNum = CATapp1.Length;
                if (CATapp1.Length == 1 || CATapp1.Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region CAT文档

        /// <summary>
        /// 判断当前Product是否是 终节点
        /// 终节点返回true，否则返回false
        /// </summary>
        /// <param name="oProduct"></param>
        /// <returns></returns>
        public static bool IsLeaf(Product oProduct)
        {
            Products oProducts;
            oProducts = oProduct.Products;
            return oProducts.Count == 0;
        }

        /// <summary>
        /// 检查当前打开的catia文档类型
        /// </summary>
        /// <returns></returns>
        public static string CheckCATDocument(INFITF.Application CATIA_1)
        {
            if (CATIA_1 == null)
            {
                return string.Empty;
            }

            Document document;

            try
            {
                document = CATIA_1.ActiveDocument;
            }
            catch
            {
                return string.Empty;
            }

            string[] ActiveDocType = CATIA_1.ActiveDocument.FullName.Split('.');
            string choice = ActiveDocType[ActiveDocType.Length - 1];

            if (document != null)
            {
                return choice;
            }
            else
            {
                return string.Empty;
            }
        }

        public static void SetProeuctDisplayStuats(INFITF.Application CATIA_1, AnyObject obj, CatVisPropertyShow catVisPropertyShow)
        {
            try
            {
                if (obj == null || CATIA_1 == null )
                {
                    throw new ArgumentNullException("参数异常");
                }

                Document productDocument1 = CATIA_1.ActiveDocument;
                INFITF.Selection selection1 = productDocument1.Selection;
                INFITF.VisPropertySet visPropertySet1 = selection1.VisProperties;

                selection1.Clear();
                selection1.Add(obj);
                visPropertySet1.SetShow(catVisPropertyShow);
                selection1.Clear();
            }
            catch(Exception)
            {
                throw;
            }
        }

        //隐藏平面
        public static void HiddenPlaneElement(CATIAHelper cATIAHelper)
        {
            try
            {
                INFITF.VisPropertySet visProperties = cATIAHelper.selectoion_1.VisProperties;
                INFITF.Selection selection2 = cATIAHelper.selectoion_1;
                selection2.Search("CatPrtSearch.Plane,All");
                visProperties.SetShow(CatVisPropertyShow.catVisPropertyNoShowAttr);
                selection2.Clear();
            }
            catch (Exception)
            {
                throw;
            }
        }
        //隐藏平面
        public static void HiddenConstructElement(INFITF.Selection selectoion_1)
        {
            try
            {
                INFITF.VisPropertySet visProperties = selectoion_1.VisProperties;
                INFITF.Selection selection2 = selectoion_1;
                selection2.Search("CatPrtSearch.Plane,All");
                visProperties.SetShow(CatVisPropertyShow.catVisPropertyNoShowAttr);
                selection2.Clear();


                INFITF.Selection selection4 = selectoion_1;
                selection4.Search("CatPrtSearch.Point,All");
                visProperties.SetShow(CatVisPropertyShow.catVisPropertyNoShowAttr);
                selection4.Clear();                
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion


        //添加frame
        public  static void CreateCustFrame(Part part1,object[] m)
        {
            CATIAHelper.CreateCustFrame(part1, CATIAHelper.ConvertToDoubleArray(m));
        }
        //添加frame
        public static void CreateCustFrame(Part part1, double[] m)
        {
            try
            {
                //创建一共axis
                AxisSystems axisSystems1 = part1.AxisSystems;
                AxisSystem axisSystem1 = axisSystems1.Add();
                axisSystem1.OriginType = CATAxisSystemOriginType.catAxisSystemOriginByCoordinates;

                axisSystem1.PutOrigin(new object[] { m[9], m[10], m[11] });

                //axisSystem1.XAxisType = CATAxisSystemAxisType.catAxisSystemAxisByCoordinates;
                //axisSystem1.YAxisType = CATAxisSystemAxisType.catAxisSystemAxisByCoordinates;
                //axisSystem1.ZAxisType = CATAxisSystemAxisType.catAxisSystemAxisByCoordinates;

                //axisSystem1.PutXAxis(new object[] { m[0], m[1], m[2] });
                //axisSystem1.PutYAxis(new object[] { m[3], m[4], m[5] });
                //axisSystem1.PutZAxis(new object[] { m[6], m[7], m[8] });

                //axisSystem1.PutVectors(new object[] { m[0], m[1], m[2] }, new object[] { m[3], m[4], m[5] });


                axisSystem1.XAxisDirection = CreateNewPoint4Coord(part1, new double[] { m[0], m[1], m[2] });
                axisSystem1.YAxisDirection = CreateNewPoint4Coord(part1, new double[] { m[3], m[4], m[5] });
                axisSystem1.ZAxisDirection = CreateNewPoint4Coord(part1, new double[] { m[6], m[7], m[8] });

                axisSystem1.IsCurrent = false;
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        //object转double
        public static double[] ConvertToDoubleArray(object[] obj)
        {
            if (obj == null || obj.Length==0)
            {
                return null;
            }

            double[] result = new double[obj.Length];

            try
            {
                for (int i = 0; i < obj.Length; i++)
                {
                    result[i] = Convert.ToDouble(obj[i]);
                }
            }
            catch 
            {
                return null;
            }         

            return result;
        }

        //创建point
        private static Reference CreateNewPoint4Coord(Part part1, double[] coord)
        {
            HybridShapeFactory hybridShapeFactory = (HybridShapeFactory)part1.HybridShapeFactory;
            HybridShapePointCoord hybridShapePointCoord = hybridShapeFactory.AddNewPointCoord(coord[0], coord[1], coord[2]);
            Body body = part1.MainBody;
            body.InsertHybridShape(hybridShapePointCoord);
            part1.InWorkObject = hybridShapePointCoord;            

            return part1.CreateReferenceFromGeometry(hybridShapePointCoord);
        }

        #endregion


        /// <summary>
        /// 求组件的绝对位姿
        /// </summary>
        /// <param name="Product1"></param>
        /// <returns></returns>
        public static AJTMatrix FindAbsolutePoseOfComponent(Product Product1, out AJTMatrix first_parent_matrix)
        {
            //第一层父级相对于世界坐标系的姿态
            first_parent_matrix = null;

            object[] pos1 = new object[12];
            //一维position
            Product1.Position.GetComponents(pos1);

            //一维position转齐次矩阵
            AJTMatrix pos2 = new AJTMatrix((double)pos1[0], (double)pos1[3], (double)pos1[6], (double)pos1[9],
                                           (double)pos1[1], (double)pos1[4], (double)pos1[7], (double)pos1[10],
                                           (double)pos1[2], (double)pos1[5], (double)pos1[8], (double)pos1[11]);

            //直接父级
            Products comp = (Products)Product1.Parent;
            //直接父级
            Product comp_parent = (Product)comp.Parent;

            //判断是否是最顶层
            while (GetTypeName(comp_parent) == "Product")
            {
                comp_parent.Position.GetComponents(pos1);
                pos2 = AJTMatrix.Multiply(new AJTMatrix((double)pos1[0], (double)pos1[3], (double)pos1[6], (double)pos1[9],
                                                        (double)pos1[1], (double)pos1[4], (double)pos1[7], (double)pos1[10],
                                                        (double)pos1[2], (double)pos1[5], (double)pos1[8], (double)pos1[11]),
                                                        pos2);
                //赋值直接父级
                if (first_parent_matrix == null)
                {
                    first_parent_matrix = new AJTMatrix((double)pos1[0], (double)pos1[3], (double)pos1[6], (double)pos1[9],
                                           (double)pos1[1], (double)pos1[4], (double)pos1[7], (double)pos1[10],
                                           (double)pos1[2], (double)pos1[5], (double)pos1[8], (double)pos1[11]);
                }
                else
                {
                    first_parent_matrix = AJTMatrix.Multiply(new AJTMatrix((double)pos1[0], (double)pos1[3], (double)pos1[6], (double)pos1[9],
                                                        (double)pos1[1], (double)pos1[4], (double)pos1[7], (double)pos1[10],
                                                        (double)pos1[2], (double)pos1[5], (double)pos1[8], (double)pos1[11]), first_parent_matrix);
                }

                try
                {
                    comp = (Products)comp_parent.Parent;
                    comp_parent = (Product)comp.Parent;
                }
                catch { comp_parent = null; }

            }
            return pos2;
        }

        /*
        /// <summary>
        /// 矩阵乘法
        /// </summary>
        /// <param name="M1"></param>
        /// <param name="M2"></param>
        /// <returns></returns>
        public static double[,] MatrixMultiplication(double[,] M1, double[,] M2)
        {
           
            double[,] MatrixProduct = MultiplyMatrix(M1, M2);
            return MatrixProduct;
        }

        ///   <summary> 
        ///   矩阵乘法 
        ///   </summary> 
        ///   <param   name= "MatrixEin "> </param> 
        ///   <param   name= "MatrixZwei "> </param> 
        public static double[,] MultiplyMatrix(double[,] MatrixEin, double[,] MatrixZwei)
        {
            double[,] MatrixResult = new double[MatrixEin.GetLength(0), MatrixZwei.GetLength(1)];
            for (int i = 0; i < MatrixEin.GetLength(0); i++)
            {
                for (int j = 0; j < MatrixZwei.GetLength(1); j++)
                {
                    for (int k = 0; k < MatrixEin.GetLength(1); k++)
                    {
                        MatrixResult[i, j] += MatrixEin[i, k] * MatrixZwei[k, j];
                    }
                }
            }
            return MatrixResult;
        }

        /// <summary>
        /// 矩阵的逆
        /// </summary>
        /// <param name="M"></param>
        /// <returns></returns>
        public static double[,] MatrixInverse(double[,] M)
        {
            double[,] InverseMatrix = Athwart(M);
            return InverseMatrix;
        }

        ///   <summary> 
        ///   矩阵的逆矩阵 
        ///   </summary> 
        ///   <param   name= "iMatrix "> </param> 
        public static double[,] Athwart(double[,] iMatrix)
        {
            int i = 0;
            int row = iMatrix.GetLength(0);
            double[,] MatrixZwei = new double[row, row * 2];
            double[,] iMatrixInv = new double[row, row];
            for (i = 0; i < row; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    MatrixZwei[i, j] = iMatrix[i, j];
                }
            }
            for (i = 0; i < row; i++)
            {
                for (int j = row; j < row * 2; j++)
                {
                    MatrixZwei[i, j] = 0;
                    if (i + row == j)
                        MatrixZwei[i, j] = 1;
                }
            }

            for (i = 0; i < row; i++)
            {
                if (MatrixZwei[i, i] != 0)
                {
                    double intTemp = MatrixZwei[i, i];
                    for (int j = 0; j < row * 2; j++)
                    {
                        MatrixZwei[i, j] = MatrixZwei[i, j] / intTemp;
                    }
                }
                for (int j = 0; j < row; j++)
                {
                    if (j == i)
                        continue;
                    double intTemp = MatrixZwei[j, i];
                    for (int k = 0; k < row * 2; k++)
                    {
                        MatrixZwei[j, k] = MatrixZwei[j, k] - MatrixZwei[i, k] * intTemp;
                    }
                }
            }

            for (i = 0; i < row; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    iMatrixInv[i, j] = MatrixZwei[i, j + row];
                }
            }
            return iMatrixInv;
        }
        
        /// <summary>
        /// 一维position转齐次矩阵
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static double[,] OneDimToHomogeneousMatrix(object[] pos)
        {
            double[,] HomogeneousMatrix = new double[4, 4];
            HomogeneousMatrix[0, 0] = (double)pos[0];
            HomogeneousMatrix[1, 0] = (double)pos[1];
            HomogeneousMatrix[2, 0] = (double)pos[2];
            HomogeneousMatrix[3, 0] = 0;
            HomogeneousMatrix[0, 1] = (double)pos[3];
            HomogeneousMatrix[1, 1] = (double)pos[4];
            HomogeneousMatrix[2, 1] = (double)pos[5];
            HomogeneousMatrix[3, 1] = 0;
            HomogeneousMatrix[0, 2] = (double)pos[6];
            HomogeneousMatrix[1, 2] = (double)pos[7];
            HomogeneousMatrix[2, 2] = (double)pos[8];
            HomogeneousMatrix[3, 2] = 0;
            HomogeneousMatrix[0, 3] = (double)pos[9];
            HomogeneousMatrix[1, 3] = (double)pos[10];
            HomogeneousMatrix[2, 3] = (double)pos[11];
            HomogeneousMatrix[3, 3] = 1;
            return HomogeneousMatrix;
        }
        /// <summary>
        /// 齐次矩阵转一维position数组
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public static double[] HomogeneousMatrixToOneDim(double[,] qc)
        {
            double[] pos = new double[12];
            pos[0] = (double)qc[0, 0];
            pos[1] = (double)qc[1, 0];
            pos[2] = (double)qc[2, 0];
            pos[3] = (double)qc[0, 1];
            pos[4] = (double)qc[1, 1];
            pos[5] = (double)qc[2, 1];
            pos[6] = (double)qc[0, 2];
            pos[7] = (double)qc[1, 2];
            pos[8] = (double)qc[2, 2];
            pos[9] = (double)qc[0, 3];
            pos[10] = (double)qc[1, 3];
            pos[11] = (double)qc[2, 3];
            return pos;
        }

        */

        /// <summary>
        /// VB中的TypeName
        /// </summary>
        /// <param name="iObejct"></param>
        /// <returns></returns>
        public static string GetTypeName(object iObejct)
        {
            bool flag = iObejct == null;
            string result = string.Empty;

            if (flag)
            {
                return result;
            }
            else
            {
                try
                {
                    result = Microsoft.VisualBasic.Information.TypeName(iObejct);
                }
                catch
                {
                    return result;
                }
            }

            return result;
        }
    }
}