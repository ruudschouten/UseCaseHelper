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
            RectanglePos = new Rectangle(Position.X, Position.Y, 32, 64);
            center = RectanglePos.Width / 2;

            //Head
            int headSize = 20;
            Rectangle HeadPosition = new Rectangle(GetCenter().X - headSize/2, GetCenter().Y - headSize, headSize, headSize);
            g.FillEllipse(new SolidBrush(Color.White), HeadPosition);
            g.DrawEllipse(Pen, HeadPosition);
            //Body
            int bodyLength = 20;
            g.DrawLine(Pen, GetCenter().X, GetCenter().Y, GetCenter().X, GetCenter().Y + bodyLength);
            //Arms
            int armLength = 11;
            int armHeightStart = 8;
            int armHeightEnd = 7;
            g.DrawLine(Pen, GetCenter().X - armLength, GetCenter().Y + armHeightStart, GetCenter().X, GetCenter().Y + armHeightEnd);
            g.DrawLine(Pen, GetCenter().X + armLength, GetCenter().Y + armHeightStart, GetCenter().X, GetCenter().Y + armHeightEnd);
            //Legs
            int legLength = 12;
            int legHeightStart = 20;
            int legHeightEnd = 28;
            g.DrawLine(Pen, GetCenter().X - legLength, GetCenter().Y + legHeightEnd, GetCenter().X, GetCenter().Y + legHeightStart);
            g.DrawLine(Pen, GetCenter().X + legLength, GetCenter().Y + legHeightEnd, GetCenter().X, GetCenter().Y + legHeightStart);
            
//            g.DrawRectangle(Pen, RectanglePos);
            g.DrawString(Naam, font, new SolidBrush(Color.Black), new PointF(RectanglePos.X + center - stringSize.Width / 2, RectanglePos.Y + RectanglePos.Height));
        }

        public override string ToString() {
            return Naam;
        }
    }
}
