namespace Bosco.Core.Models;

public class BrandModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsDolarValue { get; set; }
    public BrandModel()
    {
        Name = string.Empty;
    }
}
