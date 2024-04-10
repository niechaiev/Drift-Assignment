using Photon.Pun;
using UnityEngine;

namespace Tuning
{
    public class CarTuning : MonoBehaviour
    {
        [SerializeField] private CarTuningData data;

        public CarTuningData Data => data;
    
        public bool HasTuning<T>(Tuning<T> tuning)
        {
            return tuning.PriceObjectPairs.Length > 1;
        }

        private void Awake()
        {
            var view = GetComponent<PhotonView>();
            if (view.IsMine || view.Owner == null) return;
            var property = view.Owner.CustomProperties["Tuning"];
            view.GetComponent<CarTuning>().Data.ApplyTuning(JsonUtility.FromJson<CarTuningData>(property.ToString()));
        }
    }
}