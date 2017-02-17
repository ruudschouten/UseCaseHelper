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
        private List<UseCase> useCases;
        private Point position;
        private bool editing = false;
        public UseCaseCreateForm() {
            InitializeComponent();
        }

        public UseCaseCreateForm(UseCase u) : this() {
            useCase = u;
            tbNaam.Text = useCase.Naam;
            tbSamenvatting.Text = useCase.Samenvatting;
            useCase.SetActoren(u.Actoren);
            tbActoren.Text = useCase.GetActoren();
            tbAannamen.Text = useCase.Aannamen;
            rtbBeschrijving.Text = useCase.Beschrijving;
            rtbUitzonderingen.Text = useCase.Uitzonderingen;
            tbResultaat.Text = useCase.Resultaat;
            position = useCase.Position;
            editing = true;
        }

        public UseCaseCreateForm(UseCase u, List<UseCase> cases, Point position) : this() {
            useCase = u;
            this.position = position;
            useCases = cases;
        }

        private void UseCaseCreateForm_FormClosing(object sender, FormClosingEventArgs e) {
            e.Cancel = true;
            bool exists = false;
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
                if (editing) {
                    var actoren = useCase.Actoren;
                    useCase = new UseCase(naam, samenvatting, aannamen, beschrijving, uitzonderingen, resultaat, position);
                    useCase.SetActoren(actoren);
                    e.Cancel = false;
                }
                else {
                    if (useCases.Any(c => c.Naam == naam)) {
                        exists = true;
                    }
                    if (exists) {
                        MessageBox.Show("Naam is al in gebruik");
                    }
                    else {
                        var actoren = useCase.Actoren;
                        useCase = new UseCase(naam, samenvatting, aannamen, beschrijving, uitzonderingen, resultaat, position);
                        useCase.SetActoren(actoren);
                        e.Cancel = false;
                    }
                }
            }
        }

        public UseCase GetUseCase() {
            return useCase;
        }

        private void btnClose_Click(object sender, EventArgs e) {
            Close();
        }
    }
}

