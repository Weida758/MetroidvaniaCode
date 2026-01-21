using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace CrashCourse.BehaviorTree
{
    public interface IStrategy
    {
        Node.Status Process();
        void Reset();
    }

    public class PatrolStrategy : IStrategy
    {
        private readonly Transform entity;
        private readonly NavMeshAgent agent;
        private readonly List<Transform> patrolPoints;
        private readonly float patrolSpeed;
        private int currentIndex;
        private bool isPathCalculated;

        public PatrolStrategy(Transform entity, NavMeshAgent agent, List<Transform> patrolPoints,
            float patrolSpeed = 2f)
        {
            this.entity = entity;
            this.agent = agent;
            this.patrolPoints = patrolPoints;
            this.patrolSpeed = patrolSpeed;
        }

        public Node.Status Process()
        {
            if (currentIndex == patrolPoints.Count) return Node.Status.Success;

            var target = patrolPoints[currentIndex];
            agent.SetDestination(target.position);
            entity.LookAt(target);


            if (isPathCalculated && agent.remainingDistance < 0.1f)
            {
                currentIndex++;
                isPathCalculated = false;
            }

            if (agent.pathPending)
            {
                isPathCalculated = true;
            }

            return Node.Status.Success;
        }

        public void Reset() => currentIndex = 0;
    }
}