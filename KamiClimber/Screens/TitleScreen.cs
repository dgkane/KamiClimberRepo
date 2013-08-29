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

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Common;
using FarseerPhysics.Collision.Shapes;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
// using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

using KamiClimber.Screens;
using KamiClimber.Entities;
using KamiClimber.GUI;

namespace KamiClimber.Screens
{

    // *** Gameplay screen ***
    //
    // Handles entity creation and game logic

    public class TitleScreen : Screen
    {

        private Text title;
        private Text instruction;

        public TitleScreen()
            : base("TitleScreen")
        {
            // Don't put initialization code here, do it in
            // the Initialize method below
            //   |   |   |   |   |   |   |   |   |   |   |
            //   |   |   |   |   |   |   |   |   |   |   |
            //   V   V   V   V   V   V   V   V   V   V   V

        }

        public override void Initialize(bool addToManagers)
        {

            // AddToManagers should be called LAST in this method:
            if (addToManagers)
            {
                AddToManagers();
            }

        }

        public override void Activity(bool firstTimeCalled)
        {
            base.Activity(firstTimeCalled);

            TextManager.RemoveText(title);
            title = TextManager.AddText("Kamikaze Climber");
            title.Spacing = 5f;
            title.NewLineDistance = 10f;
            title.Scale = 5f;
            title.Alpha = 0.75f;
            title.X = -35;
            title.Y = 10;

            TextManager.RemoveText(instruction);
            instruction = TextManager.AddText("Press Enter to start");
            instruction.Spacing = 3f;
            instruction.NewLineDistance = 10f;
            instruction.Scale = 3f;
            instruction.Alpha = 0.5f;
            instruction.X = -25;
            instruction.Y = -5;

            if ((InputManager.Keyboard.KeyPushed(Keys.Enter)))
            {
                MoveToScreen(typeof(KamiClimber.Screens.GameScreen).FullName);
            }

            // foreach (TouchLocation tl in touchLocations)
            // {
            //     if (tl.State == TouchLocationState.Pressed || tl.State == TouchLocationState.Moved)
            //     {
            //         MoveToScreen(typeof(KamiClimber.Screens.TitleScreen).FullName);
            //     }
            //     i++;
            // }

        }

        public override void Destroy()
        {
            title.RemoveSelfFromListsBelongingTo();
            instruction.RemoveSelfFromListsBelongingTo();
            base.Destroy();


        }


    }
}