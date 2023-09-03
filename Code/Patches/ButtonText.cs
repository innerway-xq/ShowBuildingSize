namespace ShowBuildingSize
{
    using System;
    using ColossalFramework;
    using ColossalFramework.UI;
    using AlgernonCommons;
    using HarmonyLib;
    using UnityEngine;

    [HarmonyPatch(typeof(UIButton), nameof(UIButton.Invalidate))]
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
                    BuildingInfo buildinginfo = uibutton.objectUserData as BuildingInfo;
                    string size_str;
                    int length, width, main_length, main_width;
                    length = main_length = buildinginfo.GetLength();
                    width = main_width = buildinginfo.GetWidth();
                    if (buildinginfo.m_subBuildings != null && buildinginfo.m_subBuildings.Length > 0)
                    {
                        bool toolong = false, toowide = false;
                        for (int i = 0; i < buildinginfo.m_subBuildings.Length; i++)
                        {
                            BuildingInfo subbuildinginfo = buildinginfo.m_subBuildings[i].m_buildingInfo;
                            if (subbuildinginfo.GetLength() != main_length && subbuildinginfo.GetWidth() == main_width && buildinginfo.m_subBuildings[i].m_position.z < 0)
                            {
                                toolong = true;
                                break;
                            }
                            else if (subbuildinginfo.GetLength() == main_length && subbuildinginfo.GetWidth() != main_width && buildinginfo.m_subBuildings[i].m_position.x < 0)
                            {
                                toowide = true;
                                break;
                            }
                            else if (subbuildinginfo.GetLength() == main_length && subbuildinginfo.GetWidth() == main_width)
                            {
                                if (buildinginfo.m_subBuildings[i].m_position.z < 0)
                                {
                                    toolong = true;
                                    break;
                                }
                                else if (buildinginfo.m_subBuildings[i].m_position.x < 0)
                                {
                                    toowide = true;
                                }
                            }
                        }

                        for (int i = 0; i < buildinginfo.m_subBuildings.Length; i++)
                        {
                            BuildingInfo subbuildinginfo = buildinginfo.m_subBuildings[i].m_buildingInfo;
                            if ((buildinginfo.m_subBuildings[i].m_position.x * buildinginfo.m_subBuildings[i].m_position.z == 0) && (buildinginfo.m_subBuildings[i].m_position.x + buildinginfo.m_subBuildings[i].m_position.z != 0))
                            {
                                if (subbuildinginfo.GetLength() == main_length && toowide)
                                {
                                    width += subbuildinginfo.GetWidth();
                                }
                                else if (subbuildinginfo.GetWidth() == main_width && toolong)
                                {
                                    length += subbuildinginfo.GetLength();
                                }
                            }
                        }
                    }
                    if (ModSettings.SizeOrder)
                    {
                        size_str = $"{length}x{width}";
                    }
                    else
                    {
                        size_str = $"{width}x{length}";
                    }
                    

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
                        sizelabel.relativePosition = new Vector3(30, component.height - 20);
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

        static void Postfix(UIButton __instance)
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