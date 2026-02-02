using Unity.Services.Analytics;

namespace Assets.Scripts.AnalyticsEvent
{
    public class ClassicPlayed : Event
    {
        public ClassicPlayed()
            : base("ClassicPlayed") { }

        public string difficulty
        {
            set { SetParameter("difficulty", value); }
        }

        public static void SendAnalyticsEvent(string difficulty)
        {
            ClassicPlayed classicPlayedEvent = new ClassicPlayed();
            classicPlayedEvent.difficulty = difficulty;
            AnalyticsService.Instance.RecordEvent(classicPlayedEvent);
        }
    }

    public class ChallengePlayed : Event
    {
        public ChallengePlayed()
            : base("ChallengePlayed") { }

        public static void SendAnalyticsEvent()
        {
            ChallengePlayed challengePlayedEvent = new ChallengePlayed();
            AnalyticsService.Instance.RecordEvent(challengePlayedEvent);
        }
    }

    public class ClassicContinued : Event
    {
        public ClassicContinued()
            : base("ClassicContinued") { }

        public string difficulty
        {
            set { SetParameter("difficulty", value); }
        }

        public static void SendAnalyticsEvent(string difficulty)
        {
            ClassicContinued classicContinuedEvent = new ClassicContinued();
            classicContinuedEvent.difficulty = difficulty;
            AnalyticsService.Instance.RecordEvent(classicContinuedEvent);
        }
    }

    public class GameFinished : Event
    {
        public GameFinished()
            : base("GameFinished") { }

        public string difficulty
        {
            set { SetParameter("difficulty", value); }
        }
        public int solves
        {
            set { SetParameter("solves", value); }
        }

        public double time
        {
            set { SetParameter("time", value); }
        }

        public static void SendAnalyticsEvent(string difficulty, double time, int solves = 1)
        {
            GameFinished gameFinishedEvent = new GameFinished();
            gameFinishedEvent.difficulty = difficulty;
            gameFinishedEvent.solves = solves;
            gameFinishedEvent.time = time;
            AnalyticsService.Instance.RecordEvent(gameFinishedEvent);
        }
    }

    public class HintUsed : Event
    {
        public HintUsed()
            : base("HintUsed") { }

        public string difficulty
        {
            set { SetParameter("difficulty", value); }
        }
        public bool success
        {
            set { SetParameter("success", value); }
        }

        public static void SendAnalyticsEvent(string difficulty, bool success)
        {
            HintUsed hintUsed = new HintUsed();
            hintUsed.difficulty = difficulty;
            hintUsed.success = success;
            AnalyticsService.Instance.RecordEvent(hintUsed);
        }
    }

    public class UndoUsed : Event
    {
        public UndoUsed()
            : base("UndoUsed") { }

        public string difficulty
        {
            set { SetParameter("difficulty", value); }
        }

        public static void SendAnalyticsEvent(string difficulty)
        {
            UndoUsed undoUsed = new UndoUsed();
            undoUsed.difficulty = difficulty;
            AnalyticsService.Instance.RecordEvent(undoUsed);
        }
    }

    public class ProfileScreen : Event
    {
        public ProfileScreen()
            : base("ProfileScreen") { }

        public string difficulty
        {
            set { SetParameter("difficulty", value); }
        }

        public static void SendAnalyticsEvent(string difficulty)
        {
            ProfileScreen profileEvent = new ProfileScreen();
            profileEvent.difficulty = difficulty;
            AnalyticsService.Instance.RecordEvent(profileEvent);
        }
    }

    public class Language : Event
    {
        public Language()
            : base("Language") { }

        public string language
        {
            set { SetParameter("language", value); }
        }

        public static void SendAnalyticsEvent(string language)
        {
            Language languageEvent = new Language();
            languageEvent.language = language;
            AnalyticsService.Instance.RecordEvent(languageEvent);
        }
    }

    public class Theme : Event
    {
        public Theme()
            : base("Theme") { }

        public string theme
        {
            set { SetParameter("theme", value); }
        }

        public static void SendAnalyticsEvent(string theme)
        {
            Theme themeEvent = new Theme();
            themeEvent.theme = theme;
            AnalyticsService.Instance.RecordEvent(themeEvent);
        }
    }

    public class Control : Event
    {
        public Control()
            : base("Control") { }

        public string type
        {
            set { SetParameter("type", value); }
        }

        public static void SendAnalyticsEvent(bool isDrag)
        {
            Control controlEvent = new Control();
            controlEvent.type = isDrag ? "drag" : "double-tap";
            AnalyticsService.Instance.RecordEvent(controlEvent);
        }
    }

    public class Vibration : Event
    {
        public Vibration()
            : base("Vibration") { }

        public bool enabled
        {
            set { SetParameter("enabled", value); }
        }

        public static void SendAnalyticsEvent(bool enabled)
        {
            Vibration vibrationEvent = new Vibration();
            vibrationEvent.enabled = enabled;
            AnalyticsService.Instance.RecordEvent(vibrationEvent);
        }
    }

    public class Music : Event
    {
        public Music()
            : base("Music") { }

        public bool enabled
        {
            set { SetParameter("enabled", value); }
        }

        public static void SendAnalyticsEvent(bool enabled)
        {
            Music musicEvent = new Music();
            musicEvent.enabled = enabled;
            AnalyticsService.Instance.RecordEvent(musicEvent);
        }
    }

    public class Sound : Event
    {
        public Sound()
            : base("Sound") { }

        public bool enabled
        {
            set { SetParameter("enabled", value); }
        }

        public static void SendAnalyticsEvent(bool enabled)
        {
            Sound soundEvent = new Sound();
            soundEvent.enabled = enabled;
            AnalyticsService.Instance.RecordEvent(soundEvent);
        }
    }

    public class VisualAid : Event
    {
        public VisualAid()
            : base("VisualAid") { }

        public bool enabled
        {
            set { SetParameter("enabled", value); }
        }

        public static void SendAnalyticsEvent(bool enabled)
        {
            VisualAid visualAidEvent = new VisualAid();
            visualAidEvent.enabled = enabled;
            AnalyticsService.Instance.RecordEvent(visualAidEvent);
        }
    }

    public class ChallengeShared : Event
    {
        public ChallengeShared()
            : base("ChallengeShared") { }

        public int solves
        {
            set { SetParameter("solves", value); }
        }

        public double time
        {
            set { SetParameter("time", value); }
        }

        public static void SendAnalyticsEvent(double time, int solves = 1)
        {
            ChallengeShared challengeSharedEvent = new ChallengeShared();
            challengeSharedEvent.solves = solves;
            challengeSharedEvent.time = time;
            AnalyticsService.Instance.RecordEvent(challengeSharedEvent);
        }
    }

    public class Purchase : Event
    {
        public const string SUCCESS = "success";
        public const string REPEATED = "repeated";
        public const string FAILED = "failed";

        public Purchase()
            : base("Purchase") { }

        public string product
        {
            set { SetParameter("product", value); }
        }
        public string type
        {
            set { SetParameter("type", value); }
        }

        public static void SendAnalyticsEvent(string product, string type)
        {
            Purchase purchaseEvent = new Purchase();
            purchaseEvent.product = product;
            purchaseEvent.type = type;
            AnalyticsService.Instance.RecordEvent(purchaseEvent);
        }
    }

    public class AdLoaded : Event
    {
        public const string REWARD = "reward";
        public const string INTERSTITIAL = "interstitial";
        public const string BANNER = "banner";

        public AdLoaded()
            : base("AdLoadded") { }

        public string type
        {
            set { SetParameter("type", value); }
        }

        public bool success
        {
            set { SetParameter("success", value); }
        }

        public static void SendAnalyticsEvent(string type, bool success)
        {
            AdLoaded adEvent = new AdLoaded();
            adEvent.type = type;
            adEvent.success = success;
            AnalyticsService.Instance.RecordEvent(adEvent);
        }
    }

    public class AdClick : Event
    {
        public const string REWARD = "reward";
        public const string INTERSTITIAL = "interstitial";
        public const string BANNER = "banner";

        public AdClick()
            : base("AdClick") { }

        public string type
        {
            set { SetParameter("type", value); }
        }

        public static void SendAnalyticsEvent(string type)
        {
            AdClick adEvent = new AdClick();
            adEvent.type = type;
            AnalyticsService.Instance.RecordEvent(adEvent);
        }
    }

    public class Feedback : Event
    {
        public Feedback()
            : base("Feedback") { }

        public static void SendAnalyticsEvent()
        {
            Feedback feedbackEvent = new Feedback();
            AnalyticsService.Instance.RecordEvent(feedbackEvent);
        }
    }

    public class Leaderboard : Event
    {
        public Leaderboard()
            : base("Leaderboard") { }

        public static void SendAnalyticsEvent()
        {
            Leaderboard eventInstance = new Leaderboard();
            AnalyticsService.Instance.RecordEvent(eventInstance);
        }
    }

    public class Achievements : Event
    {
        public const string MAIN_MENU = "main_menu";
        public const string PROFILE = "profile";

        public Achievements()
            : base("Achievements") { }

        public string type
        {
            set { SetParameter("type", value); }
        }

        public static void SendAnalyticsEvent(string type)
        {
            Achievements eventInstance = new Achievements();
            eventInstance.type = type;
            AnalyticsService.Instance.RecordEvent(eventInstance);
        }
    }

    public class ManualLogin : Event
    {
        public ManualLogin()
            : base("ManualLogin") { }

        public static void SendAnalyticsEvent()
        {
            ManualLogin eventInstance = new ManualLogin();
            AnalyticsService.Instance.RecordEvent(eventInstance);
        }
    }

    public class ChallengeFinished : Event
    {
        public ChallengeFinished()
            : base("ChallengeFinished") { }

        public int levelReached
        {
            set { SetParameter("level_reached", value); }
        }
        public double time
        {
            set { SetParameter("time", value); }
        }
        public string timeFormatted
        {
            set { SetParameter("time_formatted", value); }
        }

        public static void SendAnalyticsEvent(int levelReached, double time)
        {
            ChallengeFinished eventInstance = new ChallengeFinished();
            eventInstance.levelReached = levelReached;
            eventInstance.time = System.Math.Round(time, 2);
            eventInstance.timeFormatted = Timer.FormatTimeForText(System.Math.Round(time, 2));
            AnalyticsService.Instance.RecordEvent(eventInstance);
        }
    }

    public class ChallengeLevelCompleted : Event
    {
        public ChallengeLevelCompleted()
            : base("ChallengeLevelCompleted") { }

        public int levelReached
        {
            set { SetParameter("level", value); }
        }
        public string difficulty
        {
            set { SetParameter("difficulty", value); }
        }
        public int timeGained
        {
            set { SetParameter("time_gained", value); }
        }
        public int timeRemaining
        {
            set { SetParameter("time_remaining", value); }
        }

        public static void SendAnalyticsEvent(
            int levelReached,
            string difficulty,
            int timeGained,
            int timeRemaining
        )
        {
            ChallengeLevelCompleted eventInstance = new ChallengeLevelCompleted();
            eventInstance.levelReached = levelReached;
            eventInstance.difficulty = difficulty;
            eventInstance.timeGained = timeGained;
            eventInstance.timeRemaining = timeRemaining;
            AnalyticsService.Instance.RecordEvent(eventInstance);
        }
    }
}
