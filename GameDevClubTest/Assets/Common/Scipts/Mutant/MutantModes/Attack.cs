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
        private bool alreadyAttacked=true;

        public IEnumerator CoroutineAttackHero(HeroController heroController,NavMeshAgent navMeshAgent, Transform MutantTransform, float timeBetweenAttacks,int damage)
        {
            navMeshAgent.SetDestination(MutantTransform.position);

            if (alreadyAttacked)
            {
                alreadyAttacked = false;
                heroController.TakingHeroDamage(damage);
                yield return new WaitForSeconds(timeBetweenAttacks);
                alreadyAttacked = true;
            }
        }
    }
}
