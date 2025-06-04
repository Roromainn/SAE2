using System;
using TeamsMaker_METIER.Personnages.Generation;

class Program
{
    static void Main(string[] args)
    {

        Generateur.GenererPersonnages(
            nbPersonnages: 50,
            mode: "aleatoire"
        );

        Console.WriteLine("Terminé. Appuyez sur une touche pour quitter.");
        Console.ReadKey();
    }
}



