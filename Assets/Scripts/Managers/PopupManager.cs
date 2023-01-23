using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Abstracts;
using DG.Tweening;

public class PopupManager : MonoBehaviourSingleton<PopupManager>
{
    [SerializeField]
    private Popup _prefabPopup;

    private struct PopupCommand
    {
        public string PopupTitle;
        public string PopupBody;
    }

    private Sequence _movePopupsSequence;
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
        if (_popupCommandQueue.Count >= KeyDictionary.POPUPMANAGER_MAX_POPUP_ADD) return;
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
        while (_popupCommandQueue.Count > 0 && _popupList.Count < KeyDictionary.POPUPMANAGER_MAX_POPUP_SHOW)
        {
            PopupCommand popupCommand = _popupCommandQueue.Dequeue();
            Popup popup = Instantiate(_prefabPopup, transform);
            popup.Setup(popupCommand.PopupTitle, popupCommand.PopupBody, DestroyPopup);
            _popupList.Add(popup);
            popup.GetComponent<RectTransform>().anchoredPosition = GetPopupPosition(popup);
            popup.Show();
        }
    }

    private void UpdatePopupPositions(int startIndex)
    {
        _movePopupsSequence?.Kill();
        _movePopupsSequence = DOTween.Sequence();
        for (int i = startIndex; i < _popupList.Count; i++)
        {
            var popup = _popupList.ToArray()[i];
            _movePopupsSequence.Join(popup.GetComponent<RectTransform>().DOAnchorPos(GetPopupPosition(popup), KeyDictionary.POPUP_TWEEN_DURATION_MOVE, true));
        }
        _movePopupsSequence.Play();
    }

    private Vector2 GetPopupPosition(Popup popup)
    {
        return new Vector2(0f, _popupList.FindIndex(x => x == popup) * ( _popupHeight + KeyDictionary.POPUPMANAGER_MAX_POPUP_SPACING) * -1f);
    }

    private void OnDestroy()
    {
        _movePopupsSequence.Kill();
    }
}
