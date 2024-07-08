using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INeverFall
{
    public abstract class StateMachine
    {
        public State CurrentState { get; private set; }

        public void Initialize(State state)
        {
            CurrentState = state;
            state.Enter();
            
            StateChanged?.Invoke(state);
        }
        
        public void TransitionTo(State nextState)
        {
            //Debug.LogError("Next state : " + nextState);
        
            CurrentState.Exit();
            CurrentState = nextState;
            nextState.Enter();

            // notify other objects that state has changed
            StateChanged?.Invoke(nextState);
        }
    
        public void Update()
        {
            if (CurrentState != null)
            {
                CurrentState.Update();
            }
        }
        
        public event Action<State> StateChanged;
    }
}