namespace ShowBuildingSize
{
    using ColossalFramework;
    using ColossalFramework.UI;
    using HarmonyLib;


    [HarmonyPatch(typeof(GeneratedScrollPanel), nameof(GeneratedScrollPanel.OnTooltipEnter))]
    class HideButtonText
    {

        private static void hideButtonText(UIComponent button, UIMouseEventParameter p)
        {
            if (button != null)
                (button as UIButton).text = "";
            PrefabInfo prefabinfo = button.objectUserData as PrefabInfo;
            UISprite uisprite = button.components[0] as UISprite;
            if (uisprite != null)
            {
                if (button.isEnabled)
                {
                    if (!prefabinfo.CanBeBuilt())
                    {
                        uisprite.spriteName = "ThumbnailBuildingAlreadyBuilt";
                        uisprite.enabled = true;
                    }
                    else
                    {
                        uisprite.spriteName = "ThumbnailBuildingNoMoney";
                        int constructionCost = prefabinfo.GetConstructionCost();
                        uisprite.enabled = (Singleton<EconomyManager>.instance.PeekResource(EconomyManager.Resource.Construction, constructionCost) != constructionCost);
                    }
                }
                else
                {
                    uisprite.enabled = false;
                }
            }
        }

        private static void showButtonText(UIButton button)
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
                    uibutton.eventMouseLeave += hideButtonText;
                    showButtonText(uibutton);
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