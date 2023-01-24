using UnityEngine;
using Abstracts;
using UnityEngine.UI;
using System;

public class ToggleSet : MonoBehaviour
{
    [SerializeField]
    private ToggleSetButton[] _toggleSetButtonArray;
    [SerializeField]
    private BaseCanvasGroup[] _canvasGroupArray;

    private void Awake()
    {
        TrySetButtonConnections();
    }

    private void Start()
    {
        ToggleSetButtonClicked(0);
    }

    private void TrySetButtonConnections()
    {
        try
        {
            for (int i = 0; i < _toggleSetButtonArray.Length; i++)
            {
                var index = i;
                _toggleSetButtonArray[i].GetComponent<Button>().onClick.AddListener(() => ToggleSetButtonClicked(index));
            }
        }
        catch (Exception)
        {
            Debug.LogError($"ToggleSet { gameObject.name } not setup properly!");
        }
    }

    private void ToggleSetButtonClicked(int index)
    {
        ShowCanvasGroup(index);
        SelectToggleSetButton(index);
    }

    private void ShowCanvasGroup(int index)
    {
        for (int i = 0; i < _canvasGroupArray.Length; i++)
        {
            if (i == index) _canvasGroupArray[i].Show();
            else _canvasGroupArray[i].Hide();
        }
    }

    private void SelectToggleSetButton(int index)
    {
        for (int i = 0; i < _toggleSetButtonArray.Length; i++)
        {
            if (i == index) _toggleSetButtonArray[i].Select();
            else _toggleSetButtonArray[i].Deselect();
        }
    }
}
