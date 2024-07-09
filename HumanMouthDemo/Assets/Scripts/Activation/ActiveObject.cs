namespace ATG.Activation
{
    public abstract class ActivateObject: IActivateable
    {
        public bool IsActive { get; set; }

        public virtual void SetActive(bool isActive)
        {
            IsActive = isActive;
        }
    }
}