using Assets.Common.Scipts.Mutant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Common.Infrastructure
{
    public interface IMutantFactory
    {
        public void Load();
        public void Create(MutantType mutantType);
    }
}
