using System;
using Tecnomatix.Engineering;

namespace AutoJTMathUtilities
{
    public class MathUtilities
    {
        /// <summary>
        /// Returns the absolute value of a double
        /// </summary>
        /// <param name="value">The input value</param>
        /// <returns>The absolute value of the input parameter</returns>
        public static double Abs(double value)
        {
            return Math.Abs(value);
        }

        /// <summary>
        /// Calculate the length of a 3D vector
        /// </summary>
        /// <param name="x">The x value</param>
        /// <param name="y">The y value</param>
        /// <param name="z">The z value</param>
        /// <returns>The length of the vector</returns>
        public static double Length(double x, double y, double z)
        {
            double xp = Math.Pow(x, 2.0);
            double yp = Math.Pow(y, 2.0);
            double zp = Math.Pow(z, 2.0);
            return Math.Sqrt(xp + yp + zp);
        }

        /// <summary>
        /// Converts an angle from radians to degrees
        /// </summary>
        /// <param name="radians">Value in radians</param>
        /// <returns>The value in degrees</returns>
        public static double Rad2Deg(double radians)
        {
            return (radians / Math.PI) * 180.0;
        }

        /// <summary>
        /// Converts an angle from degrees to radians
        /// </summary>
        /// <param name="degrees">Value in degrees</param>
        /// <returns>The value in radians</returns>
        public static double Deg2Rad(double degrees)
        {
            return (degrees / 180.0) * Math.PI;
        }

        /// <summary>
        /// Rounds a double value to the nearest integral
        /// </summary>
        /// <param name="value">The value that has to be rounded</param>
        /// <returns></returns>
        public static double Round(double value)
        {
            return Math.Round(value, 3);
        }

        /// <summary>
        /// Create a TxTransformation object in RPY representation
        /// </summary>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="z">The z coordinate</param>
        /// <param name="rx">The rx rotation</param>
        /// <param name="ry">The ry rotation</param>
        /// <param name="rz">The rz rotation</param>
        /// <param name="convertToRadians">Convert the rotation values to radians</param>
        /// <returns>The TxTransformation object</returns>
        public static TxTransformation CreateRpyTransformation(double x, double y, double z, double rx, double ry, double rz, bool convertToRadians = false)
        {
            TxVector translation = CreateVector(x, y, z, false);
            TxVector rotation = CreateVector(rx, ry, rz, true);
            return new TxTransformation(translation, rotation, TxTransformation.TxRotationType.RPY_XYZ);
        }
        public static TxTransformation CreateRpyTransformation2(double x, double y, double z, double rx, double ry, double rz, bool convertToRadians = false)
        {
            TxVector translation = CreateVector(x, y, z, false);
            TxVector rotation = CreateVector(rx, ry, rz, false);
            return new TxTransformation(translation, rotation, TxTransformation.TxRotationType.RPY_XYZ);
        }
        /// <summary>
        /// Create a TxVector object
        /// </summary>
        /// <param name="v1">The first value</param>
        /// <param name="v2">The second value</param>
        /// <param name="v3">The third value</param>
        /// <param name="convertToRadians">Convert the values to radians</param>
        /// <returns></returns>
        public static TxVector CreateVector(double v1, double v2, double v3, bool convertToRadians)
        {
            TxVector vector;
            if (convertToRadians)
            {
                vector = new TxVector(Deg2Rad(v1), Deg2Rad(v2), Deg2Rad(v3));
            }
            else
            {
                vector = new TxVector(v1, v2, v3);
            }
            return vector;
        }
        public static TxVector CreateVector2(double v1, double v2, double v3)
        {
            TxVector vector;
            vector = new TxVector(v1, v2, v3);
            return vector;
        }
        public static TxVector CreateVector2(double[] v1)
        {
            TxVector vector;
            vector = new TxVector(v1[0], v1[1], v1[2]);
            return vector;
        }

        //判断两个值是否相等//if (cd1 < 1e-3 && cd1 > -1e-3)
        public static bool ValuesIsEqual(double[] v1, double[] v2, double num = 1e-3)
        {
            if (v1 == null || v2 == null)
            {
                return false;
            }

            bool result = false;

            if (num < 0)
            {
                num = 1e-3;
            }

            for (int i = 0; i < v1.Length; i++)
            {
                double c1 = v1[i] - v2[i];
                bool flag1 = c1 >= -num && c1 <= num;
                if (flag1)
                {
                    result = true;
                }
                else
                {
                    result = false;
                    break;
                }
            }



            return result;
        }

        //判断两个值是否相等//if (cd1 < 1e-3 && cd1 > -1e-3)
        public static bool ValuesIsEqual(double v1, double v2, double num = 1e-1)
        {
            bool result = false;

            if (num < 0)
            {
                num = 1e-1;
            }

            double c1 = v1 - v2;
            bool flag1 = c1 >= -num && c1 <= num;
            if (flag1)
            {
                result = true;
            }
            else
            {
                result = false;
            }


            return result;
        }

        #region 两个点之间的距离

        /// <summary>
        /// 两个点之间的距离
        /// </summary>
        /// <param name="firstPoint"></param>
        /// <param name="secondPoint"></param>
        /// <returns></returns>
        public static double P2PDist_obj(object[] firstPoint, object[] secondPoint)
        {
            double distance;

            distance = Math.Sqrt(Math.Pow((double)secondPoint[0] - (double)firstPoint[0], 2) + Math.Pow((double)secondPoint[1] - (double)firstPoint[1], 2) + Math.Pow((double)secondPoint[2] - (double)firstPoint[2], 2));

            return distance;
        }
        /// <summary>
        /// 两个点之间的距离
        /// </summary>
        /// <param name="firstPoint"></param>
        /// <param name="secondPoint"></param>
        /// <returns></returns>
        public static double P2PDist_object(double[] firstPoint, double[] secondPoint)
        {
            double distance;

            distance = Math.Sqrt(Math.Pow(secondPoint[0] - firstPoint[0], 2) + Math.Pow(secondPoint[1] - firstPoint[1], 2) + Math.Pow(secondPoint[2] - firstPoint[2], 2));

            return distance;
        }

        /// <summary>
        /// 两个点之间的距离 2d
        /// </summary>
        /// <param name="firstPoint"></param>
        /// <param name="secondPoint"></param>
        /// <returns></returns>
        public static double P2PDist_2D(object[] firstPoint, object[] secondPoint)
        {
            double distance;

            distance = Math.Abs(Math.Sqrt(Math.Pow((double)secondPoint[0] - (double)firstPoint[0], 2) + Math.Pow((double)secondPoint[1] - (double)firstPoint[1], 2)));

            return distance;
        }

        #endregion


        #region 向量计算



        /// <summary>
        /// 向量的叉乘
        /// a=(0a,1a,2a);
        /// b=(0b,1b,2b);
        /// a * b =（1a * 2b - 1b * 2a , -（0a * 2b - 0b * 2a）, 0a * 1b - 0b * 1a）
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        static double[] CrossProduct(double[] vector1, double[] vector2)
        {
            double[] result = new double[3];
            //return new douvector2le[] { vector1[1] * vector2[2] - vector2[1] * vector1[2],
            //                           -(vector1[0] * vector2[2] - vector2[0] * vector1[2]),
            //                           vector1[0] * vector2[1] - vector2[0] * vector1[1] };

            result[0] = vector1[1] * vector2[2] - vector1[2] * vector2[1];
            result[1] = vector1[2] * vector2[0] - vector1[0] * vector2[2];
            result[2] = vector1[0] * vector2[1] - vector1[1] * vector2[0];

            return result;
        }

        /// <summary>
        /// 向量的点积和参数向量
        /// </summary>
        /// <param name="Vec1"></param>
        /// <param name="Vec2"></param>
        /// <returns></returns>
        static double DotProduct(double[] Vec1, double[] Vec2)//此向量的点积和参数向量
        {
            return Vec1[0] * Vec2[0] + Vec1[1] * Vec2[1] + Vec1[2] * Vec2[2];
        }

        /// <summary>
        /// 这个向量的长度|a|
        /// </summary>
        /// <returns></returns>
        static double sqrMagnitude(double[] v1)//这个向量的长度
        {
            return Math.Sqrt(v1[0] * v1[0] + v1[1] * v1[1] + v1[2] * v1[2]);
        }

        /// <summary>
        /// 用参数向量减去此向量并返回结果
        /// </summary>
        /// <param name="Vec2"></param>
        /// <returns></returns>
        static double[] Subtract(double[] Vec1, double[] Vec2)//用参数向量减去此向量并返回结果
        {
            return new double[] { Vec1[0] - Vec2[0],
                                Vec1[1] - Vec2[1],
                                Vec1[2] - Vec2[2] };
        }
        /// <summary>
        /// 两个向量相加
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <returns></returns>
        static double[] Add(double[] vector1, double[] vector2)
        {
            return new double[]{vector1[0] + vector2[0],
                               vector1[1] + vector2[1],
                               vector1[2]+ vector2[2] };
        }
        /// <summary>
        /// 向量乘标量
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="scalar"></param>
        /// <returns></returns>
        static double[] Multiply(double[] vector, double scalar)
        {
            return new double[]{vector[0] * scalar,
                                vector[1] * scalar,
                                vector[2] * scalar };
        }

        /// <summary>
        /// 判断线与线之间的相交
        /// </summary>
        /// <param name="intersection">交点</param>
        /// <param name="p1">直线1上一点</param>
        /// <param name="v1">直线1方向</param>
        /// <param name="p2">直线2上一点</param>
        /// <param name="v2">直线2方向</param>
        /// <returns>是否相交</returns>
        public static bool LineLineIntersection(out double[] intersection, double[] p1, double[] v1, double[] p2, double[] v2)
        {
            intersection = new double[3];
            if (Math.Abs(DotProduct(v1, v2)) == 1)
            {
                // 两线平行
                return false;
            }

            double[] startPointSeg = Subtract(p2, p1);
            double[] vecS1 = CrossProduct(v1, v2);            // 有向面积1
            double[] vecS2 = CrossProduct(startPointSeg, v2); // 有向面积2
            double num = DotProduct(startPointSeg, vecS1);


            // 判断两这直线是否共面
            if (num >= 1E-05f || num <= -1E-05f)
            {
                return false;
            }

            // 有向面积比值，利用点乘是因为结果可能是正数或者负数
            double num2 = DotProduct(vecS2, vecS1) / sqrMagnitude(vecS1);

            intersection = Add(p1, Multiply(v1, num2));
            return true;
        }



        //点(x1,y1)与(x2,y2)为第一条选段的两端点
        //点(x3,y3)与(x4,y4)为第二条选段的两端点

        public static double[] GetLineVector_double(double[] FirstPoint, double[] SecondPoint)
        {
            double[] result = new double[3];

            double dist = P2PDist_object(FirstPoint, SecondPoint);

            result[0] = SecondPoint[0] / dist - FirstPoint[0] / dist;
            result[1] = SecondPoint[1] / dist - FirstPoint[1] / dist;
            result[2] = SecondPoint[2] / dist - FirstPoint[2] / dist;

            return result;
        }

        //求空间两条直线的夹角
        public static double SpaceAngleBetweenLines(TxVector[] txVectors1, TxVector[] txVectors2)
        {
            double result = double.NaN;
            /*
             *  n1=(x1-x2,y1-y2)
                n2=(x1-x3,y1-y3)
                cos x=(n1.n2)/(│n1│.│n2│)

                L1:(x-x1)/a1=(y-y1)/b1=(z-z1)/c1
                L2:(x–x2)/a2=(y–y2)/b2=(z–z2)/c2   
                这里L1&L2分别以笛卡尔形式表示在3D空间中通过点(x1,y1,z1)和(x2,y2,z2)的两条直线。
            */

            double[] L1 = GetLineVector_double(new double[] { txVectors1[0].X, txVectors1[0].Y, txVectors1[0].Z }, new double[] { txVectors1[1].X, txVectors1[1].Y, txVectors1[1].Z });

            double[] L2 = GetLineVector_double(new double[] { txVectors2[0].X, txVectors2[0].Y, txVectors2[0].Z }, new double[] { txVectors2[1].X, txVectors2[1].Y, txVectors2[1].Z });

            result = Math.Acos(DotProduct(L1, L2) / (sqrMagnitude(L1) * sqrMagnitude(L2)));

            return result;
        }
        public static double SpaceAngleBetweenLines<T1, T2, T3, T4>(T1 txVectors1, T2 txVectors2, T3 txVectors3, T4 txVectors4)
            where T1 : AJTVector
            where T2 : AJTVector
            where T3 : AJTVector
            where T4 : AJTVector
        {
            double result = double.NaN;
            /*
             *  n1=(x1-x2,y1-y2)
                n2=(x1-x3,y1-y3)
                cos x=(n1.n2)/(│n1│.│n2│)

                L1:(x-x1)/a1=(y-y1)/b1=(z-z1)/c1
                L2:(x–x2)/a2=(y–y2)/b2=(z–z2)/c2   
                这里L1&L2分别以笛卡尔形式表示在3D空间中通过点(x1,y1,z1)和(x2,y2,z2)的两条直线。
            */

            double[] L1 = GetLineVector_double(new double[] { txVectors1.X, txVectors1.Y, txVectors1.Z }, new double[] { txVectors2.X, txVectors2.Y, txVectors2.Z });

            double[] L2 = GetLineVector_double(new double[] { txVectors3.X, txVectors3.Y, txVectors3.Z }, new double[] { txVectors4.X, txVectors4.Y, txVectors4.Z });

            result = Math.Acos(DotProduct(L1, L2) / (sqrMagnitude(L1) * sqrMagnitude(L2)));

            return result;
        }
        #endregion
    }

}
