using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Common.Scipts.Mutant.MutantModes
{
    public class Attack
    {
        private bool alreadyAttacked = true;

        public IEnumerator CoroutineAttackHero(HeroController heroController, NavMeshAgent navMeshAgent, Transform MutantTransform, float timeBetweenAttacks, int damage)
        {
            navMeshAgent.SetDestination(MutantTransform.position);

            if (alreadyAttacked)
            {
                alreadyAttacked = false;
                heroController.SetDamage(damage);
                yield return new WaitForSeconds(timeBetweenAttacks);
                alreadyAttacked = true;
            }
        }
    }
}
