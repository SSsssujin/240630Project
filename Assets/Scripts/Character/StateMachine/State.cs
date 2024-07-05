using System;
using System.Collections;
using System.Collections.Generic;
using INeverFall.Monster;
using UnityEngine;

namespace INeverFall
{
    // [수정] 나중에 interface로 수정할지 생각해보기
    public abstract class State
    {
        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
    }
}
