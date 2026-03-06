using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Factories;

namespace Engine.Models
{
    public class Location(int xCoordinate, int yCoordinate, string name, string description, string? imageName = null)
    {
        public int XCoordinate { get; set; } = xCoordinate;
        public int YCoordinate { get; set; } = yCoordinate;

        public string Name { get; set; } = name;

        public string Description { get; set; } = description;
        public string? ImageName { get; set; } = imageName;
        public List<Quest> QuestsAvailableHere { get; set; } = new List<Quest>();
        public List<MonsterEncounter> MonstersHere { get; set; } = new List<MonsterEncounter>();

        public void AddMonster(int monsterID, int chanceOfEncountering)
        {
            if (MonstersHere.Exists(m => m.MonsterID == monsterID))
            {
                // This monster has already been added to this location.
                // So, overwrite the chanceOfEncountering with the new number.
                MonstersHere.First(m => m.MonsterID == monsterID).ChanceOfEncountering = chanceOfEncountering;
            }
            else
            {
                MonstersHere.Add(new MonsterEncounter(monsterID, chanceOfEncountering));
            }
        }

        public Monster? GetMonster()
        {
            if (!MonstersHere.Any())
            {
                return null;
            }

            // Total the percentages of all monsters at this location.
            int totalChances = MonstersHere.Sum(m => m.ChanceOfEncountering);

            // Select a random number between 1 and the total (in case the total chances is not 100).
            int randomNumber = RandomNumberGenerator.SimpleNumberBetween(1, totalChances);

            // Loop through the monster list,
            // adding the monster's percentage chance of appearing to the runningTotal variable.
            // that is the monster to return.
            int runningTotal = 0;

            foreach (MonsterEncounter monsterEncounter in MonstersHere)
            {
                runningTotal += monsterEncounter.ChanceOfEncountering;

                if (randomNumber <= runningTotal)
                {
                    return MonsterFactory.GetMonster(monsterEncounter.MonsterID);
                }
            }

            // If there was a problem, return the last monster in the list.
            return MonsterFactory.GetMonster(MonstersHere.Last().MonsterID);
        }
    }
}
