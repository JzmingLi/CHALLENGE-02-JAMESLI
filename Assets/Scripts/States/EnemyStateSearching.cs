using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Worq;
using static UnityEngine.GraphicsBuffer;

namespace Overtime.FSM.Enemy
{
    public class EnemyStateSearching : EnemyStateBase
    {
        GameObject target;

        public override void BuildTransitions()
        {
            AddTransition(EnemyStateTransition.PLAYERFOUND, EnemyStateID.ALERTED);
            AddTransition(EnemyStateTransition.PLAYERSEEN, EnemyStateID.SHOOTING);
        }

        public override void Enter()
        {
            target = GameObject.FindGameObjectWithTag("Player");
            gameObject.GetComponent<AWSPatrol>().enabled = true;
        }

        public override void Exit()
        {
            gameObject.GetComponent<AWSPatrol>().enabled = false;
        }

        public override void Update()
        {
            if (CheckLineOfSight())
            {
                MakeTransition(EnemyStateTransition.PLAYERSEEN);
                Debug.Log("Player Spotted!");
            }
            else if (GameObject.FindGameObjectWithTag("Player").GetComponent<DetectionBroadcaster>().PLAYER_SOUND)
            {
                MakeTransition(EnemyStateTransition.PLAYERFOUND);
                Debug.Log("Player Heard!");
            }
        }

        private bool CheckLineOfSight()
        {
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<DetectionBroadcaster>().PLAYER_INVISIBLE) return false;

            // Define the ray starting from the current object's position and going towards the target
            Ray ray = new Ray(transform.position, target.transform.position - transform.position);

            // Detection Range
            float maxRaycastDistance = 10f;

            // Perform the raycast
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, maxRaycastDistance))
            {
                // Check if hit player
                if (hitInfo.collider.CompareTag("Player"))
                {
                    // Ray hit player
                    Debug.DrawLine(ray.origin, hitInfo.point, Color.green);
                    return true;
                }
                else
                {
                    // Ray hit something else
                    Debug.DrawLine(ray.origin, hitInfo.point, Color.yellow);
                }
            }
            else
            {
                // Ray didn't hit anything
                Debug.DrawLine(ray.origin, ray.origin + ray.direction * maxRaycastDistance, Color.red);
            }
            return false;
        }
    }
}