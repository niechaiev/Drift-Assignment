using UnityEngine;

namespace Level
{
    public class Singleplayer : MonoBehaviour
    {
        [SerializeField] private Game game;
   
        private GameObject _car;

        public void Setup(Spawner spawner)
        {
            spawner.Spawn();
            game.SetupGame();
            game.StartGame();
            spawner.StartCar();
        }
    }
}
