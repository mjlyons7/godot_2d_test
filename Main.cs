using Godot;
using System;

public partial class Main : Node
{

    // linked to the mob scene in the node tree
    [Export]
    public PackedScene mobScene;

    private int _score;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //NewGame();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {

    }

    /// <summary>
    /// Spawn a new Mob enemy
    /// </summary>
    private void OnMobTimerTimeout()
    {
        // set mob's location
        var random = new Random();
        var mob = mobScene.Instantiate<Mob>();
        var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
        mobSpawnLocation.ProgressRatio = (float) random.NextDouble();
        mob.Position = mobSpawnLocation.Position;

        // set rotation
        mob.Rotation = mobSpawnLocation.Rotation + (float)Math.PI / 2;
        var randomAngle = random.NextDouble() * Math.PI / 2 - Math.PI / 4;
        mob.Rotation += (float) randomAngle;

        // velocity
        Vector2 randomVelocity = new Vector2((float)random.NextDouble() * 100 + 150, 0);
        mob.LinearVelocity = randomVelocity.Rotated(mob.Rotation);

        AddChild(mob);

    }

    /// <summary>
    /// increment score
    /// </summary>
    private void OnScoreTimerTimeout()
    {
        _score++;
        var hud = GetNode<hud>("HUD");
        hud.UpdateScore(_score);
    }

    /// <summary>
    /// Start other timers
    /// </summary>
    private void OnStartTimerTimeout()
    {
        GetNode<Timer>("MobTimer").Start();
        GetNode<Timer>("ScoreTimer").Start();
    }

    /// <summary>
    /// What to do when the game ends
    /// </summary>
    public void GameOver()
    {
        // reset timers
        GetNode<Timer>("MobTimer").Stop();
        GetNode<Timer>("ScoreTimer").Stop();

        // update hud
        var hud = GetNode<hud>("HUD");
        hud.ShowGameOver();

        // game over sound, end game music
        GetNode<AudioStreamPlayer>("DeathSound").Play();
        GetNode<AudioStreamPlayer>("Music").Stop();
    }

    /// <summary>
    /// reset score and player position, reset start timer
    /// </summary>
    public void NewGame()
    {
        _score = 0;
        GetNode<Player>("Player").Start(GetNode<Marker2D>("PlayerStartPosition").Position);
        GetNode<Timer>("StartTimer").Start();

        // clear any old mob enemies from previous play session
        GetTree().CallGroup("mobs", Node.MethodName.QueueFree);

        // update hud
        var hud = GetNode<hud>("HUD");
        hud.ShowMessageAndStartMessageTimer("GET READY!");
        hud.UpdateScore(_score);

        // start game music
        GetNode<AudioStreamPlayer>("Music").Play();
    }
}
