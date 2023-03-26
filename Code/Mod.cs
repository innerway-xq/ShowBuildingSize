namespace ShowBuildingSize
{
    using AlgernonCommons.Patching;
    using AlgernonCommons.Translation;
    using ICities;


    public sealed class Mod : PatcherMod<OptionsPanel, PatcherBase>, IUserMod
    {
        public override string BaseName => "Show Building Size";

        public override string HarmonyID => "innerway-xq.csl.showbuildingsize";

        public string Description => "Show each building's size in the panel.";

        public override void SaveSettings() => ModSettings.Save();

        public override void LoadSettings() => ModSettings.Load();
    }
}