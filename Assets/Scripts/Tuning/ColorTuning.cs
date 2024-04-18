using System;
using UnityEngine;

namespace Tuning
{
    [Serializable]
    public class ColorTuning : Tuning<Material>
    {
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private int materialIndex;
    
        public override void ApplyUpgrade(int upgradeIndex)
        {
            if (meshRenderer is null) return;
            var materials = meshRenderer.materials;

            materials[materialIndex] = PriceObjectPairs[upgradeIndex].Upgrade;
            
            meshRenderer.materials = materials;
            Selected = upgradeIndex;
        }

        public ColorTuning(Tuning<Material> tuning) : base(tuning)
        {
        }
    }
}