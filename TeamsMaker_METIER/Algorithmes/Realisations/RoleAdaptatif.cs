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
        public override Repartition Repartir(JeuTest jeuTest)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Repartition repartition = new Repartition(jeuTest);

            List<Personnage> remaining = new List<Personnage>(jeuTest.Personnages);

            while (CanFormTeam(remaining))
            {
                Equipe equipe = new Equipe();

                // Trouver tank
                Personnage tank = remaining.Find(p => p.RolePrincipal == Role.Tank);
                if (tank == null) tank = remaining.Find(p => p.RoleSecondaire == Role.Tank);
                if (tank != null)
                {
                    equipe.AjouterMembre(tank);
                    remaining.Remove(tank);
                }

                // Trouver support
                Personnage support = remaining.Find(p => p.RolePrincipal == Role.Support);
                if (support == null) support = remaining.Find(p => p.RoleSecondaire == Role.Support);
                if (support != null)
                {
                    equipe.AjouterMembre(support);
                    remaining.Remove(support);
                }

                // Trouver DPS
                List<Personnage> dpsList = remaining.FindAll(p => p.RolePrincipal == Role.DPS || p.RoleSecondaire == Role.DPS);
                if (dpsList.Count >= 2)
                {
                    equipe.AjouterMembre(dpsList[0]);
                    equipe.AjouterMembre(dpsList[1]);
                    remaining.Remove(dpsList[0]);
                    remaining.Remove(dpsList[1]);
                }

                if (equipe.Membres.Count == 4)
                {
                    repartition.AjouterEquipe(equipe);
                }
            }

            stopwatch.Stop();
            this.TempsExecution = stopwatch.ElapsedMilliseconds;
            return repartition;
        }

        private bool CanFormTeam(List<Personnage> remaining)
        {
            bool hasTank = remaining.Exists(p => p.RolePrincipal == Role.Tank || p.RoleSecondaire == Role.Tank);
            bool hasSupport = remaining.Exists(p => p.RolePrincipal == Role.Support || p.RoleSecondaire == Role.Support);
            int dpsCount = remaining.Count(p => p.RolePrincipal == Role.DPS || p.RoleSecondaire == Role.DPS);
            return hasTank && hasSupport && dpsCount >= 2;
        }
    }
}

