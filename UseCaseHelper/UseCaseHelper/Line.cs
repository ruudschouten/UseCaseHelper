using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCaseHelper {

    public class Line {
        public Actor Actor { get; private set; }
        public UseCase UseCase { get; private set; }
        public Point StartPosition { get; private set; }
        public Point EndPosition { get; private set; }
        public Rectangle PositionRectangle { get; private set; }

        public Line(Actor a) {
            Actor = a;
            StartPosition = a.GetCenter();
        }

        public void FinishLine(UseCase u) {
            UseCase = u;
            EndPosition = u.GetCenter();
        }

        public void Draw(Graphics g, Pen p) {
            g.DrawLine(p, StartPosition, EndPosition);
        }

        public override string ToString() {
            return $"Actor: {Actor.Naam}, UseCase {UseCase.Naam}; StartPos: {StartPosition} EndPos: {EndPosition}";
        }
    }
}
