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

        public static void SendAnalyticsEvent(string difficulty)
        {
            HintUsed hintUsed = new HintUsed();
            hintUsed.difficulty = difficulty;
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

    public class Stats : Event
    {
        public Stats()
            : base("Stats") { }

        public string difficulty
        {
            set { SetParameter("difficulty", value); }
        }

        public static void SendAnalyticsEvent(string difficulty)
        {
            Stats statsEvent = new Stats();
            statsEvent.difficulty = difficulty;
            AnalyticsService.Instance.RecordEvent(statsEvent);
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

        public static void SendAnalyticsEvent(string type)
        {
            Control controlEvent = new Control();
            controlEvent.type = type;
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
        public Purchase()
            : base("Purchase") { }

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
            Purchase purchaseEvent = new Purchase();
            purchaseEvent.type = type;
            purchaseEvent.success = success;
            AnalyticsService.Instance.RecordEvent(purchaseEvent);
        }
    }
}
