using System;
using System.Globalization;

namespace AutoJTMathUtilities
{
    /// <summary>
    /// 3D vector 
    /// </summary>
    public class AJTVector
    {
        #region Constructors

        public const double PiDivOneEighty = 0.017453292519943295;

        public const double OneEightyDivPi = 57.295779513082323;

        private double m_X;

        private double m_Y;

        private double m_Z;

        private static bool m_Status;

        private static double m_MaxDifference = 1E-07;

        public AJTVector()
        {
            this.X = 0.0;
            this.Y = 0.0;
            this.Z = 0.0;
        }

        public AJTVector(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public AJTVector(double[] v)
        {
            if (v != null)
            {
                this.X = v[0];
                this.Y = v[1];
                this.Z = v[2];
            }
            else
            {
                new AJTVector();
            }
        }

        public AJTVector(AJTVector vec)
        {
            this.X = vec.X;
            this.Y = vec.Y;
            this.Z = vec.Z;
        }

        public AJTVector(string vec, string sep = "_")
        {
            string[] separator = new string[]
            {
                sep
            };
            string[] array = vec.Split(separator, StringSplitOptions.None);
            if (array.Length != 3)
            {
                throw new ArgumentException(string.Concat(new string[]
                {
                    "The string must consist of exactly 3 values.",
                    Environment.NewLine,
                    "Splitter: ",
                    sep,
                    Environment.NewLine,
                    "String: ",
                    vec
                }));
            }
            double x;
            double y;
            double z;
            try
            {
                x = double.Parse(array[0], CultureInfo.InvariantCulture);
                y = double.Parse(array[1], CultureInfo.InvariantCulture);
                z = double.Parse(array[2], CultureInfo.InvariantCulture);
            }
            catch
            {
                throw new ArgumentException(string.Concat(new string[]
                {
                    "Cannot convert to double: ",
                    array[0],
                    ", ",
                    array[1],
                    ", ",
                    array[2]
                }));
            }
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public double X
        {
            get
            {
                return this.m_X;
            }
            set
            {
                this.m_X = value;
            }
        }

        public double Y
        {
            get
            {
                return this.m_Y;
            }
            set
            {
                this.m_Y = value;
            }
        }

        public double Z
        {
            get
            {
                return this.m_Z;
            }
            set
            {
                this.m_Z = value;
            }
        }

        public static bool Status
        {
            get
            {
                return AJTVector.m_Status;
            }
            private set
            {
                AJTVector.m_Status = value;
            }
        }

        public static double MaxDifference
        {
            get
            {
                return AJTVector.m_MaxDifference;
            }
            private set
            {
                AJTVector.m_MaxDifference = value;
            }
        }

        public double Length
        {
            get
            {
                if (this.IsNull())
                {
                    return 0.0;
                }
                AJTVector cvector = new AJTVector(this.X, this.Y, this.Z);
                return Math.Sqrt(AJTVector.DotProduct(cvector, cvector));
            }
        }

        public AJTVector Clone()
        {
            return new AJTVector(this.X, this.Y, this.Z);
        }


        #endregion


        public static double Radiant(double w)
        {
            return AJTMath.Radian(w);
        }

        public static double Degree(double w)
        {
            return AJTMath.Degree(w);
        }

        /// <summary>
        /// 向量加法
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static AJTVector Add(AJTVector v1, AJTVector v2)
        {
            new AJTVector();
            AJTVector cvector = new AJTVector();
            cvector.X = v1.X + v2.X;
            cvector.Y = v1.Y + v2.Y;
            cvector.Z = v1.Z + v2.Z;
            AJTVector.Status = true;
            return AJTVector.Copy(cvector);
        }

        /// <summary>
        /// 两个向量的角度
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double Angle(AJTVector v1, AJTVector v2)
        {
            AJTVector v3 = new AJTVector();
            AJTVector v4 = new AJTVector();
            AJTVector.Status = true;
            v3 = AJTVector.Normalize(v1);
            if (!AJTVector.Status)
            {
                return -1.0;
            }
            v4 = AJTVector.Normalize(v2);
            if (!AJTVector.Status)
            {
                return -1.0;
            }
            double num = AJTVector.DotProduct(v3, v4);
            if (num > 1.0)
            {
                num = 1.0;
            }
            if (num < -1.0)
            {
                num = -1.0;
            }
            return Math.Acos(num);
        }

        /// <summary>
        /// copy 向量
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public static AJTVector Copy(AJTVector vec)
        {
            AJTVector cvector = new AJTVector();
            cvector.X = vec.X;
            cvector.Y = vec.Y;
            cvector.Z = vec.Z;
            AJTVector.Status = true;
            return cvector;
        }
        /// <summary>
        /// copy 向量 1 to 2
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static AJTVector CopyD(AJTVector v1, AJTVector v2)
        {
            v2.X = v1.X;
            v2.Y = v1.Y;
            v2.Z = v1.Z;
            AJTVector.Status = true;
            return v2;
        }

        /// <summary>
        /// 向量差积
        /// Note that the cross product depends on the order of the two vectors: CrossProduct(a,b) is not equal to CrossProduct(b,a).
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static AJTVector CrossProduct(AJTVector v1, AJTVector v2)
        {
            new AJTVector();
            AJTVector cvector = new AJTVector();
            double num;
            if ((num = AJTVector.Angle(v1, v2)) > AJTVector.MaxDifference && num < 3.1415926535 - AJTVector.MaxDifference)
            {
                cvector.X = v1.Y * v2.Z - v1.Z * v2.Y;
                cvector.Y = v1.Z * v2.X - v1.X * v2.Z;
                cvector.Z = v1.X * v2.Y - v1.Y * v2.X;
            }
            AJTVector result = AJTVector.Copy(cvector);
            AJTVector.Status = true;
            return result;
        }
        /// <summary>
        /// 向量差积safe
        /// Note that the cross product depends on the order of the two vectors: CrossProduct(a,b) is not equal to CrossProduct(b,a).
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static AJTVector CrossProductSafe(AJTVector v1, AJTVector v2)
        {
            new AJTVector();
            AJTVector cvector = new AJTVector();
            double num;
            if ((num = AJTVector.Angle(v1, v2)) > AJTVector.MaxDifference && num < 3.1415926535 - AJTVector.MaxDifference)
            {
                cvector.X = v1.Y * v2.Z - v1.Z * v2.Y;
                cvector.Y = v1.Z * v2.X - v1.X * v2.Z;
                cvector.Z = v1.X * v2.Y - v1.Y * v2.X;
                AJTVector result = AJTVector.Copy(cvector);
                AJTVector.Status = true;
                return result;
            }
            return null;
        }
        /// <summary>
        /// 向量差积
        /// Note that the cross product depends on the order of the two vectors: CrossProduct(a,b) is not equal to CrossProduct(b,a).
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static AJTVector CrossProductD(AJTVector v1, AJTVector v2)
        {
            AJTVector cvector = new AJTVector();
            AJTVector cvector2 = new AJTVector();
            cvector.X = v1.Y * v2.Z - v1.Z * v2.Y;
            cvector.Y = v1.Z * v2.X - v1.X * v2.Z;
            cvector.Z = v1.X * v2.Y - v1.Y * v2.X;
            cvector2 = AJTVector.CopyD(cvector, cvector2);
            AJTVector.Status = true;
            return cvector2;
        }

        /// <summary>
        /// 将向量除以标量。
        //  结果返回向量。
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="div"></param>
        /// <returns></returns>
        public static AJTVector Divide(AJTVector vec, double div)
        {
            new AJTVector();
            AJTVector result = AJTVector.Copy(new AJTVector
            {
                X = vec.X / div,
                Y = vec.Y / div,
                Z = vec.Z / div
            });
            AJTVector.Status = true;
            return result;
        }
        /// <summary>
        /// 两个向量点积
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double DotProduct(AJTVector v1, AJTVector v2)
        {
            AJTVector.Status = true;
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }
        /// <summary>
        /// 两个向量点积
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double DotProductD(AJTVector v1, AJTVector v2)
        {
            AJTVector.Status = true;
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        /// <summary>
        /// get length
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public static double GetLength(AJTVector vec)
        {
            AJTVector.Status = true;
            return Math.Sqrt(AJTVector.DotProduct(vec, vec));
        }

        /// <summary>
        /// 单位向量
        /// </summary>
        /// <returns></returns>
        public static AJTVector Ident()
        {
            AJTVector cvector = new AJTVector();
            cvector.X = 0.0;
            cvector.Y = 0.0;
            cvector.Z = 0.0;
            AJTVector.Status = true;
            return cvector;
        }
        /// <summary>
        /// 主轴
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public static int MainAxis(AJTVector vec)
        {
            int num = 0;
            double value = 0.0;
            if (Math.Abs(vec.Y) > Math.Abs(vec.X))
            {
                num = 1;
            }
            if (num == 0)
            {
                value = vec.X;
            }
            if (num == 1)
            {
                value = vec.Y;
            }
            if (num == 2)
            {
                value = vec.Z;
            }
            if (Math.Abs(vec.Z) > Math.Abs(value))
            {
                num = 2;
            }
            return num;
        }

        /// <summary>
        /// 向量乘以标量
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="mlt"></param>
        /// <returns></returns>
        public static AJTVector Multiply(AJTVector vec, double mlt)
        {
            new AJTVector();
            AJTVector result = AJTVector.Copy(new AJTVector
            {
                X = vec.X * mlt,
                Y = vec.Y * mlt,
                Z = vec.Z * mlt
            });
            AJTVector.Status = true;
            return result;
        }

        /// <summary>
        /// 返回一个向量，其方向与指定向量相同，但长度为一。
        /// 保留矢量方向并将矢量长度更改为 1。
        /// </summary>
        /// <returns></returns>
        public AJTVector Normalize()
        {
            return AJTVector.Normalize(new AJTVector(this.X, this.Y, this.Z));
        }

        /// <summary>
        /// 返回一个向量，其方向与指定向量相同，但长度为一。
        /// 保留矢量方向并将矢量长度更改为 1。
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public static AJTVector Normalize(AJTVector vec)
        {
            AJTVector result = new AJTVector();
            AJTVector cvector = new AJTVector();
            double num = Math.Sqrt(AJTVector.DotProduct(vec, vec));
            if (num == 0.0)
            {
                Console.WriteLine("m_vec_norm: zero vector!\n");
                AJTVector.Status = false;
                return result;
            }
            cvector.X = (double)((float)(vec.X / num));
            cvector.Y = (double)((float)(vec.Y / num));
            cvector.Z = (double)((float)(vec.Z / num));
            result = AJTVector.Copy(cvector);
            AJTVector.Status = true;
            return result;
        }
        /// <summary>
        /// 返回一个向量，其方向与指定向量相同，但长度为一。
        /// 保留矢量方向并将矢量长度更改为 1
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public static AJTVector NormalizeD(AJTVector vec)
        {
            AJTVector cvector = new AJTVector();
            AJTVector cvector2 = new AJTVector();
            double num = Math.Sqrt(AJTVector.DotProductD(vec, vec));
            if (num == 0.0)
            {
                AJTVector.Status = false;
                return cvector2;
            }
            cvector.X = vec.X / num;
            cvector.Y = vec.Y / num;
            cvector.Z = vec.Z / num;
            AJTVector.CopyD(cvector, cvector2);
            AJTVector.Status = true;
            return cvector2;
        }

        /// <summary>
        /// 向量反转
        /// </summary>
        /// <returns></returns>
        public AJTVector Reverse()
        {
            new AJTVector();
            AJTVector result = AJTVector.Copy(new AJTVector
            {
                X = this.X * -1.0,
                Y = this.Y * -1.0,
                Z = this.Z * -1.0
            });
            AJTVector.Status = true;
            return result;
        }

        /// <summary>
        /// 向量反转
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public static AJTVector Reverse(AJTVector vec)
        {
            new AJTVector();
            AJTVector result = AJTVector.Copy(new AJTVector
            {
                X = vec.X * -1.0,
                Y = vec.Y * -1.0,
                Z = vec.Z * -1.0
            });
            AJTVector.Status = true;
            return result;
        }

        /// <summary>
        /// 向量减法
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static AJTVector Subtract(AJTVector v1, AJTVector v2)
        {
            new AJTVector();
            AJTVector result = AJTVector.Copy(new AJTVector
            {
                X = v1.X - v2.X,
                Y = v1.Y - v2.Y,
                Z = v1.Z - v2.Z
            });
            AJTVector.Status = true;
            return result;
        }

        /// <summary>
        /// 点积标量
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static AJTVector CrossProductNormalize(AJTVector v1, AJTVector v2)
        {
            new AJTVector();
            AJTVector.Status = true;
            return AJTVector.Normalize(AJTVector.CrossProduct(v1, v2));
        }
        /// <summary>
        /// 点积标量
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static AJTVector CrossProductNormalizeD(AJTVector v1, AJTVector v2)
        {
            new AJTVector();
            AJTVector vec = AJTVector.CrossProductD(v1, v2);
            AJTVector.Status = true;
            return AJTVector.NormalizeD(vec);
        }
        /// <summary>
        /// 返回矩阵的一部分
        /// </summary>
        /// <param name="m"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public AJTVector VectorFromMatrix(AJTMatrix m, int f)
        {
            AJTVector result = AJTMatrix.VectorFromMatrix(m, f);
            AJTVector.Status = AJTMatrix.Status;
            return result;
        }

        /// <summary>
        /// 向量乘矩阵
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static AJTVector MultiplyVectorMatrix(AJTVector vec, AJTMatrix m)
        {
            AJTVector cvector = new AJTVector();
            new AJTVector();
            cvector.X = m.Item[0, 0] * vec.X + m.Item[1, 0] * vec.Y + m.Item[2, 0] * vec.Z;
            cvector.Y = m.Item[0, 1] * vec.X + m.Item[1, 1] * vec.Y + m.Item[2, 1] * vec.Z;
            cvector.Z = m.Item[0, 2] * vec.X + m.Item[1, 2] * vec.Y + m.Item[2, 2] * vec.Z;
            AJTVector result = AJTVector.Copy(cvector);
            AJTVector.Status = true;
            return result;
        }

        /// <summary>
        /// 矩阵乘向量
        /// </summary>
        /// <param name="m"></param>
        /// <param name="vec"></param>
        /// <returns></returns>
        public static AJTVector MultiplyMatrixVector(AJTMatrix m, AJTVector vec)
        {
            AJTVector cvector = new AJTVector();
            new AJTVector();
            cvector.X = m.Item[0, 0] * vec.X + m.Item[0, 1] * vec.Y + m.Item[0, 2] * vec.Z + m.Item[0, 3];
            cvector.Y = m.Item[1, 0] * vec.X + m.Item[1, 1] * vec.Y + m.Item[1, 2] * vec.Z + m.Item[1, 3];
            cvector.Z = m.Item[2, 0] * vec.X + m.Item[2, 1] * vec.Y + m.Item[2, 2] * vec.Z + m.Item[2, 3];
            AJTVector result = AJTVector.Copy(cvector);
            AJTVector.Status = true;
            return result;
        }

        /// <summary>
        /// 向量相对于另一个向量旋转
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="rv"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static AJTVector Rotate(AJTVector vec, AJTVector rv, double angle)
        {
            new AJTVector();
            AJTMatrix m = new AJTMatrix();
            m = AJTMatrix.IJK_ToMatrix(rv.X, rv.Y, rv.Z);
            AJTVector result = AJTVector.RotateZ(vec, m, angle);
            AJTVector.Status = true;
            return result;
        }

        /// <summary>
        /// 向量绕矩阵x轴旋转
        /// </summary>
        /// <param name="v"></param>
        /// <param name="m"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static AJTVector RotateX(AJTVector v, AJTMatrix m, double angle)
        {
            AJTVector cvector = new AJTVector();
            AJTMatrix m2 = new AJTMatrix();
            cvector = AJTVector.MultiplyVectorMatrix(v, m);
            m2 = AJTMatrix.RotateX(m2, angle);
            cvector = AJTVector.MultiplyMatrixVector(m2, cvector);
            m2 = AJTMatrix.Inverse(m);
            cvector = AJTVector.MultiplyVectorMatrix(cvector, m2);
            AJTVector.Status = true;
            return cvector;
        }

        /// <summary>
        /// 向量绕矩阵y轴旋转
        /// </summary>
        /// <param name="v"></param>
        /// <param name="m"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static AJTVector RotateY(AJTVector v, AJTMatrix m, double angle)
        {
            AJTVector cvector = new AJTVector();
            AJTMatrix m2 = new AJTMatrix();
            cvector = AJTVector.MultiplyVectorMatrix(v, m);
            m2 = AJTMatrix.RotateY(m2, angle);
            cvector = AJTVector.MultiplyMatrixVector(m2, cvector);
            m2 = AJTMatrix.Inverse(m);
            cvector = AJTVector.MultiplyVectorMatrix(cvector, m2);
            AJTVector.Status = true;
            return cvector;
        }

        /// <summary>
        /// 向量绕矩阵z轴旋转
        /// </summary>
        /// <param name="v"></param>
        /// <param name="m"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static AJTVector RotateZ(AJTVector v, AJTMatrix m, double angle)
        {
            AJTVector cvector = new AJTVector();
            AJTMatrix m2 = new AJTMatrix();
            cvector = AJTVector.MultiplyVectorMatrix(v, m);
            m2 = AJTMatrix.RotateZ(m2, angle);
            cvector = AJTVector.MultiplyMatrixVector(m2, cvector);
            m2 = AJTMatrix.Inverse(m);
            cvector = AJTVector.MultiplyVectorMatrix(cvector, m2);
            AJTVector.Status = true;
            return cvector;
        }

        /// <summary>
        /// get hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return ((9973 * 6133 + this.X.GetHashCode()) * 6133 + this.Y.GetHashCode()) * 6133 + this.Z.GetHashCode();
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return this == obj || (!(obj.GetType() != typeof(AJTVector)) && this.Equals(obj as AJTVector));
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public bool Equals(AJTVector vec)
        {
            return Math.Abs(AJTVector.Subtract(this, vec).Length) <= 1E-06;
        }

        /// <summary>
        /// Equals 最大范围内
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="maxDiff"></param>
        /// <returns></returns>
        public bool Equals(AJTVector vec, double maxDiff)
        {
            return Math.Abs(AJTVector.Subtract(this, vec).Length) <= maxDiff;
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="vec"></param>
        /// <param name="decimalPlaces"></param>
        /// <param name="valueType"></param>
        /// <returns></returns>
        public bool Equals(AJTVector vec, int decimalPlaces, EnumValueType valueType)
        {
            if (valueType == EnumValueType.Linear)
            {
                return Math.Round(Math.Abs(this.X - vec.X), decimalPlaces) == 0.0 && Math.Round(Math.Abs(this.Y - vec.Y), decimalPlaces) == 0.0 && Math.Round(Math.Abs(this.Z - vec.Z), decimalPlaces) == 0.0;
            }
            return Math.Round(Math.Abs(AJTMath.Degree(this.X) - AJTMath.Degree(vec.X)), decimalPlaces) == 0.0 && Math.Round(Math.Abs(AJTMath.Degree(this.Y) - AJTMath.Degree(vec.Y)), decimalPlaces) == 0.0 && Math.Round(Math.Abs(AJTMath.Degree(this.Z) - AJTMath.Degree(vec.Z)), decimalPlaces) == 0.0;
        }

        /// <summary>
        /// 四条向量的交集
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <param name="v4"></param>
        /// <returns></returns>
        public static AJTVector Intersection2D(AJTVector v1, AJTVector v2, AJTVector v3, AJTVector v4)
        {
            if (v4.X * v2.Y == v2.X * v4.Y)
            {
                return null;
            }
            double mlt = (v2.Y * (v1.X - v3.X) + v2.X * (v3.Y - v1.Y)) / (v4.X * v2.Y - v2.X * v4.Y);
            return AJTVector.Add(v3, AJTVector.Multiply(v4, mlt));
        }

        public static bool LinesAreParallel2D(AJTVector positionVector1, AJTVector euclideanVector1, AJTVector positionVector2, AJTVector euclideanVector2)
        {
            return AJTVector.CrossProduct(euclideanVector1, euclideanVector2).IsNull();
        }

        public static bool LinesAreIdentical2D(AJTVector positionVector1, AJTVector euclideanVector1, AJTVector positionVector2, AJTVector euclideanVector2)
        {
            if (!AJTVector.LinesAreParallel2D(positionVector1, euclideanVector1, positionVector2, euclideanVector2))
            {
                return false;
            }
            double num = (positionVector1.X - positionVector2.X) / euclideanVector2.X;
            double num2 = (positionVector1.Y - positionVector2.Y) / euclideanVector2.Y;
            if (num == num2 && euclideanVector2.X != 0.0)
            {
                return true;
            }
            if (double.IsNaN(num) || double.IsInfinity(num))
            {
                return positionVector1.X - positionVector2.X == 0.0;
            }
            return (double.IsNaN(num2) || double.IsInfinity(num2)) && positionVector1.Y - positionVector2.Y == 0.0;
        }

        public static double operator *(AJTVector a, AJTVector b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        public static AJTVector operator +(AJTVector a, AJTVector b)
        {
            return new AJTVector(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static AJTVector operator /(AJTVector a, double b)
        {
            return new AJTVector(a.X / b, a.Y / b, a.Z / b);
        }

        public static AJTVector operator *(AJTVector a, double b)
        {
            return new AJTVector(a.X * b, a.Y * b, a.Z * b);
        }

        public static AJTVector operator -(AJTVector a, AJTVector b)
        {
            return new AJTVector(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public override string ToString()
        {
            return string.Concat(new string[]
            {
                this.X.ToString("F5"),
                " ",
                this.Y.ToString("F5"),
                " ",
                this.Z.ToString("F5")
            });
        }

        public string ToString(string Sep)
        {
            return string.Concat(new string[]
            {
                this.X.ToString(CultureInfo.InvariantCulture),
                Sep,
                this.Y.ToString(CultureInfo.InvariantCulture),
                Sep,
                this.Z.ToString(CultureInfo.InvariantCulture)
            });
        }
        public double[] ToDouble()
        {
            double[] v = new double[3];

            v[0] = this.X;
            v[1] = this.Y;
            v[2] = this.Z;

            return v;
        }
        public string ToString(string sep, int noOfDigits, CultureInfo cultureInfo = null)
        {
            if (cultureInfo == null)
            {
                cultureInfo = CultureInfo.InvariantCulture;
            }
            return string.Concat(new string[]
            {
                this.X.ToString("F" + noOfDigits.ToString(), cultureInfo),
                sep,
                this.Y.ToString("F" + noOfDigits.ToString(), cultureInfo),
                sep,
                this.Z.ToString("F" + noOfDigits.ToString(), cultureInfo)
            });
        }

        internal static string ConvertDoubleToString(double value, int decimalnum)
        {
            if (decimalnum < 0)
            {
                decimalnum = 0;
            }
            if (decimalnum == 0)
            {
                return string.Format("{0:0}", value);
            }
            if (decimalnum == 1)
            {
                return string.Format("{0:0.0}", value);
            }
            if (decimalnum == 2)
            {
                return string.Format("{0:0.00}", value);
            }
            if (decimalnum == 3)
            {
                return string.Format("{0:0.000}", value);
            }
            if (decimalnum == 4)
            {
                return string.Format("{0:0.0000}", value);
            }
            if (decimalnum == 5)
            {
                return string.Format("{0:0.00000}", value);
            }
            if (decimalnum == 6)
            {
                return string.Format("{0:0.000000}", value);
            }
            if (decimalnum == 7)
            {
                return string.Format("{0:0.0000000}", value);
            }
            if (decimalnum == 8)
            {
                return string.Format("{0:0.00000000}", value);
            }
            if (decimalnum == 9)
            {
                return string.Format("{0:0.000000000}", value);
            }
            if (decimalnum == 10)
            {
                return string.Format("{0:0.0000000000}", value);
            }
            if (decimalnum == 11)
            {
                return string.Format("{0:0.00000000000}", value);
            }
            if (decimalnum == 12)
            {
                return string.Format("{0:0.000000000000}", value);
            }
            if (decimalnum == 13)
            {
                return string.Format("{0:0.0000000000000}", value);
            }
            if (decimalnum == 14)
            {
                return string.Format("{0:0.00000000000000}", value);
            }
            if (decimalnum >= 15)
            {
                return string.Format("{0:0.000000000000000}", value);
            }
            return "0.0";
        }

        public bool IsNull()
        {
            return this.X == 0.0 && this.Y == 0.0 && this.Z == 0.0;
        }

        public static AJTVector KgFCmS2ToKgCm2(double x, double y, double z)
        {
            return AJTVector.KgFCmS2ToKgCm2(new AJTVector(x, y, z));
        }

        public static AJTVector KgFCmS2ToKgCm2(AJTVector vec)
        {
            if (vec == null)
            {
                return null;
            }
            return vec * 980.0;
        }

        public static AJTVector KgCm2ToKgFCmS2(double x, double y, double z)
        {
            return AJTVector.KgCm2ToKgFCmS2(new AJTVector(x, y, z));
        }

        public static AJTVector KgCm2ToKgFCmS2(AJTVector vec)
        {
            if (vec == null)
            {
                return null;
            }
            return vec * 0.0010204081632653062;
        }

    }
}
