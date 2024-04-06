using System;
using UnityEngine;

public static class Player
{
    private static string name;
    private static int gold;
    private static int cash;
    private static int[] ownedCars;
    private static int selectedCar;
    
    public static string Name => name;
    public static int Gold => gold;

    public static int Cash
    {
        get => cash;
        set
        {
            cash = value;
            PlayerPrefs.SetInt("cash", value);
        }
    }

    public static int[] OwnedCars => ownedCars;

    public static int SelectedCar
    {
        get => selectedCar;
        set
        {
            selectedCar = value;
            PlayerPrefs.SetInt("selectedCar", value);
        }
    }

    static Player()
    {
        name = PlayerPrefs.GetString("name", string.Empty);
        gold = PlayerPrefs.GetInt("gold", 0);
        cash = PlayerPrefs.GetInt("cash", 0);
        var ownedCarsString = PlayerPrefs.GetString("ownedCars").Split(new []{"#"}, StringSplitOptions.None);
        if (ownedCarsString.Length == 1) ownedCarsString[0] = "0";
        ownedCars = Array.ConvertAll(ownedCarsString, int.Parse);
        selectedCar = PlayerPrefs.GetInt("selectedCar", 0);
    }
}