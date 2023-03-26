namespace ShowBuildingSize
{
    using AlgernonCommons;
    using AlgernonCommons.Keybinding;
    using AlgernonCommons.Translation;
    using AlgernonCommons.UI;
    using ColossalFramework.UI;
    using ICities;
    public class OptionsPanel : UIPanel
    {
        public OptionsPanel()
        {
            autoLayout = true;
            autoLayoutDirection = LayoutDirection.Vertical;
            UIHelper helper = new UIHelper(this);

            UIHelperBase orderGroup = helper.AddGroup("Show size order");
            orderGroup.AddCheckbox("Change to \"length x width\" (Default \"width x length\")", ModSettings.SizeOrder, (isChecked) => ModSettings.SizeOrder = isChecked);
        }
    }
}
