using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace MIXMO_BR
{
    
    

    class Program
    {
        private static readonly string ADRESSE_SAUVEGARDE = "/Users/brahimtalb/Desktop/MIXMO_TALB/Sauvegardes/";
            

        /// <summary>
        /// Nombre de lettre de depart
        /// </summary>
        public const int NOMBRE_LETTRES_PIOCHE_DEPART = 6;

        /// <summary>
        /// Nombres de lettres dans l'alphabet
        /// </summary>
        public const int NOMBRE_LETTRES = 26;

        public const int TAILLE_GRILLE = 10;

        private static bool joker = false;

        internal static Lettres lettres;

        private static Dictionnaire dico;

        ///<summary>
        ///Liste contenant tous les joueurs. L'ordre de passage pour jouer est l'index auquel sont les joueurs dans cette liste
        ///</summary>
        private static List<Joueur> joueurs;

        static void Main(string[] args)
        {
            Console.WriteLine("Bienvenue dans Mixmo");
            Console.WriteLine("Entrez le nombre de joueur");
            int nbr = Convert.ToInt32(Console.ReadLine());
            Console.OutputEncoding = Encoding.UTF8;
            InitLettres();
            InitDico();
            InitJoueurs(nbr);

            
            for (int i = 0; i < nbr; i++)
            {
                bool test = true;

                while (test==true)
                {
               

                    Console.WriteLine(joueurs[i].Grille);
                    Console.WriteLine("joueur " + (i+1));
                    Console.WriteLine("score : " + joueurs[i].AddScore());
                    Console.WriteLine(joueurs[i].Pioche);
                    Console.WriteLine("Souhaitez vous terminer votre tour ? (oui/non)");
                    string test_tour = Console.ReadLine();
                    if (test_tour == "oui")

                    {
                        test = false;
                    }

                    if (test == true)
                    {
                        Console.WriteLine("X :");
                        int posX = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Y :");
                        int posY = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("coté ? 1 = bas, 0 = droite :");
                        bool bas = Convert.ToInt32(Console.ReadLine()) == 1;

                        Console.WriteLine("mot avec lettres :");
                        string mot = Console.ReadLine();

                        string realMot = joueurs[i].EliminateGridLetters(mot, posX - 1, posY - 1, bas);

                        if (joueurs[i].UniqueWord(mot))
                        {
                            if (joueurs[i].ContainsMot(realMot)) 
                            {
                                if (dico.RechercheDicoRécursif(mot))
                                {
                                    if (realMot != null && joueurs[i].Grille.TryAddMot(mot, posX - 1, posY - 1, bas))
                                    {
                                        joueurs[i].Add_Mot(mot);
                                        joueurs[i].OteLettre(realMot);
                                        joueurs[i].Add_Lettres(2, lettres, new Random());
                               
                                    }
                                    else
                                    {
                                        Console.WriteLine("Erreur : impossible de placer le mot dans la grille...");
                                    }
                                }
                                
                                else
                                {
                                    Console.WriteLine("Erreur : le mot n'est pas présent dans le dico...");
                                }
                                
                            }
                            
                            else
                            {
                                Console.WriteLine("Erreur : impossible de faire le mot avec la pioche actuelle ou dans la grille...");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Erreur : le mot a déjà été placé...");
                        }
                    }

                }


            }

            Console.WriteLine("Fin de la partie!");

            for (int i = 0; i < nbr; i++)
            {
                Console.WriteLine("le joueur " + (i+1) + " a reçu " + joueurs[i].AddScore());
            }
        }

        /// <summary>
        /// Nous permet de lire le fichier et de recuperer chaque mot dans un tableau
        /// </summary>
        public static string[] RécupérationMotsFichier()
        {
            List<string> motsList = new List<string>();
            if (!File.Exists(ADRESSE_SAUVEGARDE + "MotsPossibles.txt"))  
            {
                Console.WriteLine("ERREUR : Un ou plusieurs fichiers de sauvegardes ne sont pas présent à l'adresse de sauvegarde indiquée.");
            }
            else
            {
                using (StreamReader sr = new StreamReader(ADRESSE_SAUVEGARDE + "MotsPossibles.txt"))
                {
                    string mot = string.Empty;
                    while (!sr.EndOfStream)
                    {
                        char charactère = (char)sr.Read();

                        
                        if (IsLetter(charactère))
                        {
                            mot += charactère;
                        }
                        
                        else if (!IsNullOrEmpty(mot))
                        {
                            motsList.Add(mot);
                            mot = string.Empty;
                        }
                    }
                    if (!IsNullOrEmpty(mot))
                    {
                        motsList.Add(mot);
                    }

                    sr.Close();
                }
            }
            return motsList.ToArray();
        }

        private static void InitDico()
        {
            dico = new Dictionnaire(RécupérationMotsFichier());
        }


        /// <summary>
        /// Retourne une matrice de tableau de char contenant des lettres
        /// </summary>
        private static int[,] RécupérationLettresFichier()
        {
            if (!File.Exists(ADRESSE_SAUVEGARDE + "Lettre.txt"))
            {
                Console.WriteLine("ERREUR : Un ou plusieurs fichiers de sauvegardes ne sont pas présent à l'adresse de sauvegarde indiquée.");
                
                return null;
            }

            int[,] chars = new int[NOMBRE_LETTRES, 2];
            StreamReader sr = new StreamReader(ADRESSE_SAUVEGARDE + "Lettre.txt");

            //On met tous les charactères dans une liste
            List<char> charactères = sr.ReadToEnd().ToCharArray().ToList();

            //On check si il y a suffisemment de charactères par rapport à ce que le jeu attend
            if (charactères.Count < NOMBRE_LETTRES * 3)
            {
                Console.WriteLine("Il y a un problème avec le fichier des lettres. Pas assez de charactères par rapport" +
                    "à ce qui est attendu dans le jeu. \nLe programme va s'arrêter\n");
                
                sr.Close();

                return null;
            }

            int currentLetter = -1;

            string amountCurrentLetter = "";
            bool b_amountCurrentLetter = false;

            string poidsCurrentLetter = "";
            bool b_poidsCurrentLetter = false;


            //Tout va bien, on peut commencer à remplir la matrice
            for (int i = 0; i < charactères.Count; i++)
            {
                if (IsLetter(charactères[i]))
                {
                    currentLetter = charactères[i].ToString().ToUpper().FirstOrDefault() - 'A';
                }
                else if (char.IsDigit(charactères[i]))
                {
                    if (amountCurrentLetter == "" || b_amountCurrentLetter)
                    {
                        b_amountCurrentLetter = true;
                        amountCurrentLetter += charactères[i].ToString();
                    }
                    else if (poidsCurrentLetter == "" || b_poidsCurrentLetter)
                    {
                        poidsCurrentLetter += charactères[i].ToString();
                        b_poidsCurrentLetter = true;
                    }

                    if (i == charactères.Count - 1 && b_poidsCurrentLetter)
                    {
                        chars[currentLetter, 1] = Convert.ToInt32(poidsCurrentLetter);
                    }
                }
                else //séparateur ','
                {
                    if (b_amountCurrentLetter)
                    {
                        chars[currentLetter, 0] = Convert.ToInt32(amountCurrentLetter);
                        b_amountCurrentLetter = false;
                    }
                    if (b_poidsCurrentLetter)
                    {
                        chars[currentLetter, 1] = Convert.ToInt32(poidsCurrentLetter);

                        currentLetter = -1;

                        amountCurrentLetter = "";
                        b_amountCurrentLetter = false;

                        poidsCurrentLetter = "";
                        b_poidsCurrentLetter = false;

                    }
                }
            }
            sr.Close();

            if (joker)
            {
                int[,] jokerMat = new int[chars.GetLength(0) + 1, 2];
                for (int i = 0; i < jokerMat.GetLength(0) - 1; i++)
                {
                    for (int j = 0; j < jokerMat.GetLength(1); j++)
                    {
                        jokerMat[i, j] = chars[i, j];
                    }
                }
                jokerMat[jokerMat.GetLength(0) - 1, 0] = 2;

                return jokerMat;
            }


            return chars;
        }

        private static void InitLettres()
        {
            lettres = new Lettres(RécupérationLettresFichier());
        }


        private static void InitJoueurs(int nbr)
        {
            joueurs = new List<Joueur>();

            for (int i = 0; i < nbr; i++)
            {
                joueurs.Add(new Joueur("Joueur " + i));
                joueurs[i].Add_Lettres(NOMBRE_LETTRES_PIOCHE_DEPART, lettres, new Random());
            }
        }



        

        #region Autres fonctions
        
        /// <summary>
        /// Renvoie <see langword="true"/> si le <see cref="char"/> passé en paramètre est une lettre de l'alphabet français (hors lettres spéciales). Renvoie <see langword="false"/> sinon
        /// </summary>
        public static bool IsLetter(char c)
        {
            return char.ToUpper(c) >= 'A' && char.ToUpper(c) <= 'Z';
        }


        /// <summary>
        /// Renvoie <see langword="true"/> si le <see cref="string"/> passé en paramètre est vide ou <see langword="null"/>. Renvoie <see langword="false"/> sinon
        /// </summary>
        public static bool IsNullOrEmpty(string s)
        {
            return s == null || s.Length <= 0;
        }


        /// <summary>
        /// Renvoie <see langword="true"/> si le <see cref="string"/> passé en paramètre est, <see langword="null"/> ou constitué uniquement d'espace blanc. Renvoie <see langword="false"/> sinon
        /// </summary>
        public static bool IsNullOrWhiteSpace(string s)
        {
            for (int index = 0; index < (s?.Length ?? 0); ++index)
            {
                if (!IsWhiteSpace(s[index]))
                    return false;
            }
            return true;
        }


        /// <summary>
        /// Renvoie <see langword="true"/> si le <see cref="char"/> passé en paramètre est un espace blanc. Renvoie <see langword="false"/> sinon
        /// </summary>
        private static bool IsWhiteSpace(char c)
        {
            return c == ' ';
        }

        #endregion

    }
}
