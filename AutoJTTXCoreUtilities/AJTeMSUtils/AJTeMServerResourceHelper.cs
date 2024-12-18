using EMPAPPLICATIONLib;
using EMPMODELLib;
using EMPTYPELIBRARYLib;
using EngineeringInternalExtension;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Tecnomatix.Engineering;
using Tecnomatix.Planning;

namespace AutoJTTXCoreUtilities.AJTeMSUtils
{
    public class AJTeMServerResourceHelper
    {
        #region Static method

        public static EmpObjectKey[] GetResourceChildren(EmpContext context, int internalID)
        {
            EmpObjectKey empObjectKey = new EmpObjectKey
            {
                objectId = internalID
            };
            return AJTeMServerResourceHelper.GetResourceChildren(context, empObjectKey);
        }
        public static EmpObjectKey[] GetResourceChildren(EmpContext context, EmpObjectKey objectKey)
        {
            try
            {
                EmpCompoundResource empCompoundResource = new EmpCompoundResourceClass();
                return empCompoundResource.GetChildren(ref context, ref objectKey);
            }
            catch (Exception ex)
            {
                AJTTxMessageHandling.WriteException(ex);
            }
            return new EmpObjectKey[0];
        }


        public static EmpObjectKey[] GetCollectionChildren(EmpContext context, int internalID)
        {
            EmpObjectKey empObjectKey = new EmpObjectKey
            {
                objectId = internalID
            };
            return AJTeMServerResourceHelper.GetCollectionChildren(context, empObjectKey);
        }
        public static EmpObjectKey[] GetCollectionChildren(EmpContext context, EmpObjectKey objectKey)
        {
            try
            {
                EmpCollection empCollection = new EmpCollectionClass();
                return empCollection.GetChildren(ref context, ref objectKey);
            }
            catch (Exception ex)
            {
                AJTTxMessageHandling.WriteException(ex);
            }
            return new EmpObjectKey[0];
        }


        public static string GetName(EmpContext context, EmpObjectKey objectKey)
        {
            try
            {
                return ((IEmpNode)new EmpNodeClass()).get_Name(ref context, ref objectKey);
            }
            catch (Exception ex)
            {
                AJTTxMessageHandling.WriteException(ex);
            }
            return null;
        }

        public static bool SetFileNameStatic(EmpContext empContext, EmpObjectKey threeDRepKey ,string fileName)
        {
            bool result = false;
            try
            {
                IEmpExternalDocument empExternalDocument = (EmpExternalDocument)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("84563F22-83EE-11D4-8DAE-00D0B717BCE5")));

                empExternalDocument.set_Filename(ref empContext, ref threeDRepKey, fileName);

                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        #endregion

        #region Field

        private EmpApplication pd;

        private EmpContext context;



        private ITxPlanningObject itxplanning_obj;



        private EmpObjectKey itxplanning_objKey;

        public EmpObjectKey m_objPrototypeKey { get; set; }

        public EmpObjectKey[] m_instances { get; set; }

       



        //private EmpCollection empCollection = (EmpCollection)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("94F83278-1043-11D4-A08B-00104B17FD2C")));

        //private EmpObject empObject = (EmpObject)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("2BFDC393-83E5-11D4-8DAC-00D0B717BCE5")));

        //private EmpTypeDefinition empTypeDefinition = (EmpTypeDefinition)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("ABDB0A13-563B-11D4-958C-080009D5C296")));

        private EmpNode empNode = (EmpNode)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("94F83299-1043-11D4-A08B-00104B17FD2C")));

        //private EmpTreeServices treeServices = (EmpTreeServices)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("D88663C8-D4BF-11D4-A0D6-00104B17FD2C")));

        private EmpToolPrototype empToolPrototype = (EmpToolPrototype)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("94F832F8-1043-11D4-A08B-00104B17FD2C")));

        private EmpPartPrototype empPartPrototype = (EmpPartPrototype)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("94F832C4-1043-11D4-A08B-00104B17FD2C")));

        private EmpToolInstance empToolInstance = (EmpToolInstance)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("94F832F4-1043-11D4-A08B-00104B17FD2C")));

        private EmpPartInstance empPartInstance = (EmpPartInstance)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("94F832BB-1043-11D4-A08B-00104B17FD2C")));

        private EmpEquipmentInstance empEquipmentInstance = (EmpEquipmentInstance)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("94F83307-1043-11D4-A08B-00104B17FD2C")));

        private EmpExternalDocument empExternalDocument = (EmpExternalDocument)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("84563F22-83EE-11D4-8DAE-00D0B717BCE5")));



        public static List<string> s_legalPrototypeTypes => new List<string>
        {
            "PmPartPrototype", "PmToolPrototype", "PmEquipmentPrototype", "Cell", "Clamp", "Container", "Conveyer", "Device", "Dock_System", "Fixture",
            "Flange", "Gripper", "Gun", "Human", "LightSensor", "ResourcePlaceholder", "ProximitySensor", "Robot", "Security_Window", "Turn_Table",
            "Work_Table"
        };

        public static List<string> s_legalInstanceTypes => new List<string>
        {
            "PmPartInstance", "PmToolInstance", "PmEquipmentInstance"
        };

        #endregion

        public AJTeMServerResourceHelper()
        {
            this.pd = (EmpApplication)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("A11B3AD7-1B9F-11D5-8E0C-0060080B4115")));
            this.context = this.pd.Context;
        }

        public string GetFileNameWithSysRoot(int _objKey, out EmpObjectKey threeDRepKey)
        {
            string result = string.Empty;
            threeDRepKey = default;

            try
            {
                result = AJTTxDocumentUtilities.GetFullPath(this.GetFileName(_objKey, out threeDRepKey));
            }
            catch
            {
                result = "";
            }

            return result;
        }

        public string GetFileName(int _objKey, out EmpObjectKey threeDRepKey, bool isGetInstances = false)
        {
            string filename = "";
            threeDRepKey = default;

            try
            {
                this.itxplanning_obj = AJTTxPlanningObjectUtilities.GetPlanningObjtByInternalID(_objKey);
                ITxEmsServicesProvider txEms = this.itxplanning_obj?.PlatformServicesProvider as ITxEmsServicesProvider;
                if (txEms != null)
                {
                    this.itxplanning_objKey.objectId = txEms.InternalId;
                }

                string type = this.itxplanning_obj.PlanningType;
                bool flag = s_legalPrototypeTypes.Contains(type);
                if (flag)
                {
                    this.m_objPrototypeKey = this.itxplanning_objKey;
                    bool flag2 = type == "PmPartPrototype";
                    if (flag2)
                    {
                        IEmpPartPrototype empPartPrototype = this.empPartPrototype;
                        EmpContext empContext = this.context;
                        EmpObjectKey empObjectKey = this.m_objPrototypeKey;
                        threeDRepKey = empPartPrototype.GetThreeDRep(ref empContext, ref empObjectKey);
                        IEmpExternalDocument empExternalDocument = this.empExternalDocument;
                        empContext = this.context;
                        empObjectKey = threeDRepKey;
                        filename = empExternalDocument.get_Filename(ref empContext, ref empObjectKey);

                        if (isGetInstances)
                        {
                            IEmpPartPrototype empPartPrototype2 = this.empPartPrototype;
                            empContext = this.context;
                            empObjectKey = this.m_objPrototypeKey;
                            this.m_instances = empPartPrototype2.GetInstances(ref empContext, ref empObjectKey);
                        }                       
                    }
                    else
                    {
                        IEmpToolPrototype empToolPrototype = this.empToolPrototype;
                        EmpContext empContext = this.context;
                        EmpObjectKey empObjectKey = this.m_objPrototypeKey;
                        threeDRepKey = empToolPrototype.GetThreeDRep(ref empContext, ref empObjectKey);
                        IEmpExternalDocument empExternalDocument2 = this.empExternalDocument;
                        empContext = this.context;
                        empObjectKey = threeDRepKey;
                        filename = empExternalDocument2.get_Filename(ref empContext, ref empObjectKey);

                        if (isGetInstances)
                        {
                            IEmpToolPrototype empToolPrototype2 = this.empToolPrototype;
                            empContext = this.context;
                            empObjectKey = this.m_objPrototypeKey;
                            this.m_instances = empToolPrototype2.GetInstances(ref empContext, ref empObjectKey);
                        }                        
                    }
                }
                else
                {
                    bool flag3 = s_legalInstanceTypes.Contains(type);
                    if (flag3)
                    {
                        bool flag4 = type == "PmPartInstance";
                        if (flag4)
                        {
                            IEmpPartInstance empPartInstance = this.empPartInstance;
                            EmpContext empContext = this.context;
                            EmpObjectKey empObjectKey = this.itxplanning_objKey;
                            this.m_objPrototypeKey = empPartInstance.get_Prototype(ref empContext, ref empObjectKey);
                            IEmpPartPrototype empPartPrototype3 = this.empPartPrototype;
                            empContext = this.context;
                            empObjectKey = this.m_objPrototypeKey;
                            threeDRepKey = empPartPrototype3.GetThreeDRep(ref empContext, ref empObjectKey);
                            IEmpExternalDocument empExternalDocument3 = this.empExternalDocument;
                            empContext = this.context;
                            empObjectKey = threeDRepKey;
                            filename = empExternalDocument3.get_Filename(ref empContext, ref empObjectKey);

                            if (isGetInstances)
                            {
                                IEmpPartPrototype empPartPrototype4 = this.empPartPrototype;
                                empContext = this.context;
                                empObjectKey = this.m_objPrototypeKey;
                                this.m_instances = empPartPrototype4.GetInstances(ref empContext, ref empObjectKey);
                            }                            
                        }
                        else
                        {
                            bool flag5 = type == "PmToolInstance";
                            if (flag5)
                            {
                                IEmpToolInstance empToolInstance = this.empToolInstance;
                                EmpContext empContext = this.context;
                                EmpObjectKey empObjectKey = this.itxplanning_objKey;
                                this.m_objPrototypeKey = empToolInstance.get_Prototype(ref empContext, ref empObjectKey);
                                IEmpToolPrototype empToolPrototype3 = this.empToolPrototype;
                                empContext = this.context;
                                empObjectKey = this.m_objPrototypeKey;
                                threeDRepKey = empToolPrototype3.GetThreeDRep(ref empContext, ref empObjectKey);
                                IEmpExternalDocument empExternalDocument4 = this.empExternalDocument;
                                empContext = this.context;
                                empObjectKey = threeDRepKey;
                                filename = empExternalDocument4.get_Filename(ref empContext, ref empObjectKey);

                                if (isGetInstances)
                                {
                                    IEmpToolPrototype empToolPrototype4 = this.empToolPrototype;
                                    empContext = this.context;
                                    empObjectKey = this.m_objPrototypeKey;
                                    this.m_instances = empToolPrototype4.GetInstances(ref empContext, ref empObjectKey);
                                }                                
                            }
                            else
                            {
                                bool flag6 = type == "PmEquipmentInstance";
                                if (flag6)
                                {
                                    IEmpEquipmentInstance empEquipmentInstance = this.empEquipmentInstance;
                                    EmpContext empContext = this.context;
                                    EmpObjectKey empObjectKey = this.itxplanning_objKey;
                                    this.m_objPrototypeKey = empEquipmentInstance.get_Prototype(ref empContext, ref empObjectKey);
                                    IEmpToolPrototype empToolPrototype5 = this.empToolPrototype;
                                    empContext = this.context;
                                    empObjectKey = this.m_objPrototypeKey;
                                    threeDRepKey = empToolPrototype5.GetThreeDRep(ref empContext, ref empObjectKey);
                                    IEmpExternalDocument empExternalDocument5 = this.empExternalDocument;
                                    empContext = this.context;
                                    empObjectKey = threeDRepKey;
                                    filename = empExternalDocument5.get_Filename(ref empContext, ref empObjectKey);

                                    if (isGetInstances)
                                    {
                                        IEmpToolPrototype empToolPrototype6 = this.empToolPrototype;
                                        empContext = this.context;
                                        empObjectKey = this.m_objPrototypeKey;
                                        this.m_instances = empToolPrototype6.GetInstances(ref empContext, ref empObjectKey);
                                    }                                   
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                return "";
            }

            return filename;
        }
    }
}