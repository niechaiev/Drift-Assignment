using System;
using UnityEngine;

namespace Tuning
{
    [Serializable]
    public class SpoilerTuning : Tuning<GameObject>
    {
        public override void ApplyUpgrade(int upgradeIndex)
        {
            foreach (var priceObject in PriceObjectPairs)
            {
                if (priceObject.Upgrade != null)
                    priceObject.Upgrade.gameObject.SetActive(false);
            }

            if (PriceObjectPairs.Length > upgradeIndex && PriceObjectPairs[upgradeIndex].Upgrade != null)
                PriceObjectPairs[upgradeIndex].Upgrade.SetActive(true);
            Selected = upgradeIndex;
        }

        public SpoilerTuning(Tuning<GameObject> tuning) : base(tuning)
        {
        }
    }
}