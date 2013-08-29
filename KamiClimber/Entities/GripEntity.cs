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
    class GripEntity : PositionedObject
    {

        #region Fields

        private World farWorld;

        private Body farGrip;

        private Circle frbCollision;

        private bool isActive;

        private bool isTimed;

        private double endTime;

        private Text counter;

        private double timeLeft;

        private double holdStrength;

        #endregion

        #region Constructor

        public GripEntity(string contentManagerName, World world)
        {
            farWorld = world;
            SpriteManager.AddPositionedObject(this);

        }



        #endregion

        #region Private Methods

        private void CreateCollision()
        {
            frbCollision = ShapeManager.AddCircle();
            frbCollision.Radius = 1f;
            frbCollision.AttachTo(this, false);
        }

        // >>> Creating grip as a static body will make climber's hands collide and 'stick' to outside of it.
        // >>> Game seems more forgiving when hands latch to the center of the grip
        // >>> So creating it as a Farseer body was removed from constructor.
        private void CreateBody()
        {
            farGrip = BodyFactory.CreateCircle(farWorld, 1f, 1f, new Vector2(this.X, this.Y));
            farGrip.Restitution = 1f;
            farGrip.BodyType = BodyType.Static;
        }

        #endregion Public Methods

        #region Public Methods

        public Circle Collision
        {
            get { return frbCollision; }
        }


        public Body getBody
        {

            get { return farGrip; }
        }

        public double getTimeLeft
        {

            get { return timeLeft; }
        }

        public void setActive(bool b, double d)
        {
            isActive = b;
            endTime = d + holdStrength;

        }

        public bool checkIsActive
        {
            get { return isActive; }

        }

        public void Initialize(Vector2 pos, bool b, double d)
        {

            isTimed = b;
            endTime = -1f;
            holdStrength = d;
            this.X = pos.X;
            this.Y = pos.Y;
            CreateCollision();
            isActive = false;
            timeLeft = 999;
            // CreateBody();


        }

        public void Activity()
        {
            if (isTimed == true)
            {
                if (isActive == true)
                {
                    timeLeft = endTime - TimeManager.CurrentTime;
                    TextManager.RemoveText(counter);
                    counter = TextManager.AddText("" + (int)(timeLeft));
                    counter.Spacing = 2f;
                    counter.Scale = 2.5f;
                    counter.X = this.X;
                    counter.Y = this.Y + 5;

                    if (timeLeft < 0)
                    {
                        TextManager.RemoveText(counter);
                        ShapeManager.Remove(frbCollision);
                    }
                    if (timeLeft < -8)
                    {
                        endTime = -1;
                        timeLeft = 999;
                        isActive = false;
                        CreateCollision();
                        
                    }
                }
            }
                    
        }

        public void Destroy()
        {
            if (counter != null)
            {
                counter.RemoveSelfFromListsBelongingTo();
            }
            if (frbCollision != null)
            {
                ShapeManager.Remove(frbCollision);
            }
            SpriteManager.RemovePositionedObject(this);
        }

        #endregion


    }
}