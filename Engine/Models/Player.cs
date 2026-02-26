using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Player(string name, string characterClass) : INotifyPropertyChanged
    {
        private string _name = name;
        private string _characterClass = characterClass;
        private int _hitPoints = 100;
        private int _experiencePoints = 0;
        private int _level = 1;
        private int _gold = 1000000;

        public string Name { get { return _name; } set { _name = value; OnPropertyChanged("Name"); } }
        public string CharacterClass { get { return _characterClass; } set { _characterClass = value; OnPropertyChanged("CharacterClass"); } }
        public int HitPoints { get { return _hitPoints; } set { _hitPoints = value; OnPropertyChanged("HitPoints"); } }
        public int ExperiencePoints
        {
            get { return _experiencePoints; }
            set
            {
                _experiencePoints = value;
                OnPropertyChanged("ExperiencePoints");
            }
        }
        public int Level { get { return _level; } set { _level = value; OnPropertyChanged("Level"); } }
        public int Gold { get { return _gold; } set { _gold = value; OnPropertyChanged("Gold"); } }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
