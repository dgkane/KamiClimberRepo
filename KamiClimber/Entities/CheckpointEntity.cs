using System;
using System.Collections.Generic;
using System.Linq;

using FlatRedBall;
using FlatRedBall.Graphics;
using FlatRedBall.Utilities;
using FlatRedBall.Math;
using FlatRedBall.Math.Geometry;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Common;
using FarseerPhysics.Collision.Shapes;

using KamiClimber.Screens;


namespace KamiClimber.Entities
{
    class CheckpointEntity : PositionedObject
    {

        #region Fields

        private AxisAlignedRectangle frbCollision;
        private float timeBonus;

        #endregion

        #region Constructor

        public CheckpointEntity(string contentManagerName)
        {
            SpriteManager.AddPositionedObject(this);

        }



        #endregion

        #region Private Methods

        private void CreateCollision()
        {
            frbCollision = ShapeManager.AddAxisAlignedRectangle();
            frbCollision.Color = Color.Red;
            frbCollision.ScaleX = 1f;
            frbCollision.ScaleY = 5f;
            frbCollision.AttachTo(this, false);
        }


        #endregion Public Methods

        #region Public Methods

        public AxisAlignedRectangle Collision
        {
            get { return frbCollision; }
        }


        public void Initialize(Vector2 pos, float f)
        {
            CreateCollision();
            X = pos.X;
            Y = pos.Y;
            timeBonus = f;
            


        }

        public void Activity()
        {

        }

        public void Destroy()
        {

            ShapeManager.Remove(frbCollision);
            SpriteManager.RemovePositionedObject(this);
        }

        public float getTimeBonus
        {
            get { return timeBonus; }
        }

        #endregion


    }
}