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
    /// Lógica de interacción para Actualizar.xaml
    /// </summary>
    public partial class Actualizar : Page
    {
        public DataClasses1DataContext dataContext;
        public String marca, modelo, version;
        public Vehiculos cocheAActualizar = new Vehiculos();
        public String[] campos = new String[10];

        public Actualizar(DataClasses1DataContext dataContext)
        {
            InitializeComponent();
            this.dataContext = dataContext;

            var marcas = dataContext.Vehiculos.Select(x => x.Marca).Distinct();

            rellenarComboBoxMarca(marcas);
        }
        public Actualizar(DataClasses1DataContext dataContext, Vehiculos cocheAActualizar)
        {
            InitializeComponent();
            this.dataContext = dataContext;
            this.cocheAActualizar = cocheAActualizar;


            var marcas = dataContext.Vehiculos.Select(x => x.Marca).Distinct();

            rellenarComboBoxMarca(marcas);

            comboBoxMarca.SelectedItem = cocheAActualizar.Marca;
            comboBoxModelo.SelectedItem = cocheAActualizar.Modelo;
            comboBoxVersion.SelectedItem = cocheAActualizar.Version;

        }
        private void rellenarComboBoxMarca(IQueryable<String> marcas)
        {
            //selecciono una marca, y en funcion de ella ofrezco modelos y versiones

            foreach (var marca in marcas)
            {
                comboBoxMarca.Items.Add(marca);
            }

        }
        private void comboBoxMarca_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //relleno el modelo con el item seleccionado
            marca = (String)comboBoxMarca.SelectedItem;
            rellenarComboBoxModelo(marca);

            //agrego los coches con la marca seleccionada
            if (comboBoxMarca.SelectedItem != null)
            {
                cocheAActualizar.Marca = comboBoxMarca.SelectedItem.ToString();
                agregarCocheAWrapPanelMarca();
            }
        }
        private void rellenarComboBoxModelo(String marca)
        {
            //en funcion de la seleccion de la marca, relleno modelo

            //primero los limpio
            comboBoxModelo.Items.Clear();

            var modelosAgrupadosMarca = from modelos in dataContext.Vehiculos where modelos.Marca == marca group modelos by modelos.Modelo into modelo select modelo;

            foreach (var modelo in modelosAgrupadosMarca)
            {
                comboBoxModelo.Items.Add(modelo.Key);
            }
        }
        private void comboBoxModelo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //relleno la version con el modelo seleccionado
            modelo = (String)comboBoxModelo.SelectedItem;
            rellenarComboBoxVersion(marca);

            //agrego los coches con el modelo seleccionado
            if (comboBoxModelo.SelectedItem != null)
            {
                cocheAActualizar.Modelo = comboBoxModelo.SelectedItem.ToString();
                agregarCocheAWrapPanelMarcaModelo();
            }
        }
        private void rellenarComboBoxVersion(String marca)
        {
            //en funcion de la seleccion del modelo, relleno la version

            //primero los limpio
            comboBoxVersion.Items.Clear();

            var modelosAgrupadosVersion = from modelos in dataContext.Vehiculos where modelos.Marca == marca & modelos.Modelo == modelo group modelos by modelos.Version into modelo select modelo;

            foreach (var version in modelosAgrupadosVersion)
            {
                comboBoxVersion.Items.Add(version.Key);
            }
        }
        private void comboBoxVersion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            version = (String)comboBoxVersion.SelectedItem;

            if(comboBoxVersion.SelectedItem != null)
            {
                //agrego los coches con la version seleccionada
                cocheAActualizar.Version = comboBoxVersion.SelectedItem.ToString();
                agregarCocheAWrapPanelMarcaModeloVersion();
            }
        }
        private void Button_Click_Precio(object sender, RoutedEventArgs e)
        {

        }
        private void Button_Click_Atras(object sender, RoutedEventArgs e)
        {
            paginaActualizar.Content = new Index(dataContext);
        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //cuando el usuario pulse sobre un coche, le muestro la ventana para actualizar
            //detecto el coche pulsado
            Object objGrid = sender as Object;
            Grid miGrid = (Grid)objGrid;

            //separo el uid para obtener elementos
            campos = new String[10];
            campos = miGrid.Uid.Split(',');
            cocheAActualizar.Marca = campos[0];
            cocheAActualizar.Modelo = campos[1];
            cocheAActualizar.Version = campos[2];
            cocheAActualizar.Precio = campos[3];
            cocheAActualizar.Combustible = campos[4];
            cocheAActualizar.Foto = campos[5];
            cocheAActualizar.Ex1 = campos[6];
            cocheAActualizar.Ex2 = campos[7];
            cocheAActualizar.Ex3 = campos[8];
            cocheAActualizar.Ex4 = campos[9];

            //abro la nueva ventana para obtener datos sobre la actualizacion
            paginaActualizar.Content = new InformacionActualizacion(dataContext, cocheAActualizar);

        }
        private void agregarCocheAWrapPanelMarca()
        {
            var coches = this.dataContext.Vehiculos.Where(x => x.Marca == cocheAActualizar.Marca);

            wrapPanel.Children.Clear();

            foreach (var coche in coches)
            {
                try
                {
                    //hago una consulta con los datos obtenidos del combobox, y aplico esos valores a los distintos controles del 
                    //cada coche que encuentre, lo va a representar mediante un grid que se ira agrupando en un wrap pannel
                    Grid gridCoche = new Grid();

                    gridCoche.Margin = new Thickness(10);
                    //estilos para animacion
                    gridCoche.Style = (Style)this.Resources["gridCoche"];
                    gridCoche.MouseDown += new MouseButtonEventHandler(Grid_MouseDown);

                    //filas y columnas
                    ColumnDefinition columna1 = new ColumnDefinition();
                    columna1.Width = new GridLength(320);
                    RowDefinition fila1 = new RowDefinition();
                    RowDefinition fila2 = new RowDefinition();
                    RowDefinition fila3 = new RowDefinition();
                    fila1.Height = new GridLength(60);
                    fila1.Height = new GridLength(270);
                    fila1.Height = new GridLength(60);

                    //adicion filas y columnas
                    gridCoche.ColumnDefinitions.Add(columna1);
                    gridCoche.RowDefinitions.Add(fila1);
                    gridCoche.RowDefinitions.Add(fila2);
                    gridCoche.RowDefinitions.Add(fila3);

                    //elementos del grid
                    //stackpanel marca modelo y precio
                    StackPanel stackPanelMarcaModeloPrecio = new StackPanel();
                    Grid.SetRow(stackPanelMarcaModeloPrecio, 0);
                    //marca y modelo
                    TextBlock tbMarcaModelo = new TextBlock();
                    tbMarcaModelo.Text = coche.Marca + " " + coche.Modelo;
                    tbMarcaModelo.HorizontalAlignment = HorizontalAlignment.Center;
                    tbMarcaModelo.FontSize = 22;
                    //precio
                    TextBlock tbPrecio = new TextBlock();
                    tbPrecio.Text = coche.Precio + "€";
                    tbPrecio.HorizontalAlignment = HorizontalAlignment.Center;
                    tbPrecio.FontSize = 18;
                    //añado todo al stackpanel
                    stackPanelMarcaModeloPrecio.Children.Add(tbMarcaModelo);
                    stackPanelMarcaModeloPrecio.Children.Add(tbPrecio);
                    //imagen
                    Image imagen = new Image();
                    imagen.Name = "img";
                    imagen.Source = new BitmapImage(new Uri(@"/" + coche.Foto, UriKind.Relative));
                    Grid.SetRow(imagen, 1);
                    //stackpanel version combustible
                    StackPanel stackPanelVersionCombustible = new StackPanel();
                    Grid.SetRow(stackPanelVersionCombustible, 3);
                    //version
                    TextBlock tbVersion = new TextBlock();
                    tbVersion.Text = coche.Version;
                    tbVersion.HorizontalAlignment = HorizontalAlignment.Center;
                    tbVersion.FontSize = 22;
                    //combustible
                    TextBlock tbCombustible = new TextBlock();
                    tbCombustible.Text = coche.Combustible;
                    tbCombustible.HorizontalAlignment = HorizontalAlignment.Center;
                    tbCombustible.FontSize = 18;
                    //añado todo al stackpanel
                    stackPanelVersionCombustible.Children.Add(tbVersion);
                    stackPanelVersionCombustible.Children.Add(tbCombustible);

                    //añado todos los elementos al grid
                    gridCoche.Children.Add(stackPanelMarcaModeloPrecio);
                    gridCoche.Children.Add(imagen);
                    gridCoche.Children.Add(stackPanelVersionCombustible);

                    //añado todo el grid al wrap panel
                    wrapPanel.Children.Add(gridCoche);

                    //id que uso par identificar
                    String idCoche = coche.Marca + "," + coche.Modelo
                                     + "," + coche.Version + "," + coche.Precio
                                     + "," + coche.Combustible + "," + coche.Foto + "," + coche.Ex1
                                     + "," + coche.Ex2 + "," + coche.Ex3 + "," + coche.Ex4;
                    gridCoche.Uid = idCoche;


                }
                catch (InvalidOperationException exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }

        }
        private void agregarCocheAWrapPanelMarcaModelo()
        {
            var coches = this.dataContext.Vehiculos.Where(x => x.Marca == cocheAActualizar.Marca & x.Modelo == cocheAActualizar.Modelo);

            wrapPanel.Children.Clear();

            foreach (var coche in coches)
            {
                try
                {
                    //hago una consulta con los datos obtenidos del combobox, y aplico esos valores a los distintos controles del 
                    //cada coche que encuentre, lo va a representar mediante un grid que se ira agrupando en un wrap pannel
                    Grid gridCoche = new Grid();

                    gridCoche.Margin = new Thickness(10);
                    //estilos para animacion
                    gridCoche.Style = (Style)this.Resources["gridCoche"];
                    gridCoche.MouseDown += new MouseButtonEventHandler(Grid_MouseDown);

                    //filas y columnas
                    ColumnDefinition columna1 = new ColumnDefinition();
                    columna1.Width = new GridLength(320);
                    RowDefinition fila1 = new RowDefinition();
                    RowDefinition fila2 = new RowDefinition();
                    RowDefinition fila3 = new RowDefinition();
                    fila1.Height = new GridLength(60);
                    fila1.Height = new GridLength(270);
                    fila1.Height = new GridLength(60);

                    //adicion filas y columnas
                    gridCoche.ColumnDefinitions.Add(columna1);
                    gridCoche.RowDefinitions.Add(fila1);
                    gridCoche.RowDefinitions.Add(fila2);
                    gridCoche.RowDefinitions.Add(fila3);

                    //elementos del grid
                    //stackpanel marca modelo y precio
                    StackPanel stackPanelMarcaModeloPrecio = new StackPanel();
                    Grid.SetRow(stackPanelMarcaModeloPrecio, 0);
                    //marca y modelo
                    TextBlock tbMarcaModelo = new TextBlock();
                    tbMarcaModelo.Text = coche.Marca + " " + coche.Modelo;
                    tbMarcaModelo.HorizontalAlignment = HorizontalAlignment.Center;
                    tbMarcaModelo.FontSize = 22;
                    //precio
                    TextBlock tbPrecio = new TextBlock();
                    tbPrecio.Text = coche.Precio + "€";
                    tbPrecio.HorizontalAlignment = HorizontalAlignment.Center;
                    tbPrecio.FontSize = 18;
                    //añado todo al stackpanel
                    stackPanelMarcaModeloPrecio.Children.Add(tbMarcaModelo);
                    stackPanelMarcaModeloPrecio.Children.Add(tbPrecio);
                    //imagen
                    Image imagen = new Image();
                    imagen.Name = "img";
                    imagen.Source = new BitmapImage(new Uri(@"/" + coche.Foto, UriKind.Relative));
                    Grid.SetRow(imagen, 1);
                    //stackpanel version combustible
                    StackPanel stackPanelVersionCombustible = new StackPanel();
                    Grid.SetRow(stackPanelVersionCombustible, 3);
                    //version
                    TextBlock tbVersion = new TextBlock();
                    tbVersion.Text = coche.Version;
                    tbVersion.HorizontalAlignment = HorizontalAlignment.Center;
                    tbVersion.FontSize = 22;
                    //combustible
                    TextBlock tbCombustible = new TextBlock();
                    tbCombustible.Text = coche.Combustible;
                    tbCombustible.HorizontalAlignment = HorizontalAlignment.Center;
                    tbCombustible.FontSize = 18;
                    //añado todo al stackpanel
                    stackPanelVersionCombustible.Children.Add(tbVersion);
                    stackPanelVersionCombustible.Children.Add(tbCombustible);

                    //añado todos los elementos al wrap panel
                    gridCoche.Children.Add(stackPanelMarcaModeloPrecio);
                    gridCoche.Children.Add(imagen);
                    gridCoche.Children.Add(stackPanelVersionCombustible);

                    //añado todo el grid al wrap panel
                    wrapPanel.Children.Add(gridCoche);

                    //id que uso par identificar
                    String idCoche = coche.Marca + "," + coche.Modelo
                                     + "," + coche.Version + "," + coche.Precio
                                     + "," + coche.Combustible + "," + coche.Foto + "," + coche.Ex1
                                     + "," + coche.Ex2 + "," + coche.Ex3 + "," + coche.Ex4;
                    gridCoche.Uid = idCoche;

                }
                catch (InvalidOperationException exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }

        }
        private void agregarCocheAWrapPanelMarcaModeloVersion()
        {
            var coches = this.dataContext.Vehiculos.Where(x => x.Marca == cocheAActualizar.Marca & x.Modelo == cocheAActualizar.Modelo & x.Version == cocheAActualizar.Version);

            wrapPanel.Children.Clear();

            foreach (var coche in coches)
            {
                try
                {
                    //hago una consulta con los datos obtenidos del combobox, y aplico esos valores a los distintos controles del 
                    //cada coche que encuentre, lo va a representar mediante un grid que se ira agrupando en un wrap pannel
                    Grid gridCoche = new Grid();

                    gridCoche.Margin = new Thickness(10);
                    //estilos para animacion
                    gridCoche.Style = (Style)this.Resources["gridCoche"];
                    gridCoche.MouseDown += new MouseButtonEventHandler(Grid_MouseDown);

                    //filas y columnas
                    ColumnDefinition columna1 = new ColumnDefinition();
                    columna1.Width = new GridLength(320);
                    RowDefinition fila1 = new RowDefinition();
                    RowDefinition fila2 = new RowDefinition();
                    RowDefinition fila3 = new RowDefinition();
                    fila1.Height = new GridLength(60);
                    fila1.Height = new GridLength(270);
                    fila1.Height = new GridLength(60);

                    //adicion filas y columnas
                    gridCoche.ColumnDefinitions.Add(columna1);
                    gridCoche.RowDefinitions.Add(fila1);
                    gridCoche.RowDefinitions.Add(fila2);
                    gridCoche.RowDefinitions.Add(fila3);

                    //elementos del grid
                    //stackpanel marca modelo y precio
                    StackPanel stackPanelMarcaModeloPrecio = new StackPanel();
                    Grid.SetRow(stackPanelMarcaModeloPrecio, 0);
                    //marca y modelo
                    TextBlock tbMarcaModelo = new TextBlock();
                    tbMarcaModelo.Text = coche.Marca + " " + coche.Modelo;
                    tbMarcaModelo.HorizontalAlignment = HorizontalAlignment.Center;
                    tbMarcaModelo.FontSize = 22;
                    //precio
                    TextBlock tbPrecio = new TextBlock();
                    tbPrecio.Text = coche.Precio + "€";
                    tbPrecio.HorizontalAlignment = HorizontalAlignment.Center;
                    tbPrecio.FontSize = 18;
                    //añado todo al stackpanel
                    stackPanelMarcaModeloPrecio.Children.Add(tbMarcaModelo);
                    stackPanelMarcaModeloPrecio.Children.Add(tbPrecio);
                    //imagen
                    Image imagen = new Image();
                    imagen.Name = "img";
                    imagen.Source = new BitmapImage(new Uri(@"/" + coche.Foto, UriKind.Relative));
                    Grid.SetRow(imagen, 1);
                    //stackpanel version combustible
                    StackPanel stackPanelVersionCombustible = new StackPanel();
                    Grid.SetRow(stackPanelVersionCombustible, 3);
                    //version
                    TextBlock tbVersion = new TextBlock();
                    tbVersion.Text = coche.Version;
                    tbVersion.HorizontalAlignment = HorizontalAlignment.Center;
                    tbVersion.FontSize = 22;
                    //combustible
                    TextBlock tbCombustible = new TextBlock();
                    tbCombustible.Text = coche.Combustible;
                    tbCombustible.HorizontalAlignment = HorizontalAlignment.Center;
                    tbCombustible.FontSize = 18;
                    //añado todo al stackpanel
                    stackPanelVersionCombustible.Children.Add(tbVersion);
                    stackPanelVersionCombustible.Children.Add(tbCombustible);

                    //añado todos los elementos al wrap panel
                    gridCoche.Children.Add(stackPanelMarcaModeloPrecio);
                    gridCoche.Children.Add(imagen);
                    gridCoche.Children.Add(stackPanelVersionCombustible);

                    //añado todo el grid al wrap panel
                    wrapPanel.Children.Add(gridCoche);

                    String idCoche = coche.Marca + "," + coche.Modelo
                                     + "," + coche.Version + "," + coche.Precio
                                     + "," + coche.Combustible + "," + coche.Foto + "," + coche.Ex1
                                     + "," + coche.Ex2 + "," + coche.Ex3 + "," + coche.Ex4;
                    gridCoche.Uid = idCoche;

                }
                catch (InvalidOperationException exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }

        }
    }
}
