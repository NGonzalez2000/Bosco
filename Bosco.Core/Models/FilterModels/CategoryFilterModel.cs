using Bosco.Core.Services;

namespace Bosco.Core.Models.FilterModels
{
    public class CategoryFilterModel : IFilter
    {
        public CategoryModel Category { get; set; }
        public CategoryFilterModel()
        {
            Category= new();
        }

        // o object is pass to compare if stays visible or not
        public bool Filter(object o)
        {
            bool ret = true;
            if (o is CategoryModel c)
            {
                if(!string.IsNullOrEmpty(Category.Name))
                {
                    if (!c.Name.Contains(Category.Name)) ret = false;
                }
            }
            return ret;
        }
    }
}
