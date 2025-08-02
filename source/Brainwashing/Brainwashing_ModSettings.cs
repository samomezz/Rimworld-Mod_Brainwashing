using Verse;

namespace Brainwashing;

public class Brainwashing_ModSettings : ModSettings
{
    public bool EnableSkillLoss = true;

    public bool EnableCancer = true;

    public bool EnableFactionViolation = true;

    public bool DisableAllSideEffects = false;

    public float CancerRangeDays = 173f;

    public bool PropagandaCooldown = true;

    public float NoPropagandaCooldownRarity = 1f;

    public bool EnableDebugSettings = false;

    public bool Debug_TestCatastrophicOutcomes = false;

    public bool Debug_TestRareOutcomes = false;

    public bool Debug_TestCommonOutcomes = false;

    public bool Debug_TestObligatoryOutcomes = false;

    public float Debug_ProbabilityTweaker = 1f;

    public override void ExposeData()
    {
        Scribe_Values.Look(ref EnableSkillLoss, "EnableSkillLoss", defaultValue: true);
        Scribe_Values.Look(ref EnableCancer, "EnableCancer", defaultValue: true);
        Scribe_Values.Look(ref EnableFactionViolation, "EnableFactionViolation", defaultValue: true);
        Scribe_Values.Look(ref PropagandaCooldown, "PropagandaCooldown", defaultValue: true);
        Scribe_Values.Look(ref DisableAllSideEffects, "DisableAllSideEffects", defaultValue: false);
        Scribe_Values.Look(ref NoPropagandaCooldownRarity, "NoPropagandaCooldownRarity", 1f);
        Scribe_Values.Look(ref EnableDebugSettings, "EnableDebugSettings", defaultValue: false);
        Scribe_Values.Look(ref Debug_TestCatastrophicOutcomes, "Debug_TestCatastrophicOutcomes", defaultValue: false);
        Scribe_Values.Look(ref Debug_TestRareOutcomes, "Debug_TestRareOutcomes", defaultValue: false);
        Scribe_Values.Look(ref Debug_TestCommonOutcomes, "Debug_TestCatastrophicOutcomes", defaultValue: false);
        Scribe_Values.Look(ref Debug_TestObligatoryOutcomes, "Debug_TestCatastrophicOutcomes", defaultValue: false);
        Scribe_Values.Look(ref Debug_ProbabilityTweaker, "DebugProbabilityTweaker", 1f);
        base.ExposeData();
    }
}
