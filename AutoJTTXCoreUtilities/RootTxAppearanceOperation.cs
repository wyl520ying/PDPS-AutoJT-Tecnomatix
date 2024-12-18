using Tecnomatix.Engineering;

namespace AutoJTTXCoreUtilities
{
    public class RootTxAppearanceOperation
    {
        private Tecnomatix.Engineering.ITxObject txCompoundPart_A;
        private Tecnomatix.Engineering.ITxObject txCompoundPart_B;

        public RootTxAppearanceOperation()
        {
            txCompoundPart_A = null;
            txCompoundPart_B = null;
        }

        #region 计算TxPartAppearance的绝对位置

        //计算TxPartAppearance的绝对位置  
        public double[] GetAbsoluteLocation(ITxLocatableObject txLocatableObject, TxWeldLocationOperation txWeldLocationOperation)
        {
            Tecnomatix.Engineering.TxPartAppearance appearance1 = txLocatableObject as Tecnomatix.Engineering.TxPartAppearance;//找到对象的Appearance

            double[] abslute_vector = new double[6];

            //没有appearance，直接获取
            if (appearance1 == null)
            {
                TxTransformation txTransformation = txWeldLocationOperation.LocationRelativeToWorkingFrame;//GetLocationRelativeToObject(txLocatableObject);
                TxVector txVector = txTransformation.Translation;
                TxVector txVectorR = txTransformation.RotationRPY_XYZ;

                abslute_vector[0] = txVector.X;
                abslute_vector[1] = txVector.Y;
                abslute_vector[2] = txVector.Z;

                abslute_vector[3] = txVectorR.X;
                abslute_vector[4] = txVectorR.Y;
                abslute_vector[5] = txVectorR.Z;
            }
            //计算rootappearance，再获取
            else
            {
                //没有appearance的实例
                //找到最顶层的compoundPart
                txCompoundPart_A = appearance1;
                txCompoundPart_B = appearance1.Collection;
                GetRootTxCompoundPart();

                ITxLocatableObject finalTxcom = txCompoundPart_A as ITxLocatableObject;

                TxTransformation txTransformation_1 = txWeldLocationOperation.GetLocationRelativeToObject(finalTxcom);
                TxVector txVector_1 = txTransformation_1.Translation;
                TxVector txVectorR = txTransformation_1.RotationRPY_XYZ;

                abslute_vector[0] = txVector_1.X;
                abslute_vector[1] = txVector_1.Y;
                abslute_vector[2] = txVector_1.Z;

                abslute_vector[3] = txVectorR.X;
                abslute_vector[4] = txVectorR.Y;
                abslute_vector[5] = txVectorR.Z;
            }

            return abslute_vector;
        }

        //Return TxTransformation
        public TxTransformation GetAbsoluteLocation_2(ITxLocatableObject txLocatableObject, TxWeldLocationOperation txWeldLocationOperation)
        {
            TxTransformation txTransformation1 = null;
            Tecnomatix.Engineering.TxPartAppearance appearance1 = txLocatableObject as Tecnomatix.Engineering.TxPartAppearance;//找到对象的Appearance            

            //没有appearance，直接获取
            if (appearance1 == null)
            {
                txTransformation1 = txWeldLocationOperation.LocationRelativeToWorkingFrame;//GetLocationRelativeToObject(txLocatableObject);
            }
            //计算rootappearance，再获取
            else
            {
                //没有appearance的实例
                //找到最顶层的compoundPart
                txCompoundPart_A = appearance1;
                txCompoundPart_B = appearance1.Collection;
                GetRootTxCompoundPart();

                ITxLocatableObject finalTxcom = txCompoundPart_A as ITxLocatableObject;

                txTransformation1 = txWeldLocationOperation.GetLocationRelativeToObject(finalTxcom);
            }

            return txTransformation1;
        }




        //过度点
        public double[] GetAbsoluteLocation(ITxLocatableObject txLocatableObject, TxRoboticViaLocationOperation txWeldLocationOperation)
        {
            double[] abslute_vector = new double[6];
            Tecnomatix.Engineering.TxPartAppearance appearance1 = txLocatableObject as Tecnomatix.Engineering.TxPartAppearance;//找到对象的Appearance            

            //没有appearance，直接获取
            if (appearance1 == null)
            {
                if (txLocatableObject == null)
                {
                    TxTransformation txTransformation = txWeldLocationOperation.LocationRelativeToWorkingFrame;//GetLocationRelativeToObject(txLocatableObject);
                    TxVector txVector = txTransformation.Translation;
                    TxVector txVectorR = txTransformation.RotationRPY_XYZ;

                    abslute_vector[0] = txVector.X;
                    abslute_vector[1] = txVector.Y;
                    abslute_vector[2] = txVector.Z;

                    abslute_vector[3] = txVectorR.X;
                    abslute_vector[4] = txVectorR.Y;
                    abslute_vector[5] = txVectorR.Z;
                }
                else
                {
                    TxTransformation txTransformation = txWeldLocationOperation.GetLocationRelativeToObject(txLocatableObject);
                    TxVector txVector = txTransformation.Translation;
                    TxVector txVectorR = txTransformation.RotationRPY_XYZ;

                    abslute_vector[0] = txVector.X;
                    abslute_vector[1] = txVector.Y;
                    abslute_vector[2] = txVector.Z;

                    abslute_vector[3] = txVectorR.X;
                    abslute_vector[4] = txVectorR.Y;
                    abslute_vector[5] = txVectorR.Z;
                }
            }
            //计算rootappearance，再获取
            else
            {
                //没有appearance的实例
                //找到最顶层的compoundPart
                txCompoundPart_A = appearance1;
                txCompoundPart_B = appearance1.Collection;
                GetRootTxCompoundPart();

                ITxLocatableObject finalTxcom = txCompoundPart_A as ITxLocatableObject;

                TxTransformation txTransformation_1 = txWeldLocationOperation.GetLocationRelativeToObject(finalTxcom);
                TxVector txVector_1 = txTransformation_1.Translation;
                TxVector txVectorR = txTransformation_1.RotationRPY_XYZ;

                abslute_vector[0] = txVector_1.X;
                abslute_vector[1] = txVector_1.Y;
                abslute_vector[2] = txVector_1.Z;

                abslute_vector[3] = txVectorR.X;
                abslute_vector[4] = txVectorR.Y;
                abslute_vector[5] = txVectorR.Z;
            }

            return abslute_vector;

        }
        //Return TxTransformation
        public TxTransformation GetAbsoluteLocation_2(ITxLocatableObject txLocatableObject, TxRoboticViaLocationOperation txWeldLocationOperation)
        {
            TxTransformation txTransformation = null;
            Tecnomatix.Engineering.TxPartAppearance appearance1 = txLocatableObject as Tecnomatix.Engineering.TxPartAppearance;//找到对象的Appearance            

            //没有appearance，直接获取
            if (appearance1 == null)
            {
                if (txLocatableObject == null)
                {
                    txTransformation = txWeldLocationOperation.LocationRelativeToWorkingFrame;//GetLocationRelativeToObject(txLocatableObject);

                }
                else
                {
                    txTransformation = txWeldLocationOperation.GetLocationRelativeToObject(txLocatableObject);
                }
            }
            //计算rootappearance，再获取
            else
            {
                //没有appearance的实例
                //找到最顶层的compoundPart
                txCompoundPart_A = appearance1;
                txCompoundPart_B = appearance1.Collection;
                GetRootTxCompoundPart();

                ITxLocatableObject finalTxcom = txCompoundPart_A as ITxLocatableObject;

                txTransformation = txWeldLocationOperation.GetLocationRelativeToObject(finalTxcom);
            }

            return txTransformation;
        }


        //找到最顶层的compoundPart
        private void GetRootTxCompoundPart()
        {
            txCompoundPart_B = txCompoundPart_A.Collection as Tecnomatix.Engineering.ITxPartAppearance;
            if (txCompoundPart_B != null)
            {
                if (!(txCompoundPart_B.Collection is Tecnomatix.Engineering.ITxPartAppearance))
                {
                    txCompoundPart_A = txCompoundPart_B;
                    goto e;
                }
                else
                {
                    txCompoundPart_A = txCompoundPart_B;
                    GetRootTxCompoundPart();
                }
            }
        e:;
        }

        #endregion

        #region 查找最顶层的apperance
        //计算rootappearance
        public Tecnomatix.Engineering.TxPartAppearance GetRootTxPartAppearance(ITxLocatableObject txLocatableObject)
        {
            try
            {
                Tecnomatix.Engineering.TxPartAppearance result = null;

                Tecnomatix.Engineering.TxPartAppearance appearance1 = txLocatableObject as Tecnomatix.Engineering.TxPartAppearance;//找到对象的Appearance
                if (appearance1 != null)
                {
                    //没有appearance的实例
                    //找到最顶层的compoundPart
                    txCompoundPart_A = appearance1;
                    txCompoundPart_B = appearance1.Collection;
                    GetRootTxCompoundPart();

                    TxPartAppearance finalTxcom = txCompoundPart_A as TxPartAppearance;

                    if (finalTxcom != null)
                    {
                        result = finalTxcom;
                    }
                    else
                    {
                        result = null;
                    }
                }

                return result;
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }

    //Appearance操作类
    public static class TxAppearanceHelper
    {
        //获取选中对象的父级
        public static ITxPartAppearance GetParentPartAppearance(ITxPartAppearance partAppearance)
        {
            if (partAppearance is null)
            {
                return null;
            }

            ITxPartAppearance txPart = partAppearance;

            //判断父级
            while (txPart.Collection is ITxPartAppearance p1)
            {
                txPart = p1;
            }

            return txPart;
        }
    }
}