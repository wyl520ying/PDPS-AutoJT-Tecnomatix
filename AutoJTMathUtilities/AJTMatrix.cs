using System;
using System.Globalization;

namespace AutoJTMathUtilities
{
    /// <summary>
    /// 3D Matrix 
    /// </summary>
    public class AJTMatrix
    {
        #region Constructors

        private double[,] m_Item;

        private int m_Cols = 4;

        private int m_Rows = 4;

        private static bool m_Status;

        private static double m_MaxDifference = 1E-07;





        public enum eAxis
        {
            X,
            Y,
            Z
        }

        public AJTMatrix()
        {
            this.Item = new double[this.m_Rows, this.m_Cols];

            this.Item[0, 0] = 1.0;
            this.Item[0, 1] = 0.0;
            this.Item[0, 2] = 0.0;
            this.Item[0, 3] = 0.0;

            this.Item[1, 0] = 0.0;
            this.Item[1, 1] = 1.0;
            this.Item[1, 2] = 0.0;
            this.Item[1, 3] = 0.0;

            this.Item[2, 0] = 0.0;
            this.Item[2, 1] = 0.0;
            this.Item[2, 2] = 1.0;
            this.Item[2, 3] = 0.0;

            this.Item[3, 0] = 0.0;
            this.Item[3, 1] = 0.0;
            this.Item[3, 2] = 0.0;
            this.Item[3, 3] = 1.0;
        }

        public AJTMatrix(AJTMatrix matrix)
        {
            this.Item = new double[this.m_Rows, this.m_Cols];

            this.Item[0, 0] = matrix.Item[0, 0];
            this.Item[0, 1] = matrix.Item[0, 1];
            this.Item[0, 2] = matrix.Item[0, 2];
            this.Item[0, 3] = matrix.Item[0, 3];

            this.Item[1, 0] = matrix.Item[1, 0];
            this.Item[1, 1] = matrix.Item[1, 1];
            this.Item[1, 2] = matrix.Item[1, 2];
            this.Item[1, 3] = matrix.Item[1, 3];

            this.Item[2, 0] = matrix.Item[2, 0];
            this.Item[2, 1] = matrix.Item[2, 1];
            this.Item[2, 2] = matrix.Item[2, 2];
            this.Item[2, 3] = matrix.Item[2, 3];

            this.Item[3, 0] = matrix.Item[3, 0];
            this.Item[3, 1] = matrix.Item[3, 1];
            this.Item[3, 2] = matrix.Item[3, 2];
            this.Item[3, 3] = matrix.Item[3, 3];

        }

        /// <summary>
        /// The fourth row of the 4x4 transformation matrix is always (0, 0, 0, 1).
        /// </summary>
        /// <param name="a00"></param>
        /// <param name="a01"></param>
        /// <param name="a02"></param>
        /// <param name="a03"></param>
        /// <param name="a10"></param>
        /// <param name="a11"></param>
        /// <param name="a12"></param>
        /// <param name="a13"></param>
        /// <param name="a20"></param>
        /// <param name="a21"></param>
        /// <param name="a22"></param>
        /// <param name="a23"></param>
        public AJTMatrix(double a00, double a01, double a02, double a03,
                         double a10, double a11, double a12, double a13,
                         double a20, double a21, double a22, double a23)
        {
            this.Item = new double[this.m_Rows, this.m_Cols];
            this.SetTrans(a00, a01, a02, a03,
                          a10, a11, a12, a13,
                          a20, a21, a22, a23,
                          0.0, 0.0, 0.0, 1.0);
        }

        public AJTMatrix(double[] matix)
        {
            this.Item = new double[this.m_Rows, this.m_Cols];
            int rods = 6;
            this.SetTrans(Math.Round(matix[0], rods), Math.Round(matix[1], rods), Math.Round(matix[2], rods), Math.Round(matix[3], rods),
                         Math.Round(matix[4], rods), Math.Round(matix[5], rods), Math.Round(matix[6], rods), Math.Round(matix[7], rods),
                          Math.Round(matix[8], rods), Math.Round(matix[9], rods), Math.Round(matix[10], rods), Math.Round(matix[11], rods),
                          0.0, 0.0, 0.0, 1.0);
        }

        public double[,] Item
        {
            get
            {
                return this.m_Item;
            }
            set
            {
                this.m_Item = value;
            }
        }

        public int Cols
        {
            get
            {
                return this.m_Cols;
            }
        }

        public int Rows
        {
            get
            {
                return this.m_Rows;
            }
        }

        /// <summary>
        /// 获取 RotationRPY_XYZ
        /// </summary>
        public AJTVector RotationRPY_XYZ
        {
            get
            {
                AJTVector cvector = new AJTVector();
                double[] rpy = AJTMatrix.GetRPY(AJTMatrix.MatrixFromItem(this.Item));
                cvector.X = rpy[3];
                cvector.Y = rpy[4];
                cvector.Z = rpy[5];
                return cvector;
            }
        }

        //获取和设置位置
        public AJTVector Translation
        {
            get
            {
                return AJTMatrix.VectorFromMatrix(this, 3);
            }
            set
            {
                this.Item[0, 3] = value.X;
                this.Item[1, 3] = value.Y;
                this.Item[2, 3] = value.Z;
            }
        }

        public double X
        {
            get
            {
                return this.Item[0, 3];
            }
            set
            {
                this.Item[0, 3] = value;
            }
        }

        public double Y
        {
            get
            {
                return this.Item[1, 3];
            }
            set
            {
                this.Item[1, 3] = value;
            }
        }

        public double Z
        {
            get
            {
                return this.Item[2, 3];
            }
            set
            {
                this.Item[2, 3] = value;
            }
        }

        public static bool Status
        {
            get
            {
                return AJTMatrix.m_Status;
            }
            private set
            {
                AJTMatrix.m_Status = value;
            }
        }

        public static double MaxDifference
        {
            get
            {
                return AJTMatrix.m_MaxDifference;
            }
            private set
            {
                AJTMatrix.m_MaxDifference = value;
            }
        }

        private static AJTMatrix MatrixFromItem(double[,] ItemValues)
        {
            AJTMatrix cmatrix = new AJTMatrix();

            cmatrix.Item[0, 0] = ItemValues[0, 0];
            cmatrix.Item[0, 1] = ItemValues[0, 1];
            cmatrix.Item[0, 2] = ItemValues[0, 2];
            cmatrix.Item[0, 3] = ItemValues[0, 3];

            cmatrix.Item[1, 0] = ItemValues[1, 0];
            cmatrix.Item[1, 1] = ItemValues[1, 1];
            cmatrix.Item[1, 2] = ItemValues[1, 2];
            cmatrix.Item[1, 3] = ItemValues[1, 3];

            cmatrix.Item[2, 0] = ItemValues[2, 0];
            cmatrix.Item[2, 1] = ItemValues[2, 1];
            cmatrix.Item[2, 2] = ItemValues[2, 2];
            cmatrix.Item[2, 3] = ItemValues[2, 3];

            cmatrix.Item[3, 0] = ItemValues[3, 0];
            cmatrix.Item[3, 1] = ItemValues[3, 1];
            cmatrix.Item[3, 2] = ItemValues[3, 2];
            cmatrix.Item[3, 3] = ItemValues[3, 3];

            AJTMatrix.Status = true;
            return cmatrix;
        }



        private void SetTrans(double a11, double a12, double a13, double a14,
            double a21, double a22, double a23, double a24, double
            a31, double a32, double a33, double a34,
            double a41, double a42, double a43, double a44)
        {
            this.Item[0, 0] = a11;
            this.Item[0, 1] = a12;
            this.Item[0, 2] = a13;
            this.Item[0, 3] = a14;

            this.Item[1, 0] = a21;
            this.Item[1, 1] = a22;
            this.Item[1, 2] = a23;
            this.Item[1, 3] = a24;

            this.Item[2, 0] = a31;
            this.Item[2, 1] = a32;
            this.Item[2, 2] = a33;
            this.Item[2, 3] = a34;

            this.Item[3, 0] = a41;
            this.Item[3, 1] = a42;
            this.Item[3, 2] = a43;
            this.Item[3, 3] = a44;

            AJTMatrix.Status = true;
        }

        #endregion

        #region Methods

        #region toString

        public override string ToString()
        {
            return this.ToString(", ", false, true);
        }

        public string ToString(string Sep)
        {
            return string.Concat(new string[]
            {
                this.X.ToString(CultureInfo.InvariantCulture),
                Sep,
                this.Y.ToString(CultureInfo.InvariantCulture),
                Sep,
                this.Z.ToString(CultureInfo.InvariantCulture),
                Sep,
                this.RotationRPY_XYZ.X.ToString(CultureInfo.InvariantCulture),
                Sep,
                this.RotationRPY_XYZ.Y.ToString(CultureInfo.InvariantCulture),
                Sep,
                this.RotationRPY_XYZ.Z.ToString(CultureInfo.InvariantCulture)
            });
        }

        public string ToString(string Sep, bool matrix, bool degree)
        {
            string text = "";
            if (matrix)
            {
                for (int i = 0; i < this.Item.GetLength(0); i++)
                {
                    for (int j = 0; j < this.Item.GetLength(1); j++)
                    {
                        text = text + this.Item[i, j].ToString("0.000000", CultureInfo.InvariantCulture) + Sep;
                    }
                }
                text = text.TrimEnd(new char[0]);
            }
            else if (degree)
            {
                text = string.Concat(new string[]
                {
                    this.X.ToString("0.000000", CultureInfo.InvariantCulture),
                    Sep,
                    this.Y.ToString("0.000000", CultureInfo.InvariantCulture),
                    Sep,
                    this.Z.ToString("0.000000", CultureInfo.InvariantCulture),
                    Sep,
                    AJTMath.Degree(this.RotationRPY_XYZ.X).ToString("0.000000", CultureInfo.InvariantCulture),
                    Sep,
                    AJTMath.Degree(this.RotationRPY_XYZ.Y).ToString("0.000000", CultureInfo.InvariantCulture),
                    Sep,
                    AJTMath.Degree(this.RotationRPY_XYZ.Z).ToString("0.000000", CultureInfo.InvariantCulture)
                });
            }
            else
            {
                text = string.Concat(new string[]
                {
                    this.X.ToString("0.000000", CultureInfo.InvariantCulture),
                    Sep,
                    this.Y.ToString("0.000000", CultureInfo.InvariantCulture),
                    Sep,
                    this.Z.ToString("0.000000", CultureInfo.InvariantCulture),
                    Sep,
                    this.RotationRPY_XYZ.X.ToString("0.000000", CultureInfo.InvariantCulture),
                    Sep,
                    this.RotationRPY_XYZ.Y.ToString("0.000000", CultureInfo.InvariantCulture),
                    Sep,
                    this.RotationRPY_XYZ.Z.ToString("0.000000", CultureInfo.InvariantCulture)
                });
            }
            return text;
        }

        public string ToString(string Sep, bool degree, int decimalNumbers = 6, CultureInfo cultureInfo = null)
        {
            string result;
            if (degree)
            {
                result = string.Concat(new string[]
                {
                    AJTMatrix.ConvertDoubleToString(this.X, decimalNumbers, cultureInfo),
                    Sep,
                    AJTMatrix.ConvertDoubleToString(this.Y, decimalNumbers, cultureInfo),
                    Sep,
                    AJTMatrix.ConvertDoubleToString(this.Z, decimalNumbers, cultureInfo),
                    Sep,
                    AJTMatrix.ConvertDoubleToString(AJTMath.Degree(this.RotationRPY_XYZ.X), decimalNumbers, cultureInfo),
                    Sep,
                    AJTMatrix.ConvertDoubleToString(AJTMath.Degree(this.RotationRPY_XYZ.Y), decimalNumbers, cultureInfo),
                    Sep,
                    AJTMatrix.ConvertDoubleToString(AJTMath.Degree(this.RotationRPY_XYZ.Z), decimalNumbers, cultureInfo)
                });
            }
            else
            {
                result = string.Concat(new string[]
                {
                    AJTMatrix.ConvertDoubleToString(this.X, decimalNumbers, cultureInfo),
                    Sep,
                    AJTMatrix.ConvertDoubleToString(this.Y, decimalNumbers, cultureInfo),
                    Sep,
                    AJTMatrix.ConvertDoubleToString(this.Z, decimalNumbers, cultureInfo),
                    Sep,
                    AJTMatrix.ConvertDoubleToString(this.RotationRPY_XYZ.X, decimalNumbers, cultureInfo),
                    Sep,
                    AJTMatrix.ConvertDoubleToString(this.RotationRPY_XYZ.Y, decimalNumbers, cultureInfo),
                    Sep,
                    AJTMatrix.ConvertDoubleToString(this.RotationRPY_XYZ.Z, decimalNumbers, cultureInfo)
                });
            }
            return result;
        }

        public static double[] ToDoubleArray(AJTMatrix ajtMation)
        {
            double[] result = null;

            result = new double[] { ajtMation.Item[0,0], ajtMation.Item[0, 1], ajtMation.Item[0, 2], ajtMation.Item[0, 3] ,
                                    ajtMation.Item[1,0], ajtMation.Item[1, 1], ajtMation.Item[1, 2], ajtMation.Item[1, 3] ,
                                    ajtMation.Item[2,0], ajtMation.Item[2, 1], ajtMation.Item[2, 2], ajtMation.Item[2, 3] };

            return result;
        }
        public static object[] ToDoubleArrayCAT(AJTMatrix ajtMation)
        {
            object[] result = null;

            result = new object[] { ajtMation.Item[0,0], ajtMation.Item[0, 1], ajtMation.Item[0, 2],
                                    ajtMation.Item[1,0], ajtMation.Item[1, 1], ajtMation.Item[1, 2],
                                    ajtMation.Item[2,0], ajtMation.Item[2, 1], ajtMation.Item[2, 2],
                                    ajtMation.Item[0, 3], ajtMation.Item[1, 3], ajtMation.Item[2, 3] };

            return result;
        }
        #endregion

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Equals(AJTMatrix obj)
        {
            return obj != null && !(base.GetType() != obj.GetType()) && (Math.Abs(this.X - obj.X) < AJTMatrix.MaxDifference && Math.Abs(this.Y - obj.Y) < AJTMatrix.MaxDifference && Math.Abs(this.Z - obj.Z) < AJTMatrix.MaxDifference && Math.Abs(this.RotationRPY_XYZ.X - obj.RotationRPY_XYZ.X) < AJTMatrix.MaxDifference && Math.Abs(this.RotationRPY_XYZ.Y - obj.RotationRPY_XYZ.Y) < AJTMatrix.MaxDifference && Math.Abs(this.RotationRPY_XYZ.Z - obj.RotationRPY_XYZ.Z) < AJTMatrix.MaxDifference);
        }

        /// <summary>
        /// 单位矩阵
        /// </summary>
        /// <returns></returns>
        public static AJTMatrix Ident()
        {
            AJTMatrix cmatrix = new AJTMatrix();

            cmatrix.Item[0, 0] = 1.0;
            cmatrix.Item[0, 1] = 0.0;
            cmatrix.Item[0, 2] = 0.0;
            cmatrix.Item[0, 3] = 0.0;

            cmatrix.Item[1, 0] = 0.0;
            cmatrix.Item[1, 1] = 1.0;
            cmatrix.Item[1, 2] = 0.0;
            cmatrix.Item[1, 3] = 0.0;

            cmatrix.Item[2, 0] = 0.0;
            cmatrix.Item[2, 1] = 0.0;
            cmatrix.Item[2, 2] = 1.0;
            cmatrix.Item[2, 3] = 0.0;

            cmatrix.Item[3, 0] = 0.0;
            cmatrix.Item[3, 1] = 0.0;
            cmatrix.Item[3, 2] = 0.0;
            cmatrix.Item[3, 3] = 1.0;

            AJTMatrix.Status = true;
            return cmatrix;
        }

        #region set location

        /// <summary>
        /// set loaction
        /// </summary>
        /// <param name="m"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static AJTMatrix PosFromVec(AJTMatrix m, double x, double y, double z)
        {
            m.Item[0, 3] = x;
            m.Item[1, 3] = y;
            m.Item[2, 3] = z;
            return m;
        }

        public static AJTMatrix PosFromVec(AJTMatrix m, AJTVector pos)
        {
            m.Item[0, 3] = pos.X;
            m.Item[1, 3] = pos.Y;
            m.Item[2, 3] = pos.Z;
            return m;
        }

        #endregion


        public static AJTMatrix OriFromVec(AJTVector xvec, AJTVector yvec, AJTVector zvec)
        {
            return AJTMatrix.OriFromVec(new AJTMatrix(), xvec, yvec, zvec);
        }

        public static AJTMatrix OriFromVec(AJTMatrix m, AJTVector xvec, AJTVector yvec, AJTVector zvec)
        {
            AJTVector cvector = new AJTVector();
            AJTVector cvector2 = new AJTVector();
            AJTVector cvector3 = new AJTVector();
            cvector = xvec.Normalize();
            if (!AJTVector.Status)
            {
                AJTMatrix.Status = false;
                return m;
            }
            cvector2 = yvec.Normalize();
            if (!AJTVector.Status)
            {
                AJTMatrix.Status = false;
                return m;
            }
            cvector3 = zvec.Normalize();
            if (!AJTVector.Status)
            {
                AJTMatrix.Status = false;
                return m;
            }
            m.Item[0, 0] = cvector.X;
            m.Item[1, 0] = cvector.Y;
            m.Item[2, 0] = cvector.Z;
            m.Item[3, 0] = 0.0;
            m.Item[0, 1] = cvector2.X;
            m.Item[1, 1] = cvector2.Y;
            m.Item[2, 1] = cvector2.Z;
            m.Item[3, 1] = 0.0;
            m.Item[0, 2] = cvector3.X;
            m.Item[1, 2] = cvector3.Y;
            m.Item[2, 2] = cvector3.Z;
            m.Item[3, 2] = 0.0;
            AJTMatrix.Status = true;
            return m;
        }


        /// <summary>
        /// 返回矩阵的一部分
        /// </summary>
        /// <param name="m"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static AJTVector VectorFromMatrix(AJTMatrix m, int f)
        {
            AJTVector cvector = new AJTVector();
            if (f == 0)
            {
                cvector.X = m.Item[0, 0];
                cvector.Y = m.Item[1, 0];
                cvector.Z = m.Item[2, 0];
            }
            else if (f == 1)
            {
                cvector.X = m.Item[0, 1];
                cvector.Y = m.Item[1, 1];
                cvector.Z = m.Item[2, 1];
            }
            else if (f == 2)
            {
                cvector.X = m.Item[0, 2];
                cvector.Y = m.Item[1, 2];
                cvector.Z = m.Item[2, 2];
            }
            else if (f == 3)
            {
                cvector.X = m.Item[0, 3];
                cvector.Y = m.Item[1, 3];
                cvector.Z = m.Item[2, 3];
            }
            AJTMatrix.Status = true;
            return cvector;
        }

        public static AJTMatrix Copy(AJTMatrix m1)
        {
            AJTMatrix cmatrix = new AJTMatrix();
            cmatrix.Item[0, 0] = m1.Item[0, 0];
            cmatrix.Item[0, 1] = m1.Item[0, 1];
            cmatrix.Item[0, 2] = m1.Item[0, 2];
            cmatrix.Item[1, 0] = m1.Item[1, 0];
            cmatrix.Item[1, 1] = m1.Item[1, 1];
            cmatrix.Item[1, 2] = m1.Item[1, 2];
            cmatrix.Item[2, 0] = m1.Item[2, 0];
            cmatrix.Item[2, 1] = m1.Item[2, 1];
            cmatrix.Item[2, 2] = m1.Item[2, 2];
            cmatrix.Item[0, 3] = m1.Item[0, 3];
            cmatrix.Item[1, 3] = m1.Item[1, 3];
            cmatrix.Item[2, 3] = m1.Item[2, 3];
            cmatrix.Item[3, 0] = (cmatrix.Item[3, 1] = (cmatrix.Item[3, 2] = 0.0));
            cmatrix.Item[3, 3] = 1.0;
            AJTMatrix.Status = true;
            return cmatrix;
        }
        /// <summary>
        /// 获取变换矩阵的逆矩阵。
        /// 变换矩阵的逆
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static AJTMatrix Inverse(AJTMatrix m)
        {
            new AJTMatrix();
            AJTMatrix cmatrix = new AJTMatrix();
            cmatrix.Item[0, 0] = m.Item[0, 0];
            cmatrix.Item[0, 1] = m.Item[1, 0];
            cmatrix.Item[0, 2] = m.Item[2, 0];
            cmatrix.Item[1, 0] = m.Item[0, 1];
            cmatrix.Item[1, 1] = m.Item[1, 1];
            cmatrix.Item[1, 2] = m.Item[2, 1];
            cmatrix.Item[2, 0] = m.Item[0, 2];
            cmatrix.Item[2, 1] = m.Item[1, 2];
            cmatrix.Item[2, 2] = m.Item[2, 2];
            cmatrix.Item[3, 0] = (cmatrix.Item[3, 1] = (cmatrix.Item[3, 2] = 0.0));
            cmatrix.Item[3, 3] = 1.0;
            cmatrix.Item[0, 3] = -m.Item[0, 3] * m.Item[0, 0] - m.Item[1, 3] * m.Item[1, 0] - m.Item[2, 3] * m.Item[2, 0];
            cmatrix.Item[1, 3] = -m.Item[0, 3] * m.Item[0, 1] - m.Item[1, 3] * m.Item[1, 1] - m.Item[2, 3] * m.Item[2, 1];
            cmatrix.Item[2, 3] = -m.Item[0, 3] * m.Item[0, 2] - m.Item[1, 3] * m.Item[1, 2] - m.Item[2, 3] * m.Item[2, 2];
            AJTMatrix result = AJTMatrix.Copy(cmatrix);
            AJTMatrix.Status = true;
            return result;
        }

        /// <summary>
        /// Multiplies one matrix by another: (matrix specified by parameter "a") * (matrix specified by parameter "b").
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static AJTMatrix Multiply(AJTMatrix m1, AJTMatrix m2)
        {
            AJTMatrix cmatrix = new AJTMatrix();
            cmatrix.Item[0, 0] = m1.Item[0, 0] * m2.Item[0, 0] + m1.Item[0, 1] * m2.Item[1, 0] + m1.Item[0, 2] * m2.Item[2, 0];
            cmatrix.Item[0, 1] = m1.Item[0, 0] * m2.Item[0, 1] + m1.Item[0, 1] * m2.Item[1, 1] + m1.Item[0, 2] * m2.Item[2, 1];
            cmatrix.Item[0, 2] = m1.Item[0, 0] * m2.Item[0, 2] + m1.Item[0, 1] * m2.Item[1, 2] + m1.Item[0, 2] * m2.Item[2, 2];
            cmatrix.Item[0, 3] = m1.Item[0, 0] * m2.Item[0, 3] + m1.Item[0, 1] * m2.Item[1, 3] + m1.Item[0, 2] * m2.Item[2, 3] + m1.Item[0, 3];
            cmatrix.Item[1, 0] = m1.Item[1, 0] * m2.Item[0, 0] + m1.Item[1, 1] * m2.Item[1, 0] + m1.Item[1, 2] * m2.Item[2, 0];
            cmatrix.Item[1, 1] = m1.Item[1, 0] * m2.Item[0, 1] + m1.Item[1, 1] * m2.Item[1, 1] + m1.Item[1, 2] * m2.Item[2, 1];
            cmatrix.Item[1, 2] = m1.Item[1, 0] * m2.Item[0, 2] + m1.Item[1, 1] * m2.Item[1, 2] + m1.Item[1, 2] * m2.Item[2, 2];
            cmatrix.Item[1, 3] = m1.Item[1, 0] * m2.Item[0, 3] + m1.Item[1, 1] * m2.Item[1, 3] + m1.Item[1, 2] * m2.Item[2, 3] + m1.Item[1, 3];
            cmatrix.Item[2, 0] = m1.Item[2, 0] * m2.Item[0, 0] + m1.Item[2, 1] * m2.Item[1, 0] + m1.Item[2, 2] * m2.Item[2, 0];
            cmatrix.Item[2, 1] = m1.Item[2, 0] * m2.Item[0, 1] + m1.Item[2, 1] * m2.Item[1, 1] + m1.Item[2, 2] * m2.Item[2, 1];
            cmatrix.Item[2, 2] = m1.Item[2, 0] * m2.Item[0, 2] + m1.Item[2, 1] * m2.Item[1, 2] + m1.Item[2, 2] * m2.Item[2, 2];
            cmatrix.Item[2, 3] = m1.Item[2, 0] * m2.Item[0, 3] + m1.Item[2, 1] * m2.Item[1, 3] + m1.Item[2, 2] * m2.Item[2, 3] + m1.Item[2, 3];
            cmatrix.Item[3, 0] = 0.0;
            cmatrix.Item[3, 1] = 0.0;
            cmatrix.Item[3, 2] = 0.0;
            cmatrix.Item[3, 3] = 1.0;
            AJTMatrix.Status = true;
            return cmatrix;
        }

        /// <summary>
        /// RotateX 弧度
        /// </summary>
        /// <param name="m"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static AJTMatrix RotateX(AJTMatrix m, double angle)
        {
            AJTMatrix result = new AJTMatrix();
            AJTMatrix cmatrix = new AJTMatrix();
            cmatrix.Item[1, 1] = (cmatrix.Item[2, 2] = Math.Cos(angle));
            cmatrix.Item[2, 1] = Math.Sin(angle);
            cmatrix.Item[1, 2] = -cmatrix.Item[2, 1];
            result = AJTMatrix.Multiply(m, cmatrix);
            if (!AJTMatrix.Status)
            {
                AJTMatrix.Status = false;
                return result;
            }
            AJTMatrix.Status = true;
            return result;
        }

        /// <summary>
        /// RotateY 弧度
        /// </summary>
        /// <param name="m"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static AJTMatrix RotateY(AJTMatrix m, double angle)
        {
            AJTMatrix result = new AJTMatrix();
            AJTMatrix cmatrix = new AJTMatrix();
            cmatrix.Item[0, 0] = (cmatrix.Item[2, 2] = Math.Cos(angle));
            cmatrix.Item[0, 2] = Math.Sin(angle);
            cmatrix.Item[2, 0] = -cmatrix.Item[0, 2];
            result = AJTMatrix.Multiply(m, cmatrix);
            if (!AJTMatrix.Status)
            {
                AJTMatrix.Status = false;
                return result;
            }
            AJTMatrix.Status = true;
            return result;
        }

        /// <summary>
        /// RotateZ 弧度
        /// </summary>
        /// <param name="m"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static AJTMatrix RotateZ(AJTMatrix m, double angle)
        {
            AJTMatrix result = new AJTMatrix();
            AJTMatrix cmatrix = new AJTMatrix();
            cmatrix.Item[0, 0] = (cmatrix.Item[1, 1] = Math.Cos(angle));
            cmatrix.Item[1, 0] = Math.Sin(angle);
            cmatrix.Item[0, 1] = -cmatrix.Item[1, 0];
            result = AJTMatrix.Multiply(m, cmatrix);
            if (!AJTMatrix.Status)
            {
                AJTMatrix.Status = false;
                return result;
            }
            AJTMatrix.Status = true;
            return result;
        }

        /// <summary>
        /// RotateK 弧度
        /// </summary>
        /// <param name="m"></param>
        /// <param name="angle"></param>
        /// <param name="kx"></param>
        /// <param name="ky"></param>
        /// <param name="kz"></param>
        /// <returns></returns>
        public static AJTMatrix RotateK(AJTMatrix m, double angle, double kx, double ky, double kz)
        {
            AJTMatrix result = new AJTMatrix();
            AJTMatrix cmatrix = new AJTMatrix();
            double num = Math.Cos(angle);
            double num2 = Math.Sin(angle);
            double num3 = 1.0 - num;
            double num4 = kx * num3;
            double num5 = ky * num3;
            double num6 = kz * num3;
            double num7 = kx * num2;
            double num8 = ky * num2;
            double num9 = kz * num2;
            cmatrix.Item[0, 0] = kx * num4 + num;
            cmatrix.Item[1, 0] = kx * num5 + num9;
            cmatrix.Item[2, 0] = kx * num6 - num8;
            cmatrix.Item[0, 1] = ky * num4 - num9;
            cmatrix.Item[1, 1] = ky * num5 + num;
            cmatrix.Item[2, 1] = ky * num6 + num7;
            cmatrix.Item[0, 2] = kz * num4 + num8;
            cmatrix.Item[1, 2] = kz * num5 - num7;
            cmatrix.Item[2, 2] = kz * num6 + num;
            result = AJTMatrix.Multiply(m, cmatrix);
            if (!AJTMatrix.Status)
            {
                AJTMatrix.Status = false;
                return result;
            }
            AJTMatrix.Status = true;
            return result;
        }

        /// <summary>
        /// 移动位置
        /// </summary>
        /// <param name="m"></param>
        /// <param name="vec"></param>
        /// <returns></returns>
        public static AJTMatrix ShiftByVector(AJTMatrix m, AJTVector vec)
        {
            m.Item[0, 3] += vec.X;
            m.Item[1, 3] += vec.Y;
            m.Item[2, 3] += vec.Z;
            AJTMatrix.Status = true;
            return m;
        }

        /// <summary>
        /// 移动位置
        /// </summary>
        /// <param name="m"></param>
        /// <param name="xx"></param>
        /// <param name="yy"></param>
        /// <param name="zz"></param>
        /// <returns></returns>
        public static AJTMatrix Translate(AJTMatrix m, double xx, double yy, double zz)
        {
            AJTMatrix result = new AJTMatrix();

            AJTMatrix cmatrix = new AJTMatrix();
            cmatrix.Item[0, 3] = xx;
            cmatrix.Item[1, 3] = yy;
            cmatrix.Item[2, 3] = zz;

            result = AJTMatrix.Multiply(m, cmatrix);

            if (!AJTMatrix.Status)
            {
                AJTMatrix.Status = false;
                return result;
            }
            return result;
        }

        /// <summary>
        /// 移动位置		
        /// </summary>
        /// <param name="m"></param>
        /// <param name="vec"></param>
        /// <returns></returns>
        public static AJTMatrix Translate(AJTMatrix m, AJTVector vec)
        {
            return AJTMatrix.Translate(m, vec.X, vec.Y, vec.Z);
        }


        public static AJTMatrix GetBy3XY_Point(AJTVector x0, AJTVector x1, AJTVector y1)
        {
            AJTVector cvector = new AJTVector();
            AJTVector cvector2 = new AJTVector();
            AJTVector cvector3 = new AJTVector();

            AJTMatrix cmatrix = new AJTMatrix();

            //向量减法
            cvector = AJTVector.Subtract(x1, x0);
            cvector2 = AJTVector.Subtract(y1, x0);

            //单位向量
            cvector = AJTVector.Normalize(cvector);
            if (!AJTVector.Status)
            {
                AJTMatrix.Status = false;
                return cmatrix;
            }
            //单位向量
            cvector2 = AJTVector.Normalize(cvector2);
            if (!AJTVector.Status)
            {
                AJTMatrix.Status = false;
                return cmatrix;
            }
            //单位向量
            cvector3 = AJTVector.CrossProductNormalize(cvector, cvector2);
            if (!AJTVector.Status)
            {
                AJTMatrix.Status = false;
                return cmatrix;
            }
            //单位向量
            cvector2 = AJTVector.CrossProductNormalize(cvector3, cvector);
            if (!AJTVector.Status)
            {
                AJTMatrix.Status = false;
                return cmatrix;
            }

            cmatrix = AJTMatrix.PosFromVec(cmatrix, x0);
            AJTMatrix.OriFromVec(cmatrix, cvector, cvector2, cvector3);
            if (!AJTMatrix.Status)
            {
                AJTMatrix.Status = false;
                return cmatrix;
            }
            if (!AJTMatrix.IsOrientationValid(cmatrix))
            {
                cvector3 = AJTVector.Reverse(cvector3);
                cmatrix = AJTMatrix.OriFromVec(cmatrix, cvector, cvector2, cvector3);
                if (!AJTMatrix.Status)
                {
                    AJTMatrix.Status = false;
                    return cmatrix;
                }
            }
            AJTMatrix.Status = true;
            return cmatrix;
        }

        /// <summary>
        /// 复制方向
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static AJTMatrix CopyOrientation(AJTMatrix m1, AJTMatrix m2)
        {
            m2.Item[0, 0] = m1.Item[0, 0];
            m2.Item[0, 1] = m1.Item[0, 1];
            m2.Item[0, 2] = m1.Item[0, 2];

            m2.Item[1, 0] = m1.Item[1, 0];
            m2.Item[1, 1] = m1.Item[1, 1];
            m2.Item[1, 2] = m1.Item[1, 2];

            m2.Item[2, 0] = m1.Item[2, 0];
            m2.Item[2, 1] = m1.Item[2, 1];
            m2.Item[2, 2] = m1.Item[2, 2];

            m2.Item[3, 0] = (m2.Item[3, 1] = (m2.Item[3, 2] = 0.0));
            m2.Item[3, 3] = 1.0;
            AJTMatrix.Status = true;
            return m2;
        }

        /// <summary>
        /// 复制方向
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static AJTMatrix CopyPosition(AJTMatrix m1, AJTMatrix m2)
        {
            m2.Item[0, 3] = m1.Item[0, 3];
            m2.Item[1, 3] = m1.Item[1, 3];
            m2.Item[2, 3] = m1.Item[2, 3];
            m2.Item[3, 0] = (m2.Item[3, 1] = (m2.Item[3, 2] = 0.0));
            m2.Item[3, 3] = 1.0;
            AJTMatrix.Status = true;
            return m2;
        }

        #region GetRPY_Matrix

        /// <summary>
        /// Gets or sets the rotation component of the matrix, in Euler-ZYZ order.
        /// 通过 phi, the, psi 三个轴的旋转获取矩阵
        /// </summary>
        /// <param name="xx"></param>
        /// <param name="yy"></param>
        /// <param name="zz"></param>
        /// <param name="phi"></param>
        /// <param name="the"></param>
        /// <param name="psi"></param>
        /// <returns></returns>
        public static AJTMatrix GetEulerZYZ_Matrix(double xx, double yy, double zz, double phi, double the, double psi)
        {
            AJTMatrix cmatrix = new AJTMatrix();
            cmatrix.Item[0, 3] = xx;
            cmatrix.Item[1, 3] = yy;
            cmatrix.Item[2, 3] = zz;

            AJTMatrix result = AJTMatrix.RotateZ(AJTMatrix.RotateY(AJTMatrix.RotateZ(cmatrix, phi), the), psi);
            AJTMatrix.Status = true;
            return result;
        }

        /// <summary>
        /// 通过 ww, pp, rr 三个轴的旋转获取矩阵
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="rot"></param>
        /// <returns></returns>
        public static AJTMatrix GetRPY_Matrix(AJTVector trans, AJTVector rot)
        {
            return AJTMatrix.GetRPY_Matrix(trans.X, trans.Y, trans.Z, rot.X, rot.Y, rot.Z);
        }
        /// <summary>
        /// 通过 ww, pp, rr 三个轴的旋转获取矩阵
        /// </summary>
        /// <param name="xx"></param>
        /// <param name="yy"></param>
        /// <param name="zz"></param>
        /// <param name="ww"></param>
        /// <param name="pp"></param>
        /// <param name="rr"></param>
        /// <returns></returns>
        public static AJTMatrix GetRPY_Matrix(double xx, double yy, double zz, double ww, double pp, double rr)
        {
            AJTMatrix cmatrix = new AJTMatrix();
            AJTVector cvector = new AJTVector();
            AJTVector cvector2 = new AJTVector();
            AJTVector cvector3 = new AJTVector();
            double num = Math.Cos(rr);
            double num2 = Math.Sin(rr);
            double num3 = Math.Cos(pp);
            double num4 = Math.Sin(pp);
            double num5 = Math.Cos(ww);
            double num6 = Math.Sin(ww);
            cvector.X = num3 * num;
            cvector.Y = num3 * num2;
            cvector.Z = -num4;
            cvector2.X = -num5 * num2 + num6 * num4 * num;
            cvector2.Y = num5 * num + num6 * num4 * num2;
            cvector2.Z = num6 * num3;
            cvector3.X = num6 * num2 + num5 * num4 * num;
            cvector3.Y = -num6 * num + num5 * num4 * num2;
            cvector3.Z = num5 * num3;
            cmatrix.Item[0, 3] = xx;
            cmatrix.Item[1, 3] = yy;
            cmatrix.Item[2, 3] = zz;
            cmatrix.Item[3, 3] = 1.0;
            AJTMatrix.OriFromVec(cmatrix, cvector, cvector2, cvector3);
            return cmatrix;
        }

        #endregion

        #region 计算矩阵的欧拉

        /// <summary>
        /// 计算矩阵的 欧拉ZYZ
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static double[] GetEulerZYZ(AJTMatrix m)
        {
            double[] array = new double[6];
            array[0] = m.Item[0, 3];
            array[1] = m.Item[1, 3];
            array[2] = m.Item[2, 3];
            double num = Math.Atan2(m.Item[1, 2], m.Item[0, 2]);
            double num2 = Math.Cos(num);
            double num3 = Math.Sin(num);
            double num4 = Math.Atan2(num2 * m.Item[0, 2] + num3 * m.Item[1, 2], m.Item[2, 2]);
            double num5 = Math.Atan2(num2 * m.Item[1, 0] - num3 * m.Item[0, 0], num2 * m.Item[1, 1] - num3 * m.Item[0, 1]);
            if (Math.Abs(num4) < AJTMatrix.MaxDifference && num + num5 < AJTMatrix.MaxDifference)
            {
                array[3] = 0.0;
                array[4] = 0.0;
                array[5] = 0.0;
            }
            else
            {
                array[3] = num;
                array[4] = num4;
                array[5] = num5;
            }
            AJTMatrix.Status = true;
            return array;
        }
        /// <summary>
        /// 计算矩阵的 欧拉ZXZ
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static double[] GetEulerZXZ(AJTMatrix m)
        {
            double[] array = new double[6];
            array[0] = m.X;
            array[1] = m.Y;
            array[2] = m.Z;
            double num = Math.Atan2(-m.Item[0, 2], m.Item[1, 2]);
            double num2 = Math.Cos(num);
            double num3 = Math.Sin(num);
            double num4 = Math.Atan2(num3 * m.Item[0, 2] - num2 * m.Item[1, 2], m.Item[2, 2]);
            double y = num2 * -m.Item[0, 1] - num3 * m.Item[1, 1];
            double x = num2 * m.Item[0, 0] + num3 * m.Item[1, 0];
            double num5 = Math.Atan2(y, x);
            if (Math.Abs(num4) < 1E-05 && num + num5 < 1E-05)
            {
                array[3] = 0.0;
                array[4] = 0.0;
                array[5] = 0.0;
            }
            else
            {
                array[3] = AJTMath.Degree(num);
                array[4] = AJTMath.Degree(num4);
                array[5] = AJTMath.Degree(num5);
            }
            return array;
        }

        #endregion

        #region 获取矩阵的 RPY

        public static void GetRotationRPY(AJTMatrix m, out double rx, out double ry, out double rz)
        {
            double num = m.Item[1, 0];
            double num2 = m.Item[0, 0];
            double num3;
            double num4;
            double num5;
            if (num < 1E-06 && num > -1E-06)
            {
                if (num2 > -1E-06)
                {
                    num3 = 0.0;
                    num4 = 1.0;
                    num5 = num2;
                    if (num2 < 1E-06)
                    {
                        num5 = 0.0;
                    }
                }
                else
                {
                    num3 = 0.0;
                    num4 = -1.0;
                    num5 = -num2;
                }
            }
            else if (num2 < 1E-06 && num2 > -1E-06)
            {
                if (num > 1E-06)
                {
                    num3 = 1.0;
                    num4 = 0.0;
                    num5 = num;
                }
                else
                {
                    num3 = -1.0;
                    num4 = 0.0;
                    num5 = -num;
                }
            }
            else
            {
                num5 = Math.Sqrt(num * num + num2 * num2);
                num3 = num / num5;
                num4 = num2 / num5;
            }
            num = -m.Item[2, 0];
            num2 = num5;
            double y;
            double x;
            if (num < 1E-06 && num > -1E-06)
            {
                y = 0.0;
                x = 1.0;
            }
            else if (num2 < 1E-06 && num2 > -1E-06)
            {
                if (num > 1E-06)
                {
                    y = 1.0;
                    x = 0.0;
                }
                else
                {
                    y = -1.0;
                    x = 0.0;
                }
            }
            else
            {
                num5 = Math.Sqrt(num * num + num2 * num2);
                y = num / num5;
                x = num2 / num5;
            }
            num = num3 * m.Item[0, 2] - num4 * m.Item[1, 2];
            num2 = num4 * m.Item[1, 1] - num3 * m.Item[0, 1];
            double y2;
            double x2;
            if (num < 1E-06 && num > -1E-06)
            {
                if (num2 > -1E-06)
                {
                    y2 = 0.0;
                    x2 = 1.0;
                }
                else
                {
                    y2 = 0.0;
                    x2 = -1.0;
                }
            }
            else if (num2 < 1E-06 && num2 > -1E-06)
            {
                if (num > 1E-06)
                {
                    y2 = 1.0;
                    x2 = 0.0;
                }
                else
                {
                    y2 = -1.0;
                    x2 = 0.0;
                }
            }
            else
            {
                num5 = Math.Sqrt(num * num + num2 * num2);
                y2 = num / num5;
                x2 = num2 / num5;
            }
            rx = Math.Atan2(y2, x2);
            ry = Math.Atan2(y, x);
            rz = Math.Atan2(num3, num4);
        }

        public static double[] GetRPY(AJTMatrix m)
        {
            double[] array = new double[6];
            array[0] = m.Item[0, 3];
            array[1] = m.Item[1, 3];
            array[2] = m.Item[2, 3];
            double num;
            double num4;
            double num5;
            double num6;
            double num7;
            double num8;
            if (Math.Abs(m.Item[0, 0]) >= AJTMatrix.MaxDifference || Math.Abs(m.Item[1, 0]) >= AJTMatrix.MaxDifference)
            {
                num = Math.Atan2(m.Item[1, 0], m.Item[0, 0]);
                double num2 = Math.Cos(num);
                double num3 = Math.Sin(num);
                num4 = Math.Atan2(-m.Item[2, 0], num2 * m.Item[0, 0] + num3 * m.Item[1, 0]);
                num5 = Math.Atan2(num3 * m.Item[0, 2] - num2 * m.Item[1, 2], num2 * m.Item[1, 1] - num3 * m.Item[0, 1]);
                num6 = Math.Atan2(-m.Item[1, 0], -m.Item[0, 0]);
                num2 = Math.Cos(num6);
                num3 = Math.Sin(num6);
                num7 = Math.Atan2(-m.Item[2, 0], num2 * m.Item[0, 0] + num3 * m.Item[1, 0]);
                num8 = Math.Atan2(num3 * m.Item[0, 2] - num2 * m.Item[1, 2], num2 * m.Item[1, 1] - num3 * m.Item[0, 1]);
            }
            else
            {
                num = 0.0;
                num4 = Math.Atan2(-m.Item[2, 0], 0.0);
                num5 = Math.Atan2(-m.Item[1, 2], m.Item[1, 1]);
                num6 = num;
                num7 = num4;
                num8 = num5;
            }
            if (num * num + num4 * num4 + num5 * num5 <= num6 * num6 + num7 * num7 + num8 * num8)
            {
                array[3] = num5;
                array[4] = num4;
                array[5] = num;
            }
            else
            {
                array[3] = num8;
                array[4] = num7;
                array[5] = num6;
            }
            AJTMatrix.Status = true;
            return array;
        }

        #endregion

        #region 通过四元数计算矩阵
        /// <summary>
        /// 通过四元数计算矩阵
        /// </summary>
        /// <param name="xx"></param>
        /// <param name="yy"></param>
        /// <param name="zz"></param>
        /// <param name="q1"></param>
        /// <param name="q2"></param>
        /// <param name="q3"></param>
        /// <param name="q4"></param>
        /// <returns></returns>
        public static AJTMatrix GetQuaternion(double xx, double yy, double zz, double q1, double q2, double q3, double q4)
        {
            AJTVector cvector = new AJTVector();
            AJTVector cvector2 = new AJTVector();
            AJTVector cvector3 = new AJTVector();
            AJTMatrix cmatrix = new AJTMatrix();
            AJTMatrix.Status = true;
            double num;
            if ((num = Math.Sqrt(q1 * q1 + q2 * q2 + q3 * q3 + q4 * q4)) < 1E-08)
            {
                Console.WriteLine("m_quaternion_to_mat: zero quaternion vector!\n");
                AJTMatrix.Status = false;
                return cmatrix;
            }
            q1 /= num;
            q2 /= num;
            q3 /= num;
            q4 /= num;
            cvector.X = 2.0 * (q1 * q1 + q2 * q2) - 1.0;
            cvector.Y = 2.0 * (q2 * q3 + q1 * q4);
            cvector.Z = 2.0 * (q2 * q4 - q1 * q3);
            cvector2.X = 2.0 * (q2 * q3 - q1 * q4);
            cvector2.Y = 2.0 * (q1 * q1 + q3 * q3) - 1.0;
            cvector2.Z = 2.0 * (q3 * q4 + q1 * q2);
            cvector3.X = 2.0 * (q2 * q4 + q1 * q3);
            cvector3.Y = 2.0 * (q3 * q4 - q1 * q2);
            cvector3.Z = 2.0 * (q1 * q1 + q4 * q4) - 1.0;
            cmatrix.Item[0, 3] = xx;
            cmatrix.Item[1, 3] = yy;
            cmatrix.Item[2, 3] = zz;
            cmatrix.Item[3, 3] = 1.0;
            AJTMatrix.Status = true;
            return AJTMatrix.OriFromVec(cmatrix, cvector, cvector2, cvector3);
        }

        #endregion

        /// <summary>
        /// Gets the sum of the main diagonal of a matrix.
        /// The sum of the main diagonal of a matrix
        /// 获取矩阵的主对角线的总和。
        /// 矩阵的主对角线之和
        /// </summary>
        /// <returns></returns>
        public double Trace()
        {
            double[,] item;
            double[,] array3;
            double[,] array2;
            double[,] array = array2 = (array3 = (item = this.Item));
            double num = array3[1, 1] + item[0, 0];
            double num2 = array2[2, 2] + num;
            return array[3, 3] + num2;
        }

        /// <summary>
        /// 计算矩阵的四元数
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static double[] MatrixToQquaternion(AJTMatrix m)
        {
            AJTVector translation = m.Translation;
            double num = m.Trace();
            double num3;
            double num4;
            double num5;
            double num6;
            if (num > 1E-05)
            {
                double num2 = 0.5 / Math.Sqrt(num);
                num3 = 0.25 / num2;
                num4 = (m.Item[2, 1] - m.Item[1, 2]) * num2;
                num5 = (m.Item[0, 2] - m.Item[2, 0]) * num2;
                num6 = (m.Item[1, 0] - m.Item[0, 1]) * num2;
            }
            else
            {
                int num7 = 0;
                double num8 = m.Item[0, 0];
                for (int i = 1; i < 3; i++)
                {
                    if (m.Item[i, i] > num8)
                    {
                        num7 = i;
                        num8 = m.Item[i, i];
                    }
                }
                if (num7 != 1)
                {
                    if (num7 != 2)
                    {
                        double num9 = Math.Sqrt(1.0 + m.Item[0, 0] - m.Item[1, 1] - m.Item[2, 2]) * 2.0;
                        num4 = 0.25 * num9;
                        num5 = (m.Item[0, 1] + m.Item[1, 0]) / num9;
                        num6 = (m.Item[0, 2] + m.Item[2, 0]) / num9;
                        num3 = (m.Item[2, 1] - m.Item[1, 2]) / num9;
                    }
                    else
                    {
                        double num10 = Math.Sqrt(1.0 + m.Item[2, 2] - m.Item[0, 0] - m.Item[1, 1]) * 2.0;
                        num4 = (m.Item[0, 2] + m.Item[2, 0]) / num10;
                        num5 = (m.Item[1, 2] + m.Item[2, 1]) / num10;
                        num6 = 0.25 * num10;
                        num3 = (m.Item[1, 0] - m.Item[0, 1]) / num10;
                    }
                }
                else
                {
                    double num11 = Math.Sqrt(1.0 + m.Item[1, 1] - m.Item[0, 0] - m.Item[2, 2]) * 2.0;
                    num4 = (m.Item[0, 1] + m.Item[1, 0]) / num11;
                    num5 = 0.25 * num11;
                    num6 = (m.Item[1, 2] + m.Item[2, 1]) / num11;
                    num3 = (m.Item[0, 2] - m.Item[2, 0]) / num11;
                }
            }
            return new double[]
            {
                translation.X,
                translation.Y,
                translation.Z,
                num3,
                num4,
                num5,
                num6
            };
        }

        /// <summary>
        /// 计算矩阵的四元数 Alternative
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static double[] MatrixToQquaternion_Alternative(AJTMatrix m)
        {
            double[] array = new double[7];
            array[0] = m.Item[0, 3];
            array[1] = m.Item[1, 3];
            array[2] = m.Item[2, 3];
            double num = m.Item[0, 0] + m.Item[1, 1] + m.Item[2, 2];
            double num3;
            double num4;
            double num5;
            double num6;
            if (num > 0.0)
            {
                double num2 = Math.Sqrt(num + 1.0) * 2.0;
                num3 = 0.25 * num2;
                num4 = (m.Item[2, 1] - m.Item[1, 2]) / num2;
                num5 = (m.Item[0, 2] - m.Item[2, 0]) / num2;
                num6 = (m.Item[1, 0] - m.Item[0, 1]) / num2;
            }
            else if (m.Item[0, 0] > m.Item[1, 1] & m.Item[0, 0] > m.Item[2, 2])
            {
                double num7 = Math.Sqrt(1.0 + m.Item[0, 0] - m.Item[1, 1] - m.Item[2, 2]) * 2.0;
                num3 = (m.Item[2, 1] - m.Item[1, 2]) / num7;
                num4 = 0.25 * num7;
                num5 = (m.Item[0, 1] + m.Item[1, 0]) / num7;
                num6 = (m.Item[0, 2] + m.Item[2, 0]) / num7;
            }
            else if (m.Item[1, 1] > m.Item[2, 2])
            {
                double num8 = Math.Sqrt(1.0 + m.Item[1, 1] - m.Item[0, 0] - m.Item[2, 2]) * 2.0;
                num3 = (m.Item[0, 2] - m.Item[2, 0]) / num8;
                num4 = (m.Item[0, 1] + m.Item[1, 0]) / num8;
                num5 = 0.25 * num8;
                num6 = (m.Item[1, 2] + m.Item[2, 1]) / num8;
            }
            else
            {
                double num9 = Math.Sqrt(1.0 + m.Item[2, 2] - m.Item[0, 0] - m.Item[1, 1]) * 2.0;
                num3 = (m.Item[1, 0] - m.Item[0, 1]) / num9;
                num4 = (m.Item[0, 2] + m.Item[2, 0]) / num9;
                num5 = (m.Item[1, 2] + m.Item[2, 1]) / num9;
                num6 = 0.25 * num9;
            }
            array[3] = num3;
            array[4] = num4;
            array[5] = num5;
            array[6] = num6;
            AJTMatrix.Status = true;
            return array;
        }

        /// <summary>
        /// 检查输入的矩阵和四元数
        /// </summary>
        /// <param name="m"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        public static bool CheckQuaternion(AJTMatrix m, double[] q)
        {
            AJTMatrix cmatrix = new AJTMatrix();
            bool result = true;
            cmatrix = AJTMatrix.GetQuaternion(q[0], q[1], q[2], q[3], q[4], q[5], q[6]);
            if (!AJTMatrix.Status && (Math.Abs(m.Item[0, 0] - cmatrix.Item[0, 0]) > AJTMatrix.MaxDifference || Math.Abs(m.Item[1, 0] - cmatrix.Item[1, 0]) > AJTMatrix.MaxDifference || Math.Abs(m.Item[2, 0] - cmatrix.Item[2, 0]) > AJTMatrix.MaxDifference || Math.Abs(m.Item[0, 1] - cmatrix.Item[0, 1]) > AJTMatrix.MaxDifference || Math.Abs(m.Item[1, 1] - cmatrix.Item[1, 1]) > AJTMatrix.MaxDifference || Math.Abs(m.Item[2, 1] - cmatrix.Item[2, 1]) > AJTMatrix.MaxDifference || Math.Abs(m.Item[0, 2] - cmatrix.Item[0, 2]) > AJTMatrix.MaxDifference || Math.Abs(m.Item[1, 2] - cmatrix.Item[1, 2]) > AJTMatrix.MaxDifference || Math.Abs(m.Item[2, 2] - cmatrix.Item[2, 2]) > AJTMatrix.MaxDifference))
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 固定矩阵到四元数
        /// </summary>
        /// <param name="m"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        public static double[] FixMatrixToQuaternion(AJTMatrix m, double[] q)
        {
            double[] array = new double[]
            {
                q[0],
                q[1],
                q[2],
                q[3],
                q[4],
                q[5],
                q[6]
            };
            bool flag = false;
            int num = 0;
            while (num < 8 && !flag)
            {
                if (AJTMatrix.CheckQuaternion(m, array))
                {
                    q[4] = array[4];
                    q[5] = array[5];
                    q[6] = array[6];
                    flag = true;
                }
                else
                {
                    if (num == 3)
                    {
                        array[4] = -array[4];
                    }
                    if ((num + 1) % 2 == 0)
                    {
                        array[5] = -array[5];
                    }
                    array[6] = -array[6];
                }
                num++;
            }
            AJTMatrix.Status = flag;
            return q;
        }

        /// <summary>
        /// ABC To Matrix
        /// </summary>
        /// <param name="xx"></param>
        /// <param name="yy"></param>
        /// <param name="zz"></param>
        /// <param name="aa"></param>
        /// <param name="bb"></param>
        /// <param name="cc"></param>
        /// <returns></returns>
        public static AJTMatrix ABC_ToMatrix(double xx, double yy, double zz, double aa, double bb, double cc)
        {
            AJTVector cvector = new AJTVector();
            AJTVector cvector2 = new AJTVector();
            AJTVector cvector3 = new AJTVector();
            AJTMatrix cmatrix = new AJTMatrix();
            double num = Math.Cos(aa);
            double num2 = Math.Cos(bb);
            double num3 = Math.Cos(cc);
            double num4 = Math.Sin(aa);
            double num5 = Math.Sin(bb);
            double num6 = Math.Sin(cc);
            cvector.X = -num2 * num;
            cvector.Y = -num2 * num4;
            cvector.Z = -num5;
            cvector2.X = num3 * num4 + num6 * num5 * num;
            cvector2.Y = -num3 * num + num6 * num5 * num4;
            cvector2.Z = -num6 * num2;
            cvector3.X = num6 * num4 - num3 * num5 * num;
            cvector3.Y = -num6 * num - num3 * num5 * num4;
            cvector3.Z = num3 * num2;
            cmatrix.Item[0, 3] = (double)((float)xx);
            cmatrix.Item[1, 3] = (double)((float)yy);
            cmatrix.Item[2, 3] = (double)((float)zz);
            cmatrix.Item[3, 3] = 1.0;
            AJTMatrix.Status = true;
            return AJTMatrix.OriFromVec(cmatrix, cvector, cvector2, cvector3);
        }

        /// <summary>
        /// GetABC
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static double[] GetABC(double[,] m)
        {
            double[] array = new double[6];
            array[0] = m[0, 3];
            array[1] = m[1, 3];
            array[2] = m[2, 3];
            if (Math.Abs(m[0, 0]) >= AJTMatrix.MaxDifference || Math.Abs(m[1, 0]) >= AJTMatrix.MaxDifference)
            {
                double num = Math.Atan2(-m[1, 0], -m[0, 0]);
                array[3] = num;
                double num2 = Math.Cos(num);
                double num3 = Math.Sin(num);
                array[4] = Math.Atan2(-m[2, 0], num2 * -m[0, 0] + num3 * -m[1, 0]);
                array[5] = Math.Atan2(-m[2, 1], m[2, 2]);
            }
            else
            {
                array[3] = 0.0;
                array[4] = Math.Atan2(-m[2, 0], 0.0);
                array[5] = Math.Atan2(-m[2, 1], -m[1, 1]);
            }
            AJTMatrix.Status = true;
            return array;
        }

        /// <summary>
        /// RPY_To_IJK
        /// </summary>
        /// <param name="yy"></param>
        /// <param name="pp"></param>
        /// <param name="rr"></param>
        /// <returns></returns>
        public static double[] RPY_To_IJK(double yy, double pp, double rr)
        {
            double[] array = new double[4];
            double num = Math.Cos(rr);
            double num2 = Math.Sin(rr);
            double num3 = Math.Cos(pp);
            double num4 = Math.Sin(pp);
            double num5 = Math.Cos(yy);
            double num6 = Math.Sin(yy);
            array[0] = num6 * num2 + num5 * num4 * num;
            array[1] = -num6 * num + num5 * num4 * num2;
            array[2] = num5 * num3;
            AJTMatrix.Status = true;
            return array;
        }
        /// <summary>
        /// IJK_To_RPY
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static double[] IJK_To_RPY(double i, double j, double k)
        {
            double[] array = new double[4];
            double y = Math.Sqrt(i * i + j * j);
            if (k == 1.0)
            {
                array[0] = 0.0;
                array[1] = 0.0;
                array[2] = 0.0;
                AJTMatrix.Status = true;
                return array;
            }
            array[0] = 0.0;
            array[1] = Math.Atan2(y, k);
            array[2] = Math.Atan2(j, i);
            AJTMatrix.Status = true;
            return array;
        }

        /// <summary>
        /// IJK_ToMatrix
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static AJTMatrix IJK_ToMatrix(double i, double j, double k)
        {
            AJTMatrix cmatrix = new AJTMatrix();
            AJTVector cvector = new AJTVector();
            AJTVector cvector2 = new AJTVector();
            AJTVector cvector3 = new AJTVector();
            if (k == 1.0)
            {
                AJTMatrix.Status = true;
                return cmatrix;
            }
            double y = Math.Sqrt(i * i + j * j);
            double num = Math.Cos(Math.Atan2(j, i));
            double num2 = Math.Sin(Math.Atan2(j, i));
            double num3 = Math.Cos(Math.Atan2(y, k));
            double num4 = Math.Sin(Math.Atan2(y, k));
            cvector.X = num3 * num;
            cvector.Y = num3 * num2;
            cvector.Z = -num4;
            cvector2.X = -num2;
            cvector2.Y = num;
            cvector2.Z = 0.0;
            cvector3.X = num4 * num;
            cvector3.Y = num4 * num2;
            cvector3.Z = num3;
            return AJTMatrix.OriFromVec(cmatrix, cvector, cvector2, cvector3);
        }

        /// <summary>
        /// 方向是否有效
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static bool IsOrientationValid(AJTMatrix m)
        {
            double num = m.Item[0, 0] * m.Item[1, 1] * m.Item[2, 2] - m.Item[0, 0] * m.Item[2, 1] * m.Item[1, 2] + m.Item[1, 0] * m.Item[2, 1] * m.Item[0, 2] - m.Item[1, 0] * m.Item[0, 1] * m.Item[2, 2] + m.Item[2, 0] * m.Item[0, 1] * m.Item[1, 2] - m.Item[2, 0] * m.Item[1, 1] * m.Item[0, 2];
            return Math.Abs(1.0 - num) < AJTMatrix.MaxDifference;
        }

        /// <summary>
        /// IsEqual
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static bool IsEqual(AJTMatrix m1, AJTMatrix m2)
        {
            bool result = true;
            if (Math.Abs(m1.Item[0, 3] - m2.Item[0, 3]) > AJTMatrix.MaxDifference || Math.Abs(m1.Item[1, 3] - m2.Item[1, 3]) > AJTMatrix.MaxDifference || Math.Abs(m1.Item[2, 3] - m2.Item[2, 3]) > AJTMatrix.MaxDifference || Math.Abs(m1.Item[0, 0] - m2.Item[0, 0]) > AJTMatrix.MaxDifference || Math.Abs(m1.Item[1, 0] - m2.Item[1, 0]) > AJTMatrix.MaxDifference || Math.Abs(m1.Item[2, 0] - m2.Item[2, 0]) > AJTMatrix.MaxDifference || Math.Abs(m1.Item[0, 1] - m2.Item[0, 1]) > AJTMatrix.MaxDifference || Math.Abs(m1.Item[1, 1] - m2.Item[1, 1]) > AJTMatrix.MaxDifference || Math.Abs(m1.Item[2, 1] - m2.Item[2, 1]) > AJTMatrix.MaxDifference || Math.Abs(m1.Item[0, 2] - m2.Item[0, 2]) > AJTMatrix.MaxDifference || Math.Abs(m1.Item[1, 2] - m2.Item[1, 2]) > AJTMatrix.MaxDifference || Math.Abs(m1.Item[2, 2] - m2.Item[2, 2]) > AJTMatrix.MaxDifference)
            {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// IsEqual
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <param name="maxDiff"></param>
        /// <returns></returns>
        public static bool IsEqual(AJTMatrix m1, AJTMatrix m2, double maxDiff)
        {
            bool result = true;
            if (Math.Abs(m1.Item[0, 3] - m2.Item[0, 3]) > maxDiff || Math.Abs(m1.Item[1, 3] - m2.Item[1, 3]) > maxDiff || Math.Abs(m1.Item[2, 3] - m2.Item[2, 3]) > maxDiff || Math.Abs(m1.Item[0, 0] - m2.Item[0, 0]) > maxDiff || Math.Abs(m1.Item[1, 0] - m2.Item[1, 0]) > maxDiff || Math.Abs(m1.Item[2, 0] - m2.Item[2, 0]) > maxDiff || Math.Abs(m1.Item[0, 1] - m2.Item[0, 1]) > maxDiff || Math.Abs(m1.Item[1, 1] - m2.Item[1, 1]) > maxDiff || Math.Abs(m1.Item[2, 1] - m2.Item[2, 1]) > maxDiff || Math.Abs(m1.Item[0, 2] - m2.Item[0, 2]) > maxDiff || Math.Abs(m1.Item[1, 2] - m2.Item[1, 2]) > maxDiff || Math.Abs(m1.Item[2, 2] - m2.Item[2, 2]) > maxDiff)
            {
                result = false;
            }
            return result;
        }

        public static double ArcLength(AJTVector p1, AJTVector p2, AJTVector p3)
        {
            AJTVector cvector = new AJTVector();
            AJTVector cvector2 = new AJTVector();
            AJTVector cvector3 = new AJTVector();
            AJTVector cvector4 = new AJTVector();
            double num = AJTMatrix.CircleBy3Points(p1, p2, p3, cvector, cvector2);
            if (!AJTMatrix.Status)
            {
                cvector3 = AJTVector.Subtract(p2, p1);
                cvector4 = AJTVector.Subtract(p3, p2);
                double num2 = AJTVector.Angle(cvector3, cvector4);
                if (Math.Abs(num2) < AJTMatrix.MaxDifference)
                {
                    return AJTMatrix.PointToPointDistance(cvector4, cvector3);
                }
                Console.WriteLine("m_arc_length: degenerate case!\n");
                AJTMatrix.Status = false;
                return 0.0;
            }
            else
            {
                cvector3 = AJTVector.Subtract(p3, p1);
                cvector3 = AJTVector.Divide(cvector3, 2.0);
                cvector4 = AJTVector.CrossProductNormalize(cvector3, cvector2);
                if (!AJTMatrix.Status)
                {
                    AJTMatrix.Status = false;
                    return 0.0;
                }
                cvector4 = AJTVector.Multiply(cvector4, num);
                cvector4 = AJTVector.Add(cvector, cvector4);
                cvector3 = AJTVector.Add(p1, cvector3);
                double num3 = AJTMatrix.PointToPointDistance(cvector4, cvector3);
                cvector3 = AJTVector.Subtract(p1, cvector);
                cvector4 = AJTVector.Subtract(p3, cvector);
                double num2 = AJTVector.Angle(cvector3, cvector4);
                if (num3 <= num)
                {
                    return num * num2;
                }
                return num * (6.283185307 - num2);
            }
        }

        public static double PointToPointDistance(AJTVector p1, AJTVector p2)
        {
            new AJTVector();
            AJTVector cvector = AJTVector.Subtract(p1, p2);
            return Math.Sqrt(AJTVector.DotProduct(cvector, cvector));
        }

        public static double PointToLineDistance(AJTVector p1, AJTVector pl1, AJTVector pl2)
        {
            AJTVector cvector = pl2 - pl1;
            return AJTVector.GetLength(AJTVector.CrossProduct(p1 - pl1, cvector)) / AJTVector.GetLength(cvector);
        }

        public static double CircleBy3Points(AJTVector p1, AJTVector p2, AJTVector p3, AJTVector cv, AJTVector nv)
        {
            AJTVector cvector = new AJTVector();
            AJTVector cvector2 = new AJTVector();
            AJTVector cvector3 = new AJTVector();
            AJTVector v = new AJTVector();
            new AJTVector();
            AJTVector v2 = new AJTVector();
            AJTVector v3 = new AJTVector();
            AJTVector v4 = new AJTVector();
            AJTVector v5 = new AJTVector();
            AJTVector v6 = new AJTVector();
            double result = 0.0;
            cvector = AJTVector.Subtract(p2, p1);
            cvector2 = AJTVector.Subtract(p3, p2);
            nv = AJTVector.CrossProductNormalize(cvector, cvector2);
            if (!AJTMatrix.Status)
            {
                AJTMatrix.Status = false;
                return result;
            }
            v5 = AJTVector.Divide(cvector, 2.0);
            v6 = AJTVector.Divide(cvector2, 2.0);
            v2 = AJTVector.Add(p1, v5);
            v3 = AJTVector.Add(p2, v6);
            v4 = AJTVector.Subtract(v2, v3);
            cvector3 = AJTVector.CrossProductNormalize(nv, cvector);
            if (!AJTMatrix.Status)
            {
                AJTMatrix.Status = false;
                return result;
            }
            v = AJTVector.CrossProductNormalize(nv, cvector2);
            if (!AJTMatrix.Status)
            {
                AJTMatrix.Status = false;
                return result;
            }
            double num = AJTVector.DotProduct(cvector3, v);
            if (Math.Abs(num) == 1.0)
            {
                AJTMatrix.Status = false;
                return result;
            }
            double mlt = (num * AJTVector.DotProduct(v4, v) - AJTVector.DotProduct(v4, cvector3)) / (1.0 - num * num);
            v5 = AJTVector.Multiply(cvector3, mlt);
            cv = AJTVector.Add(v2, v5);
            AJTVector cvector4 = AJTVector.Subtract(p1, cv);
            result = Math.Sqrt(AJTVector.DotProduct(cvector4, cvector4));
            AJTMatrix.Status = true;
            return result;
        }

        public static AJTMatrix CenterDirXDirYToTransformation(AJTVector center, AJTVector pntDirX, AJTVector pntDirY)
        {
            AJTVector cvector = pntDirX - center;
            AJTVector v = pntDirY - center;
            AJTVector cvector2 = AJTVector.CrossProduct(cvector, v);
            AJTVector cvector3 = AJTVector.CrossProduct(cvector2, cvector);
            cvector.Normalize();
            cvector3.Normalize();
            cvector2.Normalize();
            return new AJTMatrix(cvector.X, cvector3.X, cvector2.X, center.X, cvector.Y, cvector3.Y, cvector2.Y, center.Y, cvector.Z, cvector3.Z, cvector2.Z, center.Z);
        }

        public static AJTMatrix CenterDirXDirYToTransformation(AJTVector center, AJTVector pntDirX, AJTVector pntDirY, bool dirVectorsRelative)
        {
            AJTVector cvector = pntDirX;
            AJTVector v = pntDirY;
            if (!dirVectorsRelative)
            {
                cvector = pntDirX - center;
                v = pntDirY - center;
            }
            AJTVector cvector2 = AJTVector.CrossProduct(cvector, v);
            AJTVector cvector3 = AJTVector.CrossProduct(cvector2, cvector);
            cvector.Normalize();
            cvector3.Normalize();
            cvector2.Normalize();
            return new AJTMatrix(cvector.X, cvector3.X, cvector2.X, center.X, cvector.Y, cvector3.Y, cvector2.Y, center.Y, cvector.Z, cvector3.Z, cvector2.Z, center.Z);
        }

        internal static string ConvertDoubleToString(double value, int decimalnum, CultureInfo cultureInfo = null)
        {
            if (decimalnum < 0)
            {
                decimalnum = 0;
            }
            if (cultureInfo == null)
            {
                cultureInfo = CultureInfo.CurrentCulture;
            }
            if (decimalnum == 0)
            {
                return string.Format(cultureInfo, "{0:0}", new object[]
                {
                    value
                });
            }
            if (decimalnum == 1)
            {
                return string.Format(cultureInfo, "{0:0.0}", new object[]
                {
                    value
                });
            }
            if (decimalnum == 2)
            {
                return string.Format(cultureInfo, "{0:0.00}", new object[]
                {
                    value
                });
            }
            if (decimalnum == 3)
            {
                return string.Format(cultureInfo, "{0:0.000}", new object[]
                {
                    value
                });
            }
            if (decimalnum == 4)
            {
                return string.Format(cultureInfo, "{0:0.0000}", new object[]
                {
                    value
                });
            }
            if (decimalnum == 5)
            {
                return string.Format(cultureInfo, "{0:0.00000}", new object[]
                {
                    value
                });
            }
            if (decimalnum == 6)
            {
                return string.Format(cultureInfo, "{0:0.000000}", new object[]
                {
                    value
                });
            }
            if (decimalnum == 7)
            {
                return string.Format(cultureInfo, "{0:0.0000000}", new object[]
                {
                    value
                });
            }
            if (decimalnum == 8)
            {
                return string.Format(cultureInfo, "{0:0.00000000}", new object[]
                {
                    value
                });
            }
            if (decimalnum == 9)
            {
                return string.Format(cultureInfo, "{0:0.000000000}", new object[]
                {
                    value
                });
            }
            if (decimalnum == 10)
            {
                return string.Format(cultureInfo, "{0:0.0000000000}", new object[]
                {
                    value
                });
            }
            if (decimalnum == 11)
            {
                return string.Format(cultureInfo, "{0:0.00000000000}", new object[]
                {
                    value
                });
            }
            if (decimalnum == 12)
            {
                return string.Format(cultureInfo, "{0:0.000000000000}", new object[]
                {
                    value
                });
            }
            if (decimalnum == 13)
            {
                return string.Format(cultureInfo, "{0:0.0000000000000}", new object[]
                {
                    value
                });
            }
            if (decimalnum == 14)
            {
                return string.Format(cultureInfo, "{0:0.00000000000000}", new object[]
                {
                    value
                });
            }
            if (decimalnum >= 15)
            {
                return string.Format(cultureInfo, "{0:0.000000000000000}", new object[]
                {
                    value
                });
            }
            return "0.0";
        }

        /// <summary>
        /// 将矩阵以xz平面镜像
        /// </summary>
        /// <param name="aJTMatrix"></param>
        public static AJTMatrix MirrorBy_XZPlane(AJTMatrix aJTMatrix)
        {
            AJTMatrix aJTMatrix1 = new AJTMatrix();


            //计算rpy_xyz
            AJTMatrix.GetRotationRPY(aJTMatrix, out double rx, out double ry, out double rz);
            AJTVector vec = new AJTVector(rx, ry, rz);

            aJTMatrix1 = GetRPY_Matrix(aJTMatrix.X, -aJTMatrix.Y, aJTMatrix.Z, -vec.X, vec.Y, -vec.Z);


            return aJTMatrix1;
        }
        #endregion
    }
}
