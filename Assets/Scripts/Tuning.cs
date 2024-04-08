using System;
using UnityEngine;

[Serializable]
public abstract class Tuning<T>
{
    [SerializeField] private PriceUpgradePair<T>[] priceObjectPairs;
    private int selected;

    public PriceUpgradePair<T>[] PriceObjectPairs => priceObjectPairs;
    public int Selected
    {
        get => selected;
        set => selected = value;
    }
}

[Serializable]
public class PriceUpgradePair<T>
{
    [SerializeField] private int price;
    [SerializeField] private T upgrade;
    [SerializeField] private Sprite thumbnail;
    
    public int Price => price;

    public T Upgrade => upgrade;
    
    public Sprite Thumbnail => thumbnail;
}

[Serializable]
public class TuningGameObject : Tuning<GameObject>
{
    public GameObject SelectedGameObject => PriceObjectPairs[Selected].Upgrade;
}

[Serializable]
public class SpoilerTuning : TuningGameObject { }

[Serializable]
public class WheelTuning : TuningGameObject { }
    
[Serializable]
public class ColorTuning : Tuning<Material>
{
    [SerializeField] private GameObject carBody;

    public GameObject CarBody => carBody;
}