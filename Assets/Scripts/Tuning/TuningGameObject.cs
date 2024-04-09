using System;
using UnityEngine;

namespace Tuning
{
    [Serializable]
    public abstract class TuningGameObject : Tuning<GameObject>
    {
        public GameObject SelectedGameObject => PriceObjectPairs[Selected].Upgrade;

        protected TuningGameObject(Tuning<GameObject> tuning) : base(tuning)
        {
        }
    }
}