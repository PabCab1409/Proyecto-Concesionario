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
using System.Collections;
namespace Concesionario_LinqXml.Clases
{
    /// <summary>
    /// Lógica de interacción para Borrar.xaml
    /// </summary>
    public partial class Borrar : Page
    {

        public DataClasses1DataContext dataContext;
        public String marca, modelo, version,precio;
        public Vehiculos cocheABorrar = new Vehiculos();
        public IQueryable<Vehiculos> cochesABorrar;

        public Borrar(DataClasses1DataContext dataContext)
        {
            InitializeComponent();
            this.dataContext = dataContext;

            var marcas = this.dataContext.Vehiculos.Select(x => x.Marca).Distinct();

            rellenarComboBoxMarca(marcas);

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            paginaBorrar.Content = new Index(this.dataContext);
        }
        private void rellenarComboBoxMarca(IQueryable<String> marcas)
        {
            comboBoxMarca.Items.Clear();

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

            //cada vez que haga una seleccion nueva, aplico un nuevo filtro, pero voy reflejando la bsuqueda
            cocheABorrar.Marca = marca;
            agregarCocheAWrapPanelMarca();

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

            //cada vez que haga una seleccion nueva, aplico un nuevo filtro, pero voy reflejando la bsuqueda
            cocheABorrar.Modelo = modelo;
            agregarCocheAWrapPanelMarcaModelo();

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
            //relleno el precio con la version seleccionada
            version = (String)comboBoxVersion.SelectedItem;

            //cada vez que haga una seleccion nueva, aplico un nuevo filtro, pero voy reflejando la bsuqueda
            cocheABorrar.Version = version;
            agregarCocheAWrapPanelMarcaModeloVersion();
        }
        private void Button_Click_Borrar(object sender, RoutedEventArgs e)
        {

            try
            {
                //obtengo el coche que quiero borrar
                Vehiculos coche = this.dataContext.Vehiculos
                    .Where(x => x.Marca == cocheABorrar.Marca 
                    & x.Modelo == cocheABorrar.Modelo 
                    & x.Version == cocheABorrar.Version 
                    & x.Combustible == cocheABorrar.Combustible
                    //& x.Ex1 == cocheABorrar.Ex1
                    //& x.Ex2 == cocheABorrar.Ex2
                    //& x.Ex3 == cocheABorrar.Ex3
                    //& x.Ex4 == cocheABorrar.Ex4
                    ).First(); //TODO BP1P1

                this.dataContext.Vehiculos.DeleteOnSubmit(coche);
                this.dataContext.SubmitChanges();


                //reflejo los cambios
                agregarCocheAWrapPanelMarca();

                //proporciono un objeto nuevo para insertar un nuevo coche
                cocheABorrar = new Vehiculos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

                
        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //cuando el usuario pulse sobre un coche, le pregunto si lo quiere borrar

            //detecto el coche pulsado


            Object objGrid = sender as Object;
            Grid miGrid = (Grid)objGrid;
            MessageBox.Show("Si quieres eliminar el coche seleccionado pulsa el boton borrar");

            //separo el uid para obtener elementos
            String[] campos = new String[10];
            campos = miGrid.Uid.Split(',');
            cocheABorrar.Marca = campos[0];
            cocheABorrar.Modelo = campos[1];
            cocheABorrar.Version = campos[2];
            cocheABorrar.Precio = campos[3];
            cocheABorrar.Combustible = campos[4];
            cocheABorrar.Foto = campos[5];
            cocheABorrar.Ex1 = campos[6];
            cocheABorrar.Ex2 = campos[7];
            cocheABorrar.Ex3 = campos[8];
            cocheABorrar.Ex4 = campos[9];

            //para que la consulta de borrado funcione sin extras
            if (cocheABorrar.Ex1 == "") cocheABorrar.Ex1 = null;
            if (cocheABorrar.Ex2 == "") cocheABorrar.Ex2 = null;
            if (cocheABorrar.Ex3 == "") cocheABorrar.Ex3 = null;
            if (cocheABorrar.Ex4 == "") cocheABorrar.Ex4 = null;


        }
        private void agregarCocheAWrapPanelMarca()
        {
            var coches = this.dataContext.Vehiculos.Where(x => x.Marca == cocheABorrar.Marca);
            cochesABorrar = coches;

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
                    StackPanel stackPanelVersionCombustibleExtras = new StackPanel();
                    Grid.SetRow(stackPanelVersionCombustibleExtras, 3);
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
                    stackPanelVersionCombustibleExtras.Children.Add(tbVersion);
                    stackPanelVersionCombustibleExtras.Children.Add(tbCombustible);
                    //extras
                    TextBlock tbExtras;
                    if (coche.Ex1 != null)
                    {
                        tbExtras = new TextBlock();
                        tbExtras.Text = coche.Ex1;
                        tbExtras.HorizontalAlignment = HorizontalAlignment.Center;
                        tbExtras.FontSize = 13;
                        stackPanelVersionCombustibleExtras.Children.Add(tbExtras);
                    }
                    if (coche.Ex2 != null)
                    {
                        tbExtras = new TextBlock();
                        tbExtras.Text = coche.Ex2;
                        tbExtras.HorizontalAlignment = HorizontalAlignment.Center;
                        tbExtras.FontSize = 13;
                        stackPanelVersionCombustibleExtras.Children.Add(tbExtras);
                    }
                    if (coche.Ex3 != null)
                    {
                        tbExtras = new TextBlock();
                        tbExtras.Text = coche.Ex3;
                        tbExtras.HorizontalAlignment = HorizontalAlignment.Center;
                        tbExtras.FontSize = 13;
                        stackPanelVersionCombustibleExtras.Children.Add(tbExtras);
                    }
                    if (coche.Ex4 != null)
                    {
                        tbExtras = new TextBlock();
                        tbExtras.Text = coche.Ex4;
                        tbExtras.HorizontalAlignment = HorizontalAlignment.Center;
                        tbExtras.FontSize = 13;
                        stackPanelVersionCombustibleExtras.Children.Add(tbExtras);
                    }

                    //añado todos los elementos al wrap panel
                    gridCoche.Children.Add(stackPanelMarcaModeloPrecio);
                    gridCoche.Children.Add(imagen);
                    gridCoche.Children.Add(stackPanelVersionCombustibleExtras);

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
            var coches = this.dataContext.Vehiculos.Where(x => x.Marca == cocheABorrar.Marca & x.Modelo == cocheABorrar.Modelo);

            cochesABorrar = coches;

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
                    StackPanel stackPanelVersionCombustibleExtras = new StackPanel();
                    Grid.SetRow(stackPanelVersionCombustibleExtras, 3);
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
                    stackPanelVersionCombustibleExtras.Children.Add(tbVersion);
                    stackPanelVersionCombustibleExtras.Children.Add(tbCombustible);
                    //extras
                    TextBlock tbExtras;
                    if (coche.Ex1 != null)
                    {
                        tbExtras = new TextBlock();
                        tbExtras.Text = coche.Ex1;
                        tbExtras.HorizontalAlignment = HorizontalAlignment.Center;
                        tbExtras.FontSize = 13;
                        stackPanelVersionCombustibleExtras.Children.Add(tbExtras);
                    }
                    if (coche.Ex2 != null)
                    {
                        tbExtras = new TextBlock();
                        tbExtras.Text = coche.Ex2;
                        tbExtras.HorizontalAlignment = HorizontalAlignment.Center;
                        tbExtras.FontSize = 13;
                        stackPanelVersionCombustibleExtras.Children.Add(tbExtras);
                    }
                    if (coche.Ex3 != null)
                    {
                        tbExtras = new TextBlock();
                        tbExtras.Text = coche.Ex3;
                        tbExtras.HorizontalAlignment = HorizontalAlignment.Center;
                        tbExtras.FontSize = 13;
                        stackPanelVersionCombustibleExtras.Children.Add(tbExtras);
                    }
                    if (coche.Ex4 != null)
                    {
                        tbExtras = new TextBlock();
                        tbExtras.Text = coche.Ex4;
                        tbExtras.HorizontalAlignment = HorizontalAlignment.Center;
                        tbExtras.FontSize = 13;
                        stackPanelVersionCombustibleExtras.Children.Add(tbExtras);
                    }

                    //añado todos los elementos al wrap panel
                    gridCoche.Children.Add(stackPanelMarcaModeloPrecio);
                    gridCoche.Children.Add(imagen);
                    gridCoche.Children.Add(stackPanelVersionCombustibleExtras);

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

            var coches = this.dataContext.Vehiculos.Where(x => x.Marca == cocheABorrar.Marca & x.Modelo == cocheABorrar.Modelo & x.Version == cocheABorrar.Version);
            cochesABorrar = coches;

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
                    StackPanel stackPanelVersionCombustibleExtras = new StackPanel();
                    Grid.SetRow(stackPanelVersionCombustibleExtras, 3);
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
                    stackPanelVersionCombustibleExtras.Children.Add(tbVersion);
                    stackPanelVersionCombustibleExtras.Children.Add(tbCombustible);
                    //extras
                    TextBlock tbExtras;
                    if (coche.Ex1 != null)
                    {
                        tbExtras = new TextBlock();
                        tbExtras.Text = coche.Ex1;
                        tbExtras.HorizontalAlignment = HorizontalAlignment.Center;
                        tbExtras.FontSize = 13;
                        stackPanelVersionCombustibleExtras.Children.Add(tbExtras);
                    }
                    if (coche.Ex2 != null)
                    {
                        tbExtras = new TextBlock();
                        tbExtras.Text = coche.Ex2;
                        tbExtras.HorizontalAlignment = HorizontalAlignment.Center;
                        tbExtras.FontSize = 13;
                        stackPanelVersionCombustibleExtras.Children.Add(tbExtras);
                    }
                    if (coche.Ex3 != null)
                    {
                        tbExtras = new TextBlock();
                        tbExtras.Text = coche.Ex3;
                        tbExtras.HorizontalAlignment = HorizontalAlignment.Center;
                        tbExtras.FontSize = 13;
                        stackPanelVersionCombustibleExtras.Children.Add(tbExtras);
                    }
                    if (coche.Ex4 != null)
                    {
                        tbExtras = new TextBlock();
                        tbExtras.Text = coche.Ex4;
                        tbExtras.HorizontalAlignment = HorizontalAlignment.Center;
                        tbExtras.FontSize = 13;
                        stackPanelVersionCombustibleExtras.Children.Add(tbExtras);
                    }

                    //añado todos los elementos al wrap panel
                    gridCoche.Children.Add(stackPanelMarcaModeloPrecio);
                    gridCoche.Children.Add(imagen);
                    gridCoche.Children.Add(stackPanelVersionCombustibleExtras);

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
    }
}
