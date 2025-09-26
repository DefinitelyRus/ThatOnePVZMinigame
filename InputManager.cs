using System.Numerics;
using Raylib_cs;

namespace ThatOnePVZMinigame;
internal class InputManager {

	public static Vector2 MousePosition => Raylib.GetMousePosition();
	public static MouseButton InteractKey = MouseButton.Left;
	public static event Action<Vector2>? OnClick;

	public static KeyboardKey BuyChumA = KeyboardKey.Q;
	public static KeyboardKey BuyChumB = KeyboardKey.W;

	public static KeyboardKey BuyFishA = KeyboardKey.One;
	public static KeyboardKey BuyFishB = KeyboardKey.Two;
	public static KeyboardKey BuyFishC = KeyboardKey.Three;
	public static KeyboardKey BuyFishD = KeyboardKey.Four;


	public static void Update(bool v = false, int s = 0) {
		if (Raylib.IsMouseButtonPressed(InteractKey)) OnClick?.Invoke(MousePosition);

		if (Raylib.IsKeyPressed(BuyChumA)) WorldManager.SpawnChum<SmallChum>(MousePosition, v, s + 1);

		if (Raylib.IsKeyPressed(BuyChumB)) WorldManager.SpawnChum<LargeChum>(MousePosition, v, s + 1);

		if (Raylib.IsKeyPressed(BuyFishA)) StoreManager.Purchase<Cod>(v, s + 1);

		//if (Raylib.IsKeyPressed(BuyFishB)) {
		//	StoreManager.Purchase<Snapper>(v, s + 1);
		//}

		//if (Raylib.IsKeyPressed(BuyFishC)) {
		//	StoreManager.Purchase<JanitorFish>(v, s + 1);
		//}

		//if (Raylib.IsKeyPressed(BuyFishD)) {
		//	StoreManager.Purchase<CarnivoreFish>(v, s + 1);
		//}
	}
}
