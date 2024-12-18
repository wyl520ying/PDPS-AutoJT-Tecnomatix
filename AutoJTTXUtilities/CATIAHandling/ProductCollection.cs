





using AutoJTMathUtilities;
using ProductStructureTypeLib;


namespace AutoJTTXUtilities.CATIAHandling
{
  public struct ProductCollection
  {
    public string Name { get; set; }

    public Product iProduct { get; set; }

    public AJTMatrix iPosition { get; set; }
  }
}
