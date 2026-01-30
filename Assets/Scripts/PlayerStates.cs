using UnityEngine;

public class IdleState : PlayerState
{
    public IdleState(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Entered Idle State");
    }

    public override void Update()
    {
        if (player.MoveInput.x != 0)
        {
            stateMachine.ChangeState(player.MoveState);
        }
        else if (!player.IsGrounded)
        {
            stateMachine.ChangeState(player.FallState);
        }
    }

    public override void FixedUpdate()
    {
        player.Rb.velocity = new Vector2(0, player.Rb.velocity.y);
    }
}

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
            stateMachine.ChangeState(player.IdleState);
        }
        else if (!player.IsGrounded)
        {
            stateMachine.ChangeState(player.FallState);
        }
    }

    public override void FixedUpdate()
    {
        player.Rb.velocity = new Vector2(player.MoveInput.x * player.speed, player.Rb.velocity.y);
    }
}

public class JumpState : PlayerState
{
    public JumpState(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Entered Jump State");
        player.Rb.velocity = new Vector2(player.Rb.velocity.x, player.jumpForce);
    }

    public override void Update()
    {
        if (player.Rb.velocity.y < 0)
        {
            stateMachine.ChangeState(player.FallState);
        }
    }

    public override void FixedUpdate()
    {
        player.Rb.velocity = new Vector2(player.MoveInput.x * player.speed, player.Rb.velocity.y);
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
        if (player.IsGrounded)
        {
            if (player.MoveInput.x != 0)
            {
                stateMachine.ChangeState(player.MoveState);
            }
            else
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
    }

    public override void FixedUpdate()
    {
        player.Rb.velocity = new Vector2(player.MoveInput.x * player.speed, player.Rb.velocity.y);
    }
}