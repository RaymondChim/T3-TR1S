﻿//Оставь надежду, всяк сюда входящий,
//Ибо си строки кода наполнены гневом, отчаением и печалью,
//И пренесли они великие ужасы тем глупцам, 
//Что посчитали себя способными их написать.
//Прододолжай на свой страх и риск, путник.
//И пусть пребудет с тобой терпение,
//Которое было так несвойственно этой разработке. -TCok team.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;
using System;

using NAT.Controllers;
using NAT.Models;
using NAT.Views;
using NAT.Services;
using NAT.Menu;

namespace NAT {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameMain : Game {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        public TetrisGameController _tetrisController;
        public RaceGameController _raceController;
        ControllerSenpai senpai;

        SpriteFont font;
        NamerKeysInputAdapter adapter; 

        public GameMain() {

            graphics = new GraphicsDeviceManager(this);
            /*
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            */
            Content.RootDirectory = "Content";

            _tetrisController = new TetrisGameController();
            _raceController = new RaceGameController();
            var view = new View(this);

            _tetrisController.Init(new TetrisGameModel(), view);
            _raceController.Init(new RaceGameModel(), view);


            senpai = new ControllerSenpai(new List<Tuple<string, IGameController>>()
                {
                    new Tuple<string, IGameController>("tetris",_tetrisController),
                    new Tuple<string, IGameController>("race",_raceController)
                }
            ) {
                CoreView = view
            };

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            senpai.Start(ControllerSenpai.AnySelector);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();
            senpai.Update(gameTime, ControllerSenpai.ActiveSelector);
            // TODO: Add your update logic here
            base.Update(gameTime);  
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.DimGray);
            // TODO: Add your drawing code here
            //_controller.Render();
            senpai.Render(ControllerSenpai.ActiveSelector);
            base.Draw(gameTime);
            //adapter.DrawNamer(spriteBatch, font, new Vector2(100, 100), new Color(0xff, 0xcb, 0x28));
        }
    }
}
