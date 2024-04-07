using System;
using UnityEngine;

[Serializable]
public abstract class Tuning<T>
{
    [SerializeField] private PriceAddonPair<T>[] priceObjectPairs;
    private int selected;

    public PriceAddonPair<T>[] PriceObjectPairs => priceObjectPairs;
    public int Selected
    {
        get => selected;
        set => selected = value;
    }
}

[Serializable]
public class PriceAddonPair<T>
{
    [SerializeField] private int price;
    [SerializeField] private T addon;
    public int Price => price;

    public T Addon => addon;
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