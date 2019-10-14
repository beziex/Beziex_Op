using System.Windows;

namespace Beziex_Op.Views {

    public partial class MainWindow : Window {

        public MainWindow()
        {
            InitializeComponent();
        }

        public void  GetWindowPosSize( out double x, out double y, out double w, out double h )
        {
            x = SystemParameters.WorkArea.Left;
            y = SystemParameters.WorkArea.Top;
            w = SystemParameters.WorkArea.Width;
            h = SystemParameters.WorkArea.Height;
        }

        public void  SetWindowPosSize( double x, double y, double w, double h )
        {
            Left   = x;
            Top    = y;
            Width  = w;
            Height = h;
        }
    }
}
