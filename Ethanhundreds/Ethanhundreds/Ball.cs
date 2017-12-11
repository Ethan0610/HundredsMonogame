using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Ethanhundreds
{
    class Ball
    {

        public Texture2D image;
        public Vector2 position;
        public Color tint;
        public Vector2 speed;
        public float scale;


        public bool IsDebug = false;

        public int score = 0;


        public float Radius
        {
            get
            {
                return (image.Width * scale) / 2;
            }
        }

        public Vector2 Center
        {
            get
            {
                return position + new Vector2(Radius);
            }
        }


        public Ball(Texture2D image, Vector2 position, Color tint, Vector2 speed)
        {
            this.image = image;
            this.position = position;
            this.tint = tint;
            this.speed = speed;
            scale = 0.05f;
        }




        public void Wall(Viewport screen)
        {
            position += speed;

            if (position.Y + Height > screen.Height)
            {
                speed.Y = -Math.Abs(speed.Y);

            }

            if (position.X + Width > screen.Width)
            {
                speed.X = -Math.Abs(speed.X);
            }

            if (position.X < 0)
            {
                speed.X = Math.Abs(speed.X);
            }

            if (position.Y < 0)
            {
                speed.Y = Math.Abs(speed.Y);
            }
        }
        
        public bool IsColliding(Ball otherBall)
        {
            float sumOfRadii = Radius + otherBall.Radius;
            float sumOfRadiiSquared = sumOfRadii * sumOfRadii;

            Vector2 vectorDistance = Center - otherBall.Center;
            float distanceSqured = vectorDistance.LengthSquared();

            return sumOfRadiiSquared > distanceSqured;
        }

        public bool Collision(Ball balls)
        {
            Ball circle1 = this;
            Ball circle2 = balls;

            float r1 = circle1.Radius;
            float r2 = circle2.Radius;

            float circle1X = (float)(circle1.position.X + r1);

            float circle1Y = (float)(circle1.position.Y + r1);

            float circle2X = (float)(circle2.position.X + r2);

            float circle2Y = (float)(circle2.position.Y + r2);

            float distanceBetweenCirclesSquared = (float)(Math.Sqrt( Math.Pow(circle2X - circle1X, 2) + Math.Pow(circle2Y - circle1Y, 2) ));

            if (r1+r2 > distanceBetweenCirclesSquared)
            {
                return true;
            }

            // calculate the new radius and distance if the speeds were reversed for both balls
            // then compare the distance of this what-if scenario to the original distance
            // if the new distance is smaller, then don't do the swap
            // if the new distance is larger, then continue with the swap of speeds

            return false;

        }

        public Rectangle Hitbox
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, (int)(Width), (int)(Height));
            }
        }


        public float Width
        {
            get
            {
                return image.Width * scale;
            }
        }

        public float Height
        {
            get
            {
                return image.Height * scale;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(image, position, null, tint, 0, Vector2.Zero, scale, SpriteEffects.None, 1f);

            //spriteBatch.Draw(image, position, tint);
            spriteBatch.Draw(image, new Rectangle((int)position.X, (int)position.Y, (int)Radius*2, (int)Radius*2), Color.Red * 0.6f);

            if(IsDebug)
            {
                spriteBatch.Draw(Game1.Pixel, new Vector2(Hitbox.X, Hitbox.Y), Hitbox, Color.Red * 0.5f);
            }

        }

    }
}
