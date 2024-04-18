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
    private string _nickname;
    private int _gold;
    private int _cash;
    private List<int> _ownedCars;
    private int _selectedCar;
    private List<CarTuningData> _carTunings = new();
    private string _fullPath;
    private bool _isOnline;

    public bool IsOnline
    {
        get => _isOnline;
        set => _isOnline = value;
    }

    public Action<int> OnGoldChange;
    public Action<int> OnCashChange;

    
    public static Player Instance => _instance;
    public List<CarTuningData> CarTunings => _carTunings;
    public string Nickname
    {
        get => _nickname;
        set
        {
            _nickname = value;
            PlayerPrefs.SetString("nickname", value);
        }
    }

    public int Gold
    {
        get => _gold;
        set
        {
            _gold = value;
            PlayerPrefs.SetInt("gold", value);
            OnGoldChange?.Invoke(value);
        }
    }

    public int Cash
    {
        get => _cash;
        set
        {
            _cash = value;
            PlayerPrefs.SetInt("cash", value);
            OnCashChange?.Invoke(value);
        }
    }

    public IReadOnlyList<int> OwnedCars => _ownedCars;

    private void Awake()
    {
        if (Instance && Instance != this)
            Destroy(this);
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            Load();
        }
    }

    private void Load()
    {
        _fullPath = Path.Combine(Application.persistentDataPath, nameof(CarTuningDataList));
        _nickname = PlayerPrefs.GetString("nickname", string.Empty);
        Gold = PlayerPrefs.GetInt("gold", 0);
        Cash = PlayerPrefs.GetInt("cash", 0);
        var ownedCarsString = PlayerPrefs.GetString("ownedCars").Split(new []{"#"}, StringSplitOptions.None);
        if (ownedCarsString.Length == 1) ownedCarsString[0] = "0";
        _ownedCars = Array.ConvertAll(ownedCarsString, int.Parse).ToList();
        _selectedCar = PlayerPrefs.GetInt("selectedCar", 0);
        
        if (LoadTuning()) return;
        _carTunings.Clear();
        foreach (Car car in carList)
        {
            _carTunings.Add(new CarTuningData(car.carInfo.CarTuning.Data));
        }
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

            _carTunings = JsonUtility.FromJson<CarTuningDataList>(dataToLoad).carTuningDatas;
            return _carTunings.Count > 0;
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
            var dataToStore = JsonUtility.ToJson(new CarTuningDataList(_carTunings), true);

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
        Load();
    }

    public void AddBalance(bool isGold, int amount)
    {
        if (isGold)
            Gold += amount;
        else
            Cash += amount;
    }
    
    public void OwnedCarsAdd(int carIndex)
    {
        _ownedCars.Add(carIndex);
        PlayerPrefs.SetString("ownedCars", string.Join("#", _ownedCars));
    }

    public int SelectedCar
    {
        get => _selectedCar;
        set
        {
            _selectedCar = value;
            PlayerPrefs.SetInt("selectedCar", value);
            
        }
    }

    public int SelectPreviousCar()
    {
        if (_selectedCar == _ownedCars[0])
        {
            SelectedCar = _ownedCars.Last();
        }
        else
        {
            var index = _ownedCars.IndexOf(_selectedCar);
            SelectedCar = _ownedCars[--index % _ownedCars.Count];
        }

        return _selectedCar;
    }
    
    public int SelectNextCar()
    {
        if (_selectedCar == _ownedCars.Last())
        {
            SelectedCar = _ownedCars[0];
        }
        else
        {
            var index = _ownedCars.IndexOf(_selectedCar);
            SelectedCar = _ownedCars[++index % _ownedCars.Count];
        }

        return _selectedCar;
    }
}