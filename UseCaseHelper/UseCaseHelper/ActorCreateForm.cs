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
    public partial class ActorCreateForm : Form {
        private Actor actor;
        private Point position;
        public ActorCreateForm() {
            InitializeComponent();
            ActiveControl = tbNaam;
        }

        public ActorCreateForm(Actor a) : this() {
            actor = a;
            tbNaam.Text = a.Naam;
            position = actor.Position;
        }

        public ActorCreateForm(Actor a, Point position) : this() {
            actor = a;
            this.position = position;
        }

        private void btnCreate_Click(object sender, EventArgs e) {
            Close();
        }

        private void ActorCreateForm_FormClosing(object sender, FormClosingEventArgs e) {
            e.Cancel = true;
            if (string.IsNullOrEmpty(tbNaam.Text)) {
                var messageResult = MessageBox.Show("Naam is niet ingevuld, wilt u toch afsluiten?", "Onopgeslagen aanpassingein!", MessageBoxButtons.YesNo);
                switch (messageResult) {
                    case DialogResult.None:
                    case DialogResult.No:
                        e.Cancel = true;
                        break;
                    case DialogResult.Yes:
                        e.Cancel = false;
                        break;
                }
            }
            actor = new Actor(tbNaam.Text, position);
            e.Cancel = false;
        }

        public Actor GetActor() {
            return actor;
        }

        private void tbNaam_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyData == Keys.Enter) {
                Close();
            }
        }
    }
}
