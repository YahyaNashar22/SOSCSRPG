using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;

namespace Engine.Factories
{
    public static class MonsterFactory
    {
        public static Monster? GetMonster(int monsterID)
        {
            switch (monsterID)
            {
                case 1:
                    Monster snake = new Monster("Snake", "Snake.png", 4, 4, 5, 1);

                    AddLootItem(snake, 9001, 25);
                    AddLootItem(snake, 9002, 75);

                    return snake;

                case 2:
                    Monster rat = new Monster("Rat", "Rat.png", 5, 5, 5, 1);

                    AddLootItem(rat, 9001, 25);
                    AddLootItem(rat, 9002, 75);

                    return rat;

                case 3:
                    Monster spider = new Monster("Spider", "GiantSpider.png", 10, 10, 10, 3);

                    AddLootItem(spider, 9001, 25);
                    AddLootItem(spider, 9002, 75);

                    return spider;

                default:
                    throw new ArgumentException($"Monster type {monsterID} does not exist");
            }


        }

        private static void AddLootItem(Monster monster, int itemID, int percentage)
        {
            if (RandomNumberGenerator.SimpleNumberBetween(1, 100) <= percentage)
            {
                monster.Inventory.Add(new ItemQuantity(itemID, 1));
            }
        }
    }
}
