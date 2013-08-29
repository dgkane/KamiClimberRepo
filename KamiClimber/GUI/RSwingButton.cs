using System;
using System.Collections.Generic;
using System.Linq;

using FlatRedBall;
using FlatRedBall.Input;
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


namespace KamiClimber.GUI
{
    class RSwingButton : PositionedObject
    {

        #region Fields

        private AxisAlignedRectangle frbCollision;

        #endregion

        #region Constructor

        public RSwingButton(string contentManagerName)
        {
            SpriteManager.AddPositionedObject(this);
        }

        #endregion

        #region Public Methods

        public AxisAlignedRectangle Collision
        {
            get { return frbCollision; }
        }

        public override void Initialize()
        {
            CreateCollision();

        }

        public void Activity()
        {
            X = SpriteManager.Camera.X - 60;
            Y = SpriteManager.Camera.Y - 30;

        }

        public void Destroy()
        {
            SpriteManager.RemovePositionedObject(this);
            ShapeManager.Remove(frbCollision);
        }

        #endregion

        #region Private Methods

        private void CreateCollision()
        {
            frbCollision = ShapeManager.AddAxisAlignedRectangle();
            frbCollision.ScaleX = 4;
            frbCollision.ScaleY = 4;
            frbCollision.AttachTo(this, false);
        }

        #endregion
    }
}