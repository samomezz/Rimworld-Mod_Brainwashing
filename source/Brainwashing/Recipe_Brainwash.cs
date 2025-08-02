using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace Brainwashing;

public class Recipe_Brainwash : RecipeWorker
{
    [MayRequireAnomaly]
    private HediffDef blissLobo = HediffDefOf.BlissLobotomy;

    [MayRequireAnomaly]
    private HediffDef psychicDead = HediffDefOf.PsychicallyDead;

    public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
    {
        if (billDoer != null)
        {
            Brainwash(pawn, ingredients, billDoer, out var messageAddendum);
            TaleRecorder.RecordTale(TaleDefOf.DidSurgery, billDoer, pawn);
            string text = "";
            text += messageAddendum;
            string text2 = pawn.Name.ToStringShort + " brainwashed";
            Find.LetterStack.ReceiveLetter(text2, text, LetterDefOf.NeutralEvent, pawn);
        }
    }

    private void Brainwash(Pawn pawn, List<Thing> ingredients, Pawn billDoer, out string messageAddendum)
    {
        Dictionary<MentalStateDef, string> dictionary = new Dictionary<MentalStateDef, string>();
        List<HediffDef> list = new List<HediffDef>();
        bool flag = false;
        float num = ingredients[0].def.defName switch
        {
            "BrainwasherMki" => 1f, 
            "BrainwasherMkii" => (float)Math.Sqrt(2.0), 
            "BrainwasherMkiii" => (float)Math.Sqrt(3.0), 
            "IdeologyChip" => 10f, 
            _ => 1f, 
        };
        messageAddendum = "";
        string text = pawn.Name.ToStringShort + " has been successfully brainwashed. ";
        bool enableFactionViolation = LoadedModManager.GetMod<Brainwashing_Mod>().GetSettings<Brainwashing_ModSettings>().EnableFactionViolation;
        if (pawn.Faction != Faction.OfPlayer && enableFactionViolation)
        {
            ReportViolation(pawn, billDoer, pawn.Faction, 20);
        }
        bool enableDebugSettings = LoadedModManager.GetMod<Brainwashing_Mod>().GetSettings<Brainwashing_ModSettings>().EnableDebugSettings;
        bool debug_TestCatastrophicOutcomes = LoadedModManager.GetMod<Brainwashing_Mod>().GetSettings<Brainwashing_ModSettings>().Debug_TestCatastrophicOutcomes;
        bool debug_TestRareOutcomes = LoadedModManager.GetMod<Brainwashing_Mod>().GetSettings<Brainwashing_ModSettings>().Debug_TestRareOutcomes;
        float debug_ProbabilityTweaker = LoadedModManager.GetMod<Brainwashing_Mod>().GetSettings<Brainwashing_ModSettings>().Debug_ProbabilityTweaker;
        bool flag2 = num == 1f;
        num *= (enableDebugSettings ? debug_ProbabilityTweaker : 1f);
        bool flag3 = !LoadedModManager.GetMod<Brainwashing_Mod>().GetSettings<Brainwashing_ModSettings>().DisableAllSideEffects;
        bool enableSkillLoss = LoadedModManager.GetMod<Brainwashing_Mod>().GetSettings<Brainwashing_ModSettings>().EnableSkillLoss;
        if (flag3 && (Rand.Chance(0.01f / num) || (enableDebugSettings && debug_TestCatastrophicOutcomes)))
        {
            flag = true;
            if (LoadedModManager.GetMod<Brainwashing_Mod>().GetSettings<Brainwashing_ModSettings>().EnableCancer && flag2 && (Rand.Bool || (enableDebugSettings && debug_TestCatastrophicOutcomes)))
            {
                BrainwashingDefOf.CatastrophicBrainwash.hediffGivers[0].TryApply(pawn);
                messageAddendum = messageAddendum + billDoer.Name.ToStringShort + " messed up slightly and may have created a brain tumour. ";
            }
            pawn.story.traits.allTraits.Clear();
            pawn.Strip();
            if (enableSkillLoss)
            {
                foreach (SkillRecord skill in pawn.skills.skills)
                {
                    skill.levelInt = ((!Rand.Chance(0.65f)) ? skill.levelInt : 0);
                    bool flag4 = Rand.Chance(0.1f);
                    skill.passion = (flag4 ? ((Passion)Rand.RangeInclusive(1, 2)) : Passion.None);
                }
            }
            pawn.relations.ClearAllRelations();
            BackstoryDef brainwashed = BrainwashingDefOf.Brainwashed69;
            BackstoryDef brainwashed2 = BrainwashingDefOf.Brainwashed420;
            if (pawn.DevelopmentalStage == DevelopmentalStage.Adult)
            {
                pawn.story.Adulthood = brainwashed2;
            }
            pawn.story.Childhood = brainwashed;
            List<FactionRelation> list2 = new List<FactionRelation>();
            foreach (Faction item in Find.FactionManager.AllFactionsListForReading)
            {
                if (!item.def.PermanentlyHostileTo(FactionDefOf.Beggars))
                {
                    list2.Add(new FactionRelation
                    {
                        other = item,
                        kind = FactionRelationKind.Neutral
                    });
                }
            }
            Faction faction = FactionGenerator.NewGeneratedFactionWithRelations(BrainwashingDefOf.Forgotten, list2);
            faction.temporary = true;
            faction.Name = "Forgotten";
            Find.FactionManager.Add(faction);
            pawn.SetFaction(faction);
            pawn.guest.SetGuestStatus(Faction.OfPlayer);
            pawn.guest.WaitInsteadOfEscapingFor(30f.SecondsToTicks());
            pawn.needs.mood.thoughts.memories.Memories.Clear();
            messageAddendum = messageAddendum + pawn.Name.ToStringShort + " has incidentally forgotten where " + pawn.gender.GetPronoun() + " came from...and everything else, too. ";
        }
        bool debug_TestCommonOutcomes = LoadedModManager.GetMod<Brainwashing_Mod>().GetSettings<Brainwashing_ModSettings>().Debug_TestCommonOutcomes;
        if (flag3 && (Rand.Chance(0.02f / num) || (enableDebugSettings && debug_TestRareOutcomes)))
        {
            flag = true;
            List<HediffDef> list3 = new List<HediffDef>();
            list3.Add(HediffDefOf.Blindness);
            if (blissLobo != null)
            {
                list3.Add(blissLobo);
            }
            list3.Add(HediffDefOf.Blindness);
            list3.Add(HediffDefOf.CatatonicBreakdown);
            list3.Add(HediffDefOf.Dementia);
            if (!pawn.Downed && !pawn.InMentalState && pawn.health.CanCrawlOrMove)
            {
                List<MentalStateDef> list4 = new List<MentalStateDef>();
                list4.Add(MentalStateDefOf.Berserk);
                list4.Add(MentalStateDefOf.ManhunterPermanent);
                list4.Add(MentalStateDefOf.Manhunter);
                list4.Add(MentalStateDefOf.PanicFlee);
                list4.Add(MentalStateDefOf.SocialFighting);
                dictionary.Add(list4[Rand.Range(0, list4.Count)], "Rare brainwash reaction");
            }
            list.Add(list3[Rand.Range(0, list3.Count)]);
        }
        bool debug_TestObligatoryOutcomes = LoadedModManager.GetMod<Brainwashing_Mod>().GetSettings<Brainwashing_ModSettings>().Debug_TestObligatoryOutcomes;
        if (flag3 && Rand.Chance(0.5f / num) && enableDebugSettings && debug_TestCommonOutcomes)
        {
            flag = true;
            List<HediffDef> list5 = new List<HediffDef>();
            if (psychicDead != null)
            {
                list5.Add(psychicDead);
            }
            list5.Add(HediffDefOf.Heatstroke);
            if (!pawn.Downed && !pawn.InMentalState && pawn.health.CanCrawlOrMove)
            {
                List<MentalStateDef> list6 = new List<MentalStateDef>();
                list6.Add(MentalStateDefOf.Wander_Psychotic);
                list6.Add(MentalStateDefOf.Wander_Sad);
                dictionary.Add(list6[Rand.Range(0, list6.Count)], "Common brainwash reaction");
            }
            list.Add(list5[Rand.Range(0, list5.Count)]);
        }
        if (flag3 && Rand.Chance(0.9f / num) && debug_TestObligatoryOutcomes)
        {
            flag = true;
            List<HediffDef> list7 = new List<HediffDef>();
            list7.Add(HediffDefOf.MorningSickness);
            list.Add(list7[Rand.Range(0, list7.Count)]);
            if (enableSkillLoss)
            {
                string text2 = "";
                int num2 = 0;
                int num3 = 0;
                foreach (SkillRecord skill2 in pawn.skills.skills)
                {
                    bool flag5 = Rand.Chance(0.1f);
                    skill2.levelInt = ((!flag5) ? skill2.levelInt : 0);
                    if (flag5)
                    {
                        num2++;
                        text2 += ((text2.Length > 1) ? (", " + skill2.def.skillLabel) : skill2.def.skillLabel);
                        num3 = skill2.def.skillLabel.Length;
                        skill2.passion = Passion.None;
                    }
                }
                if (num2 > 1)
                {
                    text2.Remove(text2.Length - num3 - 3, 1);
                    text2 = ((num2 > 2) ? text2.Insert(text2.Length - num3 - 2, ", and") : text2.Insert(text2.Length - num3 - 2, " and"));
                    messageAddendum = messageAddendum + pawn.Name.ToStringShort + " has forgotten everything " + pawn.gender.GetPronoun() + " knew about " + text2;
                }
            }
        }
        if (num > 9f)
        {
            pawn.SetFaction(Faction.OfPlayer);
            pawn.ideo.IdeoConversionAttempt(1, Faction.OfPlayer.ideos.PrimaryIdeo);
        }
        else
        {
            if (pawn.Faction != Faction.OfPlayer && !pawn.guest.Recruitable)
            {
                pawn.guest.Recruitable = true;
                messageAddendum = messageAddendum + pawn.Name.ToStringShort + " is no longer unwaveringly loyal. ";
            }
            if (pawn.Ideo == Faction.OfPlayer.ideos.PrimaryIdeo)
            {
                pawn.ideo.Reassure(Rand.Range(0.25f, 1f));
            }
            else
            {
                pawn.ideo.IdeoConversionAttempt(Rand.Range(0.25f, 1f), Faction.OfPlayer.ideos.PrimaryIdeo);
            }
        }
        if (dictionary.Count > 0)
            {
                Pawn otherPawn = ((dictionary.First().Key == MentalStateDefOf.SocialFighting) ? billDoer : null);
                pawn.mindState.mentalStateHandler.TryStartMentalState(dictionary.First().Key, dictionary.First().Value, forced: true, forceWake: false, causedByMood: false, otherPawn);
            }
        if (list.Count > 0)
        {
            pawn.health.AddHediff(list[0]);
        }
        messageAddendum = ((!flag) ? ("Nothing seems to have gone wrong, " + pawn.Name.ToStringShort + " seems none the wiser.") : ((messageAddendum == "") ? (pawn.Name.ToStringShort + " is not too happy about it.") : messageAddendum));
        messageAddendum = text + messageAddendum;
    }
}
