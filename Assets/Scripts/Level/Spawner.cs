using Drive;
using Photon.Pun;
using Tuning;
using UnityEngine;

namespace Level
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private CarList carList;
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private GameObject canvases;
        [SerializeField] private CameraController cameraController;
        private GameObject _car;
    
        private void Awake()
        {
            var newCanvases = Instantiate(canvases);
            if (Player.Instance.IsOnline)
            {
                PhotonNetwork.OfflineMode = false;
                newCanvases.GetComponent<Multiplayer>().Setup(this);
            }
            else
            {
                PhotonNetwork.OfflineMode = true;
                newCanvases.GetComponent<Singleplayer>().Setup(this);
            }
        }
    

        public void Spawn()
        {
            if (Player.Instance.IsOnline)
            {
                var spawnIndex = (PhotonNetwork.CurrentRoom.PlayerCount - 1) % spawnPoints.Length;

                _car = PhotonNetwork.Instantiate(carList[Player.Instance.SelectedCar].carPrefab.name,
                    spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
            }
            else
            {
                _car = Instantiate(carList[Player.Instance.SelectedCar].carPrefab,
                    spawnPoints[0].position, spawnPoints[0].rotation);
            }

            _car.GetComponent<CarTuning>().Data.ApplyTuning();
            cameraController.enabled = true;
        }

        public void StartCar()
        {
            _car.GetComponent<CarSetup>().SetupLocalCar();
        }
    }
}
