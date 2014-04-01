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

namespace Bullblazer
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Song _music;
        private bool _musicPlaying = false;

        private Texture2D _cowboy1, _cowboy2, _bull, _playfield;
        private Vector2 _cowboy1Position, _cowboy2Position, _bullPosition;
        private int _bullAttachedTo = 0;
        private SpriteFont _font;

        private int _cowboy1Score, _cowboy2Score;
        private KeyboardState _prevState, _currState;

        private readonly Vector2 _bullStartPosition = new Vector2(304, 220);
        private readonly Vector2 _cowboy1StartPosition = new Vector2(16, 220);
        private readonly Vector2 _cowboy2StartPosition = new Vector2(614, 220);

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferHeight = 480;
            _graphics.PreferredBackBufferWidth = 640;
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
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        private void ResetGameField()
        {
            _cowboy1Position = _cowboy1StartPosition;
            _cowboy2Position = _cowboy2StartPosition;
            _bullPosition = _bullStartPosition;
            _bullAttachedTo = 0;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _music = Content.Load<Song>("music");
            _cowboy1 = Content.Load<Texture2D>("cowboy1");
            _cowboy2 = Content.Load<Texture2D>("cowboy2");
            _bull = Content.Load<Texture2D>("bull");
            _playfield = Content.Load<Texture2D>("playfield");
            _font = Content.Load<SpriteFont>("bullfont");

            ResetGameField();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (!_musicPlaying)
            {
                MediaPlayer.Play(_music);
                MediaPlayer.IsRepeating = true;
                _musicPlaying = true;
            }

            _prevState = _currState;
            _currState = Keyboard.GetState();


            if (_currState.IsKeyDown(Keys.F1) && !_prevState.IsKeyDown(Keys.F1))
            {
                _graphics.ToggleFullScreen();
            }

            if (_currState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (_currState.IsKeyDown(Keys.D))
            {
                _cowboy1Position += new Vector2(1, 0);
                if (_bullAttachedTo == 1)
                {
                    _bullPosition += new Vector2(1, 0);
                }
            }
            if (_currState.IsKeyDown(Keys.A))
            {
                _cowboy1Position += new Vector2(-1, 0);
                if (_bullAttachedTo == 1)
                {
                    _bullPosition += new Vector2(-1, 0);
                }
            }
            if (_currState.IsKeyDown(Keys.W))
            {
                _cowboy1Position += new Vector2(0, -1);
                if (_bullAttachedTo == 1)
                {
                    _bullPosition += new Vector2(0, -1);
                }
            }
            if (_currState.IsKeyDown(Keys.S))
            {
                _cowboy1Position += new Vector2(0, 1);
                if (_bullAttachedTo == 1)
                {
                    _bullPosition += new Vector2(0, 1);
                }
            }

            if (_currState.IsKeyDown(Keys.Right))
            {
                _cowboy2Position += new Vector2(1, 0);
                if (_bullAttachedTo == 2)
                {
                    _bullPosition += new Vector2(1, 0);
                }
            }
            if (_currState.IsKeyDown(Keys.Left))
            {
                _cowboy2Position += new Vector2(-1, 0);
                if (_bullAttachedTo == 2)
                {
                    _bullPosition += new Vector2(-1, 0);
                }
            }
            if (_currState.IsKeyDown(Keys.Up))
            {
                _cowboy2Position += new Vector2(0, -1);
                if (_bullAttachedTo == 2)
                {
                    _bullPosition += new Vector2(0, -1);
                }
            }
            if (_currState.IsKeyDown(Keys.Down))
            {
                _cowboy2Position += new Vector2(0, 1);
                if (_bullAttachedTo == 2)
                {
                    _bullPosition += new Vector2(0, 1);
                }
            }

            if (_currState.IsKeyDown(Keys.Space) && _bullAttachedTo == 1)
            {
                _bullPosition += new Vector2(32, 0);
                _bullAttachedTo = 0;
            }
            if (_currState.IsKeyDown(Keys.Enter) && _bullAttachedTo == 2)
            {
                _bullPosition -= new Vector2(32, 0);
                _bullAttachedTo = 0;
            }

            _cowboy1Position = new Vector2(Math.Max(12, Math.Min(640 - 20, _cowboy1Position.X)), Math.Max(0, Math.Min(480 - 32, _cowboy1Position.Y)));
            _cowboy2Position = new Vector2(Math.Max(12, Math.Min(640 - 20, _cowboy2Position.X)), Math.Max(0, Math.Min(480 - 32, _cowboy2Position.Y)));
            _bullPosition = new Vector2(Math.Max(0, Math.Min(640 - 8, _bullPosition.X)), Math.Max(0, Math.Min(480 - 32, _bullPosition.Y)));

            if (_bullPosition.Y >= 188 && _bullPosition.Y < 268)
            {
                if (_bullPosition.X <= 0)
                {
                    _cowboy2Score++;
                    ResetGameField();
                }
                if (_bullPosition.X >= 628)
                {
                    _cowboy1Score++;
                    ResetGameField();
                }
            }

            var cowboy1Rect = new Rectangle((int) _cowboy1Position.X, (int) _cowboy1Position.Y, 8, 32);
            var cowboy2Rect = new Rectangle((int) _cowboy2Position.X, (int) _cowboy2Position.Y, 8, 32);
            var bullRect = new Rectangle((int) _bullPosition.X, (int) _bullPosition.Y, 32, 26);
            if (cowboy1Rect.Intersects(bullRect) && _bullAttachedTo != 1)
            {
                _bullAttachedTo = 1;
            }
            else if (cowboy2Rect.Intersects(bullRect) && _bullAttachedTo != 2)
            {
                _bullAttachedTo = 2;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkOliveGreen);

            _spriteBatch.Begin();

            _spriteBatch.Draw(_playfield, Vector2.Zero, Color.White);
            _spriteBatch.DrawString(_font, _cowboy1Score.ToString(), new Vector2(300, 5), Color.White);
            _spriteBatch.DrawString(_font, _cowboy2Score.ToString(), new Vector2(340, 5), Color.White);
            _spriteBatch.Draw(_cowboy1, _cowboy1Position, Color.White);
            _spriteBatch.Draw(_cowboy2, _cowboy2Position, Color.White);
            _spriteBatch.Draw(_bull, _bullPosition, Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
