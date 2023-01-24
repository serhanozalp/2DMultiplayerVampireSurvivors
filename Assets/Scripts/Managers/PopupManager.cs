using System.Collections.Generic;
using UnityEngine;
using Abstracts;
using DG.Tweening;
using Interfaces.Tweens;

public class PopupManager : MonoBehaviourSingleton<PopupManager>
{
    [SerializeField]
    private Popup _prefabPopup;

    private struct PopupCommand
    {
        public string PopupTitle;
        public string PopupBody;
    }

    private float _popupHeight;
    private readonly Queue<PopupCommand> _popupCommandQueue = new Queue<PopupCommand>();
    private readonly List<Popup> _popupList = new List<Popup>();

    protected override void Awake()
    {
        base.Awake();
        _popupHeight = _prefabPopup.GetComponent<RectTransform>().sizeDelta.y;
    }

    public void AddPopup(string title, string body)
    {
        if (_popupCommandQueue.Count + _popupList.Count >= ConstantDictionary.PopupConstantDictionary.POPUPMANAGER_MAX_POPUP_ADD) return;
        _popupCommandQueue.Enqueue(new PopupCommand { PopupTitle = title, PopupBody = body });
        ShowAvailablePopups();
    }

    private void DestroyPopup(Popup popup)
    {
        var popupIndex = _popupList.FindIndex(x => x == popup);
        _popupList.Remove(popup);
        Destroy(popup.gameObject);
        UpdatePopupPositions(popupIndex);
        ShowAvailablePopups();
    }

    private void ShowAvailablePopups()
    {
        while (_popupCommandQueue.Count > 0 && _popupList.Count < ConstantDictionary.PopupConstantDictionary.POPUPMANAGER_MAX_POPUP_SHOW)
        {
            PopupCommand popupCommand = _popupCommandQueue.Dequeue();
            Popup popup = Instantiate(_prefabPopup, transform);
            popup.Setup(popupCommand.PopupTitle, popupCommand.PopupBody, DestroyPopup);
            _popupList.Add(popup);
            popup.GetComponent<RectTransform>().anchoredPosition = GetPopupActualPositionByIndex(popup);
            popup.Show();
        }
    }

    private void UpdatePopupPositions(int startIndex)
    {
        for (int i = startIndex; i < _popupList.Count; i++)
        {
            var popup = _popupList.ToArray()[i];
            popup.StartPositionTween(popup.GetComponent<RectTransform>().anchoredPosition, GetPopupActualPositionByIndex(popup), new TweenWrapper
            {
                TweenDuration = ConstantDictionary.PopupConstantDictionary.POPUP_TWEEN_DURATION_MOVE,
                EaseType = ConstantDictionary.PopupConstantDictionary.POPUP_TWEEN_EASETYPE_MOVE,
                OnStartCallBack = () => { },
                OnCompleteCallBack = () => { }
            }).Play();
        }
    }

    private Vector2 GetPopupActualPositionByIndex(Popup popup)
    {
        return new Vector2(0f, _popupList.FindIndex(x => x == popup) * ( _popupHeight + ConstantDictionary.PopupConstantDictionary.POPUPMANAGER_MAX_POPUP_SPACING) * -1f);
    }
}
