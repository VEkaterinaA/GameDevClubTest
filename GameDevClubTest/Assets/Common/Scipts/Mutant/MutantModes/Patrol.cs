using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.AI;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.UIElements;
using Assets.Common.Scipts.Mutant.HelperClasses;

namespace Assets.Common.Scipts.Mutant.MutantModes
{
    public class Patrol
    {
        private Vector2 walkPoint;
        bool walkPointSet;
        public IEnumerator CoroutinePatroling(MutantTurn mutantTurn,NavMeshAgent navMeshAgent,Vector2 PointPositiontOfRandomPointSearchArea,float timeBetweenPatrols, float walkPointRange, MutantPositionGeneration _mutantPositionGeneration)
        {
            var MutantTransformScale = navMeshAgent.transform.localScale;
            var MutantPositionX = navMeshAgent.transform.position.x;
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
                    mutantTurn.RotationMutantRelativeToMovement(navMeshAgent.velocity, navMeshAgent.transform, walkPoint);

                }
                Vector2 distanceToWalkPoint = PointPositiontOfRandomPointSearchArea - walkPoint;

                if (distanceToWalkPoint.magnitude < 1f)
                {
                    yield return new WaitForSeconds(timeBetweenPatrols);
                    walkPointSet = false;
                }
            }
        }
        private void SearchWalkPoint(Vector2 position, float walkPointRange, MutantPositionGeneration _mutantPositionGeneration)
        {
            walkPoint = _mutantPositionGeneration.GetRandomMutantPositionGeneration(position, walkPointRange);

            walkPointSet = true;
        }

    }
}
