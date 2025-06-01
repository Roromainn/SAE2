using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.JeuxTest;
using TeamsMaker_METIER.Personnages;

namespace TeamsMaker_METIER.Algorithmes.Realisations
{
    /// <summary>
    /// Un algorithme de test
    /// </summary>
    public class AlgorithmeTest : Algorithme
    {
        public override Repartition Repartir(JeuTest jeuTest)
        {
            Repartition repartition = new Repartition(jeuTest);
            for (int i = 0; i < jeuTest.Personnages.Count() / 4; i++)
            {
                Equipe equipe = new Equipe();
                for(int j = 4*i;j<4*(i+1);j++) equipe.AjouterMembre(jeuTest.Personnages[j]);
                repartition.AjouterEquipe(equipe);
            }
            return repartition;
        }
    }
}
