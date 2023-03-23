namespace ShowBuildingSize
{
    using System;
    using ColossalFramework;
    using ColossalFramework.UI;
    using HarmonyLib;


    [HarmonyPatch(typeof(GeneratedScrollPanel), nameof(GeneratedScrollPanel.OnTooltipEnter), new Type[] { typeof(UIButton), typeof(PrefabInfo)})]
    class ButtonText
    {

        private static void HideButtonText(UIComponent component, UIMouseEventParameter eventParam)
        {
            if (component != null)
            {
                (component as UIButton).text = "";
                UISprite uisprite = component.components[0] as UISprite;
                if (uisprite != null)
                {
                    uisprite.enabled = true;
                }
            }
                
        }

        private static void ShowButtonText(UIButton button)
        {
            PrefabInfo prefabinfo = button.objectUserData as PrefabInfo;
            string size_str = $"{prefabinfo.GetLength()}x{prefabinfo.GetWidth()}";
            button.text = size_str;
        }

        static void Postfix(UIButton source, PrefabInfo info)
        {
            if (source != null && info != null && info.GetType() == typeof(BuildingInfo))
            {
                source.eventMouseLeave += HideButtonText;
                ShowButtonText(source);
                UISprite uisprite = source.components[0] as UISprite;
                if (uisprite != null)
                {
                    uisprite.enabled = false;
                }
            }
                    
            
        }
    }



}