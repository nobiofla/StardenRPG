﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardenRPG.SpriteManager;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace StardenRPG
{
    public class Sprite
    {
        public Vector2 Position { get; set; }
        public Point CellSize { get; set; }
        public Point Size { get; set; }
        
        // Physics
        public Body Body { get; private set; }
        public World World { get; private set; }
        public Body PhysicsBody;

        public Texture2D spriteTexture { get; set; }
        protected SpriteSheetAnimationPlayer _animationPlayer;
        public SpriteSheetAnimationPlayer animationPlayer
        {
            get { return _animationPlayer; }
            set
            {
                if (_animationPlayer != value && _animationPlayer != null)
                    _animationPlayer.OnAnimationStopped -= OnAnimationStopped;

                _animationPlayer = value;
                _animationPlayer.OnAnimationStopped += OnAnimationStopped;
            }
        }

        public Color Tint { get; set; }

        protected Rectangle sourceRect
        {
            get
            {
                if (animationPlayer != null)
                    return new Rectangle((int)animationPlayer.CurrentCell.X, (int)animationPlayer.CurrentCell.Y, CellSize.X, CellSize.Y);
                else
                {
                    if (CellSize == Point.Zero)
                        CellSize = new Point(spriteTexture.Width, spriteTexture.Height);

                    return new Rectangle(0, 0, CellSize.X, CellSize.Y);
                }
            }
        }

        public Sprite(Texture2D spriteSheetAsset, Point size, Point cellSize, World world, Vector2 position)
        {
            spriteTexture = spriteSheetAsset;
            Tint = Color.White;
            Size = size;
            CellSize = cellSize;
            World = world; 
            Position = position;

            //Body = World.CreateBody(Position, 0, BodyType.Dynamic);
            Body = World.CreateRectangle(Size.X, Size.Y, 1, Position);
            Body.BodyType = BodyType.Dynamic;
            Body.FixedRotation = true;
            Body.OnCollision += OnCollision;
        }

        private bool OnCollision(Fixture sender, Fixture other, Contact contact)
        {
            // You can add custom collision handling logic here.
            return true;
        }

        protected virtual void OnAnimationStopped(SpriteSheetAnimationClip clip)
        {
            return;
        }

        public virtual void StartAnimation(string animation)
        {
            if (animationPlayer != null)
                animationPlayer.StartClip(animation);
        }

        public virtual void StopAnimation()
        {
            if (animationPlayer != null)
                animationPlayer.StopClip();
        }

        public virtual void Update(GameTime gameTime)
        {
            if (animationPlayer != null)
                animationPlayer.Update(gameTime.ElapsedGameTime);

            Position = Body.Position;
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteTexture, new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y), sourceRect, Tint);
        }
    }
}
