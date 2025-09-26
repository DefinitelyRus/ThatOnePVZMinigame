using System.Numerics;
using Raylib_cs;

namespace ThatOnePVZMinigame;
public class SinkingObject : WorldObject {

	/// <summary>
	/// How fast the object sinks (pixels per second).
	/// </summary>
	protected float DropRate = 20f;

	public SinkingObject(
		Vector2 startPos,
		string textureName,
		float dropRate,
		bool v = false,
		int s = 0
		) : base (
			startPos: startPos,
			textureName: textureName,
			v: v,
			s: s + 1
			)
		{
		Log.Me(() => $"Creating sinking object...", v, s + 1);

		DropRate = dropRate;

		Log.Me(() => "Done!", v, s + 1);
	}

	/// <summary>
	/// Updates the object's position, draws it on the screen,
	/// and marks itself for deletion if it goes off-screen.
	/// </summary>
	public override void Update(bool v = false, int s = 0) {
		// Sink
		Position.Y += DropRate * Raylib.GetFrameTime();

		// Remove if off screen
		if (Position.Y > Raylib.GetScreenHeight()) Despawn(v, s + 1);

		base.Update();
	}
}
