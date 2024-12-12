﻿//Valeurs pour tester A SUPPRIMER
int positionYOwen = 0;
int positionXOwen = 0;
int positionYIR = 0;
int positionXIR = 0;
int positionYMaisie = 0;
int positionXMaisie = 0;
int positionYBlue = 0;
int positionXBlue = 0;
Console.WriteLine("Déterminez la hauteur du plateau :");
int hauteurPlateau = Convert.ToInt32(Console.ReadLine()!);
Console.WriteLine("Déterminez la longueur du plateau :");
int longueurPlateau = Convert.ToInt32(Console.ReadLine()!);
string[,] plateau = CréerPlateau(hauteurPlateau, longueurPlateau);
int nbGrenade = plateau.GetLength(1);
int nbGrenadeSpe = 1;
int pdvMaisie = 100;
int pdvBlue = 100;
int pdvIR = 10 * nbGrenade;
int coorYGrenadeSpe;
int coorXGrenadeSpe;
string nomMaisie = "Maisie";
string nomBlue = "Blue";
string nomOwen = "Owen";
Random rng = new Random();
bool enervement = false;

// Lancer ou non d'une grenade, spéciale ou non
void Grenade(int positionYOwen, int positionXOwen, int nbGrenade, int pdvIR, int pdvBlue, int pdvMaisie)
{
    int coorYGrenade;
    int coorXGrenade;
    int randomY = 0;
    int randomX = 0;
    Console.WriteLine("Lancer une grenade? (répondre Oui ou Non)");
    string reponse = Console.ReadLine()!;
    if (reponse == "Oui")
    {
        Console.WriteLine("Lancer une grenade spéciale ou normale?");
        string type = Console.ReadLine()!;
        if ((type == "spéciale") || (type == "Spéciale"))
        {
            if (nbGrenadeSpe > 0)
            {
                nbGrenadeSpe -= 1;
                Console.WriteLine("Sélectionnez où lancer la grenade:");
                Console.WriteLine("Entrez le numéro de ligne :");
                coorYGrenade = Convert.ToInt32(Console.ReadLine()!);
                Console.WriteLine("Entrez le numéro de colonne :");
                coorXGrenade = Convert.ToInt32(Console.ReadLine()!);
                if ((coorXGrenade >= plateau.GetLength(1)) || (coorXGrenade < 0) || (coorYGrenade >= plateau.GetLength(0)) || (coorYGrenade < 0))
                {
                    Console.WriteLine("Impossible, c'est en dehors des limites du plateau");
                    Console.WriteLine("Sélectionnez où lancer la grenade:");
                    Console.WriteLine("Entrez le numéro de ligne :");
                    coorYGrenade = Convert.ToInt32(Console.ReadLine()!);
                    Console.WriteLine("Entrez le numéro de colonne :");
                    coorXGrenade = Convert.ToInt32(Console.ReadLine()!);
                }
                while ((coorYGrenade >= positionYOwen + 3) || (coorYGrenade <= positionYOwen - 3) || (coorXGrenade >= positionXOwen + 3) || (coorXGrenade <= positionXOwen - 3))
                {
                    Console.WriteLine("Impossible, Owen a une portée de 3 cases maximum");
                    Console.WriteLine("Sélectionnez où lancer la grenade:");
                    Console.WriteLine("Entrez le numéro de ligne :");
                    coorYGrenade = Convert.ToInt32(Console.ReadLine()!);
                    Console.WriteLine("Entrez le numéro de colonne :");
                    coorXGrenade = Convert.ToInt32(Console.ReadLine()!);
                }
                if ((coorYGrenade <= positionYOwen + 3) || (coorYGrenade >= positionYOwen - 3) || (coorXGrenade <= positionXOwen + 3) || (coorXGrenade >= positionXOwen - 3))
                {
                    if (plateau[coorYGrenade, coorXGrenade] == "🟥") // Savoir si l'IR a été touchée et lui enlever des pv et pas de crevasse
                    {
                        Console.WriteLine("IR a été touchée, -10 points de vie");
                        pdvIR -= 10;
                        Console.WriteLine($"Points de vie de l'IR : {pdvIR}");
                        enervement = true; //sera plus rapide au prochain déplacement
                        Console.WriteLine("IR est énervée, faites attention au prochain tour");
                    }
                    else if (plateau[coorYGrenade, coorXGrenade] == "🟦")
                    {
                        Console.WriteLine("Blue a été tuée par Owen. Fin de la partie.");
                    }
                    else if (plateau[coorYGrenade, coorXGrenade] == "🟪")
                    {
                        Console.WriteLine("Maisie a été tuée par Owen. Fin de la partie.");
                    }
                    else // sinon on crée une crevasse
                    {
                        plateau[coorYGrenade, coorXGrenade] = "💥";
                        while ((randomY == 0) && (randomX == 0) && ((coorXGrenade + randomX <= plateau.GetLength(1) - 1) || (coorYGrenade + randomY <= plateau.GetLength(0) - 1) || (coorXGrenade + randomX > 0) || (coorYGrenade + randomY > 0))) // Pour éviter que la case random soit la même que celle où la grenade atterit et prendre en compte les bordures
                        {
                            randomY = rng.Next(-1, 2);
                            randomX = rng.Next(-1, 2);
                        }
                        coorYGrenadeSpe = coorYGrenade + randomY;
                        coorXGrenadeSpe = coorXGrenade + randomX;
                        if (plateau[coorYGrenadeSpe, coorXGrenadeSpe] == "🟦")
                        {
                            SystèmePV(pdvBlue, nomBlue, nomOwen);
                        }
                        else if (plateau[coorYGrenadeSpe, coorXGrenadeSpe] == "🟪")
                        {
                            SystèmePV(pdvMaisie, nomMaisie, nomOwen);
                        }
                        else
                            plateau[coorYGrenadeSpe, coorXGrenadeSpe] = "💥";
                        do
                        {
                            randomY = rng.Next(-1, 2);
                            randomX = rng.Next(-1, 2);
                        }
                        while ((randomY == 0) && (randomX == 0) && ((coorXGrenadeSpe + randomX <= plateau.GetLength(1) - 1) || (coorYGrenadeSpe + randomY <= plateau.GetLength(0) - 1) || (coorXGrenadeSpe + randomX > 0) || (coorYGrenadeSpe + randomY > 0))); // Pour éviter que la case random soit la même que celle où la grenade atterit
                        {
                            randomY = rng.Next(-1, 2);
                            randomX = rng.Next(-1, 2);
                        }
                        if (plateau[coorYGrenadeSpe + randomY, coorXGrenadeSpe + randomX] == "🟦")
                        {
                            SystèmePV(pdvBlue, nomBlue, nomOwen);
                        }
                        else if (plateau[coorYGrenadeSpe + randomY, coorXGrenadeSpe + randomX] == "🟪")
                        {
                            SystèmePV(pdvMaisie, nomMaisie, nomOwen);
                        }
                        plateau[coorYGrenadeSpe + randomY, coorXGrenadeSpe + randomX] = "💥";
                    }

                }

            }
            else
            {
                Console.WriteLine("Vous n'avez plus de grenade spéciale");
                Console.WriteLine("Voulez vous lancer une grenade normale? (si oui, répondre normale)");
            }
        }

        else if ((type == "normale") || (type == "Normale"))
        {
            if (nbGrenade > 0)
            {
                nbGrenade -= 1;
                Console.WriteLine("Sélectionnez où lancer la grenade:");
                Console.WriteLine("Entrez le numéro de ligne :");
                coorYGrenade = Convert.ToInt32(Console.ReadLine()!);
                Console.WriteLine("Entrez le numéro de colonne :");
                coorXGrenade = Convert.ToInt32(Console.ReadLine()!);
                if ((coorXGrenade >= plateau.GetLength(1)) || (coorXGrenade < 0) || (coorYGrenade >= plateau.GetLength(0)) || (coorYGrenade < 0))
                {
                    Console.WriteLine("Impossible, c'est en dehors des limites du plateau");
                    Console.WriteLine("Sélectionnez où lancer la grenade:");
                    Console.WriteLine("Entrez le numéro de ligne :");
                    coorYGrenade = Convert.ToInt32(Console.ReadLine()!);
                    Console.WriteLine("Entrez le numéro de colonne :");
                    coorXGrenade = Convert.ToInt32(Console.ReadLine()!);
                }
                while ((coorYGrenade >= positionYOwen + 3) || (coorYGrenade <= positionYOwen - 3) || (coorXGrenade >= positionXOwen + 3) || (coorXGrenade <= positionXOwen - 3))
                {
                    Console.WriteLine("Impossible, Owen a une portée de 3 cases maximum");
                    Console.WriteLine("Sélectionnez où lancer la grenade:");
                    Console.WriteLine("Entrez le numéro de ligne :");
                    coorYGrenade = Convert.ToInt32(Console.ReadLine()!);
                    Console.WriteLine("Entrez le numéro de colonne :");
                    coorXGrenade = Convert.ToInt32(Console.ReadLine()!);
                }
                if ((coorYGrenade <= positionYOwen + 3) || (coorYGrenade >= positionYOwen - 3) || (coorXGrenade <= positionXOwen + 3) || (coorXGrenade >= positionXOwen - 3))
                {
                    if (plateau[coorYGrenade, coorXGrenade] == "🟥") // Savoir si l'IR a été touchée et lui enlever des pv et pas de crevasse
                    {
                        Console.WriteLine("IR a été touchée, -10 points de vie");
                        pdvIR -= 10;
                        Console.WriteLine($"Points de vie de l'IR : {pdvIR}");
                        enervement = true; //sera plus rapide au prochain déplacement
                        Console.WriteLine("IR est énervée, faites attention au prochain tour");
                    }
                    else if (plateau[coorYGrenade, coorXGrenade] == "🟦")
                        Console.WriteLine("Blue a été tuée par Owen. Fin de la partie.");
                    else if (plateau[coorYGrenade, coorXGrenade] == "🟪")
                        Console.WriteLine("Maisie a été tuée par Owen. Fin de la partie.");
                    else // sinon on crée une crevasse
                    {
                        plateau[coorYGrenade, coorXGrenade] = "💥";
                        while ((randomY == 0) && (randomX == 0) && ((coorXGrenade + randomX <= plateau.GetLength(1) - 1) || (coorYGrenade + randomY <= plateau.GetLength(0) - 1) || (coorXGrenade + randomX > 0) || (coorYGrenade + randomY > 0))) // Pour éviter que la case random soit la même que celle où la grenade atterit
                        {
                            randomY = rng.Next(-1, 2);
                            randomX = rng.Next(-1, 2);
                        }
                        if (plateau[coorYGrenade + randomY, coorXGrenade + randomX] == "🟦")
                        {
                            SystèmePV(pdvBlue, nomBlue, nomOwen);
                        }
                        else if (plateau[coorYGrenade + randomY, coorXGrenade + randomX] == "🟪")
                        {
                            SystèmePV(pdvMaisie, nomMaisie, nomOwen);
                        }
                        else
                            plateau[coorYGrenade + randomY, coorXGrenade + randomX] = "💥";
                    }
                }
            }
            if (nbGrenade == 0)
                Console.WriteLine("Vous n'avez plus de grenades, bonne chance!");
        }
    }
    else if (reponse == "Non")
    {
        Console.WriteLine("Owen ne lance pas de grenade.");
    }
    AfficherPlateau(plateau);
    Console.WriteLine($"Il vous reste {nbGrenade} grenades et {nbGrenadeSpe} grenades spéciales.");
}


//Sous programme pour gérer les points de vie de Maisie et Blue en cas d'impact
void SystèmePV(int pV, string nom, string owen)
{
    pV /= 2;
    if (pV == 0)
        Console.WriteLine($"{nom} a été tuée par {owen}. Fin de la partie.");
    if (pdvMaisie > 0)
        Console.WriteLine($"{nom} a été touchée par l'impact, attention");
}

void PouvoirBlue(ref int positionYIR, ref int positionXIR) // Lancé que si Blue et IR en même position
{
    plateau[positionYIR, positionXIR] = "⬜"; // Supprimer le caractère I du plateau aux anciennes positions
    Console.WriteLine("Sélectionnez la direction dans laquelle envoyer l'IR: Nord, Sud, Est ou Ouest ?");
    string direction = Console.ReadLine()!;
    if (direction == "Ouest")
    {
        for (int i = 1; i <= 3; i++)
        {
            if ((positionXIR - i < 0) || (plateau[positionYIR, positionXIR - i] == "💥")) // Si on dépasse les limites du plateau ou qu'une crevasse est atteinte
                positionXIR -= (i - 1);

        }
        //Si absence de crevasse et de bordure
        if ((positionXIR - 3 >= 0) && (plateau[positionYIR, positionXIR - 1] != "💥") && (plateau[positionYIR, positionXIR - 2] != "💥") && (plateau[positionYIR, positionXIR - 3] != "💥"))
            positionXIR -= 3;
    }
    else if (direction == "Est")
    {
        for (int i = 1; i <= 3; i++)
        {
            if ((positionXIR + i >= plateau.GetLength(1) - 1) || (plateau[positionYIR, positionXIR + i] == "💥"))
                positionXIR += (i - 1);

        }
        if ((positionXIR + 3 < plateau.GetLength(1)) && (plateau[positionYIR, positionXIR + 1] != "💥") && (plateau[positionYIR, positionXIR + 2] != "💥") && (plateau[positionYIR, positionXIR + 3] != "💥"))
            positionXIR += 3;
    }

    else if (direction == "Sud")
    {
        for (int i = 1; i <= 3; i++)
        {
            if ((positionYIR + i >= plateau.GetLength(0) - 1) || (plateau[positionYIR + i, positionXIR] == "💥"))
                positionYIR += (i - 1);
        }
        if ((positionYIR + 3 < plateau.GetLength(1)) && (plateau[positionYIR + 1, positionXIR] != "💥") && (plateau[positionYIR + 2, positionXIR] != "💥") && (plateau[positionYIR + 3, positionXIR] != "💥"))
            positionYIR += 3;
    }

    else if (direction == "Nord")
    {
        for (int i = 1; i <= 3; i++)
        {
            if ((positionYIR - i < 0) || (plateau[positionYIR - i, positionXIR] == "💥"))
                positionYIR -= (i - 1);
        }
        if ((positionYIR - 3 >= 0) && (plateau[positionYIR - 1, positionXIR] != "💥") && (plateau[positionYIR - 2, positionXIR] != "💥") && (plateau[positionYIR - 3, positionXIR] != "💥"))
            positionYIR -= 3;
    }
    plateau[positionYIR, positionXIR] = "🟥"; // Positionner I aux nouvelles coor
}

void Croquer(int positionYIR, int positionXIR, int positionYOwen, int positionXOwen, int positionYMaisie, int positionXMaisie)
{
    if ((positionYIR == positionYMaisie) && (positionXIR == positionXMaisie))
    {
        plateau[positionYMaisie, positionXMaisie] = "🟥";
        Console.WriteLine("Maisie a été mangée, fin de la partie");
    }
    else if ((positionYIR == positionYOwen) && (positionXIR == positionXOwen))
    {
        plateau[positionYOwen, positionXOwen] = "🟥";
        Console.WriteLine("Owen a été mangé, fin de la partie");
    }
    else
        Console.WriteLine("Bien joué ! Personne n'a été croqué.e ");
}

//Recuperer grenade

void RecupererGrenadeSpe(int positionYOwen, int positionXOwen)
{
    if (plateau[positionYOwen, positionXOwen] == "🧨")
        nbGrenadeSpe += 1;
    plateau[positionYOwen, positionXOwen] = "🟩";
}

//Création du plateau

string[,] CréerPlateau(int dim1, int dim2)
{

    string[,] plateau = new string[dim1, dim2];

    for (int i = 0; i < plateau.GetLength(0); i++)	//Initialisation du plateau vide
    {
        for (int j = 0; j < plateau.GetLength(1); j++)
        {
            plateau[i, j] = "⬜";
        }
    }
    //Placement aléatoire des joueurs
    PlacerAléatoire("🟩", plateau); //Owen         
    PlacerAléatoire("🟦", plateau); //Blue
    PlacerAléatoire("🟪", plateau); //Maisie
    PlacerAléatoire("🟥", plateau); //IR

    for (int i = 0; i < 2; i++)   //Grenades spéciales placées aléatoirement, changer le i<2 si on en veut plus !
    {
        PlacerAléatoire("🧨", plateau);  //"💥" symbole à utiliser pour les trous de grenade 
    }

    return plateau;
}

string[,] PlacerAléatoire(string perso, string[,] plateau)
{
    int x;
    int y;

    do
    {
        x = TirerNbAléatoire(plateau.GetLength(1));     // Tirer un x (abscisse) aléatoire entre 0 et le nombre de colonnes du plateau
        y = TirerNbAléatoire(plateau.GetLength(0));     // Tirer un y (ordonné) aléatoire entre 0 et le nombre de lignes du plateau
    }
    while (plateau[y, x] != "⬜");

    plateau[y, x] = perso;
    return plateau;
}

void AfficherPlateau(string[,] plateau)      //Afficher le plateau
{
    plateau[positionYOwen, positionXOwen] = "🟩";
    plateau[positionYMaisie, positionXMaisie] = "🟪";
    plateau[positionYBlue, positionXBlue] = "🟦";
    plateau[positionYIR, positionXIR] = "🟥";

    for (int i = 0; i < plateau.GetLength(0); i++)
    {
        for (int j = 0; j < plateau.GetLength(1); j++)
        {
            Console.Write(plateau[i, j]);
        }
        Console.WriteLine();
    }
}

int TirerNbAléatoire(int max)   //Tirer un nombre aléatoire
{
    Random rng = new Random();
    int nb = rng.Next(0, max);   //max : borne supérieure en paramètre
    return (nb);
}

void RécupérerCoord(string[,] plateau, ref int positionXOwen, ref int positionYOwen, ref int positionXIR, ref int positionYIR, ref int positionXMaisie, ref int positionYMaisie, ref int positionXBlue, ref int positionYBlue)
{
    for (int i = 0; i < plateau.GetLength(0); i++)
    {
        for (int j = 0; j < plateau.GetLength(1); j++)
        {
            if (plateau[i, j] == "🟩")
            {
                positionYOwen = i;
                positionXOwen = j;
            }
            if (plateau[i, j] == "🟪")
            {
                positionYMaisie = i;
                positionXMaisie = j;
            }
            if (plateau[i, j] == "🟦")
            {
                positionYBlue = i;
                positionXBlue = j;
            }
            if (plateau[i, j] == "🟥")
            {
                positionYIR = i;
                positionXIR = j;
            }
        }
    }
}


// Maisie et l'Indominus se déplacent de manière aléatoire d'une case à la fois

void DeplacementAleatoire(string personnage, ref int x, ref int y)
{
    plateau[y, x] = "⬜"; //Réinitialise le plateau
    Random rng = new Random();
    int nbrCaseX = rng.Next(-1, 2); // Génère un chiffre aléatoire entre -1 et 1 pour changer la valeur de la coordonnée x
    int nbrCaseY = rng.Next(-1, 2); // Génère un chiffre aléatoire entre -1 et 1 pour changer la valeur de la coordonnée y
    while (nbrCaseX == 0 && nbrCaseY == 0) // Eviter que le déplacement soit nul (les deux coordonnées restent les mêmes)
    {
        nbrCaseX = rng.Next(-1, 2);
        nbrCaseY = rng.Next(-1, 2);
    }
    x = x + nbrCaseX; // Ajoute la valeur aléatoire à la coordonnée initiale
    y = y + nbrCaseY;
    while (x < 0 || y < 0 || x > (plateau.GetLength(1) - 1) || y > (plateau.GetLength(0) - 1)) // Evite que les nouvelles coordonnées soient négatives et donc qu'elles sortent du plateau 
    {
        nbrCaseX = rng.Next(-1, 2);
        nbrCaseY = rng.Next(-1, 2);
        x = x + nbrCaseX;
        y = y + nbrCaseY;
    }
    plateau[y, x] = personnage; // Affiche la nouvelle position du personnage 

}



// Si l'Indominus est énervée elle peut se déplacer de 2 cases à la fois

void DeplacementAleatoireEnervee(string personnage, ref int x, ref int y)
{
    plateau[y, x] = "⬜"; //Réinitialise le plateau

    Random rng = new Random();
    int nbrCaseX = rng.Next(-2, 3); // Génère un chiffre aléatoire entre -2 et 2 pour changer la valeur de la coordonnée x
    int nbrCaseY = rng.Next(-2, 3); // Génère un chiffre aléatoire entre -2 et 2 pour changer la valeur de la coordonnée y

    while (nbrCaseX == 0 && nbrCaseY == 0) // Eviter que le déplacement soit nul (les deux coordonnées restent les mêmes)
    {
        nbrCaseX = rng.Next(-2, 3);
        nbrCaseY = rng.Next(-2, 3);
    }
    x = x + nbrCaseX; // Ajoute la valeur aléatoire à la coordonnée initiale
    y = y + nbrCaseY;

    while (x < 0 || y < 0 || x > (plateau.GetLength(1) - 1) || y > (plateau.GetLength(0) - 1)) // Evite que les nouvelles coordonnées soient négatives et donc qu'elles sortent du plateau 
    {
        nbrCaseX = rng.Next(-2, 3);
        nbrCaseY = rng.Next(-2, 3);
        x = x + nbrCaseX;
        y = y + nbrCaseY;
    }
    plateau[y, x] = personnage; // Affiche la nouvelle position de l'Indominus

}



// Déplace le personnage d'une case à l'aide des flèches du clavier

void DeplacementClavier(string personnage, ref int x, ref int y, string nom)
{
    Console.WriteLine($"Presser une flèche du clavier pour déplacer {nom}");

    plateau[y, x] = "⬜"; // Réinitialise le plateau

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

    plateau[y, x] = personnage; // Affiche le personnage sur sa nouvelle position

}

//Tests à supprimer

RécupérerCoord(plateau, ref positionXOwen, ref positionYOwen, ref positionXIR, ref positionYIR, ref positionXMaisie, ref positionYMaisie, ref positionXBlue, ref positionYBlue);

//AfficherPlateau(plateau);

/*if (enervement == false)
{
    DeplacementAleatoire("🟥", ref positionXIR, ref positionYIR);
}
else
{
    DeplacementAleatoireEnervee("🟥", ref positionXIR, ref positionYIR);
}
AfficherPlateau(plateau);

Croquer(positionYIR, positionXIR, positionYOwen, positionXOwen, positionYMaisie, positionXMaisie);

DeplacementAleatoire("🟪", ref positionXMaisie, ref positionYMaisie);
AfficherPlateau(plateau);

DeplacementClavier("🟦", ref positionXBlue, ref positionYBlue, nomBlue);
AfficherPlateau(plateau);

if ((positionYBlue == positionYIR) && (positionXBlue == positionXIR))
{
    PouvoirBlue(ref positionYIR, ref positionXIR);
    AfficherPlateau(plateau);
}

DeplacementClavier("🟩", ref positionXOwen, ref positionYOwen, nomOwen);
AfficherPlateau(plateau);

RecupererGrenadeSpe(positionYOwen, positionXOwen);

Grenade(positionYOwen, positionXOwen, nbGrenade, pdvIR, pdvBlue, pdvMaisie);*/




//Croquer(positionYIR, positionXIR, positionYOwen, positionXOwen, positionYMaisie, positionXMaisie);
//AfficherPlateau(plateau);
//PouvoirBlue(ref positionYIR, ref positionXIR);


void VerifierEnclos(int positionXIR, int positionYIR)
{
    int hauteur = plateau.GetLength(0);
    int largeur = plateau.GetLength(1);
    bool[,] casesEnclos = new bool[hauteur, largeur];
    bool[,] casesVisitees = new bool[plateau.GetLength(0), plateau.GetLength(1)];

    bool enclosFerme = RechercherEnclos(positionXIR, positionYIR, casesEnclos, casesVisitees);

    if (enclosFerme)
    {
        bool autrePersonnageEnferme = VerifierPersonnageEnferme(casesEnclos);

        if (autrePersonnageEnferme)
        {
            Console.WriteLine("Perdu ! Quelqu'un est enfermé avec l'IR");
        }
        else
        {
            Console.WriteLine("Bien joué ! Tu as enfermé l'IR");
        }
    }
    else
    {
        Console.WriteLine("Pas d'enclos");
    }
}



bool RechercherEnclos(int positionXIR, int positionYIR, bool[,] casesEnclos, bool[,] casesVisitees)
{
    int hauteur = plateau.GetLength(0);
    int largeur = plateau.GetLength(1);
    int departX = 0;
    int departY = 0;

    while ((departX == 0) && (departY == 0))
    {
        TrouverDepart(plateau, ref departX, ref departY);
    }

    int retourX = departX;
    int retourY = departY;
    int nouveauX = departX;
    int nouveauY = departY;
    int[,] directions = { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 }, { -1, -1 }, { -1, 1 }, { 1, -1 }, { 1, 1 } };
    bool toutesDirections = false;
    bool uneDirection = true;
    while ((nouveauX != retourX) || (nouveauY != retourY))
    {
        if (uneDirection)
        {
            RechercherProchain(plateau, directions, ref nouveauX, ref nouveauY, ref departX, ref departY, ref uneDirection, ref toutesDirections, casesVisitees);
        }
        else if (!toutesDirections)
        {
            return false; // Aucune direction valide, on sort
        }
    }
    MarquerCasesEnclos(casesVisitees, casesEnclos);

    return true;
}

void MarquerCasesEnclos(bool[,] casesVisitees, bool[,] casesEnclos) // Marquer = rendre true
{
    int hauteur = plateau.GetLength(0);
    int largeur = plateau.GetLength(1);

    // On reset casesEnclos à chaque nouvelle itération
    for (int y = 0; y < hauteur; y++)
    {
        for (int x = 0; x < largeur; x++)
        {
            casesEnclos[y, x] = false;
        }
    }

    // Marquer les crevasses
    for (int y = 0; y < hauteur; y++)
    {
        for (int x = 0; x < largeur; x++)
        {
            if (casesVisitees[y, x] && plateau[y, x] == "💥")
            {
                casesVisitees[y, x] = true;
            }
        }
    }

    // On marque tout ce qu'il y a à l'intérieur de l'enclos excepté les crevasses
    for (int y = 0; y < hauteur; y++)
    {
        for (int x = 0; x < largeur; x++)
        {
            if (casesVisitees[y, x] && plateau[y, x] != "💥")
            {
                casesEnclos[y, x] = true;
            }
        }
    }
}

bool TrouverDepart(string[,] plateau, ref int x, ref int y)
{
    for (int j = 0; j < plateau.GetLength(1); j++)
    {
        for (int i = 0; i < plateau.GetLength(0); i++)
        {
            if (plateau[i, j] == "💥")
            {
                x = j;
                y = i;
                return true;
            }
        }
    }
    x = 0;
    y = 0;
    return false;
}

void RechercherProchain(string[,] plateau, int[,] directions, ref int nouveauX, ref int nouveauY, ref int departX, ref int departY, ref bool uneDirection, ref bool toutesDirections, bool[,] casesVisitees)
{
    uneDirection = false;
    for (int i = 0; i < directions.GetLength(0); i++)
    {
        nouveauX = departX + directions[i, 1];
        nouveauY = departY + directions[i, 0];

        if (nouveauX >= plateau.GetLength(0) || nouveauX < 0 ||
            nouveauY >= plateau.GetLength(1) || nouveauY < 0)
        {
            continue; // Ignore les positions hors des limites
        }

        if (!casesVisitees[nouveauX, nouveauY])
        {
            casesVisitees[nouveauX, nouveauY] = true;
            departX = nouveauX;
            departY = nouveauY;
            uneDirection = true;
            toutesDirections = true;
        }
        if (plateau[nouveauX, nouveauY] == "💥")
        {
            departX = nouveauX;
            departY = nouveauY;
            uneDirection = true;
            toutesDirections = true;
        }
    }
}

bool VerifierPersonnageEnferme(bool[,] casesEnclos)
{
    // On regarde si chaque personnage est sur une case marquée
    return (casesEnclos[positionYBlue, positionXBlue] ||
            casesEnclos[positionYOwen, positionXOwen] ||
            casesEnclos[positionYMaisie, positionXMaisie]);
}


/*Test enclos
positionXIR = 4;
positionYIR = 4;
plateau[2, 3] = "💥";
plateau[2, 4] = "💥";
plateau[2, 5] = "💥";
plateau[3, 6] = "💥";
plateau[3, 7] = "💥";
plateau[3, 8] = "💥";
plateau[4, 8] = "💥";
plateau[5, 8] = "💥";
plateau[5, 7] = "💥";
plateau[6, 7] = "💥";
plateau[6, 6] = "💥";
plateau[6, 5] = "💥";
plateau[6, 4] = "💥";
plateau[6, 3] = "💥";
plateau[5, 3] = "💥";
plateau[4, 2] = "💥";
plateau[3, 2] = "💥";
AfficherPlateau(plateau);
VerifierEnclos(positionXIR, positionYIR);
*/
positionXIR = 4;
positionYIR = 4;
plateau[0, 8] = "💥";
plateau[1, 8] = "💥";
plateau[2, 8] = "💥";
plateau[3, 8] = "💥";
plateau[4, 8] = "💥";
plateau[5, 8] = "💥";
plateau[5, 7] = "💥";
plateau[6, 7] = "💥";
plateau[6, 6] = "💥";
plateau[6, 5] = "💥";
plateau[6, 4] = "💥";
plateau[6, 3] = "💥";
plateau[5, 3] = "💥";
plateau[4, 2] = "💥";
plateau[4, 1] = "💥";
plateau[4, 0] = "💥";
AfficherPlateau(plateau);
VerifierEnclos(positionXIR, positionYIR);

