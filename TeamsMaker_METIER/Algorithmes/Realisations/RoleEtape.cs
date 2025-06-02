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
    internal class RoleEtape : Algorithme
    {
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

            // Trier les tanks par proximité au niveau 50
            TrierParProximite(tanks);

            // Trier les supports par proximité au niveau 50
            TrierParProximite(supports);

            // Trier les DPS par proximité au niveau 50
            TrierParProximite(dps);

            // Formation des équipes optimales
            while (tanks.Count > 0 && supports.Count > 0 && dps.Count >= 2)
            {
                var tank = tanks[0]; // Équivalent de First()
                var support = supports[0]; // Équivalent de First()

                // Trouver les 2 DPS qui minimisent le score
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

        // Méthode pour trier une liste par proximité au niveau 50
        private void TrierParProximite(List<Personnage> personnages)
        {
            // Tri par sélection simple
            for (int i = 0; i < personnages.Count - 1; i++)
            {
                int indexMin = i;
                int scoreMin = Math.Abs(personnages[i].LvlPrincipal - 50);

                for (int j = i + 1; j < personnages.Count; j++)
                {
                    int score = Math.Abs(personnages[j].LvlPrincipal - 50);
                    if (score < scoreMin)
                    {
                        scoreMin = score;
                        indexMin = j;
                    }
                }

                // Échanger si nécessaire
                if (indexMin != i)
                {
                    Personnage temp = personnages[i];
                    personnages[i] = personnages[indexMin];
                    personnages[indexMin] = temp;
                }
            }
        }

        
    }
}