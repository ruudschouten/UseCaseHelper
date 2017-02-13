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
        public UseCaseCreateForm() {
            InitializeComponent();
        }

        public UseCaseCreateForm(UseCase u) : this() {
            useCase = u;
        }
    }
}

