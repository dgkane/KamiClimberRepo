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
    class TutorialBubble : PositionedObject
    {

        #region Fields


        private Sprite frbBubble;

        private Text tut1;

        private int phase;

        #endregion

        #region Constructor

        public TutorialBubble(string contentManagerName)
        {
            SpriteManager.AddPositionedObject(this);
        }

        #endregion

        #region Public Methods

        

        public override void Initialize()
        {
            CreateVisual();
            phase = 1;
        }

        public void Activity()
        {
            X = SpriteManager.Camera.X;
            Y = SpriteManager.Camera.Y + 22;
            if (TimeManager.CurrentTime < 5f && phase == 1)
            {
                TextManager.RemoveText(tut1);
                tut1 = TextManager.AddText("This is the Kamikaze Climber tutorial.\nYou do what I say.");
                tut1.SetColor(0, 0, 0);
                tut1.Spacing = 2f;
                tut1.NewLineDistance = 5f;
                tut1.Scale = 2f;
                tut1.Alpha = 1f;
                tut1.X = SpriteManager.Camera.X - 50;
                tut1.Y = SpriteManager.Camera.Y + 25;
            }
            if (TimeManager.CurrentTime > 5f && phase == 1)
            {
                TextManager.RemoveText(tut1);
                tut1 = TextManager.AddText("Use LEFT MOUSE BUTTON to guide your hand.\nSwing to next grip point. NOW!");
                tut1.SetColor(0, 0, 0);
                tut1.Spacing = 2f;
                tut1.NewLineDistance = 5f;
                tut1.Scale = 2f;
                tut1.Alpha = 1f;
                tut1.X = SpriteManager.Camera.X - 52;
                tut1.Y = SpriteManager.Camera.Y + 25;
            }
            if (phase == 2)
            {
                TextManager.RemoveText(tut1);
                tut1 = TextManager.AddText("Press Q to release left hand.\n Use LEFT MOUSE BUTTON to grab same grip point\nyou just did.");
                tut1.SetColor(0, 0, 0);
                tut1.Spacing = 2f;
                tut1.NewLineDistance = 5f;
                tut1.Scale = 2f;
                tut1.Alpha = 1f;
                tut1.X = SpriteManager.Camera.X - 52;
                tut1.Y = SpriteManager.Camera.Y + 25;
            }
            if (phase == 3)
            {
                TextManager.RemoveText(tut1);
                tut1 = TextManager.AddText("You might run out of steam.\nUse ARROW KEYS to swing left and right.\n Press W to release right hand and swing for next grip point.");
                tut1.SetColor(0, 0, 0);
                tut1.Spacing = 2f;
                tut1.NewLineDistance = 5f;
                tut1.Scale = 2f;
                tut1.Alpha = 1f;
                tut1.X = SpriteManager.Camera.X - 52;
                tut1.Y = SpriteManager.Camera.Y + 26;
            }
            if (phase == 4)
            {
                TextManager.RemoveText(tut1);
                tut1 = TextManager.AddText("This next one too high.\nUse UP arrow key to reach with all your might.");
                tut1.SetColor(0, 0, 0);
                tut1.Spacing = 2f;
                tut1.NewLineDistance = 5f;
                tut1.Scale = 2f;
                tut1.Alpha = 1f;
                tut1.X = SpriteManager.Camera.X - 52;
                tut1.Y = SpriteManager.Camera.Y + 26;
            }
            if (phase == 5)
            {
                TextManager.RemoveText(tut1);
                tut1 = TextManager.AddText("You can only reach up when bar at top of screen is charged.\nAnyway, you get idea. Climb! And get revenge!!");
                tut1.SetColor(0, 0, 0);
                tut1.Spacing = 2f;
                tut1.NewLineDistance = 5f;
                tut1.Scale = 2f;
                tut1.Alpha = 1f;
                tut1.X = SpriteManager.Camera.X - 52;
                tut1.Y = SpriteManager.Camera.Y + 26;
            }
            if (phase == 6)
            {
                this.Destroy();
            }
        }

        public void Destroy()
        {
            SpriteManager.RemoveSprite(frbBubble);
            tut1.RemoveSelfFromListsBelongingTo();
            SpriteManager.RemovePositionedObject(this);


        }

        public void setPhase(int i)
        {
            phase = i;
        }

        #endregion

        #region Private Methods

        private void CreateVisual()
        {

            frbBubble = SpriteManager.AddSprite("Content/tutbubble.png");
            frbBubble.Position = this.Position;
            frbBubble.ScaleX = 60;
            frbBubble.ScaleY = 12;
            frbBubble.AttachTo(this, false);
        }

        #endregion
    }
}