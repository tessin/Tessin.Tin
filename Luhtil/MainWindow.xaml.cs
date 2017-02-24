using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Luhnaris.Framework;

namespace Luhtil
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CompanyType companyType = CompanyType.Any;
        private Gender gender = Gender.Any;
        private string pnr;
        private string onr;
        
        public MainWindow()
        {
            InitializeComponent();
            pnr = LuhnGenerate.GeneratePnr();
            PnrBox.Text = pnr.Insert(6,"-");
            onr = LuhnGenerate.GenerateOnr();
            OrgBox.Text = onr.Insert(6,"-");

            ValidateBox.PreviewTextInput += ValidateBox_TextInput;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void PnrClipButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetDataObject(pnr, true);
        }

        private void OrgClipButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetDataObject(onr, true);
        }

        private void PnrNewButton_Click(object sender, RoutedEventArgs e)
        {
            pnr = LuhnGenerate.GeneratePnr(gender);
            PnrBox.Text = pnr.Insert(6, "-");
        }

        private void OrgNewButton_Click(object sender, RoutedEventArgs e)
        {
            onr = LuhnGenerate.GenerateOnr(companyType);
            OrgBox.Text = onr.Insert(6, "-");
        }

        private string GetCompanyTypeDescription(CompanyType companyType)
        {
            switch (companyType)
            {
                case CompanyType.Aktiebolag: return "Aktiebolag";
                case CompanyType.EkonomiskForening: return "Ekonomisk förening";
                case CompanyType.Enkelt: return "Enkelt bolag" ;
                case CompanyType.IdeelForening: return "Idéel förening eller stiftelse";
                case CompanyType.Other: return "Handelsbolag, kommanditbolag eller enkelt bolag";
                case CompanyType.State: return "Stat, landsting, kommun eller församling";
                case CompanyType.Enskild: return "Enskild firma";
                default: return "Okänd eller ogiltig bolagsform";
            }
        }

        private string GetGenderDescription(Gender gender)
        {
            switch (gender)
            {
                case Gender.Male: return "Man";
                default: return "Kvinna";
            }
        }

        private void PnrBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (pnr == null) return;
            PnrInfoBox.Text = GetGenderDescription(LuhnUtil.GetGender(pnr));
        }

        private void OrgBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (onr == null) return; 
            OrgInfoBox.Text = GetCompanyTypeDescription(LuhnUtil.GetCompanyType(onr));
        }

        private void Window_OnClosing(object sender, CancelEventArgs e)
        {
            Properties.Settings.Default.Top = RestoreBounds.Top;
            Properties.Settings.Default.Left = RestoreBounds.Left;
            Properties.Settings.Default.Save();
        }

        private void OrgGroupRadioButton_Click(object sender, RoutedEventArgs e)
        {
            RadioButton rb = e.OriginalSource as RadioButton;
            if (rb == null) return;
            string tag = rb.Tag as string;
            if (tag == null) return; int ct;
            if (int.TryParse(tag, out ct))
            {
                companyType = (CompanyType) ct;
            }
            else
            {
                companyType = CompanyType.Any;
            }
        }

        private void PnrGroupRadioButton_Click(object sender, RoutedEventArgs e)
        {
            var rb = e.OriginalSource as RadioButton;
            var tag = rb?.Tag as string;
            if (tag == null) return; int value;
            if (int.TryParse(tag, out value))
            {
                gender = (Gender)value;
            }
            else
            {
                gender = Gender.Any;
            }

        }

        private void ExportPnr_Click(object sender, RoutedEventArgs e)
        {
        }


        private void ExportOnr_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ValidateBox_TextInput(object sender, TextCompositionEventArgs e)
        {
            var value = ValidateBox.Text.Trim();
            var validPnr = LuhnValidate.ValidatePnr(value) || LuhnValidate.ValidatePnrCanonical(value);
            if (validPnr)
            {
                ValidateBox.Background = Brushes.PaleGreen;
                ValidateResultBox.Text = "Valid";
                return;
            }

            var validOnr = LuhnValidate.ValidateOnr(value);

            if (validOnr)
            {
                ValidateBox.Background = Brushes.PaleGreen;
                ValidateResultBox.Text = "Valid";
                return;
            }

            ValidateBox.Background = Brushes.PaleVioletRed;
            ValidateResultBox.Text = "Invalid";
        }
    }
}
