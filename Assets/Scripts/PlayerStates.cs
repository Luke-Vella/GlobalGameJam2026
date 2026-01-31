using UnityEngine;

public class MoveState : PlayerState
{
    public MoveState(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Entered Move State");
    }

    public override void Update()
    {
        if (player.MoveInput.x == 0)
        {
            stateMachine.ChangeState(player.IdleSwimState);
        }
    }

    public override void FixedUpdate()
    {
        player.Rb.velocity = new Vector2(player.MoveInput.x * player.swimSpeed, player.Rb.velocity.y);
    }
}

public class FallState : PlayerState
{
    public FallState(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Entered Fall State");
    }

    public override void Update()
    {
        if (player.MoveInput.x != 0)
        {
            stateMachine.ChangeState(player.SwimState);
        }
        else
        {
            stateMachine.ChangeState(player.IdleSwimState);
        }

    }

    public override void FixedUpdate()
    {
        player.Rb.velocity = new Vector2(player.MoveInput.x * player.swimSpeed, player.Rb.velocity.y);
    }
}

public class IdleSwimState : PlayerState
{
    private float sinkSpeed = 0.5f; // Adjust this value to control descent speed

    public IdleSwimState(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Entered Idle Swim State - Floating");
    }

    public override void Update()
    {
        // Check if player is giving any movement input
        if (player.MoveInput.magnitude > 0.1f)
        {
            stateMachine.ChangeState(player.SwimState);
        }
    }

    public override void FixedUpdate()
    {
        // Apply slow downward force to simulate sinking
        player.Rb.velocity = new Vector2(player.Rb.velocity.x, -sinkSpeed);
    }
}

public class SwimState : PlayerState
{
    private float sinkSpeed = 0.5f; // Constant downward pull

    public SwimState(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Entered Swim State - Swimming");
    }

    public override void Update()
    {
        // Return to idle when no input
        if (player.MoveInput.magnitude < 0.1f)
        {
            stateMachine.ChangeState(player.IdleSwimState);
        }
    }

    public override void FixedUpdate()
    {
        // Full 2D movement - both X and Y axes
        Vector2 swimForce = player.MoveInput.normalized * player.swimSpeed;
        player.Rb.AddForce(swimForce, ForceMode2D.Force);
        
        // Apply constant downward pull - player must swim up to counteract
        player.Rb.AddForce(Vector2.down * sinkSpeed, ForceMode2D.Force);
    }
}