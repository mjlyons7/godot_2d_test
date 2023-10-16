using Godot;
using System;

public partial class Mob : RigidBody2D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        var animationTypes = animatedSprite.SpriteFrames.GetAnimationNames();

        // choose a random animation to play
        var random = new Random();
        var randomAnimationIndex = random.NextInt64(0, animationTypes.Length - 1);
        animatedSprite.Play(animationTypes[randomAnimationIndex]);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {

    }

    // called on signal screen_exited()
    public void OnVisibleOnScreenNotifier2dScreenExited()
    {
        QueueFree();
    }
}
