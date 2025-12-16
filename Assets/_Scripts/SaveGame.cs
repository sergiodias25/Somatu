using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.SaveGame
{
    public class SaveGame
    {
        public double TimeStamp;
        public Constants.Difficulty? UnlockedDifficulty = Constants.Difficulty.Easy;
        public int TimesBeatenCurrentDifficulty = 0;
        public int HintsAvailableClassic = 0;
        public int HintsAvailableChallenge = 0;
        public GameInProgress GameInProgressData;
        public Purchases PurchaseData;
        public Settings SettingsData;
        public ModeStats EasyStats;
        public ModeStats MediumStats;
        public ModeStats HardStats;
        public ModeStats ExtremeStats;
        public ModeStats ChallengeStats;
        public Onboarding Onboardings;

        public class Purchases
        {
            public bool UnlimitedHints = false;
            public bool RemovedAds = false;
            public bool SunriseTheme = false;
            public bool SunsetTheme = false;
        }

        public class Settings
        {
            public bool SoundEnabled = true;
            public bool MusicEnabled = true;
            public bool VibrationEnabled = true;
            public int SelectedThemeIndex = 0;
            public string LanguageSelected = "English";
            public bool LanguageChangedOnce = false;
            public bool ControlMethodDrag = false;
            public bool VisualAidEnabled = true;
        }

        public class Onboarding
        {
            public bool Welcome = false;
            public bool ClassicExplanation = false;
            public bool ClassicUndo = false;
            public bool ClassicHint = false;
            public bool ChallengeExplanation = false;
        }

        public class GameInProgress
        {
            public List<int> GameNumbers;
            public List<int> SolutionNumbers;
            public Constants.Difficulty? Difficulty;
            public double TimerValue;
            public Undo UndoData;

            public GameInProgress()
            {
                GameNumbers = new List<int>();
                SolutionNumbers = new List<int>();
                Difficulty = null;
                UndoData = new Undo();
            }

            public class Undo
            {
                public List<string> FirstNodes;
                public List<string> SecondNodes;

                public Undo()
                {
                    FirstNodes = new List<string>();
                    SecondNodes = new List<string>();
                }

                public void ClearMoveUndone()
                {
                    if (ThereIsDataToUndo())
                    {
                        FirstNodes.RemoveAt(FirstNodes.Count - 1);
                        SecondNodes.RemoveAt(SecondNodes.Count - 1);
                    }
                }

                public void ClearUndoData()
                {
                    if (ThereIsDataToUndo())
                    {
                        FirstNodes.Clear();
                        SecondNodes.Clear();
                    }
                }

                public bool ThereIsDataToUndo()
                {
                    return FirstNodes.Count > 0 && SecondNodes.Count > 0;
                }

                internal void StoreMoveToUndo(string firstNode, string secondNode)
                {
                    if (!ThereIsDataToUndo())
                    {
                        FirstNodes = new List<string>();
                        SecondNodes = new List<string>();
                    }
                    FirstNodes.Add(firstNode);
                    SecondNodes.Add(secondNode);
                }
            }
        }

        public class ModeStats
        {
            public int GamesPlayed;
            public int GamesCompleted;
            public double TimeBest;
            public double TimeAverage;
            public int SolveCountBest;
            public double SolveCountAverage;
            public int HintsUsed;

            public ModeStats()
            {
                GamesPlayed = 0;
                GamesCompleted = 0;
                TimeBest = 0;
                TimeAverage = 0;
                SolveCountBest = 0;
                SolveCountAverage = 0.0;
                HintsUsed = 0;
            }
        }

        public SaveGame()
        {
            GameInProgressData = new GameInProgress();
            PurchaseData = new Purchases();
            SettingsData = new Settings();
            EasyStats = new ModeStats();
            MediumStats = new ModeStats();
            HardStats = new ModeStats();
            ExtremeStats = new ModeStats();
            ChallengeStats = new ModeStats();
            Onboardings = new Onboarding();
            TimeStamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
        }

        public void IncrementTimesBeaten(Constants.Difficulty difficulty)
        {
            if (difficulty == UnlockedDifficulty)
            {
                TimesBeatenCurrentDifficulty++;
            }
            UnlockNextLevel(difficulty);
        }

        public bool IsDifficultyUnlocked(Constants.Difficulty difficulty)
        {
            if (difficulty == Constants.Difficulty.Challenge)
            {
                return Onboardings.ClassicExplanation;
            }
            return difficulty <= UnlockedDifficulty;
        }

        public void UnlockNextLevel(Constants.Difficulty difficulty)
        {
            if (
                difficulty < Constants.Difficulty.Extreme
                && difficulty >= UnlockedDifficulty
                && TimesBeatenCurrentDifficulty
                    >= Constants.GetNumberOfSolvesToUnlockNextDifficulty(difficulty)
            )
            {
                UnlockedDifficulty++;
                switch (UnlockedDifficulty)
                {
                    case Constants.Difficulty.Medium:
                        GoogleServices.UnlockAchievement(GPGSIds.achievement_math_is_easy);
                        break;
                    case Constants.Difficulty.Hard:
                        GoogleServices.UnlockAchievement(
                            GPGSIds.achievement_be_greater_than_average
                        );
                        break;
                    case Constants.Difficulty.Extreme:
                        GoogleServices.UnlockAchievement(GPGSIds.achievement_math_is_hard);
                        break;
                }
                TimesBeatenCurrentDifficulty = 0;
                UnityEngine.Object
                    .FindObjectOfType<UIManager>()
                    .ShowUnlockPopup((Constants.Difficulty)UnlockedDifficulty);
            }
        }

        public bool IsHalfwayThroughCurrentDifficulty(
            Constants.Difficulty calculatedDifficulty,
            Constants.Difficulty selectedDifficulty,
            int TimesBeatenDifficulty
        )
        {
            int valueToUse =
                selectedDifficulty == Constants.Difficulty.Challenge
                    ? TimesBeatenDifficulty
                    : TimesBeatenCurrentDifficulty;
            if (
                (selectedDifficulty < UnlockedDifficulty)
                || (
                    valueToUse
                    >= (Constants.GetNumberOfSolvesToUnlockNextDifficulty(calculatedDifficulty) / 2)
                )
            )
            {
                return true;
            }
            return false;
        }

        public void UnlockAllLevels()
        {
            UnlockedDifficulty = Constants.Difficulty.Challenge;
        }

        public void ClearInProgressSavedGame()
        {
            GameInProgressData.GameNumbers = new List<int>();
            GameInProgressData.SolutionNumbers = new List<int>();
            GameInProgressData.Difficulty = null;
            GameInProgressData.TimerValue = 0.0;
            GameInProgressData.UndoData = new GameInProgress.Undo();
        }

        public void UpdateInProgressSavedGame(
            GameObject generatedNodesObject,
            List<int> solutionNumbers,
            Constants.Difficulty difficulty,
            double timerValue
        )
        {
            for (int i = 0; i < generatedNodesObject.transform.childCount; i++)
            {
                Node node = generatedNodesObject.transform
                    .GetChild(i)
                    .gameObject.GetComponent<Node>();
                if (
                    GameInProgressData.GameNumbers.Count == 9
                    && GameInProgressData.GameNumbers[i] != node.GetBlockInNode().Value
                )
                {
                    GameInProgressData.GameNumbers[i] = node.GetBlockInNode().Value;
                }
                else if (GameInProgressData.GameNumbers.Count == i)
                {
                    GameInProgressData.GameNumbers.Add(node.GetBlockInNode().Value);
                }
                if (
                    GameInProgressData.SolutionNumbers.Count == 9
                    && GameInProgressData.SolutionNumbers[i] != solutionNumbers[i]
                )
                {
                    GameInProgressData.SolutionNumbers[i] = solutionNumbers[i];
                }
                else if (GameInProgressData.SolutionNumbers.Count == i)
                {
                    GameInProgressData.SolutionNumbers.Add(solutionNumbers[i]);
                }
            }

            GameInProgressData.Difficulty = difficulty;
            GameInProgressData.TimerValue = timerValue;
        }

        public void IncrementGamesPlayed(ModeStats mode)
        {
            mode.GamesPlayed++;
        }

        public void IncrementGamesCompleted(ModeStats mode)
        {
            mode.GamesCompleted++;
        }

        public void IncrementHintsUsedClassic(ModeStats mode)
        {
            mode.HintsUsed++;
            HintsAvailableClassic--;
        }

        public void IncrementHintsUsedChallenge(ModeStats mode)
        {
            mode.HintsUsed++;
            HintsAvailableChallenge--;
        }

        public void IncrementHintsAvailableClassic(int numberOfHintsToAdd)
        {
            HintsAvailableClassic += numberOfHintsToAdd;
        }

        public void IncrementHintsAvailableChallenge(int numberOfHintsToAdd)
        {
            HintsAvailableChallenge += numberOfHintsToAdd;
        }

        public void GrantUnlimitedHints()
        {
            PurchaseData.UnlimitedHints = true;
        }

        public void RemoveAds()
        {
            PurchaseData.RemovedAds = true;
        }

        public void EnableSunriseTheme()
        {
            PurchaseData.SunriseTheme = true;
        }

        public void EnableSunsetTheme()
        {
            PurchaseData.SunsetTheme = true;
            UpdateSaveGameTimeStamp();
        }

        public void ManageChallengeSolves(int solvesCount)
        {
            if (solvesCount > ChallengeStats.SolveCountBest)
            {
                ChallengeStats.SolveCountBest = solvesCount;
            }

            if (ChallengeStats.GamesCompleted == 0)
            {
                ChallengeStats.SolveCountAverage = solvesCount;
            }
            else
            {
                ChallengeStats.SolveCountAverage =
                    (
                        (ChallengeStats.GamesCompleted * ChallengeStats.SolveCountAverage)
                        + solvesCount
                    ) / (ChallengeStats.GamesCompleted + 1);
            }
        }

        public void ManageTime(double timeToComplete, ref ModeStats playerStats)
        {
            CheckFastestTime(timeToComplete, ref playerStats.TimeBest);
            CalculateAverageTime(
                timeToComplete,
                ref playerStats.TimeAverage,
                playerStats.GamesCompleted
            );
        }

        public void ManageChallengeTime(double timeToComplete, ref ModeStats playerStats)
        {
            CheckLongestTime(timeToComplete, ref playerStats.TimeBest);
            CalculateAverageTime(
                timeToComplete,
                ref playerStats.TimeAverage,
                playerStats.GamesCompleted
            );
        }

        private void CheckLongestTime(double timeToComplete, ref double previousBestTime)
        {
            if (previousBestTime == 0.0 || timeToComplete > previousBestTime)
            {
                previousBestTime = timeToComplete;
            }
        }

        private void CalculateAverageTime(
            double timeToComplete,
            ref double previousTimeAverage,
            int gamesCompleted
        )
        {
            if (gamesCompleted == 0)
            {
                previousTimeAverage = timeToComplete;
            }
            else
            {
                previousTimeAverage =
                    ((gamesCompleted * previousTimeAverage) + timeToComplete)
                    / (gamesCompleted + 1);
            }
        }

        private void CheckFastestTime(double timeToComplete, ref double previousTimeFastest)
        {
            if (previousTimeFastest == 0.0 || timeToComplete < previousTimeFastest)
            {
                previousTimeFastest = timeToComplete;
            }
        }

        private void UpdateSaveGameTimeStamp()
        {
            TimeStamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
        }

        public async void PersistData()
        {
            UpdateSaveGameTimeStamp();
            ISaveClient _client;
            /*
            try
            {
                _client = new CloudSaveClient();
                //throw new Exception("Cloud Save Error");
                await SaveService.SaveData(
                    _client,
                    Unity.Services.Authentication.AuthenticationService.Instance.PlayerId,
                    this
                );
                Debug.Log("Saved data in the Cloud");
            }
            catch (Exception)
            {
                Debug.LogError("Failed to save data in the Cloud");
            }
            */
            try
            {
                _client = new PlayerPrefClient();
                await SaveService.SaveData(
                    _client,
                    Unity.Services.Authentication.AuthenticationService.Instance.PlayerId,
                    this
                );
                //Debug.Log("Saved data in PlayerPrefs");
            }
            catch (Exception)
            {
                Debug.LogError("Failed to save data in PlayerPrefs");
            }
        }

        public static async Task<SaveGame> LoadSaveGame()
        {
            ISaveClient _client;
            Task<SaveGame> getCloudSaveGame = null;
            Task<SaveGame> getPlayerPrefsSaveGame = null;
            /*
                        try
                        {
                            _client = new CloudSaveClient();
                            //throw new Exception("Cloud Load Error");
                            getCloudSaveGame = SaveService.GetSavedGame<SaveGame>(
                                _client,
                                Unity.Services.Authentication.AuthenticationService.Instance.PlayerId
                            );
                        }
                        catch (Exception)
                        {
                            Debug.LogError("Failed to load data from the cloud");
                        }
            */
            try
            {
                _client = new PlayerPrefClient();
                getPlayerPrefsSaveGame = SaveService.GetSavedGame<SaveGame>(
                    _client,
                    Unity.Services.Authentication.AuthenticationService.Instance.PlayerId
                );
            }
            catch (Exception)
            {
                Debug.LogError("Failed to load data from PlayerPrefs");
            }
            if (getCloudSaveGame != null)
            {
                await getCloudSaveGame;
            }
            await getPlayerPrefsSaveGame;

            if (
                getCloudSaveGame != null
                && getCloudSaveGame.Result != null
                && !getCloudSaveGame.IsFaulted
            )
            {
                //Debug.Log("Loaded data from Cloud");
                return getCloudSaveGame.Result;
            }
            else if (!getPlayerPrefsSaveGame.IsFaulted && getPlayerPrefsSaveGame.Result != null)
            {
                //Debug.Log("Loaded data from PlayerPrefs");
                return getPlayerPrefsSaveGame.Result;
            }

            return new SaveGame();
        }
    }
}
