using System.Numerics;
namespace ThatOnePVZMinigame;

public abstract class Chum : SinkingObject
{
	/// <summary>
	/// How much health the chum restores when eaten.
	/// </summary>
	public float HealAmount = 0f;

	[Obsolete("This property is never used. Use the Cost property of specific chum implementations instead.", true)]
	/// <summary>
	/// How much the chum costs to buy and spawn.
	/// </summary>
	public static int Cost { get; protected set; } = 0;
	
	public Chum(
		Vector2 startPos,
		string spriteName,
		float dropRate,
		float healAmount,
		bool v = false,
		int s = 0
		) : base(
			startPos: startPos,
			textureName: spriteName,
			dropRate: dropRate,
			v: v,
			s: s + 1
			) {
		Log.Me(() => $"Creating base chum...", v, s + 1);
		HealAmount = healAmount;

		Log.Me(() => "Done!", v, s + 1);
	}
}



#region Implementation

public class SmallChum : Chum
{
	public SmallChum(
		Vector2 startPos,
		bool v = false,
		int s = 0
		) : base(
			startPos: startPos,
			spriteName: "pellet_small",
			dropRate: 30f,
			healAmount: 50f,
			v: v,
			s: s + 1
			) {
		Log.Me(() => $"Creating small chum...", v, s + 1);
	}

	public new static int Cost { get; protected set; } = 15;
}



public class LargeChum : Chum
{
	public LargeChum(
		Vector2 startPos,
		bool v = false,
		int s = 0
		) : base(
			startPos: startPos,
			spriteName: "pellet_big",
			dropRate: 20f,
			healAmount: 250f,
			v: v,
			s: s + 1
			) {
		Log.Me(() => $"Creating large chum...", v, s + 1);
	}

	public new static int Cost { get; protected set; } = 50;
}

#endregion
