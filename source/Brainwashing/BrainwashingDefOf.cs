using RimWorld;
using Verse;

namespace Brainwashing;

[DefOf]
public class BrainwashingDefOf
{
    public static RecipeDef RecipeBrainwash;

    public static ResearchProjectDef BrainwashingDevices;

    public static ResearchProjectDef ExperimentalBrainwashing;

    public static ResearchProjectDef SurgicalBrainwashing;

    public static ThingDef BrainwasherMki;

    public static ThingDef BrainwasherMkii;

    public static ThingDef BrainwasherMkiii;
    public static ThingDef IdeologyChip;

    public static ThingDef PropagandaSpeaker;

    public static ThingCategoryDef Brainwashers;

    public static BackstoryDef Brainwashed420;

    public static BackstoryDef Brainwashed69;

    public static FactionDef Forgotten;

    public static HediffGiverSetDef CatastrophicBrainwash;

    public static TraitDef PropagandaResistant;

    public static ThoughtDef HeardPropaganda;

    public static ThoughtDef SwayedByPropaganda;

    public static ThoughtDef GreatMass;

    public static ThoughtDef DreadfulMass;

    public static ThoughtDef WeirdMass;

    public static ThoughtDef MasterfulMass;

    public static RitualBehaviorDef HoldMass_Behaviour;

    public static RitualPatternDef HoldMass_Pattern;

    public static RitualOutcomeEffectDef HoldMass_Outcome;

    public static PreceptDef HoldMass_Precept;

    public static AbilityDef HoldMass_Ability;

    static BrainwashingDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(BrainwashingDefOf));
    }
}
