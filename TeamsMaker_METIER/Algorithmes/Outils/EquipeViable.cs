using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.Personnages.Classes;
using TeamsMaker_METIER.Personnages;

namespace TeamsMaker_METIER.Algorithmes.Outils
{
    /// <summary>
    /// Classe utilitaire pour vérifier si une équipe est viable.
    /// </summary>
    internal static class EquipeViable
    {
        /// <summary>
        /// Vérifie si une équipe est viable en fonction des rôles des personnages restants.
        /// </summary>
        /// <param name="restant">liste des personages restants</param>
        /// <returns></returns>
        public static bool FormationEquipe(List<Personnage> restant)
        {
            bool hasTank = false;
            bool hasSupport = false;
            int dpsCount = 0;

            foreach (Personnage p in restant)
            {
                if (p.RolePrincipal == Role.TANK || p.RoleSecondaire == Role.TANK)
                {
                    hasTank = true;
                }

                if (p.RolePrincipal == Role.SUPPORT || p.RoleSecondaire == Role.SUPPORT)
                {
                    hasSupport = true;
                }

                if (p.RolePrincipal == Role.DPS || p.RoleSecondaire == Role.DPS)
                {
                    dpsCount++;
                }
            }

            return hasTank && hasSupport && dpsCount >= 2;
        }
    }
}
