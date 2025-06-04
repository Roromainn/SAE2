using System;

class Program
{
    static void Main(string[] args)
    {

        GenerateurPersonnages.GenererPersonnages(
            nbPersonnages: 50,
            mode: "aleatoire"
        );

        Console.WriteLine("Terminé. Appuyez sur une touche pour quitter.");
        Console.ReadKey();
    }
}



