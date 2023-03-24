namespace ShowBuildingSize
{
    using System;
    using ColossalFramework;
    using ColossalFramework.UI;
    using AlgernonCommons;
    using HarmonyLib;
    using UnityEngine;

    [HarmonyPatch(typeof(AssetSearchBar), "CopyAssetButtonProperties")]
    class SearchBarButtonText
    {
        private static void HideButtonText(UIComponent component, UIMouseEventParameter eventParam)
        {
            if (component != null && component.GetType() == typeof(UIButton))
            {
                if ((component.objectUserData as UIButton).objectUserData != null && (component.objectUserData as UIButton).objectUserData.GetType() == typeof(BuildingInfo))
                {

                    UISprite uisprite = component.components[0] as UISprite;
                    if (uisprite != null)
                    {
                        uisprite.enabled = true;
                    }

                    if (component.Find<UILabel>("SizeLabel"))
                    {
                        UILabel sizelabel = component.Find<UILabel>("SizeLabel");
                        sizelabel.isVisible = false;
                    }
                }

            }

        }

        private static void ShowButtonText(UIComponent component, UIMouseEventParameter eventParam)
        {

            if (component != null)
            {
                if ((component.objectUserData as UIButton).objectUserData != null && (component.objectUserData as UIButton).objectUserData.GetType() == typeof(BuildingInfo))
                {
                    UIButton uibutton = component as UIButton;
                    PrefabInfo prefabinfo = (component.objectUserData as UIButton).objectUserData as PrefabInfo;
                    string size_str = $"{prefabinfo.GetLength()}x{prefabinfo.GetWidth()}";

                    UISprite uisprite = component.components[0] as UISprite;
                    if (uisprite != null)
                    {
                        uisprite.enabled = false;
                    }

                    if (!component.Find<UILabel>("SizeLabel"))
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
                        sizelabel.isVisible = true;
                        sizelabel.text = size_str;
                    }


                }
            }

        }

        static void Postfix(UIButton source, UIButton target)
        {
            
            if (target.GetType() == typeof(UIButton) && (target.objectUserData as UIButton).objectUserData != null)
            {
                if ((target.objectUserData as UIButton).objectUserData.GetType() == typeof(BuildingInfo))
                {
                    target.eventMouseLeave -= HideButtonText;
                    target.eventMouseLeave += HideButtonText;
                    target.eventMouseEnter -= ShowButtonText;
                    target.eventMouseEnter += ShowButtonText;
                }
            }

        }
    }

}