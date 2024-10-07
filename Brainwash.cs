using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using UnityEngine;
using Verse;

namespace Brainwashing
{
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
        //Obligatory is a hyperbole; default probability is 9/10 for the Mk1 device.
        public bool Debug_TestObligatoryOutcomes = false;

        public float Debug_ProbabilityTweaker = 1f;
        public override void ExposeData()
        {
            Scribe_Values.Look(ref EnableSkillLoss, "EnableSkillLoss", true);
            Scribe_Values.Look(ref EnableCancer, "EnableCancer", true);
            Scribe_Values.Look(ref EnableFactionViolation, "EnableFactionViolation", true);
            Scribe_Values.Look(ref PropagandaCooldown, "PropagandaCooldown", true);
            Scribe_Values.Look(ref DisableAllSideEffects, "DisableAllSideEffects", false);
            Scribe_Values.Look(ref NoPropagandaCooldownRarity, "NoPropagandaCooldownRarity", 1f);

            Scribe_Values.Look(ref EnableDebugSettings, "EnableDebugSettings", false);
            Scribe_Values.Look(ref Debug_TestCatastrophicOutcomes, "Debug_TestCatastrophicOutcomes", false);
            Scribe_Values.Look(ref Debug_TestRareOutcomes, "Debug_TestRareOutcomes", false);
            Scribe_Values.Look(ref Debug_TestCommonOutcomes, "Debug_TestCatastrophicOutcomes", false);
            Scribe_Values.Look(ref Debug_TestObligatoryOutcomes, "Debug_TestCatastrophicOutcomes", false);
            Scribe_Values.Look(ref Debug_ProbabilityTweaker, "DebugProbabilityTweaker", 1f);

            base.ExposeData();
        }
    }
        public class Brainwashing_Mod : Mod
        {
            Brainwashing_ModSettings settings;
            public Brainwashing_Mod(ModContentPack content) : base(content)
            {
                this.settings = GetSettings<Brainwashing_ModSettings>();
            
        }
        private bool doReset;
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
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.Label("Surgery options", -1, "These options override debug options when conflicts arise.");
            listingStandard.Label("");
            listingStandard.CheckboxLabeled("Enable Skill Loss Chance", ref settings.EnableSkillLoss, "Enables the die to roll on skill loss after brainwashing");
            listingStandard.CheckboxLabeled("Enable Cancer Chance", ref settings.EnableCancer, "Enables the rare (1 in 100) chance that the Mk1 will cause extensive carcinoma within 172 days");
            listingStandard.CheckboxLabeled("Enable Faction Violation", ref settings.EnableFactionViolation, "Enables brainwashing surgery to count as a violation towards the brainwashed pawn's faction");
            listingStandard.CheckboxLabeled("Do Propaganda Effect Cooldown", ref settings.PropagandaCooldown, "Whether or not propaganda will work before previous mood change is lost. If disabled, propaganda will work on pawns at random.");
            settings.NoPropagandaCooldownRarity = listingStandard.SliderLabeled("No Propaganda Cooldown Rarity: " + (settings.NoPropagandaCooldownRarity * 33.3333f ).ToString() + "%", settings.NoPropagandaCooldownRarity, 0f, 3f, 0.5f, "Likelihood modifier for propaganda speakers. Higher = more likely");
            listingStandard.CheckboxLabeled("Disable All Side Effects", ref settings.DisableAllSideEffects, "Disables all side effects; if true, the above settings are overruled.");
            listingStandard.Label("");
            listingStandard.Label("Debugging settings");
            listingStandard.Label("");
            listingStandard.CheckboxLabeled("Enable Debug Settings", ref settings.EnableDebugSettings, "Toggles the mod's usage of debug settings below");
            listingStandard.CheckboxLabeled("Test Catastrophic Outcomes", ref settings.Debug_TestCatastrophicOutcomes, "Forces the extremely rare side effects to occur (Mk1 only)");
            listingStandard.CheckboxLabeled("Test Rare Outcomes", ref settings.Debug_TestRareOutcomes, "Forces some rare side effects to occur");
            listingStandard.CheckboxLabeled("Test Common Outcomes", ref settings.Debug_TestCommonOutcomes, "Forces some common side effects to occur");
            listingStandard.CheckboxLabeled("Test Obligatory Outcomes", ref settings.Debug_TestObligatoryOutcomes, "Forces some extremely common side effects to occur");
            settings.Debug_ProbabilityTweaker = listingStandard.SliderLabeled("Debug Probability Tweaker: " + settings.Debug_ProbabilityTweaker.ToString(), settings.Debug_ProbabilityTweaker, 0f, 5f, 0.5f, "Multiplies the surgery outcome chance's denominator, which is equal to the sqrt of the ingredient Mk number");
            listingStandard.Label("");
            doReset = listingStandard.ButtonText("Reset all settings to default");
            listingStandard.End();
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

        
    [DefOf]
    public class BrainwashingDefOf
    {
        static BrainwashingDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(BrainwashingDefOf));
        }
        public static RecipeDef RecipeBrainwash;

        public static ResearchProjectDef BrainwashingDevices;
        public static ResearchProjectDef ExperimentalBrainwashing;
        public static ResearchProjectDef SurgicalBrainwashing;

        public static ThingDef BrainwasherMki;
        public static ThingDef BrainwasherMkii;
        public static ThingDef BrainwasherMkiii;
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
    }
    public class Recipe_Brainwash : RecipeWorker
    {
        public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
        {
            if (billDoer != null)
			{
                Brainwash(pawn, ingredients, billDoer, out string messageAddendum);
                TaleRecorder.RecordTale(TaleDefOf.DidSurgery, new object[]
				{
					billDoer,
					pawn
				});

                string text = "";
                text += messageAddendum;
                string label = pawn.Name.ToStringShort + " brainwashed";
                Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NeutralEvent, pawn, null, null, null, null, 0, true);
				
			}
	}
        [MayRequireAnomaly]
        private HediffDef blissLobo = HediffDefOf.BlissLobotomy;
        [MayRequireAnomaly]
        private HediffDef psychicDead = HediffDefOf.PsychicallyDead;
        private void Brainwash(Pawn pawn, List<Thing> ingredients, Pawn billDoer, out string messageAddendum)
        {
            Dictionary<MentalStateDef, string> adverseReactions = new Dictionary<MentalStateDef, string>();
            List<HediffDef> adverseSideEffects = new List<HediffDef>();
            bool adverseSideEffectOccurred = false;
            float coef;
            switch (ingredients[0].def.defName)
            {
                case "BrainwasherMki":
                    coef = 1f;
                    break;
                case "BrainwasherMkii":
                    coef = (float)Math.Sqrt(2);
                    break;
                case "BrainwasherMkiii":
                    coef = (float)Math.Sqrt(3);
                    break;
                default:
                    coef = 1f;
                    break;
            }
            messageAddendum = "";
            string initMessage = pawn.Name.ToStringShort + " has been successfully brainwashed. ";
            bool violation = LoadedModManager.GetMod<Brainwashing_Mod>().GetSettings<Brainwashing_ModSettings>().EnableFactionViolation;
            if (pawn.Faction != Faction.OfPlayer && violation)
                {
                ReportViolation(pawn, billDoer, pawn.Faction, 20);
            }
            bool debugging = LoadedModManager.GetMod<Brainwashing_Mod>().GetSettings<Brainwashing_ModSettings>().EnableDebugSettings;

            bool catastrophic = LoadedModManager.GetMod<Brainwashing_Mod>().GetSettings<Brainwashing_ModSettings>().Debug_TestCatastrophicOutcomes;
            bool rare = LoadedModManager.GetMod<Brainwashing_Mod>().GetSettings<Brainwashing_ModSettings>().Debug_TestRareOutcomes;

            float probabilityTweak = LoadedModManager.GetMod<Brainwashing_Mod>().GetSettings<Brainwashing_ModSettings>().Debug_ProbabilityTweaker;
            bool isMki = coef == 1f;
            coef *= debugging ? probabilityTweak : 1; 


            bool doSideEffects = !LoadedModManager.GetMod<Brainwashing_Mod>().GetSettings<Brainwashing_ModSettings>().DisableAllSideEffects;
            bool skill = LoadedModManager.GetMod<Brainwashing_Mod>().GetSettings<Brainwashing_ModSettings>().EnableSkillLoss;

            if ( doSideEffects && (Rand.Chance(0.01f / coef) || (debugging && catastrophic)))//1 in 100 odds; full memory wipe—decreases to 1 in 100*sqrt(3) or 173 odds when using Mk3
            {
                adverseSideEffectOccurred = true;
                bool cancer = LoadedModManager.GetMod<Brainwashing_Mod>().GetSettings<Brainwashing_ModSettings>().EnableCancer;
                if (cancer && (isMki && (Rand.Bool || (debugging && catastrophic))))
                {
                    BrainwashingDefOf.CatastrophicBrainwash.hediffGivers[0].TryApply(pawn);
                    messageAddendum += billDoer.Name.ToStringShort + " messed up slightly and may have created a brain tumour. ";
                }
                    pawn.story.traits.allTraits.Clear();
                    pawn.Strip();
                    if (skill)
                    {
                        foreach (SkillRecord s in pawn.skills.skills)
                        {
                            s.levelInt = Rand.Chance(0.65f) ? 0 : s.levelInt;
                            bool rand = Rand.Chance(0.1f);
                            s.passion = rand ? (Passion)Rand.RangeInclusive(1, 2) : Passion.None;
                        }
                    }
                    pawn.relations.ClearAllRelations();

                BackstoryDef brainwashed = BrainwashingDefOf.Brainwashed69;
                    
                BackstoryDef brainwashed2 = BrainwashingDefOf.Brainwashed420;


                if (pawn.DevelopmentalStage == DevelopmentalStage.Adult) { pawn.story.Adulthood = brainwashed2; }
                pawn.story.Childhood = brainwashed;

                List<FactionRelation> list = new List<FactionRelation>();
                foreach (Faction faction2 in Find.FactionManager.AllFactionsListForReading)
                {
                    if (!faction2.def.PermanentlyHostileTo(FactionDefOf.Beggars))
                    {
                        list.Add(new FactionRelation
                        {
                            other = faction2,
                            kind = FactionRelationKind.Neutral
                        });
                    }
                }
                Faction faction = FactionGenerator.NewGeneratedFactionWithRelations(BrainwashingDefOf.Forgotten, list, false);
                faction.temporary = true;
                faction.Name = "Forgotten";
                Find.FactionManager.Add(faction);

                pawn.SetFaction(faction);
                pawn.guest.SetGuestStatus(Faction.OfPlayer, GuestStatus.Guest);
                pawn.guest.WaitInsteadOfEscapingFor(GenTicks.SecondsToTicks(30f));
                pawn.needs.mood.thoughts.memories.Memories.Clear();
                messageAddendum += pawn.Name.ToStringShort + " has incidentally forgotten where " + pawn.gender.GetPronoun() + " came from...and everything else, too. ";
                
            }
            bool common = LoadedModManager.GetMod<Brainwashing_Mod>().GetSettings<Brainwashing_ModSettings>().Debug_TestCommonOutcomes;
            if (doSideEffects && (Rand.Chance(0.02f / coef) || (debugging && rare)))//1 in 50 odds
            {
                adverseSideEffectOccurred = true;
                List<HediffDef> RareSideEffects = new List<HediffDef>();
                RareSideEffects.Add(HediffDefOf.Blindness);
                if(blissLobo != null) { RareSideEffects.Add(blissLobo); }
                RareSideEffects.Add(HediffDefOf.Blindness);
                RareSideEffects.Add(HediffDefOf.CatatonicBreakdown);
                RareSideEffects.Add(HediffDefOf.Dementia);

                if (!pawn.Downed && !pawn.InMentalState && pawn.health.CanCrawlOrMove)
                {
                    List<MentalStateDef> RareBreaks = new List<MentalStateDef>();
                    RareBreaks.Add(MentalStateDefOf.Berserk);
                    RareBreaks.Add(MentalStateDefOf.ManhunterPermanent);
                    RareBreaks.Add(MentalStateDefOf.Manhunter);
                    RareBreaks.Add(MentalStateDefOf.PanicFlee);
                    RareBreaks.Add(MentalStateDefOf.SocialFighting);

                    adverseReactions.Add(RareBreaks[Rand.Range(0, RareBreaks.Count)], "Rare brainwash reaction");
                }
                adverseSideEffects.Add(RareSideEffects[Rand.Range(0, RareSideEffects.Count)]);


            }
            bool oblige = LoadedModManager.GetMod<Brainwashing_Mod>().GetSettings<Brainwashing_ModSettings>().Debug_TestObligatoryOutcomes;
            if (doSideEffects && (Rand.Chance(0.5f / coef) && (debugging && common))) //1 in 2 odds
            {
                adverseSideEffectOccurred = true;
                List<HediffDef> CommonSideEffects = new List<HediffDef>();
                if (psychicDead != null) { CommonSideEffects.Add(psychicDead); }
                CommonSideEffects.Add(HediffDefOf.Heatstroke);

                if (!pawn.Downed && !pawn.InMentalState && pawn.health.CanCrawlOrMove)
                {
                    List<MentalStateDef> CommonBreaks = new List<MentalStateDef>();
                    CommonBreaks.Add(MentalStateDefOf.Wander_Psychotic);
                    CommonBreaks.Add(MentalStateDefOf.Wander_Sad);

                    adverseReactions.Add(CommonBreaks[Rand.Range(0, CommonBreaks.Count)], "Common brainwash reaction");
                }
                adverseSideEffects.Add(CommonSideEffects[Rand.Range(0, CommonSideEffects.Count)]);
            }
            if (doSideEffects && (Rand.Chance(0.9f / coef) && oblige)) //9 in 10 odds
            {
                adverseSideEffectOccurred = true;
                List<HediffDef> ObligatorySideEffects = new List<HediffDef>();
                ObligatorySideEffects.Add(HediffDefOf.MorningSickness);
                adverseSideEffects.Add(ObligatorySideEffects[Rand.Range(0, ObligatorySideEffects.Count)]);
                if (skill)
                {
                    string temp = "";
                    int skills = 0;
                    int lastCharLength = 0;
                    foreach (SkillRecord s in pawn.skills.skills)
                    {
                        bool loseSkill = Rand.Chance(0.1f);
                        s.levelInt = loseSkill ? 0 : s.levelInt;
                        if (loseSkill)
                        {
                            skills++;
                            temp += temp.Length > 1 ? ", " + s.def.skillLabel : s.def.skillLabel;
                            lastCharLength = s.def.skillLabel.Length;
                            s.passion = Passion.None;
                        }

                    }if(skills > 1)
                    {
                        temp.Remove(temp.Length - lastCharLength - 3, 1);
                        temp = skills > 2 ? temp.Insert(temp.Length - lastCharLength - 2, ", and") : temp.Insert(temp.Length - lastCharLength - 2, " and");
                        messageAddendum += pawn.Name.ToStringShort + " has forgotten everything " + pawn.gender.GetPronoun() + " knew about " + temp;
                    }
                }
            }
            if(pawn.Faction != Faction.OfPlayer && !pawn.guest.Recruitable) { 
                pawn.guest.Recruitable = true;
                messageAddendum += pawn.Name.ToStringShort + " is no longer unwaveringly loyal. ";
            }
            if (pawn.Ideo == Faction.OfPlayer.ideos.PrimaryIdeo) { 
                pawn.ideo.Reassure(Rand.Range(0.25f, 1f)); 
            } else {
                pawn.ideo.IdeoConversionAttempt(Rand.Range(0.25f, 1f), Faction.OfPlayer.ideos.PrimaryIdeo); 
            }
            if (adverseReactions.Count > 0)
            {
                Pawn other = adverseReactions.First().Key == MentalStateDefOf.SocialFighting ? billDoer : null;
                pawn.mindState.mentalStateHandler.TryStartMentalState(adverseReactions.First().Key, adverseReactions.First().Value, true, false, false, other);
            }
            if (adverseSideEffects.Count > 0)
            {
                pawn.health.AddHediff(adverseSideEffects[0]);
            }
            messageAddendum = !adverseSideEffectOccurred ? "Nothing seems to have gone wrong, " + pawn.Name.ToStringShort + " seems none the wiser." : messageAddendum == "" ? pawn.Name.ToStringShort + " is not too happy about it." : messageAddendum;
            messageAddendum = initMessage + messageAddendum;

        }

    }
    public class Building_PropagandaSpeaker : Building, IObservedThoughtGiver
    {
        public Ideo TargetIdeo
        {
            get { return base.Faction.ideos.PrimaryIdeo; }
        }
        public CompPowerTrader Power
        {
            get
            {
                return this.TryGetComp<CompPowerTrader>();
            }
        }
        public Thought_Memory GiveObservedThought(Pawn observer)
        {
            if (!Power.PowerOn) { return null; }
            List<Thought> thoughts = new List<Thought>();
            observer.needs.mood.thoughts.GetAllMoodThoughts(thoughts);
            bool affectPawn = Rand.Chance(0.333f * LoadedModManager.GetMod<Brainwashing_Mod>().GetSettings<Brainwashing_ModSettings>().NoPropagandaCooldownRarity);
            if (thoughts.Count > 0)
            {
                foreach (Thought t in thoughts) {
                    if (t.def == BrainwashingDefOf.SwayedByPropaganda || t.def == BrainwashingDefOf.HeardPropaganda)
                    {
                        return LoadedModManager.GetMod<Brainwashing_Mod>().GetSettings<Brainwashing_ModSettings>().PropagandaCooldown ? null : affectPawn ? DoPropaganda(observer) : null;
                    }
                }
            }
            return affectPawn ? DoPropaganda(observer) : null;
        }
        public Thought_Memory DoPropaganda(Pawn observer) { 
            bool isResistant = observer.story.traits.HasTrait(BrainwashingDefOf.PropagandaResistant);
            float coef = isResistant ? 1f / Mathf.Sqrt(Mathf.Clamp(observer.skills.GetSkill(SkillDefOf.Intellectual).levelInt, 1f, 16f)) : 1f;
            if (observer.Ideo != TargetIdeo)
            {
                observer.ideo.IdeoConversionAttempt(Rand.Range(0.001f, 0.10f * coef), TargetIdeo);
            }
            else { observer.ideo.Reassure(Rand.Range(0.005f, 0.02f * coef)); }
            Thought_MemoryObservation thought_MemoryObservation = !isResistant ? (Thought_MemoryObservation)ThoughtMaker.MakeThought(BrainwashingDefOf.SwayedByPropaganda) : (Thought_MemoryObservation)ThoughtMaker.MakeThought(BrainwashingDefOf.HeardPropaganda);
            thought_MemoryObservation.Target = this;
            return thought_MemoryObservation;
        }
        public HistoryEventDef GiveObservedHistoryEvent(Pawn observer)
        {
            return null;
        }
    }
    public class RitualBehaviorWorker_HoldMass : RitualBehaviorWorker
    {
        public RitualBehaviorWorker_HoldMass() { }
        public RitualBehaviorWorker_HoldMass(RitualBehaviorDef def) : base(def){}
        public override string CanStartRitualNow(TargetInfo target, Precept_Ritual ritual, Pawn selectedPawn = null, Dictionary<string, Pawn> forcedForRole = null)
        {
            Precept_Role precept_Role = ritual.ideo.RolesListForReading.FirstOrDefault((Precept_Role r) => r.def == PreceptDefOf.IdeoRole_Moralist);
            if (precept_Role == null)
            {
                return null;
            }
            if (precept_Role.ChosenPawnSingle() == null)
            {
                return "CantStartRitualRoleNotAssigned".Translate(precept_Role.LabelCap);
            }
            return base.CanStartRitualNow(target, ritual, selectedPawn, forcedForRole);
        }
    }
    public class RitualOutcomeEffectWorker_HoldMass : RitualOutcomeEffectWorker_FromQuality
    {
        public RitualOutcomeEffectWorker_HoldMass(RitualOutcomeEffectDef def) : base(def)
        {
        }
        public RitualOutcomeEffectWorker_HoldMass(){ }
        public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual){
            float quality = base.GetQuality(jobRitual, progress);
            RitualOutcomePossibility outcome = this.GetOutcome(quality, jobRitual);
            LookTargets lookTargets = jobRitual.selectedTarget;
            string text = null;
            if (jobRitual.Ritual != null)
            {
                this.ApplyAttachableOutcome(totalPresence, jobRitual, outcome, out text, ref lookTargets);
            }
            Pawn speaker = jobRitual.PawnWithRole("moralist");
            foreach(Pawn participant in totalPresence.Keys)
            {
                if (participant != speaker)
                {
                    float certaintyFactor = participant.Ideo == speaker.Ideo ? outcome.ideoCertaintyOffset : -outcome.ideoCertaintyOffset;
                    if (certaintyFactor < 0)
                    {
                        if (participant.Ideo == speaker.Ideo)
                        {
                            participant.ideo.OffsetCertainty(certaintyFactor);
                        }
                        else
                        {
                            certaintyFactor = Rand.Range(Mathf.Clamp01(-certaintyFactor - 0.1f), -certaintyFactor + 0.1f);
                            participant.ideo.IdeoConversionAttempt(certaintyFactor, speaker.Ideo);
                        }
                    }
                    if (certaintyFactor > 0) {
                        certaintyFactor = Rand.Range(Mathf.Clamp01(certaintyFactor - 0.1f), certaintyFactor + 0.1f);
                        participant.ideo.Reassure(certaintyFactor);
                    }
                    if (outcome.memory != null)
                    {
                        Thought_AttendedRitual newThought = (Thought_AttendedRitual)base.MakeMemory(participant, jobRitual, outcome.memory);
                        participant.needs.mood.thoughts.memories.TryGainMemory(newThought, null);
                    }
                }
            }
            TaggedString taggedString = outcome.description.Formatted(jobRitual.Ritual.Label).CapitalizeFirst();
            string text2 = this.def.OutcomeMoodBreakdown(outcome);
            if (!text2.NullOrEmpty())
            {
                taggedString += "\n\n" + text2;
            }
            if (text != null)
            {
                taggedString += "\n\n" + text;
            }
            taggedString += "\n\n" + this.OutcomeQualityBreakdownDesc(quality, progress, jobRitual);
            string text3;
            this.ApplyDevelopmentPoints(jobRitual.Ritual, outcome, out text3);
            if (text3 != null)
            {
                taggedString += "\n\n" + text3;
            }
            Find.LetterStack.ReceiveLetter("OutcomeLetterLabel".Translate(outcome.label.Named("OUTCOMELABEL"), jobRitual.Ritual.Label.Named("RITUALLABEL")), taggedString, outcome.Positive ? LetterDefOf.RitualOutcomePositive : LetterDefOf.RitualOutcomeNegative, lookTargets, null, null, null, null, 0, true);
        }

    }
}

