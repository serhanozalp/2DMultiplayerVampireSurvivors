using UnityEngine;
using DG.Tweening;
using Interfaces;
using Extensions;

namespace Abstracts
{
    public abstract class BaseMainMenuCanvasGroup : BaseCanvasGroup, ITweenPosition
    {
        protected MainMenuMediator _mainMenuMediator;

        private Vector2 _canvasGroupEnterPosition = new Vector3(1500f, 0f);
        private Vector2 _canvasGroupExitPosition = new Vector3(-1500f, 0f);

        protected override void Awake()
        {
            base.Awake();
            _mainMenuMediator = GetComponentInParent<MainMenuMediator>();
        }

        public override void Hide()
        {
            StartPositionTween(Vector3.zero, _canvasGroupExitPosition, new TweenWrapper
            {
                TweenDuration = ConstantDictionary.MAINMENU_TWEEN_DURATION_MOVE,
                EaseType = ConstantDictionary.MAINMENU_TWEEN_EASETYPE_MOVE,
                OnStartCallBack = () =>  Block(),
                OnCompleteCallBack = () => Unblock()
            }).Play();
        }

        public override void Show()
        {
            StartPositionTween(_canvasGroupEnterPosition, Vector3.zero, new TweenWrapper
            {
                TweenDuration = ConstantDictionary.MAINMENU_TWEEN_DURATION_MOVE,
                EaseType = ConstantDictionary.MAINMENU_TWEEN_EASETYPE_MOVE,
                OnStartCallBack = () => Block(),
                OnCompleteCallBack = () => Unblock()
            }).Play();
        }

        public Tween StartPositionTween(Vector3 startPosition, Vector3 endPosition, TweenWrapper tweenWrapper)
        {
            return _myRectTransform.DOAnchorPos(endPosition.ToVector2(), tweenWrapper.TweenDuration, true)
                .From(startPosition.ToVector2())
                .SetEase(tweenWrapper.EaseType)
                .OnStart(() => tweenWrapper.OnStartCallBack())
                .OnComplete(() => tweenWrapper.OnCompleteCallBack());
        }

        public void KillAllTweens()
        {
            DOTween.Kill(this);
        }

        protected virtual void OnDestroy()
        {
            KillAllTweens();
        }
    }
}
