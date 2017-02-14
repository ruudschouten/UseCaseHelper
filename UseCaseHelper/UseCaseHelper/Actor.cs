using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCaseHelper {
    public class Actor {
        public string Naam { get; set; }
        public Point Position { get; set; }

        public Actor() { }

        public Actor(string naam, Point position) {
            Naam = naam;
            Position = position;
        }

        public void Paint(Graphics g) {

        }

        public override string ToString() {
            return Naam;
        }
    }
}
