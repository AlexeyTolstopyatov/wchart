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
using Wpf.Ui.Controls;

using wchart.Pages;
using wchart.Core;
using Wpf.Ui.Appearance;
using System.Diagnostics;
using Microsoft.Win32;
using System.IO;

namespace wchart
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : FluentWindow
    {
        public Dictionary<string, Page> Charts { get; set; } = new Dictionary<string, Page>();

        public MainWindow()
        {
            InitializeComponent();
            InitializePage();
        }

        /// <summary>
        /// Менеджмент страниц описывается здесь.
        /// Первоначально проверяется файл конфигурации. Если файл существует и версия больше 0
        /// Ядро инициализируется, страница с информацией о программном обеспечении
        /// Отображается.
        /// </summary>
        /// <returns></returns>
        private void InitializePage()
        {
            if (Office.getConfiguration() == Office.nullVersion.Major.ToString())
                OpenButton.IsEnabled = false;

            MainFrame.Content = new InformationPage();
        }

        private Task InitializeChart()
        {
            CardAction chart = new CardAction() {
                Content = App.Document 
            };
            
            // ???
            chart.Click += (object s, RoutedEventArgs e) => {
                MainFrame.Content = Charts.LastOrDefault().Value;
            };

            MainMenu.Children.Add(chart);
            return Task.CompletedTask;
        }

        /// <summary>
        /// "About" page call
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AboutCardActionClicked
            (object s, RoutedEventArgs e)
            => MainFrame.Content = new AboutPage();

        private void MainCardActionClicked
            (object s, RoutedEventArgs e)
            => MainFrame.Content = new InformationPage();

        /// <summary>
        /// Call the wchart.Config.exe
        /// Да простит меня Бог за такой нейминг.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        [Obsolete] // Странно, но ладно. Почитаю, почем уже так.
        private void OpenConfigWizardCardExpanderButtonClicked
            (object s, RoutedEventArgs e)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\wchart.Config.exe"))
                Process.Start(AppDomain.CurrentDomain.BaseDirectory + "\\wchart.Config.exe");
            else
            {
                var err = new Wpf.Ui.Controls.MessageBox() { 
                    Title = "wchart.exe",
                    Content = "Отсутствует wchart.Config.exe",
                    CloseButtonText = "Закрыть",
                };
                
                _ = err.ShowDialogAsync();
            }
        }

        private void OpenDocumentCardExpanderButtonClicked
            (object s, RoutedEventArgs e)
        {
            OpenFileDialog opend = new OpenFileDialog()
            {
                Filter = "Microsoft Word|*.DOCX|Microsoft Word 97-2003|*.DOC",
                DefaultExt = "*.DOCX",
                InitialDirectory = Environment.SpecialFolder.Desktop.ToString()
            };

            if (opend.ShowDialog()!.Value != true)
                return;
            
            App.Document = new FileInfo(opend.FileName).Name;
            Charts.Add(App.Document, new ChartPage());
            _ = InitializeChart();
        }

        private void FreeChartsItemClicked
            (object s, RoutedEventArgs e)
        {
            Charts.Clear();
            MainMenu.Children.Clear();
        }
    }
}