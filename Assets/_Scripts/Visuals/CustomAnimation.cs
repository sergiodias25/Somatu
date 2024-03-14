using System;
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

        public static Sequence ButtonClicked(Transform target)
        {
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

            return sequence;
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

        public static Sequence SumIsCorrect(Transform transform)
        {
            return transform
                .DOJump(
                    transform.position,
                    SUM_CORRECT_ANIMATION_JUMP_FORCE,
                    1,
                    SUM_CORRECT_ANIMATION_DURATION
                )
                .SetId("NumberJump");
            //.Join(SumIsIncorrect(transform))
            ;
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

        public static Sequence SumIsIncorrect(Transform transform)
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

            return sequence;
            //return null;
        }

        public static async void AnimatePuzzleSolved(Block[] blocks)
        {
            await WaitForAnimation("MoveNumberBack");

            var sequence = DOTween.Sequence();
            sequence.Append(blocks[3].AnimatePartialSumCorrect());
            sequence.Join(blocks[4].AnimatePuzzleCompleted());
            sequence.Join(blocks[5].AnimatePuzzleCompleted());
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

        internal static void AnimateEndGameButtonSwitch(
            Transform inGamePanelTransform,
            Transform endGamePanelTransform
        )
        {
            Vector3 originalPosition = inGamePanelTransform.position;
            var sequence = DOTween.Sequence();
            sequence
                .Append(inGamePanelTransform.DOMoveY(inGamePanelTransform.position.y - 0.1f, 0.5f))
                .Join(inGamePanelTransform.DOScale(0.01f, 0.5f))
                .Join(inGamePanelTransform.GetComponent<CanvasGroup>().DOFade(0.1f, 0.3f))
                .Join(endGamePanelTransform.GetComponent<CanvasGroup>().DOFade(0.1f, 0.4f).From())
                .Join(
                    endGamePanelTransform
                        .DOMoveY(endGamePanelTransform.position.y - 0.1f, 0.5f)
                        .From()
                )
                .Join(endGamePanelTransform.DOScale(0.1f, 0.5f).From());
            sequence.SetId("EndGameButtonSwitch");
            sequence
                .Play()
                .OnComplete(() =>
                {
                    inGamePanelTransform.gameObject.SetActive(false);
                    inGamePanelTransform.DOScale(1f, 0f);
                    inGamePanelTransform.position = originalPosition;
                    inGamePanelTransform.GetComponent<CanvasGroup>().DOFade(1f, 0f);
                });
        }

        internal static void AnimateStartGameButtons(Transform inGamePanelTransform)
        {
            var sequence = DOTween.Sequence();
            sequence
                .Append(inGamePanelTransform.GetComponent<CanvasGroup>().DOFade(0.1f, 0.4f).From())
                .Join(
                    inGamePanelTransform
                        .DOMoveY(inGamePanelTransform.position.y - 0.1f, 0.5f)
                        .From()
                )
                .Join(inGamePanelTransform.DOScale(0.1f, 0.5f).From());
            sequence.SetId("AnimateStartGameButtons");
            inGamePanelTransform.gameObject.SetActive(true);
            sequence.Play();
        }
    }
}
