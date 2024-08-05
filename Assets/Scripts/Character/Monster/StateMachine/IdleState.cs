using UnityEngine;
using INeverFall.Player;

namespace INeverFall.Monster
{
    public class IdleState : IState
    {
        private readonly BossMonster _controller;

        private float _entranceTimer;
        private float _randomTimer;
        
        private PlayerCharacter _player;
        private BossStateMachine _stateMachine;

        public IdleState(BossMonster controller)
        {
            _controller = controller;
        }
        
        public void Enter()
        {
            _stateMachine = _controller.StateMachine;
            _randomTimer = Random.Range(0.5f, 1.0f);
        }

        private bool _isPlayerEntered;

        // [보충] if문 없애고 깔끔하게 상태 전환 할 수 있는 방법 찾아보기
        public void Update()
        {
            if (!PlayerChecker.Instance.IsPlayerInBossRoom)
            {
                return;
            }
            else if (!_isPlayerEntered)
            {
                _controller.Animator.SetTrigger(AnimationID.RiseTrigger);
                _isPlayerEntered = true;
            }
            
            // Wait for Rise animation
            if ( _controller.Animator.IsSpecificAnimationPlaying("Stun_Loop") || 
                 _controller.Animator.IsSpecificAnimationPlaying(BossAnimation.Rise))
            {
                return;
            }
            
            _entranceTimer += Time.deltaTime;

            if (_entranceTimer <= _randomTimer)
                return;
            
            if (_stateMachine.FrontAttackState.IsReady)
            {
                _TransitState(_stateMachine.FrontAttackState);
                return;
            }
            if (_stateMachine.ThrowStoneAttackState.IsReady)
            {
                _TransitState(_stateMachine.ThrowStoneAttackState);
                return;
            }
            if (_stateMachine.DashAttackState.IsReady)
            {
                _TransitState(_stateMachine.DashAttackState);
                return;
            }
            _TransitState(_stateMachine.TraceState);
        }

        private void _TransitState(IState state)
        {
            _stateMachine.TransitionTo(state);
        }

        public void Exit()
        {
            _entranceTimer = 0;
        }
    }
}