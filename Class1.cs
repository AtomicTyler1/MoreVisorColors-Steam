using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace VisorColors
{
    [ContentWarningPlugin("com.atomic.morevisorcolors", "1.0.0", true)]
    internal class ContentPrioritization
    {
        static ContentPrioritization() { new GameObject().AddComponent<MoreColors>(); }
    }

    public class MoreColors : MonoBehaviour
    {
        private readonly Harmony harmony = new Harmony("com.atomic.morevisorcolors");

        public static Color HexToUnity(float r, float g, float b)
        {
            return new Color(r / 255f, g / 255f, b / 255f, 1f);
        }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
                harmony.PatchAll();
                UnityEngine.Debug.Log("MoreVisorColors loaded! Report any issues to @atomictyler. This is heavily made from Viviko ❤");
            }
            else
            {
                Destroy(this);
                return;
            }
        }

        public MoreColors()
        {
            List<Color> list = new List<Color>(47);
            list.Add(MoreColors.HexToUnity(0f, 0f, 0f));
            list.Add(MoreColors.HexToUnity(255f, 255f, 255f));
            list.Add(MoreColors.HexToUnity(255f, 0f, 0f));
            list.Add(MoreColors.HexToUnity(0f, 255f, 0f));
            list.Add(MoreColors.HexToUnity(0f, 0f, 255f));
            list.Add(MoreColors.HexToUnity(255f, 255f, 0f));
            list.Add(MoreColors.HexToUnity(0f, 255f, 255f));
            list.Add(MoreColors.HexToUnity(255f, 0f, 255f));
            list.Add(MoreColors.HexToUnity(192f, 192f, 192f));
            list.Add(MoreColors.HexToUnity(128f, 128f, 128f));
            list.Add(MoreColors.HexToUnity(128f, 0f, 0f));
            list.Add(MoreColors.HexToUnity(128f, 128f, 0f));
            list.Add(MoreColors.HexToUnity(0f, 128f, 0f));
            list.Add(MoreColors.HexToUnity(128f, 0f, 128f));
            list.Add(MoreColors.HexToUnity(0f, 128f, 128f));
            list.Add(MoreColors.HexToUnity(0f, 0f, 128f));
            list.Add(MoreColors.HexToUnity(255f, 99f, 71f));
            list.Add(MoreColors.HexToUnity(127f, 255f, 0f));
            list.Add(MoreColors.HexToUnity(184f, 134f, 11f));
            list.Add(MoreColors.HexToUnity(0f, 250f, 154f));
            list.Add(MoreColors.HexToUnity(47f, 79f, 79f));
            list.Add(MoreColors.HexToUnity(175f, 238f, 238f));
            list.Add(MoreColors.HexToUnity(106f, 90f, 205f));
            list.Add(MoreColors.HexToUnity(188f, 143f, 143f));
            list.Add(MoreColors.HexToUnity(230f, 230f, 250f));
            list.Add(MoreColors.HexToUnity(176f, 196f, 222f));
            list.Add(MoreColors.HexToUnity(139f, 69f, 19f));
            list.Add(MoreColors.HexToUnity(255f, 192f, 203f));
            list.Add(MoreColors.HexToUnity(136f, 176f, 75f));
            list.Add(MoreColors.HexToUnity(247f, 202f, 201f));
            list.Add(MoreColors.HexToUnity(146f, 168f, 209f));
            list.Add(MoreColors.HexToUnity(149f, 82f, 81f));
            list.Add(MoreColors.HexToUnity(181f, 101f, 167f));
            list.Add(MoreColors.HexToUnity(0f, 155f, 119f));
            list.Add(MoreColors.HexToUnity(221f, 65f, 36f));
            list.Add(MoreColors.HexToUnity(214f, 80f, 118f));
            list.Add(MoreColors.HexToUnity(68f, 184f, 172f));
            list.Add(MoreColors.HexToUnity(239f, 192f, 80f));
            list.Add(MoreColors.HexToUnity(91f, 94f, 166f));
            list.Add(MoreColors.HexToUnity(155f, 35f, 53f));
            list.Add(MoreColors.HexToUnity(223f, 207f, 190f));
            list.Add(MoreColors.HexToUnity(85f, 180f, 176f));
            list.Add(MoreColors.HexToUnity(225f, 93f, 68f));
            list.Add(MoreColors.HexToUnity(127f, 205f, 205f));
            list.Add(MoreColors.HexToUnity(188f, 36f, 60f));
            list.Add(MoreColors.HexToUnity(195f, 68f, 122f));
            list.Add(MoreColors.HexToUnity(152f, 180f, 212f));
            this.colors = list;
        }

        public PlayerCustomizer customizer;
        public static MoreColors Instance;
        public bool applyToSuit;
        public List<Color> colors;

        [HarmonyPatch(typeof(BedBoss))]
        public class BedBossPatch
        {
            private static HashSet<BedBoss> patchedBedBosses = new HashSet<BedBoss>();

            [HarmonyPatch("Awake")]
            [HarmonyPrefix]
            private static void AwakePatch(BedBoss __instance)
            {
                if (patchedBedBosses.Contains(__instance))
                {
                    UnityEngine.Debug.Log("BedBoss already patched for this instance.");
                    return;
                }

                List<Material> materials = __instance.materials.ToList();
                Material oldMaterial = __instance.materials[0];

                MoreColors.Instance.colors.ForEach(color =>
                {
                    Material material = UnityEngine.Object.Instantiate(oldMaterial);
                    material.color = color;
                    materials.Add(material);
                });

                __instance.materials = materials.ToArray();

                patchedBedBosses.Add(__instance);
                UnityEngine.Debug.Log("BedBoss successfully patched.");
            }
        }


        [HarmonyPatch(typeof(PlayerCustomizer))]
        public class PlayerCustomizerPatch
        {
            private static HashSet<PlayerCustomizer> patchedCustomizers = new HashSet<PlayerCustomizer>();

            [HarmonyPatch("Awake")]
            [HarmonyPrefix]
            private static void AwakePatch(PlayerCustomizer __instance)
            {
                if (patchedCustomizers.Contains(__instance))
                {
                    UnityEngine.Debug.Log("PlayerCustomizer already patched for this instance.");
                    return;
                }

                MoreColors.Instance.customizer = __instance;

                Transform transform = __instance.colorsRoot.transform;
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 30f, transform.localPosition.z);

                GridLayoutGroup component = __instance.colorsRoot.GetComponent<GridLayoutGroup>();
                component.cellSize = new Vector2(90f, 90f);
                component.spacing = new Vector2(2f, 2f);
                component.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                component.constraintCount = 6;
                component.childAlignment = TextAnchor.UpperCenter;

                Transform transform2 = transform.parent.Find("Back");
                transform2.localPosition = new Vector3(transform2.localPosition.x, transform2.localPosition.y - 50f, transform2.localPosition.z);

                Transform transform3 = transform.parent.Find("Apply");
                transform3.localPosition = new Vector3(transform3.localPosition.x, transform3.localPosition.y - 50f, transform3.localPosition.z);

                Transform transform4 = transform.parent.Find("Hats");
                transform4.localPosition = new Vector3(transform4.localPosition.x - 45f, transform4.localPosition.y + 100f, transform4.localPosition.z);

                List<Color> existingColors = __instance.colorsToPickFrom;
                MoreColors.Instance.colors.ForEach(color =>
                {
                    if (!existingColors.Contains(color))
                    {
                        existingColors.Add(color);
                    }
                });

                patchedCustomizers.Add(__instance);
                UnityEngine.Debug.Log("PlayerCustomizer successfully patched.");
            }
        }

        [HarmonyPatch(typeof(PlayerVisor))]
        public class PlayerVisorPatch
        {
            [HarmonyPatch("Update")]
            [HarmonyPrefix]
            private static void UpdatePatch(PlayerVisor __instance)
            {
                if (__instance == null)
                {
                    Debug.LogError("Debugging, Instance is null!");
                    return;
                }

                try
                {
                    Color color = MoreColors.Instance.customizer.colorsToPickFrom[__instance.visorColorIndex];

                    Material visorMaterial = __instance.visorRenderer.materials[2];
                    if (visorMaterial == null)
                    {
                        Debug.LogError("Debugging, M_PlayerVisor material is null!");
                        return;
                    }
                    visorMaterial.SetColor("_Color", color);

                }
                catch (Exception ex)
                {
                    Debug.LogError($"Debugging, Exception: {ex}");
                }
            }
        }

    }
}
