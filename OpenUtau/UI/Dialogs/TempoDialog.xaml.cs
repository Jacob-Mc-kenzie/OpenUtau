using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Shell;

using WinInterop = System.Windows.Interop;
using System.Runtime.InteropServices;
using Microsoft.Win32;

using OpenUtau.UI.Models;
using OpenUtau.UI.Controls;
using OpenUtau.Core;
using OpenUtau.Core.USTx;

namespace OpenUtau.UI.Dialogs {
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class TempoDialog : Window {
        public TempoDialog() {
            InitializeComponent();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e) {
            DocManager.Inst.Project.BPM = Convert.ToDouble(this.BPM.Text);
            this.Close();
        }

        private void Confirm_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                DocManager.Inst.Project.BPM = Convert.ToDouble(this.BPM.Text);
                this.Close();
            }
        }
    }
}
