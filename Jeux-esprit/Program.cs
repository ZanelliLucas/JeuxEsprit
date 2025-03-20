using System;
using System.Windows;
using JeuxEspritUI.Views;

namespace JeuxDesprit
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                var application = new Application();
                var loginWindow = new LoginWindow();
                application.Run(loginWindow);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite lors du démarrage de l'application : {ex.Message}", 
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}