using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MediaCenter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Media _selectedMedia = null;
        private MCDatabase _MCDB = null;
        public MainWindow()
        {
            InitializeComponent();
            _MCDB = new MCDatabase();
            
            Debug debug = new Debug();
            debug.Show("TEST CONSTRUCTEUR");
            //foreach (DataRow dr in _MCDB.GetDB().Tables["csv"].Rows)
            //{
            //    debug.Show(dr["name"].ToString());
            //}
            //debug.Show();
            //debug.Hide();
            this.MainDataGrid.AutoGenerateColumns = true;

            DataTable DTCSV = _MCDB.GetDB().Tables["csv"];
            DTCSV.Columns[0].ColumnName = "ID";
            DTCSV.Columns[1].ColumnName = "Type";
            DTCSV.Columns[2].ColumnName = "Name";
            DTCSV.Columns[3].ColumnName = "Path";
            DTCSV.Columns[4].ColumnName = "Size";
            DTCSV.Columns[5].ColumnName = "Rating";
            DTCSV.Columns[6].ColumnName = "AudioType";
            DTCSV.Columns[7].ColumnName = "IsHD";
            this.MainDataGrid.DataContext = DTCSV.DefaultView;
            this.MainDataGrid.UpdateLayout();
        }

        private void AddMedia_Click(object sender, RoutedEventArgs e)
        {
            MediaWindow mediaWindow = new MediaWindow();
            mediaWindow.ShowDialog();
            
        }

        private void EditMedia_Click(object sender, RoutedEventArgs e)
        {
            MediaWindow mediaWindow = new MediaWindow(_selectedMedia);
            mediaWindow.ShowDialog();
        }

        private void MainDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGridRow selectedRow = GetSelectedRow();
            Debug debug = new Debug();
            debug.Show("TEST");
            debug.Show("SelectedRow");
            debug.Show(selectedRow.GetValue());
        }
        
        private DataGridRow GetSelectedRow()
        {
            return (DataGridRow)MainDataGrid.ItemContainerGenerator.ContainerFromItem(MainDataGrid.SelectedItem);
        }


    }
}
