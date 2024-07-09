namespace ATG.StateMachine
{
    public abstract class StatementWithData<T>: Statement
    {
        public StatementWithData(IStateSwitcher sw): base(sw) {}
        public abstract void SetupData(T data);
    }
}
