using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedEventTrigger : MonoBehaviour {


    public enum ScriptedEvent
    {
        StartPlayingMusic,
        StopPlayingMusic
    }

    public ScriptedEvent ScriptedEventToRun;

    public string FlagCondition;

    public void Event()
    {
        if(FlagManager.TheFlagManager.ActualFlags[FlagManager.TheFlagManager.FlagName.IndexOf(FlagCondition)])
        {
            switch (ScriptedEventToRun)
            {
                case ScriptedEvent.StartPlayingMusic:

                    break;
                case ScriptedEvent.StopPlayingMusic:

                    break;
            }
        }
    }
}
