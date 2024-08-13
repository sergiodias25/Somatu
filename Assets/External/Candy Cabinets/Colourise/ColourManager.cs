using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CandyCabinets.Components.Colour
{
    [System.Serializable]
    public class ColouriseComponent
    {
        public string Name;
        public string FullName;
        public string BaseName;
        public string PaletteColour;
        public int ColourIndex;
        public bool IsChanged = false;

        public ColouriseComponent(string _name, string _fullname, string _baseName)
        {
            Name = _name;
            FullName = _fullname;
            BaseName = _baseName;
            IsChanged = true;
        }
    }

    [System.Serializable]
    public class Palette
    {
        [Tooltip("Name of palette.")]
        [SerializeField]
        public string Name;

        [SerializeField]
        public bool Selected;

        [Tooltip("Colour collection.")]
        [SerializeField]
        public List<Color> Colours;

        public Palette(string _name, bool _selected, List<Color> _colours)
        {
            Name = _name;
            Selected = _selected;
            Colours = new List<Color>(_colours);
        }
    }

    public class ColourManager : MonoBehaviour
    {
        public static ColourManager Instance { get; private set; }

        [Tooltip("Don't destroy the Colour Manager when switching scenes.")]
        public bool Persistent = false;

        // If you want to add more colours to the palette, just increase this value.
        [Tooltip("Number of colours per palette. Minimum 3 and maximum of 12.")]
        [Range(3, 13)]
        [SerializeField]
        public int ColoursPerPalette = 13;
        public int OldColoursPerPalette = 13;

        [Tooltip("Palette collection.")]
        public List<Palette> Palettes;
        public int OldPaletteCount = 0;
        public int SelectedPaletteIdx = -1;

        private UnityEvent colourUpdateEvent;

        void Awake()
        {
            OnAwake();
        }

        public void OnAwake()
        {
            if (Instance != null)
            {
                if (Instance != this)
                    Destroy(gameObject);
                return;
            }

            Instance = this;

            if (Persistent)
                DontDestroyOnLoad(gameObject);

            colourUpdateEvent = new UnityEvent();
        }

        public void StartListening(UnityAction listener)
        {
            colourUpdateEvent.AddListener(listener);
        }

        public void StopListening(UnityAction listener)
        {
            colourUpdateEvent.RemoveListener(listener);
        }

        public void TriggerUpdate()
        {
            colourUpdateEvent.Invoke();
        }

        public int SelectedPaletteIndex()
        {
            if (Palettes.Count <= 0)
            {
                Debug.LogError("Please add a Palette to Colour Manager.");
                return -1;
            }
            int selected = Palettes.FindIndex(x => x.Selected);
            return selected;
        }

        public string SelectedPaletteName()
        {
            Palette selected = SelectedPalette();
            if (selected == null)
                return null;
            return selected.Name;
        }

        public Palette SelectedPalette()
        {
            return Palettes.Find(x => x.Selected);
        }

        public void SelectPalette(string paletteName)
        {
            if (Palettes.Count <= 0)
            {
                Debug.LogError("Please add a Palette to Colour Manager.");
                return;
            }
            for (int i = 0; i < Palettes.Count; i++)
            {
                Palette p = Palettes[i];
                if (p.Name == paletteName)
                {
                    p.Selected = true;
                    SelectedPaletteIdx = i;
                }
                else
                {
                    p.Selected = false;
                }
            }
            TriggerUpdate();
        }

        public void SelectPalette(int index)
        {
            if (Palettes.Count <= 0)
            {
                Debug.LogError("Please add a Palette to Colour Manager.");
                return;
            }
            for (int i = 0; i < Palettes.Count; i++)
            {
                if (i == index)
                {
                    Palettes[i].Selected = true;
                    SelectedPaletteIdx = i;
                }
                else
                {
                    Palettes[i].Selected = false;
                }
            }
            TriggerUpdate();
        }
    }
}
