using ATG.Activation;

namespace ATG.Update
{
    public interface IUpdateExecutor: IActivateable
    {
        void Add(IUpdateable update);
        void Remove(IUpdateable update);
    }
}