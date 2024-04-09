using System;
using UnityEngine;

namespace Tuning
{
    [Serializable]
    public class PriceUpgradePair<T>
    {
        [SerializeField] private int price;
        [SerializeField] private T upgrade;
        [SerializeField] private Sprite thumbnail;
    
        public int Price
        {
            get => price;
            set => price = value;
        }

        public T Upgrade => upgrade;
    
        public Sprite Thumbnail => thumbnail;
    
        public PriceUpgradePair(PriceUpgradePair<T> pair)
        {
            price = pair.price;
            upgrade = pair.upgrade;
            thumbnail = pair.thumbnail;
        }
    }
}