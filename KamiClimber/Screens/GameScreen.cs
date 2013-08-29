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

    public class GameScreen : Screen
    {


        #region fields

        World farWorld;


        #region entities

        // *** Game entities - characters, props ***
        //
        private ClimberEntity frbClimber;

        private GripEntity frbGrip1;
        private GripEntity frbGrip2;
        private GripEntity frbGrip3;
        private GripEntity frbGrip4;
        private GripEntity frbGrip5;
        private GripEntity frbGrip6;
        private GripEntity frbGrip7;
        private GripEntity frbGrip8;
        private GripEntity frbGrip9;
        private GripEntity frbGrip10;
        private GripEntity frbGrip11;
        private GripEntity frbGrip12;
        private GripEntity frbGrip13;
        private GripEntity frbGrip14;
        private GripEntity frbGrip15;

        private CheckpointEntity frbCheckpoint1;

        // private Platform platform;
        //
        // ***

        #endregion


        #region entity_collections

        // *** Collections for entities used multiple times ***
        //
        private PositionedObjectList<GripEntity> frbGrips;
        private PositionedObjectList<CheckpointEntity> frbCheckpoints;
        //
        // ***

        #endregion


        #region ui_elements

        // *** UI elements ***
        //

        private CursorEntity frbCursor;

        private AxisAlignedRectangle exertionMeter;

        private RGripButton frbRGripButton;
        private LGripButton frbLGripButton;

        private Text Strength;

        private Text okText;

        private Text timer;

        private Text instructions;

        private TutorialBubble frbTut;
        private int TutorialPhase;

        // private RGripButton frbRGripButton;
        // private LGripButton frbLGripButton;

        // private RSwingButton frbRSwingButton;
        // private LSwingButton frbLSwingButton;

        // private PullUpButton frbPullUpButton;

        private float timeLimit;
        
        //

        #endregion

        #endregion

        public GameScreen()
            : base("GameScreen")
        {
            // Don't put initialization code here, do it in
            // the Initialize method below
            //   |   |   |   |   |   |   |   |   |   |   |
            //   |   |   |   |   |   |   |   |   |   |   |
            //   V   V   V   V   V   V   V   V   V   V   V

        }


        // *** Set initial game state ***
        public override void Initialize(bool addToManagers)
        {
            // Set the screen up here instead of in the Constructor to avoid
            // exceptions occurring during the constructor.

            TimeManager.CurrentTime = 0f;
            timeLimit = 200f;

            #region farseer_world_init

            // *** Farseer world initialisation ***
            //
            float gravity = -10;
            farWorld = new World(new Vector2(0f, gravity));
            //
            // ***

            #endregion


            // >>> Remove cursor for WP.
            #region ui_init

            // *** UI initialisation ***
            //
            // *** Cursor *** (not required for WP)
            //
            frbCursor = new CursorEntity("CursorSprite");
            frbCursor.Initialize();
            //
            frbRGripButton = new RGripButton("RGripButtonSprite");
            frbRGripButton.Initialize();
            frbLGripButton = new LGripButton("LGripButtonSprite");
            frbLGripButton.Initialize();
            //
            // frbRSwingButton = new RSwingButton("RSwingButtonSprite");
            //frbRSwingButton.Initialize();
            // frbLSwingButton = new LSwingButton("LSwingButtonSprite");
            // frbLSwingButton.Initialize();

            // frbPullUpButton = new PullUpButton("PullUpButtonSprite");
            // frbPullUpButton.Initialize();

            frbTut = new TutorialBubble("TutorialBubble");
            frbTut.Initialize();
        
            // ***

            #endregion


            #region entity_init

            // *** Create & initialise entities for game start ***
            //
            // *** Player character ***
            //
            frbClimber = new ClimberEntity(farWorld, new Vector2(0f, 0f));
            frbClimber.Initialize();
            //
            //
            #region grip_entites

            // *** Grips ***
            frbGrips = new PositionedObjectList<GripEntity>();
            //
            frbGrip1 = new GripEntity("GripSprite1", farWorld);
            frbGrip1.Initialize(new Vector2(-10f, 0f), false, 0);
            frbGrips.Add(frbGrip1);
            //
            frbGrip2 = new GripEntity("GripSprite2", farWorld);
            frbGrip2.Initialize(new Vector2(5f, 0f), false, 0);
            frbGrips.Add(frbGrip2);
            //
            frbGrip3 = new GripEntity("GripSprite3", farWorld);
            frbGrip3.Initialize(new Vector2(20f, 0f), false, 0);
            frbGrips.Add(frbGrip3);
            //
            frbGrip4 = new GripEntity("GripSprite4", farWorld);
            frbGrip4.Initialize(new Vector2(30f, 5f), false, 0);
            frbGrips.Add(frbGrip4);
            //
            frbGrip5 = new GripEntity("GripSprite5", farWorld);
            frbGrip5.Initialize(new Vector2(40f, 10f), false, 0);
            frbGrips.Add(frbGrip5);
            //
            frbGrip6 = new GripEntity("GripSprite6", farWorld);
            frbGrip6.Initialize(new Vector2(30f, 15f), false, 0);
            frbGrips.Add(frbGrip6);
            //
            frbGrip7 = new GripEntity("GripSprite7", farWorld);
            frbGrip7.Initialize(new Vector2(40f, 20f), false, 0);
            frbGrips.Add(frbGrip7);
            //
            frbGrip8 = new GripEntity("GripSprite8", farWorld);
            frbGrip8.Initialize(new Vector2(45f, 30f), false, 0);
            frbGrips.Add(frbGrip8);
            //
            frbGrip9 = new GripEntity("GripSprite9", farWorld);
            frbGrip9.Initialize(new Vector2(40f, 40f), false, 0);
            frbGrips.Add(frbGrip9);
            //
            frbGrip10 = new GripEntity("GripSprite10", farWorld);
            frbGrip10.Initialize(new Vector2(45f, 55f), false, 0);
            frbGrips.Add(frbGrip10);
            //
            frbGrip11 = new GripEntity("GripSprite11", farWorld);
            frbGrip11.Initialize(new Vector2(40f, 70f), false, 0);
            frbGrips.Add(frbGrip11);
            //
            frbGrip12 = new GripEntity("GripSprite12", farWorld);
            frbGrip12.Initialize(new Vector2(45f, 85f), true, 15);
            frbGrips.Add(frbGrip12);
            //
            frbGrip13 = new GripEntity("GripSprite13", farWorld);
            frbGrip13.Initialize(new Vector2(60f, 85f), true, 15);
            frbGrips.Add(frbGrip13);
            //
            frbGrip14 = new GripEntity("GripSprite14", farWorld);
            frbGrip14.Initialize(new Vector2(75f, 85f), true, 8);
            frbGrips.Add(frbGrip14);
            //
            frbGrip15 = new GripEntity("GripSprite15", farWorld);
            frbGrip15.Initialize(new Vector2(90f, 85f), false, 0);
            frbGrips.Add(frbGrip15);
            //

            #endregion
            //
            //
            // *** Scenery ***
            //
            // platform = new Platform(farWorld, new Vector2(100f, 1.5f), new Vector2(0f, -40f));
            //
            frbCheckpoints = new PositionedObjectList<CheckpointEntity>();
            //
            frbCheckpoint1 = new CheckpointEntity("CheckPointSprite1");
            frbCheckpoint1.Initialize(new Vector2(82.5f, 82.5f), 120f);
            frbCheckpoints.Add(frbCheckpoint1);

            #endregion


            // AddToManagers should be called LAST in this method:
            if (addToManagers)
            {
                AddToManagers();
            }

        }


        public override void AddToManagers()
        {


        }


        // *** Updates game screen each frame based on entity activity, user input ***
        public override void Activity(bool firstTimeCalled)
        {
            base.Activity(firstTimeCalled);


            // Farseer : step (update) world; rate defined in Game1.cs
            //
            farWorld.Step(TimeManager.SecondDifference);


            getKeyboard();


            #region invoke_entity_activity

            frbRGripButton.Activity();
            frbLGripButton.Activity();

            frbClimber.Activity();

            frbCursor.Activity();

            foreach(GripEntity grip in frbGrips)
            {
                grip.Activity();
            }

            // frbRGripButton.Activity();
            // frbLGripButton.Activity();

            // frbRSwingButton.Activity();
            // frbLSwingButton.Activity();

            // frbPullUpButton.Activity();


            #endregion


            // ***
            //
            // Check whether each of climber's hands is in the 'release' state
            //
            // If so, they are open to attaching to a grip on collision
            //
            if (frbClimber.getRHandFree == true)
            // {
            //     frbClimber.setRHandGrip(false, frbClimber.RHandBody.Position);
            // }
            // else
            {
                frbRGripButton.setActive(false);
                foreach (GripEntity grip in frbGrips)
                {
                    if (frbClimber.RHandCollision.CollideAgainst(grip.Collision))
                    {
                        if (grip.checkIsActive == false)
                        {
                            grip.setActive(true, TimeManager.CurrentTime);
                        }
                        frbClimber.setRHandGrip(true, new Vector2(grip.Position.X, grip.Position.Y));
                        frbClimber.setRHandFree(false);
                        frbRGripButton.setActive(true);
                        if (grip.getTimeLeft < 0)
                        {
                            frbClimber.RHandRelease();
                        }
                    }
                }
            }
            //
            if (frbClimber.getLHandFree == true)
            // {
            //     frbClimber.setLHandGrip(false, frbClimber.LHandBody.Position);
            // }
            // else
            {
                frbLGripButton.setActive(false);
                foreach (GripEntity grip in frbGrips)
                {
                    if (frbClimber.LHandCollision.CollideAgainst(grip.Collision))
                    {
                        if (grip.checkIsActive == false)
                        {
                            grip.setActive(true, TimeManager.CurrentTime);
                        }
                        grip.setActive(true, TimeManager.CurrentTime);
                        frbClimber.setLHandGrip(true, new Vector2(grip.Position.X, grip.Position.Y));
                        frbClimber.setLHandFree(false);
                        frbLGripButton.setActive(true);
                        if (grip.getTimeLeft < 0)
                        {
                            frbClimber.LHandRelease();
                        }
                    }
                }
            }
            //
            // ***


            // >> Perhaps incorporate into entity class?
            //
            // ***
            //
            // Check if climber's left/right swinging property is true
            //
            // If so, apply a force to body in respective direction
            //
            if (frbClimber.getSwingLeft == true)
            {
                frbClimber.TorsoBody.ApplyForce(new Vector2(-50, 0));
            }
            //
            if (frbClimber.getSwingRight == true)
            {
                frbClimber.TorsoBody.ApplyForce(new Vector2(50, 0));
            }
            //
            //
            // Check if climber's pulling up property is true
            //
            // If so, apply upward force to body
            //
            if (frbClimber.getPullUp == true && frbClimber.getIsRecharging == false)
            {
                frbClimber.TorsoBody.ApplyForce(new Vector2(0, 100));
            }
            //


            // >>> This will need to be modified for touch input where two buttons are not available for L/R hand reach
            // >>> Possibly incorporate into entity class
            //
            // ***
            //
            // If one of climber's hands 'grip' property is true,
            //
            // and the opposite hand's 'reach' property is true,
            //
            // Apply a linear velocity to that hand
            //
            //
            // First 'if' condition necessary?
            if (frbClimber.getRHandGrip == true && frbClimber.getLHandGrip == false)
            {
                if (frbClimber.getReach == true)
                {
                    frbClimber.TorsoBody.ApplyForce(new Vector2(-20, 20));
                    Vector2 vec = new Vector2(frbCursor.X - frbClimber.LHandCollision.X, frbCursor.Y - frbClimber.LHandCollision.Y);
                    vec.Normalize();
                    frbClimber.LHandBody.LinearVelocity = vec * 20;
                }
            }
            //
            if (frbClimber.getRHandGrip == false && frbClimber.getLHandGrip == true)
            {
                if (frbClimber.getReach == true)
                {
                    frbClimber.TorsoBody.ApplyForce(new Vector2(20, 20));
                    Vector2 vec = new Vector2(frbCursor.X - frbClimber.RHandCollision.X, frbCursor.Y - frbClimber.RHandCollision.Y);
                    vec.Normalize();
                    frbClimber.RHandBody.LinearVelocity = vec * 20;
                }
            }

            foreach (CheckpointEntity cp in frbCheckpoints)
            {
                if (frbClimber.TorsoCollision.CollideAgainst(cp.Collision))
                {
                    float f = cp.getTimeBonus;
                    timeLimit += f;
                    cp.Destroy();
                }
            }

            // if (frbClimber.getRHandGrip == true && frbClimber.getLHandGrip == true)
            // {
            //     frbClimber.setRHandGrip(false, new Vector2(0, 0));
            //     frbClimber.RHandBody.LinearVelocity = new Vector2(frbCursor.X - frbClimber.RHandCollision.X, frbCursor.Y - frbClimber.RHandCollision.Y);
            // 
            // }
            

            #region ui_operations

            // *** UI operations ***
            //
            // Center view on center of climber's body
            //
            SpriteManager.Camera.X = frbClimber.getClimberCentre.X;
            if (frbClimber.getClimberCentre.Y > -50)
            {
                SpriteManager.Camera.Y = frbClimber.getClimberCentre.Y;
            }
            //
            //
            // *** Update HUD each frame so it stays relative to camera position ***
            //
            TextManager.RemoveText(Strength);
            Strength = TextManager.AddText("Strength : ");
            Strength.Spacing = 2f;
            Strength.Scale = 2.5f;
            Strength.X = SpriteManager.Camera.X - 65;
            Strength.Y = SpriteManager.Camera.Y + 36;
            //
            //
            // Get climber's current 'exertion' value to inform length of UI meter
            //
            if (exertionMeter != null)
            {
                ShapeManager.Remove(exertionMeter);
            }
            exertionMeter = new AxisAlignedRectangle();
            exertionMeter.Y = SpriteManager.Camera.Y + 36;
            exertionMeter.X = SpriteManager.Camera.X - 38;
            exertionMeter.ScaleY = 2;
            exertionMeter.ScaleX = 8 - (frbClimber.getExertion / 30);
            ShapeManager.AddAxisAlignedRectangle(exertionMeter);
            //
            //
            // Display 'OK!' if exertion meter has recharged and climber can 'pull up' again
            //
            TextManager.RemoveText(okText);
            if (frbClimber.getIsExerting == false && frbClimber.getIsRecharging == false)
            {
                okText = TextManager.AddText("O K !");
                okText.Spacing = 2f;
                okText.Scale = 2.5f;
                okText.X = SpriteManager.Camera.X - 26;
                okText.Y = SpriteManager.Camera.Y + 36;
            }
            //
            //
            // Display time remaining to reach next checkpoint
            //
            TextManager.RemoveText(timer);
            timer = TextManager.AddText("Checkpoint : " + (int)(timeLimit - TimeManager.CurrentTime));
            timer.Spacing = 2f;
            timer.Scale = 2.5f;
            timer.X = SpriteManager.Camera.X + 38;
            timer.Y = SpriteManager.Camera.Y + 36;
            //
            //
            // Instruction overlay
            //
            // >>> To be replaced with help screen
            // TextManager.RemoveText(instructions);
            // instructions = TextManager.AddText("Arrow keys swing left and right and pull up.\nQ and W release left and right hand from grips.\nLMB and RMB reach with left and right hand.");
            // instructions.Spacing = 2f;
            // instructions.NewLineDistance = 4f;
            // instructions.Scale = 2.5f;
            // instructions.Alpha = 0.5f;
            // instructions.X = SpriteManager.Camera.X - 65;
            // instructions.Y = SpriteManager.Camera.Y - 32;
            //
            if (frbClimber.RHandCollision.CollideAgainst(frbGrip2.Collision))
            {
                frbTut.setPhase(2);
            }
            if (frbClimber.RHandCollision.CollideAgainst(frbGrip2.Collision) && frbClimber.LHandCollision.CollideAgainst(frbGrip2.Collision))
            {
                frbTut.setPhase(3);
            }
            if (frbClimber.RHandCollision.CollideAgainst(frbGrip3.Collision) && frbClimber.LHandCollision.CollideAgainst(frbGrip3.Collision))
            {
                frbTut.setPhase(4);
            }
            if (frbClimber.RHandCollision.CollideAgainst(frbGrip4.Collision))
            {
                frbTut.setPhase(5);
            }
            if (frbClimber.RHandCollision.CollideAgainst(frbGrip4.Collision) && frbClimber.LHandCollision.CollideAgainst(frbGrip4.Collision))
            {
                frbTut.setPhase(6);
            }
            //
            if (frbClimber.getClimberCentre.Y < -100)
            {


                this.Destroy();

            }

            frbTut.Activity();
            
            // ***

            #endregion


        }

        private void setTimeLimit(float f)
        {
            timeLimit += f;
        }



        public override void Destroy()
        {
            frbCursor.Destroy();
            frbClimber.Destroy();
            frbGrip1.Destroy();
            frbGrip2.Destroy();
            frbGrip3.Destroy();
            frbGrip4.Destroy();
            frbGrip5.Destroy();
            frbGrip6.Destroy();
            frbGrip7.Destroy();
            frbGrip8.Destroy();
            frbGrip9.Destroy();
            frbGrip10.Destroy();
            frbGrip11.Destroy();
            frbGrip12.Destroy();
            frbGrip13.Destroy();
            frbGrip14.Destroy();
            frbGrip15.Destroy();
            frbLGripButton.Destroy();
            // frbLSwingButton.Destroy();
            // frbPullUpButton.Destroy();
            frbRGripButton.Destroy();
            // frbRSwingButton.Destroy();
            // frbTouchPoint1.Destroy();
            // frbTouchPoint2.Destroy();
            // frbTouchPoint3.Destroy();
            // frbTouchPoint4.Destroy();
            ShapeManager.Remove(exertionMeter);
            SpriteManager.RemovePositionedObject(exertionMeter);
            Strength.RemoveSelfFromListsBelongingTo();
            okText.RemoveSelfFromListsBelongingTo();
            timer.RemoveSelfFromListsBelongingTo();
            frbTut.Destroy();
            frbCheckpoint1.Destroy();


            // base.Destroy();

            MoveToScreen(typeof(KamiClimber.Screens.GameOverScreen).FullName);


        }


        // *** Checks for user input and performs actions / changes Climber state accordingly ***
        // >>> Needs updating to incorporate touch controls
        private void getKeyboard()
        {

            if (InputManager.Keyboard.KeyPushed(Keys.E))
            {
                frbClimber.RHandRelease();
            }
            // else
            // {
            //     frbClimber.setRHandRelease(false);
            // }


            if (InputManager.Keyboard.KeyPushed(Keys.Q))
            {
                frbClimber.LHandRelease();
            }


            // else
            // {
            //     frbClimber.setLHandRelease(false);
            // }


            if (InputManager.Keyboard.KeyDown(Keys.A))
            {
                frbClimber.setSwingLeft(true);
            }


            else
            {
                frbClimber.setSwingLeft(false);
            }


            if (InputManager.Keyboard.KeyDown(Keys.D))
            {
                frbClimber.setSwingRight(true);
            }


            else
            {
                frbClimber.setSwingRight(false);
            }


            if (InputManager.Keyboard.KeyDown(Keys.W))
            {

                frbClimber.setPullUp(true);
            }

            else
            {
                frbClimber.setPullUp(false);
            }

            // if (frbCursor.Collision.CollideAgainst(frbRGripButton.Collision) && InputManager.Mouse.ButtonPushed(FlatRedBall.Input.Mouse.MouseButtons.LeftButton))
            // {
            //     frbClimber.RHandRelease();
            // }
            //
            // if (frbCursor.Collision.CollideAgainst(frbLGripButton.Collision) && InputManager.Mouse.ButtonPushed(FlatRedBall.Input.Mouse.MouseButtons.LeftButton))
            // {
            //     frbClimber.LHandRelease();
            // }

            
            
            if (InputManager.Mouse.ButtonDown(FlatRedBall.Input.Mouse.MouseButtons.LeftButton))
            {
            //     if (frbCursor.Collision.CollideAgainst(frbRSwingButton.Collision))
            //     {
            //         frbClimber.setSwingRight(true);
            //     }
            //     else if (frbCursor.Collision.CollideAgainst(frbLSwingButton.Collision))
            //     {
            //         frbClimber.setSwingLeft(true);
            //     }
            //     else if (frbCursor.Collision.CollideAgainst(frbPullUpButton.Collision))
            //     {
            //         frbClimber.setPullUp(true);
            //     }
            //     else
            //     {
                     frbClimber.setReach(true);
            //     }
            }
            else
            {
            //     frbClimber.setSwingRight(false);
            //     frbClimber.setSwingLeft(false);
            //     frbClimber.setPullUp(false);
                 frbClimber.setReach(false);
            }
            


            // if (InputManager.Mouse.ButtonDown(FlatRedBall.Input.Mouse.MouseButtons.RightButton))
            // {
            //     frbClimber.setRHandReach(true);
            // }
            // else
            // {
            //     frbClimber.setRHandReach(false);
            // }

        }


        // >>> For reference in finding angle between two objects
        // private void SetFiringAngle()
        // { 
        //     float dX = frbCursor.X - frbClimber.LHandCollision.X;
        //     float dY = frbCursor.Y - frbClimber.LHandCollision.Y;
        // 
        //     if (dX != 0 || dY != 0)
        //     {
        //        double angle = Math.Atan2(dY, dX);
        //
        // float angleDifference = (float)MathFunctions.AngleToAngle(PlayerSprite.RotationZ, angle);

        // const float minDifferenceForSmoothing = .04f;

        // if (Math.Abs(angleDifference) < minDifferenceForSmoothing)
        // {
        //       frbClimber.LHandCollision.RotationZ = (float)angle;
        // }
        // else
        // {
        //     const float rotationSpeed = 2.3f; // could raise this for more powerful guns
        //     PlayerSprite.RotationZVelocity = Math.Sign(angleDifference) * rotationSpeed;
        // }
        // }
    }
}