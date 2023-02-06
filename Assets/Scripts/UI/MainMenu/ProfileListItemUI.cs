using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ProfileListItemUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _textProfileName;
    [SerializeField]
    private Button _buttonProfileName, _buttonDeleteProfile;

    private Action<ProfileListItemUI> _onButtonProfileNameClicked, _onButtonDeleteProfileClicked;

    public string ProfileName { get => _textProfileName.text; }

    private void Awake()
    {
        _buttonProfileName.onClick.AddListener(() => _onButtonProfileNameClicked(this));
        _buttonDeleteProfile.onClick.AddListener(() => _onButtonDeleteProfileClicked(this));
    }

    public void Setup(string profileName, Action<ProfileListItemUI> OnButtonProfileNameClicked, Action<ProfileListItemUI> OnButtonDeleteProfileClicked)
    {
        _textProfileName.text = profileName;
        _onButtonProfileNameClicked = OnButtonProfileNameClicked;
        _onButtonDeleteProfileClicked = OnButtonDeleteProfileClicked;
    }
}
