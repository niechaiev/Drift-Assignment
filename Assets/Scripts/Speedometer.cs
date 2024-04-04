using TMPro;
using UnityEngine;

public class Speedometer : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    private GameObject player;
    private Rigidbody playerRb;
    
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerRb = player.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        text.SetText($"{Mathf.RoundToInt(playerRb.velocity.magnitude * 3.6f)} KPH");
    }
}
