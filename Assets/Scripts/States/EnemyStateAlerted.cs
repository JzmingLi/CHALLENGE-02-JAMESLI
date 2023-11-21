using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Worq;
using static UnityEngine.GraphicsBuffer;

namespace Overtime.FSM.Enemy
{
    public class EnemyStateAlerted : EnemyStateBase
    {
        GameObject targetWaypoint;
        GameObject target;

        public override void BuildTransitions()
        {
            AddTransition(EnemyStateTransition.PLAYERSEEN, EnemyStateID.SHOOTING);
            AddTransition(EnemyStateTransition.PLAYERLOST, EnemyStateID.SEARCHING);
        }

        public override void Enter()
        {
            target = GameObject.FindGameObjectWithTag("Player");
            FindShootingPosition();
        }

        public override void Update()
        {
            if (!CheckShootingPosition())
            {
                FindShootingPosition();
            }

            gameObject.GetComponent<NavMeshAgent>().destination = targetWaypoint.transform.position;
            
            if (CheckLineOfSight())
            {
                if (GameObject.FindGameObjectWithTag("Player").GetComponent<DetectionBroadcaster>().PLAYER_INVISIBLE)
                {
                    MakeTransition(EnemyStateTransition.PLAYERLOST);
                    Debug.Log("Player Lost!");
                }
                else
                {
                    MakeTransition(EnemyStateTransition.PLAYERSEEN);
                    Debug.Log("Player Spotted!");
                }
            }
        }

        private bool CheckLineOfSight()
        {
            // Define the ray starting from the current object's position and going towards the target
            Ray ray = new Ray(transform.position, target.transform.position - transform.position);

            // Detection Range
            float maxRaycastDistance = 100f;

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

        private void FindShootingPosition()
        {
            GameObject[] waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
            foreach (GameObject waypoint in waypoints)
            {
                GameObject target = GameObject.FindGameObjectWithTag("Player");
                Ray ray = new Ray(waypoint.transform.position, target.transform.position - waypoint.transform.position);

                // Detection Range
                float maxRaycastDistance = 100f;

                // Perform the raycast
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo, maxRaycastDistance))
                {
                    // Check if hit player
                    if (hitInfo.collider.CompareTag("Player"))
                    {
                        // Ray hit player
                        Debug.DrawLine(ray.origin, hitInfo.point, Color.green);
                        targetWaypoint = waypoint;
                        break;
                    }
                }
            }
        }

        private bool CheckShootingPosition()
        {
            
            Ray ray = new Ray(targetWaypoint.transform.position, target.transform.position - targetWaypoint.transform.position);

            // Detection Range
            float maxRaycastDistance = 100f;

            // Perform the raycast
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, maxRaycastDistance, gameObject.GetComponent<EnemyBehaviour>().layermask))
            {
                // Check if hit player
                if (hitInfo.collider.CompareTag("Player"))
                {
                    // Ray hit player
                    Debug.DrawLine(ray.origin, hitInfo.point, Color.green);
                    return true;
                }
            }
            return false;
        }
    }
}