using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Common.Scipts.Mutant
{
    public class MutantMarker : MonoBehaviour
    {
        public MutantType enemyType;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position,0.7f);
        }
    }
}
