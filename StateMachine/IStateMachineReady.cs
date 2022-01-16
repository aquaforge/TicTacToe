namespace StateMachineLibrary
{
    public interface IStateMachineReady
    {
        public int State { get; }
        internal void StateChange(int endState, Action<object>? action, object arguments);
    }

}