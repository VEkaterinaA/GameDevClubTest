using Assets.Common.Scipts.CommonForHeroAndMutantsScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Common.Scipts.Hero
{
    [Serializable]
    public class HeroCharacteristics : CommonCharacteristics
    {
        public Transform HeroTransform;
        public TransformInfo transformInfo;
        public HeroCharacteristics(Transform heroTransform)
        {
            HeroTransform = heroTransform;
        }
        public TransformInfo ConvertTransformToTransformInfo()
        {
            return new TransformInfo(HeroTransform);
        }
        public void ConvertTransformInfoToTransform(TransformInfo transformInfo)
        {
            HeroTransform.position = transformInfo.pos;
            HeroTransform.rotation = transformInfo.rot;
            HeroTransform.localScale = transformInfo.scale;
        }
    }
    [Serializable]
    public class TransformInfo
    {
        public Vector3 pos;
        public Quaternion rot;
        public Vector3 scale;
        public TransformInfo(Transform transform)
        {
            pos = transform.position;
            rot = transform.rotation;
            scale = transform.localScale;
        }
    }
}
