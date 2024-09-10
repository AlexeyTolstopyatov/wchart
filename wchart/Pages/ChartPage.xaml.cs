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

namespace wchart.Pages
{
    /// <summary>
    /// Логика взаимодействия для ChartPage.xaml
    /// </summary>
    public partial class ChartPage : Page
    {
        public ChartPage()
        {
            InitializeComponent();
            _ = InitializeChart();
        }

        private Task InitializeChart()
        {
           
            // Interop.Microsoft.Office.Interop.Word, Version=8.7.0.0, Culture=neutral, PublicKeyToken=null'. Не удается найти указанный файл
            try
            {
                Word.getTitles(App.Document);
            }
            catch
            {
                var err = new Wpf.Ui.Controls.MessageBox()
                {
                    Title = "wchart.exe",
                    Content = "Отсутствует Microsoft.Office.Interop.Word",
                    CloseButtonText = "Закрыть",
                };
                _ = err.ShowDialogAsync();
            }
            return Task.CompletedTask; 
        }
    }
}
