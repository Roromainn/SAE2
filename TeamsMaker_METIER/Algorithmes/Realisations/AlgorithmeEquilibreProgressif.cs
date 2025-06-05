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
    public class AlgorithmeEquilibreProgressif : Algorithme
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
                Equipe equipe = new Equipe();
                float moyenneEquipe = 0;

                while (equipe.Membres.Length < 4)
                {
                    Personnage meilleurPersonnage = null;
                    float meilleurEcart = float.MaxValue;

                    // On parcourt la liste des personnages restants
                    foreach (Personnage personnage in listeRestante)
                    {
                        // Calcul de la nouvelle moyenne si ce personnage est ajouté
                        float nouvelleMoyenne = (moyenneEquipe * equipe.Membres.Length + personnage.LvlPrincipal) / (equipe.Membres.Length + 1);
                        float ecart = Math.Abs(nouvelleMoyenne - 50);
                        // Si écart est plus petit que l'écart précédent le perso devient meilleur choix
                        if (ecart < meilleurEcart)
                        {
                            meilleurEcart = ecart;
                            meilleurPersonnage = personnage;
                        } 
                    }
                    equipe.AjouterMembre(meilleurPersonnage);
                    moyenneEquipe = (moyenneEquipe * (equipe.Membres.Length - 1) + meilleurPersonnage.LvlPrincipal) / equipe.Membres.Length;
                    listeRestante.Remove(meilleurPersonnage);
                }
                repartition.AjouterEquipe(equipe);
            }
            return repartition;
        }

        #endregion
    }

}

