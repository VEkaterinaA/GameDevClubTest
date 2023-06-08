using Assets.Common.Scipts.Mutant.HelperClasses;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Common.Scipts.Mutant.MutantModes
{
    public class Chase
    {
        public void ChaseHero(MutantControl mutantTurn, NavMeshAgent navMeshAgent, Vector3 HeroPosition)
        {
            navMeshAgent.SetDestination(HeroPosition);
            mutantTurn.RotationMutantRelativeToMovement(navMeshAgent.velocity);

        }
    }
}
