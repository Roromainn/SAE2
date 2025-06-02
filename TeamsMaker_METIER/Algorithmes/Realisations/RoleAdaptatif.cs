using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages.Classes;
using TeamsMaker_METIER.Personnages;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    internal class RoleAdaptatif : Algorithme
    {
        private const double NIVEAU_CIBLE = 50.0;
        private const double TOLERANCE = 5.0; // Tolérance pour le niveau moyen

        public override Repartition Repartir(JeuTest jeuTest)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Repartition repartition = new Repartition(jeuTest);
            List<Personnage> remaining = new List<Personnage>(jeuTest.Personnages);

            // Tri des personnages par niveau pour optimiser les combinaisons
            remaining = remaining.OrderBy(p => Math.Abs(p.LvlPrincipal - NIVEAU_CIBLE)).ToList();

            while (CanFormTeam(remaining))
            {
                Equipe meilleureEquipe = TrouverMeilleureEquipe(remaining);
                if (meilleureEquipe != null && meilleureEquipe.Membres.Length == 4)
                {
                    repartition.AjouterEquipe(meilleureEquipe);
                    // Retirer les membres de l'équipe de la liste
                    foreach (var membre in meilleureEquipe.Membres)
                    {
                        remaining.Remove(membre);
                    }
                }
                else
                {
                    break; // Plus possible de former une équipe optimale
                }
            }

            stopwatch.Stop();
            this.TempsExecution = stopwatch.ElapsedMilliseconds;
            return repartition;
        }

        private Equipe TrouverMeilleureEquipe(List<Personnage> remaining)
        {
            Equipe meilleureEquipe = null;
            double meilleurScore = double.MaxValue;

            // Récupérer tous les candidats par rôle
            var tanks = GetPersonnagesParRole(remaining, Role.TANK);
            var supports = GetPersonnagesParRole(remaining, Role.SUPPORT);
            var dps = GetPersonnagesParRole(remaining, Role.DPS);

            if (tanks.Count == 0 || supports.Count == 0 || dps.Count < 2)
                return null;

            // Tester toutes les combinaisons possibles (optimisé)
            foreach (var tank in tanks.Take(Math.Min(tanks.Count, 5))) // Limiter pour performance
            {
                foreach (var support in supports.Take(Math.Min(supports.Count, 5)))
                {
                    // Prendre les 2 meilleurs DPS pour cette combinaison tank/support
                    var dpsOptimaux = TrouverMeilleursDPS(dps, tank, support);

                    if (dpsOptimaux.Count >= 2)
                    {
                        for (int i = 0; i < dpsOptimaux.Count - 1; i++)
                        {
                            for (int j = i + 1; j < Math.Min(dpsOptimaux.Count, i + 6); j++) // Limiter les combinaisons
                            {
                                Equipe equipe = new Equipe();
                                equipe.AjouterMembre(tank);
                                equipe.AjouterMembre(support);
                                equipe.AjouterMembre(dpsOptimaux[i]);
                                equipe.AjouterMembre(dpsOptimaux[j]);

                                double score = EvaluerEquipe(equipe);
                                if (score < meilleurScore)
                                {
                                    meilleurScore = score;
                                    meilleureEquipe = equipe;
                                }
                            }
                        }
                    }
                }
            }

            return meilleureEquipe;
        }

        private List<Personnage> GetPersonnagesParRole(List<Personnage> personnages, Role role)
        {
            return personnages
                .Where(p => p.RolePrincipal == role || p.RoleSecondaire == role)
                .OrderBy(p => p.RolePrincipal == role ? 0 : 1) // Prioriser rôle principal
                .ThenBy(p => Math.Abs(p.LvlPrincipal - NIVEAU_CIBLE))
                .ToList();
        }

        private List<Personnage> TrouverMeilleursDPS(List<Personnage> dps, Personnage tank, Personnage support)
        {
            double niveauMoyenActuel = (tank.LvlPrincipal + support.LvlPrincipal) / 2.0;
            double niveauCibleDPS = (NIVEAU_CIBLE * 4 - tank.LvlPrincipal - support.LvlPrincipal) / 2.0;

            return dps.OrderBy(p => Math.Abs(p.LvlPrincipal - niveauCibleDPS)).ToList();
        }

        private double EvaluerEquipe(Equipe equipe)
        {
            if (equipe.Membres.Length != 4) return double.MaxValue;
            double niveauMoyen = equipe.Membres.Average(m => m.LvlPrincipal);

            // Score basé uniquement sur la distance au niveau cible
            return Math.Abs(niveauMoyen - NIVEAU_CIBLE);
        }

        private bool CanFormTeam(List<Personnage> remaining)
        {
            bool hasTank = remaining.Exists(p => p.RolePrincipal == Role.TANK || p.RoleSecondaire == Role.TANK);
            bool hasSupport = remaining.Exists(p => p.RolePrincipal == Role.SUPPORT || p.RoleSecondaire == Role.SUPPORT);
            int dpsCount = remaining.Count(p => p.RolePrincipal == Role.DPS || p.RoleSecondaire == Role.DPS);

            return hasTank && hasSupport && dpsCount >= 2;
        }
    }
}