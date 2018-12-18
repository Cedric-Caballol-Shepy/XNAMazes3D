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
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static Random r;
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public Camera3D camera3D;
        private Floor floor;
        public BasicEffect effect;
        public Laby_Dessins_Collisions labyrinthe;
        public int tailleMurs;
        public int tailleLabyL, tailleLabyC;
        private SoundEffect bruitPorte;
        private KeyboardState ks;
        private Keys[] lastPressedKeys;
        public String code;
        private Cube3DTex cubeFin;
        public Cellule celluleCourante;
        private Minimap map;
        private HUD hud;
        public SpriteFont fontHUD;
        public int vie;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferMultiSampling = false;
            graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            r = new Random();
            this.vie = 100;
            this.tailleLabyL = 10;
            this.tailleLabyC = 10;
            this.tailleMurs = 3;
            labyrinthe = new Laby_Dessins_Collisions(this.tailleLabyL, this.tailleLabyC, 5, 4, this, this.tailleMurs);
            //labyrinthe 10*10 avec 5 portes avec 4 types de conversion différents pour les portes et taille murs = 3
            this.camera3D = new Camera3D(this, new Vector3(1.0f, 2.0f, 1.0f), Vector3.Zero,8f);
            this.cubeFin = new Cube3DTex(this, "", new Vector3((this.tailleLabyL * this.tailleMurs) - 1.5f,
                1.0f, (this.tailleLabyC * this.tailleMurs) - 1.5f), new Vector3(0, 0, 0), 0.5f);
            this.Components.Add(cubeFin);
            Color[] floorColor = new Color[2] {Color.White , Color.Black};
            this.floor = new Floor(this, this.tailleMurs * this.tailleLabyL, this.tailleMurs * this.tailleLabyC, floorColor);
            this.effect = new BasicEffect(GraphicsDevice);
            this.code = "";
            this.lastPressedKeys = new Keys[0];
            this.map = new Minimap(this,400);
            this.hud = new HUD(this);
            base.Initialize();
        }

        public void loadMap2()
        {
            this.hud.timer = 180;
            this.vie = 100;
            this.tailleLabyL = 20;
            this.tailleLabyC = 20;
            labyrinthe = new Laby_Dessins_Collisions(this.tailleLabyL, this.tailleLabyC, 10, 4, this, this.tailleMurs);
            //labyrinthe 20*20 avec 10 portes avec 7 types de conversion différents pour les portes et taille murs = 3
            Color[] floorColor = new Color[2] { Color.DarkRed, Color.White };
            this.floor = new Floor(this, this.tailleMurs * this.tailleLabyL, this.tailleMurs * this.tailleLabyC, floorColor);
            this.Components.Remove(cubeFin);
            this.cubeFin = new Cube3DTex(this, "", new Vector3((this.tailleLabyL * this.tailleMurs) - 1.5f,
                1.0f, (this.tailleLabyC * this.tailleMurs) - 1.5f), new Vector3(0, 0, 0), 0.5f);
            this.Components.Add(cubeFin);
            this.camera3D = new Camera3D(this, new Vector3(1.0f, 2.0f, 1.0f), Vector3.Zero, 8f);
            this.code = "";
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
            bruitPorte = Content.Load<SoundEffect>("open_door");
            this.fontHUD = Content.Load<SpriteFont>("fontHUD");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            this.celluleCourante = labyrinthe.laby.maze[((int)this.camera3D.position.X / this.tailleMurs), ((int)this.camera3D.position.Z / this.tailleMurs)];
            this.ks = Keyboard.GetState();
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || ks.IsKeyDown(Keys.Escape))
                this.Exit();
            if (ks.IsKeyDown(Keys.R)) //efface le code a l'appui sur R
                this.code = "";
            entreesClavierCode();
            camera3D.Update(gameTime);
            if (this.celluleCourante.Equals(labyrinthe.laby.maze[this.tailleLabyL-1, this.tailleLabyC-1]))
                loadMap2();
            this.hud.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            this.floor.Draw(effect);
            this.labyrinthe.Draw();
            base.Draw(gameTime);
            this.map.Draw();
            this.hud.Draw(gameTime);
            if (this.hud.timer < 0 || this.vie < 0)
            {
                if (this.tailleLabyC == 10)
                {
                    Initialize();
                }
                else
                {
                    loadMap2();
                }
            }
        }
        public Camera3D getCam()
        {
            return this.camera3D;
        }

        
        /// <summary>
        /// Ecriture du code porte/ennemi par l'utilisateur
        /// Aide :
        /// https://stackoverflow.com/questions/10154046/making-text-input-in-xna-for-entering-names-chatting
        /// </summary>
        private void entreesClavierCode()
        {
            this.ks = Keyboard.GetState();
            Keys[] pressedKeys = ks.GetPressedKeys();
            //check if any of the previous update's keys are no longer pressed
            foreach (Keys key in lastPressedKeys)
            {
                if (!pressedKeys.Contains(key))
                    OnKeyUp(key);
            }
            //check if the currently pressed keys were already pressed
            foreach (Keys key in pressedKeys)
            {
                if (!lastPressedKeys.Contains(key) && this.ks.IsKeyDown(Keys.Space))
                    OnKeyDown(key);
            }

            //save the currently pressed keys so we can compare on the next update
            lastPressedKeys = pressedKeys;
        }
        private void OnKeyDown(Keys key)
        {
            if (key.Equals(Keys.A))
                this.code += "A";
            if (key.Equals(Keys.B))
                this.code += "B";
            if (key.Equals(Keys.C))
                this.code += "C";
            if (key.Equals(Keys.D))
                this.code += "D";
            if (key.Equals(Keys.E))
                this.code += "E";
            if (key.Equals(Keys.F))
                this.code += "F";
            if (key.Equals(Keys.NumPad0) || key.Equals(Keys.D0))
                this.code += "0";
            if (key.Equals(Keys.NumPad1) || key.Equals(Keys.D1))
                this.code += "1";
            if (key.Equals(Keys.NumPad2) || key.Equals(Keys.D2))
                this.code += "2";
            if (key.Equals(Keys.NumPad3) || key.Equals(Keys.D3))
                this.code += "3";
            if (key.Equals(Keys.NumPad4) || key.Equals(Keys.D4))
                this.code += "4";
            if (key.Equals(Keys.NumPad5) || key.Equals(Keys.D5))
                this.code += "5";
            if (key.Equals(Keys.NumPad6) || key.Equals(Keys.D6))
                this.code += "6";
            if (key.Equals(Keys.NumPad7) || key.Equals(Keys.D7))
                this.code += "7";
            if (key.Equals(Keys.NumPad8) || key.Equals(Keys.D8))
                this.code += "8";
            if (key.Equals(Keys.NumPad9) || key.Equals(Keys.D9))
                this.code += "9";
            if (key.Equals(Keys.Back) && this.code.Length>0)
                this.code = this.code.Remove(this.code.Length-1);
            // cheats //
            if (key.Equals(Keys.U))
                loadMap2();
            if (key.Equals(Keys.I))
                this.vie += 10;
            if (key.Equals(Keys.K))
                this.vie -= 10;
            ////////////
        }

        private void OnKeyUp(Keys key)
        {
            if (key.Equals(Keys.Space) && code.Length>0)
            {
                labyrinthe.ouvrirPorte(this.celluleCourante,bruitPorte, this.code);
            }
        }
    }
}
