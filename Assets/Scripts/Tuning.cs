using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Tuning<T>
{
    [SerializeField] private PriceUpgradePair<T>[] priceObjectPairs;
    [SerializeField][HideInInspector] private int selected;

    public PriceUpgradePair<T>[] PriceObjectPairs => priceObjectPairs;
    public int Selected
    {
        get => selected;
        set => selected = value;
    }
    
    public Tuning(Tuning<T> tuning)
    {
        var pairs = new List<PriceUpgradePair<T>>();
        for (var index = 0; index < tuning.priceObjectPairs.Length; index++)
        {
            var pair = tuning.priceObjectPairs[index];
            pairs.Add(new PriceUpgradePair<T>(pair));
        }

        priceObjectPairs = pairs.ToArray();
        
        selected = tuning.Selected;
    }

    public abstract void ApplyUpgrade(int upgradeIndex);
}

[Serializable]
public class PriceUpgradePair<T>
{
    [SerializeField] private int price;
    [SerializeField] private T upgrade;
    [SerializeField] private Sprite thumbnail;
    
    public int Price
    {
        get => price;
        set => price = value;
    }

    public T Upgrade => upgrade;
    
    public Sprite Thumbnail => thumbnail;
    
    public PriceUpgradePair(PriceUpgradePair<T> pair)
    {
        price = pair.price;
        upgrade = pair.upgrade;
        thumbnail = pair.thumbnail;
    }
}

[Serializable]
public abstract class TuningGameObject : Tuning<GameObject>
{
    public GameObject SelectedGameObject => PriceObjectPairs[Selected].Upgrade;

    protected TuningGameObject(Tuning<GameObject> tuning) : base(tuning)
    {
    }
}

[Serializable]
public class SpoilerTuning : TuningGameObject
{
    public override void ApplyUpgrade(int upgradeIndex)
    {
        foreach (var priceObject in PriceObjectPairs)
        {
            if (priceObject.Upgrade != null)
                priceObject.Upgrade.gameObject.SetActive(false);
        }

        if (PriceObjectPairs.Length > upgradeIndex && PriceObjectPairs[upgradeIndex].Upgrade != null)
            PriceObjectPairs[upgradeIndex].Upgrade.SetActive(true);
        Selected = upgradeIndex;
    }

    public SpoilerTuning(Tuning<GameObject> tuning) : base(tuning)
    {
    }
}

[Serializable]
public class WheelTuning : TuningGameObject
{
    public override void ApplyUpgrade(int upgradeIndex)
    {
        throw new NotImplementedException();
    }

    public WheelTuning(Tuning<GameObject> tuning) : base(tuning)
    {
    }
}
    
[Serializable]
public class ColorTuning : Tuning<Material>
{
    [SerializeField] private GameObject carBody;

    public GameObject CarBody => carBody;
    
    
    public override void ApplyUpgrade(int upgradeIndex)
    {
        if(carBody == null) return;
        var carBodyRenderer = carBody.GetComponent<Renderer>();
        var materials = carBodyRenderer.materials;
        if (materials.Length > 1) //TODO::Rearrange materials
            materials[1] = PriceObjectPairs[upgradeIndex].Upgrade;
        else
            materials[0] = PriceObjectPairs[upgradeIndex].Upgrade;

        carBodyRenderer.materials = materials;
        Selected = upgradeIndex;
    }

    public ColorTuning(Tuning<Material> tuning) : base(tuning)
    {
    }
}