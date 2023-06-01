using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Common.Scipts.Mutant.MutantModes
{
    public class Attack
    {
        private bool alreadyAttacked;

        public IEnumerator CoroutineAttackHero(NavMeshAgent navMeshAgent, Transform transform, float timeBetweenAttacks)
        {
            navMeshAgent.SetDestination(transform.position);

            if (!alreadyAttacked)
            {
                //Attack

                //
                yield return new WaitForSeconds(timeBetweenAttacks);
                alreadyAttacked = false;
            }
        }
    }
}
