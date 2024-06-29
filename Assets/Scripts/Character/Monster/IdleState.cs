using System.Collections;
using System.Collections.Generic;
using INeverFall;
using UnityEngine;

namespace INeverFall.Monster
{
    public class IdleState : State
    {
        private PlayerCharacter _player;

        public IdleState(Boss character) : base(character)
        {
            
        }
        
        public override void Enter()
        {
            if (_TryFindCharacter(out _player))
            {
                
            }
        }

        public override void Update()
        {

        }

        public override void Exit()
        {
            
        }

        private bool _TryFindCharacter(out PlayerCharacter player)
        {
            player = Object.FindFirstObjectByType<PlayerCharacter>();
            return player != null;
        }
    }
}