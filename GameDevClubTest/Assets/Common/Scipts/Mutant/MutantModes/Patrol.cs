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

namespace Assets.Common.Scipts.Mutant.MutantModes
{
    public class Patrol
    {
        private Vector2 walkPoint;
        bool walkPointSet;
        public IEnumerator CoroutinePatroling(NavMeshAgent navMeshAgent,Vector2 position,float timeBetweenPatrols, float walkPointRange, MutantPositionGeneration _mutantPositionGeneration)
        {
            if (!walkPointSet)
            {
                SearchWalkPoint(position, walkPointRange, _mutantPositionGeneration);
            }
            if (walkPointSet)
            {
                navMeshAgent.SetDestination(walkPoint);
            }
            Vector2 distanceToWalkPoint = position - walkPoint;

            if (distanceToWalkPoint.magnitude < 1f)
            {
                yield return new WaitForSeconds(timeBetweenPatrols);
                walkPointSet = false;
            }
        }
        private void SearchWalkPoint(Vector2 position, float walkPointRange, MutantPositionGeneration _mutantPositionGeneration)
        {
            walkPoint = _mutantPositionGeneration.GetRandomMutantPositionGeneration(position, walkPointRange);

            walkPointSet = true;
        }

    }
}
