using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Worq;
using static UnityEngine.GraphicsBuffer;

namespace Overtime.FSM.Enemy
{
    public class EnemyStateShooting : EnemyStateBase
    {
        float timeSinceLastShot;
        GameObject target;
        NavMeshAgent agent;
        

        public override void BuildTransitions()
        {
            AddTransition(EnemyStateTransition.PLAYERVISUALLOST, EnemyStateID.ALERTED);
            AddTransition(EnemyStateTransition.OUTOFAMMO, EnemyStateID.TOCOVER);
        }

        public override void Enter()
        {
            agent = gameObject.GetComponent<NavMeshAgent>();
            agent.speed = 1f;
            
            target = GameObject.FindGameObjectWithTag("Player");
        }
        public override void Exit()
        {
            agent.speed = 3.5f;
        }

        public override void Update()
        {
            Vector3 directionAway = transform.position - target.transform.position;
            agent.SetDestination(transform.position + directionAway);

            timeSinceLastShot += Time.deltaTime;
            if (gameObject.GetComponent<EnemyBehaviour>().ammo > 0)
            {
                if (CheckLineOfSight())
                {
                    if (timeSinceLastShot > 0.1)
                    {
                        GameObject bullet = Instantiate(gameObject.GetComponent<EnemyBehaviour>().bullet, gameObject.transform);
                        bullet.transform.SetParent(null);
                        bullet.GetComponent<Rigidbody>().AddForce((target.transform.position - bullet.transform.position).normalized * 30, ForceMode.Impulse);
                        Destroy(bullet, 5);
                        timeSinceLastShot = 0;
                        gameObject.GetComponent<EnemyBehaviour>().ammo--;
                    }
                }
                else
                {
                    MakeTransition(EnemyStateTransition.PLAYERVISUALLOST);
                    Debug.Log("Visual Lost!");
                }
            }
            else
            {
                MakeTransition(EnemyStateTransition.OUTOFAMMO);
                Debug.Log("Out of ammo! Going for cover!");
            }
        }

        private bool CheckLineOfSight()
        {
            GameObject target = GameObject.FindGameObjectWithTag("Player");
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<DetectionBroadcaster>().PLAYER_INVISIBLE) return false;

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
    }
}