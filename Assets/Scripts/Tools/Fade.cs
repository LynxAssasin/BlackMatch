using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI; 

namespace Game.Tools
{
    public class Fade : MonoBehaviour
    {
        private Image overlayPanel;
        private Canvas canvas;
        private Color fadeColor;  
        private const float  FADE_TIME = 0.5f; 

        private void CreateOverlayPanel()
        {
            if(overlayPanel == null)
            {
                GameObject obj = new GameObject("OverlayPanel");
                canvas = obj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;

                GameObject panel = new GameObject("Panel");
                panel.AddComponent<CanvasRenderer>();
                overlayPanel = panel.AddComponent<Image>();

                panel.transform.SetParent(canvas.transform, false);
                panel.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
            }
        }
        public void FadeIn(Action callback = null)
        {
            FadeColor(true, callback);
        }

        public void FadeOut()
        {
            FadeColor(false);
        }

        private void FadeColor(bool alphaIn, Action callback = null)
        {
            CreateOverlayPanel();
            canvas.enabled = true;
            Color alphaFade = fadeColor;
            alphaFade.a = alphaIn ? 0 : 1;
            overlayPanel.color = alphaFade;
            alphaFade.a = 1 - alphaFade.a;
            overlayPanel.DOColor(alphaFade, FADE_TIME).OnComplete(()=>
            {
                callback();
                if (!alphaIn)
                {
                    canvas.enabled = false;
                }
            });
        }
    }
}
