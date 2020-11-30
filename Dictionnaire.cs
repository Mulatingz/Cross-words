using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIXMO_BR
{ 
 
    public class Dictionnaire
    {
        private string[][] Dico;

        public Dictionnaire(string[] mots)
        {
            TabString(mots);
            TriAlphabétique();
        }


        /// <summary>
        /// Initialisation du tab de string en fonctions de leur tailles
        /// </summary>
        private void TabString(string[] mots)
        {
            List<int> tailles = new List<int>();
            for (int i = 0; i < mots.Length; i++)
            {
                if (!tailles.Contains(mots[i].Length))
                    tailles.Add(mots[i].Length);
            }

            this.Dico = new string[tailles.Max(x => x) + 1][];


            for (int i = 0; i < tailles.Count; i++)
            {
                this.Dico[tailles[i]] = new string[GetNumberElementBySize(mots, tailles[i])];
            }

            for (int i = 0; i < this.Dico.Length; i++) 
            {
                if (this.Dico[i] == null)
                {
                    this.Dico[i] = new string[0];
                    tailles.Insert(i, 0);  
                }
            }

            int indexDico = 0;
            for (int i = 0; i < this.Dico.Length; i++)   
            {
                for (int j = 0; j < this.Dico[i].Length; j++)
                {
                    if (mots[indexDico].Length == tailles[i])
                        this.Dico[i][j] = mots[indexDico];
                    indexDico++;
                }
            }
        }

        /// <summary>
        /// Nous permet de trouver chaque mot d'une longueur donnée
        /// </summary>
        private int GetNumberElementBySize(string[] mots, int taille)
        {
            int index = 0;
            for (int i = 0; i < mots.Length; i++)
            {
                if (mots[i].Length == taille)
                    index++;
            }
            return index;
        }

        /// <summary>
        /// Nous permet de trier alphabetiquement chaque mots de notre tableau
        /// </summary>
        private void TriAlphabétique()
        {
            for (int i = 0; i < this.Dico.Length; i++)
            {
                this.Dico[i] = Fiss(this.Dico[i]);
            }
        }

        private static string[] Fiss(string[] tab)
        {
            if (tab.Length <= 1)
            {
                return tab;
            }

            int mid = tab.Length / 2;
            string[] ta = new string[mid], bleau = new string[tab.Length - mid];

            for (int i = 0; i < mid; i++)
                ta[i] = tab[i];

            for (int i = mid; i < tab.Length; i++)
                bleau[i - mid] = tab[i];

            return Fuse(Fiss(ta), Fiss(bleau));
        }

        private static string[] Fuse(string[] tab1, string[] tab2)
        {
            string[] fusionTab = new string[tab1.Length + tab2.Length];
            int i = 0, j = 0;
            while (i < tab1.Length && j < tab2.Length)
            {
                if (Comp(tab1[i], tab2[j]))
                {
                    fusionTab[i + j] = tab1[i++];
                }
                else
                {
                    fusionTab[i + j] = tab2[j++];
                }
            }

            while (j < tab2.Length) { fusionTab[j + i] = tab2[j++]; }

            while (i < tab1.Length) { fusionTab[j + i] = tab2[i++]; }

                return fusionTab;
        }

        /// <summary>
        /// Nous permet de comparer alphabetiquement deux char
        /// </summary>
        private static bool Comp(string s1, string s2)
        {
            int oui = s1.Length < s2.Length ? s1.Length : s2.Length;
            for (int i = 0; i < oui; i++)
            {
                if (s1[i] < s2[i])
                {
                    return true;
                }
                if (s1[i] > s2[i])
                {
                    return false;
                }
            }
            return oui == s1.Length;

        }

        
        /// <summary>
        /// test si on a un mot a vraiment un mot avant d'appeler la fonction de recherche dichotomique
        /// </summary>
        public bool RechercheDicoRécursif(string mot)
        {
            if (mot != null && mot.Length < this.Dico.Length)  
            {
                return RechercheDicoRécursif(0, this.Dico[mot.Length].Length, mot.ToUpper(), 0);
            }

            return false;
        }
        
        private bool RechercheDicoRécursif(int début, int fin, string mot, int index)
        {
            if (index == mot.Length)
                return true;

            
            bool Test = false;
            for (int i = début; i < fin; i++)
            {
                
                if (!Test && this.Dico[mot.Length][i][index] == mot[index])
                {
                    début = i;
                    Test = true;  
                }
                else if (Test && this.Dico[mot.Length][i][index] != mot[index])
                {
                    fin = i;
                    break;   
                }
            }

            if (Test)
            {
                return RechercheDicoRécursif(début, fin, mot, index + 1);
            }

            return false;
        }
    }
}
