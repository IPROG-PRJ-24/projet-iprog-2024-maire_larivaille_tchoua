//Valeurs pour tester A SUPPRIMER
char[,] plateau = new char[10, 10];
int nbGrenade = plateau.GetLength(0);
int nbGrenadeSpe = 1;
int positionYOwen = 3;
int positionXOwen = 4;
int positionYIR = 2;
int positionXIR = 2;
int positionYMaisie = 2;
int positionXMaisie = 3;
int pdvMaisie = 100;
int pdvBlue = 100;
int pdvIR = 10 * nbGrenade;
int coorYGrenadeSpe;
int coorXGrenadeSpe;
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
                coorYGrenade = Convert.ToInt32(Console.ReadLine()!);
                Console.WriteLine("Entrez le numéro de colonne :");
                coorXGrenade = Convert.ToInt32(Console.ReadLine()!);
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
                    if (plateau[coorYGrenade, coorXGrenade] == 'I') // Savoir si l'IR a été touchée et lui enlever des pv et pas de crevasse
                    {
                        Console.WriteLine("IR a été touchée, -10 points de vie");
                        pdvIR -= 10;
                        Console.WriteLine($"Points de vie de l'IR : {pdvIR}");
                        enervement = true; //sera plus rapide au prochain déplacement
                        Console.WriteLine("IR est énervée, faites attention au prochain tour");
                    }
                    if (plateau[coorYGrenade, coorXGrenade] == 'B')
                    {
                        Console.WriteLine("Blue a été tuée par Owen. Fin de la partie.");
                    }
                    if (plateau[coorYGrenade, coorXGrenade] == 'M')
                    {
                        Console.WriteLine("Maisie a été tuée par Owen. Fin de la partie.");
                    }
                    else // sinon on crée une crevasse
                    {
                        plateau[coorYGrenade, coorXGrenade] = 'X';
                        while ((randomY == 0) && (randomX == 0)) // Pour éviter que la case random soit la même que celle où la grenade atterit
                        {
                            randomY = rng.Next(-1, 2);
                            randomX = rng.Next(-1, 2);
                        }
                        coorYGrenadeSpe = coorYGrenade + randomY;
                        coorXGrenadeSpe = coorXGrenade + randomX;
                        if (plateau[coorYGrenadeSpe, coorXGrenadeSpe] == 'B')
                        {
                            pdvBlue /= 2;
                            if (pdvBlue == 0)
                                Console.WriteLine("Blue a été tuée par Owen. Fin de la partie.");
                            else
                                Console.WriteLine("Blue a été touchée par l'impact, attention");
                        }
                        else if (plateau[coorYGrenadeSpe, coorXGrenadeSpe] == 'M')
                        {
                            pdvMaisie /= 2;
                            if (pdvMaisie == 0)
                                Console.WriteLine("Maisie a été tuée par Owen. Fin de la partie.");
                            else
                                Console.WriteLine("Maisie a été touchée par l'impact, attention");
                        }
                        else
                            plateau[coorYGrenadeSpe, coorXGrenadeSpe] = 'X';
                        do
                        {
                            randomY = rng.Next(-1, 2);
                            randomX = rng.Next(-1, 2);
                        }
                        while ((randomY == 0) && (randomX == 0)); // Pour éviter que la case random soit la même que celle où la grenade atterit
                        {
                            randomY = rng.Next(-1, 2);
                            randomX = rng.Next(-1, 2);
                        }
                        if (plateau[coorYGrenadeSpe + randomY, coorXGrenadeSpe + randomX] == 'B')
                        {
                            pdvBlue /= 2;
                            if (pdvBlue == 0)
                                Console.WriteLine("Blue a été tuée par Owen. Fin de la partie.");
                            else
                                Console.WriteLine("Blue a été touchée par l'impact, attention");
                        }
                        else if (plateau[coorYGrenadeSpe + randomY, coorXGrenadeSpe + randomX] == 'M')
                        {
                            pdvMaisie /= 2;
                            if (pdvMaisie == 0)
                                Console.WriteLine("Maisie a été tuée par Owen. Fin de la partie.");
                            else
                                Console.WriteLine("Maisie a été touchée par l'impact, attention");
                        }
                        plateau[coorYGrenadeSpe + randomY, coorXGrenadeSpe + randomX] = 'X';
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
            if (nbGrenade > 0)
            {
                nbGrenade -= 1;
                Console.WriteLine("Sélectionnez où lancer la grenade:");
                Console.WriteLine("Entrez le numéro de ligne :");
                coorYGrenade = Convert.ToInt32(Console.ReadLine()!);
                Console.WriteLine("Entrez le numéro de colonne :");
                coorXGrenade = Convert.ToInt32(Console.ReadLine()!);
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
                    if (plateau[coorYGrenade, coorXGrenade] == 'I') // Savoir si l'IR a été touchée et lui enlever des pv et pas de crevasse
                    {
                        Console.WriteLine("IR a été touchée, -10 points de vie");
                        pdvIR -= 10;
                        Console.WriteLine($"Points de vie de l'IR : {pdvIR}");
                        enervement = true; //sera plus rapide au prochain déplacement
                        Console.WriteLine("IR est énervée, faites attention au prochain tour");
                    }
                    else if (plateau[coorYGrenade, coorXGrenade] == 'B')
                        Console.WriteLine("Blue a été tuée par Owen. Fin de la partie.");
                    else if (plateau[coorYGrenade, coorXGrenade] == 'M')
                        Console.WriteLine("Maisie a été tuée par Owen. Fin de la partie.");
                    else // sinon on crée une crevasse
                    {
                        plateau[coorYGrenade, coorXGrenade] = 'X';
                        while ((randomY == 0) && (randomX == 0)) // Pour éviter que la case random soit la même que celle où la grenade atterit
                        {
                            randomY = rng.Next(-1, 2);
                            randomX = rng.Next(-1, 2);
                        }
                        if (plateau[coorYGrenade + randomY, coorXGrenade + randomX] == 'B')
                        {
                            pdvBlue /= 2;
                            if (pdvBlue == 0)
                                Console.WriteLine("Blue a été tuée par Owen. Fin de la partie.");
                            if (pdvBlue > 0)
                                Console.WriteLine("Blue a été touchée par l'impact, attention");
                        }
                        else if (plateau[coorYGrenade + randomY, coorXGrenade + randomX] == 'M')
                        {
                            pdvMaisie /= 2;
                            if (pdvMaisie == 0)
                                Console.WriteLine("Maisie a été tuée par Owen. Fin de la partie.");
                            if (pdvMaisie > 0)
                                Console.WriteLine("Maisie a été touchée par l'impact, attention");
                        }
                        else
                            plateau[coorYGrenade + randomY, coorXGrenade + randomX] = 'X';
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
    plateau[positionYOwen, positionXOwen] = 'O';
    plateau[positionYMaisie, positionXMaisie] = 'M';
    plateau[positionYIR, positionXIR] = 'I';
    for (int i = 0; i < plateau.GetLength(0) - 1; i++)
    {
        for (int j = 0; j < plateau.GetLength(1) - 1; j++)
        {
            Console.Write(plateau[i, j] + "\t");
        }

        Console.WriteLine("");
    }
}

void PouvoirBlue(ref int positionYIR, ref int positionXIR) // Lancé que si Blue et IR en même position
{
    plateau[positionYIR, positionXIR] = '-'; // Supprimer le caractère I du plateau aux anciennes positions
    Console.WriteLine("Sélectionnez la direction dans laquelle envoyer l'IR: Nord, Sud, Est ou Ouest ?");
    string direction = Console.ReadLine()!;
    if (direction == "Ouest")
    {
        for (int i = 1; i <= 3; i++)
        {
            if ((positionXIR - i < 0) || (plateau[positionYIR, positionXIR - i] == 'X')) // Si on dépasse les limites du plateau ou qu'une crevasse est atteinte
                positionXIR -= (i - 1);

        }
        //Si absence de crevasse et de bordure
        if ((positionXIR - 3 >= 0) && (plateau[positionYIR, positionXIR - 1] != 'X') && (plateau[positionYIR, positionXIR - 2] != 'X') && (plateau[positionYIR, positionXIR - 3] != 'X'))
            positionXIR -= 3;
    }
    else if (direction == "Est")
    {
        for (int i = 1; i <= 3; i++)
        {
            if ((positionXIR + i >= plateau.GetLength(1) - 1) || (plateau[positionYIR, positionXIR + i] == 'X'))
                positionXIR += (i - 1);

        }
        if ((positionXIR + 3 < plateau.GetLength(1)) && (plateau[positionYIR, positionXIR + 1] != 'X') && (plateau[positionYIR, positionXIR + 2] != 'X') && (plateau[positionYIR, positionXIR + 3] != 'X'))
            positionXIR += 3;
    }

    else if (direction == "Sud")
    {
        for (int i = 1; i <= 3; i++)
        {
            if ((positionYIR + i >= plateau.GetLength(0) - 1) || (plateau[positionYIR + i, positionXIR] == 'X'))
                positionYIR += (i - 1);
        }
        if ((positionYIR + 3 < plateau.GetLength(1)) && (plateau[positionYIR + 1, positionXIR] != 'X') && (plateau[positionYIR + 2, positionXIR] != 'X') && (plateau[positionYIR + 3, positionXIR] != 'X'))
            positionYIR += 3;
    }

    else if (direction == "Nord")
    {
        for (int i = 1; i <= 3; i++)
        {
            if ((positionYIR - i < 0) || (plateau[positionYIR - i, positionXIR] == 'X'))
                positionYIR -= (i - 1);
        }
        if ((positionYIR - 3 >= 0) && (plateau[positionYIR - 1, positionXIR] != 'X') && (plateau[positionYIR - 2, positionXIR] != 'X') && (plateau[positionYIR - 3, positionXIR] != 'X'))
            positionYIR -= 3;
    }
    plateau[positionYIR, positionXIR] = 'I'; // Positionner I aux nouvelles coor
}

void Croquer(int positionYIR, int positionXIR, int positionYOwen, int positionXOwen, int positionYMaisie, int positionXMaisie)
{
    if ((positionYIR == positionYMaisie) && (positionXIR == positionXMaisie))
    {
        plateau[positionYMaisie, positionXMaisie] = 'I';
        Console.WriteLine("Maisie a été mangée, fin de la partie");
    }
    else if ((positionYIR == positionYOwen) && (positionXIR == positionXOwen))
    {
        plateau[positionYOwen, positionXOwen] = 'I';
        Console.WriteLine("Owen a été mangé, fin de la partie");
    }
    else
        Console.WriteLine("Bien joué ! Personne n'a été croqué.e ");
}

void RecupererGrenadeSpe(int positionYOwen, int positionXOwen)
{
    if (plateau[positionYOwen, positionXOwen] == 'G')
        nbGrenadeSpe += 1;
    plateau[positionYOwen, positionXOwen] = 'O';
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
Grenade(positionYOwen, positionXOwen, nbGrenade, pdvIR, pdvBlue, pdvMaisie);
//Croquer(positionYIR, positionXIR, positionYOwen, positionXOwen, positionYMaisie, positionXMaisie);
//AfficherPlateau(plateau);
//PouvoirBlue(ref positionYIR, ref positionXIR);