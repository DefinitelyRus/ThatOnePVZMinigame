using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThatOnePVZMinigame;
public static class StoreManager {
	public static int Money { get; private set; } = 100;
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

	public static void Purchase<T>(bool v = false, int s = 0) {
		if (!typeof(T).IsSubclassOf(typeof(Fish))) {
			Log.Err(() => $"Cannot purchase fish of type {typeof(T).Name} because it does not inherit from Fish.", v, s + 1);
			return;
		}

		int cost = 0;
		cost = typeof(T).Name switch {
			nameof(Cod) => Cod.Cost,
			_ => int.MaxValue
		};


		if (Money < cost) {
			Log.Me(() => $"Not enough money to purchase a {nameof(T)}. Needed: {cost}, Available: {Money}.", v, s + 1);
			return;
		}

		Money -= cost;
		Log.Me(() => $"Purchased a {nameof(T)} for {cost}. Remaining money: {Money}.", v, s + 1);
		WorldManager.SpawnFish<T>(v, s + 1);

		Log.Me(() => "Done!", v, s + 1);
	}
}
