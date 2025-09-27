# "That one PVZ Minigame" (Insaniquarium)
## How to Play
> This is a simple minigame based on the Zombiquarium minigame from Plants vs. Zombies.
> I know it's also based off Insaniquarium, but I prefer to ripoff the ripoff. \:\)

You start with 100 money and you earn money by feeding your fish and collecting coins they drop periodically.
With the money, you can buy more fish and food to feed them.

Your goal is to earn enough coins to buy the winning item for 5000.
You lose if all your fish dies and you have no more money to buy fish.


## Game Conditions
### Start
You start with 100 money and 1 pre-spawned Cod fish.

### Win
You win the game by buying the "Diamond" item, which costs 5000 money.

### Lose
You lose the game if you run out of money and have no fish left to generate income.


## Controls
### Buy Food
- **Q**: Buy small chum for 15 (Heals 50hp)
- **W**: Buy large chum for 50 (Heals 250hp)

### Buy Fish
- **1**: Buy Cod fish for 150
- **2**: Buy Bass fish for 350
- **3**: Buy Janitorfish for 1000
- **4**: Buy Carnivorefish for 2500
- **5**: Win for 5000.

### Others
- **Left-click**: Collect coin
- (DEBUG ONLY) **Alt + Space + F4 + Enter**: Give 1000 money


## Game AI
All fish slowly deplete health, some faster than others. They heal when they eat.

Whenever a fish eats, they produce poop.

Cod and Bass eat chum. Janitorfishes only eat poop, and Carnivorefishes only eat other fish.

They also produce coins periodically whether or not they eat.
- Cods slowly produce silver coins.
- Bass produces silver coins 75% of the time and gold coins 25% of the time.
- Janitorfish slowly produces only gold coins.
- Carnivorefish quickly produces only gold coins.