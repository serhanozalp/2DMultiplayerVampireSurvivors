using Abstracts;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using System;
using System.Collections;

public class Popup : BaseCanvasGroup, ITweenable
{
    [SerializeField]
    private TMP_Text _textTitle, _textBody;
    [SerializeField]
    private Button _buttonClose;

    private Action<Popup> _onHiddenCallBack;
    private Sequence _scaleSelfSequence;
    private Sequence _fadeSelfSequence;

    protected override void Awake()
    {
        base.Awake();
        _buttonClose.onClick.AddListener(() => ButtonCloseClicked());
        StartCoroutine(HideCoroutine());
    }

    private IEnumerator HideCoroutine()
    {
        yield return new WaitForSecondsRealtime(KeyDictionary.POPUP_LIFE_DURATION);
        Hide();
    }

    public void Setup(string title, string body, Action<Popup> OnHiddenCallBack)
    {
        _textTitle.text = title;
        _textBody.text = body;
        _onHiddenCallBack = OnHiddenCallBack;
    }

    public override void Hide()
    {
        StartHideTween();
    }

    public override void Show()
    {
        StartShowTween();
    }

    private void StartShowTween()
    {
        _scaleSelfSequence = DOTween.Sequence();
        _scaleSelfSequence.PrependCallback(() => Block());
        _scaleSelfSequence.Join(_myRectTransform.DOScale(Vector3.one, KeyDictionary.POPUP_TWEEN_DURATION_SHOW).SetEase(Ease.InQuart));
        _scaleSelfSequence.AppendCallback(() => Unblock());
        _scaleSelfSequence.Play();
    }

    private void StartHideTween()
    {
        _fadeSelfSequence = DOTween.Sequence();
        _fadeSelfSequence.PrependCallback(() => Block());
        _fadeSelfSequence.Join(_myCanvasGroup.DOFade(0f, KeyDictionary.POPUP_TWEEN_DURATION_HIDE));
        _fadeSelfSequence.AppendCallback(() => { Unblock(); _onHiddenCallBack(this); });
        _fadeSelfSequence.Play();
    }

    private void ButtonCloseClicked()
    {
        Hide();
    }

    private void OnDestroy()
    {
        _scaleSelfSequence?.Kill();
        _fadeSelfSequence?.Kill();
        StopAllCoroutines();
    }
}
