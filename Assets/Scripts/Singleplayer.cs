using UnityEngine;

public class Singleplayer : MonoBehaviour
{
    [SerializeField] private Game game;
   
    private GameObject car;

    public void Setup(Spawner spawner)
    {
        spawner.Spawn();
        game.SetupGame();
        game.StartGame();
        spawner.StartCar();
    }
}
