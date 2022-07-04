using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
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
using System.Xml.Linq;
using Concesionario_LinqXml.Clases;
namespace Concesionario_LinqXml.Clases
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public static DataClasses1DataContext dataContext;
        public Window1()
        {
            InitializeComponent();

            //cadena conexion
            String conexion = ConfigurationManager.ConnectionStrings["Concesionario_LinqXml.Properties.Settings.ConcesionarioLinqXmlConnectionString"].ConnectionString;
            dataContext = new DataClasses1DataContext(conexion);

            MainWindow.Content = new Index(dataContext);

        }

    }
}
