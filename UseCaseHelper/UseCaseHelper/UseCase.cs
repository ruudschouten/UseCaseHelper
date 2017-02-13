using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCaseHelper {
    public class UseCase {
        public string Naam { get; set; }
        public string Samenvatting { get; set; }
        public List<Actor> Actoren { get; set; } = new List<Actor>();
        public string Aannamen { get; set; }
        public string Beschrijving { get; set; }
        public string Uitzonderingen { get; set; }
        public string Resultaat { get; set; }
        public Point Position { get; set; }

        public UseCase() { }

        public UseCase(string naam, string samenvatting, string aannamen, string beschrijving,
            string uitzonderingen, string resultaat, Point position) {
            Naam = naam;
            Samenvatting = samenvatting;
            Aannamen = aannamen;
            Beschrijving = beschrijving;
            Uitzonderingen = uitzonderingen;
            Resultaat = resultaat;
            Position = position;
        }

        public void VoegActorToe(Actor a) {
            Actoren.Add(a);
        }
    }
}
