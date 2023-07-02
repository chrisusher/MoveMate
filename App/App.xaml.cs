using ChrisUsher.MoveMate.App;

namespace App;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new MainPage();
    }
}