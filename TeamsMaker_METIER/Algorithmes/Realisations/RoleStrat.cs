using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.Algorithmes.Outils;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages.Classes;
using TeamsMaker_METIER.Personnages;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    internal class RoleStrat : Algorithme
    {
     
        

    public override Repartition Repartir(JeuTest jeuTest)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            Repartition repartition = new Repartition(jeuTest);

            // Tri des personnages par rôle et niveau
            var tanks = jeuTest.Personnages
                .Where(p => p.RolePrincipal == Role.TANK)
                .OrderBy(p => Math.Abs(p.LvlPrincipal - 50))
                .ToList();

            var supports = jeuTest.Personnages
                .Where(p => p.RolePrincipal == Role.SUPPORT)
                .OrderBy(p => Math.Abs(p.LvlPrincipal - 50))
                .ToList();

            var dps = jeuTest.Personnages
                .Where(p => p.RolePrincipal == Role.DPS)
                .OrderBy(p => Math.Abs(p.LvlPrincipal - 50))
                .ToList();

            // Formation des équipes optimales
            while (tanks.Count > 0 && supports.Count > 0 && dps.Count >= 2)
            {
                var tank = tanks.First();
                var support = supports.First();

                // Trouver les 2 DPS qui minimisent le score
                var bestDpsPair = FindBestDpsPair(dps, tank.LvlPrincipal, support.LvlPrincipal);

                if (bestDpsPair == null) break;

                var equipe = new Equipe();
                equipe.AjouterMembre(tank);
                equipe.AjouterMembre(support);
                equipe.AjouterMembre(bestDpsPair.Item1);
                equipe.AjouterMembre(bestDpsPair.Item2);

                repartition.AjouterEquipe(equipe);

                // Retirer les membres utilisés
                tanks.Remove(tank);
                supports.Remove(support);
                dps.Remove(bestDpsPair.Item1);
                dps.Remove(bestDpsPair.Item2);
            }

            stopwatch.Stop();
            TempsExecution = stopwatch.ElapsedMilliseconds;
            return repartition;
        }

        private Tuple<Personnage, Personnage> FindBestDpsPair(List<Personnage> dps, int tankLevel, int supportLevel)
        {
            if (dps.Count < 2) return null;

            Tuple<Personnage, Personnage> bestPair = null;
            double bestScore = double.MaxValue;

            for (int i = 0; i < Math.Min(20, dps.Count); i++) // Limite la recherche pour performance
            {
                for (int j = i + 1; j < Math.Min(20, dps.Count); j++)
                {
                    double avg = (tankLevel + supportLevel + dps[i].LvlPrincipal + dps[j].LvlPrincipal) / 4.0;
                    double score = Math.Pow(50 - avg, 2);

                    if (score < bestScore)
                    {
                        bestScore = score;
                        bestPair = Tuple.Create(dps[i], dps[j]);
                    }
                }
            }

            return bestPair ?? Tuple.Create(dps[0], dps[1]);
        }
    }

    }


    


