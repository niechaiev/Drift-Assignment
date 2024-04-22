using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tuning
{
    [Serializable]
    public class WheelTuning : Tuning<GameObject>
    {
        [HideInInspector] public GameObject[] wheelMeshes;
        public override void ApplyUpgrade(int upgradeIndex)
        {
            if (PriceObjectPairs.Length == 0 || !PriceObjectPairs[upgradeIndex].Upgrade) return;
            for (var i = 0; i < wheelMeshes.Length; i++)
            {
                var wheelMesh = wheelMeshes[i];
                
                var newTransform = wheelMesh.transform;
                newTransform.localScale = wheelMeshes[i].transform.localScale;
                var newWheelMeshParent = new GameObject
                {
                    transform =
                    {
                        position = wheelMesh.transform.position,
                        rotation = wheelMesh.transform.rotation
                    }
                };
                newWheelMeshParent.transform.SetParent(wheelMesh.transform.parent);
                
                var newWheelMesh = Object.Instantiate(PriceObjectPairs[upgradeIndex].Upgrade,
                    newWheelMeshParent.transform, false);
                
                if (i is 0 or 2)
                {
                    newWheelMesh.transform.rotation = Quaternion.Euler(0, 0, 180);
                }
                
                Object.Destroy(wheelMesh);
                wheelMeshes[i] = newWheelMeshParent;
            }

            Selected = upgradeIndex;
        }

        public WheelTuning(Tuning<GameObject> tuning) : base(tuning)
        {
        }
    }
}