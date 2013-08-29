using System;
using System.Collections.Generic;
using System.Linq;

using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Graphics;
using FlatRedBall.Utilities;
using FlatRedBall.Math;
using FlatRedBall.Math.Geometry;
// >>> N.B WP7 version uses a more up-to-date version of flatredball.dll
// >>> Screen classes now included in dll rather than in the 'Screens' folder.
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

    public class GameOverScreen : Screen
    {

        private Text title;
        private Text instruction;

        public GameOverScreen()
            : base("GameOverScreen")
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
            title = TextManager.AddText("You fell!");
            title.Spacing = 5f;
            title.NewLineDistance = 10f;
            title.Scale = 5f;
            title.Alpha = 0.75f;
            title.X = -35;
            title.Y = 10;

            TextManager.RemoveText(instruction);
            instruction = TextManager.AddText("Press Enter to restart");
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

            // TouchCollection touchLocations = TouchPanel.GetState();
            // 
            // foreach (TouchLocation tl in touchLocations)
            // {
            //     if (tl.State == TouchLocationState.Pressed || tl.State == TouchLocationState.Moved)
            //     {
            //         MoveToScreen(typeof(KamiClimberPhone.Screens.GameScreen).FullName);
            //     }
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