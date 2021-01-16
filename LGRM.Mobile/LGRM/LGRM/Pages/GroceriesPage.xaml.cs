using LGRM.Model;
using LGRM.XamF.ViewModels.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LGRM.XamF.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroceriesPage : ContentPage
    {
        public Kind kind;
        public Color colorA1;

        public GroceriesPage()
        {

            InitializeComponent();
            BindingContext = ViewModelLocator.GroceriesVM;

            Application.Current.Resources.TryGetValue("String2HexColor", out var resourceValue);
            var fromHex = (IValueConverter)resourceValue;
            Application.Current.Resources.TryGetValue("StringExists2Visibility", out resourceValue);
            var ToBeVisible = (IValueConverter)resourceValue;
            //Application.Current.Resources.TryGetValue("LocalConverterToHighlightSelections", out resourceValue);
            //var ToBeHighlighted = (IValueConverter)resourceValue;
                        
            //switch (kind)
            //{
            //    case Kind.Lean:
            //        //BindingContext = App.LeansVM;
            //        Title = "Add Leans...";
            //        Application.Current.Resources.TryGetValue("LeansA1", out resourceValue);
            //        colorA1 = (Color)resourceValue;
            //        //Application.Current.Resources.TryGetValue("LeansA2", out resourceValue);
            //        //colorA2 = (Color)resourceValue;
            //        break;
            //    case Kind.Green:
            //        //BindingContext = App.GreensVM;
            //        Title = "Add Greens...";
            //        Application.Current.Resources.TryGetValue("GreensA1", out resourceValue);
            //        colorA1 = (Color)resourceValue;
            //        //Application.Current.Resources.TryGetValue("GreensA2", out resourceValue);
            //        //colorA2 = (Color)resourceValue;
            //        break;
            //    case Kind.HealthyFat:
            //        //BindingContext = App.HealthyFatsVM;
            //        Title = "Add Healthy Fats...";
            //        Application.Current.Resources.TryGetValue("HealthyFatsA1", out resourceValue);
            //        colorA1 = (Color)resourceValue;
            //        //Application.Current.Resources.TryGetValue("HealthyFatsA2", out resourceValue);
            //        //colorA2 = (Color)resourceValue;
            //        break;
            //    case Kind.Condiment:
            //        //BindingContext = App.CondimentsVM;
            //        Title = "Add Condiments...";
            //        Application.Current.Resources.TryGetValue("CondimentsA1", out resourceValue);
            //        colorA1 = (Color)resourceValue;
            //        //Application.Current.Resources.TryGetValue("CondimentsA2", out resourceValue);
            //        //colorA2 = (Color)resourceValue;
            //        break;
            //    default:
            //        break;
            //}








        }
    }
}