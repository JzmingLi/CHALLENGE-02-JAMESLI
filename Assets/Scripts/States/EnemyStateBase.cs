using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using Overtime.FSM;

namespace Overtime.FSM.Enemy
{
    public enum EnemyStateID
    {
        SEARCHING,
        ALERTED,
        SHOOTING,
        TOCOVER,
        RELOADING
    }

    public enum EnemyStateTransition
    {
        PLAYERFOUND,
        PLAYERSEEN,
        OUTOFAMMO,
        CHANGINGMAG,
        RELOADED,
        PLAYERLOST,
        PLAYERVISUALLOST
    }

    public abstract class EnemyStateBase : State<EnemyBehaviour, EnemyStateID, EnemyStateTransition>
    {
        public override void BuildTransitions()
        {

        }

        public override void Enter()
        {

        }

        public override void Exit()
        {

        }

        public override void FixedUpdate()
        {

        }

        public override void Update()
        {

        }
    }
}
