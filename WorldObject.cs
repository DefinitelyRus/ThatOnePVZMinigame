using System.Numerics;
using Raylib_cs;

namespace ThatOnePVZMinigame;
public abstract class WorldObject {
	protected Texture2D Sprite;
	protected Rectangle Hitbox;
	public Vector2 Position = Vector2.Zero;

	/// <summary>
	/// A flag indicating whether the object should be deleted on the next update.
	/// </summary>
	public bool ToDelete = false;

	/// <summary>
	/// Triggers when a world object is created.
	/// </summary>
	public static event Action<WorldObject, bool, int>? OnAnnounceExistence;

	/// <summary>
	/// Triggers when a world object is deleted.
	/// </summary>
	public static event Action<WorldObject, bool, int>? OnDelete;

	/// <summary>
	/// How often (in seconds) the world object should announce its existence.
	/// </summary>
	protected float TimeBetweenAnnouncements = 0.5f;

	/// <summary>
	/// How much time (in seconds) has passed since the last announcement.
	/// </summary>
	private float TimeSinceLastAnnouncement = 0f;


	public WorldObject(
		Vector2 startPos,
		string textureName,
		bool v = false,
		int s = 0
		)
		{
		Log.Me(() => $"Creating world object at ({startPos.X}, {startPos.Y}) with texture \"{textureName}\"...", v, s + 1);

		Position = startPos;

		Sprite = ResourceManager.GetTexture(textureName, v, s + 1);

		// Hitbox is double the size of the sprite, centered on the sprite
		int width = Sprite.Width * 2;
		int height = Sprite.Height * 2;
		int centerX = (int) Position.X - Sprite.Width / 2;
		int centerY = (int) Position.Y - Sprite.Height / 2;
		int cornerX = centerX - width / 2;
		int cornerY = centerY - height / 2;
		Hitbox = new(cornerX, cornerY, width, height);

		Log.Me(() => "Broadcasting OnAnnounceExistence event...", v, s + 1);
		OnAnnounceExistence?.Invoke(this, v, s + 1);

		Log.Me(() => "Done!", v, s + 1);
	}

	public virtual void Update(bool v = false, int s = 0) {
		Log.Me(() => "Drawing world object...", v, s + 1);

		// Draw
		Raylib.DrawTexture(Sprite, (int) Position.X, (int) Position.Y, Color.White);

		// Update hitbox position
		Hitbox.X = (int) Position.X - Hitbox.Width / 2;
		Hitbox.Y = (int) Position.Y - Hitbox.Height / 2;

		//Announce existence periodically
		TimeSinceLastAnnouncement += Raylib.GetFrameTime();
		if (TimeSinceLastAnnouncement >= TimeBetweenAnnouncements) {
			Log.Me(() => "Broadcasting OnAnnounceExistence event...", v, s + 1);
			OnAnnounceExistence?.Invoke(this, v, s + 1);
			TimeSinceLastAnnouncement = 0f;
		}

		Log.Me(() => "Done!", v, s + 1);
	}
		Log.Me(() => "Done!", v, s + 1);
	}
}
