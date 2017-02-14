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

        public Form1() {
            InitializeComponent();
            DoubleBuffered = true;
            ResizeRedraw = false;
        }

        private void pnCanvas_Paint(object sender, PaintEventArgs e) {
            Pen black = Pens.Black;
            foreach (var useCase in useCases) {
                useCase.Draw(e.Graphics, black);
            }
        }

        private void pbCanvas_Click(object sender, EventArgs e) {
            var position = pbCanvas.PointToClient(MousePosition);
            if (mode == Mode.Create) {
                switch (element) {
                    case Element.Actor:
                        break;
                    case Element.UseCase:
                        UseCase useCase = new UseCase();
                        var useCaseForm = new UseCaseCreateForm(useCase, position);
                        useCaseForm.ShowDialog();
                        useCases.Add(useCaseForm.GetUseCase());
                        break;
                    case Element.Line:
                        break;
                }
            }
            else {
                switch (element) {
                    case Element.Actor:
                        break;
                    case Element.UseCase:
                        break;
                    case Element.Line:
                        break;
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
