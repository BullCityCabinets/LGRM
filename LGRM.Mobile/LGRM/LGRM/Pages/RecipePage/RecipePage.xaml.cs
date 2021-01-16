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

            
            var headerRecipeNameStack = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.StartAndExpand, /*BackgroundColor = colorGeneralBG,*/ Margin = new Thickness(0, 8, 0, 0), Padding = 0, Spacing = 0 };

            //canvasL = new SKCanvasView() { HorizontalOptions = LayoutOptions.Fill, WidthRequest = 30, Margin = 0 };
            //canvasL.PaintSurface += skiaPainter.OnCanvasPaintSurfaceL;

            var headerRecipeName = new Label() { /*FontSize = fontL, */FontAttributes = FontAttributes.Bold, BackgroundColor = Color.White, VerticalTextAlignment = TextAlignment.Center };
            headerRecipeName.SetBinding(Label.TextProperty, "Recipe.Name");

            var canvasR = new SKCanvasView() { HorizontalOptions = LayoutOptions.Fill, WidthRequest = 30, Margin = 0 };
            canvasR.PaintSurface += skiaPainter.OnCanvasPaintSurfaceR;
            var headerButtonAddGroceries = new Button() { ImageSource = "baseline_add_circle_white_24x24.png", WidthRequest = 40, BackgroundColor = Color.White, Margin = new Thickness(0, 8, 0, 0), Padding = 3, CornerRadius = 0 };

            var headerButtonSaveRecipe = new Button() { ImageSource = "baseline_edit_white_24dp.png", WidthRequest = 40, BackgroundColor = Color.LightSlateGray, Margin = new Thickness(0, 12, 0, 0), Padding = 3, CornerRadius = 0 };
            headerButtonSaveRecipe.SetBinding(Button.CommandProperty, "SaveRecipeCommand");

            var canvasR2 = new SKCanvasView() { HorizontalOptions = LayoutOptions.Fill, WidthRequest = 30 };
            canvasR2.PaintSurface += skiaPainter.OnCanvasPaintSurfaceR2;

            Summaries.Children.Add(new SummaryBy(Kind.Lean));
            Summaries.Children.Add(new SummaryBy(Kind.Green));
            Summaries.Children.Add(new SummaryBy(Kind.HealthyFat));
            Summaries.Children.Add(new SummaryBy(Kind.Condiment));

            var ingListL = new IngredientsCollection(Kind.Lean);
            var ingListG = new IngredientsCollection(Kind.Green);
            var ingListH = new IngredientsCollection(Kind.HealthyFat);
            var ingListC = new IngredientsCollection(Kind.Condiment);
            IngredientLists.Children.Add(ingListL);
            IngredientLists.Children.Add(ingListG);
            IngredientLists.Children.Add(ingListH);
            IngredientLists.Children.Add(ingListC);



        }

        void canvasL_PaintSurface(object sender, SKPaintSurfaceEventArgs args) => skiaPainter.OnCanvasPaintSurfaceL(sender, args);
        void canvasR_PaintSurface(object sender, SKPaintSurfaceEventArgs args) => skiaPainter.OnCanvasPaintSurfaceR(sender, args);
        void canvasR2_PaintSurface(object sender, SKPaintSurfaceEventArgs args) => skiaPainter.OnCanvasPaintSurfaceR2(sender, args);

    }
}