using UnityEngine;
using DG.Tweening;

namespace Abstracts
{
    public abstract class BaseMainMenuCanvasGroup : BaseCanvasGroup, ITweenable
    {
        protected MainMenuMediator _mainMenuMediator;

        private Vector3 _canvasGroupEnterPosition = new Vector3(1500f, 0f, 0f);
        private Vector3 _canvasGroupExitPosition = new Vector3(-1500f, 0f, 0f);
        private Sequence _moveSelfSequence;

        protected override void Awake()
        {
            base.Awake();
            _mainMenuMediator = GetComponentInParent<MainMenuMediator>();
        }
        public override void Hide()
        {
            StartPositionTween(Vector3.zero, _canvasGroupExitPosition);
        }
        public override void Show()
        {
            StartPositionTween(_canvasGroupEnterPosition, Vector3.zero);
        }

        private void StartPositionTween(Vector3 startPosition, Vector3 endPosition)
        {
            _moveSelfSequence = DOTween.Sequence();
            _moveSelfSequence.PrependCallback(() => { Block(); _myRectTransform.localPosition = startPosition; });
            _moveSelfSequence.Join(_myRectTransform.DOAnchorPos(endPosition, KeyDictionary.MAINMENU_TWEEN_DURATION, true).SetEase(Ease.OutElastic));
            _moveSelfSequence.AppendCallback(() => Unblock());
            _moveSelfSequence.Play();
        }

        protected virtual void OnDestroy()
        {
            _moveSelfSequence?.Kill();
        }
    }
}
