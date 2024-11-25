//Valeurs pour tester A SUPPRIMER
char[,] plateau = new char[10, 10];
int nbGrenade = plateau.GetLength(0);
int nbGrenadeSpe = 1;
int positionXOwen = 3;
int positionYOwen = 4;
int positionXIR = 2;
int positionYIR = 2;
int positionXMaisie = 2;
int positionYMaisie = 3;
int pdvMaisie = 100;
int pdvBlue = 100;
int pdvIR = 10 * nbGrenade;
int coorXGrenadeSpe;
int coorYGrenadeSpe;
Random rng = new Random();
bool enervement = false;

// Lancer ou non d'une grenade, spéciale ou non
void Grenade(int positionXOwen, int positionYOwen, int nbGrenade, int positionXIR, int positionYIR, int pdvIR, int pdvBlue, int pdvMaisie)
{
    int coorXGrenade;
    int coorYGrenade;
    int randomX = 0;
    int randomY = 0;
    Console.WriteLine("Lancer une grenade? (répondre Oui ou Non)");
    if (Console.ReadLine() == "Oui")
    {
        Console.WriteLine("Lancer une grenade spéciale ou normale?");
        if (Console.ReadLine() == "spéciale")
        {
            if (nbGrenadeSpe > 0)
            {
                nbGrenadeSpe -= 1;
                Console.WriteLine("Sélectionnez où lancer la grenade:");
                Console.WriteLine("Entrez le numéro de ligne :");
                coorXGrenade = Convert.ToInt32(Console.ReadLine()!);
                Console.WriteLine("Entrez le numéro de colonne :");
                coorYGrenade = Convert.ToInt32(Console.ReadLine()!);
                while ((coorXGrenade >= positionXOwen + 3) || (coorXGrenade <= positionXOwen - 3) || (coorYGrenade >= positionYOwen + 3) || (coorYGrenade <= positionYOwen - 3))
                {
                    Console.WriteLine("Impossible, Owen a une portée de 3 cases maximum");
                    Console.WriteLine("Sélectionnez où lancer la grenade:");
                    Console.WriteLine("Entrez le numéro de ligne :");
                    coorXGrenade = Convert.ToInt32(Console.ReadLine()!);
                    Console.WriteLine("Entrez le numéro de colonne :");
                    coorYGrenade = Convert.ToInt32(Console.ReadLine()!);
                }
                if ((coorXGrenade <= positionXOwen + 3) || (coorXGrenade >= positionXOwen - 3) || (coorYGrenade <= positionYOwen + 3) || (coorYGrenade >= positionYOwen - 3))
                {
                    if (plateau[coorXGrenade, coorYGrenade] == 'I') // Savoir si l'IR a été touchée et lui enlever des pv et pas de crevasse
                    {
                        Console.WriteLine("IR a été touchée, -10 points de vie");
                        pdvIR -= 10;
                        Console.WriteLine($"Points de vie de l'IR : {pdvIR}");
                        enervement = true; //sera plus rapide au prochain déplacement
                        Console.WriteLine("IR est énervée, faites attention au prochain tour");
                    }
                    if (plateau[coorXGrenade, coorYGrenade] == 'B')
                    {
                        Console.WriteLine("Blue a été tuée par Owen. Fin de la partie.");
                    }
                    if (plateau[coorXGrenade, coorYGrenade] == 'M')
                    {
                        Console.WriteLine("Maisie a été tuée par Owen. Fin de la partie.");
                    }
                    else // sinon on crée une crevasse
                    {
                        plateau[coorXGrenade, coorYGrenade] = 'X';
                        while ((randomX == 0) && (randomY == 0)) // Pour éviter que la case random soit la même que celle où la grenade atterit
                        {
                            randomX = rng.Next(-1, 2);
                            randomY = rng.Next(-1, 2);
                        }
                        coorXGrenadeSpe = coorXGrenade + randomX;
                        coorYGrenadeSpe = coorYGrenade + randomY;
                        if (plateau[coorXGrenadeSpe, coorYGrenadeSpe] == 'B')
                        {
                            pdvBlue /= 2;
                            if (pdvBlue == 0)
                                Console.WriteLine("Blue a été tuée par Owen. Fin de la partie.");
                            else
                                Console.WriteLine("Blue a été touchée par l'impact, attention");
                        }
                        else if (plateau[coorXGrenadeSpe, coorYGrenadeSpe] == 'M')
                        {
                            pdvMaisie /= 2;
                            if (pdvMaisie == 0)
                                Console.WriteLine("Maisie a été tuée par Owen. Fin de la partie.");
                            else
                                Console.WriteLine("Maisie a été touchée par l'impact, attention");
                        }
                        else
                            plateau[coorXGrenadeSpe, coorYGrenadeSpe] = 'X';
                        do
                        {
                            randomX = rng.Next(-1, 2);
                            randomY = rng.Next(-1, 2);
                        }
                        while ((randomX == 0) && (randomY == 0)); // Pour éviter que la case random soit la même que celle où la grenade atterit
                        {
                            randomX = rng.Next(-1, 2);
                            randomY = rng.Next(-1, 2);
                        }
                        if (plateau[coorXGrenadeSpe + randomX, coorYGrenadeSpe + randomY] == 'B')
                        {
                            pdvBlue /= 2;
                            if (pdvBlue == 0)
                                Console.WriteLine("Blue a été tuée par Owen. Fin de la partie.");
                            else
                                Console.WriteLine("Blue a été touchée par l'impact, attention");
                        }
                        else if (plateau[coorXGrenadeSpe + randomX, coorYGrenadeSpe + randomY] == 'M')
                        {
                            pdvMaisie /= 2;
                            if (pdvMaisie == 0)
                                Console.WriteLine("Maisie a été tuée par Owen. Fin de la partie.");
                            else
                                Console.WriteLine("Maisie a été touchée par l'impact, attention");
                        }
                        plateau[coorXGrenadeSpe + randomX, coorYGrenadeSpe + randomY] = 'X';
                    }

                }

            }
            else
            {
                Console.WriteLine("Vous n'avez plus de grenade spéciale");
                Console.WriteLine("Voulez vous lancer une grenade normale? (si oui, répondre normale)");
            }
        }

        if (Console.ReadLine() == "normale")
        {
            nbGrenade -= 1;
            if (nbGrenade > 0)
            {
                Console.WriteLine("Sélectionnez où lancer la grenade:");
                Console.WriteLine("Entrez le numéro de ligne :");
                coorXGrenade = Convert.ToInt32(Console.ReadLine()!);
                Console.WriteLine("Entrez le numéro de colonne :");
                coorYGrenade = Convert.ToInt32(Console.ReadLine()!);
                while ((coorXGrenade >= positionXOwen + 3) || (coorXGrenade <= positionXOwen - 3) || (coorYGrenade >= positionYOwen + 3) || (coorYGrenade <= positionYOwen - 3))
                {
                    Console.WriteLine("Impossible, Owen a une portée de 3 cases maximum");
                    Console.WriteLine("Sélectionnez où lancer la grenade:");
                    Console.WriteLine("Entrez le numéro de ligne :");
                    coorXGrenade = Convert.ToInt32(Console.ReadLine()!);
                    Console.WriteLine("Entrez le numéro de colonne :");
                    coorYGrenade = Convert.ToInt32(Console.ReadLine()!);
                }
                if ((coorXGrenade <= positionXOwen + 3) || (coorXGrenade >= positionXOwen - 3) || (coorYGrenade <= positionYOwen + 3) || (coorYGrenade >= positionYOwen - 3))
                {
                    if (plateau[coorXGrenade, coorYGrenade] == 'I') // Savoir si l'IR a été touchée et lui enlever des pv et pas de crevasse
                    {
                        Console.WriteLine("IR a été touchée, -10 points de vie");
                        pdvIR -= 10;
                        Console.WriteLine($"Points de vie de l'IR : {pdvIR}");
                        enervement = true; //sera plus rapide au prochain déplacement
                        Console.WriteLine("IR est énervée, faites attention au prochain tour");
                    }
                    else if (plateau[coorXGrenade, coorYGrenade] == 'B')
                        Console.WriteLine("Blue a été tuée par Owen. Fin de la partie.");
                    else if (plateau[coorXGrenade, coorYGrenade] == 'M')
                        Console.WriteLine("Maisie a été tuée par Owen. Fin de la partie.");
                    else // sinon on crée une crevasse
                    {
                        plateau[coorXGrenade, coorYGrenade] = 'X';
                        while ((randomX == 0) && (randomY == 0)) // Pour éviter que la case random soit la même que celle où la grenade atterit
                        {
                            randomX = rng.Next(-1, 2);
                            randomY = rng.Next(-1, 2);
                        }
                        if (plateau[coorXGrenade + randomX, coorYGrenade + randomY] == 'B')
                        {
                            pdvBlue /= 2;
                            if (pdvBlue == 0)
                                Console.WriteLine("Blue a été tuée par Owen. Fin de la partie.");
                            if (pdvBlue > 0)
                                Console.WriteLine("Blue a été touchée par l'impact, attention");
                        }
                        else if (plateau[coorXGrenade + randomX, coorYGrenade + randomY] == 'M')
                        {
                            pdvMaisie /= 2;
                            if (pdvMaisie == 0)
                                Console.WriteLine("Maisie a été tuée par Owen. Fin de la partie.");
                            if (pdvMaisie > 0)
                                Console.WriteLine("Maisie a été touchée par l'impact, attention");
                        }
                        else
                            plateau[coorXGrenade + randomX, coorYGrenade + randomY] = 'X';
                    }

                }

            }
            if (nbGrenade == 0)
                Console.WriteLine("Vous n'avez plus de grenades, bonne chance!");
        }
    }
    if (Console.ReadLine() == "Non")
    {
        Console.WriteLine("Owen ne lance pas de grenade.");
    }
    AfficherPlateau(plateau);
    Console.WriteLine($"Il vous reste {nbGrenade} grenades et {nbGrenadeSpe} grenades spéciales.");
}


// Sous programme d'affichage du plateau

void AfficherPlateau(char[,] plateau)
{
    plateau[positionXOwen, positionYOwen] = 'O';
    plateau[positionXMaisie, positionYMaisie] = 'M';
    plateau[positionXIR, positionYIR] = 'I';
    for (int i = 0; i < plateau.GetLength(0) - 1; i++)
    {
        for (int j = 0; j < plateau.GetLength(1) - 1; j++)
        {
            Console.Write(plateau[i, j] + "\t");
        }

        Console.WriteLine("");
    }
}

void PouvoirBlue(ref int positionXIR, ref int positionYIR) // Lancé que si Blue et IR en même position
{
    plateau[positionXIR, positionYIR] = '-'; // Supprimer le caractère I du plateau aux anciennes positions
    Console.WriteLine("Sélectionnez la direction dans laquelle envoyer l'IR: Nord, Sud, Est ou Ouest ?");
    string direction = Console.ReadLine()!;
    if (direction == "Ouest")
    {
        for (int i = 1; i <= 3; i++)
        {
            if ((positionYIR - i < 0) || (plateau[positionXIR, positionYIR - i] == 'X')) // Si on dépasse les limites du plateau ou qu'une crevasse est atteinte
                positionYIR -= (i - 1);

        }
        //Si absence de crevasse et de bordure
        if ((positionYIR - 3 >= 0) && (plateau[positionXIR, positionYIR - 1] != 'X') && (plateau[positionXIR, positionYIR - 2] != 'X') && (plateau[positionXIR, positionYIR - 3] != 'X'))
            positionYIR -= 3;
    }
    else if (direction == "Est")
    {
        for (int i = 1; i <= 3; i++)
        {
            if ((positionYIR + i >= plateau.GetLength(1) - 1) || (plateau[positionXIR, positionYIR + i] == 'X'))
                positionYIR += (i - 1);

        }
        if ((positionYIR + 3 < plateau.GetLength(1)) && (plateau[positionXIR, positionYIR + 1] != 'X') && (plateau[positionXIR, positionYIR + 2] != 'X') && (plateau[positionXIR, positionYIR + 3] != 'X'))
            positionYIR += 3;
    }

    else if (direction == "Sud")
    {
        for (int i = 1; i <= 3; i++)
        {
            if ((positionXIR + i >= plateau.GetLength(0) - 1) || (plateau[positionXIR + i, positionYIR] == 'X'))
                positionXIR += (i - 1);
        }
        if ((positionXIR + 3 < plateau.GetLength(1)) && (plateau[positionXIR + 1, positionYIR] != 'X') && (plateau[positionXIR + 2, positionYIR] != 'X') && (plateau[positionXIR + 3, positionYIR] != 'X'))
            positionXIR += 3;
    }

    else if (direction == "Nord")
    {
        for (int i = 1; i <= 3; i++)
        {
            if ((positionXIR - i < 0) || (plateau[positionXIR - i, positionYIR] == 'X'))
                positionXIR -= (i - 1);
        }
        if ((positionXIR - 3 >= 0) && (plateau[positionXIR - 1, positionYIR] != 'X') && (plateau[positionXIR - 2, positionYIR] != 'X') && (plateau[positionXIR - 3, positionYIR] != 'X'))
            positionXIR -= 3;
    }
    plateau[positionXIR, positionYIR] = 'I'; // Positionner I aux nouvelles coor
}

void Croquer(int positionXIR, int positionYIR, int positionXOwen, int positionYOwen, int positionXMaisie, int positionYMaisie)
{
    if ((positionXIR == positionXMaisie) && (positionYIR == positionYMaisie))
    {
        plateau[positionXMaisie, positionYMaisie] = 'I';
        Console.WriteLine("Maisie a été mangée, fin de la partie");
    }
    else if ((positionXIR == positionXOwen) && (positionYIR == positionYOwen))
    {
        plateau[positionXOwen, positionYOwen] = 'I';
        Console.WriteLine("Owen a été mangé, fin de la partie");
    }
    else
        Console.WriteLine("Bien joué ! Personne n'a été croqué.e ");
}

void RecupererGrenadeSpe(int positionXOwen, int positionYOwen)
{
    if (plateau[positionXOwen, positionYOwen] == 'G')
        nbGrenadeSpe += 1;
}

// Remplissage initial du plateau pour test A SUPPRIMER
for (int i = 0; i < plateau.GetLength(0) - 1; i++)
{
    for (int j = 0; j < plateau.GetLength(1) - 1; j++)
    {
        plateau[i, j] = '-';
    }
}


//TESTS A SUPPRIMER
AfficherPlateau(plateau);
Grenade(positionXOwen, positionYOwen, nbGrenade, positionXIR, positionYIR, pdvIR, pdvBlue, pdvMaisie);
//Croquer(positionXIR, positionYIR, positionXOwen, positionYOwen, positionXMaisie, positionYMaisie);
//AfficherPlateau(plateau);
//PouvoirBlue(ref positionXIR, ref positionYIR);