using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Abstracts;

public class CanvasGroupProfiles : BaseMainMenuCanvasGroup
{
    [SerializeField]
    private ProfileListItemUI _prefabProfileListItemUI;
    [SerializeField]
    private Transform _profileListContainer;
    [SerializeField]
    private Button _buttonBack, _buttonCreateProfile;
    [SerializeField]
    private TMP_InputField _inputFieldProfileName;

    protected override void Awake()
    {
        base.Awake();
        _buttonBack.onClick.AddListener(() => _mainMenuMediator.ShowCanvasGroupStartGame());
        _buttonCreateProfile.onClick.AddListener(() => ButtonCreateProfileClicked());
    }

    private void ButtonCreateProfileClicked()
    {
        _mainMenuMediator.AddProfile(_inputFieldProfileName.text);
        SetupProfileListUI();
    }

    public override void Show()
    {
        base.Show();
        SetupProfileListUI();
    }

    private void ClearProfileListUI()
    {
        for (int i = 0; i < _profileListContainer.childCount; i++)
        {
            Destroy(_profileListContainer.GetChild(i).gameObject);
        }
    }

    private void SetupProfileListUI()
    {
        ClearProfileListUI();
        List<string> profileList = _mainMenuMediator.GetProfileList();
        foreach (var profileName in profileList)
        {
            AddProfileListItemUI(profileName);
        }
    }

    private void AddProfileListItemUI(string profileName)
    {
        var profileListItemUI = Instantiate(_prefabProfileListItemUI, _profileListContainer);
        profileListItemUI.Setup(profileName, ChangeProfileName, DeleteProfile);
    }

    private void DeleteProfile(ProfileListItemUI sender)
    {
        _mainMenuMediator.DeleteProfile(sender.ProfileName);
        SetupProfileListUI();
    }

    private void ChangeProfileName(ProfileListItemUI sender)
    {
        _mainMenuMediator.ChangeProfileName(sender.ProfileName);
        _mainMenuMediator.ShowCanvasGroupStartGame();
    }
}