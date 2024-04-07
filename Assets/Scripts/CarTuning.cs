using System;
using UnityEngine;

[Serializable]
public class CarTuning
{
    [SerializeField] private SpoilerTuning spoilerTuning;
    [SerializeField] private WheelTuning wheelTuning;
    [SerializeField] private ColorTuning colorTuning;

    public SpoilerTuning SpoilerTuning => spoilerTuning;
    public WheelTuning WheelTuning => wheelTuning;
    public ColorTuning ColorTuning => colorTuning;

    public bool HasTuning<T>(Tuning<T> tuning)
    {
        return tuning.Prices.Length > 1;
    }
}

