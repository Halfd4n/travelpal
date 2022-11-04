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

namespace TravelPal;
public partial class HowToWindow : Window
{
    public HowToWindow()
    {
        InitializeComponent();
    }

    // Method to close the HowToWindow, called upon via a click on the Take Me There button:
    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}
