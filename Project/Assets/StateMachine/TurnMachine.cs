using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    public class TurnMachine : MonoBehaviour
    {
        private Queue<State> _queueEvent = new Queue<State>();
        private State _currentState = null;

        private bool isStarted = false;
        private bool isWaitingEvent = false;

        public void AddEvent(State state)
        {
            if (state == null)
                return;

            _queueEvent.Enqueue(state);
            isWaitingEvent = false;
        }

        public void Init()
        {
            isStarted = true;
            SetEvent();
        }

        private void SetEvent()
        {
            if (_queueEvent.Count <= 0)
            {
                isWaitingEvent = true;
                return;
            }
            else
                isWaitingEvent = false;

            _currentState?.Exit();

            _currentState = _queueEvent.Dequeue();

            _currentState?.Enter();
        }

        private void Update()
        {
            if (!isStarted || isWaitingEvent)
                return;

            _currentState.Update(Time.deltaTime);

            if(_currentState.HasEnd)
                SetEvent();
        }
    }
}
