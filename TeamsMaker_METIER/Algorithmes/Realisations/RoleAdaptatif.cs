using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages.Classes;
using TeamsMaker_METIER.Personnages;
using TeamsMaker_METIER.Algorithmes.Outils;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    internal class RoleAdaptatif : Algorithme
    {
        private const int NIVEAU_CIBLE = 50;

        public override Repartition Repartir(JeuTest jeuTest)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Repartition repartition = new Repartition(jeuTest);
            List<Personnage> remaining = new List<Personnage>(jeuTest.Personnages);

            while (TeamViable.FormationEquipe(remaining))
            {
                Equipe equipe = new Equipe();

                // 1. Tank le plus proche du niveau cible
                Personnage tank = null;
                int meilleurScoreTank = int.MaxValue;

                // Chercher tous les tanks disponibles
                for (int i = 0; i < remaining.Count; i++)
                {
                    Personnage p = remaining[i];

                    // Vérifier si c'est un tank (équivalent du Where)
                    if (p.RolePrincipal == Role.TANK || p.RoleSecondaire == Role.TANK)
                    {
                        // Calculer la distance au niveau cible (équivalent d'OrderBy)
                        int distance = Math.Abs(p.LvlPrincipal - NIVEAU_CIBLE);

                        // Garder le meilleur tank trouvé (équivalent de FirstOrDefault)
                        if (distance < meilleurScoreTank)
                        {
                            meilleurScoreTank = distance;
                            tank = p;
                        }
                    }
                }

                if (tank == null) break;
                equipe.AjouterMembre(tank);
                remaining.Remove(tank);

                // 2. Support le plus proche du niveau cible
                Personnage support = null;
                int meilleurScoreSupport = int.MaxValue;

                for (int i = 0; i < remaining.Count; i++)
                {
                    Personnage p = remaining[i];

                    if (p.RolePrincipal == Role.SUPPORT || p.RoleSecondaire == Role.SUPPORT)
                    {
                        int distance = Math.Abs(p.LvlPrincipal - NIVEAU_CIBLE);

                        if (distance < meilleurScoreSupport)
                        {
                            meilleurScoreSupport = distance;
                            support = p;
                        }
                    }
                }

                if (support == null) break;
                equipe.AjouterMembre(support);
                remaining.Remove(support);

                // 3. Deux DPS les plus proches du niveau cible (SANS TRI COMPLET)
                Personnage meilleurDps1 = null;
                Personnage meilleurDps2 = null;
                int scoreDps1 = int.MaxValue;
                int scoreDps2 = int.MaxValue;

                // Un seul passage pour trouver les 2 meilleurs DPS
                for (int i = 0; i < remaining.Count; i++)
                {
                    Personnage p = remaining[i];

                    if (p.RolePrincipal == Role.DPS || p.RoleSecondaire == Role.DPS)
                    {
                        int distance = Math.Abs(p.LvlPrincipal - NIVEAU_CIBLE);

                        if (distance < scoreDps1)
                        {
                            // Nouveau meilleur : l'ancien meilleur devient le 2ème
                            meilleurDps2 = meilleurDps1;
                            scoreDps2 = scoreDps1;
                            meilleurDps1 = p;
                            scoreDps1 = distance;
                        }
                        else if (distance < scoreDps2)
                        {
                            // Nouveau 2ème meilleur
                            meilleurDps2 = p;
                            scoreDps2 = distance;
                        }
                    }
                }

                if (meilleurDps1 == null || meilleurDps2 == null) break;

                equipe.AjouterMembre(meilleurDps1);
                equipe.AjouterMembre(meilleurDps2);
                remaining.Remove(meilleurDps1);
                remaining.Remove(meilleurDps2);

                repartition.AjouterEquipe(equipe);
            }

            stopwatch.Stop();
            this.TempsExecution = stopwatch.ElapsedMilliseconds;
            return repartition;
        }

        
    }
}