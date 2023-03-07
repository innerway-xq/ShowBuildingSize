namespace ShowBuildingSize
{
    using System;
    using ColossalFramework.UI;
    using HarmonyLib;
    using UnityEngine;


    [HarmonyPatch(typeof(GeneratedScrollPanel), nameof(GeneratedScrollPanel.OnTooltipEnter))]
    class HideButtonText
    {

        private static void hideButtonText(UIComponent button, UIMouseEventParameter p)
        {
            if(button != null)
                (button as UIButton).text = "";
        }

        private static void showButtonText(UIButton button)
        {
            if(button != null && button.objectUserData != null && button.objectUserData.GetType() == typeof(BuildingInfo))
            {
                PrefabInfo prefabinfo = button.objectUserData as PrefabInfo;
                string size_str = $"{prefabinfo.GetLength()}x{prefabinfo.GetWidth()}";
                button.text = size_str;
            }

        }

        static void Postfix(UIMouseEventParameter p)
        {
            if(p != null && p.source is UIButton)
            {
                UIButton uibutton = p.source as UIButton;
                uibutton.eventMouseLeave += hideButtonText;
                showButtonText(uibutton);
            }
      
        }
    }



}