using LGRM.XamF.ViewModels;
using LGRM.XamF.ViewModels.Framework;
using SkiaSharp.Views.Forms;
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
        SkiaPainter skiaPainter;

        public CookbookPage()
        {            
            InitializeComponent();
         
            this.BindingContext = ViewModelLocator.CookbookVM;

            skiaPainter = new SkiaPainter(); //Do not move this above if this is still the launch page!  It breaks App.xaml StaticResources
        }

        void canvas_Open2Title(object sender, SKPaintSurfaceEventArgs args) => skiaPainter.OnCanvasPaint_Open2Title(sender, args);
        void canvas_Open2Sub(object sender, SKPaintSurfaceEventArgs args) => skiaPainter.OnCanvasPaint_Title2Sub(sender, args);
        void canvas_Sub2Open(object sender, SKPaintSurfaceEventArgs args) => skiaPainter.OnCanvasPaint_Sub2Open(sender, args);

    }
}