using System;
using Tecnomatix.Engineering;

namespace AutoJTMathUtilities
{
    public class AJTMath
    {
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
        static double[,] MultiplyMatrix(double[,] MatrixEin, double[,] MatrixZwei)
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

        public static TxTransformation CreateTransformationByMatrix(double[] matix)
        {
            TxTransformation result = null;

            result = new TxTransformation(matix[0], matix[1], matix[2], matix[3],
                                          matix[4], matix[5], matix[6], matix[7],
                                          matix[8], matix[9], matix[10], matix[11]);

            return result;
        }
        public static double[] CreateMatrixByTransformation(TxTransformation txTransformation)
        {
            double[] result = null;

            result = new double[] { txTransformation[0,0], txTransformation[0, 1], txTransformation[0, 2], txTransformation[0, 3] ,
                                    txTransformation[1,0], txTransformation[1, 1], txTransformation[1, 2], txTransformation[1, 3] ,
                                    txTransformation[2,0], txTransformation[2, 1], txTransformation[2, 2], txTransformation[2, 3] };

            return result;
        }

        /// <summary>
        /// 转角度
        /// </summary>
        /// <param name="w"></param>
        /// <returns></returns>
        public static double Degree(double w)
        {
            return w * 57.295779513082323;
        }
        /// <summary>
        /// 转弧度
        /// </summary>
        /// <param name="w"></param>
        /// <returns></returns>
        public static double Radian(double w)
        {
            return w * 0.017453292519943295;
        }

        public const double PI = 3.1415926535;

        public const double PiDivOneEighty = 0.017453292519943295;

        public const double OneEightyDivPi = 57.295779513082323;


        public static string ToChar(int num)
        {  //转化为26进制
            string s = "";
            int n = num, r = num, m;
            bool flag = true;

            while (flag)
            {
                //循环求余数
                n = r;
                m = n % 26;
                r = n / 26;

                //判断是否倍26整除
                if (m == 0)
                {
                    m = 26;
                    r = r - 1;
                }
                //将26进制余数化为字符表示
                s = (char)(m + 64) + s;
                if (r == 0)
                    break;
            }
            return s;
        }

        public static bool ToNum(string s,out int num)
        {
            //转化为10进制
            num = 0;

            s = s.ToUpper();

            bool result = false;
            try
            {               
                for (int i = s.Length - 1, j = 0; i >= 0; i--, j++)
                {
                    num = (int)(num + (s[i] - 64) * Math.Pow(26, j));
                    //大写字母A的ascall码为65，将A转化为1,B转化为2....
                }

                result = true;
            }
            catch 
            {
                result = false;
            }
            
            return result;
        }
    }
}
