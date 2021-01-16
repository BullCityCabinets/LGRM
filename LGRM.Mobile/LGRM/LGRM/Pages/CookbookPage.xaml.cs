using LGRM.XamF.ViewModels;
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
    public partial class CookbookPage : ContentPage
    {
        public CookbookPage()
        {            
            InitializeComponent();
         
            this.BindingContext = ViewModelLocator.CookbookVM;













        }
    }
}