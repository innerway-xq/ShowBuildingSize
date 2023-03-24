namespace ShowBuildingSize
{
    using System;
    using ColossalFramework;
    using ColossalFramework.UI;
    using AlgernonCommons;
    using HarmonyLib;
    using UnityEngine;

    [HarmonyPatch(typeof(UIComponent), nameof(UIComponent.Invalidate))]
    class ButtonText
    {
        private static void HideButtonText(UIComponent component, UIMouseEventParameter eventParam)
        {
            if (component != null && component.GetType() == typeof(UIButton))
            {
                if (component.objectUserData != null && component.objectUserData.GetType() == typeof(BuildingInfo))
                {
                    
                    UISprite uisprite = component.components[0] as UISprite;
                    if (uisprite != null)
                    {
                        uisprite.enabled = true;
                    }

                    if (component.Find("SizeLabel") != null)
                    {
                        component.Find("SizeLabel").Hide();
                    }
                }

            }
                
        }

        private static void ShowButtonText(UIComponent component, UIMouseEventParameter eventParam)
        {

            if (component != null)
            {
                if (component.objectUserData != null && component.objectUserData.GetType() == typeof(BuildingInfo))
                {
                    UIButton uibutton = component as UIButton;
                    PrefabInfo prefabinfo = uibutton.objectUserData as PrefabInfo;
                    string size_str = $"{prefabinfo.GetLength()}x{prefabinfo.GetWidth()}";

                    UISprite uisprite = component.components[0] as UISprite;
                    if (uisprite != null)
                    {
                        uisprite.enabled = false;
                    }

                    if (component.Find("SizeLabel") == null)
                    {
                        UILabel sizelabel;
                        sizelabel = component.AddUIComponent<UILabel>();
                        sizelabel.isVisible = true;
                        sizelabel.name = "SizeLabel";
                        sizelabel.textScale = 1f;
                        sizelabel.relativePosition = new Vector3(0, component.height - 20);
                        sizelabel.text = size_str;
                    }
                    else
                    {
                        UILabel sizelabel = component.Find<UILabel>("SizeLabel");
                        sizelabel.Show();
                        sizelabel.text = size_str;
                    }
                    
                    
                }
            }

        }

        static void Postfix(UIComponent __instance)
        {
            if (__instance.GetType() == typeof(UIButton) && __instance.objectUserData != null)
            {

                if (__instance.objectUserData.GetType() == typeof(BuildingInfo))
                {
                    __instance.eventMouseLeave -= HideButtonText;
                    __instance.eventMouseLeave += HideButtonText;
                    __instance.eventMouseEnter -= ShowButtonText;
                    __instance.eventMouseEnter += ShowButtonText;
                }
                else
                {
                    if (__instance.Find("SizeLabel") != null)
                    {
                        __instance.Find("SizeLabel").Hide();
                    }
                }
            }
            
        }
    }

}