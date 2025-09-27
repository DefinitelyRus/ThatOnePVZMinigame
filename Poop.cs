using System.Numerics;
namespace ThatOnePVZMinigame;

public class Poop : SinkingObject {

	public float HealAmount { get; private set; } = 25f;

	public Poop(
		Vector2 startPos,
		bool v = false,
		int s = 0
		) : base (
			startPos: startPos,
			textureName: "poop",
			dropRate: 4f,
			v: v,
			s: s + 1
		) {
		Log.Me(() => $"Created poop.", v, s + 1);
	}
}
