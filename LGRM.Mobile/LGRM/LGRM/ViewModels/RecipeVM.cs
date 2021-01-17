using LGRM.Model;
using LGRM.XamF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace LGRM.XamF.ViewModels
{
    public class RecipeVM : BaseVM
    {
        private Recipe _recipe { get; set; }
        public Recipe Recipe
        {
            get => _recipe;
            set
            {
                _recipe = value;
                OnPropertyChanged("Recipe");
            }

        }

        private float _recipeServes { get; set; }
        public float RecipeServes
        {
            get => _recipeServes;
            set
            {
                _recipeServes = value;
                UpdateOnServingsChanged();
                OnPropertyChanged("RecipeServes");
            }
        }

        #region ObservableCollection<Ingredient>'s for binding...
        private ObservableCollection<Ingredient> leans { get; set; }
        private ObservableCollection<Ingredient> greens { get; set; }
        private ObservableCollection<Ingredient> healthyfats { get; set; }
        private ObservableCollection<Ingredient> condiments { get; set; }
        public ObservableCollection<Ingredient> Leans
        {
            get => leans;
            set
            {
                leans = value;
                OnPropertyChanged("Leans");
            }
        } 
        public ObservableCollection<Ingredient> Greens
        {
            get => greens;
            set
            {
                greens = value;
                OnPropertyChanged("Greens");
            }

        }
        public ObservableCollection<Ingredient> HealthyFats
        {
            get => healthyfats;
            set
            {
                healthyfats = value;
                OnPropertyChanged("HealthyFats");
            }

        }
        public ObservableCollection<Ingredient> Condiments
        {
            get => condiments;
            set
            {
                condiments = value;
                OnPropertyChanged("Condiments");
            }

        }

        #endregion        

        #region //~~ For adjusting collections' heights per items added...

        public int EmptyHeight = 60;
        public int HeightOf1UOM = 100;  // 102
        public int HeightOf2UOMs = 138; // 150

        public int IngredientLabelHeight = 180; //Standard Ingredient label height ... 150 was too short
        public int CollViewMargin = 12; //Standard Collection Margins

        private int _heightL { get; set; }
        public int HeightL
        {
            get => _heightL;
            set
            {
                _heightL = CalcCollectionHeight(Kind.Lean);
                OnPropertyChanged("HeightL");
            }
        }
        private int _heightG { get; set; }
        public int HeightG
        {
            get => _heightG;
            set
            {
                _heightG = CalcCollectionHeight(Kind.Green);
                OnPropertyChanged("HeightG");
            }
        }
        private int _heightH { get; set; }
        public int HeightH
        {
            get => _heightH;
            set
            {
                _heightH = CalcCollectionHeight(Kind.HealthyFat);
                OnPropertyChanged("HeightH");
            }
        }
        private int _heightC { get; set; }
        public int HeightC
        {
            get => _heightC;
            set
            {
                _heightC = CalcCollectionHeight(Kind.Condiment);
                OnPropertyChanged("HeightC");
            }
        }

 

        #endregion //~~ For adjusting View's collection heights per items added...

        #region //~~ For Recipe summary of contents in header...       \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\


        private float _totalLs { get; set; }
        private float _totalGs { get; set; }
        private float _totalHs { get; set; }
        private float _totalCs { get; set; }

        public float TotalLs
        {
            get => _totalLs;
            set
            {
                _totalLs = GetPortions(Kind.Lean);
                SetRecommendedHs();
                BGStateL = SetSummaryBackgroundColor(Kind.Lean);
                OnPropertyChanged("TotalLs");
            }

        }
        public float TotalGs
        {
            get => _totalGs;
            set
            {
                _totalGs = GetPortions(Kind.Green);
                BGStateG = SetSummaryBackgroundColor(Kind.Green);
                OnPropertyChanged("TotalGs");
            }

        }
        public float TotalHs
        {
            get => _totalHs;
            set
            {
                _totalHs = GetPortions(Kind.HealthyFat);
                BGStateH = SetSummaryBackgroundColor(Kind.HealthyFat);
                OnPropertyChanged("TotalHs");
            }

        }
        public float TotalCs
        {
            get => _totalCs;
            set
            {
                _totalCs = GetPortions(Kind.Condiment);
                BGStateC = SetSummaryBackgroundColor(Kind.Condiment);
                OnPropertyChanged("TotalCs");
            }

        }

        private float _recommendedLs { get; set; }
        private float _recommendedGs { get; set; }
        private float _recommendedHs { get; set; }
        private float _recommendedCs { get; set; }

        public float RecommendedLs
        {
            get => _recommendedLs;
            set
            {
                _recommendedLs = value;
                BGStateL = SetSummaryBackgroundColor(Kind.Lean);
                OnPropertyChanged("RecommendedLs");
            }

        }
        public float RecommendedGs
        {
            get => _recommendedGs;
            set
            {
                _recommendedGs = value;
                BGStateG = SetSummaryBackgroundColor(Kind.Green);
                OnPropertyChanged("RecommendedGs");
            }

        }
        public float RecommendedHs
        {
            get => _recommendedHs;
            set
            {
                _recommendedHs = value;
                BGStateH = SetSummaryBackgroundColor(Kind.HealthyFat);
                OnPropertyChanged("RecommendedHs");
            }

        }
        public float RecommendedCs
        {
            get => _recommendedCs;
            set
            {
                _recommendedCs = value;
                BGStateC = SetSummaryBackgroundColor(Kind.Condiment);
                OnPropertyChanged("RecommendedCs");
            }

        }

        private float _bgStateL { get; set; }
        public float BGStateL
        {
            get => _bgStateL;
            set
            {
                _bgStateL = value;
                OnPropertyChanged("BGStateL");
            }
        }
        private float _bgStateG { get; set; }
        public float BGStateG
        {
            get => _bgStateG;
            set
            {
                _bgStateG = value;
                OnPropertyChanged("BGStateG");
            }
        }
        private float _bgStateH { get; set; }
        public float BGStateH
        {
            get => _bgStateH;
            set
            {
                _bgStateH = value;
                OnPropertyChanged("BGStateH");
            }
        }
        private float _bgStateC { get; set; }
        public float BGStateC
        {
            get => _bgStateC;
            set
            {
                _bgStateC = value;
                OnPropertyChanged("BGStateC");
            }
        }

        #endregion


        //public ICommand NavigateToGroceriesCommand { get; }


        private INavigationService _navigationService;
        public ICommand VerifyClearRecipeCommand { get; set; }
        public ICommand SaveRecipeCommand { get; set; }

        #region CTOR...
        public RecipeVM(/*IPieDataService pieDataService,*/ INavigationService navigationService)
        {
            _navigationService = navigationService;
            //NavigateToGroceriesCommand = new Command<string>(OnNavigateToGroceriesCommand);
            Recipe = new Recipe();
            Leans       = new ObservableCollection<Ingredient>();
            Greens      = new ObservableCollection<Ingredient>();
            HealthyFats = new ObservableCollection<Ingredient>();
            Condiments  = new ObservableCollection<Ingredient>();
            FixForCollectionViewsEmptyViewToDisplay();

            Leans.CollectionChanged += CollectionContentsChanged;  //~~ To alert lists when their contents change (Xamarin doens't do this, naturally) https://stackoverflow.com/questions/1427471/observablecollection-not-noticing-when-item-in-it-changes-even-with-inotifyprop
            Greens.CollectionChanged += CollectionContentsChanged;
            HealthyFats.CollectionChanged += CollectionContentsChanged;
            Condiments.CollectionChanged += CollectionContentsChanged;


            //MessagingCenter.Subscribe<RecipeVM>(this, "UpdateSavedRecipesList", OnNewRecipeSavedCommand);   // ...from Lists of Groceries
            MessagingCenter.Subscribe< GroceriesVM, object >(this, "UpdateIngredients", UpdateRecipeIngredients);   // ...from Lists of Groceries

            //VerifyClearRecipeCommand = new Command(OnVerifyClearRecipeCommand);
            //SaveRecipeCommand = new Command(OnSaveRecipeCommand);

        }

        public override void Initialize(object parameter)
        {
            if (parameter == null)
            {
                Recipe = new Recipe();
            }
            else
            {
                Recipe = parameter as Recipe;
            }

            UpdateOnServingsChanged();
        }

        public void FixForCollectionViewsEmptyViewToDisplay() // Without this, the collection views will not display "Empty View" on startup ...
        {
            AddIngredient(new Ingredient() { Kind = Kind.Lean, CatalogNumber = 666 });
            AddIngredient(new Ingredient() { Kind = Kind.Green, CatalogNumber = 777 });
            AddIngredient(new Ingredient() { Kind = Kind.HealthyFat, CatalogNumber = 888 });
            AddIngredient(new Ingredient() { Kind = Kind.Condiment, CatalogNumber = 999 });
            Leans.Clear();
            Greens.Clear();
            HealthyFats.Clear();
            Condiments.Clear();

        }


        private void UpdateRecipeIngredients( GroceriesVM sender, object update )
        {
            var updateContents = (object[])update;
            var ingredientChanged = (Ingredient)updateContents[0];
            var toBeAdded = (bool)updateContents[1];

            if (toBeAdded)
            {
                AddIngredient(ingredientChanged);
            }
            else
            {
                RemoveIngredient(ingredientChanged);
            }
        }

        public ICommand NavigateToGroceriesCommand
        {
            get
            {
                return new Command<Kind>((k) => OnNavigateToGroceriesCommand(k));
            }
        }

        public void OnNavigateToGroceriesCommand(Kind k)
        {
            _navigationService.NavigateTo("GroceriesPage", k);
        }




        #endregion ...CTOR






        #region Methods...



        public List<Ingredient> GetIngredients(Kind kind = Kind.All)
        {
            var Ings = new List<Ingredient>();
            switch (kind)
            {
                case Kind.Lean:
                    return Ings = new List<Ingredient>( Leans );
                case Kind.Green:
                    return Ings = new List<Ingredient>( Greens );
                case Kind.HealthyFat:
                    return Ings = new List<Ingredient>( HealthyFats );
                case Kind.Condiment:
                    return Ings = new List<Ingredient>( Condiments );
                default:
                    var lists = new ObservableCollection<Ingredient>[] { Leans, Greens, HealthyFats, Condiments };
                    foreach (var l in lists)
                    {
                        foreach (var ing in l)
                        {
                            Ings.Add(ing);
                        }
                    }
                    return Ings;
            }
        }

        void AddIngredient(Ingredient ingredient)
        {
            Recipe.Ingredients.Add(ingredient);
            switch (ingredient.Kind)
            {
                case Kind.Lean:
                    Leans.Add(ingredient);
                    HeightL += 1;
                    break;
                case Kind.Green:
                    Greens.Add(ingredient);
                    HeightG += 1;
                    break;
                case Kind.HealthyFat:
                    HealthyFats.Add(ingredient);
                    HeightH += 1;
                    break;
                case Kind.Condiment:
                    Condiments.Add(ingredient);
                    HeightC += 1;
                    break;
                default: throw new Exception(message: "RecipeVM.AddIngredient: No Kind found!");
                    
            }
        }
        void RemoveIngredient(Ingredient ingredient)
        {
            Recipe.Ingredients.Remove(ingredient);
            switch (ingredient.Kind)
            {
                case Kind.Lean:
                    Leans.Remove(Leans.FirstOrDefault(ing => ing.CatalogNumber == ingredient.CatalogNumber));
                    HeightL -= 1;
                    break;
                case Kind.Green:
                    Greens.Remove(Greens.FirstOrDefault(ing => ing.CatalogNumber == ingredient.CatalogNumber));
                    HeightG -= 1;
                    break;
                case Kind.HealthyFat:
                    HealthyFats.Remove(HealthyFats.FirstOrDefault(ing => ing.CatalogNumber == ingredient.CatalogNumber));
                    HeightH -= 1;
                    break;
                case Kind.Condiment:
                    Condiments.Remove(Condiments.FirstOrDefault(ing => ing.CatalogNumber == ingredient.CatalogNumber));
                    HeightC -= 1;
                    break;
                default: throw new Exception(message: "RecipeVM.RemoveIngredient: No Kind found!");
                    
            }
        }

        public void UpdateOnServingsChanged()
        {
            if (RecipeServes > 0)
            {
                RecommendedLs = 1 * RecipeServes;
                RecommendedGs = 3 * RecipeServes;
                RecommendedHs = SetRecommendedHs();
                RecommendedCs = 3 * RecipeServes;
            }
            else
            {
                RecommendedLs = 1;
                RecommendedGs = 3;
                RecommendedHs = SetRecommendedHs();
                RecommendedCs = 3;
            }
        }

        public float GetPortions(Kind kind = Kind.All) // ... counts bound in header
        {
            var list = new List<Ingredient>(GetIngredients(kind));
            float portion = 0;
            foreach (var ing in list)
            {
                portion += ing.QtyPortion;
            }
            return portion;
        }

        public void UpdatePortions(Kind kind = Kind.All)
        {
            switch (kind)
            {
                case Kind.Lean:
                    if (Leans.Count > 0)
                    {
                        TotalLs = GetPortions(Kind.Lean);
                        RecommendedHs = SetRecommendedHs();
                    }
                    break;
                case Kind.Green:
                    if (Greens.Count > 0)
                    {
                        TotalGs = GetPortions(Kind.Green);
                    }
                    break;
                case Kind.HealthyFat:
                    if (HealthyFats.Count > 0)
                    {
                        TotalHs = GetPortions(Kind.HealthyFat);
                    }
                    break;
                case Kind.Condiment:
                    if (Condiments.Count > 0)
                    {
                        TotalCs = GetPortions(Kind.Condiment);
                    }
                    break;
                default:
                    UpdatePortions(Kind.Lean);
                    UpdatePortions(Kind.Green);
                    UpdatePortions(Kind.HealthyFat);
                    UpdatePortions(Kind.Condiment);
                    break;

            }
        }

        public float SetRecommendedHs()
        {
            {
                float x = 0;
                if (Leans != null)
                {
                    foreach (var ing in Leans)
                    {
                        x = ing.Info1 switch
                        {
                            2 => x += (1 * ing.QtyPortion),
                            3 => x += (2 * ing.QtyPortion),
                            _ => x += (3 * ing.QtyPortion)
                        };
                    }
                    return x < 99 ? x : 99;
                }
                else return 0; // !Leans? ... there would be no Leans, anyhow.
            }
        }

        private float SetSummaryBackgroundColor(Kind kind) //ConverterToEvaluateState sets actual colors
        {
            var x = kind switch
            {
                Kind.Lean =>        new float[] { TotalLs, RecommendedLs },
                Kind.Green =>       new float[] { TotalGs, RecommendedGs },
                Kind.HealthyFat =>  new float[] { TotalHs, RecommendedHs },
                Kind.Condiment =>   new float[] { TotalCs, RecommendedCs },
                _ => throw new NotImplementedException()
            };
            return x[0] - x[1];  //ConverterToEvaluateState uses this to determine background color
        }

        public int CalcCollectionHeight(Kind kind = Kind.All)
        {
            var list = GetIngredients(kind);
            if (list.Count > 0)
            {
                int height = 0;
                foreach (var ing in list)
                {
                    var UOMsCount = (byte)ing.UOMs;
                    height += UOMsCount > 2 ? HeightOf2UOMs : HeightOf1UOM;
                }
                return height;
            }
            else
            {
                return EmptyHeight;
            }
        }


        #region to update summary when ingredient lists change...
        public void CollectionContentsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Ingredient item in e.NewItems)
                {
                    item.PropertyChanged += IngredientPropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (Ingredient item in e.OldItems)
                {
                    item.PropertyChanged -= IngredientPropertyChanged;
                }
            }
        }   //Updates from Ingredient items on Recipe View

        public void IngredientPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdatePortions();
        }   // called when the property of an object inside the collection changes

        #endregion ... to update summary when ingredient lists change
        #endregion ... methods











    }
}
