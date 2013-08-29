
using System;
using System.Collections.Generic;
using System.Linq;

using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Graphics;
using FlatRedBall.Utilities;
using FlatRedBall.Math;
using FlatRedBall.Math.Geometry;
// using FlatRedBall.Screens;


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

using KamiClimber.Entities;

// using KamiClimber.Screens;


namespace KamiClimber.GUI
{
    class LGripButton : PositionedObject
    {

        #region Fields

        private AxisAlignedRectangle frbCollision;

        private Sprite frbButton;

        private bool gripActive;

        #endregion

        #region Constructor

        public LGripButton(string contentManagerName)
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
            CreateVisual();
            gripActive = false;

        }


        public void Activity()
        {
            X = SpriteManager.Camera.X - 55;
            Y = SpriteManager.Camera.Y - 32;

            SpriteManager.RemoveSprite(frbButton);


            if (gripActive == true)
            {
                SetVisualDown();
            }
            else
            {
                SetVisualUp();
            }

        }

        public void Destroy()
        {
            SpriteManager.RemoveSprite(frbButton);
            ShapeManager.Remove(frbCollision);
            SpriteManager.RemovePositionedObject(this);
        }

        public void setActive(bool b)
        {
            gripActive = b;
        }

        #endregion

        #region Private Methods

        private void CreateCollision()
        {
            frbCollision = ShapeManager.AddAxisAlignedRectangle();
            frbCollision.Visible = false;
            frbCollision.ScaleX = 5;
            frbCollision.ScaleY = 5;
            frbCollision.AttachTo(this, false);
        }

        private void CreateVisual()
        {

            frbButton = SpriteManager.AddSprite("Content/lgripup.png");
            frbButton.Position = this.Position;
            frbButton.ScaleX = 5;
            frbButton.ScaleY = 5;
            frbButton.AttachTo(this, false);
        }

        private void SetVisualUp()
        {

            frbButton = SpriteManager.AddSprite("Content/lgripup.png");
            frbButton.Position = this.Position;
            frbButton.ScaleX = 5;
            frbButton.ScaleY = 5;
            frbButton.AttachTo(this, false);
        }

        private void SetVisualDown()
        {

            frbButton = SpriteManager.AddSprite("Content/lgripdown.png");
            frbButton.Position = this.Position;
            frbButton.ScaleX = 5;
            frbButton.ScaleY = 5;
            frbButton.AttachTo(this, false);
        }

        #endregion
    }
}