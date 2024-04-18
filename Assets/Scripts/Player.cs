using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tuning;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private CarList carList;
    private static Player _instance;
    private PlayerData _data = new();
    private string _fullPath;
    private bool _isOnline;
    
    public PlayerData Data
    {
        get => _data;
        set
        {
            _data = value;
            SaveData();
        }
    }

    public bool IsOnline
    {
        get => _isOnline;
        set => _isOnline = value;
    }

    public Action<int> OnGoldChange;
    public Action<int> OnCashChange;

    
    public static Player Instance => _instance;
    public List<CarTuningData> CarTunings => _data.CarTunings;
    public string Nickname
    {
        get => _data.nickname;
        set
        {
            _data.nickname = value;
            PlayerPrefs.SetString("nickname", value);
        }
    }

    public int Gold
    {
        get => _data.gold;
        set
        {
            _data.gold = value;
            PlayerPrefs.SetInt("gold", value);
            OnGoldChange?.Invoke(value);
        }
    }

    public int Cash
    {
        get => _data.cash;
        set
        {
            _data.cash = value;
            PlayerPrefs.SetInt("cash", value);
            OnCashChange?.Invoke(value);
        }
    }

    public IReadOnlyList<int> OwnedCars => _data.ownedCars;

    private void Awake()
    {
        if (Instance && Instance != this)
            Destroy(this);
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();
        }
    }

    private void LoadData()
    {
        _fullPath = Path.Combine(Application.persistentDataPath, nameof(CarTuningDataList));
        _data.nickname = PlayerPrefs.GetString("nickname", string.Empty);
        Gold = PlayerPrefs.GetInt("gold", 0);
        Cash = PlayerPrefs.GetInt("cash", 0);
        var ownedCarsString = PlayerPrefs.GetString("ownedCars").Split(new []{"#"}, StringSplitOptions.None);
        if (ownedCarsString.Length == 1) ownedCarsString[0] = "0";
        _data.ownedCars = Array.ConvertAll(ownedCarsString, int.Parse).ToList();
        _data.selectedCar = PlayerPrefs.GetInt("selectedCar", 0);
        
        if (LoadTuning()) return;
        _data.CarTunings.Clear();
        foreach (Car car in carList)
        {
            _data.CarTunings.Add(new CarTuningData(car.carInfo.CarTuning.Data));
        }
    }

    private void SaveData()
    {
        Nickname = _data.nickname;
        SelectedCar = _data.selectedCar;
        Cash = _data.cash;
        Gold = _data.gold;
        PlayerPrefs.SetString("ownedCars", string.Join("#", _data.ownedCars));
        SaveTuning();
    }

    private bool LoadTuning()
    {
        if (!File.Exists(_fullPath)) return false;
        try
        {
            string dataToLoad;
            using (var stream = new FileStream(_fullPath, FileMode.Open))
            {
                using (var reader = new StreamReader(stream))
                {
                    dataToLoad = reader.ReadToEnd();
                }
            }

            _data.CarTunings = JsonUtility.FromJson<CarTuningDataList>(dataToLoad).CarTuningDatas;
            return _data.CarTunings.Count > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public void SaveTuning()
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_fullPath) ?? string.Empty);
            var dataToStore = JsonUtility.ToJson(new CarTuningDataList(_data.CarTunings), true);

            using (var stream = new FileStream(_fullPath, FileMode.Create))
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception)
        {
            // ignored
        }
    }

    public void EraseData()
    {
        PlayerPrefs.DeleteAll();
        File.Delete(_fullPath);
        LoadData();
    }

    public void AddBalance(bool isGold, int amount)
    {
        if (isGold)
            Gold += amount;
        else
            Cash += amount;
    }
    
    public void AddOwnedCar(int carIndex)
    {
        _data.ownedCars.Add(carIndex);
        PlayerPrefs.SetString("ownedCars", string.Join("#", _data.ownedCars));
    }

    public int SelectedCar
    {
        get => _data.selectedCar;
        set
        {
            _data.selectedCar = value;
            PlayerPrefs.SetInt("selectedCar", value);
            
        }
    }

    public int SelectPreviousCar()
    {
        if (_data.selectedCar == _data.ownedCars[0])
        {
            SelectedCar = _data.ownedCars.Last();
        }
        else
        {
            var index = _data.ownedCars.IndexOf(_data.selectedCar);
            SelectedCar = _data.ownedCars[--index % _data.ownedCars.Count];
        }

        return _data.selectedCar;
    }
    
    public int SelectNextCar()
    {
        if (_data.selectedCar == _data.ownedCars.Last())
        {
            SelectedCar = _data.ownedCars[0];
        }
        else
        {
            var index = _data.ownedCars.IndexOf(_data.selectedCar);
            SelectedCar = _data.ownedCars[++index % _data.ownedCars.Count];
        }

        return _data.selectedCar;
    }
}

[Serializable]
public class PlayerData
{
    public string nickname;
    public int gold;
    public int cash;
    public List<int> ownedCars;
    public int selectedCar;
    public CarTuningDataList carTunings = new();

    public List<CarTuningData> CarTunings
    {
        get => carTunings.CarTuningDatas;
        set => carTunings.CarTuningDatas = value;
    }
}