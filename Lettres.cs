using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MIXMO_BR
{
    internal class Lettres
    {
        private List<Lettre> listeLettres;


        public Lettre this[int index]
        {
            get => this.listeLettres[index];
        }

        public int Count
        {
            get => this.listeLettres.Count;
        }



        /// <summary>
        /// Initialise une nouvelle instance de la class lettre, vide.
        /// </summary>
        public Lettres()
        {
            this.listeLettres = new List<Lettre>();
        }

        /// <summary>
        /// Initialise une nouvelle instance de la classe lettre à partir d'un tableau 32 bits à 2 dimensions
        /// </summary>
        public Lettres(int[,] données)
            : this()
        {
            if (données.GetLength(0) == 26)
            {
                for (int i = 0; i < données.GetLength(0); i++)
                {
                    for (int j = 0; j < données[i, 0]; j++)
                    {
                        this.listeLettres.Add(new Lettre((char)(i + 'A'), données[i, 1]));
                    }
                }
            }
            else if (données.GetLength(0) == 27)
            {
                for (int i = 0; i < données.GetLength(0) - 1; i++)
                {
                    for (int j = 0; j < données[i, 0]; j++)
                    {
                        this.listeLettres.Add(new Lettre((char)(i + 'A'), données[i, 1]));
                    }
                }

                for (int j = 0; j < données[données.GetLength(0) - 1, 0]; j++)
                {
                    this.listeLettres.Add(new Lettre('*', données[données.GetLength(0) - 1, 1]));
                }

            }
        }




        /// <summary>
        /// Ajoute une lettre à la collection.
        /// </summary>
        public void Add(Lettre lettre)
        {
            this.listeLettres.Add(lettre);
        }

        /// <summary>
        /// Enlève la première occurence de la lettre associé à ce char dans cette collection
        /// </summary>
        public bool Remove(char lettre)
        {
            return this.listeLettres.Remove(this.listeLettres.Where(l => l.Charactère == lettre).FirstOrDefault());
        }


        public bool Contains(char c)
        {
            return Contains_Occurence(c) > 0;
        }

        private int Contains_Occurence(char c)
        {
            int occurence = 0;

            foreach (Lettre l in this.listeLettres)
            {
                if (l.Charactère == c.ToString().ToUpper().FirstOrDefault())
                {
                    occurence++;
                }
            }

            return occurence;
        }

        public bool Contains(string mot)
        {
            if (mot == null)
                return false;

            int[] occurencesLettreInMot = new int[Program.NOMBRE_LETTRES];

            mot = mot.ToUpper();


            foreach (char c in mot)
            {
                if (Program.IsLetter(c))
                {
                    occurencesLettreInMot[c - 'A']++;
                }
                else if (c == '*' )
                {
                    occurencesLettreInMot[c - 'A']++;
                }
                else
                {
                    return false;
                }
            }

            for (int i = 0; i < occurencesLettreInMot.Length; i++)
            {
                if (occurencesLettreInMot[i] > Contains_Occurence((char)('A' + i)))
                    return false;
            }

            return true;
        }

        public string MotAvecLettreDispo(string mot)
        {
            int[] occurencesLettreInList = new int[Program.NOMBRE_LETTRES];

            mot = mot.ToUpper();

            foreach (var l in this.listeLettres)
            {
                occurencesLettreInList[l.Charactère - 'A']++;
            }

            string realMot = "";

            foreach (char c in mot)
            {
                if (--occurencesLettreInList[c - 'A'] >= 0)
                {
                    realMot += c;
                }
            }

            return realMot;
        }


        public override string ToString()
        {
            string s = "";
            this.listeLettres.ForEach(l => s += $" {l.Charactère.ToString()}  ");
            return s;
        }
    }
}