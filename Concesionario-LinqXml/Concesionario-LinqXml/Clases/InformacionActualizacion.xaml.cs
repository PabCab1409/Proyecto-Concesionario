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

namespace Concesionario_LinqXml.Clases
{
    /// <summary>
    /// Lógica de interacción para InformacionActualizacion.xaml
    /// </summary>
    public partial class InformacionActualizacion : Page
    {
        public DataClasses1DataContext dataContext;
        public Vehiculos cocheAActualizar;
        public String marca, modelo, version;

        public InformacionActualizacion(DataClasses1DataContext dataContext,Vehiculos cocheAAactualizar)
        {
            InitializeComponent();
            this.dataContext = dataContext;
            this.cocheAActualizar = cocheAAactualizar;

            insertarInformacionDelCoche();

            var marcas = dataContext.Vehiculos.Select(x => x.Marca).Distinct();
            rellenarComboBoxMarca(marcas);
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

            if (comboBoxVersion.SelectedItem != null)
            {
                //agrego los coches con la version seleccionada
                cocheAActualizar.Version = comboBoxVersion.SelectedItem.ToString();
            }
        }
        private void Button_Click_Actualizar(object sender, RoutedEventArgs e)
        {
            
            try
            {
                var coche = dataContext.Vehiculos
                    .Where(x=>
                    x.Marca == cocheAActualizar.Marca 
                    & x.Modelo == cocheAActualizar.Modelo 
                    & x.Version == cocheAActualizar.Version 
                    & x.Precio == cocheAActualizar.Precio 
                    & x.Combustible == cocheAActualizar.Combustible
                    //& x.Ex1 == cocheAActualizar.Ex1
                    //& x.Ex2 == cocheAActualizar.Ex2
                    //& x.Ex3 == cocheAActualizar.Ex3
                    //& x.Ex4 == cocheAActualizar.Ex4
                    ).First();

                //obtengo lo que haya en los controles, y se lo aplico al coche
                coche.Marca = comboBoxMarca.SelectedItem.ToString();
                coche.Modelo = comboBoxModelo.SelectedItem.ToString();
                coche.Version = comboBoxVersion.SelectedItem.ToString();
                coche.Precio = tbPrecio.Text;
                if (comboBoxCombustible.SelectedIndex == 0) coche.Combustible = "Diesel";
                if (comboBoxCombustible.SelectedIndex == 1) coche.Combustible = "Gasolina";
                if (checkBoxEx1 != null) coche.Ex1 = checkBoxEx1.Content.ToString();
                if (checkBoxEx2 != null) coche.Ex2 = checkBoxEx2.Content.ToString();
                if (checkBoxEx3 != null) coche.Ex3 = checkBoxEx3.Content.ToString();
                if (checkBoxEx4 != null) coche.Ex4 = checkBoxEx4.Content.ToString();
                
                dataContext.SubmitChanges();
                paginaActualizacionInformacion.Content = new Actualizar(dataContext,coche);
                cocheAActualizar = new Vehiculos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Button_Click_Atras(object sender, RoutedEventArgs e)
        {
            paginaActualizacionInformacion.Content = new Actualizar(dataContext, cocheAActualizar);
        }
        private void insertarInformacionDelCoche()
        {
            //cuando abro la ventana actualizar, aparece el coche seleccionado con su marca en el comboBox,foto en el grid etc
            Image imagen = new Image();
            imagen.Source = new BitmapImage(new Uri(@"/" + cocheAActualizar.Foto, UriKind.Relative));
            gridImagen.Children.Add(imagen);
            comboBoxMarca.SelectedItem = cocheAActualizar.Marca;
            comboBoxModelo.SelectedItem = cocheAActualizar.Modelo;
            comboBoxVersion.SelectedItem = cocheAActualizar.Version;
            tbPrecio.Text = cocheAActualizar.Precio;
            if (cocheAActualizar.Combustible.Equals("Diesel", StringComparison.OrdinalIgnoreCase)) comboBoxCombustible.SelectedIndex = 0;
            if (cocheAActualizar.Combustible.Equals("Gasolina", StringComparison.OrdinalIgnoreCase)) comboBoxCombustible.SelectedIndex = 1;
            if (cocheAActualizar.Ex1 == "Llantas 19 pulgadas") checkBoxEx1.IsChecked = true;
            if(cocheAActualizar.Ex2 == "Asientos de cuero") checkBoxEx2.IsChecked = true;
            if(cocheAActualizar.Ex3 == "Faros led ") checkBoxEx3.IsChecked = true;
            if(cocheAActualizar.Ex4 == "Porton") checkBoxEx4.IsChecked = true;
        }
    }
}
