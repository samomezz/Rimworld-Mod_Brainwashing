using RimWorld;
using Unity.Mathematics;
using UnityEngine;
using Verse;

namespace Brainwashing;

public class RitualOutcomeEffectWorker_HoldMass : RitualOutcomeEffectWorker_FromQuality
{
    public RitualOutcomeEffectWorker_HoldMass(RitualOutcomeEffectDef def)
        : base(def)
    {
    }

    public RitualOutcomeEffectWorker_HoldMass()
    {
    }

    public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
    {
        float quality = GetQuality(jobRitual, progress);
        RitualOutcomePossibility outcome = GetOutcome(quality, jobRitual);
        LookTargets letterLookTargets = jobRitual.selectedTarget;
        string extraLetterText = null;
        if (jobRitual.Ritual != null)
        {
            ApplyAttachableOutcome(totalPresence, jobRitual, outcome, out extraLetterText, ref letterLookTargets);
        }
        Pawn pawn = jobRitual.PawnWithRole("moralist");
        foreach (Pawn key in totalPresence.Keys)
        {
            if (key == pawn)
            {
                continue;
            }
            float num = UnityEngine.Random.Range(outcome.ideoCertaintyOffset - 0.1f, outcome.ideoCertaintyOffset + 0.1f);
            if (num < 0f)
            {
                if (key.Ideo == pawn.Ideo)
                {
                    key.ideo.OffsetCertainty(num);
                }
                else
                {
                    key.ideo.Reassure(num);
                }
            }
                    
            if (num > 0f)
            {
                if (key.Ideo == pawn.Ideo)
                {
                    key.ideo.Reassure(num);
                }
                else
                {
                    key.ideo.IdeoConversionAttempt(num, pawn.Ideo);
                }
            }
            if (outcome.memory != null)
            {
                Thought_AttendedRitual newThought = (Thought_AttendedRitual)MakeMemory(key, jobRitual, outcome.memory);
                key.needs.mood.thoughts.memories.TryGainMemory(newThought);
            }
        }
        TaggedString text = outcome.description.Formatted(jobRitual.Ritual.Label).CapitalizeFirst();
        string text2 = def.OutcomeMoodBreakdown(outcome);
        if (!text2.NullOrEmpty())
        {
            text += "\n\n" + text2;
        }
        if (extraLetterText != null)
        {
            text += "\n\n" + extraLetterText;
        }
        text += "\n\n" + OutcomeQualityBreakdownDesc(quality, progress, jobRitual);
        ApplyDevelopmentPoints(jobRitual.Ritual, outcome, out var extraOutcomeDesc);
        if (extraOutcomeDesc != null)
        {
            text += "\n\n" + extraOutcomeDesc;
        }
        Find.LetterStack.ReceiveLetter("OutcomeLetterLabel".Translate(outcome.label.Named("OUTCOMELABEL"), jobRitual.Ritual.Label.Named("RITUALLABEL")), text, outcome.Positive ? LetterDefOf.RitualOutcomePositive : LetterDefOf.RitualOutcomeNegative, letterLookTargets);
    }
}
