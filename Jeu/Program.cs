// Maisie et l'Indominus se déplacent de manière aléatoire d'une case à la fois

void DeplacementAleatoire(string personnage, int x, int y) 
{
    plateau[x,y] = plateau[⬜]; //Réinitialise le plateau
    Random rng = new Random();
    int nbrCaseX = rng.Next(-1,2); // Génère un chiffre aléatoire entre -1 et 1 pour changer la valeur de la coordonnée x
    int nbrCaseY = rng.Next(-1,2); // Génère un chiffre aléatoire entre -1 et 1 pour changer la valeur de la coordonnée y
    while (nbrCaseX == 0 && nbrCaseY == 0) // Eviter que le déplacement soit nul (les deux coordonnées restent les mêmes)
    {
        nbrCaseX = rng.Next(-1,2);
        nbrCaseY = rng.Next(-1,2);
    }
    int nouvelleCoorX = x + nbrCaseX; // Ajoute la valeur aléatoire à la coordonnée initiale
    int nouvelleCoorY = y + nbrCaseY;
    while (nouvelleCoorX < 0 || nouvelleCoorY < 0) // Evite que les nouvelles coordonnées soient négatives et donc qu'elles sortent du plateau 
    {
        nbrCaseX = rng.Next(-1,2);
        nbrCaseY = rng.Next(-1,2);
        nouvelleCoorX = x + nbrCaseX; 
        nouvelleCoorY = y + nbrCaseY;
    } 
    plateau [nouvelleCoorX,nouvelleCoorY] = plateau [personnage]; // Affiche la nouvelle position du personnage 
    
}
DeplacementAleatoire(6,9);


// Si l'Indominus est énervée elle peut se déplacer de 2 cases à la fois

void DeplacementAleatoireEnervee (string personnage, int x, int y) 
{
    plateau[x,y] = plateau[⬜]; //Réinitialise le plateau
    
    Random rng = new Random();
    int nbrCaseX = rng.Next(-2,3); // Génère un chiffre aléatoire entre -2 et 2 pour changer la valeur de la coordonnée x
    int nbrCaseY = rng.Next(-2,3); // Génère un chiffre aléatoire entre -2 et 2 pour changer la valeur de la coordonnée y
    
    while (nbrCaseX == 0 && nbrCaseY == 0) // Eviter que le déplacement soit nul (les deux coordonnées restent les mêmes)
    {
        nbrCaseX = rng.Next(-2,3);
        nbrCaseY = rng.Next(-2,3);
    }
    int nouvelleCoorX = x + nbrCaseX; // Ajoute la valeur aléatoire à la coordonnée initiale
    int nouvelleCoorY = y + nbrCaseY;
    
    while (nouvelleCoorX < 0 || nouvelleCoorY < 0) // Evite que les nouvelles coordonnées soient négatives et donc qu'elles sortent du plateau 
    {
        nbrCaseX = rng.Next(-2,3);
        nbrCaseY = rng.Next(-2,3);
        nouvelleCoorX = x + nbrCaseX; 
        nouvelleCoorY = y + nbrCaseY;
    }
    plateau [nouvelleCoorX,nouvelleCoorY] = plateau [personnage]; // Affiche la nouvelle position de l'Indominus
    Console.WriteLine($"{nouvelleCoorX} {nouvelleCoorY}");
}
DeplacementAleatoireEnervee(6,9);


// Déplace le personnage d'une case à l'aide des flèches du clavier

void DeplacementClavier(string personnage, int x, int y)
{
    Console.WriteLine("Presser une fléches du clavier pour déplacer le personnage");
    
    plateau[x,y] = plateau[⬜]; // Réinitialise le plateau

    ConsoleKeyInfo key = Console.ReadKey(intercept: true);
    if (key.Key == ConsoleKey.LeftArrow && x > 0) // Flèche gauche
    {
        x -= 1;
    }
    else if (key.Key == ConsoleKey.RightArrow && x < Console.WindowWidth - 1) // Flèche droite
    {
        x += 1;
    }
    else if (key.Key == ConsoleKey.UpArrow && y > 0) // Flèche haut
    {
        y -= 1;
    }
    else if (key.Key == ConsoleKey.DownArrow && y < Console.WindowHeight - 1) // Flèche bas
    {
        y += 1;
    }
    
    plateau[x,y] = plateau[personnage]; // Affiche le personnage sur sa nouvelle position
    Console.WriteLine($"{x} {y}");
}
DeplacementClavier(5,6);


  
}
