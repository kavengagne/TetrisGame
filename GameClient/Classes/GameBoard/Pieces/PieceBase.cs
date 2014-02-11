using System.Collections.Generic;
using GameClient.Classes.Core;
using GameClient.Classes.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Classes.GameBoard.Pieces
{
    public abstract class PieceBase : ISprite
    {
        #region Properties
        protected Board Board { get; set; }
        public Point Position { get; protected set; }
        public Color Color { get; protected set; }
        public Block[] Blocks { get; protected set; }
        public PieceModel Model { get; protected set; }
        public int RotationIndex { get; protected set; }
        public Rectangle BlockSize { get; protected set; }
        public SoundManager SoundManager { get; set; }
        #endregion


        #region Constructor
        protected PieceBase(Color color, PieceModel model, int rotationIndex, Rectangle blockSize)
        {
            Color = color;
            Model = model;
            RotationIndex = 0;
            if (rotationIndex >= 0 && rotationIndex < model.Length)
            {
                RotationIndex = rotationIndex;
            }
            BlockSize = blockSize;
            CreateBlocks(Model[RotationIndex]);
            SoundManager = Application.Instance.Game.SoundManager;
        }
        #endregion


        #region Implementation of ISprite
        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (Block block in Blocks)
            {
                block.Draw(spriteBatch, gameTime);
            }
        }
        #endregion


        #region Internal Implementation
        protected void CreateBlocks(IList<Point> positions)
        {
            Blocks = new Block[positions.Count];
            for (int i = 0; i < Blocks.Length; i++)
            {
                Blocks[i] = new Block(this, positions[i], BlockSize, Color);
            }
        }

        public abstract void UpdateBlocksPositions(Point offset);
        #endregion
    }
}