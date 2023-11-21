using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Overtime.FSM;

namespace Overtime.FSM.Enemy
{

	public class EnemyBehaviour : MonoBehaviour 
	{
		public GameObject bullet;
		public int ammo;
		public LayerMask layermask;

		private StateMachine<EnemyBehaviour, EnemyStateID, EnemyStateTransition> m_FSM;
		public StateMachine<EnemyBehaviour, EnemyStateID, EnemyStateTransition> FSM
		{
			get { return m_FSM; }
		}

		public EnemyStateID m_InitialState;

		public ScriptableObject[] m_States;

		public bool m_Debug;

		void OnDestroy()
		{
			m_FSM.Destroy();
		}

		void Start()
		{
			m_FSM = new StateMachine<EnemyBehaviour, EnemyStateID, EnemyStateTransition>(this, m_States, m_InitialState, m_Debug);
			ammo = 30;
		}

		void Update()
		{
			m_FSM.Update();
		}

		void FixedUpdate()
		{
			m_FSM.FixedUpdate();
		}

		void OnTriggerEnter(Collider col)
		{
			m_FSM.OnTriggerEnter(col);
		}

		#if UNITY_EDITOR
		void OnGUI()
		{
			if(m_Debug)
			{
				GUI.color = Color.white;
				GUI.Label(new Rect(0.0f, 0.0f, 500.0f, 500.0f), string.Format("Example State: {0}", FSM.CurrentStateName));
			}
		}
		#endif
	}
}
