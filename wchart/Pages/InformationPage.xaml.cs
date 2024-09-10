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
using System.Windows.Navigation;
using System.Windows.Shapes;
using wchart.Core;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using Microsoft.Win32;

namespace wchart.Pages
{
    /// <summary>
    /// Логика взаимодействия для InformationPage.xaml
    /// </summary>
    public partial class InformationPage : Page
    {
        public InformationPage()
        {
            InitializeComponent();
            InitializeContent();

            if (ApplicationThemeManager.GetAppTheme() == ApplicationTheme.Dark
                || ApplicationThemeManager.GetAppTheme() == ApplicationTheme.HighContrast)
                ApplyColor(Brushes.White);
            else
                ApplyColor(Brushes.Black);   
            
        }

        private Task ApplyColor(Brush contentFColor)
        {
            windowsLabel.Foreground =
            _windowsLabel.Foreground =
            _officeLabel.Foreground = 
            officeLabel.Foreground = contentFColor;

            return Task.CompletedTask;
        }

        public Task InitializeContent()
        {
            officeLabel.Text = 
                Office.getNameFromVersion(
                    int.Parse(Office.getConfiguration()));

            windowsLabel.Text =
                Environment.OSVersion.VersionString;

            return Task.CompletedTask;
        }

        private void helpButton_Click
            (object sender, RoutedEventArgs e)
        {
            
        }
    }
}
