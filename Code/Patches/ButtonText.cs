namespace ShowBuildingSize
{
    using ColossalFramework;
    using ColossalFramework.UI;
    using HarmonyLib;


    [HarmonyPatch(typeof(GeneratedScrollPanel), nameof(GeneratedScrollPanel.OnTooltipEnter))]
    class ButtonText
    {

        private static void HideButtonText(UIComponent button, UIMouseEventParameter p)
        {
            if (button != null)
            {
                (button as UIButton).text = "";
                UISprite uisprite = button.components[0] as UISprite;
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

        static void Postfix(UIMouseEventParameter p)
        {
            if (p != null && p.source is UIButton)
            {
                UIButton uibutton = p.source as UIButton;
                if (uibutton != null && uibutton.objectUserData != null && uibutton.objectUserData.GetType() == typeof(BuildingInfo))
                {
                    uibutton.eventMouseLeave += HideButtonText;
                    ShowButtonText(uibutton);
                    UISprite uisprite = uibutton.components[0] as UISprite;
                    if (uisprite != null)
                    {
                        uisprite.enabled = false;
                    }
                }
                    
            }
        }
    }



}