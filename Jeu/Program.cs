using System.Reflection.PortableExecutable;
int positionXOwen = 0;
int positionYOwen = 0;
int positionXIR = 0;
int positionYIR = 0;
int positionXMaisie = 0;
int positionYMaisie = 0;
int positionYBlue = 0;
int positionXBlue = 0;

string[,] CréerPlateau(int dim1,int dim2) 
{
    string[,] plateau = new string[dim1,dim2];	            

    for (int i = 0 ; i < plateau.GetLength(0) ; i++)	//Initialisation du plateau vide
    {
	    for (int j = 0 ; j < plateau.GetLength(1) ; j++)
        {
            plateau[i,j] = "⬜";
        }
    }
                                                    //Placement aléatoire des joueurs
    PlacerAléatoire("🟩",plateau); //Owen         
    PlacerAléatoire("🟦",plateau); //Blue
    PlacerAléatoire("🟪",plateau); //Maisie
    PlacerAléatoire("🟥",plateau); //IR
    
    for (int i = 0 ; i < 2 ; i++ )   //Grenades spéciales placées aléatoirement, changer le i<2 si on en veut plus !
    {
        PlacerAléatoire("🧨",plateau);  //"💥" symbole à utiliser pour les trous de grenade 
    }

    return plateau;
}

string [,] PlacerAléatoire(string perso, string[,] plateau)     
{
    int x;
    int y;

    do
    {
        x = TirerNbAléatoire(plateau.GetLength(1));     // Tirer un x (abscisse) aléatoire entre 0 et le nombre de colonnes du plateau
        y = TirerNbAléatoire(plateau.GetLength(0));     // Tirer un y (ordonné) aléatoire entre 0 et le nombre de lignes du plateau
    }
    while (plateau[y,x]!="⬜");

    plateau[y,x] = perso;
    return plateau;
}

void AfficherPlateau(string[,] plateau)      //Afficher le plateau
{
    plateau[positionYOwen, positionXOwen] = "🟩";
    plateau[positionYMaisie, positionXMaisie] = "🟪";
    plateau[positionYBlue, positionXBlue] = "🟦";
    plateau[positionYIR, positionXIR] = "🟥";

       for (int i = 0 ; i < plateau.GetLength(0) ; i++)	
    {
	    for (int j = 0 ; j < plateau.GetLength(1) ; j++)
        {
            Console.Write(plateau[i,j]);
        }
        Console.WriteLine();
    }
}

int TirerNbAléatoire(int max)   //Tirer un nombre aléatoire
{
    Random rng = new Random();
    int nb = rng.Next(0,max);   //max : borne supérieure en paramètre
    return(nb);
}

void RécupérerCoord(string[,] plateau, ref int positionXOwen, ref int positionYOwen, ref int positionXIR, ref int positionYIR, ref int positionXMaisie, ref int positionYMaisie, ref int positionXBlue, ref int positionYBlue)
{
    for (int i = 0; i <plateau.GetLength(0) ; i++)	
    {
        for (int j = 0 ; j < plateau.GetLength(1) ; j++)
        {
            if (plateau[i,j] == "🟩")
            {
                positionYOwen = i;
                positionXOwen = j;
            }
             if (plateau[i,j] == "🟪")
            {
                positionYMaisie = i;
                positionXMaisie = j;
            }
             if (plateau[i,j] == "🟦")
            {
                positionYBlue = i;
                positionXBlue = j;
            }
             if (plateau[i,j] == "🟥")
            {
                positionYIR = i;
                positionXIR = j;
            }   
        }
    }  
}

//Tests à supprimer
string[,] plateau = CréerPlateau(15,15);

RécupérerCoord(plateau, ref positionXOwen, ref positionYOwen, ref positionXIR, ref positionYIR, ref positionXMaisie, ref positionYMaisie, ref positionXBlue, ref positionYBlue);

AfficherPlateau(plateau);

Console.WriteLine($"Position Owen : y : {positionYOwen}, x : {positionXOwen}");

