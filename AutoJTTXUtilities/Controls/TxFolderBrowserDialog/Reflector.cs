





using System;
using System.Reflection;


namespace AutoJTTXUtilities.Controls.TxFolderBrowserDialog
{
  public class Reflector
  {
    private string m_ns;
    private Assembly m_asmb;

    public Reflector(string ns)
      : this(ns, ns)
    {
    }

    public Reflector(string an, string ns)
    {
      this.m_ns = ns;
      this.m_asmb = (Assembly) null;
      foreach (AssemblyName referencedAssembly in Assembly.GetExecutingAssembly().GetReferencedAssemblies())
      {
        if (referencedAssembly.FullName.StartsWith(an))
        {
          this.m_asmb = Assembly.Load(referencedAssembly);
          break;
        }
      }
    }

    public Type GetType(string typeName)
    {
      Type type = (Type) null;
      string[] strArray = typeName.Split('.');
      if (strArray.Length != 0)
        type = this.m_asmb.GetType(this.m_ns + "." + strArray[0]);
      for (int index = 1; index < strArray.Length; ++index)
        type = type.GetNestedType(strArray[index], BindingFlags.NonPublic);
      return type;
    }

    public object New(string name, params object[] parameters)
    {
      foreach (ConstructorInfo constructor in this.GetType(name).GetConstructors())
      {
        try
        {
          return constructor.Invoke(parameters);
        }
        catch
        {
        }
      }
      return (object) null;
    }

    public object Call(object obj, string func, params object[] parameters)
    {
      return this.Call2(obj, func, parameters);
    }

    public object Call2(object obj, string func, object[] parameters)
    {
      return this.CallAs2(obj.GetType(), obj, func, parameters);
    }

    public object CallAs(Type type, object obj, string func, params object[] parameters)
    {
      return this.CallAs2(type, obj, func, parameters);
    }

    public object CallAs2(Type type, object obj, string func, object[] parameters)
    {
      return type.GetMethod(func, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Invoke(obj, parameters);
    }

    public object Get(object obj, string prop) => this.GetAs(obj.GetType(), obj, prop);

    public object GetAs(Type type, object obj, string prop)
    {
      return type.GetProperty(prop, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue(obj, (object[]) null);
    }

    public object GetEnum(string typeName, string name)
    {
      return this.GetType(typeName).GetField(name).GetValue((object) null);
    }
  }
}
