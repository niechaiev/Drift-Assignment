using UnityEngine;

[CreateAssetMenu(fileName = "Cars", menuName = "CarsScriptableObject", order = 1)]
public class Cars : ScriptableObject
{
    public GameObject[] carPrefabs;
}