using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UseCaseHelper {

    public enum Element {
        Actor,
        UseCase,
        Line
    }

    public enum Mode {
        Create,
        Select
    }

    public partial class Form1 : Form {
        private Element element = Element.Actor;
        private Mode mode = Mode.Create;

        private List<UseCase> useCases = new List<UseCase>();
        private List<Actor> actoren = new List<Actor>();
        private List<Line> lines = new List<Line>();

        private bool drawingLine;
        private Line currentLine;

        public Form1() {
            InitializeComponent();
            DoubleBuffered = true;
            ResizeRedraw = false;
        }

        private void pnCanvas_Paint(object sender, PaintEventArgs e) {
            Pen black = Pens.Black;
            foreach (var line in lines) {
                line.Draw(e.Graphics, black);
            }
            foreach (var useCase in useCases) {
                useCase.Draw(e.Graphics, black);
            }
            foreach (var actor in actoren) {
                actor.Draw(e.Graphics, black);
            }
        }

        private void pbCanvas_Click(object sender, EventArgs e) {
            var position = pbCanvas.PointToClient(MousePosition);
            if (mode == Mode.Create) {
                switch (element) {
                    case Element.Actor:
                        Actor actor = new Actor();
                        var actorForm = new ActorCreateForm(actor, position);
                        actorForm.ShowDialog();
                        actor = actorForm.GetActor();
                        if (!string.IsNullOrEmpty(actor.Naam)) {
                            actoren.Add(actor);
                        }
                        break;
                    case Element.UseCase:
                        UseCase useCase = new UseCase();
                        var useCaseForm = new UseCaseCreateForm(useCase, position);
                        useCaseForm.ShowDialog();
                        useCase = useCaseForm.GetUseCase();
                        if (!string.IsNullOrEmpty(useCase.Naam)) {
                            useCases.Add(useCase);
                        }
                        break;
                    case Element.Line:
                        if (!drawingLine) {
                            //Check if mouse is on Actor
                            foreach (var a in actoren) {
                                if (a.RectanglePos.Contains(position)) {
                                    drawingLine = true;
                                    currentLine = new Line(a);
                                    break;
                                }
                            }
                        }
                        else {
                            //Check if mouse is on Use Case
                            foreach (var u in useCases) {
                                if (u.RectanglePos.Contains(position)) {
                                    drawingLine = true;
                                    currentLine.FinishLine(u);
                                    u.VoegActorToe(currentLine.Actor);
                                    lines.Add(currentLine);
                                    drawingLine = false;
                                    break;
                                }
                            }
                        }
                        break;
                }
            }
            else {
                for (var i = 0; i < actoren.Count; i++) {
                    var actor = actoren[i];
                    if (actor.RectanglePos.Contains(position)) {
                        var actorForm = new ActorCreateForm(actor);
                        actorForm.ShowDialog();
                        actoren[i] = actorForm.GetActor();
                    }
                }
                for (var i = 0; i < useCases.Count; i++) {
                    var useCase = useCases[i];
                    if (useCase.RectanglePos.Contains(position)) {
                        var useCaseForm = new UseCaseCreateForm(useCase);
                        useCaseForm.ShowDialog();
                        useCases[i] = useCaseForm.GetUseCase();
                    }
                }
            }
            pbCanvas.Invalidate();
        }

        #region RadioButtons
        private void rbActor_CheckedChanged(object sender, EventArgs e) {
            if (rbActor.Checked) element = Element.Actor;
        }

        private void rbUseCase_CheckedChanged(object sender, EventArgs e) {
            if (rbUseCase.Checked) element = Element.UseCase;
        }

        private void rbLine_CheckedChanged(object sender, EventArgs e) {
            if (rbLine.Checked) element = Element.Line;
        }

        private void rbCreate_CheckedChanged(object sender, EventArgs e) {
            if (rbCreate.Checked) mode = Mode.Create;
        }

        private void rbSelect_CheckedChanged(object sender, EventArgs e) {
            if (rbSelect.Checked) mode = Mode.Select;
        }
        #endregion

    }
}
