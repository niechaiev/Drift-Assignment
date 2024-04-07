using System;
using UnityEngine;

[Serializable]
public abstract class Tuning<T>
{
    [SerializeField] private int[] prices;
    [SerializeField] private T[] tuningObjects;
    private int selected;
        
    public int Selected
    {
        get => selected;
        set => selected = value;
    }
        
    public int[] Prices
    {
        get => prices;
        set => prices = value;
    }
        
    public T[] TuningObjects
    {
        get => tuningObjects;
        set => tuningObjects = value;
    }
}

[Serializable]
public class SpoilerTuning : Tuning<GameObject> { }
    
[Serializable]
public class WheelTuning : Tuning<GameObject> { }
    
[Serializable]
public class ColorTuning : Tuning<Material>
{
    [SerializeField] private GameObject carBody;

    public GameObject CarBody => carBody;
}