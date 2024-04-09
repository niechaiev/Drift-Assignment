using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tuning
{
    [Serializable]
    public abstract class Tuning<T>
    {
        [SerializeField] private PriceUpgradePair<T>[] priceObjectPairs;
        [SerializeField][HideInInspector] private int selected;

        public PriceUpgradePair<T>[] PriceObjectPairs => priceObjectPairs;
        public int Selected
        {
            get => selected;
            set => selected = value;
        }
    
        public Tuning(Tuning<T> tuning)
        {
            var pairs = new List<PriceUpgradePair<T>>();
            for (var index = 0; index < tuning.priceObjectPairs.Length; index++)
            {
                var pair = tuning.priceObjectPairs[index];
                pairs.Add(new PriceUpgradePair<T>(pair));
            }

            priceObjectPairs = pairs.ToArray();
        
            selected = tuning.Selected;
        }

        public abstract void ApplyUpgrade(int upgradeIndex);
    }
}