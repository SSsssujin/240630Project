using System.Collections;
using System.Collections.Generic;
using INeverFall;
using UnityEngine;

namespace INeverFall.Monster
{
    /*
     *  State Machine for FortGolem
     */
    public class BossStateMachine : StateMachine
    {
        public readonly IdleState IdleState;
        public readonly TraceState TraceState;
        
        private Boss _controller;
        
        public BossStateMachine(Boss controller)
        {
            _controller = controller;
            
            IdleState = new IdleState(controller);
            TraceState = new TraceState(controller);
        }
    }
}