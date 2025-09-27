using System.Numerics;

namespace ThatOnePVZMinigame;
public static class StoreManager {
	public static int Money { get; private set; } = 100;

	public static int WinCost => 5000;

	public enum FishType {
		Cod,
		Snapper,
		JanitorFish,
		CarnivoreFish
	}

	public static void AddMoney(int amount) {
		if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount), "Amount to add cannot be negative.");
		Money += amount;
	}

	public static void PurchaseFish<T>(bool v = false, int s = 0) where T : Fish {
		Log.Me(() => $"Attempting to purchase a {typeof(T).Name}...", v, s + 1);
		int cost = Fish.Cost;


		if (Money < cost) {
			Log.Me(() => $"Not enough money to purchase a {nameof(T)}. Needed: {cost}, Available: {Money}.", v, s + 1);
			return;
		}

		Money -= cost;
		Log.Me(() => $"Purchased a {nameof(T)} for {cost}. Remaining money: {Money}.", v, s + 1);
		WorldManager.SpawnFish<T>(v, s + 1);

		Log.Me(() => "Done!", v, s + 1);
	}


	public static void PurchaseChum<T>(Vector2 startPos, bool v = false, int s = 0) where T : Chum {
		Log.Me(() => $"Attempting to purchase a {typeof(T).Name}...", v, s + 1);
		int cost = Chum.Cost;

		if (Money < cost) {
			Log.Me(() => $"Not enough money to purchase a {nameof(T)}. Needed: {cost}, Available: {Money}.", v, s + 1);
			return;
		}

		Money -= cost;
		Log.Me(() => $"Purchased a {nameof(T)} for {cost}. Remaining money: {Money}.", v, s + 1);
		WorldManager.SpawnChum<T>(startPos, v, s + 1);

		Log.Me(() => "Done!", v, s + 1);
	}


	public static void PurchaseWin(bool v = false, int s = 0) {
		Log.Me(() => "Attempting to purchase a win...", v, s + 1);
		if (Money < WinCost) {
			Log.Me(() => $"Not enough money to purchase a win. Needed: {WinCost}, Available: {Money}.", v, s + 1);
			return;
		}

		Money -= WinCost;
		Log.Me(() => $"Purchased a win for {WinCost}. Remaining money: {Money}.", v, s + 1);

		Master.IsGameOngoing = false;
		Master.EndText = "You win!";

		Log.Me(() => "Done!", v, s + 1);
	}
}
