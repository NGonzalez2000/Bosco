using Bosco.Core.Services;

namespace Bosco.Core.ViewModels;

public class ProductViewModel
{
    private readonly IFrontendNotifier frontendNotifier;

    public ProductViewModel(IFrontendNotifier frontendNotifier)
	{
        this.frontendNotifier = frontendNotifier;
    }
}
