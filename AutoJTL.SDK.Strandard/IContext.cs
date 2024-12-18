





using System;


namespace AutoJTL.SDK.Strandard
{
  public interface IContext
  {
    bool IsSynchronized { get; }

    void Invoke(Action action);

    void BeginInvoke(Action action);
  }
}
