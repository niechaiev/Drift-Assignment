﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tuning
{
    [Serializable]
    public class CarTuningData
    {
        [SerializeField] private int carId;
        [SerializeField] private SpoilerTuning spoilerTuning;
        [SerializeField] private WheelTuning wheelTuning;
        [SerializeField] private ColorTuning colorTuning;
        [SerializeField] private ImageTuning imageTuning;

        public int CarId => carId;
        public SpoilerTuning SpoilerTuning => spoilerTuning;
        public WheelTuning WheelTuning => wheelTuning;
        public ColorTuning ColorTuning => colorTuning;
        public ImageTuning ImageTuning => imageTuning;

        public CarTuningData(CarTuningData carTuningData)
        {
            carId = carTuningData.carId;
            spoilerTuning = new SpoilerTuning(carTuningData.spoilerTuning);
            wheelTuning = new WheelTuning(carTuningData.wheelTuning);
            colorTuning = new ColorTuning(carTuningData.colorTuning);
            imageTuning = new ImageTuning(carTuningData.ImageTuning);
        }

        public Tuning<T> GetTuningOfSameType<T>(Tuning<T> type)
        {
            return type switch
            {
                global::Tuning.SpoilerTuning => spoilerTuning as Tuning<T>,
                global::Tuning.WheelTuning => wheelTuning as Tuning<T>,
                global::Tuning.ColorTuning => colorTuning as Tuning<T>,
                global::Tuning.ImageTuning => imageTuning as Tuning<T>,
                _ => null
            };
        }

        public void SetSelectedAndBuy<T>(Tuning<T> type,int index)
        {
            var tuning = GetTuningOfSameType(type);
            var price = tuning.PriceObjectPairs[index].Price;
            Player.Instance.Cash -= price;
            GAManager.OnMoneySpent(false, price, "tuning", type.GetType() + " " + index);
            tuning.Selected = index;
            tuning.PriceObjectPairs[index].Price = 0;
        }
    
        public void ApplyTuning(CarTuningData savedCarTuningData = null)
        {
            savedCarTuningData ??= Player.Instance.CarTunings[carId];
            spoilerTuning.ApplyUpgrade(savedCarTuningData.SpoilerTuning.Selected);
            wheelTuning.ApplyUpgrade(savedCarTuningData.WheelTuning.Selected);
            colorTuning.ApplyUpgrade(savedCarTuningData.ColorTuning.Selected);
            
            var selectedImage = savedCarTuningData.ImageTuning.Selected;
            imageTuning.ApplyUpgrade(selectedImage, savedCarTuningData.imageTuning.PriceObjectPairs[selectedImage]);
        }
    }
    
    [Serializable]
    public class CarTuningDataList
    {
        public List<CarTuningData> CarTuningDatas;
        
        public CarTuningDataList()
        {
            CarTuningDatas = new List<CarTuningData>();
        }

        public CarTuningDataList(List<CarTuningData> carTuningDatas)
        {
            CarTuningDatas = carTuningDatas;
        }
    }
}