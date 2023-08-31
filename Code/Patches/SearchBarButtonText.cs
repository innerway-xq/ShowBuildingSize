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

                    UILabel sizelabel;
                    if (component.Find("SizeLabel") != null)
                    {
                        sizelabel = component.Find<UILabel>("SizeLabel");
                    }
                    else if (component.Find("UILabel") != null && component.Find("UILabel").name == "SizeLabel")
                    {
                        sizelabel = component.Find<UILabel>("UILabel");
                    }
                    else
                    {
                        sizelabel = null;
                    }

                    if (sizelabel != null)
                    {
                        sizelabel.Hide();
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
                    UIButton uibutton = component.objectUserData as UIButton;
                    BuildingInfo buildinginfo = uibutton.objectUserData as BuildingInfo;
                    string size_str;
                    int length, width, main_length, main_width;
                    length = main_length = buildinginfo.GetLength();
                    width = main_width = buildinginfo.GetWidth();
                    if (buildinginfo.m_subBuildings.Length > 0)
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

                    UILabel sizelabel;
                    if (component.Find("SizeLabel") != null)
                    {
                        sizelabel = component.Find<UILabel>("SizeLabel");
                    }
                    else if (component.Find("UILabel") != null && component.Find("UILabel").name == "SizeLabel")
                    {
                        sizelabel = component.Find<UILabel>("UILabel");
                    }
                    else
                    {
                        sizelabel = null;
                    }

                    if (sizelabel == null)
                    {
                        
                        sizelabel = component.AddUIComponent<UILabel>();
                        sizelabel.isVisible = true;
                        sizelabel.name = "SizeLabel";
                        sizelabel.textScale = 1f;
                        sizelabel.relativePosition = new Vector3(30, component.height - 20);
                        sizelabel.text = size_str;
                    }
                    else
                    {
                        sizelabel.Show();
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