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

namespace stanclova_rgb_aplikace
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool loaded = false; //toto není můj nápad, využit princip, co jste nám na hodině ukázala, aby to fungovalo po načtení

        public MainWindow()
        {
            InitializeComponent();
            loaded = true;
        }

        //POMOCNÁ FUNKCE K UPRAVE (ziskam data ze slideru)
        private void UpdateColour()
        {
            byte r = (byte)sliderRed.Value;
            byte g = (byte)sliderGreen.Value;
            byte b = (byte)sliderBlue.Value;

            block.Fill = new SolidColorBrush(Color.FromRgb(r, g, b));

            txtHex.Content = $"#{r:X2}{g:X2}{b:X2}"; //vždy 2 znaky ...hex je hexadecimální zápis
        }



        //TEXT -> SLIDER ... ZMENA
        private void txtRed_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (loaded == false || txtRed.Text == "")
            {
                return;
            }

            if (int.TryParse(txtRed.Text, out int red) == true) //jestli se povede, tak to uloží do intové hodnoty red
            {
                if (red >= 0 && red <= 255)
                {
                    sliderRed.Value = red; //změním slider
                }

                else
                {
                    MessageBox.Show("Zadej číselnou hodnotu v rozmezí 0-255", "Chybná hodnota", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtRed.Text = ((int)sliderRed.Value).ToString();
                }
            }
        }
        private void txtGreen_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (loaded == false || txtGreen.Text == "")
            {
                return;
            }

            if (int.TryParse(txtGreen.Text, out int green) == true) //jestli se povede, tak to uloží do intové hodnoty red
            {
                if (green >= 0 && green <= 255)
                {
                    sliderGreen.Value = green; //změním slider
                }

                else
                {
                    MessageBox.Show("Zadej číselnou hodnotu v rozmezí 0-255", "Chybná hodnota", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtGreen.Text = ((int)sliderGreen.Value).ToString();
                }
            }
        }
        private void txtBlue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (loaded == false || txtBlue.Text == "")
            {
                return;
            }

            if (int.TryParse(txtBlue.Text, out int blue) == true) //jestli se povede, tak to uloží do intové hodnoty red
            {
                if (blue >= 0 && blue <= 255)
                {
                    sliderBlue.Value = blue; //změním slider
                }

                else
                {
                    MessageBox.Show("Zadej číselnou hodnotu v rozmezí 0-255", "Chybná hodnota", MessageBoxButton.OK, MessageBoxImage.Warning);
                    //když to uživatel zadá špatně, tak to nastavím na hodnotu, co má slider, abych tam neměla chybnu nebo neco nahodného
                    //nastavím to tedy na poslední zadanou hodnotu
                    txtBlue.Text = ((int)sliderBlue.Value).ToString();
                }
            }
        }



        //SLIDER -> TEXT ... ZMENA
        private void sliderRed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (loaded == false)
            {
                return;
            } 

            txtRed.Text = ((int)sliderRed.Value).ToString();
            UpdateColour();
        }
        private void sliderGreen_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (loaded == false)
            {
                return;
            }

            txtGreen.Text = ((int)sliderGreen.Value).ToString();
            UpdateColour();
        }
        private void sliderBlue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (loaded == false)
            {
                return;
            }

            txtBlue.Text = ((int)sliderBlue.Value).ToString();
            UpdateColour();
        }
    }
}