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
    public partial class UseCaseCreateForm : Form {
        private UseCase useCase;
        private Point position;
        public UseCaseCreateForm() {
            InitializeComponent();
        }

        public UseCaseCreateForm(UseCase u) : this() {
            useCase = u;
            tbNaam.Text = useCase.Naam;
            tbSamenvatting.Text = useCase.Samenvatting;
            tbActoren.Text = useCase.GetActoren();
            tbAannamen.Text = useCase.Aannamen;
            rtbBeschrijving.Text = useCase.Beschrijving;
            rtbUitzonderingen.Text = useCase.Uitzonderingen;
            tbResultaat.Text = useCase.Resultaat;
            position = useCase.Position;
        }

        public UseCaseCreateForm(UseCase u, Point position) : this() {
            useCase = u;
            this.position = position;
        }

        private void UseCaseCreateForm_FormClosing(object sender, FormClosingEventArgs e) {
            e.Cancel = true;
            string naam = tbNaam.Text;
            string samenvatting = tbSamenvatting.Text;
            string aannamen = tbAannamen.Text;
            string beschrijving = rtbBeschrijving.Text;
            string uitzonderingen = rtbUitzonderingen.Text;
            string resultaat = tbResultaat.Text;
            if (string.IsNullOrEmpty(naam) || string.IsNullOrEmpty(samenvatting) || string.IsNullOrEmpty(aannamen) ||
                string.IsNullOrEmpty(beschrijving) || string.IsNullOrEmpty(uitzonderingen) || string.IsNullOrEmpty(resultaat)) {
                var messageResult = MessageBox.Show("Niet alle velden zijn ingevuld, wilt u toch afsluiten?", "Onopgeslagen aanpassingein!", MessageBoxButtons.YesNo);
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
            else {
                useCase = new UseCase(naam, samenvatting, aannamen, beschrijving, uitzonderingen, resultaat, position);
                e.Cancel = false;
            }
        }

        public UseCase GetUseCase() {
            return useCase;
        }
    }
}

