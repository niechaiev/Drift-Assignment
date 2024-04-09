using System;
using Tuning;
using UnityEngine;

[Serializable]
public class CarInfo
{
    [SerializeField] private string carName;
    [SerializeField] private bool isCurrencyGold;
    [SerializeField] private int price;
    [SerializeField] private CarTuning carTuning;

    public int CarId => carTuning.Data.CarId;

    public string CarName => carName;

    public bool IsCurrencyGold => isCurrencyGold;

    public int Price => price;

    public CarTuning CarTuning => carTuning;
}