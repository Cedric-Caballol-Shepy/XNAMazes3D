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
    public class Camera3D 
    {
        public Game1 game;
        private Vector3 cameraPosition;
        private Vector3 cameraRotation;
        private Vector3 cameraLookAt;
        private float cameraSpeed;
        private Vector3 mouseRotationBuffer;
        private MouseState currentMouseState;
        private MouseState prevMouseState;

        public Vector3 position
        {
            get 
            { 
                return cameraPosition;
            }
            set
            {
                cameraPosition = value;
                updateLookAt();
            }
        }
        public Vector3 rotation
        {
            get 
            {
                return cameraRotation;
            }
            set
            {
                cameraRotation = value;
                updateLookAt();
            }
        }

        // Transformation matrices
        public Matrix projectionMatrix { get; protected set; }
        public Matrix viewMatrix
        {
            get
            {
                return Matrix.CreateLookAt(cameraPosition, cameraLookAt, Vector3.Up);
            }
        }

        public Camera3D(Game game, Vector3 position, Vector3 rotation, float speed)
        {
            this.game = (Game1)game;
            cameraSpeed = speed;
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, this.game.GraphicsDevice.Viewport.AspectRatio, 0.05f, 1000.0f);
            moveTo(position, rotation);
        }


        private void setLookAt(Vector3 l)
        {
            this.cameraLookAt = l;
        }

        private void updateLookAt()
        {
            Matrix rotationMatrix = Matrix.CreateRotationX(cameraRotation.X) * Matrix.CreateRotationY(cameraRotation.Y);
            Vector3 lookAtOffset = Vector3.Transform(Vector3.UnitZ, rotationMatrix);
            cameraLookAt = cameraPosition + lookAtOffset;
        }

        private void setPosition(Vector3 p)
        {
            this.position = new Vector3(p.X, p.Y, p.Z);
        }
        private void setRotation(Vector3 r)
        {
            this.rotation = r;
        }
        private void moveTo(Vector3 pos, Vector3 rot)
        {
            setPosition(pos);
            setRotation(rot);
        }

        public Matrix getProjection()
        {
            return projectionMatrix;
        }

        public Matrix getView()
        {
            return viewMatrix;
        }

        public Vector3 previewMove(Vector3 amount)
        {
            Matrix rotate = Matrix.CreateRotationY(cameraRotation.Y);
            Vector3 movement = new Vector3(amount.X, amount.Y, amount.Z);
            movement = Vector3.Transform(movement, rotate);
            return cameraPosition + movement;
        }

        private void move(Vector3 scale)
        {
            moveTo(previewMove(scale), rotation);
        }

       
        public void Update(GameTime gameTime)
        {

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            currentMouseState = Mouse.GetState();
            KeyboardState ks = Keyboard.GetState();
            Vector3 moveVector = Vector3.Zero;
            if (ks.IsKeyDown(Keys.Z))
                moveVector.Z = 1;
            if (ks.IsKeyDown(Keys.S))
                moveVector.Z = -1;
            if (ks.IsKeyDown(Keys.Q))
                moveVector.X = 1;
            if (ks.IsKeyDown(Keys.D))
                moveVector.X = -1;
            // cheats //
            if (ks.IsKeyDown(Keys.M))
                moveVector.Y = -1;
            if (ks.IsKeyDown(Keys.P))
                moveVector.Y = 1;
            ////////////
            if (moveVector != Vector3.Zero)
            {
                //normalisation pour ne pas bouger plus vite en diagonale
                moveVector.Normalize();
                moveVector *= dt * cameraSpeed;
                //gestion collisions aux frontières du labyrinthe //
                bool moveOk = true;
                if (previewMove(moveVector).X - 0.1f < 0 || previewMove(moveVector).X > (this.game.labyrinthe.tailleC * this.game.tailleMurs) - 0.1f)
                    moveOk = false;
                if (previewMove(moveVector).Z - 0.1f < 0 || previewMove(moveVector).Z > (this.game.labyrinthe.tailleL * this.game.tailleMurs) - 0.1f)
                    moveOk = false;
                //gestion collisions aux murs des cellules //
                foreach (BoundingBox box in this.game.labyrinthe.collisions_getBorduresCellule((int)previewMove(moveVector).X / this.game.tailleMurs, (int)previewMove(moveVector).Z / this.game.tailleMurs))
                {

                    if (box.Contains(previewMove(moveVector)) == ContainmentType.Contains)
                        moveOk = false;
                }
                //                                        //
                if(moveOk)
                    move(moveVector);
            }
            float deltaX,deltaY;
            if (currentMouseState != prevMouseState)
            {
                deltaX = currentMouseState.X - (this.game.GraphicsDevice.Viewport.Width / 2);
                deltaY = currentMouseState.Y - (this.game.GraphicsDevice.Viewport.Height / 2);

                mouseRotationBuffer.X -= 0.06f * deltaX * dt;
                mouseRotationBuffer.Y -= 0.06f * deltaY * dt;

                //prévention contre d'éventuels "tonnaux"
                if (mouseRotationBuffer.Y < MathHelper.ToRadians(-75.0f))
                    mouseRotationBuffer.Y = mouseRotationBuffer.Y - (mouseRotationBuffer.Y - MathHelper.ToRadians(-75.0f));
                if(mouseRotationBuffer.Y > MathHelper.ToRadians(75.0f))
                    mouseRotationBuffer.Y = mouseRotationBuffer.Y - (mouseRotationBuffer.Y - MathHelper.ToRadians(75.0f));
                
                rotation = new Vector3(-MathHelper.Clamp(mouseRotationBuffer.Y,MathHelper.ToRadians(-75.0f),MathHelper.ToRadians(75.0f))
                    ,MathHelper.WrapAngle(mouseRotationBuffer.X),0);
                deltaX = 0;
                deltaY = 0;
            }
            Mouse.SetPosition(this.game.GraphicsDevice.Viewport.Width / 2, this.game.GraphicsDevice.Viewport.Height / 2);

            prevMouseState = currentMouseState;
        }
    }
}
