using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace CandyCabinets.Components.Colour
{
    [CustomEditor(typeof(Colourise))]
    public class ColouriseEditor : Editor
    {
        List<int> selectedColours = new List<int>();
        string[] _colours;
        int _colourIndex = 0;

        void OnEnable()
        {
            Colourise colourise = (Colourise)target;
            if (!colourise.isActiveAndEnabled)
                return;

            colourise.SaveComponents(colourise.ScanForComponents());
        }

        public override void OnInspectorGUI()
        {
            Colourise colourise = (Colourise)target;
            //if (!colourise.isActiveAndEnabled) return;

            // If you want to see the ColouriseComponent in action, uncomment code below.
            //base.OnInspectorGUI();

            serializedObject.Update();

            List<Object> colourManagers = new List<Object>(
                GameObject.FindObjectsOfType(typeof(ColourManager))
            );

            if (colourManagers.Count <= 0)
            {
                Debug.LogError("Please add one Colour Manager.");
                return;
            }

            if (colourManagers.Count > 1)
            {
                Debug.LogWarning("More than one Colour Manager detected.");
            }

            ColourManager colourManager = (ColourManager)colourManagers[0];

            if (colourManager.Palettes.Count <= 0)
            {
                Debug.LogError("Please add a Palette to Colour Manager.");
                return;
            }

            UpdatePalette(colourManager, colourise);

            UpdateColour(colourManager, colourise);

            serializedObject.ApplyModifiedProperties();
        }

        void UpdatePalette(ColourManager colourManager, Colourise colourise)
        {
            Palette selected = colourManager.Palettes.Find(x => x.Selected);
            if (colourise.Palette != selected.Name)
            {
                colourise.Palette = selected.Name;
                EditorGUILayout.LabelField("Palette", colourise.Palette);
            }
            EditorGUILayout.LabelField("Palette", colourise.Palette);
        }

        void UpdateColour(ColourManager colourManager, Colourise colourise)
        { //, bool paletteChanged) {
            // Create an array of palette colour names to be used as Popup dropdown
            _colours = new string[colourManager.ColoursPerPalette];
            for (int i = 0; i < colourManager.ColoursPerPalette; i++)
            {
                _colours[i] = "Colour." + (i + 1);
            }

            // Detect changes to our palette colour listing and Add/Remove as necessary
            if (colourise.ColouriseComponents.Count > selectedColours.Count)
            {
                for (int i = selectedColours.Count; i < colourise.ColouriseComponents.Count; i++)
                {
                    selectedColours.Add(i);
                }
            }
            if (colourise.ColouriseComponents.Count < selectedColours.Count)
            {
                for (
                    int i = selectedColours.Count - 1;
                    i >= colourise.ColouriseComponents.Count;
                    i--
                )
                {
                    selectedColours.RemoveAt(i);
                }
            }

            // Find our selected colour (via name vs index), assign and update as necessary
            for (int c = 0; c < colourise.ColouriseComponents.Count; c++)
            {
                ColouriseComponent cc = colourise.ColouriseComponents[c];

                // Reset values
                if (string.IsNullOrEmpty(cc.PaletteColour))
                {
                    _colourIndex = 0;
                }
                else
                {
                    List<string> lcolours = new List<string>(_colours);
                    _colourIndex = lcolours.FindIndex(f => f == cc.PaletteColour);
                    if (_colourIndex < 0)
                        _colourIndex = _colours.Length - 1;
                }

                // Assign values
                _colourIndex = EditorGUILayout.Popup(cc.Name, _colourIndex, _colours);
                cc.PaletteColour = _colours[_colourIndex];

                // Detect changes
                cc.IsChanged = false;
                if (_colourIndex != selectedColours[c])
                {
                    selectedColours[c] = _colourIndex;
                    cc.ColourIndex = _colourIndex;
                    cc.IsChanged = true;
                    colourise.ReColour();
                }
            }
        }
    }
}
