using System.Numerics;
using Raylib_cs;
namespace ThatOnePVZMinigame;

public abstract class Coin : SinkingObject {
	public int Value { get; protected set; }

	public bool ToRedeem = false;

	public Coin(Vector2 startPos,
		string textureName,
		float dropRate,
		int value,
		bool v = false,
		int s = 0
		) : base (
			startPos: startPos,
			textureName: textureName,
			dropRate: dropRate,
			v: v,
			s: s + 1
			)
		{
		Log.Me(() => $"Creating coin with value {value}...", v, s + 1);
		Value = value;

		Log.Me(() => "Making coin clickable...", v, s + 1);
		InputManager.OnClick += ReceiveClick;

		Log.Me(() => "Done!", v, s + 1);
	}

	/// <summary>
	/// Receives a click event and checks if the coin was clicked. <br/>
	/// If clicked, marks itself for redemption and deletion,
	/// and unsubscribes from further click events.
	/// <br/><br/>
	/// This method is called automatically by the InputManager.
	/// </summary>
	/// <param name="mousePos">
	/// A 2D vector value of the cursor's position when the mouse was clicked.
	/// </param>
	private void ReceiveClick(Vector2 mousePos, bool v = false, int s = 0) {
		Rectangle rect = new(Position.X, Position.Y, Sprite.Width, Sprite.Height);
		bool clickedOn = Raylib.CheckCollisionPointRec(mousePos, rect);

		if (clickedOn) {
			// Mark for redemption and deletion
			Log.Me(() => $"Coin at ({Position.X}, {Position.Y}) clicked! Marking for redemption and deletion...", false, 0);
			ToRedeem = true;
			ToDelete = true;
			InputManager.OnClick -= ReceiveClick;

			// Play random coin sound
			Log.Me(() => "Playing coin sound...", v, s + 1);
			int soundIndex = Raylib.GetRandomValue(1, 2);
			string soundName = $"coin_{soundIndex}";
			ResourceManager.PlaySound(soundName, v, s + 1);

			Log.Me(() => "Done!", v, s + 1);
		}
	}
}

#region Implementations

public class SilverCoin : Coin {
	public SilverCoin(
		Vector2 startPos,
		bool v = false,
		int s = 0
		)
		: base(
			startPos: startPos,
			textureName: "coin_silver",
			dropRate: 30f,
			value: 25,
			v: v,
			s: s + 1
			)
		{
		Log.Me(() => "Created silver coin!", v, s + 1);
	}
}

public class GoldCoin : Coin {
	public GoldCoin(
		Vector2 startPos,
		bool v = false,
		int s = 0
		)
		: base(
			startPos: startPos,
			textureName: "coin_gold",
			dropRate: 20f,
			value: 50,
			v: v,
			s: s + 1
			)
		{
		Log.Me(() => "Created gold coin!", v, s + 1);
	}
}

#endregion