using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.AppCore;
using Game.Settings;
using Game.Tools;
using DG.Tweening; 

namespace Game.UI
{
    public class UIWin : UIFullScreenPanel
    {

        [SerializeField]
        private Button restart;

        [SerializeField]
        private Button toMainMenu;

        [SerializeField]
        private List<Image> stars;

        [SerializeField]
        private Text stepsCount;

        private int unusedSteps;
        private int unusedStepsCounter;
        private List<Action> animationChain;

        private const float WAIT_TIME_BEFORE_ANIMATION = 0.6f;
        private const float SCALE_STEP_TEXT_COEF = 1.2f;
        private const float SCALE_STAR_COEF = 1.2f;
        private const float SCALE_ANIMATION_DURATION = 0.2f;
        private const float ANIMATION_PAUSE = 0.3f;

        private Color scaleStepColor = Color.red;
        private Color starCollectColor = Color.yellow;

        private Timer timer;

        public override void Init(LevelSettings levelSettings, ElementsSettings elSettings = null)
        {
            base.Init(levelSettings);
            restart.onClick.AddListener(Restart);
            toMainMenu.onClick.AddListener(ToMainMenu);
            CommandKeeper.Instance.GameWinUI += Invoke;
            CommandKeeper.Instance.UpdateSteps += UpdateSteps;
            timer = gameObject.AddComponent<Timer>();
        }

        private void UpdateSteps(int st)
        {
            unusedSteps = st;
        }

        private void Invoke()
        {
            unusedStepsCounter = unusedSteps;
            stepsCount.text = unusedStepsCounter.ToString();
            StartAnimation();
        }

        private void StartAnimation()
        {
            restart.enabled = false;
            toMainMenu.enabled = false;
            animationChain = new List<Action>();
            int starNum = 0;
            int step = Mathf.Max(1, levelSettings.StepsForStar);

            for (int i = 0; i <= unusedSteps; i += step)
            {
                if (starNum < stars.Count)
                {
                    int lockI = i;
                    if (i > 0)
                    {
                        animationChain.Add(() => { StarAnimation(lockI); StepsTextAnimation(); });
                    }
                    else
                    {
                        animationChain.Add(() => { StarAnimation(lockI); });
                    }
                }
                starNum++;
            }
            animationChain.Add(EnableButtons);

            ShowPanel(() =>
            {
                timer.StartTimer(WAIT_TIME_BEFORE_ANIMATION, NextAnimation); 
            });
        }

        private void NextAnimation()
        {
            animationChain[0].Invoke();
            animationChain.RemoveAt(0);
        }

        private void EnableButtons()
        {
            restart.enabled = true;
            toMainMenu.enabled = true;
        }

        private void DecreaseSteps()
        {
            unusedStepsCounter -= levelSettings.StepsForStar;
            stepsCount.text = unusedStepsCounter.ToString();
        }

        private void StepsTextAnimation()
        {

            stepsCount.transform.DOScale(SCALE_STEP_TEXT_COEF, SCALE_ANIMATION_DURATION)
                                .SetLoops(2, LoopType.Yoyo)
                                .SetDelay(ANIMATION_PAUSE);


            stepsCount.DOColor(scaleStepColor, SCALE_ANIMATION_DURATION)
                      .SetLoops(2, LoopType.Yoyo)
                      .SetDelay(ANIMATION_PAUSE)
                      .OnComplete(() => { DecreaseSteps(); });

        }

        private void StarAnimation(int num)
        {
            stars[num].transform.DOScale(SCALE_STAR_COEF, SCALE_ANIMATION_DURATION)
                                .SetLoops(2, LoopType.Yoyo)
                                .SetDelay(ANIMATION_PAUSE)
                                .OnComplete(() => { NextAnimation(); });

            stars[num].DOColor(starCollectColor, SCALE_ANIMATION_DURATION)
                      .SetDelay(ANIMATION_PAUSE);
        }

        private void Restart()
        {
            CommandKeeper.Instance.RestartGame(); 
        }

        private void ToMainMenu()
        {
            CommandKeeper.Instance.MakeTransitionInApp(AppMachine.Transition.ToMainMenu);
        }

        private void OnDestroy()
        {
            CommandKeeper.Instance.GameWinUI -= Invoke;
            CommandKeeper.Instance.UpdateSteps -= UpdateSteps;
        }
    }
}