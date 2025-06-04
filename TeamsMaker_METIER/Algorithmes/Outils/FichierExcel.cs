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
    public class FichierExcel
    {
        
        public static void GenererRapportExcel(string fichierSortie)
        {
            // Set the license context for EPPlus
            if (ExcelPackage.LicenseContext == LicenseContext.NonCommercial)
            {
                // Déjà configuré
            }
            else
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            }
            // Configuration des tests
            int[] taillesEchantillons = { 100, 500, 1000, 5000, 10000 };
            var algorithmes = new List<Algorithme>
           {
               new RoleEtape(),
               new RoleAdaptatif(),
               new AlgorithmeEquilibreProgressif(),
               new AlgorithmeExtremesEnPremier(),
               new AlgorithmeGloutonCroissant(),
               new AlgoNOpt(),
               new AlgoNSwap(),

               // Ajouter d'autres algorithmes ici
           };

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Performances");

                // En-têtes
                worksheet.Cells[1, 1].Value = "Taille échantillon";
                for (int i = 0; i < algorithmes.Count; i++)
                {
                    worksheet.Cells[1, i + 2].Value = algorithmes[i].GetType().Name;
                }

                // Corps du rapport
                for (int row = 0; row < taillesEchantillons.Length; row++)
                {
                    int taille = taillesEchantillons[row];
                    JeuTest jeuTest = CreerJeuTest(taille);

                    worksheet.Cells[row + 2, 1].Value = taille;

                    for (int col = 0; col < algorithmes.Count; col++)
                    {
                        var temps = MesurerPerformance(algorithmes[col], jeuTest);
                        worksheet.Cells[row + 2, col + 2].Value = temps;
                    }
                }

                // Sauvegarde
                package.SaveAs(new FileInfo(fichierSortie));
            }
        }

        private static JeuTest CreerJeuTest(int nombrePersonnages)
        {
            var jeuTest = new JeuTest();
            var generateur = new GenerateurPersonnages();

            for (int i = 0; i < nombrePersonnages; i++)
            {
                jeuTest.AjouterPersonnage(generateur.Generer());
            }

            return jeuTest;
        }

        private static long MesurerPerformance(Algorithme algorithme, JeuTest jeuTest)
        {
            // Échauffement
            algorithme.Repartir(jeuTest);

            // Mesure
            var chrono = Stopwatch.StartNew();
            for (int i = 0; i < 5; i++) // Moyenne sur 5 runs
            {
                algorithme.Repartir(jeuTest);
            }
            chrono.Stop();

            return chrono.ElapsedMilliseconds / 5;
        }
    }

    internal class GenerateurPersonnages
    {
        private readonly Random random = new Random();
        private readonly Array classes = Enum.GetValues(typeof(Classe));

        public Personnage Generer()
        {
            var classe = (Classe)classes.GetValue(random.Next(classes.Length));
            return new Personnage(
                classe: classe,
                lvlPrincipal: GenererNiveauPrincipal(),
                lvlSecondaire: GenererNiveauSecondaire(classe)
            );
        }

        private int GenererNiveauPrincipal()
        {
            // Distribution normale centrée sur 50
            double u1 = 1.0 - random.NextDouble();
            double u2 = 1.0 - random.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            int niveau = (int)(50 + randStdNormal * 15);

            return Math.Clamp(niveau, 1, 100);
        }

        private int GenererNiveauSecondaire(Classe classe)
        {
            // Seules certaines classes ont un niveau secondaire
            return classe switch
            {
                Classe.BARBARE or Classe.PALADIN or Classe.DRUIDE => random.Next(10, 40),
                _ => 0
            };
        }
    }
}