//Valeurs pour tester A SUPPRIMER

int positionYOwen = 0;
int positionXOwen = 0;
int positionYIR = 0;
int positionXIR = 0;
int positionYMaisie = 0;
int positionXMaisie = 0;
int positionYBlue = 0;
int positionXBlue = 0;


//Légende 

Console.WriteLine("=== Légende ===");
Console.WriteLine("🟩 : Owen");
Console.WriteLine("🟪 : Maisie");
Console.WriteLine("🟦 : Blue");
Console.WriteLine("🟥 : IR");
Console.WriteLine("🧨 : Grenade");
Console.WriteLine("💥 : Crevasse");
Console.WriteLine("================");


//Création plateau

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
bool finPv = false;


// Lancer ou non d'une grenade, spéciale ou non

void Grenade(int positionYOwen, int positionXOwen, int nbGrenade, int pdvIR, int pdvBlue, int pdvMaisie, ref bool finGrenade, ref bool enervement)
{

    int coorYGrenade = 900; //Pour éviter les problèmes d'assignement
    int coorXGrenade = 900;
    int randomY = 0;
    int randomX = 0;
    Console.WriteLine("Lancer une grenade? (répondre Oui ou Non)");
    string reponse = Console.ReadLine()!;
    if (reponse == "Oui" || reponse == "oui")
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
                        finGrenade = true;
                    }
                    else if (plateau[coorYGrenade, coorXGrenade] == "🟪")
                    {
                        Console.WriteLine("Maisie a été tuée par Owen. Fin de la partie.");
                        finGrenade = true;
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
                    {
                        Console.WriteLine("Blue a été tuée par Owen. Fin de la partie.");
                        finGrenade = true;
                    }
                    else if (plateau[coorYGrenade, coorXGrenade] == "🟪")
                    {
                        Console.WriteLine("Maisie a été tuée par Owen. Fin de la partie.");
                        finGrenade = true;
                    }
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
    else if (reponse == "Non" || reponse == "non")
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
    {
        Console.WriteLine($"{nom} a été tuée par {owen}. Fin de la partie.");
        finPv = true;

    }

    if (pdvMaisie > 0)
        Console.WriteLine($"{nom} a été touchée par l'impact, attention");
}


//Pouvoir de Blue

void PouvoirBlue(ref int positionYIR, ref int positionXIR) // Lancé que si Blue et IR en même position
{
    plateau[positionYIR, positionXIR] = "⬜"; // Supprimer le caractère I du plateau aux anciennes positions
    Console.WriteLine("Sélectionnez la direction dans laquelle envoyer l'IR: Nord, Sud, Est ou Ouest ?");
    string direction = Console.ReadLine()!;
    if (direction == "Ouest" || direction == "ouest")
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
    else if (direction == "Est" || direction == "est")
    {
        for (int i = 1; i <= 3; i++)
        {
            if ((positionXIR + i >= plateau.GetLength(1) - 1) || (plateau[positionYIR, positionXIR + i] == "💥"))
                positionXIR += (i - 1);

        }
        if ((positionXIR + 3 < plateau.GetLength(1)) && (plateau[positionYIR, positionXIR + 1] != "💥") && (plateau[positionYIR, positionXIR + 2] != "💥") && (plateau[positionYIR, positionXIR + 3] != "💥"))
            positionXIR += 3;
    }

    else if (direction == "Sud" || direction == "sud")
    {
        for (int i = 1; i <= 3; i++)
        {
            if ((positionYIR + i >= plateau.GetLength(0) - 1) || (plateau[positionYIR + i, positionXIR] == "💥"))
                positionYIR += (i - 1);
        }
        if ((positionYIR + 3 < plateau.GetLength(1)) && (plateau[positionYIR + 1, positionXIR] != "💥") && (plateau[positionYIR + 2, positionXIR] != "💥") && (plateau[positionYIR + 3, positionXIR] != "💥"))
            positionYIR += 3;
    }

    else if (direction == "Nord" || direction == "nord")
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


//Croquer un personnage

void Croquer(int positionYIR, int positionXIR, int positionYOwen, int positionXOwen, int positionYMaisie, int positionXMaisie, ref bool finCroc)
{

    if ((positionYIR == positionYMaisie) && (positionXIR == positionXMaisie))
    {
        plateau[positionYMaisie, positionXMaisie] = "🟥";
        Console.WriteLine("Maisie a été mangée, fin de la partie");
        finCroc = true;
    }
    else if ((positionYIR == positionYOwen) && (positionXIR == positionXOwen))
    {
        plateau[positionYOwen, positionXOwen] = "🟥";
        Console.WriteLine("Owen a été mangé, fin de la partie");
        finCroc = true;
    }
    else
        Console.WriteLine("Bien joué ! Personne n'a été croqué.e ");
}

//Recuperer grenade

void RecupererGrenadeSpe(int positionYOwen, int positionXOwen)
{
    if (plateau[positionYOwen, positionXOwen] == "🧨")
    {
        nbGrenadeSpe += 1;
        Console.WriteLine("Vous avez récupéré une grenade spéciale");
    }
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


//Placement aléatoire des personnages sur le plateau

string[,] PlacerAléatoire(string perso, string[,] plateau)
{
    int x;
    int y;

    do
    {
        x = TirerNbAléatoire(plateau.GetLength(1));     //Tirer un x (abscisse) aléatoire entre 0 et le nombre de colonnes du plateau
        y = TirerNbAléatoire(plateau.GetLength(0));     //Tirer un y (ordonné) aléatoire entre 0 et le nombre de lignes du plateau
    }
    while (plateau[y, x] != "⬜");                      //La nouvelle case doit etre libre

    plateau[y, x] = perso;
    return plateau;
}


//Afficher le plateau

void AfficherPlateau(string[,] plateau)
{
    plateau[positionYOwen, positionXOwen] = "🟩";
    plateau[positionYMaisie, positionXMaisie] = "🟪";
    plateau[positionYBlue, positionXBlue] = "🟦";
    plateau[positionYIR, positionXIR] = "🟥";

    Console.Write("  ");
    for (int t = 0; t < plateau.GetLength(0) + 2; t++)
    {
        Console.Write($"{t} ");
    }

    Console.WriteLine();
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


//Tirer un nombre aléatoire

int TirerNbAléatoire(int max)
{
    Random rng = new Random();
    int nb = rng.Next(0, max);   //max : borne supérieure en paramètre
    return (nb);
}


//Récupérer les coordonnées des personnages

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


// Déplacements aléatoires de Maisie et de l'Indominus 

void DeplacementAleatoire(string personnage, ref int x, ref int y)
{
    int newX;
    int newY;
    int nbrCaseX;
    int nbrCaseY;
    bool deplacementValide = false;
    Random rng = new Random();

    do
    {
        if (personnage == "🟥" && enervement == true)   // Si l'Indominus est énervée elle peut se déplacer de 2 cases à la fois
        {
            nbrCaseX = rng.Next(-2, 3); // Génère un chiffre aléatoire entre -2 et 2 pour changer la valeur de la coordonnée x
            nbrCaseY = rng.Next(-2, 3); // Génère un chiffre aléatoire entre -2 et 2 pour changer la valeur de la coordonnée y
        }
        else
        {
            nbrCaseX = rng.Next(-1, 2); // Génère un chiffre aléatoire entre -1 et 1 pour changer la valeur de la coordonnée x
            nbrCaseY = rng.Next(-1, 2); // Génère un chiffre aléatoire entre -1 et 1 pour changer la valeur de la coordonnée y
        }

        if (nbrCaseX != 0 && nbrCaseY != 0)    // Si le déplacement n'est pas nul (les deux coordonnées restent les mêmes)
        {
            newX = x + nbrCaseX;
            newY = y + nbrCaseY;
            if (newX > 0 && newY > 0 && newX < plateau.GetLength(1) && newY < plateau.GetLength(0))
            {
                if ((personnage == "🟪") && ((plateau[newY, newX] == "⬜") || (plateau[newY, newX] == "🟥"))) //Maisie peut tomber par accident sur IR mais pas sur un autre joueur
                {
                    deplacementValide = true;
                    plateau[y, x] = "⬜";   // Réinitialise le plateau
                    y = newY;   // Met à jour les coordonnées après déplacement
                    x = newX;
                    plateau[y, x] = personnage; // Prend la nouvelle position du personnage
                    Console.WriteLine("Maisie s'est déplacée.");
                }
                if ((personnage == "🟥") && (plateau[newY, newX] != "💥") && (plateau[newY, newX] != "🧨") && (plateau[newY, newX] != "🟦"))  //IR peut tomber sur un autre joueur et le tuer (sauf Blue car elle est trop rapide)
                {
                    deplacementValide = true;
                    plateau[y, x] = "⬜";
                    y = newY;
                    x = newX;
                    plateau[y, x] = personnage;
                    Console.WriteLine("IR s'est déplacée.");
                }
            }
        }

    } while (!deplacementValide);

}



// Déplacements clavier de Owen et Blue

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

        // Vérification : deplacement valide ou non

        if (newX < 0 || newY < 0 || newX > (plateau.GetLength(1) - 1) || newY > (plateau.GetLength(0) - 1)) //Si les nouvelles coordonnées sont en dehors du plateau 
        {
            Console.WriteLine("Déplacement impossible : hors du plateau. Pressez une autre flèche.");
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
        else if ((plateau[newY, newX] != "⬜") && (plateau[newY, newX] != "🟥"))
        {
            Console.WriteLine("Déplacement impossible : la case est occupée. Pressez une autre flèche.");
            deplacementValide = false;
            newX = x; // On reprend les coordonnées initiales
            newY = y;
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
AfficherPlateau(plateau);


//Partie

void Jeu()
{

    bool finCroc = false;
    bool finGrenade = false;

    while (finCroc == false && finGrenade == false && finPv == false) // La partie continue tant que les conditions d'échec ne sont pas vérifiées 
    {

        DeplacementAleatoire("🟥", ref positionXIR, ref positionYIR);

        AfficherPlateau(plateau);
        Croquer(positionYIR, positionXIR, positionYOwen, positionXOwen, positionYMaisie, positionXMaisie, ref finCroc);
        if (finCroc)
        {
            break; // La partie s'arrête si l'indominus mange un personnage
        }

        DeplacementAleatoire("🟪", ref positionXMaisie, ref positionYMaisie);
        AfficherPlateau(plateau);
        Croquer(positionYIR, positionXIR, positionYOwen, positionXOwen, positionYMaisie, positionXMaisie, ref finCroc);
        if (finCroc)
        {
            break; // La partie s'arrête si Maisie est mangée
        }

        DeplacementClavier("🟦", ref positionXBlue, ref positionYBlue, nomBlue);
        AfficherPlateau(plateau);

        if ((positionYBlue == positionYIR) && (positionXBlue == positionXIR))
        {
            PouvoirBlue(ref positionYIR, ref positionXIR);
            AfficherPlateau(plateau);
        }

        DeplacementClavier("🟩", ref positionXOwen, ref positionYOwen, nomOwen);
        AfficherPlateau(plateau);
        Croquer(positionYIR, positionXIR, positionYOwen, positionXOwen, positionYMaisie, positionXMaisie, ref finCroc);
        if (finCroc)
        {
            break; // La partie s'arrête si Owen est mangé
        }

        RecupererGrenadeSpe(positionYOwen, positionXOwen);

        Grenade(positionYOwen, positionXOwen, nbGrenade, pdvIR, pdvBlue, pdvMaisie, ref finGrenade, ref enervement);
        if (finGrenade || finPv)
        {
            break; // La partie s'arrête si un personnage est tué par une grenade
        }
    }
    Console.WriteLine("La partie est finie !");
}


//Commencer une partie

Console.WriteLine("Cliquer sur la touche Entrée pour commencer une partie");
ConsoleKeyInfo key;
do
{
    key = Console.ReadKey(intercept: true);  // Attente d'une touche sans l'afficher

    if (key.Key == ConsoleKey.Enter)
    {
        Jeu();
        Console.WriteLine("Cliquer sur la touche Entrée pour commencer une partie"); // Rejouer quand la partie est terminée 
        plateau = CréerPlateau(hauteurPlateau, longueurPlateau);    // Réinitialise le plateau en début de partie
        RécupérerCoord(plateau, ref positionXOwen, ref positionYOwen, ref positionXIR, ref positionYIR, ref positionXMaisie, ref positionYMaisie, ref positionXBlue, ref positionYBlue);
        AfficherPlateau(plateau);
    }
}
while (key.Key == ConsoleKey.Enter);


void VerifierEnclos(int positionXIR, int positionYIR)
{
    int hauteur = plateau.GetLength(0);
    int largeur = plateau.GetLength(1);
    bool[,] casesEnclos = new bool[hauteur, largeur];  // Va enregistrer les cases qui font partie de l'enclos
    bool[,] casesVisitees = new bool[plateau.GetLength(0), plateau.GetLength(1)];  // Va enregistrer les cases qui ont été visitées par l'itération

    bool enclosFerme = RechercherEnclos(positionXIR, positionYIR, casesEnclos, casesVisitees);

    if (enclosFerme)
    {
        bool autrePersonnageEnferme = VerifierPersonnageEnferme(casesEnclos, casesVisitees);

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

    // On démarre de la position de l'IR
    int departX = positionXIR;
    int departY = positionYIR;
    RechercherProchain(plateau, departX, departY, casesVisitees);

    // Marquer les cases à l'intérieur de l'enclos
    for (int x = 0; x < plateau.GetLength(0); x++)
    {
        for (int y = 0; y < plateau.GetLength(1); y++)
        {
            if (casesVisitees[y, x] && plateau[y, x] != "💥")  //On ne compte pas les bordures
            {
                casesEnclos[y, x] = true;
            }
        }
    }
    return true;
}
// Sous-programme qui recherche par itération les cases blanches à 'intérieur de l'enclos
bool RechercherProchain(string[,] plateau, int departX, int departY, bool[,] casesVisitees)
{
    int hauteur = plateau.GetLength(0);
    int largeur = plateau.GetLength(1);
    int[,] directions = { { -1, 0 }, { 0, 1 }, { 1, 0 }, { 0, -1 } }; //Directions dans lesquelles rechercher

    // Tableau simulant une file d'attente pour stocker les coordonnées
    int[,] file = new int[hauteur * largeur, 2]; // Dim 1 = capacité max du tableau, Dim 2=2 pour les coordonnées X et Y
    int sommetFile = -1;

    // Ajouter le point de départ + le marquer comme visité
    sommetFile++;
    file[sommetFile, 0] = departX;
    file[sommetFile, 1] = departY;
    casesVisitees[departY, departX] = true;

    while (sommetFile >= 0) // Tant qu'il reste des éléments dans la file d'attente
    {
        int x = file[sommetFile, 0];
        int y = file[sommetFile, 1]; // On récupère ses coordonnées
        sommetFile--;  // On l'enlève de la file

        // On explore les 4 directions
        for (int i = 0; i < 4; i++)
        {
            int nouveauX = x + directions[i, 1];
            int nouveauY = y + directions[i, 0]; // Et calculer les nouvelles coordonnées

            // On ignore ce qui est hors des limites du plateau
            if (nouveauX < 0 || nouveauX >= largeur ||
                nouveauY < 0 || nouveauY >= hauteur)
                continue;

            // On ignore les cases déjà visitées et les crevasses
            if (casesVisitees[nouveauY, nouveauX] ||
                plateau[nouveauY, nouveauX] == "💥")
                continue;

            // Si aucun obstacle ou pas déjà visitée, on la marque comme visitée
            casesVisitees[nouveauY, nouveauX] = true;
            sommetFile++;
            file[sommetFile, 0] = nouveauX;
            file[sommetFile, 1] = nouveauY; // Et on l'ajoute à la file pour explorer les cases qui entourent celle-ci
        }
    }

    return true;
}

bool VerifierPersonnageEnferme(bool[,] casesEnclos, bool[,] casesVisitees)
{
    for (int y = 0; y < plateau.GetLength(0); y++)
    {
        for (int x = 0; x < plateau.GetLength(1); x++)
        {
            // On regarde si la case a été vérifiée et qu'elle est bien dans l'enclos
            if (casesEnclos[y, x] && casesVisitees[y, x])
            {
                // On vérifie si un autre personnage est dans cette case
                if ((y == positionYBlue && x == positionXBlue) ||
                    (y == positionYOwen && x == positionXOwen) ||
                    (y == positionYMaisie && x == positionXMaisie))
                {
                    return true; // Si un personnage y est
                }
            }
        }
    }
    return false; // Si aucun autre personnage n'est enfermé
}

//Test enclos
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


