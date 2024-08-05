using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INeverFall
{
    public class BossRoomTextController : TextController
    {
        protected override void _TriggerExecuted()
        {
            if (GameManager.Instance.IsBossRoomOpened)
                return;
            
            base._TriggerExecuted();
        }
    }
}