using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    /// <summary>
    /// Classe qui implémente l'algorithme d'équilibrage progressif des équipes.
    /// Cette méthode forme les équipes en ajoutant un personnage à la fois, en choisissant
    /// celui qui rapproche la moyenne des niveaux des membres de l'équipe la plus proche possible de 50.
    /// </summary>
    /// <summary>
    /// Fabrique des algorithmes d'équilibrage des équipes.
    /// </summary>
    internal class AlgorithmeEquilibreProgressif : Algorithme
    {
        #region --- Méthodes ---

        /// <summary>
        /// Forme des équipes en ajoutant les personnages un à un pour équilibrer la moyenne des niveaux autour de 50.
        /// À chaque ajout, le personnage qui rapproche le plus la moyenne de 50 est choisi.
        /// </summary>
        /// <param name="jeuTest">Jeu contenant la liste des personnages à répartir dans les équipes.</param>
        /// <returns>Un objet de type <see cref="Repartition"/> contenant les équipes formées.</returns>
        public override Repartition Repartir(JeuTest jeuTest)
        {
            // Liste des personnages restants à ajouter aux équipes
            List<Personnage> listeRestante = new List<Personnage>(jeuTest.Personnages);

            // Objet représentant la répartition des personnages dans les équipes
            Repartition repartition = new Repartition(jeuTest);

            // Tant qu'il reste assez de personnages pour former une équipe complète (4 membres)
            while (listeRestante.Count >= 4)
            {
                // Création d'une nouvelle équipe vide
                Equipe equipe = new Equipe();

                // Initialisation de la moyenne de l'équipe (à 0 car l'équipe est vide au départ)
                float moyenneEquipe = 0;

                // Ajouter les membres un par un à l'équipe jusqu'à ce qu'elle ait 4 membres
                while (equipe.Membres.Length < 4)
                {
                    // Variables pour choisir le personnage le mieux adapté pour la moyenne
                    Personnage meilleurPersonnage = null;
                    float meilleurEcart = float.MaxValue;

                    // On parcourt la liste des personnages restants
                    foreach (Personnage personnage in listeRestante)
                    {
                        // Calcul de la nouvelle moyenne si ce personnage est ajouté
                        float nouvelleMoyenne = (moyenneEquipe * equipe.Membres.Length + personnage.LvlPrincipal) / (equipe.Membres.Length + 1);

                        // Calcul de l'écart entre la nouvelle moyenne et 50
                        float ecart = Math.Abs(nouvelleMoyenne - 50);

                        // Si cet écart est plus petit que l'écart précédent, ce personnage devient le meilleur choix
                        if (ecart < meilleurEcart)
                        {
                            meilleurEcart = ecart;
                            meilleurPersonnage = personnage;
                        }
                    }

                    // Ajout du meilleur personnage trouvé à l'équipe
                    equipe.AjouterMembre(meilleurPersonnage);

                    // Mise à jour de la moyenne de l'équipe après ajout du personnage
                    moyenneEquipe = (moyenneEquipe * (equipe.Membres.Length - 1) + meilleurPersonnage.LvlPrincipal) / equipe.Membres.Length;

                    // Retrait du personnage de la liste des personnages restants
                    listeRestante.Remove(meilleurPersonnage);
                }

                // Ajout de l'équipe complète à la répartition des équipes
                repartition.AjouterEquipe(equipe);
            }

            // Retourne la répartition des équipes une fois que toutes sont formées
            return repartition;
        }

        #endregion
    }

}

