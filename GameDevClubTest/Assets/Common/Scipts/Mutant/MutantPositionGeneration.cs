using UnityEngine;
using UnityEngine.AI;

namespace Assets.Common.Scipts.Mutant
{
    public class MutantPositionGeneration
    {
        public Vector2 startPoint;
        public float radius;
        public Vector2 RandomMutantPositionGeneration()
        {
            Vector2 FinalPoint = Vector2.zero;
            if (radius != 0)
            {
                while (FinalPoint == Vector2.zero)
                {
                    Vector2 vector = Random.insideUnitCircle * radius + startPoint;
                    NavMeshHit navMeshHit;
                    if (NavMesh.SamplePosition(vector, out navMeshHit, radius, 1))
                    {
                        FinalPoint = navMeshHit.position;
                    }
                }
            }
            return FinalPoint;
        }
    }
}
