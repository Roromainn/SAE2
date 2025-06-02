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

        public override Repartition Repartir(JeuTest jeuTest)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Repartition repartition = new Repartition(jeuTest);
            List<Personnage> remaining = new List<Personnage>(jeuTest.Personnages);

            while (CanFormTeam(remaining))
            {
                Equipe equipe = FormerEquipeRapide(remaining);
                if (equipe != null && equipe.Membres.Length == 4)
                {
                    repartition.AjouterEquipe(equipe);
                    // Retirer les membres de l'équipe
                    foreach (var membre in equipe.Membres)
                    {
                        remaining.Remove(membre);
                    }
                }
                else
                {
                    break;
                }
            }

            stopwatch.Stop();
            this.TempsExecution = stopwatch.ElapsedMilliseconds;
            return repartition;
        }

        private Equipe FormerEquipeRapide(List<Personnage> remaining)
        {
            // Trouver tank
            Personnage tank = null;
            for (int i = 0; i < remaining.Count; i++)
            {
                if (remaining[i].RolePrincipal == Role.TANK)
                {
                    tank = remaining[i];
                    break;
                }
            }
            if (tank == null)
            {
                for (int i = 0; i < remaining.Count; i++)
                {
                    if (remaining[i].RoleSecondaire == Role.TANK)
                    {
                        tank = remaining[i];
                        break;
                    }
                }
            }

            // Trouver support
            Personnage support = null;
            for (int i = 0; i < remaining.Count; i++)
            {
                if (remaining[i].RolePrincipal == Role.SUPPORT)
                {
                    support = remaining[i];
                    break;
                }
            }
            if (support == null)
            {
                for (int i = 0; i < remaining.Count; i++)
                {
                    if (remaining[i].RoleSecondaire == Role.SUPPORT)
                    {
                        support = remaining[i];
                        break;
                    }
                }
            }

            if (tank == null || support == null) return null;

            // Trouver 2 DPS (on prend les 2 premiers disponibles)
            List<Personnage> dps = new List<Personnage>();
            for (int i = 0; i < remaining.Count && dps.Count < 2; i++)
            {
                if (remaining[i] != tank && remaining[i] != support)
                {
                    if (remaining[i].RolePrincipal == Role.DPS || remaining[i].RoleSecondaire == Role.DPS)
                    {
                        dps.Add(remaining[i]);
                    }
                }
            }

            if (dps.Count < 2) return null;

            Equipe equipe = new Equipe();
            equipe.AjouterMembre(tank);
            equipe.AjouterMembre(support);
            equipe.AjouterMembre(dps[0]);
            equipe.AjouterMembre(dps[1]);

            return equipe;
        }

        private bool CanFormTeam(List<Personnage> remaining)
        {
            bool hasTank = false;
            bool hasSupport = false;
            int dpsCount = 0;

            for (int i = 0; i < remaining.Count; i++)
            {
                if (remaining[i].RolePrincipal == Role.TANK || remaining[i].RoleSecondaire == Role.TANK)
                    hasTank = true;
                if (remaining[i].RolePrincipal == Role.SUPPORT || remaining[i].RoleSecondaire == Role.SUPPORT)
                    hasSupport = true;
                if (remaining[i].RolePrincipal == Role.DPS || remaining[i].RoleSecondaire == Role.DPS)
                    dpsCount++;
            }

            return hasTank && hasSupport && dpsCount >= 2;
        }
    }
}