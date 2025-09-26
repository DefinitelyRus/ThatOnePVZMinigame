using System.Numerics;
using Raylib_cs;
namespace ThatOnePVZMinigame;

public class Fish : WorldObject {

	#region Callbacks

	public Fish (
		string textureName,
		float maxHealth,
		float attrition,
		int cost,
		float coinInterval,
		Type[] coinTypes,
		float speed,
		Type preference,
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
		MaxHealth = maxHealth;
		Health = MaxHealth;
		Attrition = attrition;

		Cost = cost;
		CoinInterval = coinInterval;
		CoinTypes = coinTypes;

		Speed = speed;
		PreferredTarget = preference;

		//Destination = new Vector2(
		//	Raylib.GetRandomValue(Sprite.Width, Raylib.GetScreenWidth() - Sprite.Width * 2),
		//	Raylib.GetRandomValue(Sprite.Height, Raylib.GetScreenHeight() - Sprite.Height * 2)
		//);

		Log.Me(() => "Subscribing to OnAnnounceExistence event...", v, s + 1);
		OnAnnounceExistence += NotifySpawn;

		Log.Me(() => "Subscribing to OnDelete event...", v, s + 1);
		OnDelete += NotifyDespawn;

		Log.Me(() => "Done!", v, s + 1);
	}

	/// <summary>
	/// Updates the fish's position, health, and draws it on the screen.
	/// </summary>
	/// <param name="v">Whether to print logs from this method and any methods called within it.</param>
	/// <param name="s">The stack layer this method belongs in.</param>
	public override void Update(bool v = false, int s = 0) {
		DoSomething(v, s + 1); 
		DepleteHealth(v, s + 1);
		GenerateIncome(v, s + 1);
	}

	#endregion

	#region Health

	/// <summary>
	/// IS THE FISH ALIVE??? YES? NO?
	/// </summary>
	public bool IsAlive { get; protected set; } = true;

	/// <summary>
	/// How much health the fish currently has.
	/// </summary>
	public float Health { get; set; } = 100f;

	/// <summary>
	/// How much health the fish can have at maximum.
	/// </summary>
	public float MaxHealth { get; protected set; } = 100f;

	/// <summary>
	/// How much the fish's health decreases per second.
	/// </summary>
	protected float Attrition;

	/// <summary>
	/// How fast the fish floats upwards when dead, in pixels per second.
	/// </summary>
	private readonly float FloatRate = 25f;


	/// <summary>
	/// Depletes the fish's health over time, kills it at 0 HP, and draws its health bar.
	/// </summary>
	/// <param name="v">Whether to print logs from this method and any methods called within it.</param>
	/// <param name="s">The stack layer this method belongs in.</param>
	protected void DepleteHealth(bool v = false, int s = 0) {
		if (!IsAlive) return;

		// Decrease health over time.
		float damage = Attrition * Raylib.GetFrameTime();
		Log.Me(() => $"Depleting fish health by {damage:F3}...", v, s + 1);
		Health -= damage;

		// Kill on 0 HP.
		if (Health <= 0f) {
			IsAlive = false;
			CurrentState = State.Dead;
			Log.Me(() => "Fish has died!", v, s + 1);
		}

		// HP Bar
		Log.Me(() => $"Updating HP bar...", v, s + 1);
		Raylib.DrawRectangle(
			posX: (int) Position.X,
			posY: (int) Position.Y - 10,
			width: 50,
			height: 5,
			color: Color.DarkGray
		);
		Raylib.DrawRectangle(
			posX: (int) Position.X,
			posY: (int) Position.Y - 10,
			width: (int) (50 * (Health / MaxHealth)),
			height: 5,
			color: Color.Green
		);
	}


	/// <summary>
	/// Floats the fish upwards when dead, and despawns it upon reaching the top of the screen.
	/// </summary>
	/// <param name="v">Whether to print logs from this method and any methods called within it.</param>
	/// <param name="s">The stack layer this method belongs in.</param>
	protected void Float(bool v = false, int s = 0) {
		Position.Y -= FloatRate * Raylib.GetFrameTime();

		// Flip the sprite to face upwards.
		Rectangle source = new(0, 0, Sprite.Width, -Sprite.Height);
		Raylib.DrawTextureRec(Sprite, source, Position, Color.DarkGreen);

		if (Position.Y < 0) {
			Log.Me(() => "Fish has floated to the top and will be deleted!", v, s + 1);
			Despawn(v, s + 1);
		}
	}

	#endregion

	#region Moneys

	/// <summary>
	/// How much the fish costs to buy and spawn.
	/// </summary>
	public static int Cost { get; protected set; }

	/// <summary>
	/// How often the fish drops coins, in seconds.
	/// </summary>
	protected static float CoinInterval { get; set; } = 2f;

	/// <summary>
	/// How many seconds have passed since the fish last dropped a coin.
	/// </summary>
	public float TimeSinceLastCoin = 0f;

	protected Type[] CoinTypes = [typeof(SilverCoin)];

	protected void GenerateIncome(bool v = false, int s = 0) {
		if (!IsAlive) return;
		TimeSinceLastCoin += Raylib.GetFrameTime();


		if (TimeSinceLastCoin >= CoinInterval) {
			Type coinType = CoinTypes[Raylib.GetRandomValue(0, CoinTypes.Length - 1)];
			Log.Me(() => $"Dropping coin of type {coinType.Name}...", v, s + 1);
			TimeSinceLastCoin = 0f;

			switch (coinType.Name) {
				case "SilverCoin":
					WorldManager.SpawnCoin<SilverCoin>(Position, v, s + 1);
					break;
				case "GoldCoin":
					WorldManager.SpawnCoin<GoldCoin>(Position, v, s + 1);
					break;
				default:
					Log.Err(() => $"Unknown coin type: {coinType.Name}. Defaulting to silver.", v, s + 1);
					WorldManager.SpawnCoin<SilverCoin>(Position, v, s + 1);
					break;
			}

			Log.Me(() => "Done!", v, s + 1);
		}
	}

	#endregion

	#region AI & Targeting

	/// <summary>
	/// How fast the fish swims, in pixels per second.
	/// </summary>
	protected float Speed = 20f;

	/// <summary>
	/// The current state the fish is in.
	/// </summary>
	protected State CurrentState = State.ChooseRandom;

	/// <summary>
	/// The possible states the fish can be in.
	/// </summary>
	protected enum State {
		Idle,
		Wandering,
		Chasing,
		ChooseRandom,
		Ohio,
		Dead
	}

	/// <summary>
	/// How long the fish will idle for, in seconds.
	/// </summary>
	protected float IdleTimer = 0f;

	/// <summary>
	/// Where the fish is currently swimming towards.
	/// </summary>
	protected Vector2 Destination = new(0, 0);

	/// <summary>
	/// A target object that overrides the fish's random swimming behavior.
	/// </summary>
	protected WorldObject? Target = null;

	/// <summary>
	/// The type of target object the fish prefers to chase.
	/// </summary>
	protected Type PreferredTarget = typeof(Chum);

	/// <summary>
	/// How far the fish needs to be from its destination to be considered "arrived", in pixels.
	/// </summary>
	protected float ArrivalThreshold = 10f;


	/// <summary>
	/// Behavior tree for the fish's AI.
	/// </summary>
	/// <param name="v">Whether to print logs from this method and any methods called within it.</param>
	/// <param name="s">The stack layer this method belongs in.</param>
	protected void DoSomething(bool v = false, int s = 0) {
		switch (CurrentState) {
			case State.Idle:
				Log.Me(() => "Idling...", v, s + 1);
				Idle(v, s + 1);
				break;

			case State.Wandering:
				Log.Me(() => "Wandering...", v, s + 1);
				MoveTowards(Destination, v, s + 1);
				break;

			case State.Chasing:
				Log.Me(() => "Chasing target...", v, s + 1);
				ChaseTarget(v, s + 1);
				break;

			case State.ChooseRandom:
				Log.Me(() => "Choosing new random action...", v, s + 1);
				int randomValue = Raylib.GetRandomValue(0, 1);

				// Idle
				if (randomValue == 0) {
					IdleTimer = Raylib.GetRandomValue(1, 5);
					Log.Me(() => $"Idling for {IdleTimer} seconds...", v, s + 1);
					CurrentState = State.Idle;
				}

				// Wander
				else {
					CurrentState = State.Wandering;
					Destination = GetRandomPosition(Sprite, v, s + 1);
				}

				break;

			case State.Dead:
				Log.Me(() => "Fish is dead. Floating up...", v, s + 1);
				Float(v, s + 1);
				break;

			default:
				Log.Err(() => $"Fish is in an unknown state: {CurrentState}. Reverting to wandering.", v, s + 1);
				CurrentState = State.Wandering;
				Destination = GetRandomPosition(Sprite, v, s + 1);
				break;
		}
	}


	/// <summary>
	/// Moves the fish towards the specified destination, draws it on the screen,
	/// and picks a new random destination upon arrival.
	/// </summary>
	/// <param name="destination">The 2D vector position of where the fish should go.</param>
	/// <param name="v">Whether to print logs from this method and any methods called within it.</param>
	/// <param name="s">The stack layer this method belongs in.</param>
	protected virtual void MoveTowards(Vector2 destination, bool v = false, int s = 0) {
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

		CheckArrival(v, s + 1);
	}


	/// <summary>
	/// Chases the target object if it exists; otherwise, switches to wandering state.
	/// </summary>
	/// <param name="v">Whether to print logs from this method and any methods called within it.</param>
	/// <param name="s">The stack layer this method belongs in.</param>
	protected virtual void ChaseTarget(bool v = false, int s = 0) {
		if (Target == null) {
			CurrentState = State.Wandering;
			Destination = GetRandomPosition(Sprite, v, s + 1);
			return;
		}

		Destination = Target.Position;
		MoveTowards(Destination, v, s + 1);
	}


	/// <summary>
	/// Checks if the fish has arrived at its destination.
	/// </summary>
	/// <param name="v">Whether to print logs from this method and any methods called within it.</param>
	/// <param name="s">The stack layer this method belongs in.</param>
	protected void CheckArrival(bool v = false, int s = 0) {
		// If arrived at destination, idle or pick a new random destination.
		if (Vector2.Distance(Position, Destination) < ArrivalThreshold) {
			Log.Me(() => "Arrived at destination...", v, s + 1);
			CurrentState = State.ChooseRandom;

			if (Target != null) {
				// If the target is a chum (or its subtype), eat it and gain health.
				if (Target.GetType().IsSubclassOf(typeof(Chum))) {
					Log.Me(() => "Eating target chum...", v, s + 1);
					Chum chum = (Chum) Target;
					Health += Math.Clamp(chum.HealAmount, 0, MaxHealth - Health);
					chum.Despawn(v, s + 1);
				}

				if (Target is Fish fish) {
					Health += Math.Clamp(fish.Health, 0, MaxHealth - Health);
					fish.Despawn(v, s + 1);
				}

				Target = null;
			}
		}
	}


	/// <summary>
	/// Idles the fish for a set amount of time, then switches to wandering state.
	/// </summary>
	/// <param name="v">Whether to print logs from this method and any methods called within it.</param>
	/// <param name="s">The stack layer this method belongs in.</param>
	protected virtual void Idle(bool v = false, int s = 0) {
		IdleTimer -= Raylib.GetFrameTime();
		if (IdleTimer <= 0f) {
			CurrentState = State.Wandering;
			Destination = GetRandomPosition(Sprite, v, s + 1);
		}
		base.Update(v, s + 1);
	}


	/// <summary>
	/// The method that runs when a new world object is created.
	/// </summary>
	/// <param name="obj">The object to be notified of.</param>
	/// <param name="v">Whether to print logs from this method and any methods called within it.</param>
	/// <param name="s">The stack layer this method belongs in.</param>
	protected virtual void NotifySpawn(WorldObject obj, bool v = false, int s = 0) {
		if (obj == this) return; // Don't notify self.
		if (!IsAlive) return; // Don't notify if dead.

		Log.Me(() => $"Fish notified of new object of type {obj.GetType().Name}...", v, s + 1);

		// Ignore if already have a target.
		if (Target != null) {
			Log.Me(() => "Already have a target, ignoring new object.", v, s + 1);
			return;
		}

		// Ignore if not the preferred target type.
		if (!obj.GetType().IsSubclassOf(PreferredTarget) && obj.GetType() != PreferredTarget) {
			Log.Me(() => "Not interested in this type of object.", v, s + 1);
			return;
		}

		// Ignore if eating the object would overheal the fish.
		if (obj is Fish fish && MaxHealth - Health < fish.Health / 2) {
			Log.Me(() => "Not hungry enough to chase this fish.", v, s + 1);
			return;
		}

		else if (obj is Chum chum && MaxHealth - Health < chum.HealAmount / 2) {
			Log.Me(() => "Not hungry enough to chase this chum.", v, s + 1);
			return;
		}

		// Chase the object.
		Log.Me(() => "Chasing object...", v, s + 1);
		CurrentState = State.Chasing;
		Target = obj;

		Log.Me(() => "Done!", v, s + 1);
	}


	/// <summary>
	/// The method that runs when a world object is about to be deleted.
	/// </summary>
	/// <param name="obj">The object that is about to be deleted.</param>
	/// <param name="v">Whether to print logs from this method and any methods called within it.</param>
	/// <param name="s">The stack layer this method belongs in.</param>
	protected virtual void NotifyDespawn(WorldObject obj, bool v = false, int s = 0) {
		if (obj == this) return; // Don't notify self.

		if (Target != obj) return; // Not its target.

		Log.Me(() => $"Fish notified of the despawn of its target. Reverting to random movement...", v, s + 1);
		CurrentState = State.Wandering;
		Destination = GetRandomPosition(Sprite, v, s + 1);
		Target = null;

		Log.Me(() => "Done!", v, s + 1);
	}

	#endregion

	#region Static Methods

	/// <summary>
	/// Gets a new random destination within the screen bounds, accounting for the sprite size.
	/// </summary>
	/// <param name="sprite">The sprite to take the size of.</param>
	/// <param name="v">Whether to print logs from this method and any methods called within it.</param>
	/// <param name="s">The stack layer this method belongs in.</param>
	/// <returns>A confined random 2D vector position.</returns>
	protected static Vector2 GetRandomPosition(Texture2D sprite, bool v = false, int s = 0) {
		Log.Me(() => $"Choosing new random destination within screen bounds, accounting for sprite size...", v, s + 1);
		Vector2 newDestination = GetRandomPosition(
			topLeftEdge: new Vector2(sprite.Width, sprite.Height),
			bottomRightEdge: new Vector2(Raylib.GetScreenWidth() - sprite.Width * 2, Raylib.GetScreenHeight() - sprite.Height * 2),
			v: v,
			s: s + 1
		);

		Log.Me(() => "Passing new destination back to caller...", v, s + 1);
		return newDestination;
	}


	/// <summary>
	/// Gets a new random destination within the specified bounds.
	/// </summary>
	/// <param name="topLeftEdge">The top-left edge of the area to select from.</param>
	/// <param name="bottomRightEdge">The bottom-right edge of the area to select from.</param>
	/// <param name="v">Whether to print logs from this method and any methods called within it.</param>
	/// <param name="s">The stack layer this method belongs in.</param>
	/// <returns>A confined random 2D vector position.</returns>
	protected static Vector2 GetRandomPosition(Vector2 topLeftEdge, Vector2 bottomRightEdge, bool v = false, int s = 0) {
		Log.Me(() => $"Choosing new random destination between ({topLeftEdge.X:F0}, {topLeftEdge.Y:F0}) and ({bottomRightEdge.X:F0}, {bottomRightEdge.Y:F0}...", v, s + 1);
		Vector2 newDestination = new(
			Raylib.GetRandomValue((int) topLeftEdge.X, (int) bottomRightEdge.X),
			Raylib.GetRandomValue((int) topLeftEdge.Y, (int) bottomRightEdge.Y)
		);

		Log.Me(() => $"New destination is ({newDestination.X:F0}, {newDestination.Y:F0})", v, s + 1);
		return newDestination;
	}

	#endregion

}



#region Implementations

public class Cod : Fish {
	public Cod(bool v = false, int s = 0) :
		base(
			textureName: "fish_cod",
			maxHealth: 150f,
			attrition: 5f,
			cost: 50,
			coinInterval: 5f,
			coinTypes: [typeof(SilverCoin)],
			speed: 60f,
			preference: typeof(Chum),
			v: v,
			s: s + 1
			)
		{
		Log.Me(() => $"Created Cod.", v, s + 1);
	}
}

#endregion