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
        public readonly FrontAttackState FrontAttackState;
        public readonly DashAttackState DashAttackState;
        public readonly ThrowStoneAttackState ThrowStoneAttackState;
        
        private BossMonster _controller;
        
        public BossStateMachine(BossMonster controller)
        {
            _controller = controller;
            
            IdleState = new IdleState(controller);
            TraceState = new TraceState(controller);
            FrontAttackState = new FrontAttackState(controller);
            DashAttackState = new DashAttackState(controller);
            ThrowStoneAttackState = new ThrowStoneAttackState(controller);
        }
    }
}