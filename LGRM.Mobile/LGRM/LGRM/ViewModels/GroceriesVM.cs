using LGRM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace LGRM.XamF.ViewModels
{
    public class GroceriesVM : BaseVM
    {

        #region Members...
        Kind kind { get; set; }
        ObservableCollection<Grocery> displayedGroceries { get; set; }
        public ObservableCollection<Grocery> DisplayedGroceries
        {
            get => displayedGroceries;
            set
            {
                displayedGroceries = value;
                OnPropertyChanged("DisplayedGroceries");
            }
        }

        bool isLoading { get; set; } //See GroceriesVM.Initialize

        List<object> selectedGroceries { get; set; } //To capture View's binding input
        public List<object> SelectedItems
        {
            get => selectedGroceries;
            set
            {
                if (selectedGroceries != value)                    
                {   
                    selectedGroceries = value;                    
                    if (!isLoading)                    
                    {                    
                        OnPropertyChanged("SelectedGroceries"); // neccessary?                        
                    }
                }
            }
        }

                
        private string searchQuery { get; set; }
        public string SearchQuery
        {
            get => searchQuery;
            set
            {
                if ( (string)selectedCategory != value )
                {                    
                    searchQuery = value;
                    OnPropertyChanged("SearchQuery");
                    //SetShowSelectedItemsButton();
                    if (!isLoading)
                    {
                        OnSearchCommand(searchQuery);
                    }
                }
            }
        }

        public bool IsShowingSelectedItems { get; set; }
        public void SetShowSelectedItemsButton()
        {
            if (IsShowingSelectedItems == false)
            {
                ShowSelectedItemsButtonIcon = SelectedItems.Count > 0 
                    ? "show_selected_18dp.png" 
                    : "no_icon_18dp.png";
            }
            else ShowSelectedItemsButtonIcon = "show_all_18dp.png";
            
        }
        string showSelectedItemsButtonIcon { get; set; }
        public string ShowSelectedItemsButtonIcon
        {
            get => showSelectedItemsButtonIcon;
            set
            {
                showSelectedItemsButtonIcon = value;
                OnPropertyChanged("ShowSelectedItemsButtonIcon");
            }
        }
        public void OnSearchCommand(string query)
        {   
            DisplayedGroceries = GetGroceries(query);
        }



        #endregion ... members

        Kind[] kinds;
        public List<ObservableCollection<string>> CategoriesList;

        public GroceriesVM()
        {
            kinds = new Kind[] { Kind.Lean, Kind.Green, Kind.HealthyFat, Kind.Condiment, Kind.All };
            CategoriesList = new List<ObservableCollection<string>>()
                { new ObservableCollection<string>(), new ObservableCollection<string>(), new ObservableCollection<string>(), new ObservableCollection<string>(), new ObservableCollection<string>()  };

            App.Groceries ??= new ObservableCollection<Grocery>( App.MySQLite.GroceryList??= new List<Grocery>(App.MySQLite.GetAllGroceries()) );
            for (int i = 0; i < 4; i++)
            {
                var geez = App.Groceries.Where(g => g.Kind == kinds[i]);

                CategoriesList[i].Add(App.V.CategoriesPickerDefault); // "All Categories"
                foreach (var c in geez.Select(g => g.Category).Distinct().ToList())
                {
                    CategoriesList[i].Add(c);
                }
            }
            CategoriesList[4].Add("Kind.All Selected!");
            Categories = new ObservableCollection<string>( CategoriesList[0] );            
            SelectedCategory = Categories[0];
                        
            priorCatNums = new List<int>();
            currentCatNums  = new List<int>();
            SelectedItems = new List<object>();
            SelectedGroceriesChanged = new Command(OnSelectedGroceriesChanged);

            //SearchCommand = new Command<string>(OnSearchCommand);
            //ShowSelectedItemsCommand = new Command(OnShowSelectedItemsCommand);

        }

        public Command SelectedGroceriesChanged { get; set; }        
        List<int> priorCatNums { get; set; }
        List<int> currentCatNums { get; set; }
        private void OnSelectedGroceriesChanged()
        {
            int CatalogNumberChanged;
            bool toBeAdded;
            Ingredient ingredientChanged;

            currentCatNums.Clear();
            foreach (var obj in SelectedItems)
            {
                currentCatNums.Add( (obj as Grocery).CatalogNumber );
            }

            if ( priorCatNums.Count < currentCatNums.Count ) // add item ...
            {
                CatalogNumberChanged = currentCatNums.Except(priorCatNums).ToList()[0]; // ... should only be 1 item in list
                toBeAdded = true;                
            }
            else // remove item ...
            {
                CatalogNumberChanged = priorCatNums.Except(currentCatNums).ToList()[0];
                toBeAdded = false;
            }

            ingredientChanged = new Ingredient(DisplayedGroceries.FirstOrDefault(g => g.CatalogNumber == CatalogNumberChanged));
            var ResolveData = new object[] { ingredientChanged, toBeAdded };
            
            MessagingCenter.Send<GroceriesVM, object>(this, "UpdateIngredients", ResolveData);

            // Set up for next selection change ...
            priorCatNums.Clear();
            foreach (var g in SelectedItems)
            {
                priorCatNums.Add( ( g as Grocery ).CatalogNumber );
            }
        }



        public override void Initialize(object parameter)
        {
            if (this.kind.ToString() != parameter.ToString())
            {
                this.kind = parameter switch
                {
                    Kind.Lean => Kind.Lean,
                    Kind.Green => Kind.Green,
                    Kind.HealthyFat => Kind.HealthyFat,
                    Kind.Condiment => Kind.Condiment,
                    _ => Kind.All
                };
                SetCategories();
            }
            SelectedCategory = Categories[0];
            DisplayedGroceries = GetGroceries();
        }
                
        void SetCategories()
        {
            Categories = this.kind switch
            {
                Kind.Lean => CategoriesList[0],
                Kind.Green => CategoriesList[1],
                Kind.HealthyFat => CategoriesList[2],
                Kind.Condiment => CategoriesList[3],
                _ => CategoriesList[4],
            };
        }


        ObservableCollection<string> categories { get; set; }
        public ObservableCollection<string> Categories
        {
            get => categories;
            set
            {
                categories = value;
                //if (!isLoading)
                //{
                    OnPropertyChanged("Categories");
                //}
            }
        }


        object selectedCategory { get; set; }
        public object SelectedCategory
        {
            get { 
                if (selectedCategory == null)
                {
                    return App.V.CategoriesPickerDefault;
                }
                else return selectedCategory; 
            }
            set
            {
                if (!isLoading && value != null && selectedCategory != value)
                {
                    selectedCategory = value;
                    DisplayedGroceries = GetGroceries(searchQuery);
                    //SetShowSelectedItemsButton();
                    //OnPropertyChanged("SelectedCategory");

                }
                OnPropertyChanged("SelectedCategory");

            }
        }

























        #region Methods...




        public ObservableCollection<Grocery> GetGroceries( string query = "")
        {
            var result = new ObservableCollection<Grocery>();

            if(Categories == null) { SetCategories(); }
            
            if (string.IsNullOrEmpty(query))
            {
                if (SelectedCategory.ToString() == Categories[0])
                {
                    // by Kind only ....
                    result = new ObservableCollection<Grocery>(
                        App.Groceries.Where(g => g.Kind == this.kind
                        ).ToList().OrderBy(g => g.Name1));
                    IsShowingSelectedItems = false;
                }
                else // by Category ...
                {
                    result = new ObservableCollection<Grocery>(
                        App.Groceries.Where(g => g.Kind == this.kind
                                              && g.Category == SelectedCategory.ToString()
                                              ).ToList().OrderBy(g => g.Name1));
                    IsShowingSelectedItems = false;
                }
            }
            else // by Query ...
            {
                query = query.ToLower().Trim();

                if (SelectedCategory.ToString() == Categories[0])
                {
                    // by Query ...


                    try
                    {
                        result = new ObservableCollection<Grocery>(
                            App.Groceries.Where(g => g.Kind == this.kind
                                                && (g.Name1.ToLowerInvariant().Contains(query) ||
                                                     g.Name2.ToLowerInvariant().Contains(query) ||
                                                     g.Desc1.ToLowerInvariant().Contains(query))
                                                     ).ToList().OrderBy(g => g.Name1));
                    }
                    catch (Exception x)
                    {
                        Console.WriteLine(x);
                    }
                    
                    IsShowingSelectedItems = false;
                }
                else // by Query & Category ...
                {
                    result = new ObservableCollection<Grocery>(
                        App.Groceries.Where(g => g.Kind == this.kind
                                            && g.Category == SelectedCategory.ToString()
                                            && (g.Name1.ToLowerInvariant().Contains(query) ||
                                                 g.Name2.ToLowerInvariant().Contains(query) ||
                                                 g.Desc1.ToLowerInvariant().Contains(query))
                                                 ).ToList().OrderBy(g => g.Name1));
                    IsShowingSelectedItems = false;
                }

            }
            return result.Count > 0 ? result : new ObservableCollection<Grocery>(App.Groceries.Where(g => g.Kind == this.kind).ToList().OrderBy(g => g.Name1));
        }
        #endregion ... methods


    }
}
