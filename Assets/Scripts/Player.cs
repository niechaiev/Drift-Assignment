using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private CarList carList;
    private static Player instance;
    private string username;
    private int gold;
    private int cash;
    private List<int> ownedCars;
    private int selectedCar;
    public Action<int> OnGoldChange;
    public Action<int> OnCashChange;
    public List<CarTuningData> CarTunings = new();
    
    public static Player Instance => instance;
    public string Name
    {
        get => username;
        set
        {
            username = value;
            PlayerPrefs.SetString("username", value);
        }
    }

    public int Gold => gold;

    public int Cash
    {
        get => cash;
        set
        {
            cash = value;
            PlayerPrefs.SetInt("cash", value);
            OnCashChange?.Invoke(cash);
        }
    }

    public IReadOnlyList<int> OwnedCars => ownedCars;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Load();
        }
    }

    private void Load()
    {
        username = PlayerPrefs.GetString("username", string.Empty);
        gold = PlayerPrefs.GetInt("gold", 0);
        cash = PlayerPrefs.GetInt("cash", 0);
        var ownedCarsString = PlayerPrefs.GetString("ownedCars").Split(new []{"#"}, StringSplitOptions.None);
        if (ownedCarsString.Length == 1) ownedCarsString[0] = "0";
        ownedCars = Array.ConvertAll(ownedCarsString, int.Parse).ToList();
        selectedCar = PlayerPrefs.GetInt("selectedCar", 0);


        if (LoadTuning()) return;
        foreach (Car car in carList)
        {
            CarTunings.Add(new CarTuningData(car.carInfo.CarTuning.Data));
        }
    }

    public bool LoadTuning()
    {
        var fullPath = Path.Combine(Application.persistentDataPath, nameof(CarTuningDataList));
        CarTuningDataList carTuningDataList = null;
        if (File.Exists(fullPath))
        {
            try
            {
                var dataToLoad = string.Empty;
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                CarTunings = JsonUtility.FromJson<CarTuningDataList>(dataToLoad).CarTuningDatas;
                return CarTunings.Count > 0;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        return false;

    }

    public void SaveTuning()
    {
        var fullPath = Path.Combine(Application.persistentDataPath, nameof(CarTuningDataList));
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath) ?? string.Empty);
            var dataToStore = JsonUtility.ToJson(new CarTuningDataList(CarTunings), true);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            // ignored
        }
    }
    
    public void OwnedCarsAdd(int carIndex)
    {
        ownedCars.Add(carIndex);
        PlayerPrefs.SetString("ownedCars", string.Join("#", ownedCars));
    }

    public int SelectedCar
    {
        get => selectedCar;
        set
        {
            selectedCar = value;
            PlayerPrefs.SetInt("selectedCar", value);
            
        }
    }

    private void OnDisable()
    {
        SaveTuning();
    }
}