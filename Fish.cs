using System.Numerics;
using Raylib_cs;
namespace ThatOnePVZMinigame;

public class Fish : WorldObject {

	#region Movement & AI

	/// <summary>
	/// How fast the fish swims, in pixels per second.
	/// </summary>
	public float Speed = 20f;

	/// <summary>
	/// Where the fish is currently swimming towards.
	/// </summary>
	protected Vector2 Destination = new(0, 0);

	/// <summary>
	/// How far the fish needs to be from its destination to be considered "arrived", in pixels.
	/// </summary>
	protected float ArrivalThreshold = 10f;

	/// <summary>
	/// A target object that overrides the fish's random swimming behavior.
	/// </summary>
	protected WorldObject? Target = null;

	protected enum State {
		Idle,
		Wandering,
		Chasing
	}

	#endregion

	#region Health

	public bool IsAlive = true;

	/// <summary>
	/// How fast the fish floats upwards when dead, in pixels per second.
	/// </summary>
	public float FloatRate = 15f;

	/// <summary>
	/// How much health the fish currently has.
	/// </summary>
	public float Health = 100f;

	/// <summary>
	/// How much health the fish can have at maximum.
	/// </summary>
	public float MaxHealth = 100f;

	/// <summary>
	/// How long the fish's health lasts, in seconds.
	/// </summary>
	public float Lifespan = 10f;

	#endregion

	#region Cashflow

	/// <summary>
	/// How much the fish costs to buy and spawn.
	/// </summary>
	public static int Cost { get; protected set; }

	/// <summary>
	/// How often the fish drops coins, in seconds.
	/// </summary>
	protected static float CoinRate { get; set; } = 2f;

	/// <summary>
	/// How many seconds have passed since the fish last dropped a coin.
	/// </summary>
	public float TimeSinceLastCoin = 0f;

	#endregion

	public Fish (
		string textureName,
		float speed,
		bool v = false,
		int s = 0
		) :
		base (
			startPos: new Vector2(
				Raylib.GetRandomValue(0, Raylib.GetScreenWidth()),
				Raylib.GetRandomValue(0, Raylib.GetScreenHeight())
				),
			textureName: textureName,
			v: v,
			s: s + 1
		)
		{
		Log.Me(() => $"Creating fish ...", v, s + 1);
		Speed = speed;

		Destination = new Vector2(
			Raylib.GetRandomValue(Sprite.Width, Raylib.GetScreenWidth() - Sprite.Width * 2),
			Raylib.GetRandomValue(Sprite.Height, Raylib.GetScreenHeight() - Sprite.Height * 2)
		);

		Log.Me(() => "Done!", v, s + 1);
	}



	/// <summary>
	/// Updates the fish's position, health, and draws it on the screen.
	/// </summary>
	/// <param name="v">Whether to print logs from this method and any methods called within it.</param>
	/// <param name="s">The stack layer this method belongs in.</param>
	public override void Update(bool v = false, int s = 0) {

		if (IsAlive) {
			Log.Me(() => $"Updating fish position...", v, s + 1);
			MoveTowardsDestination(Destination, v, s + 1);

			Log.Me(() => $"Updating fish health...", v, s + 1);
			Health -= (MaxHealth / Lifespan) * Raylib.GetFrameTime();

			if (Health <= 0f) {
				IsAlive = false;
				Log.Me(() => "Fish has died!", v, s + 1);
			}
		}
		
		// Float upwards if dead; despawn upon reaching the top.
		else {
			Position.Y -= FloatRate * Raylib.GetFrameTime();

			if (Position.Y < 0) {
				ToDelete = true;
				Log.Me(() => "Fish has floated to the top and will be deleted!", v, s + 1);
			}
		}

		// HP Bar
		Log.Me(() => $"Updating HP bar...", v, s + 1);
		Raylib.DrawRectangle(
			posX: (int) Position.X,
			posY: (int) Position.Y - 10,
			width: 50,
			5,
			Color.DarkGray
			);
		Raylib.DrawRectangle((int) Position.X, (int) Position.Y - 10, (int) (50 * (Health / 100)), 5, Color.Green);
	}


	/// <summary>
	/// Moves the fish towards the specified destination, draws it on the screen,
	/// and picks a new random destination upon arrival.
	/// </summary>
	/// <param name="destination">The 2D vector position of where the fish should go.</param>
	/// <param name="v">Whether to print logs from this method and any methods called within it.</param>
	/// <param name="s">The stack layer this method belongs in.</param>
	protected virtual void MoveTowardsDestination(Vector2 destination, bool v = false, int s = 0) {
		Log.Me(() => $"Moving from ({Position.X:F0}, {Position.Y:F0}) to ({destination.X:F0}, {destination.Y:F0})...", v, s + 1);
		Vector2 direction = Vector2.Normalize(destination - Position);
		Position += direction * Speed * Raylib.GetFrameTime();

		//If moving to the left, flip the sprite.
		int spriteDirX = direction.X < 0 ? -1 : 1;
		Rectangle source = new(0, 0, Sprite.Width * spriteDirX, Sprite.Height);

		// Draw
		Raylib.DrawTextureRec(Sprite, source, Position, Color.White);

		// Update hitbox position
		Hitbox.X = (int) Position.X - Hitbox.Width / 2;
		Hitbox.Y = (int) Position.Y - Hitbox.Height / 2;

		/*
		 * Yes, it draws the textures and updates the hitbox here instead of passing
		 * the job to the base method. The base method only draws statically--
		 * it cannot (reasonably) be changed to allow flipping the sprite based on direction.
		 */

		// If arrived at destination, pick a new random destination.
		if (Vector2.Distance(Position, destination) < ArrivalThreshold) {
			Log.Me(() => "Arrived at destination, choosing new destination...", v, s + 1);
			Destination = new Vector2(
				Raylib.GetRandomValue(Sprite.Width, Raylib.GetScreenWidth() - Sprite.Width * 2),
				Raylib.GetRandomValue(Sprite.Height, Raylib.GetScreenHeight() - Sprite.Height * 2)
			);
		}
	}
}



#region Implementations

public class Cod : Fish {
	public Cod(bool v = false, int s = 0) :
		base(
			textureName: "fish_cod",
			speed: 30f,
			v: v,
			s: s + 1
			)
		{
		Log.Me(() => $"Creating cod ...", v, s + 1);
		Cost = 50;
		Health = 100f;
		Lifespan = 15f;
		Log.Me(() => "Done!", v, s + 1);
	}
}


#endregion