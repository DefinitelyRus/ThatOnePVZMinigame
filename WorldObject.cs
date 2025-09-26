using System.Numerics;
using Raylib_cs;

namespace ThatOnePVZMinigame;
public abstract class WorldObject {
	protected Texture2D Sprite;
	protected Rectangle Hitbox;

	protected Vector2 Position = Vector2.Zero;

	/// <summary>
	/// A flag indicating whether the object should be deleted on the next update.
	/// </summary>
	public bool ToDelete = false;

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

		Log.Me(() => "Done!", v, s + 1);
	}

	public virtual void Update(bool v = false, int s = 0) {
		Log.Me(() => "Drawing world object...", v, s + 1);

		// Draw
		Raylib.DrawTexture(Sprite, (int) Position.X, (int) Position.Y, Color.White);

		// Update hitbox position
		Hitbox.X = (int) Position.X - Hitbox.Width / 2;
		Hitbox.Y = (int) Position.Y - Hitbox.Height / 2;

		Log.Me(() => "Done!", v, s + 1);
	}
}
