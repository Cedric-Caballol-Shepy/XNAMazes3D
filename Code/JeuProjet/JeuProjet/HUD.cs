using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace JeuProjet
{
   
    public class HUD
    {
        public float timer = 180; // 3min
        private Game1 game;

        public HUD(Game1 game)
        {
            this.game = game;
        }

        public void Draw(GameTime gameTime)
        {
            this.game.spriteBatch.Begin();

            //fond timer
            Texture2D texture = new Texture2D(this.game.graphics.GraphicsDevice, 1, 1);
            Rectangle rectFond = new Rectangle(0,0,180,40);
            texture.SetData(new Color[] { Color.White });
            this.game.spriteBatch.Draw(texture, rectFond, Color.White);
            //fond vie
            rectFond = new Rectangle(0, this.game.spriteBatch.GraphicsDevice.Viewport.Height-40, 140, 40);
            this.game.spriteBatch.Draw(texture, rectFond, Color.White);
            //timer
            this.game.spriteBatch.DrawString(this.game.fontHUD, "Time : " + this.timer.ToString("00"), new Vector2(10, 10), Color.Black);
            //vie
            this.game.spriteBatch.DrawString(this.game.fontHUD, "Vie : " + this.game.vie.ToString(), new Vector2(10, this.game.spriteBatch.GraphicsDevice.Viewport.Height - 30), Color.Black);
            
            //recherche type conversion
            Mur mur = new Mur();
            foreach (Mur m in this.game.celluleCourante.murs.Values)
            {
                mur = m;
                if (m.state.Equals(Etat.PORTE))
                    break;
            }
            string typeConv = mur.donneTypeConv();

            //fond type conv à saisir
            rectFond = new Rectangle((this.game.spriteBatch.GraphicsDevice.Viewport.Width / 2)-210, 0, 500, 80);
            this.game.spriteBatch.Draw(texture, rectFond, Color.White);

            //fond saisie
            rectFond = new Rectangle((this.game.spriteBatch.GraphicsDevice.Viewport.Width) - 300, this.game.spriteBatch.GraphicsDevice.Viewport.Height - 50, 300, 30);
            this.game.spriteBatch.Draw(texture, rectFond, Color.White);
            
            //type de conv à saisir 
            this.game.spriteBatch.DrawString(this.game.fontHUD, "Type de conversion : " +  typeConv, new Vector2((this.game.spriteBatch.GraphicsDevice.Viewport.Width/2)-200, 10), Color.Black);
            //conv à saisir
            this.game.spriteBatch.DrawString(this.game.fontHUD, "Conversion a saisir : " + mur.code, new Vector2((this.game.spriteBatch.GraphicsDevice.Viewport.Width / 2) - 200, 30), Color.Black);
            //saisie utilisateur
            this.game.spriteBatch.DrawString(this.game.fontHUD, "Saisie : " + this.game.code, new Vector2(this.game.spriteBatch.GraphicsDevice.Viewport.Width - 300,
                this.game.spriteBatch.GraphicsDevice.Viewport.Height - 50), Color.Black);
            this.game.spriteBatch.End();
            this.game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }

        public void Update(GameTime gameTime)
        {
            this.timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

        }
    }
}
