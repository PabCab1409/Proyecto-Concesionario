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
    /// Lógica de interacción para InformacionCoche.xaml
    /// </summary>
    public partial class InformacionCoche : Page
    {
        public DataClasses1DataContext dataContext;
        public Vehiculos cocheSeleccionado;
        public InformacionCoche(Vehiculos cocheSeleccionado, DataClasses1DataContext dataContext)
        {
            InitializeComponent();

            //relleno la informacion del coche seleccionado de forma dinamica

            this.dataContext = dataContext;
            this.cocheSeleccionado = cocheSeleccionado;

            insertarInformacionDelCoche();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            paginaInformacionCoche.Content = new Buscar(dataContext,cocheSeleccionado);
        }
        private void insertarInformacionDelCoche()
        {
            //cuando abro la pagina buscar, aparece el coche seleccionado con toda su informacion
            Image imagen = new Image();
            imagen.Source = new BitmapImage(new Uri(@"/" + cocheSeleccionado.Foto, UriKind.Relative));
            
            Label labelMarcaModelo = new Label();
            labelMarcaModelo.Content = cocheSeleccionado.Marca + " " + cocheSeleccionado.Modelo;
            labelMarcaModelo.HorizontalAlignment= HorizontalAlignment.Left;

            Label labelVersion = new Label();
            labelVersion.Content = cocheSeleccionado.Version;
            labelVersion.HorizontalAlignment = HorizontalAlignment.Left;

            Label labelPrecio = new Label();
            labelPrecio.Content = cocheSeleccionado.Precio + "€";
            labelPrecio.HorizontalAlignment = HorizontalAlignment.Left;
            labelPrecio.FontSize = 30;

            Label labelCombustible = new Label();
            labelCombustible.Content = cocheSeleccionado.Combustible;
            labelCombustible.HorizontalAlignment = HorizontalAlignment.Left;
            labelCombustible.FontSize = 30;

            panelImagenCoche.Children.Add(imagen);
            stackPanelInfoCoche.Children.Add(labelMarcaModelo);
            stackPanelInfoCoche.Children.Add(labelVersion);
            stackPanelInfoCoche.Children.Add(labelPrecio);
            stackPanelInfoCoche.Children.Add(labelCombustible);

            Label labelExtras = new Label();
            labelExtras.Content = "Extras:";
            labelExtras.HorizontalAlignment = HorizontalAlignment.Center;
            labelExtras.FontSize = 32;
            stackPanelInfoCoche.Children.Add(labelExtras);

            Line line = new Line();
            line.X1 = 300;
            line.Stroke = Brushes.Black;
            line.HorizontalAlignment = HorizontalAlignment.Center;
            stackPanelInfoCoche.Children.Add(line);


            TextBlock tbExtras = new TextBlock();
            tbExtras.HorizontalAlignment = HorizontalAlignment.Center;
            tbExtras.FontSize = 25;
            if(cocheSeleccionado.Ex1!= null)
            {
                tbExtras.Text = "";
                tbExtras.Text = cocheSeleccionado.Ex1;
                stackPanelInfoCoche.Children.Add(tbExtras);
            }
            if (cocheSeleccionado.Ex2 != null)
            {
                tbExtras = new TextBlock();
                tbExtras.HorizontalAlignment = HorizontalAlignment.Center;
                tbExtras.FontSize = 25;
                tbExtras.Text = cocheSeleccionado.Ex2;
                stackPanelInfoCoche.Children.Add(tbExtras);
            }
            if (cocheSeleccionado.Ex3 != null)
            {
                tbExtras = new TextBlock();
                tbExtras.HorizontalAlignment = HorizontalAlignment.Center;
                tbExtras.FontSize = 25;
                tbExtras.Text = cocheSeleccionado.Ex3;
                stackPanelInfoCoche.Children.Add(tbExtras);
            }
            if (cocheSeleccionado.Ex4 != null)
            {
                tbExtras = new TextBlock();
                tbExtras.HorizontalAlignment = HorizontalAlignment.Center;
                tbExtras.FontSize = 25;
                tbExtras.Text = cocheSeleccionado.Ex4;
                stackPanelInfoCoche.Children.Add(tbExtras);
            }

        }

    }
}
