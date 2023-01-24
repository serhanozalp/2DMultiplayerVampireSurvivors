using System;
using UnityEngine;
using DG.Tweening;

namespace Interfaces
{
    public struct TweenWrapper
    {
        public Action OnStartCallBack;
        public Action OnCompleteCallBack;
        public float TweenDuration;
        public Ease EaseType;
    }
    public interface ITween
    {
        public void KillAllTweens();
    }

    public interface ITweenPosition : ITween
    {
        public Tween StartPositionTween(Vector3 startPosition, Vector3 endPosition, TweenWrapper tweenWrapper);
    }

    public interface ITweenScale : ITween
    {
        public Tween StartScaleTween(Vector3 startScale, Vector3 endScale, TweenWrapper tweenWrapper);
    }

    public interface ITweenFade : ITween
    {
        public Tween StartFadeTween(float startAlpha, float endAlpha, TweenWrapper tweenWrapper);
    }
}

