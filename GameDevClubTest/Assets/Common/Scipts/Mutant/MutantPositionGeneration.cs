using UnityEngine;
using UnityEngine.AI;

namespace Assets.Common.Scipts.Mutant
{
    public class MutantPositionGeneration
    {
        public Vector2 startPoint;
        public float radius;
        public Vector2 GetRandomStartPointMutantPositionGeneration()
        {
            return RandomMutantPositionGeneration(startPoint, radius);
        }
        public Vector2 GetRandomMutantPositionGeneration(Vector2 StartPoint, float Radius)
        {
            return RandomMutantPositionGeneration(StartPoint, Radius);
        }

        private Vector2 RandomMutantPositionGeneration(Vector2 StartPoint, float Radius)
        {
            Vector2 FinalPoint = Vector2.zero;
            if (Radius != 0)
            {
                while (FinalPoint == Vector2.zero)
                {
                    Vector2 vector = Random.insideUnitCircle * Radius + StartPoint;
                    NavMeshHit navMeshHit;
                    if (NavMesh.SamplePosition(vector, out navMeshHit, Radius, 1))
                    {
                        FinalPoint = navMeshHit.position;
                    }
                }
            }
            return FinalPoint;
        }
    }
}
