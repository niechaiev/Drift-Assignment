using System.Collections;
using Drive;
using TMPro;
using UnityEngine;

public class Speedometer : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text torqueText;
    private CarController _carController;
    
    public void Setup(CarController carController)
    {
        _carController = carController;
        StartCoroutine(UpdateSpeed());
    }

    private IEnumerator UpdateSpeed()
    {
        while (true)
        {
            text.SetText($"{Mathf.RoundToInt(_carController.Kph)} KPH");
            torqueText.SetText($"{Mathf.RoundToInt(_carController.Wheels[2].motorTorque)} Nm");
            yield return new WaitForFixedUpdate();
        }
    }
}
