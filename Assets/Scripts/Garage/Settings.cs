using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : Page
{
    [SerializeField] private TMP_InputField nameText;
    [SerializeField] private Button saveButton;

    private void Awake()
    {
        saveButton.onClick.AddListener(Save);
    }

    private void Save()
    {
        Player.Name = nameText.text;
        gameObject.SetActive(false);
        header.BackButton.onClick?.Invoke();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        nameText.text = Player.Name;
    }
}
