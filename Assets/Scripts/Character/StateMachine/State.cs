using System;
using System.Collections;
using System.Collections.Generic;
using INeverFall.Monster;
using UnityEngine;

namespace INeverFall
{
    public abstract class State
    {
        protected Boss Character;
        
        protected State(Boss character)
        {
            Character = character;
        }
        
        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
    }
}
