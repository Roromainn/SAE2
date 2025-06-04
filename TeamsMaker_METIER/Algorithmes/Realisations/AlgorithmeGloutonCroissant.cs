using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.Algorithmes.Outils;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    public class AlgorithmeGloutonCroissant : Algorithme
    {

        public override Repartition Repartir(JeuTest jeuTest)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Repartition repartition = new Repartition(jeuTest);
            Personnage[] monTableau = jeuTest.Personnages;
            Array.Sort(monTableau, new ComparateurPersonnageParNiveauPrincipal());
            for (int i = 0; i < monTableau.Length/4; i++)
            {
                Equipe equipe = new Equipe();
                for (int j = 4*i; j < 4*(i+1) ; j++)
                {
                    equipe.AjouterMembre(monTableau[j]);
                }
                repartition.AjouterEquipe(equipe);
            }
            
            stopwatch.Stop();
            this.TempsExecution = stopwatch.ElapsedMilliseconds;
            return repartition;
        }
    }
}
