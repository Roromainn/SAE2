using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.Algorithmes;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Problemes;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    /// <summary>
    /// Algorithme N-Opt pour optimiser les équipes en fusionnant N équipes à la fois.
    /// </summary>
    public class AlgoNOpt : Algorithme
    {
        // Paramètres de configuration
        private readonly int _n;                  // Nombre d'équipes à réoptimiser
        private readonly Algorithme _algoInitial; // Algorithme pour la solution initiale
        private const int MaxIterations = 50;     // Limite anti-boucle infinie
        private readonly Probleme _probleme;

        /// <summary>
        /// Constructeur de l'algorithme N-Opt.
        /// </summary>
        public AlgoNOpt(int n, Algorithme algoInitial, Probleme probleme)
        {
            _n = n;
            _algoInitial = algoInitial ?? throw new ArgumentNullException("L'algorithme initial est requis");
            _probleme = probleme;
        }
        public AlgoNOpt() : this(3, new AlgorithmeEquilibreProgressif(), Probleme.SIMPLE) { }

        /// <summary>
        /// Répartit les personnages d'un jeu de test en équipes en utilisant l'algorithme N-Opt.
        /// </summary>
        /// <param name="jeuTest">Fichier de jeu de test contenant la liste de perso</param>
        /// <returns>Composition "optimale" des equipes selon la méthode décrite dans le document joint</returns>
        public override Repartition Repartir(JeuTest jeuTest)
        {
            // Solution initiale
            var repartition = _algoInitial.Repartir(jeuTest);
            bool improved;
            int iterations = 0;
            var rand = new Random();

            do
            {
                improved = false;

                // Sélection aléatoire de N équipes à réoptimiser
                var selectedTeams = repartition.Equipes
                    .OrderBy(_ => rand.Next()) // Mélange aléatoire
                    .Take(_n)
                    .ToList();

                var players = selectedTeams.SelectMany(t => t.Membres).ToList();

                // Recréation d'un sous-problème
                var tempTest = new JeuTest();
                foreach (var p in players)
                    tempTest.AjouterPersonnage(p);

                // Réoptimisation avec algorithme glouton
                var newRepartition = new AlgorithmeEquilibreProgressif().Repartir(tempTest);

                repartition.LancerEvaluation(_probleme);
                newRepartition.LancerEvaluation(_probleme);

                // amélioration = remplacement des équipes
                if (newRepartition.Score < repartition.Score)
                {
                    repartition = MergeRepartitions(repartition, selectedTeams, newRepartition);
                    improved = true;
                }

                iterations++;
            } while (improved && iterations < MaxIterations);

            return repartition;
        }

        /// <summary>
        /// Fusionne les anciennes et nouvelles équipes dans une répartition cohérente
        /// </summary>
        private Repartition MergeRepartitions(
            Repartition original,
            List<Equipe> toReplace,
            Repartition newPart)
        {
            var merged = new Repartition(original.jeuTest);

            // Ajout des équipes non modifiées
            foreach (var team in original.Equipes)
                if (!toReplace.Contains(team))
                    merged.AjouterEquipe(team);

            // Ajout des nouvelles équipes optimisées
            foreach (var team in newPart.Equipes)
                merged.AjouterEquipe(team);

            return merged;
        }
    }
}



