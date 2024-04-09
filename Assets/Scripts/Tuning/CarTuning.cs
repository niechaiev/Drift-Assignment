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
    }
}