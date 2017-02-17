using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
        Select,
        Delete,
        DeleteLine
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
        private bool isSelectingLine;
        private Actor lineActor;
        private UseCase lineUseCase;

        public Form1() {
            InitializeComponent();
            DoubleBuffered = true;
            ResizeRedraw = false;
            tssStatus.Text = "Klaar voor gebruik";
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
                switch (mode) {
                    case Mode.Create:
                        switch (element) {
                            case Element.Actor: CreateActor(position); break;
                            case Element.UseCase: CreateUseCase(position); break;
                            case Element.Line: CreateLine(position); break;
                        }
                        break;
                    case Mode.Select:
                        SelectActor(position);
                        SelectUseCase(position);
                        break;
                    case Mode.DeleteLine:
                        SelectLine(position);
                        break;
                }
            }
            pbCanvas.Invalidate();
        }

        private void SelectLine(Point position) {
            if (isSelectingLine) {
                lineUseCase = SelectLineEnd(position);
                if (lineUseCase == null) return;
                isSelectingLine = false;
                for (var i = 0; i < lines.Count; i++) {
                    var line = lines[i];
                    if (line.Actor == lineActor && line.UseCase == lineUseCase) {
                        tssStatus.Text = $"Lijn verwijderd tussen \"{lineActor}\" en \"{lineUseCase}\"";
                        lineUseCase.VerwijderActorAlsBestaat(lineActor);
                        lines.Remove(line);
                        lineUseCase.SetPenColor(Pens.Black);
                        lineActor.SetPenColor(Pens.Black);
                    }
                }
                tssStatus.Text = "Klaar met verwijderen van lijn";
            }
            else {
                lineActor = SelectLineStart(position);
                if (lineActor == null) return;
                tssStatus.Text = "Begonnen met verwijderen van lijn";
                isSelectingLine = true;
            }
        }

        private UseCase SelectLineEnd(Point position) {
            foreach (var useCase in useCases) {
                if (useCase.RectanglePos.Contains(position)) {
                    useCase.SetPenColor(Pens.Red);
                    pbCanvas.Invalidate();
                    tssStatus.Text = $"UseCase \"{useCase}\" geselecteerd voor lijn";
                    return useCase;
                }
            }
            return null;
        }

        private Actor SelectLineStart(Point position) {
            foreach (var actor in actoren) {
                if (actor.RectanglePos.Contains(position)) {
                    actor.SetPenColor(Pens.Red);
                    pbCanvas.Invalidate();
                    tssStatus.Text = $"Actor \"{actor}\" geselecteerd voor lijn";
                    return actor;
                }
            }
            return null;
        }

        private void SelectUseCase(Point position) {
            for (var i = 0; i < useCases.Count; i++) {
                var useCase = useCases[i];
                if (useCase.RectanglePos.Contains(position)) {
                    useCase.SetPenColor(Pens.Red);
                    pbCanvas.Invalidate();
                    tssStatus.Text = $"UseCase \"{useCase}\" geselecteerd";
                    var useCaseForm = new UseCaseCreateForm(useCase);
                    useCaseForm.ShowDialog();
                    useCases[i] = useCaseForm.GetUseCase();
                    useCases[i].SetPenColor(Pens.Black);
                }
            }
        }

        private void SelectActor(Point position) {
            for (var i = 0; i < actoren.Count; i++) {
                var actor = actoren[i];
                if (actor.RectanglePos.Contains(position)) {
                    string prevName = actor.Naam;
                    actor.SetPenColor(Pens.Red);
                    pbCanvas.Invalidate();
                    tssStatus.Text = $"Actor \"{actor}\" geselecteerd";
                    var actorForm = new ActorCreateForm(actor);
                    actorForm.ShowDialog();
                    actoren[i] = actorForm.GetActor();
                    actoren[i].SetPenColor(Pens.Black);
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
                        tssStatus.Text = $"Lijn aan het maken vanaf \"{a}\"";
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
                        tssStatus.Text = $"Lijn gemaakt tussen \"{currentLine.Actor}\" en \"{u}\"";
                        u.VoegActorToe(Actor.Clone(currentLine.Actor));
                        lines.Add(currentLine);
                        drawingLine = false;
                        break;
                    }
                }
            }
        }

        private void CreateUseCase(Point position) {
            tssStatus.Text = "UseCase aan het maken";
            UseCase useCase = new UseCase();
            var useCaseForm = new UseCaseCreateForm(useCase, useCases, position);
            useCaseForm.ShowDialog();
            useCase = useCaseForm.GetUseCase();
            if (!string.IsNullOrEmpty(useCase.Naam)) {
                useCases.Add(useCase);
                tssStatus.Text = $"\"{useCase}\" aangemaakt";
            }
        }

        private void CreateActor(Point position) {
            tssStatus.Text = "Actor aan het maken";
            Actor actor = new Actor();
            var actorForm = new ActorCreateForm(actor, actoren, position);
            actorForm.ShowDialog();
            actor = actorForm.GetActor();
            if (!string.IsNullOrEmpty(actor.Naam)) {
                actoren.Add(actor);
                tssStatus.Text = $"\"{actor}\" aangemaakt";
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

                    tssStatus.Text = $"\"{useCase}\" verwijderd";
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
                    tssStatus.Text = $"\"{actor}\" verwijderd";
                    actoren.Remove(actor);
                    break;
                }
            }
        }

        #region Events
        #region RadioButtons

        private void SetCreateMode() {
            if (rbCreate.Checked) return;
            rbCreate.Checked = true;
            rbSelect.Checked = false;
            tssStatus.Text = "In create mode";
            mode = Mode.Create;
        }
        private void rbActor_CheckedChanged(object sender, EventArgs e) {
            if (rbActor.Checked) element = Element.Actor;
            tssStatus.Text = "Actor geselecteerd";
            SetCreateMode();
        }

        private void rbUseCase_CheckedChanged(object sender, EventArgs e) {
            if (rbUseCase.Checked) element = Element.UseCase;
            tssStatus.Text = "UseCase geselecteerd";
            SetCreateMode();
        }

        private void rbLine_CheckedChanged(object sender, EventArgs e) {
            if (rbLine.Checked) element = Element.Line;
            tssStatus.Text = "Line geselecteerd";
            SetCreateMode();
        }

        private void rbCreate_CheckedChanged(object sender, EventArgs e) {
            if (rbCreate.Checked) mode = Mode.Create;
            tssStatus.Text = "In create mode";
        }

        private void rbSelect_CheckedChanged(object sender, EventArgs e) {
            if (rbSelect.Checked) mode = Mode.Select;
            tssStatus.Text = "In select mode";
        }

        private void rbSelectLine_CheckedChanged(object sender, EventArgs e) {
            if (rbSelectLine.Checked) mode = Mode.DeleteLine;
            tssStatus.Text = "In delete line mode";
        }
        private void rbDelete_CheckedChanged(object sender, EventArgs e) {
            if (rbDelete.Checked) { mode = Mode.Delete; }
            isDeleting = rbDelete.Checked;
            tssStatus.Text = "In delete mode";
        }

        #endregion
        #region Buttons
        private void btnClearAll_Click(object sender, EventArgs e) {
            actoren.Clear();
            useCases.Clear();
            lines.Clear();
            pbCanvas.Invalidate();
            tssStatus.Text = "Canvas gecleared";
        }

        private void btnImage_Click(object sender, EventArgs e) {
            int width = pbCanvas.Size.Width;
            int height = pbCanvas.Size.Height;
            Bitmap bm = new Bitmap(width, height);
            pbCanvas.DrawToBitmap(bm, new Rectangle(0, 0, width, height));
            string filepath = Directory.GetCurrentDirectory() + @"\capture.png";
            bm.Save(filepath);
            ProcessStartInfo startInfo = new ProcessStartInfo(filepath);
            Process.Start(startInfo);
        }
        #endregion

        #endregion
    }
}
