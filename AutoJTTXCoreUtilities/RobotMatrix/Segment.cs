






namespace AutoJTTXCoreUtilities.RobotMatrix
{
  public class Segment
  {
    public double startAngle;
    public double endAngle;
    public AJTApRmxUtils.EApRmxReachabilityStatus statuts;

    public Segment(
      double start,
      double end,
      AJTApRmxUtils.EApRmxReachabilityStatus reachabilityStatuts)
    {
      this.startAngle = start;
      this.endAngle = end;
      this.statuts = reachabilityStatuts;
    }
  }
}
