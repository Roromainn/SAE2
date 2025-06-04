using System;
using System.Collections.Generic;
using System.Linq;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Problemes;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    public class AlgoNSwap : Algorithme
    {
        public int SwapCount = 1;
        public Algorithme AlgoInitial;
        public Probleme Probleme; // Ajout d'une propriété pour le problème

        public override Repartition Repartir(JeuTest jeuTest)
        {
            // Solution initiale
            var bestSolution = AlgoInitial.Repartir(jeuTest);
            bestSolution.LancerEvaluation(Probleme);

            // Optimisation locale
            for (int i = 0; i < 100; i++)
            {
                var neighbor = GenerateNeighbor(bestSolution, jeuTest);
                neighbor.LancerEvaluation(Probleme);

                if (neighbor.Score < bestSolution.Score)
                    bestSolution = neighbor;
                else
                    break;
            }

            return bestSolution;
        }

        private Repartition GenerateNeighbor(Repartition original, JeuTest jeuTest)
        {
            var neighbor = new Repartition(jeuTest);
            var teams = original.Equipes.Where(t => t.Membres.Length == 4).ToList();

            if (teams.Count < 2)
                return original;

            // Copie des équipes
            foreach (var team in original.Equipes)
            {
                var newTeam = new Equipe();
                foreach (var member in team.Membres)
                    newTeam.AjouterMembre(member);
                neighbor.AjouterEquipe(newTeam);
            }

            // Échanges
            var rand = new Random();
            for (int s = 0; s < SwapCount; s++)
            {
                int i = rand.Next(teams.Count);
                int j = rand.Next(teams.Count);
                if (i == j) continue;

                var teamA = neighbor.Equipes[i];
                var teamB = neighbor.Equipes[j];
                int playerA = rand.Next(4);
                int playerB = rand.Next(4);

                // Swap
                var temp = teamA.Membres[playerA];
                teamA.Membres[playerA] = teamB.Membres[playerB];
                teamB.Membres[playerB] = temp;
            }

            return neighbor;
        }
    }
}