using System;
using System.Collections;
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
using System.Xml.Linq;
using System.IO;

namespace Concesionario_LinqXml.Clases
{
    /// <summary>
    /// Lógica de interacción para Buscar.xaml
    /// </summary>
    public partial class Buscar : Page
    {
        public XDocument xml;
        public String marca, modelo, version;
        public DataClasses1DataContext dataContext;
        public Boolean buscarMarca, buscarMarcaModelo, buscarMarcaModeloVersion;
        public Grid gridCoche;
        public Vehiculos cocheSeleccionado = new Vehiculos();
        Image imagenFlecha = new Image();
        public bool arriba,precioMarca,precioMarcaModelo, precioMarcaModeloVersion;
        public String ordenacion;

        public Buscar(DataClasses1DataContext dataContext)
        {
            InitializeComponent();
            this.dataContext = dataContext;

            var marcas = dataContext.Vehiculos.Select(x => x.Marca).Distinct();

            rellenarComboBoxMarca(marcas);

            //para que la flecha del precio funcione
            Grid.SetColumn(imagenFlecha, 1);
            imagenFlecha.Width = 30;
            imagenFlecha.Height = 30;
            imagenFlecha.Name = "abajo";

            ordenacion = null;
            precioMarca = false;
            precioMarcaModelo = false;
            precioMarcaModeloVersion = false;

        }
        public Buscar(DataClasses1DataContext dataContext, Vehiculos cocheSeleccionado)
        {
            InitializeComponent();
            this.dataContext = dataContext;
            this.cocheSeleccionado = cocheSeleccionado;

            var marcas = dataContext.Vehiculos.Select(x => x.Marca).Distinct();
            rellenarComboBoxMarca(marcas);

            comboBoxMarca.SelectedItem = cocheSeleccionado.Marca;
            comboBoxModelo.SelectedItem = cocheSeleccionado.Modelo;
            comboBoxVersion.SelectedItem = cocheSeleccionado.Version;

            //para que la flecha del precio funcione
            Grid.SetColumn(imagenFlecha, 1);
            imagenFlecha.Width = 30;
            imagenFlecha.Height = 30;
            imagenFlecha.Name = "abajo";

            ordenacion = null;
            precioMarca = false;
            precioMarcaModelo = false;
            precioMarcaModeloVersion = false;
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

            //filtrado de datos en wrap panel
            if (comboBoxMarca.SelectedItem != null)
                añadirGridMarca();

            //si quiero ordenar por precio, sabré como hacerlo gracias a estas variables
            precioMarca = false;
            precioMarcaModelo = false;
            precioMarcaModeloVersion = false;
            precioMarca = true;
        }       
        private void rellenarComboBoxModelo(String marca)
        {
            //en funcion de la seleccion de la marca, relleno modelo

            //primero los limpio
            comboBoxModelo.Items.Clear();

            var modelosAgrupadosMarca = from modelos in dataContext.Vehiculos where modelos.Marca == marca  group modelos by modelos.Modelo into modelo select modelo;
            
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

            //filtrado de datos en wrap panel
            if (comboBoxModelo.SelectedItem != null)
                añadirGridMarcaModelo();

            //si quiero ordenar por precio, sabré como hacerlo gracias a estas variables
            precioMarca = false;
            precioMarcaModelo = false;
            precioMarcaModeloVersion = false;
            precioMarcaModelo = true;
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

            //filtrado de datos en wrap panel

            if (comboBoxVersion.SelectedItem != null)
                añadirGridMarcaModeloVersion();

            //si quiero ordenar por precio, sabré como hacerlo gracias a estas variables
            precioMarcaModeloVersion = true;
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            paginaBuscar.Content = new Index(dataContext);
        }
        private void Button_Click_Precio(object sender, RoutedEventArgs e)
        {

            if(imagenFlecha.Name == "arriba")
            {
                imagenFlecha.Name = "abajo";

                if(imagenFlecha != null)
                    gridPrecio.Children.Remove(imagenFlecha);

                imagenFlecha.Source = new BitmapImage(new Uri("/img/flechaArriba.png", UriKind.Relative));
                gridPrecio.Children.Add(imagenFlecha);

                ordenacion = "ascendente";
            }else if (imagenFlecha.Name == "abajo")
            {
                imagenFlecha.Name = "arriba";

                if (imagenFlecha != null)
                    gridPrecio.Children.Remove(imagenFlecha);

                imagenFlecha.Source = new BitmapImage(new Uri("/img/flechaAbajo.png", UriKind.Relative));
                gridPrecio.Children.Add(imagenFlecha);

                ordenacion = "descendente";
            }

            //asi sabré en que seccion del comboBox me encuentro
            //if (precioMarca)
            //    añadirGridMarca();
            //else if (precioMarcaModelo)
            //    añadirGridMarcaModelo();
            //else if (precioMarcaModeloVersion)
            //    añadirGridMarcaModeloVersion();

            if (precioMarcaModeloVersion)
                añadirGridMarcaModeloVersion();
            else if (precioMarcaModelo)
                añadirGridMarcaModelo();
            else if (precioMarca)
                añadirGridMarca();

            //ponerlos en false o guardar el modelo

        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Object objGrid = sender as Object;
            Grid miGrid = (Grid)objGrid;
            //MessageBox.Show("Quieres borrar " + miGrid.Uid + "?.\nSi quieres eliminarlo, pulsa el boton borrar.");

            //separo el uid para obtener elementos
            String[] campos = new String[10];
            campos = miGrid.Uid.Split(',');
            cocheSeleccionado.Marca = campos[0];
            cocheSeleccionado.Modelo = campos[1];
            cocheSeleccionado.Version = campos[2];
            cocheSeleccionado.Precio = campos[3];
            cocheSeleccionado.Combustible = campos[4];
            cocheSeleccionado.Foto = campos[5];
            cocheSeleccionado.Ex1 = campos[6];
            cocheSeleccionado.Ex2 = campos[7];
            cocheSeleccionado.Ex3 = campos[8];
            cocheSeleccionado.Ex4 = campos[9];

            //cuando el usuario pulse un coche, le mando a la pagina que le devuelve la informacion completa acerca de este
            paginaBuscar.Content = new InformacionCoche(cocheSeleccionado,dataContext);
        }
        private void añadirGridMarca()
        {

            //elimino lo anterior
            wrapPanel.Children.Clear();

            //esta será la consulta que modifique en funcion del orden del precio
            IQueryable<Vehiculos> coches = null;

            if (ordenacion == "ascendente")
                //hago una consulta con los datos obtenidos del combobox, y aplico esos valores a los distintos controles del grid
                coches = (IQueryable<Vehiculos>)dataContext
                    .Vehiculos
                    .Where(coche => coche.Marca == marca).OrderBy(coche => coche.Precio);
            if (ordenacion == "descendente")
                //hago una consulta con los datos obtenidos del combobox, y aplico esos valores a los distintos controles del grid
                coches = (IQueryable<Vehiculos>)dataContext
                    .Vehiculos
                    .Where(coche => coche.Marca == marca).OrderByDescending(coche => coche.Precio);
            if(ordenacion == null)
                //hago una consulta con los datos obtenidos del combobox, y aplico esos valores a los distintos controles del grid
                coches = (IQueryable<Vehiculos>)dataContext
                    .Vehiculos
                    .Where(coche => coche.Marca == marca);
            
            //añado coches al wrap pannel
            try
            {
                foreach (Vehiculos coche in coches)
                {

                    //cada coche que encuentre, lo va a representar mediante un grid que se ira agrupando en un wrap pannel
                    gridCoche = new Grid();
                    gridCoche.Margin = new Thickness(10);
                    //estilos para animacion
                    gridCoche.Style = (Style)this.Resources["gridCoche"];
                    //detectando click en coche para abrir extras
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

            }
            catch (InvalidOperationException exception)
            {
                MessageBox.Show("No se han encontrado resultados");
            }

        }
        private void añadirGridMarcaModelo()
        {
            //elimino lo anterior
            wrapPanel.Children.Clear();

            //esta será la consulta que modifique en funcion del orden del precio
            IQueryable<Vehiculos> coches = null;

            if (ordenacion == "ascendente")
                //hago una consulta con los datos obtenidos del combobox, y aplico esos valores a los distintos controles del grid
                coches = (IQueryable<Vehiculos>)dataContext
                    .Vehiculos
                    .Where(coche => coche.Marca == marca && coche.Modelo == modelo).OrderBy(coche => coche.Precio);
            if (ordenacion == "descendente")
                //hago una consulta con los datos obtenidos del combobox, y aplico esos valores a los distintos controles del grid
                coches = (IQueryable<Vehiculos>)dataContext
                    .Vehiculos
                    .Where(coche => coche.Marca == marca && coche.Modelo == modelo).OrderByDescending(coche => coche.Precio);
            if (ordenacion == null)
                //hago una consulta con los datos obtenidos del combobox, y aplico esos valores a los distintos controles del grid
                coches = (IQueryable<Vehiculos>)dataContext
                    .Vehiculos
                    .Where(coche => coche.Marca == marca && coche.Modelo == modelo);

            //añado coches al wrap pannel
            try
            {
                foreach (Vehiculos coche in coches)
                {
                    //cada coche que encuentre, lo va a representar mediante un grid que se ira agrupando en un wrap pannel
                    gridCoche = new Grid();
                    gridCoche.Margin = new Thickness(10);
                    //estilos para animacion
                    gridCoche.Style = (Style)this.Resources["gridCoche"];
                    //detectando click en coche para abrir extras
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

            }
            catch (InvalidOperationException exception)
            {
                MessageBox.Show("No se han encontrado resultados");
            }

        }
        private void añadirGridMarcaModeloVersion()
        {

            //elimino lo anterior
            wrapPanel.Children.Clear();

            //esta será la consulta que modifique en funcion del orden del precio
            IQueryable<Vehiculos> coches = null;

            if (ordenacion == "ascendente")
                //hago una consulta con los datos obtenidos del combobox, y aplico esos valores a los distintos controles del grid
                coches = (IQueryable<Vehiculos>)dataContext
                    .Vehiculos
                    .Where(coche => coche.Marca == marca && coche.Modelo == modelo && coche.Version == version).OrderBy(coche => coche.Precio);
            if (ordenacion == "descendente")
                //hago una consulta con los datos obtenidos del combobox, y aplico esos valores a los distintos controles del grid
                coches = (IQueryable<Vehiculos>)dataContext
                    .Vehiculos
                    .Where(coche => coche.Marca == marca && coche.Modelo == modelo && coche.Version == version).OrderByDescending(coche => coche.Precio);
            if (ordenacion == null)
                //hago una consulta con los datos obtenidos del combobox, y aplico esos valores a los distintos controles del grid
                coches = (IQueryable<Vehiculos>)dataContext
                    .Vehiculos
                    .Where(coche => coche.Marca == marca && coche.Modelo == modelo && coche.Version == version);

            //añado coches al wrap pannel
            try
            {
                foreach (Vehiculos coche in coches)
                {
                    //cada coche que encuentre, lo va a representar mediante un grid que se ira agrupando en un wrap pannel
                    gridCoche = new Grid();
                    gridCoche.Margin = new Thickness(10);
                    //estilos para animacion
                    gridCoche.Style = (Style)this.Resources["gridCoche"];
                    //detectando click en coche para abrir extras
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

            }
            catch (InvalidOperationException exception)
            {
                MessageBox.Show("No se han encontrado resultados");
            }
        }

    }
}
