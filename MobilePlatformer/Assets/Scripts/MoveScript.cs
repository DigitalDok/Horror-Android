using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveScript : MonoBehaviour {

    public int MoveSpeed;
    public float JumpStrength;

    private bool PressingRight;
    private bool PressingLeft;

    private Rigidbody2D RigidBody;
    private Animator MyAnim;

    public bool IsFacingRight;

    private GameObject Inv_UI;
    private GameObject LeDialogText;
    private Dialogue MyDialogue;
    private NavigatorDoor MyNavigator;

    internal GameObject MainButtons;
    private bool PressingSprint;
    public float SprintMultiplier;

    internal bool IsHiding;
    private bool CanHide;

    public bool IsHoldingLantern;

    public string CurrentRoom;
    public GameObject LastDoor_IN;
    public GameObject LastDoor_OUT;

    void Start()
    {
        RigidBody = GetComponent<Rigidbody2D>();
        MyAnim = GetComponent<Animator>();
        MainButtons = GameObject.Find("MainPanel");
        Inv_UI = GameObject.Find("Canvas").transform.Find("InventoryPanel").gameObject;
        LeDialogText = GameObject.Find("DialogueText");

        if (IsHoldingLantern) Lantern_On();
        if (!IsHoldingLantern) Lantern_Off();
    }

    void FixedUpdate()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (MyDialogue)
        //    {
        //        if (MyDialogue.TextsToDisplay.Count > 0)
        //        {
        //            if (MyDialogue.DialogueText.text != MyDialogue.TempText)
        //            {
        //                MyDialogue.DialogueText.text = MyDialogue.TempText;
        //            }
        //            else
        //            {
        //                MyDialogue.MovingOn();
        //            }
        //        }
        //    }
        //}

        PressingSprint = (Input.GetKey(KeyCode.LeftShift));
        MyAnim.speed = (PressingSprint?2.5f:1);

        MyAnim.SetBool("Moving", (PressingLeft || PressingRight));
        if (PressingLeft || PressingRight)
        {
            if (PressingRight)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(MoveSpeed * Time.deltaTime * (PressingSprint?SprintMultiplier:1), RigidBody.velocity.y);
            }
            else if (PressingLeft)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveSpeed * Time.deltaTime * (PressingSprint ? SprintMultiplier : 1), RigidBody.velocity.y);
            }
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, RigidBody.velocity.y);
        }


    }

    public void Lantern_Off()
    {
        MyAnim.SetTrigger("Lantern OFF");
        IsHoldingLantern = false;
        transform.GetChild(0).GetComponent<Light>().enabled = IsHoldingLantern;
    }

    public void Lantern_On()
    {
        MyAnim.SetTrigger("Lantern ON");
        IsHoldingLantern = true;
        transform.GetChild(0).GetComponent<Light>().enabled = IsHoldingLantern;
    }


    public void Jump()
    {
        RigidBody.velocity = new Vector2(RigidBody.velocity.x, 0);
        RigidBody.AddForce(new Vector2(0, JumpStrength));
    }

    public void MoveLeft()
    {
        PressingLeft = true;
        UnHide();

        if (IsFacingRight) Flip();
    }

    private void UnHide()
    {
        if (IsHiding)
        {
            IsHiding = false;
            GetComponent<SpriteRenderer>().enabled = true;
            transform.GetChild(0).GetComponent<Light>().enabled = true;
        }
    }

    public void MoveRight()
    {
        PressingRight = true;
        UnHide();
        if (!IsFacingRight) Flip();
    }

    private void Flip()
    {
        Vector3 Scale = transform.localScale;
        Scale.x *= -1;
        transform.localScale = Scale;
        IsFacingRight = !IsFacingRight;
    }

    public void ReleaseRight()
    {
        PressingRight = false;
    }

    public void ReleaseLeft()
    {
        PressingLeft = false;
    }

    public void PressSprint()
    {
        PressingSprint = true;
    }

    public void ReleaseSprint()
    {
        PressingSprint = false;
    }

    public void Use()
    {
        if(CanHide)
        {
            IsHiding = true;
            GetComponent<SpriteRenderer>().enabled = false;
            transform.GetChild(0).GetComponent<Light>().enabled = false;
            return;
        }
        if(MyNavigator)
        {
            if(!MyNavigator.IsLocked)
            {
                MyNavigator.Navigate();
                return;
            }
        }

        if (MyDialogue)
        {
            MyDialogue.DialogueInit();
            MainButtons.SetActive(false);   
        }
    }

    public void Inventory()
    {
        Inv_UI.SetActive(true);
        Inv_UI.transform.parent.GetComponent<Inventory>().UpdateInventory();
    }

    public void BackFromInventory()
    {
        Inv_UI.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DialogueTrigger>())
        {
            if (collision.gameObject.GetComponent<DialogueTrigger>().MyDialogue.GetIndexOfStartingBranch()!=-1)
            {
                MainButtons.SetActive(false);
                collision.gameObject.GetComponent<DialogueTrigger>().MyDialogue.DialogueInit();
                PressingLeft = PressingRight = false;
                MyDialogue = collision.gameObject.GetComponent<DialogueTrigger>().MyDialogue;
            }
        }
        if (collision.gameObject.GetComponent<FlagTrigger>())
        {
            collision.gameObject.GetComponent<FlagTrigger>().Flags();
        }
        }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Dialogue>())
        {
            MyDialogue = collision.GetComponent<Dialogue>();
        }
        if (collision.GetComponent<NavigatorDoor>())
        {
            MyNavigator = collision.GetComponent<NavigatorDoor>();
        }
        if(collision.CompareTag("HideSpot"))
        {
            CanHide = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Dialogue>())
        {
            MyDialogue = null;
        }
        if (collision.GetComponent<NavigatorDoor>())
        {
            MyNavigator = null;
        }
        if (collision.CompareTag("HideSpot"))
        {
            CanHide = false;
        }
    }

    internal IEnumerator Reenable(float Sec = 0.5f)
    {
        yield return new WaitForSeconds(Sec);
        MainButtons.SetActive(true);
    }

    public void UseItem(int ItemIndex)
    {
        string Name = Inv_UI.transform.parent.GetComponent<Inventory>().CurrentItems[ItemIndex];

        if (MyDialogue.name == "Outside Door" && Name == "Amber Key")
        {
            BackFromInventory();
            Inv_UI.transform.parent.GetComponent<Inventory>().RemoveFromInventory(Name);
            MyDialogue.StartingDialogueBranch = 4;
            MyDialogue.DialogueInit();
            MainButtons.SetActive(false);
        }
        if (MyDialogue.name == "Table_For_Wine" && Name == "Wine")
        {
            BackFromInventory();
            Inv_UI.transform.parent.GetComponent<Inventory>().RemoveFromInventory(Name);
            MyDialogue.StartingDialogueBranch = 2;
            MyDialogue.DialogueInit();
            MainButtons.SetActive(false);
        }

    }
}
