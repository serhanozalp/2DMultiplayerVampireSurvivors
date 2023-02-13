using Abstracts;
using Interfaces;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using System;
using System.Collections;

public class Popup : BaseCanvasGroup, ITweenFade, ITweenPosition, ITweenScale
{
    [SerializeField]
    private TMP_Text _textTitle, _textBody;
    [SerializeField]
    private Button _buttonClose;

    private Action<Popup> _onHiddenCallBack;

    protected override void Awake()
    {
        base.Awake();
        _buttonClose.onClick.AddListener(() => ButtonCloseClicked());
        StartCoroutine(HideCoroutine());
    }

    private void ButtonCloseClicked()
    {
        Hide();
    }

    private IEnumerator HideCoroutine()
    {
        yield return new WaitForSecondsRealtime(ConstantDictionary.PopupConstantDictionary.POPUP_LIFE_DURATION);
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
        StartFadeTween(1f, 0f, new TweenWrapper
        {
            tweenDuration = ConstantDictionary.PopupConstantDictionary.POPUP_TWEEN_DURATION_FADE,
            easeType = ConstantDictionary.PopupConstantDictionary.POPUP_TWEEN_EASETYPE_FADE,
            OnStartCallBack = () => Block(),
            OnCompleteCallBack = () => { Unblock(); _onHiddenCallBack(this); }
        }).Play();
    }

    public override void Show()
    {
        StartScaleTween(Vector3.zero, Vector3.one, new TweenWrapper {
            tweenDuration = ConstantDictionary.PopupConstantDictionary.POPUP_TWEEN_DURATION_SCALE,
            easeType = ConstantDictionary.PopupConstantDictionary.POPUP_TWEEN_EASETYPE_SCALE,
            OnStartCallBack = () => Block(),
            OnCompleteCallBack = () => Unblock()
        }).Play();
    }

    public Tween StartFadeTween(float startAlpha, float endAlpha, TweenWrapper tweenWrapper)
    {
        return _myCanvasGroup.DOFade(endAlpha, tweenWrapper.tweenDuration)
            .From(startAlpha)
            .SetEase(tweenWrapper.easeType)
            .OnStart(() => tweenWrapper.OnStartCallBack())
            .OnComplete(() => tweenWrapper.OnCompleteCallBack())
            .SetLink(this.gameObject, LinkBehaviour.KillOnDestroy);
    }

    public Tween StartPositionTween(Vector3 startPosition, Vector3 endPosition, TweenWrapper tweenWrapper)
    {
        return _myRectTransform.DOAnchorPos(endPosition, tweenWrapper.tweenDuration)
            .From(startPosition)
            .SetEase(tweenWrapper.easeType)
            .OnStart(() => tweenWrapper.OnStartCallBack())
            .OnComplete(() => tweenWrapper.OnCompleteCallBack())
            .SetLink(this.gameObject, LinkBehaviour.KillOnDestroy);
    }

    public Tween StartScaleTween(Vector3 startScale, Vector3 endScale, TweenWrapper tweenWrapper)
    {
        return _myRectTransform.DOScale(endScale, tweenWrapper.tweenDuration)
            .From(startScale)
            .SetEase(tweenWrapper.easeType)
            .OnStart(() => tweenWrapper.OnStartCallBack())
            .OnComplete(() => tweenWrapper.OnCompleteCallBack())
            .SetLink(this.gameObject, LinkBehaviour.KillOnDestroy);
    }

    public void KillAllTweens()
    {
        DOTween.Kill(this);
    }

    private void OnDestroy()
    {
        KillAllTweens();
        StopAllCoroutines();
    }
}
