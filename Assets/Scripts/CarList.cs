using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "CarList", menuName = "CarListScriptableObject", order = 1)]
public class CarList : ScriptableObject, IEnumerable
{
    [SerializeField] private Car[] cars;
    public Car this[int index] => cars[index];
    public int Count => cars.Length;
    public IEnumerator GetEnumerator()
    {
        return cars.GetEnumerator();
    }
}
[Serializable]
public class Car
{
    public GameObject carPrefab;
    public CarInfo carInfo;
}