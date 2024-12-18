using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.Utilities;
using Tecnomatix.Planning;

namespace AutoJTTXUtilities.Controls
{
    internal class AJTTxMessageHandling
    {
        public AJTTxMessageHandling()
        {
            this.Errors = new Dictionary<string, List<string>>();
            this.Warnings = new Dictionary<string, List<string>>();
        }

        public void AddError(string message, string line)
        {
            this.AddError(this.Errors, message, line);
        }

        public void AddError(Dictionary<string, List<string>> errors, string message, string line)
        {
            if (errors.ContainsKey(message))
            {
                List<string> list = errors[message];
                list.Add(line);
                errors[message] = list;
            }
            else
            {
                errors.Add(message, new List<string>
                {
                    line
                });
            }
        }

        public void AddWarning(string message, string line)
        {
            this.AddWarning(this.Warnings, message, line);
        }

        public void AddWarning(Dictionary<string, List<string>> warnings, string message, string line)
        {
            if (!warnings.ContainsKey(message))
            {
                warnings.Add(message, new List<string>
                {
                    line
                });
            }
            else
            {
                List<string> list = warnings[message];
                list.Add(line);
                warnings[message] = list;
            }
        }

        public bool HasErrors()
        {
            return this.Errors.Count > 0;
        }

        public bool HasWarnings()
        {
            return this.Warnings.Count > 0;
        }

        public void ResetMessages()
        {
            this.Errors = new Dictionary<string, List<string>>();
            this.Warnings = new Dictionary<string, List<string>>();
        }

        public void ResetMessages(Dictionary<string, List<string>> errors, Dictionary<string, List<string>> warnings)
        {
            errors = new Dictionary<string, List<string>>();
            warnings = new Dictionary<string, List<string>>();
        }

        public void ShowMessages(string title = "Process Simulate")
        {
            this.ShowMessages(this.Errors, this.Warnings, title);
        }

        public void ShowMessages(Dictionary<string, List<string>> errors, Dictionary<string, List<string>> warnings, string title = "Process Simulate")
        {
            if (errors == null)
            {
                errors = new Dictionary<string, List<string>>();
            }
            if (warnings == null)
            {
                warnings = new Dictionary<string, List<string>>();
            }
            if (errors.Count > 0 || warnings.Count > 0)
            {
                Hashtable hashtable = new Hashtable();
                Hashtable hashtable2 = new Hashtable();
                foreach (KeyValuePair<string, List<string>> keyValuePair in errors)
                {
                    hashtable2.Add(keyValuePair.Key, keyValuePair.Value.ToArray());
                }
                foreach (KeyValuePair<string, List<string>> keyValuePair2 in warnings)
                {
                    hashtable.Add(keyValuePair2.Key, keyValuePair2.Value.ToArray());
                }
                try
                {
                    if (errors.Count == 0)
                    {
                        TxErrorWindow.ShowDialog(title, "Error Window Message Warnings", hashtable2, hashtable, true);
                    }
                    else if (warnings.Count == 0)
                    {
                        TxErrorWindow.ShowDialog(title, "Error Window Message Errors", hashtable2, hashtable, true);
                    }
                    else
                    {
                        TxErrorWindow.ShowDialog(title, "Error Window Message Errors And Warnings", hashtable2, hashtable, true);
                    }
                }
                catch
                {
                }
                this.ResetMessages(errors, warnings);
            }
        }

        public static void WriteException(Exception exception)
        {
            if (!TxApplication.LogWriter.Exists)
            {
                AJTTxMessageHandling._lastError = AJTTxMessageHandling.GenerateExceptionLine(exception, true);
            }
            else
            {
                AJTTxMessageHandling.WriteError(AJTTxMessageHandling.GenerateExceptionLine(exception, true));
            }
        }

        private static string GetObjectMessageString(ITxObject obj)
        {
            string text = string.Empty;
            if (obj != null)
            {
                try
                {
                    if (!(obj is ITxProcessModelObject))
                    {
                        if (obj is ITxPlanningObject)
                        {
                            ITxPlanningObject txPlanningObject = obj as ITxPlanningObject;
                            if (txPlanningObject.IsObjectLoaded())
                            {
                                text = text + " Object: " + obj.Name;
                            }
                            text += string.Format(" Object: {0}, ,{1}", txPlanningObject.PlanningObjectType, txPlanningObject.PlanningType);
                        }
                    }
                    else
                    {
                        text = text + " Object: " + obj.Name;
                        ITxProcessModelObject txProcessModelObject = obj as ITxProcessModelObject;
                        text += string.Format(" ,{0}, {1}", txProcessModelObject.ProcessModelId.ExternalId, txProcessModelObject.eMSType);
                        if (txProcessModelObject.PlanningRepresentation != null)
                        {
                            text += string.Format(" ,{0}, ,{1}", txProcessModelObject.PlanningRepresentation.PlanningObjectType, txProcessModelObject.PlanningRepresentation.PlanningType);
                        }
                    }
                }
                catch
                {
                }
            }
            return text;
        }

        public static void WriteInfo(string message, ITxObject obj)
        {
            message += AJTTxMessageHandling.GetObjectMessageString(obj);
            if (TxApplication.LogWriter.Exists)
            {
                TxApplication.LogWriter.WriteInfoLine(message);
            }
            if (AJTTxMessageHandling.WriteVSOutput)
            {
            }
        }

        public static void WriteInfo(string message)
        {
            AJTTxMessageHandling.WriteInfo(message, null);
        }

        public static void WriteDetailed(string message)
        {
            if (TxApplication.LogWriter.Exists)
            {
                TxApplication.LogWriter.WriteDetailedLine(message);
            }
            if (!AJTTxMessageHandling.WriteVSOutput)
            {
            }
        }

        public static void WriteError(string message, ITxObject obj)
        {
            string objectMessageString = AJTTxMessageHandling.GetObjectMessageString(obj);
            if (TxApplication.LogWriter.Exists)
            {
                TxApplication.LogWriter.WriteErrorLine(message += objectMessageString);
            }
            AJTTxMessageHandling._lastError = message;
            if (!AJTTxMessageHandling.WriteVSOutput)
            {
            }
        }

        public static void WriteError(string message)
        {
            AJTTxMessageHandling.WriteError(message, null);
        }

        public static void WritePerformance(string message)
        {
            if (TxApplication.LogWriter.Exists)
            {
                TxApplication.LogWriter.WritePerformanceLine(message);
            }
            if (AJTTxMessageHandling.WriteVSOutput)
            {
            }
        }

        public static string GenerateExceptionLine(Exception exception, bool includeStack)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Exception: ");
            stringBuilder.Append(exception.GetType().Name);
            stringBuilder.Append("; Message: '");
            stringBuilder.Append(exception.Message);
            stringBuilder.Append("'");
            if (includeStack)
            {
                string text = exception.StackTrace;
                if (text == null || text.Length == 0)
                {
                    text = new StackTrace(1, true).ToString();
                }
                stringBuilder.Append("; Call Stack: ");
                stringBuilder.Append(text);
            }
            string text2 = stringBuilder.ToString();
            AJTTxMessageHandling._lastError = text2;
            return text2;
        }

        public static string GetLastError()
        {
            return AJTTxMessageHandling._lastError;
        }

        public static bool WriteVSOutput = false;

        private static string _lastError = string.Empty;

        private Dictionary<string, List<string>> Errors = null;

        private Dictionary<string, List<string>> Warnings = null;
    }
}
