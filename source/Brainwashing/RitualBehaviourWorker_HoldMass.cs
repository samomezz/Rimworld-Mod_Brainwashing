using System.Collections.Generic;
using RimWorld;
using Verse;

namespace Brainwashing;

public class RitualBehaviorWorker_HoldMass : RitualBehaviorWorker
{
    public RitualBehaviorWorker_HoldMass()
    {
    }

    public RitualBehaviorWorker_HoldMass(RitualBehaviorDef def)
        : base(def)
    {
    }

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
