using Godot;
using System;

public partial class TrashMan : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	private AnimatedSprite2D _animatedSprite;
	
	private String _sceneName;
	
	public CollisionShape2D _sword;
	
	public override void _Ready()
	{
		// Access to animation globally
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_sceneName = GetTree().CurrentScene.Name;
		_animatedSprite.Play("idle");
		_sword = GetNode<CollisionShape2D>("SwordArea/CollisionShape2D");
	}
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;


		// Handle trash wack
		//if (Input.IsActionPressed("fight"))
		//	_animatedSprite.Play("fight");
		//else
			//_animatedSprite.Play("idle");

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Y = direction.Y * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
		}
		if(velocity.X < 0)
		{
			_animatedSprite.Play("idle");
			_animatedSprite.FlipH = true;
		}	
		if(velocity.X > 0)
		{
			_animatedSprite.Play("idle");
			_animatedSprite.FlipH = false;
		}	
		if(velocity.Y < 0)
		{
			_animatedSprite.Play("idleup");
			//_animatedSprite.FlipV = true;
		}
		if(velocity.Y > 0)
		{
			_animatedSprite.Play("idledown");
			//_animatedSprite.FlipV = false;
		}	
		Velocity = velocity;
		MoveAndSlide();
		swing_sword();
	}
	public void swing_sword()
	{
		// Sword starts to the right
		//node.Position = node.right;
		if (Input.IsActionJustPressed("fight"))
		{
			// Lock movement and animation
			_animatedSprite.Play("fight");
			// Renable sword area hitbox
			_sword.Disabled = false;
		}
	}
	private void _on_animated_sprite_2d_animation_finished()
	{
		_animatedSprite.Play("idle");
		// Hide sword hitbox
		_sword.Disabled = true;
	}
}
