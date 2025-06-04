using System;
using System.Collections.Generic;
using System.IO;
using TeamsMaker_METIER.Personnages.Classes;

namespace TeamsMaker_METIER.Personnages.Generation
{
    /// <summary>
    /// Classe permettant de générer des fichiers de personnages aléatoires
    /// selon différents modes de répartition des niveaux (équilibré, extrêmes, etc.).
    /// Les personnages sont ensuite enregistrés dans un fichier texte dans le dossier de sortie de l'application.
    /// </summary>
    public class Generateur
    {
        // Générateur de nombres aléatoires
        private static Random random = new Random();

        #region --- Méthodes ---

        /// <summary>
        /// Génère un fichier contenant une liste de personnages avec classe, niveau principal et niveau secondaire,
        /// selon un mode choisi (équilibré, extrêmes, biaisé, etc.).
        /// </summary>
        /// <param name="nbPersonnages">Nombre de personnages à générer (par défaut : 20).</param>
        /// <param name="mode">
        /// Mode de génération des niveaux :
        /// "equilibre" : niveaux entre 45 et 55,
        /// "extremes" : niveaux très faibles ou très élevés,
        /// "biais_fort" : niveaux entre 70 et 99,
        /// "biais_faible" : niveaux entre 1 et 30,
        /// "aleatoire" : niveaux aléatoires de 1 à 99,
        /// "progression" : niveaux croissants de 1 à 99,
        /// "identiques" : niveaux fixes à 50.
        /// </param>
        public static void GenererPersonnages(int nbPersonnages = 20, string mode = "equilibre")
        {
            // Liste contenant les lignes de texte représentant chaque personnage
            List<string> personnages = new List<string>();

            // Listes utilisées pour les modes "progression" et "identiques"
            List<int> niveauxPrincipaux = new List<int>();
            List<int> niveauxSecondaires = new List<int>();

            // Génération des valeurs fixes ou croissantes pour les modes spécifiques
            if (mode == "progression")
            {
                // Répartition des niveaux de 1 à 99 en fonction du nombre de personnages
                for (int i = 0; i < nbPersonnages; i++)
                {
                    int value = 1 + i * 98 / nbPersonnages;
                    niveauxPrincipaux.Add(value);
                    niveauxSecondaires.Add(value);
                }
            }
            else if (mode == "identiques")
            {
                // Tous les personnages ont le même niveau (50)
                for (int i = 0; i < nbPersonnages; i++)
                {
                    niveauxPrincipaux.Add(50);
                    niveauxSecondaires.Add(50);
                }
            }

            // Récupération des classes disponibles dans l'énumération Classe
            Array classesDispo = Enum.GetValues(typeof(Classe));

            // Boucle principale de génération de personnages
            for (int i = 0; i < nbPersonnages; i++)
            {
                // Sélection aléatoire d'une classe parmi celles disponibles
                Classe classe = (Classe)classesDispo.GetValue(random.Next(classesDispo.Length));

                int niveau;      // Niveau principal
                int specialite;  // Niveau de spécialité (secondaire)

                // Génération des niveaux selon le mode choisi
                switch (mode)
                {
                    case "equilibre":
                        niveau = random.Next(45, 56);
                        specialite = random.Next(45, 56);
                        break;

                    case "extremes":
                        niveau = random.Next(2) == 0 ? random.Next(1, 11) : random.Next(90, 100);
                        specialite = random.Next(2) == 0 ? random.Next(1, 11) : random.Next(90, 100);
                        break;

                    case "biais_fort":
                        niveau = random.Next(70, 100);
                        specialite = random.Next(70, 100);
                        break;

                    case "biais_faible":
                        niveau = random.Next(1, 31);
                        specialite = random.Next(1, 31);
                        break;

                    case "aleatoire":
                        niveau = random.Next(1, 100);
                        specialite = random.Next(1, 100);
                        break;

                    case "progression":
                    case "identiques":
                        // Récupération des niveaux pré-calculés dans les listes
                        niveau = niveauxPrincipaux[i];
                        specialite = niveauxSecondaires[i];
                        break;

                    default:
                        throw new ArgumentException($"Mode inconnu : {mode}");
                }

                // Formatage du personnage sous forme texte (Classe NiveauPrincipal NiveauSecondaire)
                personnages.Add($"{classe} {niveau} {specialite}");
            }

            // Construction du nom du fichier : exemple "equilibre_20.jt"
            string nomFichier = $"{mode}_{nbPersonnages}.jt";

            // Chemin du fichier dans le dossier de sortie du projet (bin/Debug/... ou bin/Release/...)
            string chemin = Path.Combine(Directory.GetCurrentDirectory(), nomFichier);

            // Écriture de toutes les lignes dans le fichier
            File.WriteAllLines(chemin, personnages);

            // Message de confirmation en console
            Console.WriteLine($"Fichier généré : {chemin} ({nbPersonnages} personnages, mode: {mode})");
        }

        #endregion
    }
}




