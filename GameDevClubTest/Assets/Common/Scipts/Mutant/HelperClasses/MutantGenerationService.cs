using UnityEngine;
using UnityEngine.AI;

namespace Assets.Common.Scipts.Mutant.HelperClasses
{
    public class MutantGenerationService
    {
        private Vector2 _startPoint;
        private float _radius;

        public void SetBaseParameters(Vector2 startPoint, float radius)
        {
            _startPoint = startPoint;
            _radius = radius;
        }

        public Vector2 GetRandomStartPosition()
        {
            return GenerateRandomPosition(_startPoint, _radius);
        }
        public Vector2 GetRandomMutantPositionGeneration(Vector2 StartPoint, float Radius)
        {
            return GenerateRandomPosition(StartPoint, Radius);
        }

        private Vector2 GenerateRandomPosition(Vector2 StartPoint, float Radius)
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
