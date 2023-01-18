namespace Bosco.Core.Services
{
    public interface IViewModel
    {
        public void Opening();
        public void Closing();
        public void SetDialog(IDialog dialogType);
    }
}
