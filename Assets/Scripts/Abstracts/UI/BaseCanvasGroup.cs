using UnityEngine;

namespace Abstracts
{
    [RequireComponent(typeof(CanvasGroup), typeof(RectTransform))]
    public abstract class BaseCanvasGroup : MonoBehaviour
    {
        protected CanvasGroup _myCanvasGroup;
        protected RectTransform _myRectTransform;

        protected virtual void Awake()
        {
            _myCanvasGroup = gameObject.GetComponent<CanvasGroup>();
            _myRectTransform = gameObject.GetComponent<RectTransform>();
        }

        public abstract void Show();
        public abstract void Hide();
        protected void Block()
        {
            _myCanvasGroup.interactable = false;
        }
        protected void Unblock()
        {
            _myCanvasGroup.interactable = true;
        }
    }
}

