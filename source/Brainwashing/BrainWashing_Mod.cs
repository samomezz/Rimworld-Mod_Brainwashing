using UnityEngine;
using Verse;

namespace Brainwashing;

public class Brainwashing_Mod : Mod
{
    private Brainwashing_ModSettings settings;

    private bool doReset;

    public Brainwashing_Mod(ModContentPack content)
        : base(content)
    {
        settings = GetSettings<Brainwashing_ModSettings>();
    }

    private void DoReset()
    {
        settings.EnableSkillLoss = true;
        settings.EnableCancer = true;
        settings.EnableFactionViolation = true;
        settings.PropagandaCooldown = true;
        settings.DisableAllSideEffects = false;
        settings.NoPropagandaCooldownRarity = 1f;
        settings.EnableDebugSettings = false;
        settings.Debug_TestCatastrophicOutcomes = false;
        settings.Debug_TestRareOutcomes = false;
        settings.Debug_TestCommonOutcomes = false;
        settings.Debug_TestObligatoryOutcomes = false;
        settings.Debug_ProbabilityTweaker = 1f;
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        Listing_Standard listing_Standard = new Listing_Standard();
        listing_Standard.Begin(inRect);
        
        listing_Standard.Label(new TaggedString("Surgery options"), -1f, (string)"These options override debug options when conflicts arise.");
        listing_Standard.CheckboxLabeled("Enable Skill Loss Chance", ref settings.EnableSkillLoss, "Enables the die to roll on skill loss after brainwashing");
        listing_Standard.CheckboxLabeled("Enable Cancer Chance", ref settings.EnableCancer, "Enables the rare (1 in 100) chance that the Mk1 will cause extensive carcinoma within 172 days");
        listing_Standard.CheckboxLabeled("Enable Faction Violation", ref settings.EnableFactionViolation, "Enables brainwashing surgery to count as a violation towards the brainwashed pawn's faction");
        listing_Standard.CheckboxLabeled("Do Propaganda Effect Cooldown", ref settings.PropagandaCooldown, "Whether or not propaganda will work before previous mood change is lost. If disabled, propaganda will work on pawns at random.");
        settings.NoPropagandaCooldownRarity = listing_Standard.SliderLabeled("No Propaganda Cooldown Rarity: " + settings.NoPropagandaCooldownRarity * 33.3333f + "%", settings.NoPropagandaCooldownRarity, 0f, 3f, 0.5f, "Likelihood modifier for propaganda speakers. Higher = more likely");
        listing_Standard.CheckboxLabeled("Disable All Side Effects", ref settings.DisableAllSideEffects, "Disables all side effects; if true, the above settings are overruled.");
        listing_Standard.Label("Debugging settings");
        listing_Standard.CheckboxLabeled("Enable Debug Settings", ref settings.EnableDebugSettings, "Toggles the mod's usage of debug settings below");
        listing_Standard.CheckboxLabeled("Test Catastrophic Outcomes", ref settings.Debug_TestCatastrophicOutcomes, "Forces the extremely rare side effects to occur (Mk1 only)");
        listing_Standard.CheckboxLabeled("Test Rare Outcomes", ref settings.Debug_TestRareOutcomes, "Forces some rare side effects to occur");
        listing_Standard.CheckboxLabeled("Test Common Outcomes", ref settings.Debug_TestCommonOutcomes, "Forces some common side effects to occur");
        listing_Standard.CheckboxLabeled("Test Obligatory Outcomes", ref settings.Debug_TestObligatoryOutcomes, "Forces some extremely common side effects to occur");
        settings.Debug_ProbabilityTweaker = listing_Standard.SliderLabeled("Debug Probability Tweaker: " + settings.Debug_ProbabilityTweaker, settings.Debug_ProbabilityTweaker, 0f, 5f, 0.5f, "Multiplies the surgery outcome chance's denominator, which is equal to the sqrt of the ingredient Mk number");
        doReset = listing_Standard.ButtonText("Reset all settings to default");
        listing_Standard.End();
        base.DoSettingsWindowContents(inRect);
        if (doReset)
        {
            DoReset();
            doReset = false;
        }
    }

    public override string SettingsCategory()
    {
        return "Brainwashing Technologies";
    }
}
