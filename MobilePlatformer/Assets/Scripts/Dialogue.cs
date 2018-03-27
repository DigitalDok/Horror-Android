using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour {

    [Header("*CODES BELOW - Remember to add *** before each code!*")]
    [Header("ITEM_ADD:N")]
    [Header("ITEM_REMOVE:N")]
    [Header("GOLD:N")]
    [Header("QUEST_C:N (Quest Completed (ID))")]
    [Header("QUEST_N:N (New Quest (ID))")]
    [Header("FLAG_T_ID:N")]
    [Header("FLAG_T_NAME:N")]
    [Header("FLAG_F_ID:N")]
    [Header("FLAG_F_NAME:N")]
    [Header("WAIT:N (Waits for N Seconds)")]
    [Header("ANIMATE:")]
    [Header("MOVE:")]
    [Header("STOP")]
    [Header("DEACTIVATE")]
    [Header("FLIP")]
    internal Text DialogueText;

    int CurCharIndx;

    private float TextSpeed = 0.15f;
    private float CurTextInterval;

    internal Queue<string> TextsToDisplay = new Queue<string>();
    internal string TempText;

    public List<string> Branch_Text = new List<string>();
    public List<Color> Branch_Colors = new List<Color>();
    public int CurrentBranch;

    public int StartingDialogueBranch;

    private Inventory LeInventory;
    private GameObject LePlayer;

    public GameObject MyTarget;

    public enum ConditionType
    {
        HasItem,
        FlagOn
    }

    public List<int> IndicesThatWillCheckForConditions = new List<int>();
    public List<ConditionType> ConditionTypes = new List<ConditionType>();
    public List<string> ValuesToCheck = new List<string>();
    public List<int> BranchToHop = new List<int>();
    private float DialogueStarted;
    

    void Start ()
    {
        DialogueText = GameObject.Find("DialogueText").GetComponent<Text>();
        LeInventory = DialogueText.transform.parent.GetComponent<Inventory>();
        LePlayer = GameObject.Find("Player");

        
	}
	
	// Update is called once per frame
	void Update ()
    {
        DialogueStarted += Time.deltaTime;
        if (TextsToDisplay.Count > 0)
        {
            if (DialogueText.text != TempText)
            {
                // Typewriter Effect.
                CurTextInterval += Time.deltaTime;
                if (CurTextInterval > TextSpeed)
                {
                    DialogueText.text += TempText[CurCharIndx];
                    CurTextInterval = 0;
                    CurCharIndx++;
                }

                // This ensures we can skip dialogues.
                if (Input.GetButtonDown("Jump"))
                {
                    DialogueText.text = TempText;
                }
            }
            else
            {
                // Pressing Jump will move on to the next item. ONLY WORKS ON PC
                if (Input.GetButtonDown("Jump"))
                {
                    MovingOn();
                }
            }
        }

        if (Input.GetMouseButtonDown(0) && DialogueStarted > 0.5f)
        {
            if (TextsToDisplay.Count > 0)
            {
                if (DialogueText.text != TempText)
                {
                    DialogueText.text = TempText;
                }
                else
                {
                    MovingOn();
                }
            }
        }
    }

    internal void MovingOn()
    {
        TextsToDisplay.Dequeue();

        if (TextsToDisplay.Count > 0)
        {
            DialogueText.text = "";
            CurCharIndx = 0;
            TempText = TextsToDisplay.Peek();
            
        }

        MoveBranch();
    }

    public void MoveBranch()
    {
        if (Branch_Text.Count > CurrentBranch + 1)
        {
            // FORMAT: ***A:V
            // *** = INDICATOR
            // A: = Action to take 
            // V = Value of action

            // Examples:

            // ***ITEM_ADD:Potion
            // ***ANIMATE:CastSpell
            // ***GOLD:-1500

            if (Branch_Text[CurrentBranch + 1].Contains("***"))
            {
                int SecondsToWait = 0;
                if (Branch_Text[CurrentBranch + 1].Contains("ITEM_ADD:"))
                {
                    // Add New Item (NAME)
                    int INDX = Branch_Text[CurrentBranch + 1].IndexOf(":") + 1;
                    string ItemName = Branch_Text[CurrentBranch + 1].Remove(0, INDX);

                    LeInventory.AddToInventory(ItemName);
                }
                else if (Branch_Text[CurrentBranch + 1].Contains("ITEM_REMOVE:"))
                {
                    // Remove Item (NAME)
                    int INDX = Branch_Text[CurrentBranch + 1].IndexOf(":") + 1;
                    string ItemName = Branch_Text[CurrentBranch + 1].Remove(0, INDX);

                    LeInventory.RemoveFromInventory(ItemName);
                }
                else if (Branch_Text[CurrentBranch + 1].Contains("FLAG_T_NAME:"))
                {
                    // Make a flag TRUE in flag manager. (ID)

                    int INDX = Branch_Text[CurrentBranch + 1].IndexOf(":") + 1;
                    string FlagID = Branch_Text[CurrentBranch + 1].Remove(0, INDX);

                    FlagManager.TheFlagManager.ActualFlags[FlagManager.TheFlagManager.FlagName.IndexOf(FlagID)] = true;
                }
                else if (Branch_Text[CurrentBranch + 1].Contains("PLAY_SFX:"))
                {
                    int INDX = Branch_Text[CurrentBranch + 1].IndexOf(":") + 1;
                    string SFX_Clip = Branch_Text[CurrentBranch + 1].Remove(0, INDX);

                    SFX_Manager.SFX.PlaySFX(SFX_Clip);
                }
                else if (Branch_Text[CurrentBranch + 1].Contains("DEACTIVATE_TARGET"))
                {
                    MyTarget.SetActive(false);
                }
                else if (Branch_Text[CurrentBranch + 1].Contains("ACTIVATE_TARGET"))
                {
                    MyTarget.SetActive(true);
                }
                else if (Branch_Text[CurrentBranch + 1].Contains("FLAG_F_NAME:"))
                {
                    // Make a flag FALSE in flag manager. (ID)

                    int INDX = Branch_Text[CurrentBranch + 1].IndexOf(":") + 1;
                    string FlagID = Branch_Text[CurrentBranch + 1].Remove(0, INDX);

                    FlagManager.TheFlagManager.ActualFlags[FlagManager.TheFlagManager.FlagName.IndexOf(FlagID)] = false;
                }
                else if (Branch_Text[CurrentBranch + 1].Contains("FLIP"))
                {
                    // Make holder Flip
                    Vector3 LocalScale = transform.localScale;
                    LocalScale.x *= -1;
                    transform.localScale = LocalScale;
                }
                else if (Branch_Text[CurrentBranch + 1].Contains("DEACTIVATE"))
                {
                    // Make holder Dissappear
                    GetComponent<SpriteRenderer>().enabled = false;
                    GetComponent<BoxCollider2D>().enabled = false;
                }
                else if (Branch_Text[CurrentBranch + 1].Contains("STOP"))
                {
                    // Make holder Stop moving
                    GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                }
                else if (Branch_Text[CurrentBranch + 1].Contains("MOVE_X:"))
                {
                    // Make holder Move with X Velocity

                    int INDX = Branch_Text[CurrentBranch + 1].IndexOf(":") + 1;
                    string Speed = Branch_Text[CurrentBranch + 1].Remove(0, INDX);

                    GetComponent<Rigidbody2D>().velocity = new Vector2(System.Convert.ToInt16(Speed), 0);
                }
                else if (Branch_Text[CurrentBranch + 1].Contains("SCRIPTED:"))
                {
                    int INDX = Branch_Text[CurrentBranch + 1].IndexOf(":") + 1;
                    string Event = Branch_Text[CurrentBranch + 1].Remove(0, INDX);

                    if(Event == "DisableGirlGlass")
                    {
                        GameObject.Find("Girl_In_living_Room").SetActive(false);
                        GameObject.Find("SingleGlass").SetActive(false);
                    }
                    else
                    {

                    }
                }
                else if (Branch_Text[CurrentBranch + 1].Contains("CONDITION:"))
                {
                    // Make holder Move with X Velocity

                    int INDX = Branch_Text[CurrentBranch + 1].IndexOf(":") + 1;
                    string Condition = Branch_Text[CurrentBranch + 1].Remove(0, INDX);

                    int CondID = System.Convert.ToInt16(Condition);

                    switch (ConditionTypes[CondID])
                    {
                        case ConditionType.HasItem:
                            if (LeInventory.GetIndexByName(ValuesToCheck[CondID]) != -1)
                            {
                                CurrentBranch = BranchToHop[CondID];
                                DialogueText.text = "";
                                StartCoroutine(WaiterMoveBranch(SecondsToWait));
                                return;
                            }
                            break;
                        case ConditionType.FlagOn:
                            if (FlagManager.TheFlagManager.ActualFlags[FlagManager.TheFlagManager.FlagName.IndexOf(ValuesToCheck[CondID])])
                            {
                                CurrentBranch = BranchToHop[CondID];
                                DialogueText.text = "";
                                StartCoroutine(WaiterMoveBranch(SecondsToWait));
                                return;
                            }
                            break;
                    }
                }
                else if (Branch_Text[CurrentBranch + 1].Contains("ANIMATE:"))
                {
                    // Make holder Play a specific animation (Trigger to Enable on Mecanim)

                    int INDX = Branch_Text[CurrentBranch + 1].IndexOf(":") + 1;
                    string AnimTrig = Branch_Text[CurrentBranch + 1].Remove(0, INDX);

                    GetComponent<Animator>().SetTrigger(AnimTrig);
                }
                else if (Branch_Text[CurrentBranch + 1].Contains("WAIT:"))
                {
                    // Make holder Wait a couple of sec without doing anything

                    int INDX = Branch_Text[CurrentBranch + 1].IndexOf(":") + 1;
                    int SecString = System.Convert.ToInt16(Branch_Text[CurrentBranch + 1].Replace("***WAIT:", ""));

                    SecondsToWait = SecString;
                }
                else if (Branch_Text[CurrentBranch + 1].Contains("TERMINATE"))
                {
                    // Make holder Wait a couple of sec without doing anything

                    EndDialogue(0);
                }
                else if (Branch_Text[CurrentBranch + 1].Contains("UNLOCK"))
                {
                    GetComponent<NavigatorDoor>().IsLocked = false;
                }

                CurrentBranch++;
                DialogueText.text = "";
                StartCoroutine(WaiterMoveBranch(SecondsToWait));
            }
            else if (Branch_Text[CurrentBranch + 1].Contains("---"))
            {
                // Branch end.

                if(Branch_Text[CurrentBranch + 1].Contains("---C:"))
                {
                    int INDX = Branch_Text[CurrentBranch + 1].IndexOf(":") + 1;
                    string Condition = Branch_Text[CurrentBranch + 1].Remove(0, INDX);

                    int CondID = System.Convert.ToInt16(Condition);

                    switch (ConditionTypes[CondID])
                    {
                        case ConditionType.HasItem:
                            if (LeInventory.GetIndexByName(ValuesToCheck[CondID]) != -1)
                            {
                                CurrentBranch = BranchToHop[CondID];
                                DialogueText.text = "";
                                StartCoroutine(WaiterMoveBranch(0));
                                return;
                            }
                            else
                            {
                                EndDialogue();
                            }
                            break;
                        case ConditionType.FlagOn:
                            if (FlagManager.TheFlagManager.ActualFlags[FlagManager.TheFlagManager.FlagName.IndexOf(ValuesToCheck[CondID])])
                            {
                                print("here 2");
                                CurrentBranch = BranchToHop[CondID];
                                DialogueText.text = "";
                                StartCoroutine(WaiterMoveBranch(0));
                                return;
                            }
                            else
                            {
                                print("here");
                                EndDialogue();
                            }
                            break;
                    }
                }
                else
                {
                    EndDialogue();

                    if (Branch_Text[CurrentBranch + 1].Length > 3)
                    {
                        // New starting branch!
                        StartingDialogueBranch = System.Convert.ToInt16(Branch_Text[CurrentBranch + 1].Remove(0, 3));

                    }
                }
            }
            else
            {
                CurrentBranch++;

                try
                {
                    Color C = Branch_Colors[CurrentBranch];
                }
                catch (Exception)
                {
                    Debug.LogError("You forgot the color, you fucking retard.");
                }
                 
                Speak(Branch_Text[CurrentBranch], Branch_Colors[CurrentBranch]);

            }
        }
        else
        {
            EndDialogue();
        }

    }

    internal int GetIndexOfStartingBranch()
    {
        if(Branch_Text[StartingDialogueBranch].Contains("---"))
        {
            //uparxei hop?
            if(IndicesThatWillCheckForConditions.Contains(StartingDialogueBranch))
            {
                int Indx = IndicesThatWillCheckForConditions.IndexOf(StartingDialogueBranch);
                if (ConditionTypes[Indx] == ConditionType.FlagOn)
                {
                    if (FlagManager.TheFlagManager.CheckFlagStatus(ValuesToCheck[Indx]))
                    {
                        CurrentBranch = BranchToHop[Indx];
                        return CurrentBranch;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else if (ConditionTypes[Indx] == ConditionType.HasItem)
                {
                    if (LeInventory.GetIndexByName(ValuesToCheck[Indx]) != -1)
                    {
                        CurrentBranch = BranchToHop[Indx];
                        return CurrentBranch;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }
            return 1;
        
    }

    public void DialogueInit()
    {
        CurrentBranch = StartingDialogueBranch;
        DialogueStarted = 0;
        // Conditions to jump branch? 
        // CurrentBrach may change

        if(Branch_Text[StartingDialogueBranch].Contains("***"))
        {
            CurrentBranch--;
            MoveBranch();
        }
        else
        {
            CheckForConditions();

            if (Branch_Text[CurrentBranch].Contains("---")) CurrentBranch++;

            Speak(Branch_Text[CurrentBranch], Branch_Colors[CurrentBranch]);

        }
    }

    public void CheckForConditions()
    {
        if (IndicesThatWillCheckForConditions.Contains(CurrentBranch))
        {
            if (DoIFulfillAllConditions(CurrentBranch))
            {
                CurrentBranch = BranchToHop[IndicesThatWillCheckForConditions.IndexOf(CurrentBranch)];
            }
        }
    }

    private bool DoIFulfillAllConditions(int currentBranch)
    {
        bool Fulfillment = false;
        int ConditionCount = 0;
        foreach (var item in IndicesThatWillCheckForConditions)
        {
            if (item == currentBranch)
            {
                ConditionCount++;
            }
        }

        if (ConditionCount == 1)
        {
            // Just One Condition;
            for (int i = 0; i < IndicesThatWillCheckForConditions.Count; i++)
            {
                if (IndicesThatWillCheckForConditions[i] == currentBranch)
                {
                    Fulfillment = ConditionChecker(ConditionTypes[i], ValuesToCheck[i]);
                }
            }
            return Fulfillment;
        }
        else
        {
            // More than 1 Conditions;
            int FulfilledConditions = 0;
            for (int i = 0; i < IndicesThatWillCheckForConditions.Count; i++)
            {
                if (IndicesThatWillCheckForConditions[i] == currentBranch)
                {
                    if (ConditionChecker(ConditionTypes[i], ValuesToCheck[i]))
                    {
                        FulfilledConditions++;
                    }
                }
            }
            return (FulfilledConditions == ConditionCount);
        }
    }

    private bool ConditionChecker(ConditionType conditionType, string v)
    {
        bool Condition = false;

        switch (conditionType)
        {
            case ConditionType.HasItem:
                return (LeInventory.GetIndexByName(v) != -1);
            
            case ConditionType.FlagOn:
                return FlagManager.TheFlagManager.CheckFlagStatus(v);
            
            default:
                break;
        }

        return Condition;
    }

    public void Speak(string Text, Color col)
    {
        TextsToDisplay.Enqueue(Text);
        DialogueText.color = col;
        DialogueText.text = "";
        CurCharIndx = 0;
        TempText = TextsToDisplay.Peek();
    }

    IEnumerator WaiterMoveBranch(int Sec)
    {
        yield return new WaitForSeconds(Sec);

        MoveBranch();
    }

    private void EndDialogue(float Sec = 0.5f)
    {
        DialogueText.text = "";
        StartCoroutine(LePlayer.GetComponent<MoveScript>().Reenable(Sec));

        //SpeechBubble.SetActive(false);
        //CameraDirector.TheCameraDirector.ToggleLetterbox();
        //CameraDirector.TheCameraDirector.target = TheMainCharacter.gameObject;
        //TheMainCharacter.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
    


}
