using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Worq;
using static UnityEngine.GraphicsBuffer;

namespace Overtime.FSM.Enemy
{
    public class EnemyStateRunToCover : EnemyStateBase
    {
        GameObject target;
        NavMeshAgent agent;
        GameObject targetWaypoint;

        public override void BuildTransitions()
        {
            AddTransition(EnemyStateTransition.CHANGINGMAG, EnemyStateID.RELOADING);
        }

        public override void Enter()
        {
            agent = gameObject.GetComponent<NavMeshAgent>();
            agent.speed = 8f;

            target = GameObject.FindGameObjectWithTag("Player");

            targetWaypoint = FindCover();
            agent.destination = targetWaypoint.transform.position;
        }
        public override void Exit()
        {
            agent.speed = 3.5f;
        }

        public override void Update()
        {
            if (agent.remainingDistance < 1)
            {
                MakeTransition(EnemyStateTransition.CHANGINGMAG);
                Debug.Log("Reloading!");
            }
        }

        private bool CheckLineOfSight()
        {
            GameObject target = GameObject.FindGameObjectWithTag("Player");

            // Define the ray starting from the current object's position and going towards the target
            Ray ray = new Ray(transform.position, target.transform.position - transform.position);

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
        private GameObject FindCover()
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
                    if (!hitInfo.collider.CompareTag("Player"))
                    {
                        // Ray hit player
                        Debug.DrawLine(ray.origin, hitInfo.point, Color.green);
                        Debug.Log(waypoint);
                        return waypoint;
                        
                    }
                }
            }

            //if not found, use random waypoint
            Debug.Log("couldn't find cover!");
            return GameObject.FindGameObjectWithTag("Waypoint");
            
        }
    }
}