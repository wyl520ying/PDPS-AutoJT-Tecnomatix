





using AutoJTL.SDK.Strandard.Response;
using System;
using System.Text.RegularExpressions;
using System.Windows;


namespace AutoJTLicensingTool.PageModel
{
  public sealed class ProductItemShowModel
  {
    public ProductItemShowModel(Product product)
    {
      套餐ID = product.套餐ID;
      套餐描述 = product.套餐描述;
      套餐时长 = product.套餐时长;
      实付价格 = product.实付价格;
      原价 = product.原价;
      新人专享价 = product.新人专享价;
      显示新人专享价 = Visibility.Collapsed;
      显示新人专享价符号 = Visibility.Visible;
      显示实付价 = Visibility.Collapsed;
      显示原价 = Visibility.Collapsed;
      显示选中符号 = Visibility.Hidden;
      if (!string.IsNullOrEmpty(product.新人专享价))
      {
        显示新人专享价 = Visibility.Visible;
        显示新人专享价符号 = Visibility.Visible;
        标头 = "超值";
        try
        {
          if (套餐时长.IndexOf("天") != -1)
          {
            int result1;
            double result2;
            if (!int.TryParse(Regex.Replace(套餐时长, "[\\u4e00-\\u9fa5]", ""), out result1) || !double.TryParse(新人专享价, out result2))
              return;
            每天价格 = Math.Round(result2 / result1, 2).ToString();
          }
          else if (套餐时长.IndexOf("月") != -1)
          {
            int result3;
            double result4;
            if (!int.TryParse(Regex.Replace(套餐时长, "[\\u4e00-\\u9fa5]", ""), out result3) || !double.TryParse(新人专享价, out result4))
              return;
            每天价格 = Math.Round(result4 / result3 / 31.0, 2).ToString();
          }
          else if (套餐时长.IndexOf("年") != -1)
          {
            int result5;
            double result6;
            if (!int.TryParse(Regex.Replace(套餐时长, "[\\u4e00-\\u9fa5]", ""), out result5) || !double.TryParse(新人专享价, out result6))
              return;
            每天价格 = Math.Round(result6 / result5 / 366.0, 2).ToString();
          }
          else
            每天价格 = "0.01";
        }
        catch
        {
        }
      }
      else
      {
        显示实付价 = Visibility.Visible;
        显示原价 = Visibility.Visible;
        标头 = "超值";
        try
        {
          if (套餐时长.IndexOf("天") != -1)
          {
            int result7;
            double result8;
            if (int.TryParse(Regex.Replace(套餐时长, "[\\u4e00-\\u9fa5]", ""), out result7) && double.TryParse(实付价格, out result8))
              每天价格 = Math.Round(result8 / result7, 2).ToString();
          }
          else if (套餐时长.IndexOf("月") != -1)
          {
            int result9;
            double result10;
            if (int.TryParse(Regex.Replace(套餐时长, "[\\u4e00-\\u9fa5]", ""), out result9) && double.TryParse(实付价格, out result10))
              每天价格 = Math.Round(result10 / result9 / 31.0, 2).ToString();
          }
          else if (套餐时长.IndexOf("年") != -1)
          {
            int result11;
            double result12;
            if (int.TryParse(Regex.Replace(套餐时长, "[\\u4e00-\\u9fa5]", ""), out result11) && double.TryParse(实付价格, out result12))
              每天价格 = Math.Round(result12 / result11 / 366.0, 2).ToString();
          }
          else
            每天价格 = "0.01";
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
}
