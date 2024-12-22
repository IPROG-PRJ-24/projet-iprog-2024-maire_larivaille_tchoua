
//Légende 

Console.WriteLine("=== Légende ===");
Console.WriteLine("🟩 : Owen");
Console.WriteLine("🟪 : Maisie");
Console.WriteLine("🟦 : Blue");
Console.WriteLine("🟥 : IR");
Console.WriteLine("🧨 : Grenade");
Console.WriteLine("💥 : Crevasse");
Console.WriteLine("================");

int DemanderHauteurPlateau()
{
    int hauteurPlateau;
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
    return hauteurPlateau;
}

int DemanderLongueurPlateau()
{
    int longueurPlateau;
    bool saisieValide;

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
    return longueurPlateau;
}

int positionXOwen = 0;
int positionYOwen = 0;
int positionXBlue = 0;
int positionYBlue = 0;
int positionXIR = 0;
int positionYIR = 0;
int positionXMaisie = 0;
int positionYMaisie = 0;
string[,] plateau = CréerPlateau(DemanderHauteurPlateau(), DemanderLongueurPlateau());
RecupererCoord(plateau, ref positionXOwen, ref positionYOwen, ref positionXIR, ref positionYIR, ref positionXMaisie, ref positionYMaisie, ref positionXBlue, ref positionYBlue);

// Initialisation de départ
int nbGrenade = plateau.GetLength(1);
int nbGrenadeSpe = 0;
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
bool finCroc = false;
bool finGrenade = false;
bool finEnclos = false;

// Lancer ou non d'une grenade, spéciale ou non

void Grenade(int positionYOwen, int positionXOwen, ref int nbGrenade, ref int pdvIR, ref int pdvBlue, ref int pdvMaisie, ref bool finGrenade, ref bool enervement, ref int nbGrenadeSpe)
{

    int coorYGrenade = 900; //Pour éviter les problèmes d'assignement
    int coorXGrenade = 900;
    int randomY = 0;
    int randomX = 0;
    Console.WriteLine($"Il vous reste {nbGrenade} grenade(s) et {nbGrenadeSpe} grenade(s) spéciale(s).");
    Console.WriteLine("Lancer une grenade? (répondre Oui ou Non)");
    string reponse;
    do
    {
        Console.WriteLine("Entrez Oui ou Non");
        reponse = Console.ReadLine()!;
    }
    while (!Enum.TryParse(reponse, out ChoixGrenade resultat) || int.TryParse(reponse, out _));

    if (reponse == "Oui" || reponse == "oui")
    {
        string type;
        Console.WriteLine("Tapez 'S' pour lancer une grenade spéciale ou 'N' pour une grenade normale ?");
        do
        {
            Console.WriteLine("Entrez S ou N");
            type = Console.ReadLine()!;
        }
        while (!Enum.TryParse(type, out ChoixType resultat) || int.TryParse(type, out _));

        if (type == "S" || type == "s")
        {
            if (nbGrenadeSpe > 0)
            {
                nbGrenadeSpe -= 1;
                SelectionCoordoneesGrenade(ref coorYGrenade, ref coorXGrenade);
                while ((coorXGrenade >= plateau.GetLength(1)) || (coorXGrenade < 0) || (coorYGrenade >= plateau.GetLength(0)) || (coorYGrenade < 0) || (coorXGrenade == positionXOwen) && (coorYGrenade == positionYOwen) || (coorYGrenade >= positionYOwen + 3) || (coorYGrenade <= positionYOwen - 3) || (coorXGrenade >= positionXOwen + 3) || (coorXGrenade <= positionXOwen - 3))
                {
                    if ((coorXGrenade == positionXOwen) && (coorYGrenade == positionYOwen))
                        Console.WriteLine("Impossible, Owen ne peut pas se lancer de grenade dessus");
                    if ((coorYGrenade >= positionYOwen + 3) || (coorYGrenade <= positionYOwen - 3) || (coorXGrenade >= positionXOwen + 3) || (coorXGrenade <= positionXOwen - 3))
                        Console.WriteLine("Impossible, Owen a une portée de 3 cases maximum");
                    if (!EstDansLimites(coorXGrenade, coorYGrenade, plateau))
                        Console.WriteLine("Impossible, c'est en dehors des limites du plateau");
                    SelectionCoordoneesGrenade(ref coorYGrenade, ref coorXGrenade);
                }
                if ((coorYGrenade <= positionYOwen + 3) || (coorYGrenade >= positionYOwen - 3) || (coorXGrenade <= positionXOwen + 3) || (coorXGrenade >= positionXOwen - 3))
                {
                    if (plateau[coorYGrenade, coorXGrenade] == "🟥") // Savoir si l'IR a été touchée et lui enlever des pv et pas de crevasse
                    {
                        Console.WriteLine("IR a été touchée, -10 points de vie");
                        pdvIR -= 10;
                        if (pdvIR == 0)
                        {
                            finPv = true;
                            Console.WriteLine("L'IR n'a plus de points de vie");
                        }
                        Console.WriteLine($"Points de vie de l'IR : {pdvIR}");
                        enervement = true; //sera plus rapide au prochain déplacement
                        Console.WriteLine("IR est énervée, faites attention au prochain tour");
                    }
                    else if (plateau[coorYGrenade, coorXGrenade] == "🟦")
                    {
                        SystemePV(ref pdvBlue, nomBlue, nomOwen);
                    }
                    else if (plateau[coorYGrenade, coorXGrenade] == "🟪")
                    {
                        SystemePV(ref pdvMaisie, nomMaisie, nomOwen);
                    }
                    else // sinon on crée une crevasse
                    {
                        plateau[coorYGrenade, coorXGrenade] = "💥";
                        do
                        {
                            randomY = rng.Next(-1, 2);
                            randomX = rng.Next(-1, 2);
                        }
                        while (((randomY == 0) && (randomX == 0)) || !EstDansLimites(coorXGrenade + randomX, coorYGrenade + randomY, plateau) || !CaseLibre(coorXGrenade + randomX, coorYGrenade + randomY, plateau)); // Pour éviter que la case random soit la même que celle où la grenade atterit et prendre en compte les bordures

                        coorYGrenadeSpe = coorYGrenade + randomY;
                        coorXGrenadeSpe = coorXGrenade + randomX;

                        if (plateau[coorYGrenadeSpe, coorXGrenadeSpe] == "🟦")
                        {
                            SystemePV(ref pdvBlue, nomBlue, nomOwen);
                        }
                        else if (plateau[coorYGrenadeSpe, coorXGrenadeSpe] == "🟪")
                        {
                            SystemePV(ref pdvMaisie, nomMaisie, nomOwen);
                        }
                        else
                            plateau[coorYGrenadeSpe, coorXGrenadeSpe] = "💥";
                        do
                        {
                            randomY = rng.Next(-1, 2);
                            randomX = rng.Next(-1, 2);
                        }
                        while (((randomY == 0) && (randomX == 0)) || !EstDansLimites(coorXGrenadeSpe + randomX, coorYGrenadeSpe + randomY, plateau) || !CaseLibre(coorXGrenadeSpe + randomX, coorYGrenadeSpe + randomY, plateau)); // Pour éviter que la case random soit la même que celle où la grenade atterit
                        if (plateau[coorYGrenadeSpe + randomY, coorXGrenadeSpe + randomX] == "🟦")
                        {
                            SystemePV(ref pdvBlue, nomBlue, nomOwen);
                        }
                        else if (plateau[coorYGrenadeSpe + randomY, coorXGrenadeSpe + randomX] == "🟪")
                        {
                            SystemePV(ref pdvMaisie, nomMaisie, nomOwen);
                        }
                        else
                            plateau[coorYGrenadeSpe + randomY, coorXGrenadeSpe + randomX] = "💥";
                    }
                }
                AfficherPlateau(plateau);
            }
            else
            {
                Console.WriteLine("Vous n'avez plus de grenade spéciale, dommage !");
            }
        }

        else if ((type == "N") || (type == "n"))
        {
            if (nbGrenade > 0)
            {
                nbGrenade -= 1;
                SelectionCoordoneesGrenade(ref coorYGrenade, ref coorXGrenade);
                while ((coorXGrenade >= plateau.GetLength(1)) || (coorXGrenade < 0) || (coorYGrenade >= plateau.GetLength(0)) || (coorYGrenade < 0) || (coorXGrenade == positionXOwen) && (coorYGrenade == positionYOwen) || (coorYGrenade >= positionYOwen + 3) || (coorYGrenade <= positionYOwen - 3) || (coorXGrenade >= positionXOwen + 3) || (coorXGrenade <= positionXOwen - 3))
                {
                    if ((coorXGrenade == positionXOwen) && (coorYGrenade == positionYOwen))
                        Console.WriteLine("Impossible, Owen ne peut pas se lancer de grenade dessus");
                    if ((coorYGrenade >= positionYOwen + 3) || (coorYGrenade <= positionYOwen - 3) || (coorXGrenade >= positionXOwen + 3) || (coorXGrenade <= positionXOwen - 3))
                        Console.WriteLine("Impossible, Owen a une portée de 3 cases maximum");
                    if (!EstDansLimites(coorXGrenade, coorYGrenade, plateau))
                        Console.WriteLine("Impossible, c'est en dehors des limites du plateau");
                    SelectionCoordoneesGrenade(ref coorYGrenade, ref coorXGrenade);
                }

                if ((coorYGrenade <= positionYOwen + 3) || (coorYGrenade >= positionYOwen - 3) || (coorXGrenade <= positionXOwen + 3) || (coorXGrenade >= positionXOwen - 3))
                {
                    if (plateau[coorYGrenade, coorXGrenade] == "🟥") // Savoir si l'IR a été touchée et lui enlever des pv et pas de crevasse
                    {
                        Console.WriteLine("IR a été touchée, -10 points de vie");
                        pdvIR -= 10;
                        if (pdvIR == 0)
                        {
                            finPv = true;
                            Console.WriteLine("L'IR n'a plus de points de vie");
                        }
                        Console.WriteLine($"Points de vie de l'IR : {pdvIR}");
                        enervement = true; //sera plus rapide au prochain déplacement
                        Console.WriteLine("IR est énervée, faites attention au prochain tour");
                    }
                    else if (plateau[coorYGrenade, coorXGrenade] == "🟦")
                    {
                        SystemePV(ref pdvBlue, nomBlue, nomOwen);
                    }
                    else if (plateau[coorYGrenade, coorXGrenade] == "🟪")
                    {
                        SystemePV(ref pdvMaisie, nomMaisie, nomOwen);
                    }
                    else // sinon on crée une crevasse
                    {
                        plateau[coorYGrenade, coorXGrenade] = "💥";
                        do
                        {
                            randomY = rng.Next(-1, 2);
                            randomX = rng.Next(-1, 2);
                        }
                        while (((randomY == 0) && (randomX == 0)) || !EstDansLimites(coorXGrenade + randomX, coorYGrenade + randomY, plateau) || !CaseLibre(coorXGrenade + randomX, coorYGrenade + randomY, plateau)); // Pour éviter que la case random soit la même que celle où la grenade atterit
                        if (plateau[coorYGrenade + randomY, coorXGrenade + randomX] == "🟦")
                        {
                            SystemePV(ref pdvBlue, nomBlue, nomOwen);
                        }
                        else if (plateau[coorYGrenade + randomY, coorXGrenade + randomX] == "🟪")
                        {
                            SystemePV(ref pdvMaisie, nomMaisie, nomOwen);
                        }
                        else
                            plateau[coorYGrenade + randomY, coorXGrenade + randomX] = "💥";
                    }
                }
                AfficherPlateau(plateau);
            }
            if (nbGrenade == 0)
                Console.WriteLine("Vous n'avez plus de grenades, bonne chance!");
        }
    }
    else if (reponse == "Non" || reponse == "non")
    {
        Console.WriteLine("Owen ne lance pas de grenade.");
    }
    Console.WriteLine($"Il vous reste {nbGrenade} grenade(s) et {nbGrenadeSpe} grenade(s) spéciale(s).");
}

bool EstDansLimites(int x, int y, string[,] plateau)
{
    return x >= 0 && x < plateau.GetLength(1) &&
           y >= 0 && y < plateau.GetLength(0);
}

bool CaseLibre(int x, int y, string[,] plateau)
{
    return plateau[y, x] != "💥" && plateau[y, x] != "🟥" && plateau[y, x] != "🟩"; //IR n'est pas affectée par les crevasses crée par les grenades, Owen non plus
}

//Entrer les coordonnées de la Grenade

void SelectionCoordoneesGrenade(ref int coorYGrenade, ref int coorXGrenade)
{
    bool saisieValide;
    string saisie;
    Console.WriteLine("Sélectionnez où lancer la grenade:");
    Console.WriteLine("Entrez le numéro de ligne :");
    do
    {
        saisie = Console.ReadLine()!;
        saisieValide = int.TryParse(saisie, out coorYGrenade);

        if (!saisieValide)
        {
            Console.WriteLine("Saisie invalide. Veuillez entrer un entier.");
        }
    } while (!saisieValide);
    coorYGrenade = Convert.ToInt32(saisie);
    Console.WriteLine("Entrez le numéro de colonne :");
    do
    {
        saisie = Console.ReadLine()!;
        saisieValide = int.TryParse(saisie, out coorXGrenade);

        if (!saisieValide)
        {
            Console.WriteLine("Saisie invalide. Veuillez entrer un entier.");
        }
    } while (!saisieValide);
    coorXGrenade = Convert.ToInt32(saisie);
}


//Sous programme pour gérer les points de vie de Maisie et Blue en cas d'impact

void SystemePV(ref int pV, string nom, string owen)
{

    pV -= 50;
    if (pV == 0)
    {
        Console.WriteLine($"{nom} a été tuée par {owen}. Fin de la partie.");
        finPv = true;

    }

    else
        Console.WriteLine($"{nom} a été touchée par l'impact, attention");
}


//Pouvoir de Blue

void PouvoirBlue(ref int positionYIR, ref int positionXIR) // Lancé que si Blue et IR en même position
{
    plateau[positionYIR, positionXIR] = "⬜"; //Réinitialise l'ancienne position de l'IR
    bool directionValide;
    int deltaX = 0, deltaY = 0; // Déplacement relatif
    int tentativeX = positionXIR;
    int tentativeY = positionYIR;

    //Verification de la validité de la saisie
    do
    {
        Console.WriteLine("Sélectionnez la direction dans laquelle envoyer l'IR: Nord, Sud, Est ou Ouest ?");
        string direction = Console.ReadLine()!;

        if (direction.Equals("Ouest", StringComparison.OrdinalIgnoreCase) && positionXIR != 0)  //Pour le déplacer vers l'ouest, l'IR ne doit pas etre dans la première colonne du plateau
        {
            deltaX = -1;
            directionValide = true;
        }
        else if (direction.Equals("Est", StringComparison.OrdinalIgnoreCase) && positionXIR != (plateau.GetLength(1) - 1))  //Pour le déplacer vers l'est, l'IR ne doit pas etre dans la dernière colonne du plateau
        {
            deltaX = 1;
            directionValide = true;
        }
        else if (direction.Equals("Nord", StringComparison.OrdinalIgnoreCase) && positionYIR != 0) //Pour le déplacer vers le nord, l'IR ne doit pas etre dans la première ligne du plateau
        {
            deltaY = -1;
            directionValide = true;
        }
        else if (direction.Equals("Sud", StringComparison.OrdinalIgnoreCase) && positionYIR != (plateau.GetLength(0) - 1)) //Pour le déplacer vers le sud, l'IR ne doit pas etre dans la dernière ligne du plateau
        {
            deltaY = 1;
            directionValide = true;
        }
        else
        {
            Console.WriteLine("Direction invalide !");
            directionValide = false;
        }
    } while (directionValide == false);

    // Calcul du déplacement maximal (3 cases ou jusqu'à un obstacle)
    for (int i = 1; i <= 3; i++)
    {
        tentativeX = positionXIR + deltaX * i;
        tentativeY = positionYIR + deltaY * i;

        // Vérifier les limites du plateau et les obstacles
        if (tentativeX < 0 || tentativeX >= plateau.GetLength(1) ||
            tentativeY < 0 || tentativeY >= plateau.GetLength(0) ||
            plateau[tentativeY, tentativeX] == "💥")
        {
            // Si la limite ou un obstacle est atteint, ne pas aller plus loin
            tentativeX = positionXIR + deltaX * (i - 1);
            tentativeY = positionYIR + deltaY * (i - 1);
            i = 4; // Forcer la fin de la boucle
        }
    }

    // Mettre à jour la position finale
    positionXIR = tentativeX;
    positionYIR = tentativeY;

    // Positionner IR aux nouvelles coordonnées
    plateau[positionYIR, positionXIR] = "🟥";
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
    for (int t = 0; t < plateau.GetLength(1); t++)
    {
        Console.Write($"{t} ");
    }

    Console.WriteLine();
    for (int i = 0; i < plateau.GetLength(0); i++)
    {
        Console.Write($"{plateau.GetLength(1) * i / plateau.GetLength(1)}");
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

void RecupererCoord(string[,] plateau, ref int positionXOwen, ref int positionYOwen, ref int positionXIR, ref int positionYIR, ref int positionXMaisie, ref int positionYMaisie, ref int positionXBlue, ref int positionYBlue)
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
                if ((personnage == "🟪") && (plateau[newY, newX] == "⬜")) //Maisie ne peut pas tomber par accident sur IR, ni sur un autre joueur, ni sur une crevasse ou une grenade spéciale
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

// Vérifie si l'IR est n'a aucun moyen d'accéder à Owen et si un personnage est présent où non dans l'enclos
void TesterEnclos(ref string[,] plateau, ref int positionXOwen, ref int positionYOwen, ref int positionXIR, ref int positionYIR, ref int positionXBlue, ref int positionYBlue, ref int positionXMaisie, ref int positionYMaisie)
{
    bool[,] casesVisitees = new bool[plateau.GetLength(0), plateau.GetLength(1)];

    //Vérifier si l'IR est isolée d'Owen
    ExplorerZone(positionXIR, positionYIR, plateau, casesVisitees);

    if (casesVisitees[positionYOwen, positionXOwen])
    {
        Console.WriteLine("Pas d'enclos détecté");
        return;
    }

    // Vérifier si un autre personnage est dans l'enclos
    bool IRSeule = true;
    if (casesVisitees[positionYMaisie, positionXMaisie])
    {
        Console.WriteLine("Maisie est enfermée avec l'IR. Partie perdue !");
        IRSeule = false;
        finEnclos = true;
    }
    if (casesVisitees[positionYBlue, positionXBlue])
    {
        Console.WriteLine("Blue est enfermée avec l'IR. Partie perdue !");
        IRSeule = false;
        finEnclos = true;
    }

    if (IRSeule)
    {
        Console.WriteLine("L'IR est isolée seule dans l'enclos. Partie gagnée !");
        finEnclos = true;
    }
}

// Explorer la zone à partir d'une position
void ExplorerZone(int x, int y, string[,] plateau, bool[,] casesVisitees)
{
    int hauteur = plateau.GetLength(0);
    int largeur = plateau.GetLength(1);

    // Vérifier si la case est hors limites, déjà visitée ou sur une explosion
    if (x < 0 || x >= largeur || y < 0 || y >= hauteur || casesVisitees[y, x] || plateau[y, x] == "💥")
    {
        return;
    }

    // Marquer la case comme visitée
    casesVisitees[y, x] = true;

    // Explorer dans les 4 directions à partir de cette case
    ExplorerZone(x - 1, y, plateau, casesVisitees); // Gauche
    ExplorerZone(x + 1, y, plateau, casesVisitees); // Droite
    ExplorerZone(x, y - 1, plateau, casesVisitees); // Haut
    ExplorerZone(x, y + 1, plateau, casesVisitees); // Bas
}



//Commencer une partie
Console.WriteLine("Cliquer sur la touche Entrée pour commencer une partie");
ConsoleKeyInfo key;
do
{
    key = Console.ReadKey(intercept: true);  // Attente d'une touche sans l'afficher

    if (key.Key == ConsoleKey.Enter)
    {
        finPv = false;
        finCroc = false;
        finGrenade = false;
        enervement = false;
        pdvMaisie = 100;
        pdvBlue = 100;
        pdvIR = 10 * nbGrenade;
        nbGrenadeSpe = 0;
        Jeu(ref finPv, ref finCroc, ref finGrenade, ref finEnclos, ref positionXOwen, ref positionYOwen, ref positionXIR, ref positionYIR, ref positionXBlue, ref positionYBlue, ref positionXMaisie, ref positionYMaisie);
    }

    Console.WriteLine("Cliquer sur la touche Entrée pour commencer une partie"); // Rejouer quand la partie est terminée 
    key = Console.ReadKey(intercept: true);
    if (key.Key == ConsoleKey.Enter)
    {
        plateau = CréerPlateau(DemanderHauteurPlateau(), DemanderLongueurPlateau());    // Réinitialise le plateau en début de partie
        RecupererCoord(plateau, ref positionXOwen, ref positionYOwen, ref positionXIR, ref positionYIR, ref positionXMaisie, ref positionYMaisie, ref positionXBlue, ref positionYBlue);
        nbGrenade = plateau.GetLength(1);
        nbGrenadeSpe = 0;
        finPv = false;
        finCroc = false;
        finGrenade = false;
        enervement = false;
        pdvIR = 10 * nbGrenade;
        pdvMaisie = 100;
        pdvBlue = 100;
        AfficherPlateau(plateau);
        Console.WriteLine("Cliquer sur la touche Entrée pour confirmer la taille du plateau");
    }

}
while (key.Key == ConsoleKey.Enter);


//Partie

void Jeu(ref bool finPv, ref bool finCroc, ref bool finGrenade, ref bool finEnclos, ref int positionXOwen, ref int positionYOwen, ref int positionXIR, ref int positionYIR, ref int positionXBlue, ref int positionYBlue, ref int positionXMaisie, ref int positionYMaisie)
{


    while (finCroc == false && finGrenade == false && finPv == false && finEnclos == false) // La partie continue tant que les conditions d'échec ne sont pas vérifiées 
    {

        DeplacementAleatoire("🟪", ref positionXMaisie, ref positionYMaisie);
        AfficherPlateau(plateau);
        Croquer(positionYIR, positionXIR, positionYOwen, positionXOwen, positionYMaisie, positionXMaisie, ref finCroc);
        if (finCroc)
        {
            return; // La partie s'arrête si Maisie est mangée
        }

        DeplacementAleatoire("🟥", ref positionXIR, ref positionYIR);
        AfficherPlateau(plateau);
        Croquer(positionYIR, positionXIR, positionYOwen, positionXOwen, positionYMaisie, positionXMaisie, ref finCroc);
        if (finCroc)
        {
            return; // La partie s'arrête si l'indominus mange un personnage
        }

        DeplacementClavier("🟦", ref positionXBlue, ref positionYBlue, nomBlue);
        AfficherPlateau(plateau);

        if ((positionYBlue == positionYIR) && (positionXBlue == positionXIR))
        {
            PouvoirBlue(ref positionYIR, ref positionXIR);
            AfficherPlateau(plateau);
            Croquer(positionYIR, positionXIR, positionYOwen, positionXOwen, positionYMaisie, positionXMaisie, ref finCroc);
            if (finCroc)
            {
                return; // La partie s'arrête si Owen est mangé
            }
        }

        DeplacementClavier("🟩", ref positionXOwen, ref positionYOwen, nomOwen);
        AfficherPlateau(plateau);
        Croquer(positionYIR, positionXIR, positionYOwen, positionXOwen, positionYMaisie, positionXMaisie, ref finCroc);
        if (finCroc)
        {
            return; // La partie s'arrête si Owen est mangé
        }

        Grenade(positionYOwen, positionXOwen, ref nbGrenade, ref pdvIR, ref pdvBlue, ref pdvMaisie, ref finGrenade, ref enervement, ref nbGrenadeSpe);
        if (finGrenade || finPv)
        {
            return; // La partie s'arrête si un personnage est tué par une grenade
        }
        TesterEnclos(ref plateau, ref positionXOwen, ref positionYOwen, ref positionXIR, ref positionYIR, ref positionXBlue, ref positionYBlue, ref positionXMaisie, ref positionYMaisie);
    }
    Console.WriteLine("La partie est finie !");
}

// Choix
enum ChoixGrenade
{
    Oui,
    oui,
    Non,
    non
}
enum ChoixType
{
    S,
    s,
    N,
    n,
}


