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
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Repartition repartition = new Repartition(jeuTest);

            // Séparation par rôles principaux uniquement
            List<Personnage> tanks = new List<Personnage>();
            List<Personnage> supports = new List<Personnage>();
            List<Personnage> dps = new List<Personnage>();

            foreach (Personnage p in jeuTest.Personnages)
            {
                switch (p.RolePrincipal)
                {
                    case Role.TANK:
                        tanks.Add(p);
                        break;
                    case Role.SUPPORT:
                        supports.Add(p);
                        break;
                    case Role.DPS:
                        dps.Add(p);
                        break;
                }
            }

            // Tri par niveau principal
            tanks.Sort(new ComparateurPersonnageParNiveauPrincipal());
            supports.Sort(new ComparateurPersonnageParNiveauPrincipal());
            dps.Sort(new ComparateurPersonnageParNiveauPrincipal());

            // Calcul du nombre maximal d'équipes possibles
            int maxTeams = Math.Min(
                Math.Min(tanks.Count, supports.Count),
                dps.Count / 2
            );

            // Formation des équipes
            for (int i = 0; i < maxTeams; i++)
            {
                Equipe equipe = new Equipe();

                // Tank
                if (i < tanks.Count)
                    equipe.AjouterMembre(tanks[i]);

                // Support
                if (i < supports.Count)
                    equipe.AjouterMembre(supports[i]);

                // DPS (2 par équipe)
                int dpsIndex1 = 2 * i;
                int dpsIndex2 = 2 * i + 1;

                if (dpsIndex1 < dps.Count)
                    equipe.AjouterMembre(dps[dpsIndex1]);

                if (dpsIndex2 < dps.Count)
                    equipe.AjouterMembre(dps[dpsIndex2]);

                // Validation que l'équipe est complète et valide
                if (equipe.Membres.Length == 4 &&
                    equipe.Membres.Count(m => m.RolePrincipal == Role.TANK) == 1 &&
                    equipe.Membres.Count(m => m.RolePrincipal == Role.SUPPORT) == 1 &&
                    equipe.Membres.Count(m => m.RolePrincipal == Role.DPS) == 2)
                {
                    repartition.AjouterEquipe(equipe);
                }
            }

            stopwatch.Stop();
            this.TempsExecution = stopwatch.ElapsedMilliseconds;
            return repartition;
        }
        }
    }


