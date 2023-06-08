using Assets.Common.Scipts.Mutant.HelperClasses;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Common.Scipts.Mutant.MutantModes
{
    public class Patrol
    {
        private Vector2 walkPoint;
        bool walkPointSet;
        public IEnumerator CoroutinePatroling(MutantControl mutantTurn, NavMeshAgent navMeshAgent, Vector2 PointPositiontOfRandomPointSearchArea,
                                              float timeBetweenPatrols, float walkPointRange, MutantGenerationService _mutantPositionGeneration)
        {
            if (navMeshAgent == null)
            {
                yield return null;
            }
            else
            {
                if (!walkPointSet)
                {
                    SearchWalkPoint(PointPositiontOfRandomPointSearchArea, walkPointRange, _mutantPositionGeneration);
                }
                if (walkPointSet)
                {
                    navMeshAgent.SetDestination(walkPoint);
                    mutantTurn.RotationMutantRelativeToMovement(navMeshAgent.velocity);

                }
                Vector2 distanceToWalkPoint = PointPositiontOfRandomPointSearchArea - walkPoint;

                if (distanceToWalkPoint.magnitude < 1f)
                {
                    yield return new WaitForSeconds(timeBetweenPatrols);
                    walkPointSet = false;
                }
            }
        }
        private void SearchWalkPoint(Vector2 position, float walkPointRange, MutantGenerationService _mutantPositionGeneration)
        {
            walkPoint = _mutantPositionGeneration.GetRandomMutantPositionGeneration(position, walkPointRange);

            walkPointSet = true;
        }

    }
}
