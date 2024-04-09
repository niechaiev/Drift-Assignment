using System;
using UnityEngine;

namespace Tuning
{
    [Serializable]
    public class WheelTuning : TuningGameObject
    {
        public override void ApplyUpgrade(int upgradeIndex)
        {
            throw new NotImplementedException();
        }

        public WheelTuning(Tuning<GameObject> tuning) : base(tuning)
        {
        }
    }
}