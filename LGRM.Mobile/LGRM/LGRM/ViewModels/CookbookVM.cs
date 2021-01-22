using LGRM.Model;
using LGRM.XamF.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace LGRM.XamF.ViewModels
{
    public class CookbookVM : BaseVM
    {
        INavigationService _navigationService;
        public string FooterText => App.V.FooterText;


        private List<Recipe> _recipesDisplayed;
        public List<Recipe> RecipesDisplayed
        {
            get => _recipesDisplayed;
            set
            {
                _recipesDisplayed = value;
                OnPropertyChanged("RecipesDisplayed");
            }
        }


        public int EmptyHeight = 60;
        public int HeightOfStandardRecipe = 100; //Standard height        
        private int _heightOfCollectionView { get; set; }
        public int HeightOfCollectionView
        {
            get
            {
                return RecipesDisplayed.Count > 0
                    ? (_heightOfCollectionView * HeightOfStandardRecipe)
                    : EmptyHeight;
            }
            set
            {
                _heightOfCollectionView = value;
                OnPropertyChanged("HeightOfCollectionView");
            }
        }



        #region CTOR...
        public CookbookVM(INavigationService navigationService)        
        {
            _navigationService = navigationService;

            RecipesDisplayed = new List<Recipe>();
            var recipeMetas = App.MySQLite.GetAllRecipeMetas();
            foreach (var item in recipeMetas)
            {
                RecipesDisplayed.Add(item);
            }

            MessagingCenter.Subscribe<RecipeVM>(this, "UpdateSavedRecipesList", OnUpdateSavedRecipesList);   // ...from Lists of Groceries            
            CreateNewRecipeCommand = new Command(OnCreateNewRecipeCommand);
        }
        #endregion ...CTOR


        public ICommand CreateNewRecipeCommand { get; }
        private void OnCreateNewRecipeCommand(object obj)
        {
            _navigationService.NavigateTo("RecipePage");
        }




        //public ICommand LoadSelectedRecipeCommand
        //{
        //    get
        //    {
        //        return new Command((e) =>
        //        {

        //            IsLoading = true;
        //            var RecipeIdToLoad = (e as Recipe).Id;
        //            _navigationService.NavigateTo("RecipePage", RecipeIdToLoad);
        //            IsLoading = false;
        //        });
        //    }
        //}

        public async Task LoadSelectedRecipe(int recipeToLoadId)
        {
            //IsLoading = true;
            await _navigationService.NavigateTo("RecipePage", recipeToLoadId);
            //IsLoading = false;
        }
        //bool _isLoading { get; set; }
        //public bool IsLoading
        //{
        //    get => _isLoading;
        //    set
        //    {
        //        _isLoading = value;
        //        OnPropertyChanged("IsLoading");
        //    }
        //}




        private void OnUpdateSavedRecipesList(RecipeVM obj)
        {
            RecipesDisplayed = App.MySQLite.GetAllRecipeMetas(); //Update the list
        }











    }
}
