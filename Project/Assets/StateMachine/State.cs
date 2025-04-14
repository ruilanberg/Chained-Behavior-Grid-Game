namespace StateMachine
{
    public abstract class State
    {
        public bool HasEnd { get; private set; }
        public void EndEvent()
        {
            HasEnd = true;
        }

        public abstract void Enter();
        public abstract void Update(float deltaTime);
        public abstract void Exit();
    }
}
