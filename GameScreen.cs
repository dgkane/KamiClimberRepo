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
using FlatRedBall.Screens;

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
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

using KamiClimberPhone.Screens;
using KamiClimberPhone.Entities;
using KamiClimberPhone.GUI;

namespace KamiClimberPhone.Screens
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
        //
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
        //
        //
        private CheckpointEntity frbCheckpoint1;
        //
        private Platform platform;
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
        private AxisAlignedRectangle exertionMeter;
        private Text Strength;
        private Text okText;
        private Text timer;
        // >>> Displays information on TouchLocations
        // private Text information;
        //
        private RGripButton frbRGripButton;
        private LGripButton frbLGripButton;
        private RSwingButton frbRSwingButton;
        private LSwingButton frbLSwingButton;
        private PullUpButton frbPullUpButton;
        //
        // >>> Array of touch locations
        private Vector2[] touchloc;
        //
        // >>> 4 TouchPoints; user can use up to 4 fingers to control Climber
        private TouchPoint frbTouchPoint1;
        private TouchPoint frbTouchPoint2;
        private TouchPoint frbTouchPoint3;
        private TouchPoint frbTouchPoint4;
        private PositionedObjectList<TouchPoint> frbTouchPoints;
        //
        // >>> To give first touch priority on direction of reach
        private bool isReaching;
        private Vector2 priorityReachPoint;
        //
        private TutorialBubble frbTut;

        private float timeLimit;
        //
        // ***

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
            timeLimit = 200;

            #region farseer_world_init

            // *** Farseer world initialisation ***
            //
            float gravity = -10;
            farWorld = new World(new Vector2(0f, gravity));
            //
            // ***

            #endregion



            #region ui_init



            // *** UI initialisation ***
            //
            touchloc = new Vector2[4];
            //
            frbTouchPoints = new PositionedObjectList<TouchPoint>();
            frbTouchPoint1 = new TouchPoint("TouchPointSprite1");
            frbTouchPoint1.Initialize();
            frbTouchPoints.Add(frbTouchPoint1);
            frbTouchPoint2 = new TouchPoint("TouchPointSprite2");
            frbTouchPoint2.Initialize();
            frbTouchPoints.Add(frbTouchPoint2);
            frbTouchPoint3 = new TouchPoint("TouchPointSprite3");
            frbTouchPoint3.Initialize();
            frbTouchPoints.Add(frbTouchPoint3);
            frbTouchPoint4 = new TouchPoint("TouchPointSprite4");
            frbTouchPoint4.Initialize();
            frbTouchPoints.Add(frbTouchPoint4);
            //
            // *** WP control buttons ***
            //
            frbRGripButton = new RGripButton("RGripButtonSprite");
            frbRGripButton.Initialize();
            frbLGripButton = new LGripButton("LGripButtonSprite");
            frbLGripButton.Initialize();
            //
            frbRSwingButton = new RSwingButton("RSwingButtonSprite");
            frbRSwingButton.Initialize();
            frbLSwingButton = new LSwingButton("LSwingButtonSprite");
            frbLSwingButton.Initialize();
            //
            frbPullUpButton = new PullUpButton("PullUpButtonSprite");
            frbPullUpButton.Initialize();
            //
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
            frbGrip12.Initialize(new Vector2(45f, 85f), false, 15);
            frbGrips.Add(frbGrip12);
            //
            frbGrip13 = new GripEntity("GripSprite13", farWorld);
            frbGrip13.Initialize(new Vector2(60f, 85f), false, 15);
            frbGrips.Add(frbGrip13);
            //
            frbGrip14 = new GripEntity("GripSprite14", farWorld);
            frbGrip14.Initialize(new Vector2(75f, 85f), false, 8);
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
            //
            frbCheckpoints = new PositionedObjectList<CheckpointEntity>();
            //
            frbCheckpoint1 = new CheckpointEntity("CheckPointSprite1");
            frbCheckpoint1.Initialize(new Vector2(82.5f, 82.5f), 120f);
            frbCheckpoints.Add(frbCheckpoint1);
            //

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


           


            // ***  ***
            //
            touchloc[0] = new Vector2(-99, -99);
            touchloc[1] = new Vector2(-99, -99);
            touchloc[2] = new Vector2(-99, -99);
            touchloc[3] = new Vector2(-99, -99);
            //
            TouchCollection touchLocations = TouchPanel.GetState();
            //
            int i = 0;
            //
            // *** If screen is being touched, replace locations of touchpoints in 'touchloc' Vector2 array (TouchPoint will be drawn at these positions) ***
            //
            foreach (TouchLocation tl in touchLocations)
            {
                if (tl.State == TouchLocationState.Pressed || tl.State == TouchLocationState.Moved)
                {
                    touchloc[i] = tl.Position;
                }
                i++;
            }
            //
            // ***


            


            // >>> Needs removing for WP
            getInput();


            #region invoke_entity_activity



            // frbCursor.Activity();

            frbRGripButton.Activity();
            frbLGripButton.Activity();

            frbRSwingButton.Activity();
            frbLSwingButton.Activity();

            frbPullUpButton.Activity();



            frbTouchPoint1.Activity(touchloc[0]);
            frbTouchPoint2.Activity(touchloc[1]);
            frbTouchPoint3.Activity(touchloc[2]);
            frbTouchPoint4.Activity(touchloc[3]);

            frbClimber.Activity();

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
                    frbClimber.TorsoBody.ApplyForce(new Vector2(0, 20));
                    Vector2 vec = new Vector2(priorityReachPoint.X - frbClimber.LHandCollision.X, priorityReachPoint.Y - frbClimber.LHandCollision.Y);
                    vec.Normalize();
                    frbClimber.LHandBody.LinearVelocity = vec * 20;
                }
            }
            //
            if (frbClimber.getRHandGrip == false && frbClimber.getLHandGrip == true)
            {
                if (frbClimber.getReach == true)
                {
                    frbClimber.TorsoBody.ApplyForce(new Vector2(0, 20));
                    Vector2 vec = new Vector2(priorityReachPoint.X - frbClimber.RHandCollision.X, priorityReachPoint.Y - frbClimber.RHandCollision.Y);
                    vec.Normalize();
                    frbClimber.RHandBody.LinearVelocity = vec * 40;
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
            // *** Update HUD each frame so it stays relative to camera position ***
            //
            TextManager.RemoveText(Strength);
            Strength = TextManager.AddText("Strength : ");
            Strength.Spacing = 2f;
            Strength.Scale = 2.5f;
            Strength.X = SpriteManager.Camera.X - 65;
            Strength.Y = SpriteManager.Camera.Y + 36;
            //
            // *** Get climber's current 'exertion' value to inform length of UI meter ***
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
            // *** Display 'OK!' if exertion meter has recharged and climber can 'pull up' again ***
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
            // *** Display time remaining to reach next checkpoint ***
            //
            TextManager.RemoveText(timer);
            timer = TextManager.AddText("Checkpoint : " + (int)(timeLimit - TimeManager.CurrentTime));
            timer.Spacing = 2f;
            timer.Scale = 2.5f;
            timer.X = SpriteManager.Camera.X + 38;
            timer.Y = SpriteManager.Camera.Y + 36;
            //
            // *** Information overlay - displays position (pixels) of touch points ***
            //
            // TextManager.RemoveText(information);
            // information = TextManager.AddText("" + touchloc[0].X + " ," + touchloc[0].Y + ".. " + touchloc[1].X + " ," + touchloc[1].Y + ".. " + touchloc[2].X + " ," + touchloc[2].Y + ".. " + touchloc[3].X + " ," + touchloc[3].Y + ".. ");
            // information.Spacing = 2f;
            // information.NewLineDistance = 4f;
            // information.Scale = 2.5f;
            // information.Alpha = 0.5f;
            // information.X = SpriteManager.Camera.X - 65;
            // information.Y = SpriteManager.Camera.Y - 32;
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
            // Check for death
            if (frbClimber.getClimberCentre.Y < -100)
            {

                MoveToScreen(typeof(KamiClimberPhone.Screens.GameOverScreen).FullName);

                
            }
            //
            frbTut.Activity();
            // ***

            #endregion


        }


        public override void Destroy()
        {
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
            frbLSwingButton.Destroy();
            frbPullUpButton.Destroy();
            frbRGripButton.Destroy();
            frbRSwingButton.Destroy();
            frbTouchPoint1.Destroy();
            frbTouchPoint2.Destroy();
            frbTouchPoint3.Destroy();
            frbTouchPoint4.Destroy();
            ShapeManager.Remove(exertionMeter);
            SpriteManager.RemovePositionedObject(exertionMeter);
            Strength.RemoveSelfFromListsBelongingTo();
            okText.RemoveSelfFromListsBelongingTo();
            timer.RemoveSelfFromListsBelongingTo();
            frbTut.Destroy();
            frbCheckpoint1.Destroy();

            base.Destroy();

        }


        // *** Checks for user input and performs actions / changes Climber state accordingly ***
        // 
        private void getInput()
        {
            
            isReaching = false;
            frbClimber.setSwingRight(false);
            frbClimber.setSwingLeft(false);
            frbClimber.setPullUp(false);
            frbClimber.setReach(false);
            frbPullUpButton.setPressed(false);
            frbLSwingButton.setPressed(false);
            frbRSwingButton.setPressed(false);

            foreach (TouchPoint tp in frbTouchPoints)
            {
                if (tp.getIsActive == true)
                {
                    if (tp.Collision.CollideAgainst(frbRSwingButton.Collision))
                    {
                        frbRSwingButton.setPressed(true);
                        frbClimber.setSwingRight(true);
                    }
                    else if (tp.Collision.CollideAgainst(frbLSwingButton.Collision))
                    {
                        frbLSwingButton.setPressed(true);
                        frbClimber.setSwingLeft(true);
                    }
                    else if (tp.Collision.CollideAgainst(frbPullUpButton.Collision))
                    {
                        frbPullUpButton.setPressed(true);
                        frbClimber.setPullUp(true);
                    }
                    else if (tp.Collision.CollideAgainst(frbRGripButton.Collision))
                    {
                        frbClimber.RHandRelease();
                    }
                    else if (tp.Collision.CollideAgainst(frbLGripButton.Collision))
                    {
                        frbClimber.LHandRelease();
                    }
                    else
                    {
                        // *** Gives the first TouchPoint not pressing a UI button priority as the reach vector. ***
                        //
                        if (isReaching == false)
                        {
                            priorityReachPoint.X = tp.X;
                            priorityReachPoint.Y = tp.Y;
                            isReaching = true;
                            frbClimber.setReach(true);
                        }
                    }
                }
            }



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