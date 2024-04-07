using UnityEngine;

public class CarTuning : MonoBehaviour
{
    [SerializeField] private SpoilerTuning spoilerTuning;
    [SerializeField] private WheelTuning wheelTuning;
    [SerializeField] private ColorTuning colorTuning;

    public SpoilerTuning SpoilerTuning => spoilerTuning;
    public WheelTuning WheelTuning => wheelTuning;
    public ColorTuning ColorTuning => colorTuning;

    public bool HasTuning<T>(Tuning<T> tuning)
    {
        return tuning.PriceObjectPairs.Length > 1;
    }
}

