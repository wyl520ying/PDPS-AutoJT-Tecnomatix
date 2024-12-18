





using AutoJTL.SDK.Strandard.Response;
using System;


namespace AutoJTLicensingTool.PageModel
{
  public class PayShowModel
  {
    public PayShowModel(Product product)
    {
      描述 = product.套餐描述;
      时长 = product.套餐时长;
      if (!string.IsNullOrEmpty(product.新人专享价))
      {
        实付 = product.新人专享价;
        try
        {
          优惠 = (Convert.ToDecimal(product.原价) - Convert.ToDecimal(product.新人专享价)).ToString();
        }
        catch
        {
        }
      }
      else
      {
        实付 = product.实付价格;
        try
        {
          优惠 = (Convert.ToDecimal(product.原价) - Convert.ToDecimal(product.实付价格)).ToString();
        }
        catch
        {
        }
      }
    }

    public string 描述 { get; set; }

    public string 时长 { get; set; }

    public string 实付 { get; set; }

    public string 优惠 { get; set; }
  }
}
