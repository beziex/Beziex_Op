using System;
using System.Windows.Forms;
using OpenTK;

namespace Beziex_Op.Views {

    public partial class BxUiScreen : System.Windows.Controls.UserControl {

        public BxUiScreen() {
            InitializeComponent();
        }

        // -------------------------------------------------------

        public GLControl  GLControl() {
            return glControl;
        }

        private void  GLControl_Paint( object sender, PaintEventArgs e )
        {
            ActionGlPaint?.Invoke();
        }

        public Action  ActionGlPaint { get; set; } = null;
    }
}
