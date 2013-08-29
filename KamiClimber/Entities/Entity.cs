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
    class Entity : PositionedObject
    {

        #region Fields

        #endregion

        #region Constructor

        public Entity()
        {

        }

        #endregion

        #region Private Methods

        #endregion Public Methods

        #region Public Methods

        public override void Initialize()
        {
            

        }

        public void Activity()
        {
        }

        public void Destroy()
        {
            
        }

        #endregion


    }
}
