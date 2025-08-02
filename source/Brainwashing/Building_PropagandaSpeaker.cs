using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace Brainwashing;

public class Building_PropagandaSpeaker : Building, IObservedThoughtGiver
{
    public Ideo TargetIdeo => base.Faction.ideos.PrimaryIdeo;

    public CompPowerTrader Power => this.TryGetComp<CompPowerTrader>();

    public Thought_Memory GiveObservedThought(Pawn observer)
    {
        if (!Power.PowerOn)
        {
            return null;
        }
        List<Thought> list = new List<Thought>();
        observer.needs.mood.thoughts.GetAllMoodThoughts(list);
        bool flag = Rand.Chance(0.333f * LoadedModManager.GetMod<Brainwashing_Mod>().GetSettings<Brainwashing_ModSettings>().NoPropagandaCooldownRarity);
        if (list.Count > 0)
        {
            foreach (Thought item in list)
            {
                if (item.def == BrainwashingDefOf.SwayedByPropaganda || item.def == BrainwashingDefOf.HeardPropaganda)
                {
                    return LoadedModManager.GetMod<Brainwashing_Mod>().GetSettings<Brainwashing_ModSettings>().PropagandaCooldown ? null : (flag ? DoPropaganda(observer) : null);
                }
            }
        }
        return flag ? DoPropaganda(observer) : null;
    }

    public Thought_Memory DoPropaganda(Pawn observer)
    {
        bool flag = observer.story.traits.HasTrait(BrainwashingDefOf.PropagandaResistant);
        float num = (flag ? (1f / Mathf.Sqrt(Mathf.Clamp(observer.skills.GetSkill(SkillDefOf.Intellectual).levelInt, 1f, 16f))) : 1f);
        if (observer.Ideo != TargetIdeo)
        {
            observer.ideo.IdeoConversionAttempt(Rand.Range(0.001f, 0.1f * num), TargetIdeo);
        }
        else
        {
            observer.ideo.Reassure(Rand.Range(0.005f, 0.02f * num));
        }
        Thought_MemoryObservation thought_MemoryObservation = ((!flag) ? ((Thought_MemoryObservation)ThoughtMaker.MakeThought(BrainwashingDefOf.SwayedByPropaganda)) : ((Thought_MemoryObservation)ThoughtMaker.MakeThought(BrainwashingDefOf.HeardPropaganda)));
        thought_MemoryObservation.Target = this;
        return thought_MemoryObservation;
    }

    public HistoryEventDef GiveObservedHistoryEvent(Pawn observer)
    {
        return null;
    }
}
