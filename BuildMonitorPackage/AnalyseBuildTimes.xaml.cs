using System;
using System.Collections.Generic;
using System.Dynamic;
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

namespace BuildMonitorPackage
{
    /// <summary>
    /// Interaction logic for AnalyseBuildTimes.xaml
    /// </summary>
    public partial class AnalyseBuildTimes : Window
    {
        public AnalyseBuildTimes()
        {
            InitializeComponent();
        }

        public AnalyseBuildTimes(IEnumerable<ExpandoObject> solutionMonthTable) : this()
        {
            // you would have thought it would be easier to show a grid of data in 2016 ...
            SolutionMonthDataGrid.ItemsSource = solutionMonthTable;

            var rows = solutionMonthTable.OfType<IDictionary<string, object>>();
            var columns = rows.SelectMany(d => d.Keys).Distinct(StringComparer.OrdinalIgnoreCase);

            foreach (string text in columns)
            {
                // now set up a column and binding for each property
                var column = new DataGridTextColumn
                {
                    Header = text,
                    Binding = new Binding(text)
                };

                SolutionMonthDataGrid.Columns.Add(column);
            }

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
