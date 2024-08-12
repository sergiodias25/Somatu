using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.CustomAnimation
{
    public static class CustomAnimation
    {
        private static float BUTTON_SHRINK_ON_CLICK = 0.9f;
        private static float BUTTON_SHRINK_REVERSE_ON_CLICK = 1f;
        private static float BUTTON_SHRINK_ON_CLICK_TIME = 0.12f;
        private static float BUTTON_SHRINK_REVERSE_ON_CLICK_TIME = 0.06f;
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

        public static async Task ButtonClicked(Transform target)
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

            await WaitForAnimation("ButtonClick" + target.name);
            target.gameObject.GetComponent<Button>().enabled = true;
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

        public static async Task SumIsIncorrect(Transform transform)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(
                transform.DOMoveX(transform.position.x + 0.05f, SUM_CORRECT_ANIMATION_DURATION / 3)
            );
            sequence.Append(
                transform.DOMoveX(transform.position.x - 0.05f, SUM_CORRECT_ANIMATION_DURATION / 3)
            );
            sequence.Append(
                transform.DOMoveX(transform.position.x, SUM_CORRECT_ANIMATION_DURATION / 3)
            );
            sequence.SetId("SumIsIncorrect" + transform.name).Play();

            await WaitForAnimation("SumIsIncorrect" + transform.name);
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
            transform
                .DOScale(0.85f, .5f)
                .From()
                .SetEase(Ease.OutBounce)
                .SetDelay(RandomizeDelayValue(0.1));
            ;
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

        internal static void NodeLoad(Transform transform)
        {
            transform
                .DOScale(1f, 1f)
                .From()
                .SetEase(Ease.OutBounce)
                .SetDelay(RandomizeDelayValue(0.2));
            ;
            AudioManager audioManager = Object.FindObjectOfType<AudioManager>();
            audioManager.PlaySFX(audioManager.NodeLoaded);
        }

        static float RandomizeDelayValue(double delay)
        {
            return (float)(Random.value * delay);
        }
    }
}
