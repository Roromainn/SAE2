using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.Personnages;

namespace TeamsMaker_METIER.Algorithmes.Outils
{
    /// <summary>
    /// Classe utilitaire pour former des paires de DPS à partir d'une liste de personnages.
    /// </summary>
    internal static class Paire
    {
        /// <summary>
        /// Forme une paire de DPS à partir d'une liste de personnages, en cherchant la paire dont le niveau moyen est le plus proche de 50.
        /// </summary>
        /// <param name="dps">liste de DPS dispo</param>
        /// <param name="tankLevel">niveau du tank</param>
        /// <param name="supportLevel">niveau du support</param>
        /// <returns>Tuple avec les 2 DPS qui conviennent a l'equipe</returns>
        public static Tuple<Personnage, Personnage> PaireDeDPS(List<Personnage> dps, int tankLevel, int supportLevel)
        {
            if (dps.Count < 2)
                return null;

            Tuple<Personnage, Personnage> bestPair = null;
            double bestScore = double.MaxValue;

            int limiteRecherche = Math.Min(20, dps.Count);

            // Limiter la recherche aux 20 premiers DPS pour accelerer la recherche (déterminé de maniere empirique)
            for (int i = 0; i < limiteRecherche; i++)
            {
                for (int j = i + 1; j < limiteRecherche; j++)
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

            // Retourner la meilleure paire trouvée, ou les 2 premiers si aucune trouvée
            if (bestPair == null)
            {
                bestPair = Tuple.Create(dps[0], dps[1]);
            }

            return bestPair;
        }
    }
}
