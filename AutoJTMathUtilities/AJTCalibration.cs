using System;
using System.Collections.Generic;

namespace AutoJTMathUtilities
{
    public class AJTCalibration
    {
        public static void CalibratePoint(ref AJTVector point, List<AJTVector> nominalMeasurementPoints, List<AJTVector> realMeasurementPoints)
        {
            if (point == null || nominalMeasurementPoints == null || nominalMeasurementPoints.Count < 1 || realMeasurementPoints == null || realMeasurementPoints.Count < 1 || nominalMeasurementPoints.Count != realMeasurementPoints.Count)
            {
                return;
            }
            AJTMatrix cmatrix = AJTCalibration.CalculateCalibrationMatrix(nominalMeasurementPoints, realMeasurementPoints);
            if (cmatrix == null)
            {
                return;
            }
            point = AJTVector.MultiplyMatrixVector(cmatrix, point);
        }

        public static AJTMatrix CalculateCalibrationMatrix(List<AJTVector> nominalMeasurementPoints, List<AJTVector> realMeasurementPoints)
        {
            if (nominalMeasurementPoints == null || nominalMeasurementPoints.Count < 1 || realMeasurementPoints == null || realMeasurementPoints.Count < 1 || nominalMeasurementPoints.Count != realMeasurementPoints.Count)
            {
                return null;
            }
            AJTMatrix cmatrix = null;
            AJTVector cvector = null;
            AJTVector cvector2 = null;
            AJTVector cvector3 = null;
            AJTVector cvector4 = null;
            AJTVector cvector5 = AJTCalibration.CalculateArithmeticCenterPointFromPoints(nominalMeasurementPoints);
            AJTCalibration.CalculateArithmeticCenterAxisFromPoints(nominalMeasurementPoints, cvector5, out cvector, out cvector2);
            AJTVector cvector6 = AJTCalibration.CalculateArithmeticCenterPointFromPoints(realMeasurementPoints);
            AJTCalibration.CalculateArithmeticCenterAxisFromPoints(realMeasurementPoints, cvector6, out cvector3, out cvector4);
            if (cvector5 == null || cvector == null || cvector2 == null || cvector6 == null || cvector3 == null || cvector4 == null)
            {
                return cmatrix;
            }
            AJTMatrix by3XY_Point = AJTMatrix.GetBy3XY_Point(cvector5, AJTVector.Add(cvector5, cvector), AJTVector.Add(cvector5, cvector2));
            AJTMatrix by3XY_Point2 = AJTMatrix.GetBy3XY_Point(cvector6, AJTVector.Add(cvector6, cvector3), AJTVector.Add(cvector6, cvector4));
            cmatrix = AJTMatrix.Multiply(AJTMatrix.Inverse(by3XY_Point), by3XY_Point2);
            cmatrix.Translation = new AJTVector(cvector6.X - cvector5.X, cvector6.Y - cvector5.Y, cvector6.Z - cvector5.Z);
            cvector = null;
            cvector2 = null;
            cvector3 = null;
            cvector4 = null;
            return cmatrix;
        }

        private static AJTVector CalculateArithmeticCenterPointFromPoints(List<AJTVector> points)
        {
            if (points == null || points.Count < 1)
            {
                return null;
            }
            double num = 0.0;
            double num2 = 0.0;
            double num3 = 0.0;
            foreach (AJTVector cvector in points)
            {
                num += cvector.X;
                num2 += cvector.Y;
                num3 += cvector.Z;
            }
            return new AJTVector
            {
                X = num / Convert.ToDouble(points.Count),
                Y = num2 / Convert.ToDouble(points.Count),
                Z = num3 / Convert.ToDouble(points.Count)
            };
        }

        private static void CalculateArithmeticCenterAxisFromPoints(List<AJTVector> points, AJTVector centerPoint, out AJTVector xAxis, out AJTVector yAxis)
        {
            xAxis = null;
            yAxis = null;
            if (points == null)
            {
                return;
            }
            List<AJTVector> list = new List<AJTVector>();
            list = new List<AJTVector>();
            for (int i = 0; i < points.Count; i++)
            {
                AJTVector cvector;
                AJTVector cvector2;
                if (i == points.Count - 2)
                {
                    cvector = new AJTVector(points[0].X - points[i].X, points[0].Y - points[i].Y, points[0].Z - points[i].Z);
                    cvector2 = new AJTVector(points[1].X - points[i].X, points[1].Y - points[i].Y, points[1].Z - points[i].Z);
                }
                else if (i == points.Count - 1)
                {
                    cvector = new AJTVector(points[1].X - points[i].X, points[1].Y - points[i].Y, points[1].Z - points[i].Z);
                    cvector2 = new AJTVector(points[2].X - points[i].X, points[2].Y - points[i].Y, points[2].Z - points[i].Z);
                }
                else
                {
                    cvector = new AJTVector(points[i + 1].X - points[i].X, points[i + 1].Y - points[i].Y, points[i + 1].Z - points[i].Z);
                    cvector2 = new AJTVector(points[i + 2].X - points[i].X, points[i + 2].Y - points[i].Y, points[i + 2].Z - points[i].Z);
                }
                cvector = cvector.Normalize();
                cvector2 = cvector2.Normalize();
                AJTVector cvector3 = AJTVector.CrossProductSafe(cvector, cvector2);
                if (cvector3 != null)
                {
                    cvector3 = cvector3.Normalize();
                    list.Add(cvector3);
                }
            }
            if (list == null || list.Count == 0)
            {
                return;
            }
            if (list.Count > 1)
            {
                xAxis = new AJTVector();
                double num = 0.0;
                for (int j = 0; j < list.Count; j++)
                {
                    if (j <= 0 || AJTVector.Angle(xAxis, list[j]) <= 2.3561944901923448)
                    {
                        xAxis.X += Math.Abs(list[j].X);
                        xAxis.Y += Math.Abs(list[j].Y);
                        xAxis.Z += Math.Abs(list[j].Z);
                        num += 1.0;
                    }
                }
                xAxis.X /= Convert.ToDouble(num);
                xAxis.Y /= Convert.ToDouble(num);
                xAxis.Z /= Convert.ToDouble(num);
                xAxis = xAxis.Normalize();
            }
            else if (list.Count.Equals(1))
            {
                xAxis = list[0];
            }
            yAxis = new AJTVector(points[1].X - points[0].X, points[1].Y - points[0].Y, points[1].Z - points[0].Z);
            yAxis = yAxis.Normalize();
            AJTVector cvector4 = AJTVector.CrossProduct(xAxis, yAxis);
            cvector4 = cvector4.Normalize();
            yAxis = AJTVector.CrossProduct(cvector4, xAxis);
            yAxis = yAxis.Normalize();
            xAxis = AJTVector.CrossProduct(xAxis, yAxis);
            xAxis = xAxis.Normalize();
        }
    }
}
