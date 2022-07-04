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
using System.Xml.Linq;

namespace Concesionario_LinqXml.Clases
{
    /// <summary>
    /// Lógica de interacción para Insertar.xaml
    /// </summary>
    public partial class Insertar : Page
    {
        public DataClasses1DataContext dataContext;
        public Vehiculos cocheAInsertar = new Vehiculos();
        public XDocument xml;
        public Insertar(DataClasses1DataContext dataContext)
        {
            InitializeComponent();
            this.dataContext = dataContext;

            xml = XDocument.Load(@"..\..\xml\vehiculos.xml");

            var marcas = xml.Descendants("marca").Select(marca => marca.Attribute("marca"));
            rellenarComboBoxMarca(marcas);
        }

        private void rellenarComboBoxMarca(IEnumerable<XAttribute> marcas)
        {
            comboBoxMarca.Items.Clear();

            foreach (var marca in marcas)
            {
                comboBoxMarca.Items.Add(marca.Value);
            }
        }
        private void comboBoxMarca_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //le pongo la marca selecionada
            if (comboBoxMarca.SelectedItem != null)
            {
                cocheAInsertar.Marca = comboBoxMarca.SelectedItem.ToString();
                //relleno los modelos en base a la marca
                rellenarComboBoxModelo(comboBoxMarca.SelectedItem.ToString());
            }
    
        }
        private void rellenarComboBoxModelo(String marca)
        {
            comboBoxModelo.Items.Clear();

            var modelos = from modelo in xml.Descendants("caracteristicas").Elements("marca")
                          where modelo.Attribute("marca").Value.Equals(marca)
                          select modelo;
            foreach (XElement modelo in modelos.Elements("modelos").Elements("modelo"))
            {
                comboBoxModelo.Items.Add(modelo.Value);
            }

        }
        private void comboBoxModelo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (comboBoxModelo.SelectedItem != null)
            {
                //le pongo el modelo selecionada
                cocheAInsertar.Modelo = comboBoxModelo.SelectedItem.ToString();

                //relleno las versiones en base a el modelo
                rellenarComboBoxVersion(comboBoxMarca.SelectedItem.ToString());
            }
        }
        private void rellenarComboBoxVersion(String marca)
        {
            comboBoxVersion.Items.Clear();

            var versiones = from version in xml.Descendants("caracteristicas").Elements("marca")
                            where version.Attribute("marca").Value.Equals(marca)
                            select version;
            foreach (XElement version in versiones.Elements("versiones").Elements("version"))
            {
                comboBoxVersion.Items.Add(version.Value);
            }
        }
        private void comboBoxVersion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxVersion.SelectedItem != null)
            {
                //le pongo la version selecionada
                cocheAInsertar.Version = comboBoxVersion.SelectedItem.ToString();
            }
        }
        private void Button_Click_Insertar(object sender, RoutedEventArgs e)
        {
            bool datosCorrectos = true;
            //inserto el coche que he ido creando con las caracterisiticas recodigas, y lo muestro en el wrap panel
            //recojo datos introducidos y me aseguro de que no sean nulos

            //marca
            if (comboBoxMarca.SelectedIndex != -1) datosCorrectos = false;
            else datosCorrectos = true;
            //modelo
            if (comboBoxModelo.SelectedIndex != -1) datosCorrectos = false;
            else datosCorrectos = true;
            //version
            if (comboBoxVersion.SelectedIndex != -1) datosCorrectos = false;
            else datosCorrectos = true;
            //precio
            if (tbPrecio.Text != null) cocheAInsertar.Precio = tbPrecio.Text;
            else datosCorrectos = false;
            //foto
            cocheAInsertar.Foto = "img/cocheDefecto.png";
            //combusitble
            if (comboBoxCombustible.SelectedIndex != -1)
            {
                if (comboBoxCombustible.SelectedIndex == 0) cocheAInsertar.Combustible = "Diesel";
                if (comboBoxCombustible.SelectedIndex == 1) cocheAInsertar.Combustible = "Gasolina";
                else datosCorrectos = true;
            }
            else datosCorrectos = false;
            //extras
            if ((bool)checkBoxEx1.IsChecked) cocheAInsertar.Ex1 = checkBoxEx1.Content.ToString();
            if ((bool)checkBoxEx2.IsChecked) cocheAInsertar.Ex2 = checkBoxEx2.Content.ToString();
            if ((bool)checkBoxEx3.IsChecked) cocheAInsertar.Ex3 = checkBoxEx3.Content.ToString();
            if ((bool)checkBoxEx4.IsChecked) cocheAInsertar.Ex4 = checkBoxEx4.Content.ToString();


            if (datosCorrectos)
            {
                try
                {
                    dataContext.Vehiculos.InsertOnSubmit(cocheAInsertar);
                    dataContext.SubmitChanges();

                    //borro todo el contenido de los combobox para volver a insertar
                    comboBoxMarca.Items.Clear();
                    comboBoxModelo.Items.Clear();
                    comboBoxVersion.Items.Clear();
                    comboBoxCombustible.SelectedIndex = -1;
                    tbPrecio.Text = null;
                    checkBoxEx1.IsChecked = false;
                    checkBoxEx2.IsChecked = false;
                    checkBoxEx3.IsChecked = false;
                    checkBoxEx4.IsChecked = false;
                    datosCorrectos = true;

                    //muestro el coche que se ha insertado
                    mostrarCocheInsertado();

                    //proporciono un objeto nuevo para insertar un nuevo coche
                    cocheAInsertar = new Vehiculos();
                }
                catch (System.InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Tienes que rellenar todos los campos obligatorios (marca, modelo, version, precio, combustible)");
            }
        }
        private void Button_Click_Atras(object sender, RoutedEventArgs e)
        {
            paginaInsertar.Content = new Index(dataContext);
        }
        private void mostrarCocheInsertado()
        {
            gridCocheInsertado.Children.Clear();
            try
            {
                //hago una consulta con los datos obtenidos del combobox, y aplico esos valores a los distintos controles del 
                //cada coche que encuentre, lo va a representar mediante un grid que se ira agrupando en un wrap pannel
                Grid gridCoche = new Grid();
                gridCoche.Margin = new Thickness(10);
                //estilos para animacion
                gridCoche.Style = (Style)this.Resources["gridCoche"];

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
                tbMarcaModelo.Text = cocheAInsertar.Marca + " " + cocheAInsertar.Modelo;
                tbMarcaModelo.HorizontalAlignment = HorizontalAlignment.Center;
                tbMarcaModelo.FontSize = 22;
                //precio
                TextBlock tbPrecio = new TextBlock();
                tbPrecio.Text = cocheAInsertar.Precio + "€";
                tbPrecio.HorizontalAlignment = HorizontalAlignment.Center;
                tbPrecio.FontSize = 18;
                //añado todo al stackpanel
                stackPanelMarcaModeloPrecio.Children.Add(tbMarcaModelo);
                stackPanelMarcaModeloPrecio.Children.Add(tbPrecio);
                //imagen
                Image imagen = new Image();
                imagen.Source = new BitmapImage(new Uri(@"/img/cocheDefecto.png", UriKind.Relative));
                Grid.SetRow(imagen, 1);
                //stackpanel version combustible extras
                StackPanel stackPanelVersionCombustibleExtras = new StackPanel();
                Grid.SetRow(stackPanelVersionCombustibleExtras, 3);
                //version
                TextBlock tbVersion = new TextBlock();
                tbVersion.Text = cocheAInsertar.Version;
                tbVersion.HorizontalAlignment = HorizontalAlignment.Center;
                tbVersion.FontSize = 22;
                //combustible
                TextBlock tbCombustible = new TextBlock();
                tbCombustible.Text = cocheAInsertar.Combustible;
                tbCombustible.HorizontalAlignment = HorizontalAlignment.Center;
                tbCombustible.FontSize = 18;
                //añado combustible y precio
                stackPanelVersionCombustibleExtras.Children.Add(tbVersion);
                stackPanelVersionCombustibleExtras.Children.Add(tbCombustible);
                //extras (se van añadiendo en funcion de si hay o no)
                TextBlock tbExtras = new TextBlock();
                if (cocheAInsertar.Ex1 != null)
                {
                    tbExtras.Text = cocheAInsertar.Ex1;
                    tbExtras.HorizontalAlignment = HorizontalAlignment.Center;
                    tbExtras.FontSize = 13;
                    stackPanelVersionCombustibleExtras.Children.Add(tbExtras);
                }
                if (cocheAInsertar.Ex2 != null)
                {
                    tbExtras = new TextBlock();
                    tbExtras.Text = cocheAInsertar.Ex2;
                    tbExtras.HorizontalAlignment = HorizontalAlignment.Center;
                    tbExtras.FontSize = 13;
                    stackPanelVersionCombustibleExtras.Children.Add(tbExtras);
                }
                if (cocheAInsertar.Ex3 != null)
                {
                    tbExtras = new TextBlock();
                    tbExtras.Text = cocheAInsertar.Ex3;
                    tbExtras.HorizontalAlignment = HorizontalAlignment.Center;
                    tbExtras.FontSize = 13;
                    stackPanelVersionCombustibleExtras.Children.Add(tbExtras);
                }
                if (cocheAInsertar.Ex4 != null)
                {
                    tbExtras = new TextBlock();
                    tbExtras.Text = cocheAInsertar.Ex4;
                    tbExtras.HorizontalAlignment = HorizontalAlignment.Center;
                    tbExtras.FontSize = 13;
                    stackPanelVersionCombustibleExtras.Children.Add(tbExtras);
                }

                //añado todos los elementos al wrap panel
                gridCoche.Children.Add(stackPanelMarcaModeloPrecio);
                gridCoche.Children.Add(imagen);
                gridCoche.Children.Add(stackPanelVersionCombustibleExtras);

                //añado todo el grid al wrap panel
                gridCocheInsertado.Children.Add(gridCoche);

                var marcas = xml.Descendants("marca").Select(marca => marca.Attribute("marca"));
                rellenarComboBoxMarca(marcas);

            }
            catch (InvalidOperationException exception)
            {
                MessageBox.Show(exception.Message);
            }

        }
    }
}
