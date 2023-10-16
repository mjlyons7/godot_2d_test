using Godot;
using System;
public partial class hud : CanvasLayer
{

	[Signal]
	public delegate void StartGameEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnStartButtonPressed()
	{
		GetNode<Button>("StartButton").Hide();
		EmitSignal(SignalName.StartGame);

    }

	private void OnMessageTimerTimeout()
	{
		GetNode<Label>("Message").Hide();
	}

	/// <summary>
	/// Show text on screen, and start message timer
	/// </summary>
	/// <param name="text"></param>
	public void ShowMessageAndStartMessageTimer(string text)
	{
		ShowMessage(text);

		var messageTimer = GetNode<Timer>("MessageTimer");
		messageTimer.Start();
	}

	private void ShowMessage(string text)
	{
        var message = GetNode<Label>("Message");
        message.Text = text;
        message.Show();
    }

	/// <summary>
	/// Show game over message, and restart
	/// </summary>
	public async void ShowGameOver()
	{
		ShowMessageAndStartMessageTimer("GAME OVER!");

		var messageTimer = GetNode<Timer>("MessageTimer");
		await ToSignal(messageTimer, Timer.SignalName.Timeout);

		// reset back to title screen
		ShowMessage("DODGE THE CREEPS!");

		var resetTimer = GetTree().CreateTimer(1);
		await ToSignal(resetTimer, SceneTreeTimer.SignalName.Timeout);
		GetNode<Button>("StartButton").Show();
    }

	public void UpdateScore(int score)
	{
		var scoreLabel = GetNode<Label>("ScoreLabel");
		scoreLabel.Text = score.ToString();
    }
}
