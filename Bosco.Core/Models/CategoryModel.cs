namespace Bosco.Core.Models;

public class CategoryModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public CategoryModel()
    {
        Name = string.Empty;
    }
}
