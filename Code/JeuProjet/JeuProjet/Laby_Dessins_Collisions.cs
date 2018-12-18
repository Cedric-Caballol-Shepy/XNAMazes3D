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
    public class Laby_Dessins_Collisions 
    {
        public int tailleL; //nombre de lignes
        public int tailleC; //nombre de colonnes

        public Labyrinthe laby;

        //variables partie dessin//
        private Game1 game;
        private GraphicsDevice device;
        private VertexBuffer bufferMurs;
        private VertexBuffer bufferPortes;
        private Vector3[] pointsMurs;
        private Texture2D[] textures;
        private int tailleMurs;
        //                       //

        public Laby_Dessins_Collisions(int tailleL, int tailleC, int nbPortes, int nbChoixConv, Game1 game, int tailleMurs)
        {
            this.laby = new Labyrinthe(tailleL, tailleC, nbPortes, nbChoixConv);
            this.tailleL = tailleL;
            this.tailleC = tailleC;
            // dessin :                         //
            this.game = game;
            this.device = this.game.GraphicsDevice;
            pointsMurs = new Vector3[8];
            textures = new Texture2D[2] { 
                this.game.Content.Load<Texture2D>("texture_mur"), 
                this.game.Content.Load<Texture2D>("text_door") 
            };
            this.tailleMurs = tailleMurs;
            pointsMurs[0] = new Vector3(0, this.tailleMurs, 0);
            pointsMurs[1] = new Vector3(0, this.tailleMurs, this.tailleMurs);
            pointsMurs[2] = new Vector3(0, 0, 0);
            pointsMurs[3] = new Vector3(0, 0, this.tailleMurs);
            pointsMurs[4] = new Vector3(this.tailleMurs, this.tailleMurs, 0);
            pointsMurs[5] = new Vector3(this.tailleMurs, this.tailleMurs, this.tailleMurs);
            pointsMurs[6] = new Vector3(this.tailleMurs, 0, 0);
            pointsMurs[7] = new Vector3(this.tailleMurs, 0, this.tailleMurs);
            dessin_ConstrBuffersMursPortes();
            //                                  //
        }


        // partie dessin                    //
        // Aide pour dessin et collisions : https://the-eye.eu/public/Books/IT%20Various/xna_4_3d_game_development_by_example.pdf
        private void dessin_ConstrBuffersMursPortes()
        {
            List<VertexPositionTexture> mursVertex = new List<VertexPositionTexture>();
            List<VertexPositionTexture> portesVertex = new List<VertexPositionTexture>();
            for (int x = 0; x < tailleL; x++)
            {
                for (int z = 0; z < tailleC; z++)
                {
                    foreach (Tuple<Etat, VertexPositionTexture> vertex in dessin_getMursConstr(x, z))
                    {
                        if (vertex.Item1.Equals(Etat.FERME)) //mur
                            mursVertex.Add(vertex.Item2);
                        else //porte
                            portesVertex.Add(vertex.Item2);
                    }
                }
            }

            this.bufferMurs = new VertexBuffer(device, VertexPositionTexture.VertexDeclaration, mursVertex.Count, BufferUsage.WriteOnly);
            this.bufferMurs.SetData<VertexPositionTexture>(mursVertex.ToArray());

            if (portesVertex.Count != 0)
            {
                this.bufferPortes = new VertexBuffer(device, VertexPositionTexture.VertexDeclaration, portesVertex.Count, BufferUsage.WriteOnly);
                this.bufferPortes.SetData<VertexPositionTexture>(portesVertex.ToArray());
            }
            else
                this.bufferPortes = null; ////null si il n'y a pas de porte


        }

        private List<Tuple<Etat, VertexPositionTexture>> dessin_getMursConstr(int x, int z)
        {
            List<Tuple<Etat, VertexPositionTexture>> triangles = new List<Tuple<Etat, VertexPositionTexture>>();
            Vector2 hg, hd, bg, bd; //sommets des faces 
            hg = new Vector2(0, 0);
            hd = new Vector2(1, 0);
            bg = new Vector2(0, 1);
            bd = new Vector2(1, 1);
            if (this.laby.maze[x, z].murs[Direction.OUEST].state.Equals(Etat.FERME) || this.laby.maze[x, z].murs[Direction.OUEST].state.Equals(Etat.PORTE))
            {
                triangles.Add(dessin_CalculePoint(0, x * this.tailleMurs, z * this.tailleMurs, hg, (this.laby.maze[x, z].murs[Direction.OUEST].state)));
                triangles.Add(dessin_CalculePoint(4, x * this.tailleMurs, z * this.tailleMurs, hd, (this.laby.maze[x, z].murs[Direction.OUEST].state)));
                triangles.Add(dessin_CalculePoint(2, x * this.tailleMurs, z * this.tailleMurs, bg, (this.laby.maze[x, z].murs[Direction.OUEST].state)));
                triangles.Add(dessin_CalculePoint(4, x * this.tailleMurs, z * this.tailleMurs, hd, (this.laby.maze[x, z].murs[Direction.OUEST].state)));
                triangles.Add(dessin_CalculePoint(6, x * this.tailleMurs, z * this.tailleMurs, bd, (this.laby.maze[x, z].murs[Direction.OUEST].state)));
                triangles.Add(dessin_CalculePoint(2, x * this.tailleMurs, z * this.tailleMurs, bg, (this.laby.maze[x, z].murs[Direction.OUEST].state)));
            }
            if (this.laby.maze[x, z].murs[Direction.SUD].state.Equals(Etat.FERME) || this.laby.maze[x, z].murs[Direction.SUD].state.Equals(Etat.PORTE))
            {
                triangles.Add(dessin_CalculePoint(4, x * this.tailleMurs, z * this.tailleMurs, hg, (this.laby.maze[x, z].murs[Direction.SUD].state)));
                triangles.Add(dessin_CalculePoint(5, x * this.tailleMurs, z * this.tailleMurs, hd, (this.laby.maze[x, z].murs[Direction.SUD].state)));
                triangles.Add(dessin_CalculePoint(6, x * this.tailleMurs, z * this.tailleMurs, bg, (this.laby.maze[x, z].murs[Direction.SUD].state)));
                triangles.Add(dessin_CalculePoint(5, x * this.tailleMurs, z * this.tailleMurs, hd, (this.laby.maze[x, z].murs[Direction.SUD].state)));
                triangles.Add(dessin_CalculePoint(7, x * this.tailleMurs, z * this.tailleMurs, bd, (this.laby.maze[x, z].murs[Direction.SUD].state)));
                triangles.Add(dessin_CalculePoint(6, x * this.tailleMurs, z * this.tailleMurs, bg, (this.laby.maze[x, z].murs[Direction.SUD].state)));
            }
            if (this.laby.maze[x, z].murs[Direction.EST].state.Equals(Etat.FERME) || this.laby.maze[x, z].murs[Direction.EST].state.Equals(Etat.PORTE))
            {
                triangles.Add(dessin_CalculePoint(5, x * this.tailleMurs, z * this.tailleMurs, hg, (this.laby.maze[x, z].murs[Direction.EST].state)));
                triangles.Add(dessin_CalculePoint(1, x * this.tailleMurs, z * this.tailleMurs, hd, (this.laby.maze[x, z].murs[Direction.EST].state)));
                triangles.Add(dessin_CalculePoint(7, x * this.tailleMurs, z * this.tailleMurs, bg, (this.laby.maze[x, z].murs[Direction.EST].state)));
                triangles.Add(dessin_CalculePoint(1, x * this.tailleMurs, z * this.tailleMurs, hd, (this.laby.maze[x, z].murs[Direction.EST].state)));
                triangles.Add(dessin_CalculePoint(3, x * this.tailleMurs, z * this.tailleMurs, bd, (this.laby.maze[x, z].murs[Direction.EST].state)));
                triangles.Add(dessin_CalculePoint(7, x * this.tailleMurs, z * this.tailleMurs, bg, (this.laby.maze[x, z].murs[Direction.EST].state)));
            }
            if (this.laby.maze[x, z].murs[Direction.NORD].state.Equals(Etat.FERME) || this.laby.maze[x, z].murs[Direction.NORD].state.Equals(Etat.PORTE))
            {
                triangles.Add(dessin_CalculePoint(1, x * this.tailleMurs, z * this.tailleMurs, hg, (this.laby.maze[x, z].murs[Direction.NORD].state)));
                triangles.Add(dessin_CalculePoint(0, x * this.tailleMurs, z * this.tailleMurs, hd, (this.laby.maze[x, z].murs[Direction.NORD].state)));
                triangles.Add(dessin_CalculePoint(3, x * this.tailleMurs, z * this.tailleMurs, bg, (this.laby.maze[x, z].murs[Direction.NORD].state)));
                triangles.Add(dessin_CalculePoint(0, x * this.tailleMurs, z * this.tailleMurs, hd, (this.laby.maze[x, z].murs[Direction.NORD].state)));
                triangles.Add(dessin_CalculePoint(2, x * this.tailleMurs, z * this.tailleMurs, bd, (this.laby.maze[x, z].murs[Direction.NORD].state)));
                triangles.Add(dessin_CalculePoint(3, x * this.tailleMurs, z * this.tailleMurs, bg, (this.laby.maze[x, z].murs[Direction.NORD].state)));
            }
            return triangles;
        }

        private Tuple<Etat, VertexPositionTexture> dessin_CalculePoint(int pointMur, int xOffset, int zOffset, Vector2 sommet, Etat etat)
        {
            VertexPositionTexture v = new VertexPositionTexture(pointsMurs[pointMur] + new Vector3(xOffset, 0, zOffset), sommet);
            return new Tuple<Etat, VertexPositionTexture>(etat, v);
        }

        public void DrawFace(VertexBuffer buffer)
        {
            device.SetVertexBuffer(buffer);
            device.SamplerStates[0] = SamplerState.LinearClamp;
            device.DrawPrimitives(PrimitiveType.TriangleList, 0, buffer.VertexCount / 3);
        }

        public void Draw()
        {
            this.game.effect.VertexColorEnabled = false;
            this.game.effect.TextureEnabled = true;

            this.game.effect.Texture = textures[0]; //on draw les murs
            foreach (EffectPass pass in this.game.effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                DrawFace(this.bufferMurs);
            }

            if (this.bufferPortes != null)
            { //si il y a au moins une porte
                this.game.effect.Texture = textures[1]; //on draw les portes
                foreach (EffectPass pass in this.game.effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    DrawFace(this.bufferPortes);
                }
            }
        }
        //                                  //

        //gestion collisions                //

        private BoundingBox collisions_constrBoundingBox(int x, int z, int p1, int p2)
        {
            BoundingBox box = new BoundingBox(pointsMurs[p1], pointsMurs[p2]);
            box.Min.X += x;
            box.Min.Z += z;
            box.Max.X += x;
            box.Max.Z += z;
            box.Min.X -= 0.1f;
            box.Min.Z -= 0.1f;
            box.Max.X += 0.1f;
            box.Max.Z += 0.1f;
            return box;
        }

        public List<BoundingBox> collisions_getBorduresCellule(int x, int z)
        {
            List<BoundingBox> boxes = new List<BoundingBox>();
            if (this.laby.maze[x, z].murs[Direction.OUEST].state.Equals(Etat.FERME) || this.laby.maze[x, z].murs[Direction.OUEST].state.Equals(Etat.PORTE))
                boxes.Add(collisions_constrBoundingBox(x * this.tailleMurs, z * this.tailleMurs, 2, 4));
            if (this.laby.maze[x, z].murs[Direction.SUD].state.Equals(Etat.FERME) || this.laby.maze[x, z].murs[Direction.SUD].state.Equals(Etat.PORTE))
                boxes.Add(collisions_constrBoundingBox(x * this.tailleMurs, z * this.tailleMurs, 6, 5));
            if (this.laby.maze[x, z].murs[Direction.EST].state.Equals(Etat.FERME) || this.laby.maze[x, z].murs[Direction.EST].state.Equals(Etat.PORTE))
                boxes.Add(collisions_constrBoundingBox(x * this.tailleMurs, z * this.tailleMurs, 3, 5));
            if (this.laby.maze[x, z].murs[Direction.NORD].state.Equals(Etat.FERME) || this.laby.maze[x, z].murs[Direction.NORD].state.Equals(Etat.PORTE))
                boxes.Add(collisions_constrBoundingBox(x * this.tailleMurs, z * this.tailleMurs, 2, 1));
            return boxes;
        }
        //                                  //

        // gestion ouverture portes         //
        public void ouvrirPorte(Cellule celluleCourante, SoundEffect bruitPorte, String code)
        {
            //on cherche le mur correspondant à la porte, et on l'ouvre... Puis on fait pareil pour le mur opposé dans la cellule voisine
            foreach (var mur in celluleCourante.murs)
            {
                //on regarde si le mur est une porte et si le code entré par le joueur est le bon
                if (mur.Value.state.Equals(Etat.PORTE) && mur.Value.convertir(code))
                {
                    mur.Value.ouvrir();
                    foreach (var vois in celluleCourante.voisins)
                    {
                        if (vois.Item2.Equals(mur.Key))
                        {
                            bruitPorte.Play();
                            vois.Item1.murs[this.laby.opposes[mur.Key]].ouvrir();
                            //Et on complète les listes d'adjacence
                            celluleCourante.addAdjacent(vois);
                            vois.Item1.addAdjacent(new Tuple<Cellule, Direction>(celluleCourante, this.laby.opposes[mur.Key]));
                            break;
                        }
                    }
                    break;
                }
            }

            dessin_ConstrBuffersMursPortes(); //on actualise le buffer pour ne plus afficher la porte
        }
        //                                  //
    }
}
