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


namespace KamiClimber.Entities
{
    class CursorEntity : PositionedObject
    {

        #region Fields

        private Circle frbCollision;

        #endregion

        #region Constructor

        public CursorEntity(string contentManagerName)
        {
            SpriteManager.AddPositionedObject(this);
        }

        #endregion

        #region Public Methods

        public Circle Collision
        {
            get { return frbCollision; }
        }

        public override void Initialize()
        {
            CreateCollision();

        }

        public void Activity()
        {
            X = InputManager.Mouse.WorldXAt(0);
            Y = InputManager.Mouse.WorldYAt(0);
        }

        public void Destroy()
        {
            ShapeManager.Remove(frbCollision);
            SpriteManager.RemovePositionedObject(this);
            
        }

        #endregion

        #region Private Methods

        private void CreateCollision()
        {
            frbCollision = ShapeManager.AddCircle();
            frbCollision.Radius = 1f;
            frbCollision.AttachTo(this, false);
        }

        #endregion
    }
}