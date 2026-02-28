using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Location(int xCoordinate, int yCoordinate, string name, string description, string? imageName = null)
    {
        public int XCoordinate { get; set; } = xCoordinate;
        public int YCoordinate { get; set; } = yCoordinate;

        public string Name { get; set; } = name;

        public string Description { get; set; } = description;
        public string? ImageName { get; set; } = imageName;
    }
}
