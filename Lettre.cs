using System;
using System.Collections.Generic;
using System.Text;

namespace MIXMO_BR
{
    class Lettre
    {
        private char lettre;

        private int poids;

        public char Charactère => this.lettre;

        public int Poids => this.poids;

        public bool Joker
        { get; private set;} = false;

        public Lettre(char lettre, int poids)
        {
            this.lettre = lettre;
            this.poids = poids;

            if (lettre == '*')
            {
                this.Joker = true;
            }
        }

        public override string ToString()
        {
            return $"Lettre : {this.lettre.ToString()} | Poids : {this.poids}";
        }
    }
}
