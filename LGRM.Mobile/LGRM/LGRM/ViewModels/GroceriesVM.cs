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
        ObservableCollection<Grocery> groceries { get; set; }
        public ObservableCollection<Grocery> Groceries
        {
            get => groceries;
            set
            {
                groceries = value;
                OnPropertyChanged("Groceries");
            }
        }

        List<object> selectedGroceries { get; set; } //To capture View's binding input
        public List<object> SelectedItems
        {
            get => selectedGroceries;
            set
            {
                if (selectedGroceries != value)
                {
                    selectedGroceries = value;
                    OnPropertyChanged("SelectedGroceries"); // neccessary?
                }
            }
        }

        ObservableCollection<string> categories { get; set; }
        public ObservableCollection<string> Categories 
        {
            get => categories;
            set
            {
                categories = value;                    
                OnPropertyChanged("Categories");
            }
        }



        object selectedCategory { get; set; }
        public object SelectedCategory
        {
            get => selectedCategory;
            set
            {
                selectedCategory = value;
                Groceries = GetGroceries(searchQuery);
                //SetShowSelectedItemsButton();
                OnPropertyChanged("SelectedCategory");
            }
        }

        private string searchQuery { get; set; }
        public string SearchQuery
        {
            get => searchQuery;
            set
            {
                searchQuery = value;
                OnPropertyChanged("SearchQuery");
                //SetShowSelectedItemsButton();
                OnSearchCommand(searchQuery);
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
            Groceries = GetGroceries( query);
        }



        #endregion ... members


        public GroceriesVM()
        {
            App.Groceries ??= new ObservableCollection<Grocery>(App.MySQLite.GetAllGroceries().OrderBy(g => g.Name1));
            SelectedItems = new List<object>();
            
            prior = new List<int>();
            current  = new List<int>();
            SelectedGroceriesChanged = new Command(OnSelectedGroceriesChanged);
            
            //SearchCommand = new Command<string>(OnSearchCommand);
            //ShowSelectedItemsCommand = new Command(OnShowSelectedItemsCommand);

        }

        public Command SelectedGroceriesChanged { get; set; }        
        List<int> prior { get; set; }
        List<int> current { get; set; }
        private void OnSelectedGroceriesChanged()
        {
            int CatalogNumberChanged;
            bool toBeAdded;
            Ingredient ingredientChanged;

            current.Clear();
            foreach (var obj in SelectedItems)
            {
                current.Add( (obj as Grocery).CatalogNumber );
            }

            if ( prior.Count < current.Count ) // add item ...
            {
                CatalogNumberChanged = current.Except(prior).ToList()[0]; // ... should only be 1 item in list
                toBeAdded = true;                
            }
            else // remove item ...
            {
                CatalogNumberChanged = prior.Except(current).ToList()[0];
                toBeAdded = false;
            }

            ingredientChanged = new Ingredient(Groceries.FirstOrDefault(g => g.CatalogNumber == CatalogNumberChanged));
            var ResolveData = new object[] { ingredientChanged, toBeAdded };
            
            MessagingCenter.Send<GroceriesVM, object>(this, "UpdateIngredients", ResolveData);

            // Set up for next selection change ...
            prior.Clear();
            foreach (var g in SelectedItems)
            {
                prior.Add( ( g as Grocery ).CatalogNumber );
            }
        }



        public override void Initialize(object parameter)
        {
            this.kind = parameter switch
            {
                Kind.Lean       => Kind.Lean,
                Kind.Green      => Kind.Green,
                Kind.HealthyFat => Kind.HealthyFat,
                Kind.Condiment  => Kind.Condiment,
                            _   => Kind.All
            };
            SetCategories();
            GetGroceries();
            
        }
                
        void SetCategories()
        {
            this.Groceries?.Clear();
            this.Groceries ??= new ObservableCollection<Grocery>();
            foreach (var item in App.Groceries.Where(g => g.Kind == kind)) { this.Groceries.Add(item); }
    
            Categories = new ObservableCollection<string>() { App.V.CategoriesPickerDefault };            
            foreach (var c in Groceries.Select(g => g.Category).Distinct().ToList()) { Categories.Add(c); }

            SelectedCategory = Categories[0]; // = "Show All"
            
        }



























        #region Methods...




        public ObservableCollection<Grocery> GetGroceries( string query = "")
        {
            var result = new ObservableCollection<Grocery>();

            if(Categories == null)
            {
                SetCategories();
            }

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
