using System.Reflection.PortableExecutable;

string[,] CréerPlateau(int dim1,int dim2) 
{
    string[,] mat = new string[dim1,dim2];	            

    for (int i = 0 ; i < mat.GetLength(0) ; i++)	//Initialisation du plateau vide
    {
	    for (int j = 0 ; j < mat.GetLength(1) ; j++)
        {
            mat[i,j] = "⬜";
        }
    }
                                                    //Placement aléatoire des joueurs
    PlacerAléatoire("🟩",mat); //Owen         
    PlacerAléatoire("🟦",mat); //Blue
    PlacerAléatoire("🟪",mat); //Maisie
    PlacerAléatoire("🟥",mat); //IR
    
    for (int i = 0 ; i < 2 ; i++ )   //Grenades spéciales placées aléatoirement, changer le i<2 si on en veut plus !
    {
        PlacerAléatoire("🧨",mat);  //"💥" symbole à utiliser pour les trous de grenade 
    }

    return mat;
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

void AfficherPlateau(string[,] mat)      //Afficher le plateau
{
       for (int i = 0 ; i < mat.GetLength(0) ; i++)	
    {
	    for (int j = 0 ; j < mat.GetLength(1) ; j++)
        {
            Console.Write(mat[i,j]);
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

//Tests à supprimer
string[,] plat = CréerPlateau(15,15);
AfficherPlateau(plat);

