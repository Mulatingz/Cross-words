using System;
using System.Collections.Generic;
using System.Text;

namespace MIXMO_BR
{
    internal class Joueur
    {
        public string Nom
        { get; private set; }

        public int Score
        { get; private set; } = 0;

        private List<string> motsTrouvés;

        private MotsCroises grille;

        private Lettres pioche;

        private Lettres histo_Pioche;

        public Lettres Pioche => this.pioche;

        public Lettres c => this.histo_Pioche;

        public MotsCroises Grille => this.grille;

        public bool JoueurValide
        { get; private set; } = true;


        /// <summary>
        /// Initialise une nouvelle instance de la class joueur à partir du nom de ce joueur
        /// </summary>
        public Joueur(string Nom)
        {
            if (Program.IsNullOrEmpty(Nom))
            {
                this.JoueurValide = false;
                Console.WriteLine("ERREUR .  Le nom du joueur n'est pas valide.");
                return;
            }

            this.Nom = Nom;

            this.motsTrouvés = new List<string>();

            this.pioche = new Lettres();

            this.histo_Pioche = new Lettres();

            this.grille = new MotsCroises(Program.TAILLE_GRILLE, Program.TAILLE_GRILLE);
        }



        /// <summary>
        /// Retourne false si le string passé en paramètre est présent dans la liste de mots trouvés
        /// </summary>
        public bool UniqueWord(string mot)
        {
            foreach (var motDico in this.motsTrouvés)
            {
                if (mot == motDico)
                    return false;
            }
            return true;
        }
        
        /// <summary>
        /// Ajoute un string à la liste de mots trouvés par ce joueur
        /// </summary>
        public void Add_Mot(string mot)
        {
            this.motsTrouvés.Add(mot);
        }


        public bool Add_Lettres(int nb, Lettres _pioche, Random r)
        {
            if (nb > _pioche.Count)
                return false;

            for (int i = 0; i < nb; i++)
            {
                this.pioche.Add(_pioche[r.Next(_pioche.Count)]);

                _pioche.Remove(this.pioche[this.pioche.Count - 1].Charactère);
            }

            return true;
        }

        public void OteLettre(string mot)
        {
            mot = mot.ToUpper();

            foreach (char c in mot)
            {
                if (this.pioche.Contains(c))
                {
                    this.pioche.Remove(c);

                    
                }
            }
        }

        public bool ContainsMot(string mot)
        {
            return this.pioche.Contains(mot);
        }

        /// <summary>
        /// Renvoie les lettres utilisées pour le mot en supprimant toutes celles utilisées qui sont déjà sur le plateau.
        /// </summary>
        public string EliminateGridLetters(string mot, int posX, int posY, bool down)
        {
            int start = down ? posY : posX;

            string realMot = "";

            int indexMot = 0;
            int amountGrilleUsedWords = 0;

            for (int i = start; i < this.grille.GetGrille.GetLength(down ? 1 : 0) && indexMot < mot.Length; i++)
            {
                if (down)
                {
                    if (this.grille.GetGrille[posX, i] != -1)
                    {
                        amountGrilleUsedWords++;
                        indexMot++;
                    }
                    else
                    {
                        realMot += mot[indexMot++];
                    }
                }
                else
                {
                    if (this.grille.GetGrille[i, posY] != -1)
                    {
                        amountGrilleUsedWords++;
                        indexMot++;
                    }
                    else
                    {
                        realMot += mot[indexMot++];
                    }
                }
            }

            if (realMot.Length != mot.Length - amountGrilleUsedWords)
            {
                return null;
            }

            return realMot;
        }

        /// <summary>
        /// Permet de calculer le score du joueur
        /// </summary>
        public int AddScore()
        {
            int score = 0;

            for (int i = 0; i <this.motsTrouvés.Count; i++)
            {
                string a = motsTrouvés[i];

                if (a.Length >= 5)
                {
                    score += a.Length;
                }

                for (int j = 0; j < a.Length; j++)
                {
                    if (a[j] == 'k'| a[j] == 'w' | a[j] == 'x' | a[j] == 'y' | a[j] == 'z')
                    {
                        score += 5;
                    }
                }
            }



            return score;
        }


        public override string ToString()
        {
            return $@"| "" {this.Nom} ""  ||  Score : {this.Score}  ||  Mots trouvés :  {string.Join(" | ", this.motsTrouvés)} ";
        }
    }
}
