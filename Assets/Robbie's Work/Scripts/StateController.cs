using System.Collections.Generic;
using System;
using UnityEngine;

namespace RobbieWagnerGames
{
    public abstract class StateController<EState> where EState : Enum
    {
        protected Dictionary<EState, BaseState<EState>> States = new Dictionary<EState, BaseState<EState>>();
        protected BaseState<EState> CurrentState;

        public virtual void Initialize()
        {
            CurrentState.EnterState();
        }

        public virtual void UpdateState()
        {
            EState nextStateKey = CurrentState.GetNextState();

            if(nextStateKey.Equals(CurrentState.StateKey))
            {
                CurrentState.UpdateState();
            }
            else
            {
                TransitionState(nextStateKey);
            }
        }

        public virtual void TransitionState(EState stateKey)
        {
            CurrentState.ExitState();
            CurrentState = States[stateKey];
            CurrentState.EnterState();
        }
    }
}