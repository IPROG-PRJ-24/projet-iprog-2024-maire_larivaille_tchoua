int nbGrenade = 10;
int positionXOwen = 3;
int positionYOwen = 4;
int positionXIR = 5;
int positionYIR = 6;
int pdvIR = 100;
char[,] plateau = new char[10, 8];
Random rng = new Random();
plateau[positionXIR, positionYIR] = 'I';
plateau[positionXOwen, positionYOwen] = 'O';
bool enervement = false;

// Sous programme pour le lancer d'une grenade
void Grenade(int positionXOwen, int positionYOwen, int nbGrenade, int positionXIR, int positionYIR, int pdvIR)
{
    int coorXGrenade;
    int coorYGrenade;
    int randomX = 0;
    int randomY = 0;
    Console.WriteLine("Lancer une grenade? (répondre Oui ou Non)");
    if (Console.ReadLine() == "Oui")
    {
        nbGrenade -= 1;
        if (nbGrenade > 0)
        {
            Console.WriteLine("Sélectionnez où lancer la grenade:");
            Console.WriteLine("Entrez le numéro de colonne :");
            coorXGrenade = Convert.ToInt32(Console.ReadLine()!);
            Console.WriteLine("Entrez le numéro de ligne :");
            coorYGrenade = Convert.ToInt32(Console.ReadLine()!);
            while ((coorXGrenade >= positionXOwen + 3) || (coorXGrenade <= positionXOwen - 3) || (coorYGrenade >= positionYOwen + 3) || (coorYGrenade <= positionYOwen - 3))
            {
                Console.WriteLine("Impossible, Owen a une portée de 3 cases maximum");
                Console.WriteLine("Sélectionnez où lancer la grenade:");
                Console.WriteLine("Entrez le numéro de colonne :");
                coorXGrenade = Convert.ToInt32(Console.ReadLine()!);
                Console.WriteLine("Entrez le numéro de ligne :");
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
                else // sinon on crée une crevasse
                {
                    plateau[coorXGrenade, coorYGrenade] = 'X';
                    while ((randomX == 0) && (randomY == 0)) // Pour éviter que la case random soit la même que celle où la grenade atterit
                    {
                        randomX = rng.Next(-1, 2);
                        randomY = rng.Next(-1, 2);
                    }
                    plateau[coorXGrenade + randomX, coorYGrenade + randomY] = 'X';
                }

            }

        }
        if (nbGrenade == 0)
            Console.WriteLine("Vous n'avez plus de grenades, bonne chance!");
    }

    if (Console.ReadLine() == "Non")
    {
        Console.WriteLine("Owen ne lance pas de grenade.");
    }
    AfficherPlateau(plateau);
    Console.WriteLine($"Il vous reste {nbGrenade} grenades.");
}

Grenade(positionXOwen, positionYOwen, nbGrenade, positionXIR, positionYIR, pdvIR);

// Sous programme d'affichage du plateau

void AfficherPlateau(char[,] plateau)
{
    for (int i = 0; i < plateau.GetLength(0) - 1; i++)
    {
        for (int j = 0; j < plateau.GetLength(1) - 1; j++)
        {
            Console.Write(plateau[i, j] + "\t");
        }

        Console.WriteLine("");
    }
}
