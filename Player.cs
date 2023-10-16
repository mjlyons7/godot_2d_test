using Godot;
using System;
using System.IO;

public partial class Player : Area2D
{
    // export keyword allows value to be overwritten in the inspector
    [Export]
    public int Speed = 400; // px/sec

    public Vector2 Screensize;

    [Signal]
    public delegate void HitEventHandler();


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Screensize = GetViewportRect().Size;
        //Hide();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        var directionVector = new Vector2();

        if (Input.IsActionPressed("move_right"))
            directionVector.X += 1;
        if (Input.IsActionPressed("move_left"))
            directionVector.X += -1;
        if (Input.IsActionPressed("move_up"))
            directionVector.Y += -1;
        if (Input.IsActionPressed("move_down"))
            directionVector.Y += 1;

        // update position
        directionVector = directionVector.Normalized();
        var velocityPxPerFrame = directionVector * Speed * (float) delta;
        Position += velocityPxPerFrame;
        Position = Position.Clamp(Vector2.Zero, Screensize);

        // animate
        var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        if (velocityPxPerFrame.X != 0)
        {
            animatedSprite2D.Animation = "walk";
            if (velocityPxPerFrame.X < 0)
                animatedSprite2D.FlipH = true;
            else
                animatedSprite2D.FlipH = false;
        }
        if (velocityPxPerFrame.Y != 0)
        {
            animatedSprite2D.Animation = "up";
            if (velocityPxPerFrame.Y > 0) // positive Y is down
                animatedSprite2D.FlipV = true;
            else
                animatedSprite2D.FlipV = false;
        }
        if (velocityPxPerFrame.Length() > 0)
            animatedSprite2D.Play();
        else
            animatedSprite2D.Stop();

    }

    private void OnBodyEntered(PhysicsBody2D body)
    {
        Hide();
        EmitSignal(SignalName.Hit);
        // disable the player's hitbox, so it doesn't keep emitting the hit signal
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
    }

    /// <summary>
    /// sets the position, shows the canvas item, enables collision
    /// </summary>
    /// <param name="position"></param>
    public void Start(Vector2 position)
    {
        Position = position;
        Show();
        GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
    }
}
