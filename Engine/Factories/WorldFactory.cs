using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;

namespace Engine.Factories
{
    internal static class WorldFactory
    {
        internal static World CreateWorld()
        {
            World newWorld = new();

            newWorld.AddLocation(new Location(-2, -1, "Farmer's Field",
                "There are rows of corn growing here, with giant rats hiding between them.",
               "FarmFields.png"));

            newWorld.LocationAt(-2, -1)?.AddMonster(2, 100);

            newWorld.AddLocation(new Location(-1, -1, "Farmer's House",
                "This is the house of your neighbor, Farmer Ted.",
                "Farmhouse.png"));

            newWorld.AddLocation(new Location(0, -1, "Home", "This is your house", "Home.png"));

            newWorld.AddLocation(new Location(-1, 0, "Trading Shop",
                "The shop of Susan, the trader.",
                "Trader.png"));

            newWorld.AddLocation(new Location(0, 0, "Town square",
                "You see a fountain here.",
                "TownSquare.png"));

            newWorld.AddLocation(new Location(1, 0, "Town Gate",
                "There is a gate here, protecting the town from giant spiders.",
                "TownGate.png"));

            newWorld.AddLocation(new Location(2, 0, "Spider Forest",
                "The trees in this forest are covered with spider webs.",
                "SpiderForest.png"));

            newWorld.LocationAt(2, 0)?.AddMonster(3, 100);

            newWorld.AddLocation(new Location(0, 1, "Herbalist's hut",
                "You see a small hut, with plants drying from the roof.",
                "HerbalistsHut.png"));

            Quest? q1 = QuestFactory.GetQuestByID(1);
            if (q1 != null)
            {
                newWorld.LocationAt(0, 1)?.QuestsAvailableHere.Add(q1);
            }

            newWorld.AddLocation(new Location(0, 2, "Herbalist's garden",
                "There are many plants here, with snakes hiding behind them.",
                "HerbalistsGarden.png"));

            newWorld.LocationAt(0, 2)?.AddMonster(1, 100);

            return newWorld;
        }
    }
}
