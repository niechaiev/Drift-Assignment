using System;
using UnityEngine;

namespace Tuning
{
    [Serializable]
    public class ColorTuning : Tuning<Material>
    {
        [SerializeField] private GameObject carBody;

        public GameObject CarBody => carBody;
    
    
        public override void ApplyUpgrade(int upgradeIndex)
        {
            if (carBody is null) return;
            var carBodyRenderer = carBody.GetComponent<Renderer>();
            var materials = carBodyRenderer.materials;
            if (materials.Length > 1) //TODO::Rearrange materials
                materials[1] = PriceObjectPairs[upgradeIndex].Upgrade;
            else
                materials[0] = PriceObjectPairs[upgradeIndex].Upgrade;

            carBodyRenderer.materials = materials;
            Selected = upgradeIndex;
        }

        public ColorTuning(Tuning<Material> tuning) : base(tuning)
        {
        }
    }
}