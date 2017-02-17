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

        private bool isDeleting;

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
                useCase.Draw(e.Graphics);
            }
            foreach (var actor in actoren) {
                actor.Draw(e.Graphics);
            }
        }

        private void pbCanvas_Click(object sender, EventArgs e) {
            var position = pbCanvas.PointToClient(MousePosition);
            if (isDeleting) {
                DeleteElement(position);
            }
            else {
                if (mode == Mode.Create) {
                    switch (element) {
                        case Element.Actor: CreateActor(position); break;
                        case Element.UseCase: CreateUseCase(position); break;
                        case Element.Line: CreateLine(position); break;
                    }
                }
                else {
                    SelectActor(position);
                    SelectUseCase(position);
                }
            }
            pbCanvas.Invalidate();
        }

        private void SelectUseCase(Point position) {
            for (var i = 0; i < useCases.Count; i++) {
                var useCase = useCases[i];
                if (useCase.RectanglePos.Contains(position)) {
                    useCase.Pen = Pens.Red;
                    pbCanvas.Invalidate();
                    var useCaseForm = new UseCaseCreateForm(useCase);
                    useCaseForm.ShowDialog();
                    useCases[i] = useCaseForm.GetUseCase();
                    useCases[i].Pen = Pens.Black;
                }
            }
        }

        private void SelectActor(Point position) {
            for (var i = 0; i < actoren.Count; i++) {
                var actor = actoren[i];
                if (actor.RectanglePos.Contains(position)) {
                    string prevName = actor.Naam;
                    actor.Pen = Pens.Red;
                    pbCanvas.Invalidate();
                    var actorForm = new ActorCreateForm(actor);
                    actorForm.ShowDialog();
                    actoren[i] = actorForm.GetActor();
                    actoren[i].Pen = Pens.Black;
                    actor = actoren[i];
                    if (actor.Naam != prevName) {
                        foreach (var useCase in useCases) {
                            useCase.PasAanAlsBestaat(actor, prevName);
                        }
                    }
                }
            }
        }

        private void CreateLine(Point position) {
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
                        u.VoegActorToe(Actor.Clone(currentLine.Actor));
                        lines.Add(currentLine);
                        drawingLine = false;
                        break;
                    }
                }
            }
        }

        private void CreateUseCase(Point position) {
            UseCase useCase = new UseCase();
            var useCaseForm = new UseCaseCreateForm(useCase, useCases, position);
            useCaseForm.ShowDialog();
            useCase = useCaseForm.GetUseCase();
            if (!string.IsNullOrEmpty(useCase.Naam)) {
                useCases.Add(useCase);
            }
        }

        private void CreateActor(Point position) {
            Actor actor = new Actor();
            var actorForm = new ActorCreateForm(actor, actoren, position);
            actorForm.ShowDialog();
            actor = actorForm.GetActor();
            if (!string.IsNullOrEmpty(actor.Naam)) {
                actoren.Add(actor);
            }
        }

        private void DeleteElement(Point position) {
            DeleteActor(position);
            DeleteUseCase(position);
        }

        private void DeleteUseCase(Point position) {
            for (var i = 0; i < useCases.Count; i++) {
                var useCase = useCases[i];
                if (useCase.RectanglePos.Contains(position)) {
                    for (var j = 0; j < lines.Count; j++) {
                        var line = lines[j--];
                        if (line.UseCase.Naam == useCase.Naam) {
                            lines.Remove(line);
                        }
                    }
                    useCases.Remove(useCase);
                }
            }
        }

        private void DeleteActor(Point position) {
            for (var i = 0; i < actoren.Count; i++) {
                var actor = actoren[i];
                if (actor.RectanglePos.Contains(position)) {
                    foreach (var useCase in useCases) {
                        useCase.VerwijderActorAlsBestaat(actor);
                    }
                    for (var j = 0; j < lines.Count; j++) {
                        var line = lines[j];
                        if (line.Actor.Naam == actor.Naam) {
                            lines.Remove(line);
                        }
                    }
                    actoren.Remove(actor);
                    break;
                }
            }
        }

        #region Events
        #region RadioButtons

        private void SetCreateMode() {
            rbCreate.Checked = true;
            rbSelect.Checked = false;
            mode = Mode.Create;
        }
        private void rbActor_CheckedChanged(object sender, EventArgs e) {
            if (rbActor.Checked) element = Element.Actor;
            SetCreateMode();
        }

        private void rbUseCase_CheckedChanged(object sender, EventArgs e) {
            if (rbUseCase.Checked) element = Element.UseCase;
            SetCreateMode();
        }

        private void rbLine_CheckedChanged(object sender, EventArgs e) {
            if (rbLine.Checked) element = Element.Line;
            SetCreateMode();
        }

        private void rbCreate_CheckedChanged(object sender, EventArgs e) {
            if (rbCreate.Checked) mode = Mode.Create;
        }

        private void rbSelect_CheckedChanged(object sender, EventArgs e) {
            if (rbSelect.Checked) mode = Mode.Select;
        }
        #endregion
        #region Buttons
        private void btnClearAll_Click(object sender, EventArgs e) {
            actoren.Clear();
            useCases.Clear();
            lines.Clear();
            pbCanvas.Invalidate();
        }

        private void btnDelete_Click(object sender, EventArgs e) {
            if (isDeleting) {
                isDeleting = false;
                btnDelete.Text = "Delete";
            }
            else {
                isDeleting = true;
                btnDelete.Text = "Stop";
            }
        }
        #endregion
        #endregion
    }
}
