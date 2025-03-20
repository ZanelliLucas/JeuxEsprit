// Jeux-esprit.UI/App.xaml.cs
using System;
using System.Windows;
using JeuxDesprit;

namespace JeuxDespritUI
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            // Tester la connexion à la base de données
            var dbManager = new DatabaseManager();
            if (!dbManager.TestConnection())
            {
                MessageBox.Show("Impossible de se connecter à la base de données. L'application fonctionnera avec des données limitées.",
                    "Avertissement de connexion",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
            
            // Vous pourriez initialiser d'autres services ici si nécessaire
        }
    }
}