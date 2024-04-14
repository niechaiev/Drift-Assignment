using System.Collections;
using TMPro;
using UnityEngine;

public class Speedometer : MonoBehaviour
{
    public const float MsToKphRatio = 3.6f;
    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text torqueText;
    private CarController _carController;
    private Rigidbody _carRigidbody;
    
    public void Setup(CarController carController)
    {
        _carController = carController;
        _carRigidbody = carController.GetComponent<Rigidbody>();
        StartCoroutine(UpdateSpeed());
    }

    private IEnumerator UpdateSpeed()
    {
        while (true)
        {
            text.SetText($"{Mathf.RoundToInt(_carRigidbody.velocity.magnitude * MsToKphRatio)} KPH");
            torqueText.SetText($"{Mathf.RoundToInt(_carController.Wheels[2].motorTorque)} Nm");
            yield return new WaitForFixedUpdate();
        }
    }
}
