using LGRM.Model;
using LGRM.XamF.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace LGRM.XamF.ViewModels
{
    public class CookbookVM : BaseVM
    {
        private INavigationService _navigationService;

        private List<Recipe> recipesDisplayed;
        public List<Recipe> RecipesDisplayed
        {
            get => recipesDisplayed;
            set
            {
                recipesDisplayed = value;
                OnPropertyChanged("RecipesDisplayed");
            }
        }


        public int IngredientLabelHeight = 100; //Standard height
        public int EmptyHeight = 60;
        private int _heightOfCollectionView { get; set; }
        public int HeightOfCollectionView
        {
            get
            {
                if (RecipesDisplayed.Count > 0)
                {
                    return (_heightOfCollectionView * IngredientLabelHeight);
                }
                else
                {
                    return EmptyHeight;
                }
            }
            set
            {
                _heightOfCollectionView = value;
                OnPropertyChanged("HeightOfCollectionView");
            }
        }




        public ICommand CreateNewRecipeCommand { get; }

        #region CTOR...
        public CookbookVM(/*IPieDataService pieDataService,*/ INavigationService navigationService)
        {
            _navigationService = navigationService;
            CreateNewRecipeCommand = new Command(OnCreateNewRecipeCommand);

            RecipesDisplayed = new List<Recipe>();
            var recipeMetas = App.MySQLite.GetAllRecipeMetas();
            foreach (var item in recipeMetas)
            {
                RecipesDisplayed.Add(item);
            }

            //MessagingCenter.Subscribe<RecipeVM>(this, "UpdateSavedRecipesList", OnNewRecipeSavedCommand);   // ...from Lists of Groceries



        }

        #endregion ...CTOR


        private void OnCreateNewRecipeCommand(object obj)
        {
            _navigationService.NavigateTo("RecipePage");
        }


















    }
}
