using System;
using System.Collections.Generic;

using FlatRedBall;
using FlatRedBall.Graphics;
using FlatRedBall.Utilities;
using FlatRedBall.Math;
using FlatRedBall.Math.Geometry;

using KamiClimber.Screens;
using Microsoft.Xna.Framework;

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Common;
using FarseerPhysics.Collision.Shapes;


#if !FRB_MDX
using System.Linq;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
#endif

namespace KamiClimber.Entities
{
    class Platform : PositionedObject
    {
        World farWorld;

        Body farPlatform;

        Polygon frbPlatform;

        public Platform(World world, Vector2 size, Vector2 pos)
        {
            farWorld = world;
            farPlatform = BodyFactory.CreateRectangle(farWorld, size.X*2, size.Y*2, 1, pos);
            farPlatform.Restitution = 1f;
            farPlatform.Rotation = 0f;
            // Dynamic objects respond to gravity.
            farPlatform.IsStatic = true;

            frbPlatform = Polygon.CreateRectangle(size.X, size.Y);
            frbPlatform.X = farPlatform.Position.X;
            frbPlatform.Y = farPlatform.Position.Y;
            frbPlatform.RotationZ = farPlatform.Rotation;
            ShapeManager.AddPolygon(frbPlatform);
        }
    }
}
