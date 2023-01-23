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

    private Action<ProfileListItemUI> _onButtonProfileNameClickedCallBack, _onButtonDeleteProfileClickedCallBack;

    public string ProfileName { get => _textProfileName.text; }

    private void Awake()
    {
        _buttonProfileName.onClick.AddListener(() => _onButtonProfileNameClickedCallBack(this));
        _buttonDeleteProfile.onClick.AddListener(() => _onButtonDeleteProfileClickedCallBack(this));
    }

    public void Setup(string profileName, Action<ProfileListItemUI> OnButtonProfileNameClickedCallBack, Action<ProfileListItemUI> OnButtonDeleteProfileClickedCallBack)
    {
        _textProfileName.text = profileName;
        _onButtonProfileNameClickedCallBack = OnButtonProfileNameClickedCallBack;
        _onButtonDeleteProfileClickedCallBack = OnButtonDeleteProfileClickedCallBack;
    }
}
