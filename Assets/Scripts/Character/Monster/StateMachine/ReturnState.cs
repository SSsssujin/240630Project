using System.Collections;
using System.Collections.Generic;
using INeverFall;
using UnityEngine;

namespace INeverFall.Monster
{
    public class ReturnState : IState
    {
        private readonly BossMonster _controller;

        public ReturnState(BossMonster controller)
        {
            _controller = controller;
        }
        
        public void Enter()
        {
        }

        public void Update()
        {
            var spawnPoint = GameObject.Find("SpawnPoint");
            _controller.NavMeshAgent.SetDestination(spawnPoint.transform.position);
        }

        public void Exit()
        {
        }
    }
}