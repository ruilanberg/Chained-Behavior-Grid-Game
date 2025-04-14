using UnityEngine;

namespace StateMachine
{
    public class StateMachineBase : MonoBehaviour
    {
        private State _currentState = null;

        protected virtual void Update()
        {
            _currentState?.Update(Time.deltaTime);
        }

        public void Init(State state)
        {
            _currentState = state;
            _currentState.Enter();
        }

        public void SwitchState(State newState)
        {
            if(_currentState == newState)
                return;

            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }
    }
}
