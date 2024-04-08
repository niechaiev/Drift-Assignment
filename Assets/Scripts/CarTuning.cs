using System;
using System.Collections.Generic;
using UnityEngine;

public class CarTuning : MonoBehaviour
{
    [SerializeField] private CarTuningData data;

    public CarTuningData Data => data;
    
    public bool HasTuning<T>(Tuning<T> tuning)
    {
        return tuning.PriceObjectPairs.Length > 1;
    }
}

[Serializable]
public class CarTuningData
{
    [SerializeField] private int carId;
    [SerializeField] private SpoilerTuning spoilerTuning;
    [SerializeField] private WheelTuning wheelTuning;
    [SerializeField] private ColorTuning colorTuning;
    
    public int CarId => carId;
    public SpoilerTuning SpoilerTuning => spoilerTuning;
    public WheelTuning WheelTuning => wheelTuning;
    public ColorTuning ColorTuning => colorTuning;

    public CarTuningData(CarTuningData carTuningData)
    {
        carId = carTuningData.carId;
        spoilerTuning = new SpoilerTuning(carTuningData.spoilerTuning);
        wheelTuning = new WheelTuning(carTuningData.wheelTuning);
        colorTuning = new ColorTuning(carTuningData.colorTuning);
    }

    public Tuning<T> GetTuningOfSameType<T>(Tuning<T> type)
    {
        switch (type)
        {
            case global::SpoilerTuning:
                return spoilerTuning as Tuning<T>;
            case global::WheelTuning:
                return wheelTuning as Tuning<T>;
            case global::ColorTuning:
                return colorTuning as Tuning<T>;
        }
        return null;
    }

    public void SetSelectedAndBuy<T>(Tuning<T> type,int index)
    {
        var tuning = GetTuningOfSameType(type);
        tuning.Selected = index;
        tuning.PriceObjectPairs[index].Price = 0;
    }
    
    public void ApplyTuning()
    {
        var savedCarTuningData = Player.Instance.CarTunings[carId];
        spoilerTuning.ApplyUpgrade(savedCarTuningData.SpoilerTuning.Selected);
        //selectedCarTuning.Data.WheelTuning.ApplyUpgrade(savedCarTuningData.WheelTuning.Selected);
        colorTuning.ApplyUpgrade(savedCarTuningData.ColorTuning.Selected);
    }
}

[Serializable]
public class CarTuningDataList
{
    public List<CarTuningData> CarTuningDatas;

    public CarTuningDataList(List<CarTuningData> carTuningDatas)
    {
        CarTuningDatas = carTuningDatas;
    }
}

