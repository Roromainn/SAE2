using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.Personnages;

namespace TeamsMaker_METIER.Algorithmes.Outils
{
    internal static class Paire
    {
        public  static Tuple<Personnage, Personnage> PaireDeDPS(List<Personnage> dps, int tankLevel, int supportLevel)
        {
            if (dps.Count < 2)
                return null;

            Tuple<Personnage, Personnage> bestPair = null;
            double bestScore = double.MaxValue;

            int limiteRecherche = Math.Min(20, dps.Count);

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
