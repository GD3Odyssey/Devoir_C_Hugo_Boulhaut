using System;
using System.Collections.Generic;
//test
class Program
{
    static void Main()
    {
        string? choix;
        do
        {
            Scenario scenario = new Scenario();
            scenario.Debuter();
            Console.WriteLine("1. Recommencer l'histoire");
            Console.Write("Votre choix : ");
            choix = Console.ReadLine();

        } while (choix == "1");
    }
}
// Classe qui contient les statistiques du joueur
class Joueur
{
    public int Vie = 100;
    public int Potions = 3;
    public int Attaque = 40;
    public void Soigner()
    {
        if (Potions > 0)
        {
            Vie = 100;
            Potions--;
            Console.WriteLine("Votre vie est restaurée !");
        }
        else
            Console.WriteLine("Vous n'avez plus de potions !");
    }
}
// Classe qui contient les statistiques des ennemis
class Ennemi
{
    public string Nom;
    public int Vie;
    public int Degats;
    public Ennemi(string nom, int vie, int degats)
    {
        Nom = nom;
        Vie = vie;
        Degats = degats;
    }
}
class Scenario
{
    List<Page> pages = new List<Page>();
    List<string> choixStocker = new List<string>();
    Joueur joueur = new Joueur();
    Random hasard = new Random();
    public Scenario()
    {
        pages.Add(new Page("\nPage 1 : Vous êtes dans une forêt sombre. Quel chemin prendre ?\n", new List<string> { "Aller à gauche", "Aller à droite" }, new List<int> { 2, 2 }, 1));
        pages.Add(new Page("\nPage 2 : Vous trouvez un trésor !\n", new List<string> { "Prendre le trésor", "Laisser le trésor" }, new List<int> { 3, 4 }, 2));
        pages.Add(new Page("\nPage 3 : Un monstre surgit pour protéger le trésor !\n", new List<string> { "Combattre le monstre" }, new List<int> { 4 }, 3));
        pages.Add(new Page("\nPage 4 : Vous arrivez dans un village rempli d'habitants !\n", new List<string> { "Leur rendre visite", "Passer son chemin" }, new List<int> { 5, 9 }, 4));
        pages.Add(new Page("\nPage 5 : Les habitants vous accueillent pour la nuit et vous offrent une potion !\n", new List<string> { "Continuer dans la forêt" }, new List<int> { 6 }, 5));
        pages.Add(new Page("\nPage 6 : Un dragon vous bloque le passage ! Vous devez l'affronter !\n", new List<string> { "Combattre le dragon" }, new List<int> { 7 }, 6));
        pages.Add(new Page("\nPage 7 : Vous avez vaincu le dragon ! Vous trouvez son oeuf en or et devenez riche !\n", new List<string>(), new List<int>(), 7));
        pages.Add(new Page("\nPage 8 : Vous fuyez le dragon, mais vous vous perdez et mourez de faim !\n", new List<string>(), new List<int>(), 8));
        pages.Add(new Page("\nPage 9 : Vous trouvez une épée magique ! Vos dégâts sont doublés !\n", new List<string> { "Continuer dans la forêt" }, new List<int> { 6 }, 9));
    }
    class Page
    {
        string titre;
        List<string> choix;
        List<int> prochainesPages;
        int idPage;
        public Page(string titre, List<string> choix, List<int> prochainesPages, int idPage)
        {
            this.titre = titre;
            this.choix = choix;
            this.prochainesPages = prochainesPages;
            this.idPage = idPage;
        }
        public int Afficher()
        {
            Console.WriteLine("" + titre);
            if (choix.Count == 0)
                return -1;
            for (int i = 0; i < choix.Count; i++)
                Console.WriteLine($"{i + 1}. {choix[i]}");
            while (true)
            {
                Console.Write("Votre choix : ");
                if (int.TryParse(Console.ReadLine(), out int choix) && choix >= 1 && choix <= this.choix.Count)
                    return choix - 1;
                Console.WriteLine("Choix invalide. Réessayez.");
            }
        }
        public string GetChoix(int index)
        {
            return choix[index];
        }
        public List<int> GetProchainesPages()
        {
            return prochainesPages;
        }
    }
    public void Debuter()
    {
        int idPage = 1;
        while (joueur.Vie > 0)
        {
            Page page = pages[idPage - 1];
            Console.WriteLine($"Vie : {joueur.Vie}/100 | Potions : {joueur.Potions} | Attaque : {joueur.Attaque}");
            int choix = page.Afficher();
            if (choix == -1)
                break;
            choixStocker.Add(page.GetChoix(choix));
            int prochainePage = page.GetProchainesPages()[choix];
            // Combat contre le monstre
            if (idPage == 3)
            {
                Ennemi monstre = new Ennemi("Monstre", 100, 10);
                int resultat = Combattre(monstre);
                if (resultat == 0)
                    break;
                if (resultat == 1)
                {
                    joueur.Potions++;
                    Console.WriteLine("Vous avez vaincu le monstre !");
                    Console.WriteLine("Vous récupérez une potion !");
                }
                else if (resultat == 2)
                {
                    Console.WriteLine("Vous abandonnez le trésor et rejoignez le village.");
                }
            }
            // Récompense du village
            if (idPage == 5)
            {
                joueur.Potions++;
                Console.WriteLine("Les habitants vous offrent une potion !");
            }
            // Épée qui double les dégâts
            if (idPage == 9)
            {
                joueur.Attaque *= 2;
            }
            // Combat contre le dragon
            if (idPage == 6)
            {
                Ennemi dragon = new Ennemi("Dragon", 200, 20);
                int resultat = Combattre(dragon);
                if (resultat == 0)
                    break;
                if (resultat == 2)
                    prochainePage = 8;
            }
            idPage = prochainePage;
        }
        if (joueur.Vie <= 0)
        {
            Console.WriteLine("GAME OVER ! Vous êtes mort !");
            Console.WriteLine("\nTHE END !");
            Console.WriteLine("\nVoici vos choix :");
            foreach (string choix in choixStocker)
            Console.WriteLine(choix);
        }
        
    }
    // Retour : 0 = mort, 1 = victoire, 2 = fuite
    public int Combattre(Ennemi ennemi)
    {
        Console.WriteLine($"Vous combattez le {ennemi.Nom} !");
        while (joueur.Vie > 0 && ennemi.Vie > 0)
        {
            Console.WriteLine($"Votre vie : {joueur.Vie}/100");
            Console.WriteLine($"Vie du {ennemi.Nom} : {ennemi.Vie}");
            Console.WriteLine($"Potions : {joueur.Potions}");
            Console.WriteLine("1. Attaquer");
            Console.WriteLine("2. Fuir");
            Console.WriteLine("3. Se soigner");
            Console.Write("Votre choix : ");
            string? input = Console.ReadLine();
            if (!int.TryParse(input, out int choix) || choix < 1 || choix > 3)
            {
                Console.WriteLine("Choix invalide !");
                continue;
            }
            if (choix == 1)
            {
                ennemi.Vie -= joueur.Attaque;
                Console.WriteLine($"Vous infligez {joueur.Attaque} dégâts !");
            }
            else if (choix == 2)
            {
                // Une chance sur cinq de réussir à fuir
                if (hasard.Next(1, 6) == 1)
                {
                    Console.WriteLine("Vous avez réussi à fuir !");
                    return 2;
                }
                Console.WriteLine("Fuite ratée !");
            }
            else if (choix == 3)
            {
                if (joueur.Potions == 0)
                {
                    Console.WriteLine("Vous n'avez plus de potions !");
                    continue;
                }
                joueur.Soigner();
            }
            // L'ennemi attaque s'il est vivant
            if (ennemi.Vie > 0)
            {
                joueur.Vie -= ennemi.Degats;
                Console.WriteLine($"Le {ennemi.Nom} vous inflige {ennemi.Degats} dégâts !");
            }
        }
        return joueur.Vie <= 0 ? 0 : 1;
    }
}

