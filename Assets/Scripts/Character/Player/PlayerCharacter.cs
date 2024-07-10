using System.Collections;
using System.Collections.Generic;
using INeverFall;
using UnityEngine;

namespace INeverFall.Player
{
    public class PlayerCharacter : CharacterBase
    {
        protected override void Start()
        {
            _maxHp = _hp = 100;
            _attackPower = 1000;
        }
    }
}