//Valeurs pour tester A SUPPRIMER
int positionYOwen = 0;
int positionXOwen = 0;
int positionYIR = 0;
int positionXIR = 0;
int positionYMaisie = 0;
int positionXMaisie = 0;
int positionYBlue = 0;
int positionXBlue = 0;

//Affichage des règles

//Légende 
Console.WriteLine("=== Légende ===");
Console.WriteLine("🟩 : Owen");
Console.WriteLine("🟪 : Maisie");
Console.WriteLine("🟦 : Blue");
Console.WriteLine("🟥 : IR");
Console.WriteLine("🧨 : Grenade");
Console.WriteLine("💥 : Crevasse");
Console.WriteLine("================");

//Demander taille tableau 
int hauteurPlateau;
int longueurPlateau;
bool saisieValide;

Console.Write("Déterminez la hauteur du plateau : ");
do
{
    string saisie = Console.ReadLine()!;
    saisieValide = int.TryParse(saisie, out hauteurPlateau);

    if (!saisieValide)
    {
        Console.WriteLine("Saisie invalide. Veuillez entrer un entier.");
    }
} while (!saisieValide);

Console.Write("Déterminez la longueur du plateau : ");
do
{
    string saisie = Console.ReadLine()!;
    saisieValide = int.TryParse(saisie, out longueurPlateau);

    if (!saisieValide)
    {
        Console.WriteLine("Saisie invalide. Veuillez entrer un entier.");
    }
} while (!saisieValide);

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
    int coorYGrenade = 900;  // Sera toujours au-delà des limites du plateau
    int coorXGrenade = 900;    // Sera toujours au-delà des limites du plateau
    int randomY = 0;
    int randomX = 0;
    Console.WriteLine("Lancer une grenade? (répondre Oui ou Non)");
    string reponse = Console.ReadLine()!;
    if (reponse == "Oui"||reponse == "oui")
    {
        Console.WriteLine("Tapez 'S' pour lancer une grenade spéciale ou 'N' pour une grenade normale ?");
        string type = Console.ReadLine()!;

        if (type == "S" || type == "s")
        {
            if (nbGrenadeSpe > 0)
            {
                nbGrenadeSpe -= 1;
                SelectionCoordoneesGrenade(ref coorYGrenade, ref coorXGrenade);
                if ((coorXGrenade >= plateau.GetLength(1)) || (coorXGrenade < 0) || (coorYGrenade >= plateau.GetLength(0)) || (coorYGrenade < 0))
                {
                    Console.WriteLine("Impossible, c'est en dehors des limites du plateau");
                    SelectionCoordoneesGrenade(ref coorYGrenade, ref coorXGrenade);
                }
                while ((coorYGrenade >= positionYOwen + 3) || (coorYGrenade <= positionYOwen - 3) || (coorXGrenade >= positionXOwen + 3) || (coorXGrenade <= positionXOwen - 3))
                {
                    Console.WriteLine("Impossible, Owen a une portée de 3 cases maximum");
                    SelectionCoordoneesGrenade(ref coorYGrenade, ref coorXGrenade);
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
                Console.WriteLine("Voulez vous lancer une grenade normale? (si oui, répondre 'N')");
            }
        }

        else if ((type == "N") || (type == "n"))
        {
            if (nbGrenade > 0)
            {
                nbGrenade -= 1;
                SelectionCoordoneesGrenade(ref coorYGrenade, ref coorXGrenade);
                if ((coorXGrenade >= plateau.GetLength(1)) || (coorXGrenade < 0) || (coorYGrenade >= plateau.GetLength(0)) || (coorYGrenade < 0))
                {
                    Console.WriteLine("Impossible, c'est en dehors des limites du plateau");
                    SelectionCoordoneesGrenade(ref coorYGrenade, ref coorXGrenade);
                }
                while ((coorYGrenade >= positionYOwen + 3) || (coorYGrenade <= positionYOwen - 3) || (coorXGrenade >= positionXOwen + 3) || (coorXGrenade <= positionXOwen - 3))
                {
                    Console.WriteLine("Impossible, Owen a une portée de 3 cases maximum");
                    SelectionCoordoneesGrenade(ref coorYGrenade, ref coorXGrenade);
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
    else if (reponse == "Non"||reponse == "non")
    {
        Console.WriteLine("Owen ne lance pas de grenade.");
    }
    AfficherPlateau(plateau);
    Console.WriteLine($"Il vous reste {nbGrenade} grenade(s) et {nbGrenadeSpe} grenade(s) spéciale(s).");
}

//Entrer les coordonnées de la Grenade
void SelectionCoordoneesGrenade(ref int coorYGrenade, ref int coorXGrenade)
{
    Console.WriteLine("Sélectionnez où lancer la grenade:");
    Console.WriteLine("Entrez le numéro de ligne :");
    coorYGrenade = Convert.ToInt32(Console.ReadLine()!);
    Console.WriteLine("Entrez le numéro de colonne :");
    coorXGrenade = Convert.ToInt32(Console.ReadLine()!);
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

    
    Console.Write("  ");
    for (int t = 0; t < plateau.GetLength(1); t++)
    {
       Console.Write(t+" ");

       if (t == plateau.GetLength(1)-1)
       {
        Console.WriteLine();
       }
    }

    for (int i = 0; i < plateau.GetLength(0); i++)
    {
       Console.Write(i);

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
    plateau[y, x] = "⬜"; //Réinitialise la case du personnage
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
    while (x < 0 || y < 0 || x > (plateau.GetLength(1) - 1) || y > (plateau.GetLength(0) - 1) || (plateau[y,x] != "⬜")) // Evite que les nouvelles coordonnées soient négatives et donc qu'elles sortent du plateau , ou qu'elle soit sur la case d'un autre joueur
    {
        nbrCaseX = rng.Next(-1, 2);
        nbrCaseY = rng.Next(-1, 2);
        x = x + nbrCaseX;
        y = y + nbrCaseY;
    }
    plateau[y, x] = personnage; // Prend la nouvelle position du personnage 

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
    int newX = x;
    int newY = y;
    bool deplacementValide = false; // Indicateur pour savoir si le déplacement est valide

    Console.WriteLine($"Presser une flèche du clavier pour déplacer {nom}");

    do
    {
        ConsoleKeyInfo key = Console.ReadKey(intercept: true);

        // Calcul des nouvelles coordonnées en fonction de la touche pressée
        if (key.Key == ConsoleKey.LeftArrow && x > 0) // Flèche gauche
        {
            newX = x - 1;
        }
        else if (key.Key == ConsoleKey.RightArrow && x < Console.WindowWidth - 1) // Flèche droite
        {
            newX = x + 1;
        }
        else if (key.Key == ConsoleKey.UpArrow && y > 0) // Flèche haut
        {
            newY = y - 1;
        }
        else if (key.Key == ConsoleKey.DownArrow && y < Console.WindowHeight - 1) // Flèche bas
        {
            newY = y + 1;
        }

        if (newX < 0 || newY < 0 || newX > (plateau.GetLength(1) - 1) || newY > (plateau.GetLength(0) - 1) || (plateau[newY,newX] != "⬜")) //Si les nouvelles coordonnées sont en dehors du plateau ou si la case cible n'est pas vide
        {
            Console.WriteLine("Déplacement impossible : la case est occupée ou hors du plateau. Pressez une autre flèche.");
            deplacementValide = false; 
            newX = x; // On reprend les coordonnées initiales
            newY = y;
        }
        else if ((plateau[newY, newX] == "🧨") && (personnage == "🟩")) // Si grenade spéciale et Owen (les autres perso ne peuvent pas récup de grenades spéciales)
        {
            nbGrenadeSpe += 1;
            plateau[y, x] = "⬜"; // Réinitialise l'ancienne case
            y = newY; // Met à jour les coordonnées après déplacement
            x = newX;
            plateau[y, x] = personnage; // Met à jour la position d'Owen
            Console.WriteLine($"Owen a récupéré une grenade spéciale ! Vous avez désormais {nbGrenadeSpe} grenade(s) spéciale(s)");
            deplacementValide = true; 
        }
        else    //Si la case cible est vide
        {
            plateau[y, x] = "⬜"; // Réinitialise l'ancienne case
            y = newY;
            x = newX;
            plateau[y, x] = personnage; // Met à jour la position du personnage
            deplacementValide = true; 
        }

    } while (!deplacementValide); // Répéter tant que le déplacement n'est pas valide

} 


//Tests à supprimer

RécupérerCoord(plateau, ref positionXOwen, ref positionYOwen, ref positionXIR, ref positionYIR, ref positionXMaisie, ref positionYMaisie, ref positionXBlue, ref positionYBlue);

//AfficherPlateau(plateau);

if (enervement == false)
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

Grenade(positionYOwen, positionXOwen, nbGrenade, pdvIR, pdvBlue, pdvMaisie);




//Croquer(positionYIR, positionXIR, positionYOwen, positionXOwen, positionYMaisie, positionXMaisie);
//AfficherPlateau(plateau);
//PouvoirBlue(ref positionYIR, ref positionXIR);

