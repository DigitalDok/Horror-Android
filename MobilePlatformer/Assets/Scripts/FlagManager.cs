using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagManager : MonoBehaviour {

    public static FlagManager TheFlagManager;

    public List<string> FlagName;
    public List<bool> ActualFlags;

    private void Start()
    {
        if (!TheFlagManager)
            TheFlagManager = this;
    }

    public void ToggleFlag(int ID)
    {
        ActualFlags[ID] = !ActualFlags[ID];
    }

    public void ToggleFlag(string Name)
    {
        ActualFlags[FlagName.IndexOf(Name)] = !ActualFlags[FlagName.IndexOf(Name)];
    }

    public void UpdateFlag(int ID, bool NewFlag)
    {
        ActualFlags[ID] = NewFlag;
    }

    public void UpdateFlag(string Name, bool NewFlag)
    {

        if (FlagName.Contains(Name))
            ActualFlags[FlagName.IndexOf(Name)] = NewFlag;
        else
        {
            FlagName.Add(Name);
            ActualFlags[FlagName.IndexOf(Name)] = NewFlag;
        }


    }

    public bool CheckFlagStatus(int ID)
    {
        return ActualFlags[ID];
    }

    public bool CheckFlagStatus(string Name)
    {
        return ActualFlags[FlagName.IndexOf(Name)];
    }
}
