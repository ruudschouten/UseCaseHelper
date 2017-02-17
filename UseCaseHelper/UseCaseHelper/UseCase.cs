using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCaseHelper {
    public class UseCase {
        public string Naam { get; private set; }
        public string Samenvatting { get; private set; }
        public List<Actor> Actoren { get; private set; } = new List<Actor>();
        public string Aannamen { get; private set; }
        public string Beschrijving { get; private set; }
        public string Uitzonderingen { get; private set; }
        public string Resultaat { get; private set; }
        public Point Position { get; private set; }
        public Rectangle RectanglePos { get; private set; }
        public Pen Pen { get; private set; }

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
            Pen = Pens.Black;
        }

        public void SetPenColor(Pen p) {
            Pen = p;
        }

        public void SetActoren(List<Actor> actoren) {
            Actoren = actoren;
        }

        public void VoegActorToe(Actor a) {
            bool contains = false;
            foreach (var actor in Actoren) {
                if (actor.Naam == a.Naam) {
                    contains = true;
                    break;
                }
            }
            if (!contains) Actoren.Add(a);
        }

        public void VerwijderActorAlsBestaat(Actor a) {
            if (Actoren == null) throw new Exception("Geen actoren gevonden");
            for (var i = 0; i < Actoren.Count; i++) {
                var actor = Actoren[i];
                if (actor.Naam == a.Naam) {
                    Actoren.RemoveAt(i);
                }
            }
        }

        public void PasAanAlsBestaat(Actor a, string prevName) {
            if (Actoren == null) throw new Exception("Geen actoren gevonden");
            for (var i = 0; i < Actoren.Count; i++) {
                var actor = Actoren[i];
                if (actor.Naam == prevName) {
                    Actoren[i] = a;
                }
            }
        }

        public Point GetCenter() {
            return new Point(RectanglePos.Left + RectanglePos.Width / 2, RectanglePos.Top + RectanglePos.Height / 2);
        }

        public void Draw(Graphics g) {
            Font font = new Font("Arial", 14);
            SizeF stringSize = g.MeasureString(Naam, font);
            RectanglePos = new Rectangle(Position, new Size((int)stringSize.Width + 20, (int)stringSize.Height + 20));
            g.FillEllipse(new SolidBrush(Color.White), RectanglePos);
            g.DrawEllipse(Pen, RectanglePos);
            g.DrawString(Naam, font, new SolidBrush(Color.Black), new PointF(RectanglePos.X + 10, RectanglePos.Y + stringSize.Height / 2));
        }

        public string GetActoren() {
            string actoren = "";
            foreach (var actor in Actoren) {
                if (actor != Actoren[Actoren.Count - 1]) {
                    actoren += $"{actor}, ";
                }
                else {
                    actoren += $"{actor}.";
                }
            }
            return actoren;
        }

        public override string ToString() {
            return $"{Naam}";
        }
    }
}
