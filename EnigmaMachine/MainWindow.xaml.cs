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
using Enigma.Logic;

namespace EnigmaMachine
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        Rotor RightRotor = new Rotor('Z', "BDFHJLCPRTXVZNYEIWGAKMUSQO", 'R');
        Rotor MiddleRotor = new Rotor('A', "AJDKSIRUXBLHWTMCQGZNPYFVOE", 'V');
        Rotor LeftRotor = new Rotor('A', "EKMFLGDQVZNTOWYHXUSPAIBRCJ", 'W');
        Rotor Reflector = new Rotor('B', "YRUHQSLDPXNGOKMIEBFZCWVJAT", '\0');

        bool PartialMode = false;
        int Texti = 0;

        public MainWindow()
        {

            InitializeComponent();

            RightRotor.SetNextRotor(MiddleRotor);
            LeftRotor.SetNextRotor(Reflector);
            LeftRotor.SetPreviousRotor(MiddleRotor);
            MiddleRotor.SetNextRotor(LeftRotor);
            MiddleRotor.SetPreviousRotor(RightRotor);
            Reflector.SetPreviousRotor(LeftRotor);

            Update();


        }

        private void MainLoader(object sender, RoutedEventArgs e)
        {

        }
        //Zamiana wprowadzonego tekstu na tylko wielkie litery
        private void EntryText_Click(object sender, RoutedEventArgs e)
        {
            string input= Text.Text.ToUpper();
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] < 65 || input[i] > 90) { input = input.Remove(i, 1);i--; }
            }
            EnteredText.Text = input;
        }
        //Zaszyfrowanie całego tekstu
        private void Encode_Click(object sender, RoutedEventArgs e)
        {
            if (Texti == 0) Code.Text = "";
            char[] input = EnteredText.Text.ToCharArray();
            while (Texti < input.Length)
            {
                
                RightRotor.Move();
                RightRotor.DataInput(input[Texti]);
                Code.Text+= RightRotor.DataOut();
                Update();
                Texti++;
            }
            Texti = 0;
            PartialMode = false;
            ToggleButtons();
        }
        //Odświeżenie zegara, oraz cykli permutacyjnych
        private void Update()
        {
            if (LetterCheckBox.IsChecked == false)
            {
            RightTB.Text = RightRotor.GetOffset2();
            MiddleTB.Text = MiddleRotor.GetOffset2();
            LeftTB.Text = LeftRotor.GetOffset2();
            }
            else
            {
                RightTB.Text = ((char)(RightRotor.GetOffset()+65)).ToString();
                MiddleTB.Text = ((char)(MiddleRotor.GetOffset()+65)).ToString();
                LeftTB.Text = ((char)(LeftRotor.GetOffset() + 65)).ToString();
            }
            Coding.Content = ShowEncoding();

        }
        //Wyliczenie cykli permutacyjnych
        private string ShowEncoding()
        {
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string result = "";
            while(alphabet.Length>0)
            {
                RightRotor.DataInput(alphabet[0]);
                char otu = RightRotor.DataOut();
                result+= "(" + alphabet[0] + otu + ") ";
                alphabet=alphabet.Remove(0,1);
                alphabet=alphabet.Remove(alphabet.IndexOf(otu), 1);

            }
            return result;
        }

        private void RightInc_Click(object sender, RoutedEventArgs e)
        {
            RightRotor.UpdateOffset(RightRotor.GetOffset() + 1);
            Update();
        }

        private void MiddleInc_Click(object sender, RoutedEventArgs e)
        {
            MiddleRotor.UpdateOffset(MiddleRotor.GetOffset() + 1);
            Update();
        }

        private void LeftInc_Click(object sender, RoutedEventArgs e)
        {
            LeftRotor.UpdateOffset(LeftRotor.GetOffset() + 1);
            Update();
        }

        private void RightDec_Click(object sender, RoutedEventArgs e)
        {
            RightRotor.UpdateOffset(RightRotor.GetOffset() - 1);
            Update();
        }

        private void MiddleDec_Click(object sender, RoutedEventArgs e)
        {
            MiddleRotor.UpdateOffset(MiddleRotor.GetOffset() - 1);
            Update();
        }

        private void LeftDec_Click(object sender, RoutedEventArgs e)
        {
            LeftRotor.UpdateOffset(LeftRotor.GetOffset() - 1);
            Update();
        }

        private void PartialEncode_Click(object sender, RoutedEventArgs e)
        {
            char[] input = EnteredText.Text.ToCharArray();
            if (input.Length == 0) return;
            if (Texti == 0) { Code.Text = ""; PartialMode = true; }
            if (Texti < input.Length)
            {

                RightRotor.Move();
                RightRotor.DataInput(input[Texti]);
                Code.Text += RightRotor.DataOut();
                Update();
                Texti++;
                if (Texti == input.Length)
                {
                    Texti = 0;
                    PartialMode = false;
                }
            }

            ToggleButtons();
        }
        private void ToggleButtons()
        {
            if(PartialMode)
            {
                EntryText.IsEnabled = false;
                ChangeRotors.IsEnabled = false;
                ResetBtn.IsEnabled = false;

                RightInc.IsEnabled = false;
                MiddleInc.IsEnabled = false;
                LeftInc.IsEnabled = false;
                RightDec.IsEnabled = false;
                MiddleDec.IsEnabled = false;
                LeftDec.IsEnabled = false;
            }
            else
            {
                EntryText.IsEnabled = true;
                ChangeRotors.IsEnabled = true;
                ResetBtn.IsEnabled = true;

                RightInc.IsEnabled = true;
                MiddleInc.IsEnabled = true;
                LeftInc.IsEnabled = true;
                RightDec.IsEnabled = true;
                MiddleDec.IsEnabled = true;
                LeftDec.IsEnabled = true;
            }
        }
        //Zamiana litery - liczby na bębnach
        private void LetterCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            RightTB.Text = ((char)(RightRotor.GetOffset() + 65)).ToString();
            MiddleTB.Text = ((char)(MiddleRotor.GetOffset() + 65)).ToString();
            LeftTB.Text = ((char)(LeftRotor.GetOffset() + 65)).ToString();
        }

        private void LetterCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            RightTB.Text = RightRotor.GetOffset2();
            MiddleTB.Text = MiddleRotor.GetOffset2();
            LeftTB.Text = LeftRotor.GetOffset2();
        }
        //Wymiana bębnów
        private void ChangeRotors_Click(object sender, RoutedEventArgs e)
        {
            if(RightCmB.Text!="")
            {
                RightRotorTB.Text = RightCmB.Text;
                RotorsChange(RightRotor, RightCmB.Text);
            }
            if(MiddleCmB.Text!="")
            {
                MiddleRotorTB.Text = MiddleCmB.Text;
                RotorsChange(MiddleRotor, MiddleCmB.Text);
            }
            if(LeftCmB.Text!="")
            {
                LeftRotorTB.Text = LeftCmB.Text;
                RotorsChange(LeftRotor, LeftCmB.Text);
            }

            ResetBtn_Click(sender, e);
        }
        private void RotorsChange(Rotor R,string RotorNumber)
        {
            if(RotorNumber=="I") { R.Updatelayout("EKMFLGDQVZNTOWYHXUSPAIBRCJ"); R.UpdatenotchPosition('R'); }
            if (RotorNumber == "II") { R.Updatelayout("AJDKSIRUXBLHWTMCQGZNPYFVOE"); R.UpdatenotchPosition('F'); }
            if (RotorNumber == "III") { R.Updatelayout("BDFHJLCPRTXVZNYEIWGAKMUSQO"); R.UpdatenotchPosition('W'); }
            if (RotorNumber == "IV") { R.Updatelayout("ESOVPZJAYQUIRHXLNFTGKDCMWB"); R.UpdatenotchPosition('K'); }
            if (RotorNumber == "V") { R.Updatelayout("VZBRGITYUPSDNHLXAWMJQOFECK"); R.UpdatenotchPosition('A'); }
        }
        //Ustawienie bębnów do początkowej pozycji
        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            RightRotor.UpdateOffset(25);
            MiddleRotor.UpdateOffset(0);
            LeftRotor.UpdateOffset(0);
            Update();
        }
    }
}
