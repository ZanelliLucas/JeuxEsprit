// Jeux-esprit.UI/Views/LoginWindow.xaml.cs
using System;
using System.Windows;

namespace JeuxEspritUI.Views
{
    /// <summary>
    /// Logique d'interaction pour LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            // Ouvrir la fenêtre de connexion
            var loginForm = new LoginFormWindow();
            loginForm.Owner = this;
            
            if (loginForm.ShowDialog() == true)
            {
                // Si la connexion réussit, ouvrir le menu principal
                var mainWindow = new MainWindow(loginForm.ConnectedUser);
                this.Hide();
                mainWindow.ShowDialog();
                this.Show();
            }
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            // Ouvrir la fenêtre d'inscription
            var registerWindow = new RegisterWindow();
            registerWindow.Owner = this;
            registerWindow.ShowDialog();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            // Fermer l'application après confirmation
            if (MessageBox.Show("Voulez-vous vraiment quitter l'application ?", 
                    "Confirmation", 
                    MessageBoxButton.YesNo, 
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
}