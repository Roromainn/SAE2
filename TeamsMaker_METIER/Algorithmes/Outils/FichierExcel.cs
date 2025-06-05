using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using OfficeOpenXml;
using TeamsMaker_METIER.Algorithmes;
using TeamsMaker_METIER.Algorithmes.Realisations;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Personnages.Classes;

namespace TeamsMaker_METIER.Algorithmes.Outils
{
    /// <summary>
    /// Classe utilitaire pour générer un rapport Excel des performances des algorithmes de répartition de personnages.
    /// </summary>
    public class FichierExcel
    {
        #region--Methodes--
        /// <summary>
        /// Génère un rapport Excel des performances des algorithmes de répartition de personnages.
        /// </summary>
        /// <param name="fichierSortie">Chemin et nom du fichier</param>
        public static void GenererRapportExcel(string fichierSortie)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Configuration des tests
            int[] taillesEchantillons = { 100, 500, 1000, 5000, 10000 };
            string[] modesGeneration = { "equilibre", "extremes", "biais_fort", "biais_faible", "aleatoire", "progression", "identiques" };

            var algorithmes = new List<Algorithme>
            {
                new RoleEtape(),
                new RoleAdaptatif(),
                new AlgorithmeEquilibreProgressif(),
                new AlgorithmeExtremesEnPremier(),
                new AlgorithmeGloutonCroissant(),
                new AlgoNOpt(),
            };

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Performances");

                int col = 1;
                worksheet.Cells[1, col++].Value = "Taille";
                worksheet.Cells[1, col++].Value = "Type Répartition";

                foreach (var algo in algorithmes)
                {
                    worksheet.Cells[1, col++].Value = algo.GetType().Name;
                }

                int row = 2;
                foreach (int taille in taillesEchantillons)
                {
                    foreach (string mode in modesGeneration)
                    {
                        col = 1;
                        JeuTest jeuTest = CreerJeuTest(taille, mode);

                        // Informations sur la configuration
                        worksheet.Cells[row, col++].Value = taille;
                        worksheet.Cells[row, col++].Value = GetLibelleMode(mode);

                        // Résultats des algorithmes
                        foreach (var algo in algorithmes)
                        {
                            var temps = MesurerPerformance(algo, jeuTest);
                            worksheet.Cells[row, col++].Value = temps;
                        }

                        row++;
                    }

                    row++;
                }
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                package.SaveAs(new FileInfo(fichierSortie));
            }
        }

        /// <summary>
        /// Obtient une représentation lisible du mode de génération des personnages.
        /// </summary>
        /// <param name="mode">type de repartition</param>
        private static string GetLibelleMode(string mode)
        {
            return mode switch
            {
                "equilibre" => "Équilibré (45-55)",
                "extremes" => "Extrêmes (1-10 ou 90-99)",
                "biais_fort" => "Biaisé fort (70-99)",
                "biais_faible" => "Biaisé faible (1-30)",
                "aleatoire" => "Aléatoire (1-99)",
                "progression" => "Progression (1-99)",
                "identiques" => "Identiques (50)",
                _ => mode
            };
        }

        /// <summary>
        /// Crée un jeu de test avec un nombre spécifié de personnages et un mode de génération.
        /// </summary>
        /// <param name="nombrePersonnages">nombre de perso a générer</param>
        /// <param name="modeGeneration">type de repartition</param>
        /// <returns></returns>
        private static JeuTest CreerJeuTest(int nombrePersonnages, string modeGeneration)
        {
            var jeuTest = new JeuTest();
            var generateur = new GenerateurPersonnages(modeGeneration);

            for (int i = 0; i < nombrePersonnages; i++)
            {
                jeuTest.AjouterPersonnage(generateur.Generer());
            }

            return jeuTest;
        }

        /// <summary>
        /// Mesure la performance d'un algorithme de répartition sur un jeu de test donné.
        /// </summary>
        /// <param name="algorithme">algorithme dont on veut mesurer la perf</param>
        /// <param name="jeuTest">le jeu de test sur lequel on va tester l'algo</param>
        /// <returns>temps en ms</returns>
        private static long MesurerPerformance(Algorithme algorithme, JeuTest jeuTest)
        {
            // Échauffement
            algorithme.Repartir(jeuTest);

            // Mesure
            var chrono = Stopwatch.StartNew();
            for (int i = 0; i < 5; i++)
            {
                algorithme.Repartir(jeuTest);
            }
            chrono.Stop();

            return chrono.ElapsedMilliseconds / 5;
        }
    }

    /// <summary>
    ///// Générateur de personnages pour les tests, permettant de créer des personnages avec des niveaux et classes variés selon un mode de génération spécifié.
    /// </summary>
    internal class GenerateurPersonnages
    {
        private readonly string modeGeneration;
        private int counter = 0;
        private readonly Array classes = Enum.GetValues(typeof(Classe));

        public GenerateurPersonnages(string modeGeneration)
        {
            this.modeGeneration = modeGeneration;
        }

        /// <summary>
        /// Génère un personnage en fonction du mode de génération spécifié.
        /// </summary>
        /// <returns>Un perso avec les niveau choisis</returns>
        public Personnage Generer()
        {
            var classe = GetClasse();
            return new Personnage(
                classe: classe,
                lvlPrincipal: GenererNiveauPrincipal(),
                lvlSecondaire: GenererNiveauSecondaire(classe)
            );
        }

        /// <summary>
        /// Obtient une classe de personnage en fonction du compteur et du mode de génération.
        /// </summary>
        /// <returns></returns>
        private Classe GetClasse()
        {
            return (Classe)classes.GetValue(counter % classes.Length);
        }

        /// <summary>
        /// Génère un niveau principal pour le personnage en fonction du mode de génération.
        /// </summary>
        private int GenererNiveauPrincipal()
        {
            int niveau = modeGeneration switch
            {
                "equilibre" => 45 + (counter % 11), // 45-55
                "extremes" => (counter % 2 == 0) ? 1 + (counter % 10) : 90 + (counter % 10),
                "biais_fort" => 70 + (counter % 30), // 70-99
                "biais_faible" => 1 + (counter % 30), // 1-30
                "aleatoire" => 1 + (counter % 100), // 1-100
                "progression" => 1 + (counter % 100), // 1-100
                "identiques" => 50, // Fixe 50
                _ => 50 // Par défaut
            };

            counter++;
            return Math.Clamp(niveau, 1, 100);
        }

        /// <summary>
        /// Génère un niveau secondaire pour le personnage en fonction de sa classe et du mode de génération.
        /// </summary>
        /// <param name="classe">classe du perso</param>
        private int GenererNiveauSecondaire(Classe classe)
        {
            if (!(classe == Classe.BARBARE || classe == Classe.PALADIN || classe == Classe.DRUIDE))
                return 0;

            return modeGeneration switch
            {
                "equilibre" => 45 + (counter % 11),
                "extremes" => (counter % 2 == 0) ? 1 + (counter % 10) : 90 + (counter % 10),
                "biais_fort" => 70 + (counter % 30),
                "biais_faible" => 1 + (counter % 30),
                "aleatoire" => 1 + (counter % 100),
                "progression" => 1 + (counter % 100),
                "identiques" => 50,
                _ => 20 + (counter % 20) 
            };
        }
        #endregion
    }
}