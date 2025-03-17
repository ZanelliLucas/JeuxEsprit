using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JeuxDesprit;

namespace JeuxDespritTests
{
    [TestClass]
    public class PartieTests
    {
        [TestMethod]
        public void TestConstructeurPartie()
        {
            // Act
            Partie partie = new Partie();

            // Assert
            Assert.AreEqual("", partie.GetStatut());
            Assert.IsTrue((DateTime.Now - partie.GetDatePartie()).TotalSeconds < 1);
            Assert.AreEqual(0, partie.GetScore());
            Assert.AreEqual(TimeSpan.Zero, partie.GetTempsEffectue());
        }

        [TestMethod]
        public void TestConstructeurPartieAvecStatut()
        {
            // Arrange
            string statut = "gagné";

            // Act
            Partie partie = new Partie(statut);

            // Assert
            Assert.AreEqual(statut, partie.GetStatut());
        }

        [TestMethod]
        public void TestMettreAJourPartie()
        {
            // Arrange
            Partie partie = new Partie();
            bool gagne = true;
            int score = 1000;
            TimeSpan temps = TimeSpan.FromSeconds(60);

            // Act
            partie.MettreAJour(gagne, score, temps);

            // Assert
            Assert.AreEqual("gagné", partie.GetStatut());
            Assert.AreEqual(score, partie.GetScore());
            Assert.AreEqual(temps, partie.GetTempsEffectue());
            Assert.IsTrue((DateTime.Now - partie.GetDatePartie()).TotalSeconds < 1);
        }

        [TestMethod]
        public void TestMettreAJourPartiePerdue()
        {
            // Arrange
            Partie partie = new Partie();
            bool gagne = false;
            int score = 0;
            TimeSpan temps = TimeSpan.FromSeconds(45);

            // Act
            partie.MettreAJour(gagne, score, temps);

            // Assert
            Assert.AreEqual("perdu", partie.GetStatut());
            Assert.AreEqual(score, partie.GetScore());
            Assert.AreEqual(temps, partie.GetTempsEffectue());
        }

        [TestMethod]
        public void TestSetterGetterPartie()
        {
            // Arrange
            Partie partie = new Partie();
            string nouveauStatut = "abandonné";
            DateTime nouvelleDate = new DateTime(2023, 5, 15);
            int nouveauScore = 500;
            TimeSpan nouveauTemps = TimeSpan.FromMinutes(2);

            // Act
            partie.SetStatut(nouveauStatut);
            partie.SetDatePartie(nouvelleDate);
            partie.SetScore(nouveauScore);
            partie.SetTempsEffectue(nouveauTemps);

            // Assert
            Assert.AreEqual(nouveauStatut, partie.GetStatut());
            Assert.AreEqual(nouvelleDate, partie.GetDatePartie());
            Assert.AreEqual(nouveauScore, partie.GetScore());
            Assert.AreEqual(nouveauTemps, partie.GetTempsEffectue());
        }
    }
}