using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsMaker_METIER.Personnages.Classes;
using TeamsMaker_METIER.Personnages;

namespace TeamsMaker_METIER.Algorithmes.Outils
{
    internal static class TeamViable
    {
        public static bool FormationEquipe (List<Personnage> remaining)
        {
            bool hasTank = remaining.Exists(p => p.RolePrincipal == Role.TANK || p.RoleSecondaire == Role.TANK);
            bool hasSupport = remaining.Exists(p => p.RolePrincipal == Role.SUPPORT || p.RoleSecondaire == Role.SUPPORT);
            int dpsCount = remaining.Count(p => p.RolePrincipal == Role.DPS || p.RoleSecondaire == Role.DPS);

            return hasTank && hasSupport && dpsCount >= 2;
        }
    }
}
