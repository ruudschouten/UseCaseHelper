using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCaseHelper {

    public class Line {
        public Actor Actor { get; set; }
        public UseCase UseCase { get; set; }
        public Point StartPosition { get; set; }
        public Point EndPosition { get; set; }

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
    }
}
