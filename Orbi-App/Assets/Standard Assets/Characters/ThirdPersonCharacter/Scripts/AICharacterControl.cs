using System;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour
    {
        public NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        public Transform target;                                    // target to aim for
        private float MOVE_RADIUS = 100f;

        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

	        agent.updateRotation = false;
	        agent.updatePosition = true;
        }


        private void Update()
        {
            // out of bounds check TODO
            //Debug.Log(agent.gameObject + " " + agent);
            if (target != null)
                agent.SetDestination(target.position);

            if (agent.remainingDistance > agent.stoppingDistance)
                character.Move(agent.desiredVelocity, false, false);
            else
                character.Move(Vector3.zero, false, false);
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        public void SetTargetPosition(Vector3 pos)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(pos, out hit, MOVE_RADIUS, 1))
            {
                target.transform.position = hit.position;
            }
        }
    }
}
