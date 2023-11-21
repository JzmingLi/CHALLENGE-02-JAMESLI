using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Worq;
using static UnityEngine.GraphicsBuffer;

namespace Overtime.FSM.Enemy
{
    public class EnemyStateReloading : EnemyStateBase
    {
        float reloadTime;

        public override void BuildTransitions()
        {
            AddTransition(EnemyStateTransition.RELOADED, EnemyStateID.ALERTED);
        }

        public override void Enter()
        {
            reloadTime = 3;
        }

        public override void Update()
        {
            reloadTime -= Time.deltaTime;
            if (reloadTime < 0)
            {
                gameObject.GetComponent<EnemyBehaviour>().ammo = 30;
                MakeTransition(EnemyStateTransition.RELOADED);
                Debug.Log("Reloaded!");
            }
        }
    }
}