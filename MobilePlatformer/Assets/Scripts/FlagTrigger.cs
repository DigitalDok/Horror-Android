using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagTrigger : MonoBehaviour {

    public enum BehaviorPerIndex
    {
        TurnFlagBasedOnFlag,
        TurnFlagAnyway
    }

    public List<BehaviorPerIndex> Behaviors = new List<BehaviorPerIndex>();
    public List<string> FlagsToTurnOn = new List<string>();
    public List<string> FlagConditions = new List<string>();

    public void Flags()
    {
        for (int i = 0; i < Behaviors.Count; i++)
        {
            if(Behaviors[i] == BehaviorPerIndex.TurnFlagAnyway)
            {
                FlagManager.TheFlagManager.UpdateFlag(FlagsToTurnOn[i], true);
            }
            else if (Behaviors[i] == BehaviorPerIndex.TurnFlagBasedOnFlag)
            {
                if(FlagManager.TheFlagManager.CheckFlagStatus(FlagConditions[i]))
                {
                    FlagManager.TheFlagManager.UpdateFlag(FlagsToTurnOn[i], true);
                }
            }
            
        }
    }
}
