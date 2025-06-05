using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.Personnages;

namespace TeamsMaker_METIER.Algorithmes.Outils
{
    /// <summary>
    /// Classe utilitaire pour trier les personnages par proximité de niveau principal.
    /// </summary>
    public static class CalculProxi
    {
        #region--Méthodes--
        /// <summary>
        /// Trie une liste de personnages en fonction de leur niveau principal, en cherchant à les rapprocher du niveau 50.
        /// </summary>
        /// <param name="personnages">liste de personnage que l'on veut classer</param>
        public static void TrierParProximite(List<Personnage> personnages)
        {
            // Tri  sélection 
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
                if (indexMin != i)
                {
                    Personnage temp = personnages[i];
                    personnages[i] = personnages[indexMin];
                    personnages[indexMin] = temp;
                }
            }
        }
        #endregion
    }
}
