using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Problemes;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    public class AlgoNSwap : Algorithme
    {

        // Paramètres de configuration
        private readonly int _n;                  // Nombre de swaps à effectuer
        private readonly Algorithme _algoInitial; // Algorithme pour la solution initiale
        private readonly Probleme _probleme;      // Problème à résoudre

        private const int MaxIterations = 100;    // Limite anti-boucle infinie
        private const int MaxNeighbors = 10;      // Nombre de voisins générés par itération

        #region--Constructeurs ---
        public AlgoNSwap(int n, Algorithme algoInitial, Probleme probleme)
        {
            _n = n;
            _algoInitial = algoInitial ?? throw new ArgumentNullException("L'algorithme initial est requis");
            _probleme = probleme;
        }
        #endregion

        #region--Methodes ---
        /// <summary>
        /// Répartit les personnages d'un jeu de test en équipes en utilisant l'algorithme N-Swap
        /// </summary>
        /// <param name="jeuTest">Fichier de jeu de test contenant la liste de perso</param>
        /// <returns>Composition "optimale" des equipes selon la méthode décrite dans le document joint</returns>
        public override Repartition Repartir(JeuTest jeuTest)
        {
            var repartition = _algoInitial.Repartir(jeuTest);
            repartition.LancerEvaluation(_probleme);     
            bool improved;
            int iterations = 0;

            do
            {
                improved = false;
                var neighbors = GenerateNeighbors(repartition, _n, jeuTest);

                // Évaluation de chaque voisin
                foreach (var neighbor in neighbors)
                {
                    neighbor.LancerEvaluation(_probleme);

                    // Si amélioration, on conserve cette solution
                    if (neighbor.Score < repartition.Score)
                    {
                        repartition = neighbor;
                        improved = true;
                        break; // On repart de cette nouvelle solution
                    }
                }
                iterations++;
            } while (improved && iterations < MaxIterations); 

            return repartition;
        }

        /// <summary>
        /// Génère des solutions voisines par échange de joueurs
        /// </summary>
        private List<Repartition> GenerateNeighbors(Repartition original, int n, JeuTest jeuTest)
        {
            var neighbors = new List<Repartition>();
            var rand = new Random();

            // Ne travailler que sur les équipes complètes (4 joueurs)
            var completeTeams = original.Equipes
                .Where(e => e.Membres.Length == 4)
                .ToList();

            // Génération de plusieurs voisins
            for (int i = 0; i < MaxNeighbors; i++)
            {
                // Clonage de la solution originale
                var neighbor = CloneRepartition(original, jeuTest);
                var teams = neighbor.Equipes.Where(e => e.Membres.Length == 4).ToList();

                for (int s = 0; s < n; s++)
                {
                    if (teams.Count < 2) break;

                    int idx1 = rand.Next(teams.Count);
                    int idx2;
                    do
                    {
                        idx2 = rand.Next(teams.Count);
                    } while (idx1 == idx2);

                    var team1 = teams[idx1];
                    var team2 = teams[idx2];

                    int idxPlayer1 = rand.Next(4);
                    int idxPlayer2 = rand.Next(4);

                    // Création de nouvelles équipes avec échange
                    var newTeam1 = new Equipe();
                    var newTeam2 = new Equipe();

                    // Copie des membres avec échange
                    for (int j = 0; j < 4; j++)
                    {
                        if (j == idxPlayer1)
                            newTeam1.AjouterMembre(team2.Membres[idxPlayer2]);
                        else
                            newTeam1.AjouterMembre(team1.Membres[j]);
                    }

                    for (int j = 0; j < 4; j++)
                    {
                        if (j == idxPlayer2)
                            newTeam2.AjouterMembre(team1.Membres[idxPlayer1]);
                        else
                            newTeam2.AjouterMembre(team2.Membres[j]);
                    }

                    //Gestion des index introuvables
                    var teamsList = neighbor.Equipes.ToList();
                    int pos1 = teamsList.IndexOf(team1);
                    int pos2 = teamsList.IndexOf(team2);

                    if (pos1 == -1 || pos2 == -1) continue;

                    teamsList[pos1] = newTeam1;
                    teamsList[pos2] = newTeam2;

                    neighbor = new Repartition(jeuTest);
                    foreach (var team in teamsList)
                        neighbor.AjouterEquipe(team);

                    teams = neighbor.Equipes.Where(e => e.Membres.Length == 4).ToList();
                }

                neighbor.LancerEvaluation(_probleme);
                neighbors.Add(neighbor);
            }
            return neighbors;
        }

        /// <summary>
        /// Crée une copie profonde d'une répartition
        /// </summary>
        private Repartition CloneRepartition(Repartition original, JeuTest jeuTest)
        {
            var clone = new Repartition(jeuTest);
            foreach (var team in original.Equipes)
            {
                var newTeam = new Equipe();
                foreach (var member in team.Membres)
                    newTeam.AjouterMembre(member);
                clone.AjouterEquipe(newTeam);
            }
            return clone;
        }
        #endregion

    }
}