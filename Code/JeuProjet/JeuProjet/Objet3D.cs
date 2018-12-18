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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Objet3D : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Model avatar;
        protected Game1 myGame;
        protected Vector3 position;
        protected Vector3 rotation;
        protected string chemin;
        protected float scale;

        public Objet3D(Game game, string chemin, Vector3 position, Vector3 rotation, float scale)
            : base(game)
        {
            myGame = (Game1)this.Game;
            this.chemin = chemin;
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;

        }

        protected override void LoadContent()
        {
            this.avatar = myGame.Content.Load<Model>(chemin);
            base.LoadContent();
        }

        void drawModel(Model model, Vector3 modelPosition, Vector3 rotation, float sc)
        {
            foreach (ModelMesh mesh in this.avatar.Meshes)
            { 
                foreach(BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.World = Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z) * Matrix.CreateScale(sc) * Matrix.CreateTranslation(modelPosition);
                    effect.Projection = myGame.getCam().getProjection();
                    effect.View = ((Camera3D)myGame.getCam()).getView();
                }
                mesh.Draw();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            this.drawModel(this.avatar,position,rotation,scale);
            base.Draw(gameTime);
        }
    }
}
