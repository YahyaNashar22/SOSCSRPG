using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Factories;
using System.ComponentModel;

namespace Engine.ViewModels
{
    public class GameSession : INotifyPropertyChanged
    {
        private Location _currentLocation;
        public World CurrentWorld { get; set; }
        public Player CurrentPlayer { get; set; }
        public Location CurrentLocation {
            get { return _currentLocation; } 
            set { 
                _currentLocation = value;
                OnPropertyChanged("CurrentLocation");
                OnPropertyChanged("HasLocationToNorth");
                OnPropertyChanged("HasLocationToEast");
                OnPropertyChanged("HasLocationToSouth");
                OnPropertyChanged("HasLocationToWest");
            }
        }

        public bool HasLocationToNorth { get { return CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1) != null; } }
        public bool HasLocationToEast { get { return CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate ) != null; } }
        public bool HasLocationToSouth { get { return CurrentWorld.LocationAt(CurrentLocation.XCoordinate , CurrentLocation.YCoordinate - 1 ) != null; } }
        public bool HasLocationToWest { get { return CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate) != null; } }

        public GameSession()
        {
             CurrentPlayer = new("Yahya", "Fighter");

            WorldFactory factory = new();
             CurrentWorld = factory.CreateWorld();

            CurrentLocation = CurrentWorld.LocationAt(0, 0)!;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
    }
}
