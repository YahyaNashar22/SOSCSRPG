using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Factories;
using System.ComponentModel;
using Engine.EventArgs;

namespace Engine.ViewModels
{
    public class GameSession : BaseNotificationClass
    {

        public event EventHandler<GameMessageEventArgs> OnMessageRaised;

        #region properties
        private Location _currentLocation;
        private Monster? _currentMonster;
        private Trader? _currentTrader;
        public World CurrentWorld { get; set; }
        public Player CurrentPlayer { get; set; }
        public Location CurrentLocation
        {
            get { return _currentLocation; }
            set
            {
                _currentLocation = value;
                OnPropertyChanged(nameof(CurrentLocation));
                OnPropertyChanged(nameof(HasLocationToNorth));
                OnPropertyChanged(nameof(HasLocationToEast));
                OnPropertyChanged(nameof(HasLocationToSouth));
                OnPropertyChanged(nameof(HasLocationToWest));

                CompleteQuestsAtLocation();
                GivePlayerQuestsAtLocation();
                GetMonsterAtLocation();

                CurrentTrader = CurrentLocation.TraderHere;
            }
        }
        public Monster? CurrentMonster
        {
            get { return _currentMonster; }
            set
            {
                _currentMonster = value;
                OnPropertyChanged(nameof(CurrentMonster));
                OnPropertyChanged(nameof(HasMonster));

                if (CurrentMonster != null)
                {
                    RaiseMessage("");
                    RaiseMessage($"You see a {CurrentMonster.Name} here!");
                }
            }
        }

        public Trader CurrentTrader
        {
            get { return _currentTrader; }
            set
            {
                _currentTrader = value;

                OnPropertyChanged(nameof(CurrentTrader));
                OnPropertyChanged(nameof(HasTrader));
            }
        }

        public Weapon CurrentWeapon { get; set; }


        public bool HasLocationToNorth { get { return CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1) != null; } }
        public bool HasLocationToEast { get { return CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate) != null; } }
        public bool HasLocationToSouth { get { return CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1) != null; } }
        public bool HasLocationToWest { get { return CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate) != null; } }

        public bool HasMonster => CurrentMonster != null;
        public bool HasTrader => CurrentTrader != null;
        #endregion

        public GameSession()
        {
            CurrentPlayer = new("Yahya", "Fighter");

            if (!CurrentPlayer.Weapons.Any())
            {
                CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(1003)!);
            }
            CurrentWorld = WorldFactory.CreateWorld();

            CurrentLocation = CurrentWorld.LocationAt(0, 0)!;
        }

        public void MoveNorth()
        {
            Location? newLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1);
            if (newLocation != null)
            {

                CurrentLocation = newLocation;
            }
        }
        public void MoveEast()
        {
            Location? newLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate);
            if (newLocation != null)
            {

                CurrentLocation = newLocation;
            }
        }
        public void MoveSouth()
        {
            Location? newLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1);
            if (newLocation != null)
            {

                CurrentLocation = newLocation;
            }
        }
        public void MoveWest()
        {
            Location? newLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate);
            if (newLocation != null)
            {
                CurrentLocation = newLocation;
            }
        }

        private void GivePlayerQuestsAtLocation()
        {
            foreach (Quest quest in CurrentLocation.QuestsAvailableHere)
            {
                if (!CurrentPlayer.Quests.Any(q => q.PlayerQuest.ID == quest.ID))
                {
                    CurrentPlayer.Quests.Add(new QuestStatus(quest));

                    RaiseMessage("");
                    RaiseMessage($"You receive the `{quest.Name}` quest");
                    RaiseMessage(quest.Description);

                    RaiseMessage("Return with:");
                    foreach (ItemQuantity itemQuantity in quest.ItemsToComplete)
                    {
                        RaiseMessage($"\t{itemQuantity.Quantity} {ItemFactory.CreateGameItem(itemQuantity.ItemID)?.Name}");
                    }

                    RaiseMessage("And you will receive:");
                    RaiseMessage($"\t {quest.RewardExperiencePoints} experience points");
                    RaiseMessage($"\t{quest.RewardGold} gold");
                    foreach (ItemQuantity itemQuantity in quest.RewardItems)
                    {
                        RaiseMessage($"\t{itemQuantity.Quantity} {ItemFactory.CreateGameItem(itemQuantity.ItemID)}");
                    }
                }
            }
        }

        private void CompleteQuestsAtLocation()
        {
            foreach (Quest quest in CurrentLocation.QuestsAvailableHere)
            {
                QuestStatus? questToComplete = CurrentPlayer.Quests.FirstOrDefault(q => q.PlayerQuest.ID == quest.ID && !q.IsCompleted);

                if (questToComplete != null)
                {
                    foreach (ItemQuantity itemQuantity in quest.ItemsToComplete)
                    {
                        int count = CurrentPlayer.Inventory.Count(i => i.ItemTypeID == itemQuantity.ItemID);
                        RaiseMessage($"Need {itemQuantity.Quantity}, have {count} of item ID {itemQuantity.ItemID}");
                    }
                    if (CurrentPlayer.HasAllTheseItems(quest.ItemsToComplete))
                    {
                        // Remove the quest comletion items from the player's inventory
                        foreach (ItemQuantity itemQuantity in quest.ItemsToComplete)
                        {
                            for (int i = 0; i < itemQuantity.Quantity; i++)
                            {
                                CurrentPlayer.RemoveItemFromInventory(CurrentPlayer.Inventory.First(item => item.ItemTypeID == itemQuantity.ItemID));
                            }
                        }

                        RaiseMessage("");
                        RaiseMessage($"You complete the '{quest.Name}' quest");

                        // Give the player quest rewards
                        CurrentPlayer.ExperiencePoints += quest.RewardExperiencePoints;
                        RaiseMessage($"You receive {quest.RewardExperiencePoints} experience points.");

                        CurrentPlayer.Gold += quest.RewardGold;
                        RaiseMessage($"You receive {quest.RewardGold} gold.");

                        foreach (ItemQuantity itemQuantity in quest.RewardItems)
                        {
                            GameItem rewardItem = ItemFactory.CreateGameItem(itemQuantity.ItemID)!;

                            for (int i = 0; i < itemQuantity.Quantity; i ++)
                            {

                            CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(itemQuantity.ItemID)!);
                            }

                            RaiseMessage($"You receive {itemQuantity.Quantity} {rewardItem.Name}.");
                        }

                        // Mark the Quest as completed
                        questToComplete.IsCompleted = true;
                    }
                    else
                    {
                        RaiseMessage("");
                        RaiseMessage($"Quest '{quest.Name}' is still in progress");
                    }
                }
            }
        }

        private void GetMonsterAtLocation()
        {
            CurrentMonster = CurrentLocation.GetMonster();
        }

        public void AttackCurrentMonster()
        {
            if (CurrentWeapon == null)
            {
                RaiseMessage("You must select a weapon to attack!");
                return;
            }
            if (CurrentMonster == null)
            {
                RaiseMessage("There is no monster here");
                return;
            }
            // Determine damage to monster
            int damageToMonster = RandomNumberGenerator.SimpleNumberBetween(CurrentWeapon.MinimumDamage, CurrentWeapon.MaximumDamage);

            if (damageToMonster == 0)
            {
                RaiseMessage($"You missed the {CurrentMonster.Name}.");
            }
            else
            {
                CurrentMonster.HitPoints -= damageToMonster;
                RaiseMessage($"You hit the {CurrentMonster.Name} for {damageToMonster} points.");
            }

            // If monster is killed, collect rewards and loot
            if (CurrentMonster.HitPoints <= 0)
            {
                RaiseMessage("");
                RaiseMessage($"You defeated the {CurrentMonster.Name}");

                CurrentPlayer.ExperiencePoints += CurrentMonster.RewardExperiencePoints;
                RaiseMessage($"You receive {CurrentMonster.RewardExperiencePoints} experience points.");

                CurrentPlayer.Gold += CurrentMonster.RewardGold;
                RaiseMessage($"You receive {CurrentMonster.RewardGold} gold.");

                foreach (ItemQuantity itemQuantity in CurrentMonster.Inventory)
                {
                    for (int i = 0; i < itemQuantity.Quantity; i++)
                    {
                    GameItem item = ItemFactory.CreateGameItem(itemQuantity.ItemID)!;
                    CurrentPlayer.AddItemToInventory(item);

                    }
                    GameItem displayItem = ItemFactory.CreateGameItem(itemQuantity.ItemID)!;
                    RaiseMessage($"You receive {itemQuantity.Quantity} {displayItem.Name}.");
                }

                // Get another monster to fight
                GetMonsterAtLocation();

            }
            else
            {
                // If monster is still alive, let the monster attack
                int damageToPlayer = RandomNumberGenerator.SimpleNumberBetween(CurrentMonster.MinimumDamage, CurrentMonster.MaximumDamage);

                if (damageToPlayer == 0)
                {
                    RaiseMessage($"{CurrentMonster.Name} missed!");
                }
                else
                {
                    CurrentPlayer.HitPoints -= damageToPlayer;
                    RaiseMessage($"You took {damageToPlayer} damage.");
                }

                // If player is killed, move them back to their home.
                if (CurrentPlayer.HitPoints <= 0)
                {
                    RaiseMessage("");
                    RaiseMessage($"You have been defeated by {CurrentMonster.Name}.");

                    CurrentLocation = CurrentWorld.LocationAt(0, -1)!; // Player's home
                    CurrentPlayer.HitPoints = CurrentPlayer.Level * 10; // Completely heal the player
                }
            }
        }
        private void RaiseMessage(string message)
        {
            OnMessageRaised?.Invoke(this, new GameMessageEventArgs(message));
        }

    }
}
