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
    public class Minimap
    {
        Game1 game;
        int tailleMiniMap;
        public Minimap(Game1 game, int tailleMiniMap)
        {
            this.game = game;
            this.tailleMiniMap = tailleMiniMap;
        }
        public void Draw()
        {
            this.game.spriteBatch.Begin();
            //fond
            Texture2D texture1px = new Texture2D(this.game.graphics.GraphicsDevice, 1, 1);
            Rectangle rectFond = new Rectangle(this.game.GraphicsDevice.Viewport.Width - this.tailleMiniMap, 0, this.tailleMiniMap, this.tailleMiniMap);
            texture1px.SetData(new Color[] { Color.White });
            this.game.spriteBatch.Draw(texture1px, rectFond, Color.White);

            //murs,cellules
            texture1px.SetData(new Color[] { Color.White });
            int largeurCase = this.tailleMiniMap / this.game.tailleLabyC;
            int hauteurCase = this.tailleMiniMap / this.game.tailleLabyL;
            Rectangle rectangle;

            //cellule de fin de niveau en vert
            rectangle = new Rectangle((int)(this.game.GraphicsDevice.Viewport.Width - largeurCase), this.tailleMiniMap - hauteurCase, largeurCase, hauteurCase);
            this.game.spriteBatch.Draw(texture1px, rectangle, Color.Green);

            //dessin cellule courante + murs
            for(int x = 0 ; x < this.game.tailleLabyL ; x++)
            {
                for (int z = 0 ; z < this.game.tailleLabyC ; z++)
                {
                    //cellule courante en jaune
                    if (this.game.labyrinthe.laby.maze[x, z].Equals(this.game.celluleCourante))
                    {
                        rectangle = new Rectangle((int)((x * largeurCase) + this.game.GraphicsDevice.Viewport.Width - this.tailleMiniMap), z * hauteurCase, largeurCase, hauteurCase);
                        this.game.spriteBatch.Draw(texture1px, rectangle, Color.Yellow);
                    }
                    //murs nord en noir
                    if (this.game.labyrinthe.laby.maze[x, z].murs[Direction.NORD].state.Equals(Etat.FERME))
                    {
                        rectangle = new Rectangle((int)((x * largeurCase) + this.game.GraphicsDevice.Viewport.Width - this.tailleMiniMap), z * hauteurCase, 3, hauteurCase);
                        this.game.spriteBatch.Draw(texture1px, rectangle, Color.Black);
                    }
                    //portes nord en rouge
                    if (this.game.labyrinthe.laby.maze[x, z].murs[Direction.NORD].state.Equals(Etat.PORTE))
                    {
                        rectangle = new Rectangle((int)((x * largeurCase) + this.game.GraphicsDevice.Viewport.Width - this.tailleMiniMap), z * hauteurCase, 3, hauteurCase);
                        this.game.spriteBatch.Draw(texture1px, rectangle, Color.Red);
                    }
                    //murs ouest en noir
                    if (this.game.labyrinthe.laby.maze[x, z].murs[Direction.OUEST].state.Equals(Etat.FERME))
                    {
                        rectangle = new Rectangle((int)((x * largeurCase) + this.game.GraphicsDevice.Viewport.Width - this.tailleMiniMap), z * hauteurCase, largeurCase, 3);
                        this.game.spriteBatch.Draw(texture1px, rectangle, Color.Black);
                    }
                    //portes ouest en rouge
                    if (this.game.labyrinthe.laby.maze[x, z].murs[Direction.OUEST].state.Equals(Etat.PORTE))
                    {
                        rectangle = new Rectangle((int)((x * largeurCase) + this.game.GraphicsDevice.Viewport.Width - this.tailleMiniMap), z * hauteurCase, largeurCase, 3);
                        this.game.spriteBatch.Draw(texture1px, rectangle, Color.Red);
                    }
                }
            }
            //mur du bas à gauche de la minimap jusqu'au bas à droite de la minimap (en noir)
            rectangle = new Rectangle((int)(this.game.GraphicsDevice.Viewport.Width - this.tailleMiniMap), this.tailleMiniMap, this.tailleMiniMap, 3);
            this.game.spriteBatch.Draw(texture1px, rectangle, Color.Black);
            //mur du haut à droite de la minimap jusqu'au bas à droite de la minimap (en noir)
            rectangle = new Rectangle((int)(this.game.GraphicsDevice.Viewport.Width - 3), 0, 3, this.tailleMiniMap);
            this.game.spriteBatch.Draw(texture1px, rectangle, Color.Black);

            this.game.spriteBatch.End();
            this.game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }
    }
}
