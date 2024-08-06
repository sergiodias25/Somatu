using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace CandyCabinets.Components.Colour
{
    public class Colourise : MonoBehaviour
    {
        public string Palette;

        [SerializeField]
        public List<ColouriseComponent> ColouriseComponents = new List<ColouriseComponent>();

        List<Component> components;
        ColourManager ColourManager;

        void Start()
        {
            List<Object> colourManagers = new List<Object>(
                GameObject.FindObjectsOfType(typeof(ColourManager))
            );

            ColourManager = (ColourManager)colourManagers[0];
            ColourManager.StartListening(ReColour);

            if (ColouriseComponents.Count <= 0)
                SaveComponents(ScanForComponents());
            ReColour();
        }

        public void ReColour()
        {
            List<Object> colourManagers = new List<Object>(
                GameObject.FindObjectsOfType(typeof(ColourManager))
            );

            ColourManager colourManager = (ColourManager)colourManagers[0];

            Palette palette = colourManager.Palettes.Find(x => x.Selected);
            if (palette == null)
            {
                return;
            }

            if (Palette != palette.Name)
            {
                Palette = palette.Name;
                ColouriseComponents.ForEach(c =>
                {
                    c.IsChanged = true;
                });
            }

            List<ColouriseComponent> updates = ColouriseComponents.FindAll(x => x.IsChanged);
            if (updates.Count <= 0)
                return;

            updates.ForEach(u =>
            {
                Component comp = gameObject.GetComponent(u.BaseName);
                System.Type t = comp.GetType();
                PropertyInfo[] propertyInfo = t.GetProperties();
                foreach (PropertyInfo info in propertyInfo)
                {
                    // Give special treatment to SpriteRenderer if running in WebGL
                    // NOTE: We can add other Renderers below if WebGL is having difficulty detecting them..
                    if (t.Name == "SpriteRenderer")
                    {
                        SpriteRenderer spriteRenderer = comp.GetComponent<SpriteRenderer>();
                        if (spriteRenderer == null)
                            break;
                        spriteRenderer.color = palette.Colours[u.ColourIndex];
                        break;
                    }

                    if (info.PropertyType.FullName == "UnityEngine.Color")
                    {
                        if (u.ColourIndex + 1 > palette.Colours.Count || u.ColourIndex < 0)
                            u.ColourIndex = palette.Colours.Count - 1;
                        info.SetValue(comp, palette.Colours[u.ColourIndex], null);
                        break;
                    }
                }
            });

            if (gameObject.name == "UndoButton")
            {
                FindObjectOfType<UIManager>().ToggleUndoButton();
            }

            if (gameObject.name == "HintButton")
            {
                FindObjectOfType<UIManager>().ToggleHintButton();
            }

            if (
                gameObject.name == "ButtonHintsUnlimited"
                || gameObject.name == "ButtonHints5"
                || gameObject.name == "ButtonUnlockLevels"
                || gameObject.name == "ButtonRemoveAds"
                || gameObject.name == "ButtonSunriseTheme"
                || gameObject.name == "ButtonSunsetTheme"
            )
            {
                var iapScript = FindObjectOfType<IAPScript>();
                if (iapScript != null)
                {
                    iapScript.LoadIAPStatus();
                }
            }
        }

        public List<Component> ScanForComponents()
        {
            components = new List<Component>(gameObject.GetComponents<Component>());
            return components;
        }

        public void SaveComponents(List<Component> comps)
        {
            List<ColouriseComponent> colouriseComponents = new List<ColouriseComponent>();

            for (int i = 0; i < comps.Count; i++)
            {
                Component c = comps[i];
                System.Type t = c.GetType();

                if (t.FullName == "CandyCabinets.Components.Colour.Colourise")
                    continue;

                PropertyInfo[] propertyInfo = t.GetProperties();
                foreach (PropertyInfo info in propertyInfo)
                {
                    // Give special treatment to SpriteRenderer if running in WebGL
                    // NOTE: We can add other Renderers below if WebGL is having difficulty detecting them..
                    if (
                        Application.platform == RuntimePlatform.WebGLPlayer
                        && t.Name == "SpriteRenderer"
                    )
                    {
                        SpriteRenderer spriteRenderer = c.GetComponent<SpriteRenderer>();
                        if (spriteRenderer == null)
                            break;
                        colouriseComponents.Add(
                            new ColouriseComponent(
                                t.Name + "." + info.PropertyType.Name,
                                t.FullName,
                                t.Name
                            )
                        );
                        break;
                    }

                    if (info.PropertyType.FullName == "UnityEngine.Color")
                    {
                        colouriseComponents.Add(
                            new ColouriseComponent(
                                t.Name + "." + info.PropertyType.Name,
                                t.FullName,
                                t.Name
                            )
                        );
                        break;
                    }
                }
            }

            // If we didn't find any component, reset our list
            if (colouriseComponents.Count <= 0)
            {
                ColouriseComponents = new List<ColouriseComponent>();
                return;
            }

            // Verify, add new and colourise
            colouriseComponents.ForEach(x =>
            {
                ColouriseComponent match = ColouriseComponents.Find(c => c.Name == x.Name);
                if (match == null)
                {
                    ColouriseComponents.Add(x);
                    ReColour();
                }
            });

            // Verify and delete old
            List<ColouriseComponent> toRemove = new List<ColouriseComponent>();
            ColouriseComponents.ForEach(x =>
            {
                ColouriseComponent match = colouriseComponents.Find(c => c.Name == x.Name);
                if (match == null)
                {
                    toRemove.Add(x);
                }
            });
            toRemove.ForEach(r =>
            {
                ColouriseComponents.Remove(r);
            });
        }

        void OnDestroy()
        {
            ColourManager.StopListening(ReColour);
        }
    }
}
