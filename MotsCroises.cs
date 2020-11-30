using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIXMO_BR
{
    class MotsCroises
    {
        public bool PlateauValide { get; private set; } = true;

        private int[,] grille;

        private bool firstMotPlacé = false;

        public int[,] GetGrille => this.grille;


       
        public MotsCroises(int height, int width)
        {
            this.grille = new int[width, height];

            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    this.grille[i, j] = -1;
        }


        /// <summary>
        /// Ajoute un mot à la grille au positionnement indiqué, renvoie un booléen indiquant si l'opération a pu être effectuée ou non.
        /// </summary>
        public bool TryAddMot(string mot, int X, int Y, bool Bas)
        {
            mot = mot.ToUpper();
            if (TestPlateau(mot, X, Y, Bas))
            {
                int start = Bas ? Y : X;

                for (int i = start; i < start + mot.Length; i++)
                {
                    if (Bas)
                    {
                        this.grille[X, i] = mot[i - start];
                    }
                    else
                    {
                        this.grille[i, Y] = mot[i - start];
                    }
                }

                this.firstMotPlacé = true;

                return true;
            }

            return false;
        }


        /// <summary>
        /// Test si un tableau de char est présent dans le plateau. L'ordre des char dans le tableau correspond 
        /// à l'ordre de recherche
        /// </summary>
        private bool TestPlateau(string mot, int posX, int posY, bool sensBas)
        {
            if (mot == null || mot?.Length == 0)
            {
                return false;
            }

            int incrX = !sensBas ? 1 : 0;
            int incrY = sensBas ? 1 : 0;

            int X = posX;
            int Y = posY;

            for (int i = 0; i < mot.Length; i++)
            {
                if (this.grille[X, Y] == mot[i] || !this.firstMotPlacé)
                {
                    if (RechercheAutourValeur(mot.ToUpper(), new int[2] { posX, posY }, 1, sensBas ? 1 : 0))
                    {
                        return true;
                    }
                }
                X += incrX;
                Y += incrY;
            }


            return false;
        }

        private bool RechercheAutourValeur(string mot, int[] positionLettre, int indexCharMot, int sens)
        {

            if (mot.Length == indexCharMot)
            {
                return true;
            }

            positionLettre[sens]++;

            if (positionLettre[0] >= this.grille.GetLength(0) || positionLettre[1] >= this.grille.GetLength(1))
            {
                return false;
            }


            if (this.grille[positionLettre[0], positionLettre[1]] == mot[indexCharMot] || this.grille[positionLettre[0], positionLettre[1]] == -1)
            {
                return RechercheAutourValeur(mot, positionLettre, indexCharMot + 1, sens);
            }

            return false;
        }




        public override string ToString()
        {
            string s = "    |";
            for (int i = 0; i < this.grille.GetLength(0); i++)
            {
                s += $"  {i + 1} {(i + 1 < 10 ? " " : "")}|";
            }
            s += "\n";
            for (int i = 0; i <= this.grille.GetLength(0); i++)
            {
                s += new string('\u2015', 6);
            }
            s += "\n";
            for (int i = 0; i < this.grille.GetLength(1); i++)
            {
                int val = i + 1;

                s += " " + val.ToString() + (val < 10 ? "  " : " ") + "|";
                for (int j = 0; j < this.grille.GetLength(0); j++)
                {
                    s += $"  { (this.grille[j, i] == -1 ? " " : ((char)(this.grille[j, i])).ToString())}  |";
                }
                s += "\n";
                for (int j = 0; j <= this.grille.GetLength(0); j++)
                {
                    s += new string('\u2015', 6);
                }
                s += "\n";
            }

            return s;
        }
    }
}
