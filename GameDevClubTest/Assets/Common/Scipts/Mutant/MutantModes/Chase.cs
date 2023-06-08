using Assets.Common.Scipts.Mutant.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Common.Scipts.Mutant.MutantModes
{
    public class Chase
    {
        public void ChaseHero(MutantTurn mutantTurn,NavMeshAgent navMeshAgent, Vector3 HeroPosition)
        {
            navMeshAgent.SetDestination(HeroPosition);
            mutantTurn.RotationMutantRelativeToMovement(navMeshAgent.velocity);

        }
    }
}
