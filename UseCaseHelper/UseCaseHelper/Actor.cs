using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCaseHelper {
    public class Actor {
        public string Naam { get; private set; }
        public Point Position { get; private set; }
        public Rectangle RectanglePos { get; private set; }
        public Pen Pen { get; private set; }

        public Actor() { }

        public Actor(string naam, Point position) {
            Naam = naam;
            Position = position;
            Pen = Pens.Black;
        }
        public void SetPenColor(Pen p) {
            Pen = p;
        }

        public static Actor Clone(Actor a) {
            return new Actor(a.Naam, a.Position);
        }
        public Point GetCenter() {
            return new Point(RectanglePos.Left + RectanglePos.Width / 2, RectanglePos.Top + RectanglePos.Height / 2);
        }

        public void Draw(Graphics g) {
            int center;
            Font font = new Font("Arial", 14);
            SizeF stringSize = g.MeasureString(Naam, font);
            RectanglePos = new Rectangle(Position, new Size(32, 64));
            center = RectanglePos.Width / 2;
            g.FillRectangle(new SolidBrush(Color.White), RectanglePos);
            g.DrawRectangle(Pen, RectanglePos);
            g.DrawString(Naam, font, new SolidBrush(Color.Black), new PointF(RectanglePos.X + center - stringSize.Width / 2, RectanglePos.Y + RectanglePos.Height));
        }

        public override string ToString() {
            return Naam;
        }
    }
}
