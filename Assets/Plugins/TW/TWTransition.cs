using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TomWill
{
    public class TWTransition : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private RectTransform imageRect;
        public enum TransitionType { DEFAULT_IN, DEFAULT_OUT, UP_IN, UP_OUT, DOWN_IN, DOWN_OUT}
        [SerializeField] private float baseTimeToFade = 1;
        [SerializeField] private Color baseColor;

        private float offsetMove = 10f;
        private float timeToFade;
        private bool inFading;
        private Color colorFading;
        private UnityEvent onComplete;

        private static TWTransition instance;

        #region Setting
        private static TWTransition Instance
        {
            get
            {
                if (instance == null)
                {
                    Debug.Log("Instance of TWTransition is not yet created");
                }

                return instance;
            }
        }
        #endregion

        #region Unity Function
        private void Awake()
        {
            instance = this;
            onComplete = new UnityEvent();
        }
        #endregion

        #region TWTransition

        public static void ScreenTransition(TransitionType type, float duration = -1f, UnityAction action = null)
        {
            Instance?.screenTransition(type, duration, action);
        }

        public static void ScreenFlash(int flashCount = 1, float duration = 0.1f, UnityAction action = null)
        {
            Instance?.screenFlash(flashCount, duration, action);
        }
        #endregion

        #region Internal Function

        private void screenFlash(int flashCount, float duration, UnityAction action)
        {
            ChangeColor(Color.white);
            transform.localPosition = new Vector3(0f, 0f, 11f);
            DOTween.Sequence()
                .Append(image.DOFade(1,duration/2f))
                .Append(image.DOFade(0, duration/2f))
                .SetLoops(flashCount)
                .OnComplete(()=> { if (action != null) action.Invoke(); });
        }

        private void screenTransition(TransitionType type, float duration, UnityAction action)
        {
            switch (type)
            {
                case TransitionType.DEFAULT_IN:
                    Instance?.screenTransitionFadeIn(duration, action);
                    break;
                case TransitionType.DEFAULT_OUT:
                    Instance?.screenTransitionFadeOut(duration, action);
                    break;
                case TransitionType.UP_IN:
                    Instance?.screenTransitionUpIn(duration, action);
                    break;
                case TransitionType.UP_OUT:
                    Instance?.screenTransitionUpOut(duration, action);
                    break;
                case TransitionType.DOWN_IN:
                    Instance?.screenTransitionDownIn(duration, action);
                    break;
                case TransitionType.DOWN_OUT:
                    Instance?.screenTransitionDownOut(duration, action);
                    break;
            }
        }

        private void screenTransitionUpIn(float duration, UnityAction action)
        {
            if (!inFading)
            {
                inFading = true;
                onComplete.RemoveAllListeners();
                onComplete.AddListener(action == null ? NullHandler : action);
                timeToFade = duration < 0 ? baseTimeToFade : duration;
                transform.localPosition = new Vector3( 0f, -offsetMove, 11f);
                ChangeToBaseColor();
                colorFading = new Color(baseColor.r, baseColor.g, baseColor.b, 1);
                image.color = colorFading;

                transform.DOLocalMoveY(0, timeToFade).OnComplete(()=>
                {
                    inFading = false;
                    onComplete.Invoke();
                }).SetEase(Ease.Linear);
            }
        }

        private void screenTransitionUpOut(float duration, UnityAction action)
        {
            if (!inFading)
            {
                inFading = true;
                onComplete.RemoveAllListeners();
                onComplete.AddListener(action == null ? NullHandler : action);
                timeToFade = duration < 0 ? baseTimeToFade : duration;
                transform.localPosition = new Vector3(0f, 0f, 11f);
                ChangeToBaseColor();
                colorFading = new Color(baseColor.r, baseColor.g, baseColor.b, 1);
                image.color = colorFading;

                transform.DOLocalMoveY(offsetMove, timeToFade).OnComplete(() =>
                {
                    inFading = false;
                    image.DOFade(0f, 0f);
                    onComplete.Invoke();
                }).SetEase(Ease.Linear);
            }
        }

        private void screenTransitionDownIn(float duration, UnityAction action)
        {
            if (!inFading)
            {
                inFading = true;
                onComplete.RemoveAllListeners();
                onComplete.AddListener(action == null ? NullHandler : action);
                timeToFade = duration < 0 ? baseTimeToFade : duration;
                transform.localPosition = new Vector3(0f, offsetMove, 11f);
                ChangeToBaseColor();
                colorFading = new Color(baseColor.r, baseColor.g, baseColor.b, 1);
                image.color = colorFading;

                transform.DOLocalMoveY(0, timeToFade).OnComplete(() =>
                {
                    inFading = false;
                    onComplete.Invoke();
                }).SetEase(Ease.Linear);
            }
        }

        private void screenTransitionDownOut(float duration, UnityAction action)
        {
            if (!inFading)
            {
                inFading = true;
                onComplete.RemoveAllListeners();
                onComplete.AddListener(action == null ? NullHandler : action);
                timeToFade = duration < 0 ? baseTimeToFade : duration;
                transform.localPosition = new Vector3(0f, 0f, 11f);
                ChangeToBaseColor();
                colorFading = new Color(baseColor.r, baseColor.g, baseColor.b, 1);
                image.color = colorFading;

                transform.DOLocalMoveY(-offsetMove, timeToFade).OnComplete(() =>
                {
                    inFading = false;
                    image.DOFade(0f, 0f);
                    onComplete.Invoke();
                }).SetEase(Ease.Linear);
            }
        }

        private void screenTransitionFadeIn(float duration, UnityAction action)
        {
            if (!inFading)
            {
                inFading = true;
                onComplete.RemoveAllListeners();
                onComplete.AddListener(action == null ? NullHandler : action);
                timeToFade = duration < 0 ? baseTimeToFade : duration;
                transform.localPosition = new Vector3(0f, 0f, 11f);
                ChangeToBaseColor();
                colorFading = new Color(baseColor.r, baseColor.g, baseColor.b, 0);
                image.color = colorFading;

                image.DOFade(1f, timeToFade).OnComplete(() =>
                {
                    inFading = false;
                    onComplete.Invoke();
                }).SetEase(Ease.Linear);
            }
        }

        private void screenTransitionFadeOut(float duration, UnityAction action)
        {
            if (!inFading)
            {
                inFading = true;
                onComplete.RemoveAllListeners();
                onComplete.AddListener(action == null ? NullHandler : action);
                timeToFade = duration < 0 ? baseTimeToFade : duration;
                transform.localPosition = new Vector3(0f, 0f, 11f);
                ChangeToBaseColor();
                colorFading = new Color(baseColor.r, baseColor.g, baseColor.b, 1);
                image.color = colorFading;

                image.DOFade(0f, timeToFade).OnComplete(() =>
                {
                    inFading = false;
                    image.DOFade(0f, 0f);
                    onComplete.Invoke();
                }).SetEase(Ease.Linear);
            }
        }

        private void NullHandler()
        {

        }

        private void ChangeColor(Color color)
        {
            image.color = color;
        }

        private void ChangeToBaseColor()
        {
            image.color = baseColor;
        }
        #endregion
    }
}