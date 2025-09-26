using System;
using System.Numerics;
namespace ThatOnePVZMinigame;

public abstract class Chum : SinkingObject
{
	/// <summary>
	/// How much health the chum restores when eaten.
	/// </summary>
	public float HealAmount = 0f;

	/// <summary>
	/// How much the chum costs to buy and spawn.
	/// </summary>
	public int Cost = 0;
	
	public Chum(
		Vector2 startPos,
		string spriteName,
		float dropRate,
		float healAmount,
		int cost = 0,
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
		Cost = cost;
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
			spriteName: "chum_small",
			dropRate: 30f,
			healAmount: 50f,
			cost: 15,
			v: v,
			s: s + 1
			) {
		Log.Me(() => $"Creating small chum...", v, s + 1);
	}
}



public class LargeChum : Chum
{
	public LargeChum(
		Vector2 startPos,
		bool v = false,
		int s = 0
		) : base(
			startPos: startPos,
			spriteName: "chum_large",
			dropRate: 20f,
			healAmount: 250f,
			cost: 50,
			v: v,
			s: s + 1
			) {
		Log.Me(() => $"Creating large chum...", v, s + 1);
	}
}

#endregion
