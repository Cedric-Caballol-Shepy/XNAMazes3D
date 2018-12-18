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
    public class Cube3DTex : Objet3D
    {
        private VertexPositionTexture[] verticesTex1;
        private VertexPositionTexture[] verticesTex2;
        private VertexPositionTexture[] verticesTex3;
        private VertexPositionTexture[] verticesTex4;
        private VertexPositionTexture[] verticesTex5;
        private VertexPositionTexture[] verticesTex6;
        private Texture2D[] faces;

        private GraphicsDevice device;
        private VertexBuffer B;

        private SpriteBatch spriteBatch;
        private BasicEffect effect;

        public Cube3DTex(Game game, string chemin, Vector3 position, Vector3 rotation, float scale)
            : base(game, chemin, position, rotation, scale)
        {
           
        }

        
        public override void Initialize()
        {
           
            base.Initialize();
        }

       
        public override void Update(GameTime gameTime)
        {
            this.rotation.X += .005f;
            this.rotation.Y += .03f;
            this.rotation.Z += .03f;
            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            this.device = myGame.GraphicsDevice;
            spriteBatch = new SpriteBatch(((Game1)this.Game).GraphicsDevice);
            this.effect = new BasicEffect(((Game1)this.Game).GraphicsDevice);
            this.CreateCubeTextured();

            this.faces = new Texture2D[6];
            this.faces[0] = myGame.Content.Load<Texture2D>("fin");
            this.faces[1] = myGame.Content.Load<Texture2D>("fin");
            this.faces[2] = myGame.Content.Load<Texture2D>("fin");
            this.faces[3] = myGame.Content.Load<Texture2D>("fin");
            this.faces[4] = myGame.Content.Load<Texture2D>("fin");
            this.faces[5] = myGame.Content.Load<Texture2D>("fin");

           // base.LoadContent();
        }

        private void CreateCubeTextured()
        { 
            //face arrière
            this.verticesTex1 = new VertexPositionTexture[]
            {
                new VertexPositionTexture( new Vector3(1 , 1 , -1), new Vector2(1,0)),
                new VertexPositionTexture( new Vector3(-1 , 1 , -1), new Vector2(0,0)),
                new VertexPositionTexture( new Vector3(-1 , -1 , -1), new Vector2(0,1)),
                new VertexPositionTexture( new Vector3(1 , 1 , -1), new Vector2(1,0)),
                new VertexPositionTexture( new Vector3(-1 , -1 , -1), new Vector2(0,1)),
                new VertexPositionTexture( new Vector3(1 , -1 , -1), new Vector2(1,1)),
            };

            //face avant
            this.verticesTex2 = new VertexPositionTexture[]
            {
                new VertexPositionTexture( new Vector3(-1 , 1 , 1)  ,new Vector2(1,0)),
                new VertexPositionTexture( new Vector3(1 , 1 , 1)   ,new Vector2(0,0)),
                new VertexPositionTexture( new Vector3(1 , -1 , 1)  ,new Vector2(0,1)),
                new VertexPositionTexture( new Vector3(-1 , 1 , 1)    ,new Vector2(1,0)),
                new VertexPositionTexture( new Vector3(1 , -1 , 1)  ,new Vector2(0,1)),
                new VertexPositionTexture( new Vector3(-1 , -1 , 1)   ,new Vector2(1,1)),
            };

            //face bas
            this.verticesTex3 = new VertexPositionTexture[]
            {
                new VertexPositionTexture( new Vector3(1 , -1 , -1)  ,new Vector2(1,0)),
                new VertexPositionTexture( new Vector3(-1 , -1 , -1)   ,new Vector2(0,0)),
                new VertexPositionTexture( new Vector3(-1 , -1 , 1)  ,new Vector2(0,1)),
                new VertexPositionTexture( new Vector3(1 , -1 , -1)    ,new Vector2(1,0)),
                new VertexPositionTexture( new Vector3(-1 , -1 , 1)  ,new Vector2(0,1)),
                new VertexPositionTexture( new Vector3(1 , -1 , 1)   ,new Vector2(1,1)),
            };

            //face haut
            this.verticesTex4 = new VertexPositionTexture[]
            {
                new VertexPositionTexture( new Vector3(1 , 1 , 1)  ,new Vector2(1,0)),
                new VertexPositionTexture( new Vector3(-1 , 1 , 1)   ,new Vector2(0,0)),
                new VertexPositionTexture( new Vector3(-1 , 1 , -1)  ,new Vector2(0,1)),
                new VertexPositionTexture( new Vector3(1 , 1 , 1)    ,new Vector2(1,0)),
                new VertexPositionTexture( new Vector3(-1 , 1 , -1)  ,new Vector2(0,1)),
                new VertexPositionTexture( new Vector3(1 , 1 , -1)   ,new Vector2(1,1)),
            };

            //face droite
            this.verticesTex5 = new VertexPositionTexture[]
            {
                new VertexPositionTexture( new Vector3(1 , 1 , 1)  ,new Vector2(1,0)),
                new VertexPositionTexture( new Vector3(1 , 1 , -1)   ,new Vector2(0,0)),
                new VertexPositionTexture( new Vector3(1 , -1 , -1)  ,new Vector2(0,1)),
                new VertexPositionTexture( new Vector3(1 , 1 , 1)    ,new Vector2(1,0)),
                new VertexPositionTexture( new Vector3(1 , -1 , -1)  ,new Vector2(0,1)),
                new VertexPositionTexture( new Vector3(1 , -1 , 1)   ,new Vector2(1,1)),
            };

            //face gauche
            this.verticesTex6 = new VertexPositionTexture[]
            {
                new VertexPositionTexture( new Vector3(-1 , 1 , -1)  ,new Vector2(1,0)),
                new VertexPositionTexture( new Vector3(-1 , 1 , 1)   ,new Vector2(0,0)),
                new VertexPositionTexture( new Vector3(-1 , -1 , 1)  ,new Vector2(0,1)),
                new VertexPositionTexture( new Vector3(-1 , 1 , -1)    ,new Vector2(1,0)),
                new VertexPositionTexture( new Vector3(-1 , -1 , 1)  ,new Vector2(0,1)),
                new VertexPositionTexture( new Vector3(-1 , -1 , -1)   ,new Vector2(1,1)),
            };
            
        }

        public void drawTexture(VertexPositionTexture[] v, Texture2D t, int i)
        {
            B = new VertexBuffer(this.GraphicsDevice, typeof(VertexPositionTexture), v.Length, BufferUsage.WriteOnly);
            B.SetData(v);

            this.GraphicsDevice.SetVertexBuffer(B);
            this.effect.VertexColorEnabled = false;
            this.effect.TextureEnabled = true;
            this.effect.Texture = t;

            foreach(EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                this.device.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, v, 0, v.Length / 3);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            effect.View = ((Game1)this.Game).getCam().getView();
            effect.Projection = ((Game1)this.Game).getCam().getProjection();
            effect.World = Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z) * Matrix.CreateScale(scale) * Matrix.CreateTranslation(position); ;

            drawTexture(this.verticesTex1, faces[0], 1);
            drawTexture(this.verticesTex2, faces[1], 2);
            drawTexture(this.verticesTex3, faces[2], 3);
            drawTexture(this.verticesTex4, faces[3], 4);
            drawTexture(this.verticesTex5, faces[4], 5);
            drawTexture(this.verticesTex6, faces[5], 6);
        }
    }
}
