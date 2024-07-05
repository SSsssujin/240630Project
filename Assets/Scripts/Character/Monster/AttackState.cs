using System.Collections;
using System.Collections.Generic;
using INeverFall;
using INeverFall.Monster;
using UnityEngine;

public abstract class AttackState : State
{
    protected readonly BossMonster _controller;

    private Coroutine _coolTimeCoroutine;
    private WaitForSeconds _coolTimeWFS;

    protected AttackState(BossMonster controller)
    {
        _controller = controller;
    }
    
    public sealed override void Enter()
    {
        _StartCoolTimeCoroutine();
        _Enter();
    }

    public sealed override void Update()
    {
        _Update();
    }

    public sealed override void Exit()
    {
        _Exit();
        //_EndCoolTimeCoroutine();
    }

    private void _StartCoolTimeCoroutine()
    {
        _coolTimeCoroutine = _controller.StartCoroutine(_cStartCoolTimeCoroutine());
    }

    private void _EndCoolTimeCoroutine()
    {
        if (_coolTimeCoroutine != null)
        {
            _controller.StopCoroutine(_coolTimeCoroutine);
            _coolTimeCoroutine = null;
            IsReady = false;
        }
    }

    private IEnumerator _cStartCoolTimeCoroutine()
    {
        _coolTimeWFS ??= new WaitForSeconds(CooldownTime);

        IsReady = false;
        yield return _coolTimeWFS;
        IsReady = true;
        
        Debug.Log(this.GetType().Name + " is ready");
    }

    // 쿨타임 체크 로직 때문에 AttackState만 State 메서드들 따로 관리
    protected virtual void _Enter() { }
    protected virtual void _Update() { }
    protected virtual void _Exit() { }

    // 각 Attack State의 쿨타임을 반드시 설정해주기 위해 abstract 프로퍼티로
    protected abstract float CooldownTime { get; set; }

    public bool IsReady { get; private set; } = true;
}
