using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace JeuProjet
{
    public class Labyrinthe
    {  
        public int tailleL; //nombre de lignes
        public int tailleC; //nombre de colonnes
        public Dictionary<Direction, Direction> opposes; //on stocke les opposés de chaque direction
        public Cellule[,] maze;

        /// <summary>
        /// constructeur de Labyrinthe défini par sa taille et la chance d'avoir des portes
        /// </summary>
        /// <param name="tailleL"></param>
        /// <param name="tailleC"></param>
        /// <param name="nbPortes">Définit le nombre de portes présentes dans le niveau</param>
        /// <param name="nbChoixConv">Définit la difficulté des portes en donnant le nombre de types de conversions possibles (maximum 7)</param>
        public Labyrinthe(int tailleL, int tailleC, int nbPortes, int nbChoixConv)
        {
            this.tailleL = tailleL;
            this.tailleC = tailleC;
            this.maze = new Cellule[tailleL, tailleC];
            this.opposes = new Dictionary<Direction,Direction>();
            this.opposes[Direction.NORD] = Direction.SUD;
            this.opposes[Direction.SUD] = Direction.NORD;
            this.opposes[Direction.EST] = Direction.OUEST;
            this.opposes[Direction.OUEST] = Direction.EST;
            initMaze();
            initVoisins();
            creuserChemins(0, 0);
            ajoutePortes(nbPortes, nbChoixConv);
        }

        /// <summary>
        /// créé une grille avec murs pleins
        /// </summary>
        private void initMaze()
        {
            for (int x = 0; x < tailleL; x++)
            {
                for (int y = 0; y < tailleC; y++)
                {
                    maze[x, y] = new Cellule();
                }
            }
        }

        /// <summary>
        /// ajoute les cellules voisines dans chaque liste des cellules
        /// </summary>
        private void initVoisins()
        {
            for (int l = 0; l < tailleL; l++) //ligne
            {
                for (int c = 0; c < tailleC; c++) //colonne
                {
                    if (l == 0 && c == 0) //0,0 coin superieur gauche
                    {
                        this.maze[l, c].addVoisin(new Tuple<Cellule, Direction>(this.maze[l, c + 1], Direction.EST));
                        this.maze[l, c].addVoisin(new Tuple<Cellule, Direction>(this.maze[l + 1, c], Direction.SUD));
                    }
                    else if (l == 0 && c == tailleC - 1) //0,MAX coin superieur droit
                    {
                        this.maze[l, c].addVoisin(new Tuple<Cellule, Direction>(this.maze[l + 1, c], Direction.SUD));
                        this.maze[l, c].addVoisin(new Tuple<Cellule, Direction>(this.maze[l, c - 1], Direction.OUEST));
                    }
                    else if (l == tailleL - 1 && c == 0) //MAX,0 coin inferieur gauche
                    {
                        this.maze[l, c].addVoisin(new Tuple<Cellule, Direction>(this.maze[l - 1, c], Direction.NORD));
                        this.maze[l, c].addVoisin(new Tuple<Cellule, Direction>(this.maze[l, c + 1], Direction.EST));
                    }
                    else if (l == tailleL - 1 && c == tailleC - 1) //MAX,MAX coin inferieur droit
                    {
                        this.maze[l, c].addVoisin(new Tuple<Cellule, Direction>(this.maze[l - 1, c], Direction.NORD));
                        this.maze[l, c].addVoisin(new Tuple<Cellule, Direction>(this.maze[l, c - 1], Direction.OUEST));
                    }
                    else if (l == 0 && c > 0 && c < tailleC - 1) //0,y ligne superieure
                    {
                        this.maze[l, c].addVoisin(new Tuple<Cellule, Direction>(this.maze[l + 1, c], Direction.SUD));
                        this.maze[l, c].addVoisin(new Tuple<Cellule, Direction>(this.maze[l, c + 1], Direction.EST));
                        this.maze[l, c].addVoisin(new Tuple<Cellule, Direction>(this.maze[l, c - 1], Direction.OUEST));
                    }
                    else if ((l > 0 && l < tailleL - 1) && c == 0) //x,0 colonne gauche
                    {
                        this.maze[l, c].addVoisin(new Tuple<Cellule, Direction>(this.maze[l - 1, c], Direction.NORD));
                        this.maze[l, c].addVoisin(new Tuple<Cellule, Direction>(this.maze[l, c + 1], Direction.EST));
                        this.maze[l, c].addVoisin(new Tuple<Cellule, Direction>(this.maze[l + 1, c], Direction.SUD));
                    }
                    else if((l > 0 && l < tailleL - 1) && c == tailleC -1 ){ //(x,MAX) colonne droite
                        this.maze[l, c].addVoisin(new Tuple<Cellule, Direction>(this.maze[l - 1, c], Direction.NORD));
                        this.maze[l, c].addVoisin(new Tuple<Cellule, Direction>(this.maze[l, c - 1], Direction.OUEST));
                        this.maze[l, c].addVoisin(new Tuple<Cellule, Direction>(this.maze[l + 1, c], Direction.SUD));
                    }
                    else if (l == tailleL - 1 && (c > 0 && c < tailleC - 1)) //(MAX,y) ligne inferieure
                    {
                        this.maze[l, c].addVoisin(new Tuple<Cellule, Direction>(this.maze[l - 1, c], Direction.NORD));
                        this.maze[l, c].addVoisin(new Tuple<Cellule, Direction>(this.maze[l, c - 1], Direction.OUEST));
                        this.maze[l, c].addVoisin(new Tuple<Cellule, Direction>(this.maze[l, c + 1], Direction.EST));
                    }
                    else if ((l > 0 && l < tailleL - 1) && (c > 0 && c < tailleC - 1)) //x,y != (0 && MAX) cas général
                    {
                        this.maze[l, c].addVoisin(new Tuple<Cellule, Direction>(this.maze[l - 1, c], Direction.NORD));
                        this.maze[l, c].addVoisin(new Tuple<Cellule, Direction>(this.maze[l + 1, c], Direction.SUD));
                        this.maze[l, c].addVoisin(new Tuple<Cellule, Direction>(this.maze[l, c + 1], Direction.EST));
                        this.maze[l, c].addVoisin(new Tuple<Cellule, Direction>(this.maze[l, c - 1], Direction.OUEST));
                    }
                }
            }
        }

        /// <summary>
        /// Recursive backtracking pour créer le labyrinthe
        /// Methode inspirée de http://weblog.jamisbuck.org/2010/12/27/maze-generation-recursive-backtracking
        /// </summary>
        /// <param name="posCouranteL"></param>
        /// <param name="posCouranteC"></param>
        private void creuserChemins(int posCouranteL, int posCouranteC)
        {
            Cellule c = this.maze[posCouranteL, posCouranteC];
            c.visited = true;
            List<Direction> directionsValides = initDirectionsValides(c);
            Direction d;
            //foreach (Direction d in directionsValides)
            while(directionsValides.Count > 0)
            {
                d = Direction.INVALIDE;
                directionsValides = initDirectionsValides(c); //on actualise les directions valides
                if (directionsValides.Count > 1) //on va vers un des voisins valide pris aléatoirement
                    d = directionsValides.ElementAt(Game1.r.Next(directionsValides.Count));
                else if (directionsValides.Count == 1)
                    d = directionsValides.ElementAt(0);
                foreach (var t in c.voisins) //on cherche la cellule voisine à c correspondant à d
                {
                    if (t.Item2.Equals(d))
                    {
                        if(d.Equals(Direction.NORD))
                        {
                            posCouranteL--;
                        }
                        else if(d.Equals(Direction.SUD))
                        {
                            posCouranteL++;
                        }
                        else if (d.Equals(Direction.EST))
                        {
                            posCouranteC++;
                        }
                        else if (d.Equals(Direction.OUEST))
                        {
                            posCouranteC--;
                        }
                        ouvrirMur(c, d);
                        creuserChemins(posCouranteL,posCouranteC);
                    }
                }
            }
        }

        /// <summary>
        /// renvoie une liste des directions valides pour creuserChemins
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private List<Direction> initDirectionsValides (Cellule c){
            List<Direction> directionsValides = new List<Direction>();
            foreach (var voisin in c.voisins)
            {
                if (!voisin.Item1.visited)
                    directionsValides.Add(voisin.Item2);
            }
            directionsValides.shuffle();
            return directionsValides;
        }

        /// <summary>
        /// ouvre le mur de la cellule donnée puis de sa cellule voisine / peut être utilisé pour ouvrir une porte
        /// </summary>
        /// <param name="c"></param>
        /// <param name="d"></param>
        private void ouvrirMur(Cellule c, Direction d)
        {
            c.murs[d].ouvrir(); //on ouvre le mur de la cellule c
            foreach (var t in c.voisins) //on cherche la cellule voisine à c correspondant à d
            {
                if (t.Item2.Equals(d))
                {
                    t.Item1.murs[opposes[d]].ouvrir(); //on ouvre le mur à l'opposé de d dans la cellule voisine t.Item1
                    c.addAdjacent(t); //on l'ajoute aux adjacents pour dire qu'on peut y aller physiquement depuis c
                    Tuple<Cellule, Direction> cd = new Tuple<Cellule,Direction>(c, opposes[d]);
                    t.Item1.addAdjacent(cd); //et on ajoute c aux adjacents de t.Item1
                    break; //il ne peut y en avoir qu'un
                }
            }
        }

        /// <summary>
        /// ajoute au labyrinthe les portes ouvrables avec conversion
        /// </summary>
        /// <param name="chance"></param>
        private void ajoutePortes(int nbPortes, int nbChoixConv)
        {
            //gestion des erreurs
            if (nbChoixConv > 7)
                nbChoixConv = 7;
            if (nbChoixConv < 1)
                nbChoixConv = 1;
                        
            int rC,rL;
            List<Tuple<int, int>> couplesPris = new List<Tuple<int,int>>();
            while (nbPortes > 0)
            {
                bool dejaPris = false;
                do
                {
                    rL = Game1.r.Next(this.tailleL);
                    rC = Game1.r.Next(this.tailleC);
                    //test pour voir si on est pas déjà venu dans cette cellule avec ajoutePortes()
                    Tuple<int, int> couple = new Tuple<int, int>(rL, rC);
                    foreach (var v in couplesPris)
                    {
                        if (couple.Equals(v))
                        {
                            dejaPris = true;
                            break; //on sort du foreach pour re-faire un tour du while(dejaPris)
                        }
                        else
                        {
                            dejaPris = false;
                        }
                    }
                    if (!dejaPris)
                    {
                        couplesPris.Add(couple);
                    }
                } while (dejaPris);
                //on pose la porte
                foreach (var m in maze[rL, rC].murs) //on regarde chaque mur...
                {
                    if(m.Value.state.Equals(Etat.OUVERT)) //...pour arriver à un mur ouvert
                    {
                        m.Value.transformerEnPorte(nbChoixConv); //on transforme ce mur en porte
                        //maintenant il faut s'occuper du mur de la cellule adjacente
                        foreach (var adj in maze[rL,rC].adjacents) //on cherche dans les cellules adjacentes à maze[rC, rL]...
                        {
                            if (adj.Item2.Equals(m.Key)) //...celles correspondant à t.Key (qui est la Direction)
                            {
                                adj.Item1.murs[opposes[m.Key]].transformerEnPorte(nbChoixConv); //et on lui met une porte à la direction opposée de d (comme dans ouvrirMur())
                                maze[rL, rC].removeAdjacent(adj); //on l'enlève des adjacents car par défaut une porte est fermée (donc on ne peut pas passer à travers)
                                adj.Item1.removeAdjacent(new Tuple<Cellule, Direction>(maze[rL, rC], opposes[m.Key])); //et on enleve maze[rL,rC] des adjacents de adj
                                break; //il ne peut y en avoir qu'une
                            }
                        }
                        nbPortes--;
                        break;
                    }
                }
            }
        }
        

        
    }


    public static class Utilitaires
    {
        /// <summary>
        /// Réorganise aléatoirement une List (méthode trouvée sur internet)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            Random rnd = new Random();
            while (n > 1)
            {
                int k = (rnd.Next(0, n) % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}

