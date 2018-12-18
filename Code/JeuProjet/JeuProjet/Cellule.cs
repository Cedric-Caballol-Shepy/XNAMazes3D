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
    public enum Direction { NORD, SUD, EST, OUEST, INVALIDE }
    public class Cellule
    {
        public Dictionary<Direction,Mur> murs; //tableau associatif : clé = Direction, valeur = Mur
        public List<Tuple<Cellule, Direction>> voisins; //cellules voisines
        public List<Tuple<Cellule, Direction>> adjacents; //cellules vers lesquelles on peut naviguer
        public bool visited { get; set; }
        
        public Cellule()
        {
            this.murs = new Dictionary<Direction,Mur>();
            this.murs.Add(Direction.NORD, new Mur());
            this.murs.Add(Direction.SUD, new Mur());
            this.murs.Add(Direction.EST, new Mur());
            this.murs.Add(Direction.OUEST, new Mur());
            voisins = new List<Tuple<Cellule, Direction>>();
            adjacents = new List<Tuple<Cellule, Direction>>();
        }

        public void addVoisin(Tuple<Cellule, Direction> vois){
            this.voisins.Add(vois);
        }
        public void removeVoisin(Tuple<Cellule, Direction> vois)
        {
            this.voisins.Remove(vois);
        }

        public void addAdjacent(Tuple<Cellule, Direction> adj)
        {
            this.adjacents.Add(adj);
        }
        public void removeAdjacent(Tuple<Cellule, Direction> adj)
        {
            this.adjacents.Remove(adj);
        }
    }
}
