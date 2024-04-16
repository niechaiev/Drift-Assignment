using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Tuning
{
    [Serializable]
    public class ImageTuning : Tuning<string>
    {
        [SerializeField] private DecalProjector[] decalProjectors;
        public override void ApplyUpgrade(int upgradeIndex)
        {
            Selected = upgradeIndex;
            foreach (var decalProjector in decalProjectors)
            {
                decalProjector.gameObject.SetActive(false);
            }
        }

        public void ApplyUpgrade(int upgradeIndex, PriceUpgradePair<string> pair)
        {
            ApplyUpgrade(upgradeIndex);
            foreach (var decalProjector in decalProjectors)
            {
                if (decalProjector is null) continue;
                if (pair.Upgrade == string.Empty)
                {
                    decalProjector.gameObject.SetActive(false);
                    return;
                }
            
                Player.Instance.StartCoroutine(Utils.GetTexture(pair.Upgrade, texture =>
                {
                    var material = new Material(Shader.Find("Shader Graphs/Decal"));
                    material.SetTexture("Base_Map", texture);
                    decalProjector.material = material;
                    decalProjector.gameObject.SetActive(true);
                }));
            }
        }
        
        public ImageTuning(Tuning<string> tuning) : base(tuning)
        {
        }
    }
}