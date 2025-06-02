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
    /// <summary>
    /// Algorithme de répartition des personnages par rôle principal.
    /// </summary>
    internal class RoleEtape : Algorithme
    {
        /// <summary>
        /// Répartit les personnages d'un jeu de test en équipes basées sur leur rôle principal.
        /// </summary>
        /// <param name="jeuTest">Fichier de jeu de test contenant la liste de perso</param>
        /// <returns>Composition "optimale" des equipes selon la méthode décrite dans le document joint</returns>
        public override Repartition Repartir(JeuTest jeuTest)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            Repartition repartition = new Repartition(jeuTest);

            List<Personnage> tanks = new List<Personnage>();
            List<Personnage> supports = new List<Personnage>();
            List<Personnage> dps = new List<Personnage>();

            // Trier les perso par rôle
            for (int i = 0; i < jeuTest.Personnages.Length; i++)
            {
                Personnage p = jeuTest.Personnages[i];

                if (p.RolePrincipal == Role.TANK)
                {
                    tanks.Add(p);
                }
                else if (p.RolePrincipal == Role.SUPPORT)
                {
                    supports.Add(p);
                }
                else if (p.RolePrincipal == Role.DPS)
                {
                    dps.Add(p);
                }
            }

            //tri de chaque classe en focntion de la distance a 50
            CalculProxi.TrierParProximite(tanks);
            CalculProxi.TrierParProximite(supports);
            CalculProxi.TrierParProximite(dps);

            //composition des equipes
            while (tanks.Count > 0 && supports.Count > 0 && dps.Count >= 2)
            {
                var tank = tanks[0]; 
                var support = supports[0]; 
                var bestDpsPair = Paire.PaireDeDPS(dps, tank.LvlPrincipal, support.LvlPrincipal);
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
    }
}