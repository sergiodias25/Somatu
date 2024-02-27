using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CandyCabinets.Components.Colour {

	[CustomEditor(typeof(ColourManager))]
	public class ColourManagerEditor : Editor {

		private List<Palette> listPalettes = new List<Palette>();

		SerializedProperty persistent;
		SerializedProperty coloursPerPalette;
		SerializedProperty palettes;
		SerializedProperty paletteName;
		SerializedProperty paletteSelected;
		SerializedProperty paletteColours;

		void OnEnable() {
			persistent = serializedObject.FindProperty("Persistent");
			coloursPerPalette = serializedObject.FindProperty("ColoursPerPalette");
			palettes = serializedObject.FindProperty("Palettes");
		}

		public override void OnInspectorGUI() {
			
			// If you want to see the original layout, uncomment code below.
			//base.OnInspectorGUI();

			serializedObject.Update();

			EditorGUILayout.PropertyField (persistent);
			EditorGUILayout.PropertyField (coloursPerPalette);
			EditorGUILayout.PropertyField (palettes);
			ShowSerializedList(palettes, true);

			serializedObject.ApplyModifiedProperties();

			if (!GUI.changed) return;

			ColourManager colourManager = (ColourManager)target;

			if (colourManager.Palettes == null) return;

			if (colourManager.Palettes.Count != colourManager.OldPaletteCount) {
				UpdatePalettes(colourManager, colourManager.OldPaletteCount);
				colourManager.OldPaletteCount = colourManager.Palettes.Count;
			}

			if (colourManager.ColoursPerPalette != colourManager.OldColoursPerPalette) {
				UpdateColourPerPalette(colourManager, colourManager.OldColoursPerPalette);
				colourManager.OldColoursPerPalette = colourManager.ColoursPerPalette;				
			}

			if (colourManager.Palettes.Count <= 0) return;

			UpdateSelected(colourManager);

			UpdateColours(colourManager);
		}

		void ShowSerializedList(SerializedProperty serializedList, bool showSize = false, string setLabel = null) {
			EditorGUI.indentLevel += 1;
			if (serializedList.isExpanded) {
				if (showSize)
					EditorGUILayout.PropertyField(serializedList.FindPropertyRelative("Array.size"));
				for (int i = 0; i < serializedList.arraySize; i++) {
					SerializedProperty element = serializedList.GetArrayElementAtIndex(i);
					if (string.IsNullOrEmpty(setLabel)) {
						EditorGUILayout.PropertyField(element);
					} else {
						if (setLabel == "Colour")
							EditorGUILayout.PropertyField(element, 
								new GUIContent(setLabel + "." + (i + 1) + " | " + ColorUtility.ToHtmlStringRGB(element.colorValue) + ""),
								GUILayout.ExpandWidth(true));
						else 
							EditorGUILayout.PropertyField(element, new GUIContent(setLabel + "." + (i + 1)));
					}
					if (element.type == "Palette")
							ShowSerializedPalette(element);
				}
			}
			EditorGUI.indentLevel -= 1;
		}

		void ShowSerializedPalette(SerializedProperty serializedPalette) {
			paletteName = serializedPalette.FindPropertyRelative("Name");
			paletteSelected = serializedPalette.FindPropertyRelative("Selected");
			paletteColours = serializedPalette.FindPropertyRelative("Colours");

			EditorGUI.indentLevel += 1;
			if (serializedPalette.isExpanded) {
				EditorGUILayout.PropertyField (paletteName);
				EditorGUILayout.PropertyField (paletteSelected);
				EditorGUILayout.PropertyField (paletteColours);
				ShowSerializedList(paletteColours, setLabel: "Colour");
			}
			EditorGUI.indentLevel -= 1;
		}

		void UpdatePalettes(ColourManager colourManager, int oldPaletteCount = 0) {
			if (colourManager.Palettes.Count <= 0) return;

			for (int i = colourManager.Palettes.Count - 1; i >= 0; i--) {
				Palette p = colourManager.Palettes[i];
				if (string.IsNullOrEmpty(p.Name))
					p.Name = "Palette" + (i + 1);

				if (p.Colours.Count <= 0)
					AddColoursToPalette(colourManager, p, p.Colours.Count);
				
				if (i > 0) {
					if (colourManager.Palettes.FindAll(x => x.Name == p.Name).Count > 1) {
						p.Name = "Palette" + (i + 1);
					}
				}
			}
		}

		void UpdateColourPerPalette(ColourManager colourManager, int oldColourPerPalette) {
			if (colourManager.Palettes.Count <= 0) return;

			if (colourManager.ColoursPerPalette >= oldColourPerPalette) {
				colourManager.Palettes.ForEach(p => {
					AddColoursToPalette(colourManager, p, p.Colours.Count);
				});
				return;
			}

			colourManager.Palettes.ForEach(p => {
				for (int c = p.Colours.Count - 1; c >= colourManager.ColoursPerPalette; c--) {
					p.Colours.RemoveAt(c);
				} 
			});
		}

		void AddColoursToPalette(ColourManager colourManager, Palette p, int startIndex) {
			for (int c = startIndex; c < colourManager.ColoursPerPalette; c++) {
				p.Colours.Add(new Color(0.7020f, 0.7020f, 0.7020f, 1f)); // You can change the default colour of the palette here..
			}
		}

		void UpdateSelected(ColourManager colourManager) {
			List<Palette> selected = colourManager.Palettes.FindAll(x => x.Selected);
			if (selected.Count > 1) { 
				colourManager.Palettes[colourManager.SelectedPaletteIdx].Selected = false;
				colourManager.SelectedPaletteIdx = colourManager.Palettes.FindIndex(x => x.Selected);
				SetColour();
			}

			if (selected.Count <= 0) {
				colourManager.SelectedPaletteIdx = 0;
				colourManager.Palettes[0].Selected = true;
				SetColour();
			}
		}

		void UpdateColours(ColourManager colourManager) {
			if (listPalettes.Count > 0) {
				// Add new palettes
				if (listPalettes.Count < colourManager.Palettes.Count) {
					for (int i = listPalettes.Count - 1; i < colourManager.Palettes.Count - 1; i++){
						Palette p = colourManager.Palettes[i];
						listPalettes.Add(new Palette(p.Name, p.Selected, p.Colours));
					}
				}
				// Remove old ones
				if (listPalettes.Count > colourManager.Palettes.Count) {
					for (int i = listPalettes.Count - 1; i >= colourManager.Palettes.Count; i--) {
						listPalettes.RemoveAt(i);
					}
				}

				for (int i = 0; i < listPalettes.Count; i++) {
					Palette p = colourManager.Palettes[i];
					if (listPalettes[i].Colours != p.Colours) {
						SetColour();
					}
				}
			} else {
				colourManager.Palettes.ForEach(p => {
					listPalettes.Add(new Palette(p.Name, p.Selected, p.Colours));
				});
			}
		}

		void SetColour() {
			List<Object> colourises = new List<Object>(GameObject.FindObjectsOfType(typeof(Colourise)));
			if (colourises.Count > 0) {
				colourises.ForEach(c => {
					Colourise colourise = (Colourise)c;
					colourise.ColouriseComponents.ForEach(x => { x.IsChanged = true; });
					colourise.ReColour();
				});
			}
		}

	}
}

