using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Ethanhundreds
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Random rnd = new Random();

        List<Ball> balls = new List<Ball>();


        public static Texture2D Pixel;

        int numberofball = 4;

        bool run = true;
        SpriteFont font;

        int totalscore = 0;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 1000;
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
            IsMouseVisible = true;

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

            Texture2D ballimage = Content.Load<Texture2D>("ball");
            font = Content.Load<SpriteFont>("File");
            SpawnBalls();
            // TODO: use this.Content to load your game content here

            //Debug
            Pixel = new Texture2D(GraphicsDevice, 1, 1);
            Pixel.SetData(new [] { Color.White });

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public void SpawnBalls()
        {
            Texture2D ballimage = Content.Load<Texture2D>("ball");
            for (int j = 0; j < numberofball; j++)
            {
                //Generate a random position, and a temporary ball
                var position = new Vector2(rnd.Next(0, 1601), rnd.Next(0, 1001));
                var tempBall = new Ball(ballimage, position, Color.White, new Vector2(7, 7)) { IsDebug = false };

                while (!isValid(tempBall))
                {
                    tempBall.position = new Vector2(rnd.Next(0, 1601), rnd.Next(0, 1001));
                }

                balls.Add(tempBall);            
            }
        }

        private bool isValid(Ball tempBall)
        {
            foreach (var ball in balls)
            {
                if(ball.Collision(tempBall))
                {
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();
            KeyboardState ks = Keyboard.GetState();
            if (run == true)
            {
                for (int i = 0; i < numberofball; i++)
                {
                    balls[i].Wall(GraphicsDevice.Viewport);

                    if (balls[i].Hitbox.Contains(ms.X, ms.Y))
                    {
                        balls[i].scale += 0.002f;
                        balls[i].score++;
                    }
                }
                for (int i = 0; i < numberofball; i++)
                {
                    for (int j = i + 1; j < numberofball; j++)
                    {
                        if (balls[i].Collision(balls[j]))
                        //if (balls[i].Hitbox.Intersects(balls[j].Hitbox))
                        {
                            balls[i].speed.X = -balls[i].speed.X;
                            balls[i].speed.Y = -balls[i].speed.Y;

                            balls[j].speed.X = -balls[j].speed.X;
                            balls[j].speed.Y = -balls[j].speed.Y;

                            if (balls[i].Hitbox.Contains(ms.X, ms.Y))
                            {
                                run = false;
                            }

                            if (balls[j].Hitbox.Contains(ms.X, ms.Y))
                            {
                                run = false;
                            }
                        }
                    }
                }

                totalscore = 0;
                for (int i = 0; i < numberofball; i++)
                {
                    totalscore += balls[i].score;
                }

                if (totalscore == 100)
                {
                    run = false;
                }
            }
            if (ks.IsKeyDown(Keys.Enter) && run == false && totalscore == 100)
            {
                Texture2D ballimage = Content.Load<Texture2D>("ball");

                numberofball++;

                balls.Add(new Ball(ballimage, new Vector2(rnd.Next(0, 1601), rnd.Next(0, 1001)), Color.White, new Vector2(7, 7)));

                for (int i = 0; i < numberofball; i++)
                {
                    balls[i].position = new Vector2(rnd.Next(0, 1601), rnd.Next(0, 1001));
                    balls[i].score = 0;
                    totalscore = 0;
                    run = true;
                    balls[i].scale = 0.05f;

                    for (int j = i + 1; j < numberofball; j++)
                    {
                        if (balls[i].Collision(balls[j]))
                        {
                            balls[i].position = new Vector2(rnd.Next(1, 1601), rnd.Next(0, 1001));
                        }
                    }
                }

            }
            if (ks.IsKeyDown(Keys.Enter) && run == false && totalscore < 100)
            {
                Texture2D ballimage = Content.Load<Texture2D>("ball");

                numberofball--;

                for (int i = 0; i < numberofball; i++)
                {
                    balls[i].position = new Vector2(rnd.Next(0, 1601), rnd.Next(0, 1001));
                    balls[i].score = 0;
                    totalscore = 0;
                    run = true;
                    balls[i].scale = 0.05f;

                    for (int j = i + 1; j < numberofball; j++)
                    {
                        if (balls[i].Collision(balls[j]))
                        {
                            balls[i].position = new Vector2(rnd.Next(1, 1601), rnd.Next(0, 1001));
                        }
                    }
                }

            }

            base.Update(gameTime);
            }
        

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();
            for (int i = 0; i < numberofball; i++)
            {
                balls[i].Draw(spriteBatch);
            }

            for (int i = 0; i < numberofball; i++)
            {
                spriteBatch.DrawString(font, balls[i].score.ToString(), balls[i].Center, Color.White);
            }

            if( run==false && totalscore == 100)
            {
                spriteBatch.DrawString(font, "Next Level Hit Enter!", new Vector2(800,500), Color.Black);
            }

            if(run == false && totalscore < 100)
            {
                spriteBatch.DrawString(font, "Game Over!Last Level Hit Enter!", new Vector2(800, 500), Color.Black);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
