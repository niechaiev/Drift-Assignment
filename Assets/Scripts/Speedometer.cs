using System.Collections;
using TMPro;
using UnityEngine;

public class Speedometer : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    private Rigidbody _carRigidBody;
    
    public void Setup(Rigidbody carRigidBody)
    {
        this._carRigidBody = carRigidBody;
        StartCoroutine(UpdateSpeed());
    }

    private IEnumerator UpdateSpeed()
    {
        while (true)
        {
            text.SetText($"{Mathf.RoundToInt(_carRigidBody.velocity.magnitude * 3.6f)} KPH");
            yield return new WaitForFixedUpdate();
        }
    }
}
