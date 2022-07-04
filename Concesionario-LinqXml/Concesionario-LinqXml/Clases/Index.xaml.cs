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

namespace Concesionario_LinqXml.Clases
{
    /// <summary>
    /// Lógica de interacción para Index.xaml
    /// </summary>
    public partial class Index : Page
    {
        DataClasses1DataContext dataContext;
        public Index(DataClasses1DataContext dataContext)
        {
            InitializeComponent();
            this.dataContext = dataContext;
        }

        private void Button_Click_Buscador(object sender, RoutedEventArgs e)
        {
            paginaIndex.Content = new Buscar(dataContext);
        }

        private void Button_Click_Insertar(object sender, RoutedEventArgs e)
        {
            paginaIndex.Content = new Insertar(dataContext);
        }

        private void Button_Click_Borrar(object sender, RoutedEventArgs e)
        {
            paginaIndex.Content = new Borrar(dataContext);
        }

        private void Button_Click_Actualizar(object sender, RoutedEventArgs e)
        {
            paginaIndex.Content = new Actualizar(dataContext);
        }

        private void Button_Click_Salir(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
