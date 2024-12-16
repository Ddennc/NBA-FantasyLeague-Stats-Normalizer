using NBAFantasyLeague.ViewModel;

namespace NBAFantasyLeague.View;

public partial class MainPage : ContentPage
{
    public MainPage(PlayersViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}