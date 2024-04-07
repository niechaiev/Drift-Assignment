using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CarList", menuName = "CarListScriptableObject", order = 1)]
public class CarList : ScriptableObject
{
    [SerializeField] private Car[] cars;
    public Car this[int index] => cars[index];
    public int Count => cars.Length;
}
[Serializable]
public class Car
{
    public GameObject carPrefab;
    public CarInfo carInfo;
}