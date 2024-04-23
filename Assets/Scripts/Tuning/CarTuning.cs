using Drive;
using Photon.Pun;
using UnityEngine;

namespace Tuning
{
    public class CarTuning : MonoBehaviour
    {
        [SerializeField] private CarTuningData data;
        private CarController _carController;
        public CarTuningData Data => data;

        private void Awake()
        {
            _carController = GetComponent<CarController>();
            data.WheelTuning.wheelMeshes = _carController.WheelMeshes;
            
            var view = GetComponent<PhotonView>();
            if (view.IsMine || view.Owner == null) return;
            var property = view.Owner.CustomProperties["Tuning"]; 
            data.ApplyTuning(JsonUtility.FromJson<CarTuningData>(property.ToString()));
        }
    }
}