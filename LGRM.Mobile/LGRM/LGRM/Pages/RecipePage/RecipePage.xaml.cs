using LGRM.Model;
using LGRM.XamF.ViewModels.Framework;
using SkiaSharp;
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
    public partial class RecipePage : ContentPage
    {
        SkiaPainter skiaPainter = new SkiaPainter();
        

        public RecipePage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.RecipeVM;
            
            Summaries.Children.Add( new SummaryBy(Kind.Lean) );
            Summaries.Children.Add( new SummaryBy(Kind.Green) );
            Summaries.Children.Add( new SummaryBy(Kind.HealthyFat) );
            Summaries.Children.Add( new SummaryBy(Kind.Condiment) );
            Summaries.Children.Add(new SummaryBy(Kind.All)); // "Serves: __"

            IngredientLists.Children.Add( new IngredientsCollection(Kind.Lean) );
            IngredientLists.Children.Add( new IngredientsCollection(Kind.Green) );
            IngredientLists.Children.Add( new IngredientsCollection(Kind.HealthyFat) );
            IngredientLists.Children.Add( new IngredientsCollection(Kind.Condiment) );



        }

        void canvas_Open2Title(object sender, SKPaintSurfaceEventArgs args) => skiaPainter.OnCanvasPaint_Open2Title(sender, args);
        void canvas_Open2Sub(object sender, SKPaintSurfaceEventArgs args) => skiaPainter.OnCanvasPaint_Title2Sub(sender, args);
        void canvas_Sub2Open(object sender, SKPaintSurfaceEventArgs args) => skiaPainter.OnCanvasPaint_Sub2Open(sender, args);

    }
}