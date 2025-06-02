using System.Threading.Tasks;
using CandyCabinets.Components.Colour;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.CustomAnimation
{
    public static class CustomAnimation
    {
        private static float BUTTON_SHRINK_ON_CLICK = 0.9f;
        private static float BUTTON_SHRINK_REVERSE_ON_CLICK = 1f;
        private static float NODE_SHRINK_ON_CLICK = 10f;
        private static float NODE_SHRINK_REVERSE_ON_CLICK = 15f;
        private static float BUTTON_SHRINK_ON_CLICK_TIME = 0.1f;
        private static float BUTTON_SHRINK_REVERSE_ON_CLICK_TIME = 0.05f;
        private static float BUTTON_SHRINK_ON_CLICK_OPACITY = 0.6f;
        private static float BUTTON_SHRINK_ON_CLICK_OPACITY_TIME = BUTTON_SHRINK_ON_CLICK_TIME;
        private static float NUMBER_CLICKED_SIZE = 2.25f;
        private static float NUMBER_CLICKED_TIME = 0.1f;
        private static float NUMBER_DROPPED_SCALE_TIME = 0.1f;
        private static float NUMBER_DROPPED_MOVE_TIME = 0.2f;
        private static float SUM_CORRECT_ANIMATION_JUMP_FORCE = 0.06f;
        private static float SUM_CORRECT_ANIMATION_DURATION = 0.3f;
        private static float PUZZLE_COMPLETED_ANIMATION_DURATION = 0.5f;
        private static int WAIT_FOR_ANIMATION_TO_FINISH_DELAY = 200;
        private static float TIME_REWARD_TEXT_POP_UP_DURATION = 0.75f;
        private static float TIME_REWARD_TEXT_MOVE_DURATION = 0.5f;
        private static float TIME_REWARD_TEXT_SHRINK_DURATION = 0.7f;
        private static float TIME_REWARD_ICON_SHAKE_DURATION = 0.6f;
        private static float TIME_REWARD_ICON_TURN_GREEN_DURATION = 0.33f;
        private static float TIME_REWARD_ICON_REVERT_FROM_GREEN_DURATION = 0.1f;

        public static async Task ButtonClicked(Transform target)
        {
            await ButtonClicked(target, Constants.AudioClip.MenuInteraction, true);
        }

        public static async Task ButtonClicked(Transform target, bool playSFX)
        {
            await ButtonClicked(target, Constants.AudioClip.MenuInteraction, playSFX);
        }

        public static async Task ButtonClicked(
            Transform target,
            Constants.AudioClip clipToPlay,
            bool playSFX
        )
        {
            target.gameObject.GetComponent<Button>().enabled = false;
            var sequence = DOTween.Sequence();
            sequence.Append(target.DOScale(BUTTON_SHRINK_ON_CLICK, BUTTON_SHRINK_ON_CLICK_TIME));
            sequence.Append(
                target
                    .GetComponent<Image>()
                    .DOFade(BUTTON_SHRINK_ON_CLICK_OPACITY, BUTTON_SHRINK_ON_CLICK_OPACITY_TIME)
                    .From()
                    .SetEase(Ease.OutQuad)
            );
            sequence.Append(
                target.DOScale(BUTTON_SHRINK_REVERSE_ON_CLICK, BUTTON_SHRINK_REVERSE_ON_CLICK_TIME)
            );
            sequence.SetId("ButtonClick" + target.name).Play();
            if (playSFX)
            {
                Object.FindObjectOfType<UIManager>().InteractionPerformed(clipToPlay);
            }

            await WaitForAnimation("ButtonClick" + target.name);
            target.gameObject.GetComponent<Button>().enabled = true;
        }

        public static async Task TextClicked(Transform target)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(target.DOScale(BUTTON_SHRINK_ON_CLICK, BUTTON_SHRINK_ON_CLICK_TIME));
            sequence.Append(
                target
                    .GetComponent<TextMeshProUGUI>()
                    .DOFade(BUTTON_SHRINK_ON_CLICK_OPACITY, BUTTON_SHRINK_ON_CLICK_OPACITY_TIME)
                    .From()
                    .SetEase(Ease.OutQuad)
            );
            sequence.Append(
                target.DOScale(BUTTON_SHRINK_REVERSE_ON_CLICK, BUTTON_SHRINK_REVERSE_ON_CLICK_TIME)
            );
            sequence.SetId("TextShrink" + target.name).Play();

            await WaitForAnimation("TextShrink" + target.name);
        }

        public static void NumberClicked(Transform transform)
        {
            transform.DOScale(NUMBER_CLICKED_SIZE, NUMBER_CLICKED_TIME);
        }

        public static Sequence NumberDropped(Transform transform, Vector3 destination)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(1f, NUMBER_DROPPED_SCALE_TIME));
            sequence.Join(transform.DOMove(destination, NUMBER_DROPPED_MOVE_TIME));

            return sequence;
        }

        public static void NumberSwitched(Transform transform, Vector3 destination)
        {
            transform.DOMove(destination, NUMBER_DROPPED_MOVE_TIME).SetId("MoveNumberBack");
        }

        public static Sequence SumIsCorrect(Transform transform, string name)
        {
            return SumIsCorrect(transform, transform.position, name);
        }

        public static Sequence SumIsCorrect(Transform transform, Vector3 position, string name)
        {
            if (DOTween.TotalTweensById("NumberJump" + name) == 0)
            {
                return transform
                    .DOJump(
                        position,
                        SUM_CORRECT_ANIMATION_JUMP_FORCE,
                        1,
                        SUM_CORRECT_ANIMATION_DURATION
                    )
                    .SetDelay(RandomizeDelayValue(0.33))
                    .SetId("NumberJump" + name);
            }
            return null;
        }

        public static Tweener ShakeAnimation(Transform transform)
        {
            return transform.DOShakePosition(
                PUZZLE_COMPLETED_ANIMATION_DURATION,
                new Vector3(0.1f, 0.1f, 0),
                10,
                0,
                false,
                true,
                ShakeRandomnessMode.Harmonic
            );
        }

        public static async Task<bool> WaitForAnimation(string animationId)
        {
            while (DOTween.TotalTweensById(animationId) > 0)
            {
                await Task.Delay(WAIT_FOR_ANIMATION_TO_FINISH_DELAY);
            }
            return true;
        }

        public static async Task SumIsIncorrect(Transform transform, string nodeName)
        {
            string animationId = "SumIsIncorrect" + nodeName;
            if (DOTween.TotalTweensById(animationId) == 0)
            {
                var sequence = DOTween.Sequence();
                sequence.Append(
                    transform.DOMoveX(
                        transform.position.x + 0.05f,
                        SUM_CORRECT_ANIMATION_DURATION / 3
                    )
                );
                sequence.Append(
                    transform.DOMoveX(
                        transform.position.x - 0.05f,
                        SUM_CORRECT_ANIMATION_DURATION / 3
                    )
                );
                sequence.Append(
                    transform.DOMoveX(transform.position.x, SUM_CORRECT_ANIMATION_DURATION / 3)
                );
                sequence.SetId(animationId).Play();

                await WaitForAnimation(animationId);
                DOTween.Kill(animationId, true);
            }
        }

        public static async void AnimatePuzzleSolved(Block[] blocks)
        {
            await WaitForAnimation("MoveNumberBack");

            var sequence = DOTween.Sequence();
            sequence.Append(blocks[3].AnimatePartialSumCorrect());
            sequence.Join(blocks[4].AnimatePuzzleCompleted());
            sequence.Join(blocks[5].AnimatePuzzleCompleted());
            sequence.AppendInterval(0.01f);
            sequence.Join(blocks[2].AnimatePuzzleCompleted());
            sequence.Join(blocks[6].AnimatePuzzleCompleted());
            sequence.Join(blocks[9].AnimatePuzzleCompleted());
            sequence.Join(blocks[12].AnimatePuzzleCompleted());
            sequence.AppendInterval(0.01f);
            sequence.Append(blocks[1].AnimatePuzzleCompleted());
            sequence.Join(blocks[7].AnimatePuzzleCompleted());
            sequence.Join(blocks[10].AnimatePuzzleCompleted());
            sequence.Join(blocks[13].AnimatePuzzleCompleted());
            sequence.AppendInterval(0.01f);
            sequence.Append(blocks[0].AnimatePuzzleCompleted());
            sequence.Join(blocks[8].AnimatePuzzleCompleted());
            sequence.Join(blocks[11].AnimatePuzzleCompleted());
            sequence.Join(blocks[14].AnimatePuzzleCompleted());
            sequence.AppendInterval(0.5f);
            sequence.SetLoops(-1);
            sequence.SetId("NumberJump");

            sequence.Play();
        }

        internal static void GameCompletedButtonSwitch(
            Transform inGamePanelTransform,
            Transform endGamePanelTransform
        )
        {
            ButtonUnload(inGamePanelTransform)
                .OnComplete(() =>
                {
                    inGamePanelTransform.gameObject.SetActive(false);
                    inGamePanelTransform.DOScale(1f, 0f);
                });
            ;
            ButtonLoad(endGamePanelTransform);
        }

        internal static void AnimateStartGameButtons(
            Transform inGamePanelTransform,
            Transform endGamePanelTransform
        )
        {
            if (endGamePanelTransform.gameObject.activeSelf)
            {
                ButtonUnload(endGamePanelTransform)
                    .OnComplete(() =>
                    {
                        endGamePanelTransform.gameObject.SetActive(false);
                        endGamePanelTransform.DOScale(1f, 0f);
                    });
                ;
            }
            ButtonLoad(inGamePanelTransform);
        }

        internal static void ButtonLoad(Transform transform)
        {
            if (DOTween.TotalTweensById(transform.name + "ButtonLoad") == 0)
            {
                transform
                    .DOScale(0.85f, .75f)
                    .From()
                    .SetEase(Ease.OutBounce)
                    .SetDelay(RandomizeDelayValue(0.1))
                    .SetId(transform.name + "ButtonLoad");
            }
        }

        internal static DG.Tweening.Core.TweenerCore<
            Vector3,
            Vector3,
            DG.Tweening.Plugins.Options.VectorOptions
        > ButtonUnload(Transform transform)
        {
            return transform
                .DOScale(0.01f, 0.5f)
                .SetEase(Ease.InBounce)
                .SetDelay(RandomizeDelayValue(0.1));
            ;
        }

        internal static void PopupLoad(Transform transform)
        {
            if (
                (DOTween.TotalTweensById(transform.name + "PopupLoad") == 0)
                && (DOTween.TotalTweensById(transform.name + "PopupUnload") == 0)
            )
            {
                transform.GetComponent<CanvasGroup>().alpha = 0;
                transform.gameObject.SetActive(true);
                transform.GetComponent<CanvasGroup>().DOFade(1f, 0.5f);
                transform
                    .DOScale(0.65f, .75f)
                    .From()
                    .SetEase(Ease.OutBack)
                    .SetId(transform.name + "PopupLoad");
            }
        }

        internal static void PopupUnload(Transform mainTransform, Transform panelTransform)
        {
            if (
                (DOTween.TotalTweensById(panelTransform.name + "PopupLoad") == 0)
                && (DOTween.TotalTweensById(panelTransform.name + "PopupUnload") == 0)
            )
            {
                mainTransform.GetComponent<CanvasGroup>().DOFade(0f, 0.5f);
                panelTransform
                    .DOScale(0.33f, 0.6f)
                    .SetEase(Ease.InBack)
                    .SetId(panelTransform.name + "PopupUnload")
                    .OnComplete(() =>
                    {
                        mainTransform.gameObject.SetActive(false);
                        panelTransform.localScale = new Vector3(1, 1, 1);
                    });
            }
        }

        internal static void NodeLoad(Transform transform)
        {
            transform
                .DOScale(.001f, .75f)
                .From()
                .SetEase(Ease.OutBounce)
                .OnStart(() =>
                {
                    Object
                        .FindObjectOfType<UIManager>()
                        .InteractionPerformed(Constants.AudioClip.NodeLoaded);
                })
                .SetDelay(RandomizeDelayValue(0.75));
        }

        internal static void StatsLoad(RectTransform transform)
        {
            transform
                .DOScale(.75f, .5f)
                .From()
                .SetEase(Ease.OutBounce)
                .SetDelay(RandomizeDelayValue(0.15));
        }

        static float RandomizeDelayValue(double delay)
        {
            return (float)(Random.value * delay);
        }

        public static async Task NodeClicked(Transform target)
        {
            target.gameObject.GetComponent<BoxCollider>().enabled = false;
            var sequence = DOTween.Sequence();
            sequence.Append(target.DOScale(NODE_SHRINK_ON_CLICK, BUTTON_SHRINK_ON_CLICK_TIME));
            sequence.Append(
                target.DOScale(NODE_SHRINK_REVERSE_ON_CLICK, BUTTON_SHRINK_REVERSE_ON_CLICK_TIME)
            );
            sequence.SetId("NodeClick" + target.name).Play();

            await WaitForAnimation("NodeClick" + target.name);
            target.gameObject.GetComponent<BoxCollider>().enabled = true;
        }

        internal static Sequence AnimateTimeReward(
            Transform timeRewardText,
            Transform timerGroup,
            Image clockIcon,
            TextMeshProUGUI timerText
        )
        {
            var sequence = DOTween.Sequence();
            sequence.Append(
                timeRewardText
                    .DOScale(1.25f, TIME_REWARD_TEXT_POP_UP_DURATION)
                    .SetEase(Ease.OutBack)
            );

            sequence.Append(
                timeRewardText
                    .GetComponent<RectTransform>()
                    .DOAnchorPos(new Vector2(175f, 281f), TIME_REWARD_TEXT_MOVE_DURATION)
                    .SetEase(Ease.InBack)
            );

            sequence.Append(
                timeRewardText.DOScale(0f, TIME_REWARD_TEXT_SHRINK_DURATION).SetEase(Ease.InBack)
            );
            sequence
                .Join(
                    clockIcon.DOColor(
                        ColourManager.Instance.SelectedPalette().Colours[Constants.COLOR_GREEN],
                        TIME_REWARD_ICON_TURN_GREEN_DURATION
                    )
                )
                .Join(
                    timerText.DOColor(
                        ColourManager.Instance.SelectedPalette().Colours[Constants.COLOR_GREEN],
                        TIME_REWARD_ICON_TURN_GREEN_DURATION
                    )
                );
            sequence.Join(
                timerGroup.DOShakeRotation(
                    TIME_REWARD_ICON_SHAKE_DURATION,
                    90f,
                    15,
                    0,
                    true,
                    ShakeRandomnessMode.Harmonic
                )
            );
            return sequence.Play();
        }

        internal static Sequence AnimateHintReward(Transform hintRewardText, Button hintButton)
        {
            hintRewardText.gameObject.SetActive(true);
            Vector3 originalPosition = hintRewardText.transform.localPosition;
            var sequence = DOTween.Sequence();

            sequence.Append(
                hintRewardText
                    .DOScale(1.25f, TIME_REWARD_TEXT_POP_UP_DURATION)
                    .SetEase(Ease.OutBack)
            );
            sequence.Append(
                hintRewardText
                    .GetComponent<RectTransform>()
                    .DOAnchorPos(new Vector2(-163f, 170f), TIME_REWARD_TEXT_MOVE_DURATION)
                    .SetEase(Ease.InBack)
            );

            sequence.Append(
                hintRewardText.DOScale(0f, TIME_REWARD_TEXT_SHRINK_DURATION).SetEase(Ease.InBack)
            );
            sequence.Join(
                hintButton.image.DOColor(
                    ColourManager.Instance.SelectedPalette().Colours[Constants.COLOR_GREEN],
                    TIME_REWARD_ICON_TURN_GREEN_DURATION
                )
            );
            sequence.Join(
                hintButton.image.rectTransform.DOShakeRotation(
                    TIME_REWARD_ICON_SHAKE_DURATION,
                    45f,
                    10,
                    0,
                    true,
                    ShakeRandomnessMode.Harmonic
                )
            );
            sequence.OnComplete(() =>
            {
                hintRewardText.gameObject.SetActive(false);

                hintButton.image.DOColor(
                    ColourManager.Instance.SelectedPalette().Colours[Constants.COLOR_NODE_NEUTRAL],
                    TIME_REWARD_ICON_REVERT_FROM_GREEN_DURATION
                );
                hintRewardText.transform.localScale = new Vector3(0f, 0f, 0f);
                hintRewardText.GetComponent<RectTransform>().DOAnchorPos(originalPosition, 0.1f);
            });

            return sequence.Play();
        }

        internal static void AnimateVisualAidSpace(
            Transform transform,
            RectTransform rectTransform,
            bool show
        )
        {
            float valueToSum = show ? -1.7f : 1.7f;
            float newScale = show ? 4f : .01f;
            var sequence = DOTween.Sequence();

            if (
                (show && transform.localPosition.x == 5.7f)
                || (!show && transform.localPosition.x != 5.7f)
            )
            {
                sequence.Append(rectTransform.DOScale(newScale, SUM_CORRECT_ANIMATION_DURATION));
                sequence.Join(
                    transform.DOLocalMove(
                        new Vector3(
                            transform.localPosition.x + valueToSum,
                            transform.localPosition.y + valueToSum,
                            transform.localPosition.z
                        ),
                        SUM_CORRECT_ANIMATION_DURATION
                    )
                );
                sequence.Play();
            }
        }

        public static void AnimateTitle(Transform transform)
        {
            transform
                .DOScale(new Vector3(1.25f, 1.25f, 1.25f), .5f)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    transform.DOScale(Vector3.one, .75f).SetUpdate(true);
                });
        }
    }
}
