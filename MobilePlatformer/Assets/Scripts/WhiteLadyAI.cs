using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteLadyAI : MonoBehaviour {

    public bool IsLookingRight;

    private Rigidbody2D MyRigid;
    private Animator MyAnim;

    public float HorizVel;

    public enum LadyStates
    {
        Patrolling,
        Idle,
        Chasing,
        Disappeared,
        LeavingRoom,
        StayCalm,
        LookingForPlayer
    }

    public LadyStates MyState;

    public string CurrentRoom;
    private MoveScript Character;

    public GameObject LastDoor_IN;
    public GameObject LastDoor_OUT;

    public GameObject CurrentWaypoint;
    public float StoppingDistance;

    void Start ()
    {
        MyAnim = GetComponent<Animator>();
        MyRigid = GetComponent<Rigidbody2D>();
        Character = GameObject.Find("Player").GetComponent<MoveScript>();
        MyRigid.velocity = new Vector2(HorizVel * ((IsLookingRight)?1:-1), 0);
    }
	
	void Update ()
    {
        switch (MyState)
        {
            case LadyStates.Patrolling:
                PatrollingLogic();
                break;
            case LadyStates.Idle:
                IdleLogic();
                break;
            case LadyStates.Chasing:
                ChasingLogic();
                break;
            case LadyStates.Disappeared:
                ReappearLogic();
                break;
            case LadyStates.LeavingRoom:
                LeavingLogic();
                break;
            case LadyStates.StayCalm:
                CalmLogic();
                break;
            case LadyStates.LookingForPlayer:
                LookingForPlayerLogic();
                break;
            default:
                break;
        }
    }


    private void ChangeState(LadyStates chasing)
    {
        MyState = chasing;
        switch (chasing)
        {
            case LadyStates.Patrolling:
                break;
            case LadyStates.Idle:
                break;
            case LadyStates.Chasing:
                break;
            case LadyStates.Disappeared:
                break;
            case LadyStates.LeavingRoom:
                break;
            case LadyStates.StayCalm:
                break;
            case LadyStates.LookingForPlayer:
                break;
            default:
                break;
        }
    }

    #region AI Regions
    
    private void LookingForPlayerLogic()
    {
        // If player is not in same room
        if(!IsPlayerInMyRoom())
        {
            // Keep moving towards player's last accessed door from your room
            // Set timer to 3 seconds.
            // After 3 seconds, spawn from player's last door and continue chasing.
        }
        else
        {
            // If player is in same room, move to chasing
            ChangeState(LadyStates.Chasing);
        }
    }

    private void CalmLogic()
    {
        // Keep calm logic on for 10 Sec.
        // If broken, kill player
        // If not broken, move to leaving state.
    }

    private void LeavingLogic()
    {
        // move towards last door you entered and when you touch it, disappear and move to disappear state.
    }

    private void PatrollingLogic()
    {
        // If see player, then change state to chasing.
        if (CanISeeThePlayer())
        {
            ChangeState(LadyStates.Chasing);
        }
        
        // If reach waypoint, then change state to idle
        if(Mathf.Abs(transform.position.x) - Mathf.Abs(CurrentWaypoint.transform.position.x) <= StoppingDistance)
        {

        }
    }

    

    private void IdleLogic()
    {

        // After X sec go back to patrolling
        // Unless see player, then change to chasing
    }

    private void ChasingLogic()
    {
        // If player still in same room
        if(IsPlayerInMyRoom())
        {
            // If player still visible
            if (!Character.IsHiding)
            {
                // Go after player
            }
            else // If player is not visible
            {
                // Go next to hiding spot
                // When next to hiding spot go to Stay Calm State
            }
        }
        else
        {
            // If player not in same room move to looking for player
            ChangeState(LadyStates.LookingForPlayer);
        }
    }

    private void ReappearLogic()
    {
        // After X sec, appear in player's room. Random position.
    }
    
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("DirChanger"))
        {
            Flip();
        }
    }

    private void Flip()
    {
        Vector3 Sca = transform.localScale;
        Sca.x *= -1;
        transform.localScale = Sca;

        IsLookingRight = !IsLookingRight;
        MyRigid.velocity = new Vector2(HorizVel * ((IsLookingRight) ? 1 : -1), 0);
    }

    private bool IsPlayerInMyRoom()
    {
        return (Character.CurrentRoom == CurrentRoom);
    }

    private bool CanISeeThePlayer()
    {
        if (IsPlayerInMyRoom())
        {
            if(IsLookingRight && transform.position.x < Character.transform.position.x)
            {
                return true;
            }
            else if(!IsLookingRight && transform.position.x > Character.transform.position.x)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
