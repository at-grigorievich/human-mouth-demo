namespace ATG.Activation
{
    public interface IActivateable
    {
        bool IsActive {get;}
        void SetActive(bool isActive);
    }
}