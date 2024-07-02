using System.Collections;
using System.Collections.Generic;
using INeverFall;
using UnityEngine;

namespace INeverFall.Monster
{
    /** State Machine for Boss - FortGolem **/
    public class BossStateMachine : StateMachine
    {
        public readonly IdleState IdleState;
        public readonly TraceState TraceState;
        public readonly AttackState AttackState;
        
        private BossMonster _controller;
        
        public BossStateMachine(BossMonster controller)
        {
            _controller = controller;
            
            IdleState = new IdleState(controller);
            TraceState = new TraceState(controller);
            AttackState = new AttackState(controller);
        }
    }
}