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
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using FarseerPhysics.Common;
using FarseerPhysics.Collision.Shapes;

using KamiClimberPhone.Screens;


namespace KamiClimberPhone.Entities
{
    class ClimberEntity
    {

        #region Fields


        private World farWorld;

        private Vector2 startPos;
        private Vector2 rHandGripPos;
        private Vector2 lHandGripPos;
        private Vector2 climberCentre;

        private int exertion;
        private int exertionStrength;

        private double RHandReleaseComplete;
        private double LHandReleaseComplete;

        #region farseer

        #region bodies

        // *** Farseer fields - bodies making up ragdoll model ***
        //
        // No visual representation - this is handled by FRB polygons
        //
        private Body farTorsoBody;
        private Body farHeadBody;
        private Body farRLowerArmBody;
        private Body farLLowerArmBody;
        private Body farRUpperArmBody;
        private Body farLUpperArmBody;
        private Body farRHand;
        private Body farLHand;
        private Body farRUpperLegBody;
        private Body farLUpperLegBody;
        private Body farRLowerLegBody;
        private Body farLLowerLegBody;
        //
        // ***

        #endregion

        #region joints

        // *** Farseer fields - joints between bodies ***
        //
        private RevoluteJoint jNeck;
        private RevoluteJoint jRShoulder;
        private RevoluteJoint jLShoulder;
        private RevoluteJoint jRElbow;
        private RevoluteJoint jLElbow;
        private RevoluteJoint jRWrist;
        private RevoluteJoint jLWrist;
        private RevoluteJoint jLKnee;
        private RevoluteJoint jRKnee;
        private RevoluteJoint jRHip;
        private RevoluteJoint jLHip;
        private FixedRevoluteJoint jRHandGrip;
        private FixedRevoluteJoint jLHandGrip;
        // ***

        #endregion

        #endregion

        private PositionedObjectList<Polygon> frbBodyPartsPolygonList;
        private PositionedObjectList<Circle> frbBodyPartsCircleList;
        private PositionedObjectList<Circle> frbBodyPartsRectList;

        #region frb_shapes

        // *** Shapes handled by FRB ***
        //
        // Correspond to Farseer bodies, used for collisions.
        //
        private Polygon frbTorsoPolygon;
        private Polygon frbRLowerArmPolygon;
        private Polygon frbLLowerArmPolygon;
        private Polygon frbRUpperArmPolygon;
        private Polygon frbLUpperArmPolygon;
        private Polygon frbRUpperLegPolygon;
        private Polygon frbLUpperLegPolygon;
        private Polygon frbRLowerLegPolygon;
        private Polygon frbLLowerLegPolygon;
        //
        private Circle frbHeadCircle;
        private Circle frbRHandCircle;
        private Circle frbLHandCircle;
        //
        // ***

        #endregion


        // *** Climber states ***
        private bool RHandGrip;
        private bool LHandGrip;
        //
        private bool RHandFree;
        private bool LHandFree;
        //
        private bool Reach;
        //private bool RHandReach;
        //private bool LHandReach;
        //
        private bool SwingLeft;
        private bool SwingRight;
        private bool PullUp;
        private bool isExerting;
        private bool IsRecharging;
        //
        // ***



        #endregion

        #region Constructor

        public ClimberEntity(World world, Vector2 s)
        {
            farWorld = world;
            startPos = s;
        }

        #endregion

        #region Public Methods


        public void Initialize()
        {
            frbBodyPartsPolygonList = new PositionedObjectList<Polygon>();
            frbBodyPartsCircleList = new PositionedObjectList<Circle>();

            CreateRagdoll();

            // Set initial state (at rest)
            //
            RHandGrip = false;
            LHandGrip = false;
            RHandFree = true;
            LHandFree = true;
            Reach = false;
            // RHandReach = false;
            // LHandReach = false;
            SwingLeft = false;
            SwingRight = false;
            PullUp = false;
            isExerting = false;
            IsRecharging = true;
            //
            RHandReleaseComplete = 0;
            LHandReleaseComplete = 0;

            exertion = 0;

            // >>> Max value of exertion before climber must recharge
            exertionStrength = 240;
        }


        public void Activity()
        {


            // *** Hand gripping ***
            //
            // If a hand collides with a grip as detected in GameScreen
            //
            // A joint is enabled that attaches that hand to the position of the grip
            //
            // Otherwise the joint is disabled
            //
            //
            if (RHandGrip == true)
            {
                jRHandGrip.WorldAnchorB = rHandGripPos;
                jRHandGrip.Enabled = true;
            }
            else
            {
                jRHandGrip.Enabled = false;
            }
            //
            //    
            if (LHandGrip == true)
            {
                jLHandGrip.WorldAnchorB = lHandGripPos;
                jLHandGrip.Enabled = true;
            }
            else
            {
                jLHandGrip.Enabled = false;
            }
            //
            // ***


            // >>> Should swinging also use up exertion?
            // *** Exerting & recharging ***
            //
            // Pulling up sets climber to a state of exertion. 
            // 
            if (PullUp == true && exertion != exertionStrength)
            {
                isExerting = true;
            }
            //
            //
            // The climber exerting causes its 'exertion' property to rise
            //
            // When this figure meets the maximum 'exertionStrength' climber cannot exert any more; enters state of recharge
            //
            // 'exertion' property decreases when recharging
            //
            // Climber cannot exert again until this property returns to 0
            //
            if (exertion == exertionStrength)
            {
                isExerting = false;
                IsRecharging = true;
            }
            //
            if (exertion == 0)
            {
                IsRecharging = false;
            }
            //
            if (IsRecharging == true)
            {
                isExerting = false;
                if (exertion > 0)
                {
                    exertion -= 2;
                }
            }
            if (isExerting == true)
            {
                if (exertion < exertionStrength)
                {
                    exertion += 2;
                }
            }

            if (TimeManager.CurrentTime > RHandReleaseComplete)
            {
                setRHandFree(true);
            }

            if (TimeManager.CurrentTime > LHandReleaseComplete)
            {
                setLHandFree(true);
            }


            #region visual_update

            // Updates visual representation of Farseer objects (FRB polygons) according to their position and rotation
            //
            //
            frbTorsoPolygon.X = farTorsoBody.Position.X;
            frbTorsoPolygon.Y = farTorsoBody.Position.Y;
            frbTorsoPolygon.RotationZ = farTorsoBody.Rotation;
            //
            frbRLowerArmPolygon.X = farRLowerArmBody.Position.X;
            frbRLowerArmPolygon.Y = farRLowerArmBody.Position.Y;
            frbRLowerArmPolygon.RotationZ = farRLowerArmBody.Rotation;
            //
            frbLLowerArmPolygon.X = farLLowerArmBody.Position.X;
            frbLLowerArmPolygon.Y = farLLowerArmBody.Position.Y;
            frbLLowerArmPolygon.RotationZ = farLLowerArmBody.Rotation;
            //
            //
            frbRUpperArmPolygon.X = farRUpperArmBody.Position.X;
            frbRUpperArmPolygon.Y = farRUpperArmBody.Position.Y;
            frbRUpperArmPolygon.RotationZ = farRUpperArmBody.Rotation;
            //
            frbLUpperArmPolygon.X = farLUpperArmBody.Position.X;
            frbLUpperArmPolygon.Y = farLUpperArmBody.Position.Y;
            frbLUpperArmPolygon.RotationZ = farLUpperArmBody.Rotation;
            //
            frbRUpperLegPolygon.X = farRUpperLegBody.Position.X;
            frbRUpperLegPolygon.Y = farRUpperLegBody.Position.Y;
            frbRUpperLegPolygon.RotationZ = farRUpperLegBody.Rotation;
            //
            frbLUpperLegPolygon.X = farLUpperLegBody.Position.X;
            frbLUpperLegPolygon.Y = farLUpperLegBody.Position.Y;
            frbLUpperLegPolygon.RotationZ = farLUpperLegBody.Rotation;
            //
            frbRLowerLegPolygon.X = farRLowerLegBody.Position.X;
            frbRLowerLegPolygon.Y = farRLowerLegBody.Position.Y;
            frbRLowerLegPolygon.RotationZ = farRLowerLegBody.Rotation;
            //
            frbLLowerLegPolygon.X = farLLowerLegBody.Position.X;
            frbLLowerLegPolygon.Y = farLLowerLegBody.Position.Y;
            frbLLowerLegPolygon.RotationZ = farLLowerLegBody.Rotation;
            //
            frbHeadCircle.X = farHeadBody.Position.X;
            frbHeadCircle.Y = farHeadBody.Position.Y;
            frbHeadCircle.RotationZ = farHeadBody.Rotation;
            //
            frbRHandCircle.X = farRHand.Position.X;
            frbRHandCircle.Y = farRHand.Position.Y;
            frbRHandCircle.RotationZ = farRHand.Rotation;
            //
            frbLHandCircle.X = farLHand.Position.X;
            frbLHandCircle.Y = farLHand.Position.Y;
            frbLHandCircle.RotationZ = farLHand.Rotation;

            #endregion


            climberCentre.X = farRLowerArmBody.Position.X;
            climberCentre.Y = farRLowerArmBody.Position.Y;

        }


        public void Destroy()
        {
            // foreach (Polygon p in frbBodyPartsPolygonList)
            // {
            //     ShapeManager.Remove(p);
            //     SpriteManager.RemovePositionedObject(p);
            // }
            // foreach (Circle c in frbBodyPartsCircleList)
            // {
            //     ShapeManager.Remove(c);
            //     SpriteManager.RemovePositionedObject(c);
            // }
            ShapeManager.Remove(frbHeadCircle);
            ShapeManager.Remove(frbLHandCircle);
            ShapeManager.Remove(frbLLowerArmPolygon);
            ShapeManager.Remove(frbLLowerLegPolygon);
            ShapeManager.Remove(frbLUpperArmPolygon);
            ShapeManager.Remove(frbLUpperLegPolygon);
            ShapeManager.Remove(frbRHandCircle);
            ShapeManager.Remove(frbRLowerArmPolygon);
            ShapeManager.Remove(frbRLowerLegPolygon);
            ShapeManager.Remove(frbRUpperArmPolygon);
            ShapeManager.Remove(frbRUpperLegPolygon);
            ShapeManager.Remove(frbTorsoPolygon);
            SpriteManager.RemovePositionedObject(frbHeadCircle);
            SpriteManager.RemovePositionedObject(frbLHandCircle);
            SpriteManager.RemovePositionedObject(frbLLowerArmPolygon);
            SpriteManager.RemovePositionedObject(frbLLowerLegPolygon);
            SpriteManager.RemovePositionedObject(frbLUpperArmPolygon);
            SpriteManager.RemovePositionedObject(frbLUpperLegPolygon);
            SpriteManager.RemovePositionedObject(frbRHandCircle);
            SpriteManager.RemovePositionedObject(frbRLowerArmPolygon);
            SpriteManager.RemovePositionedObject(frbRLowerLegPolygon);
            SpriteManager.RemovePositionedObject(frbRUpperArmPolygon);
            SpriteManager.RemovePositionedObject(frbRUpperLegPolygon);
            SpriteManager.RemovePositionedObject(frbTorsoPolygon);
        }


        #region public_access_frb_shapes


        public Circle RHandCollision
        {

            get { return frbRHandCircle; }
        }

        public Circle LHandCollision
        {

            get { return frbLHandCircle; }
        }

        public Polygon TorsoCollision
        {

            get { return frbTorsoPolygon; }
        }

        #endregion


        # region public_access_farseer_bodies


        public Body RHandBody
        {

            get { return farRHand; }
        }

        public Body LHandBody
        {

            get { return farLHand; }
        }

        public Body TorsoBody
        {

            get { return farTorsoBody; }
        }

        #endregion


        #region public_access_states

        #region RHandGrip

        public bool getRHandGrip
        {

            get { return RHandGrip; }
        }

        public void setRHandGrip(bool b, Vector2 pos)
        {

            RHandGrip = b;
            rHandGripPos = pos;
        }

        #endregion

        #region LHandGrip

        public bool getLHandGrip
        {

            get { return LHandGrip; }
        }

        public void setLHandGrip(bool b, Vector2 pos)
        {

            LHandGrip = b;
            lHandGripPos = pos;
        }

        #endregion

        #region RHandFree

        public bool getRHandFree
        {
            get { return RHandFree; }
        }

        public void setRHandFree(bool b)
        {
            RHandFree = b;
        }

        #endregion

        #region LHandFree

        public bool getLHandFree
        {
            get { return LHandFree; }
        }

        public void setLHandFree(bool b)
        {
            LHandFree = b;
        }

        #endregion

        #region RHandReach

        public bool getReach
        {
            get { return Reach; }
        }

        public void setReach(bool b)
        {
            Reach = b;
        }

        // public bool getRHandReach
        // {
        //     get { return RHandReach; }
        // }

        // public void setRHandReach(bool b)
        // {
        //     RHandReach = b;
        // }

        #endregion

        #region LHandReach

        // public bool getLHandReach
        // {
        //     get { return LHandReach; }
        // }

        // public void setLHandReach(bool b)
        // {
        //     LHandReach = b;
        // }

        #endregion

        #region SwingRight

        public bool getSwingRight
        {
            get { return SwingRight; }
        }

        public void setSwingRight(bool b)
        {
            SwingRight = b;
        }

        #endregion

        #region SwingLeft

        public bool getSwingLeft
        {
            get { return SwingLeft; }
        }

        public void setSwingLeft(bool b)
        {
            SwingLeft = b;
        }

        #endregion

        #region PullUp

        public bool getPullUp
        {
            get { return PullUp; }
        }

        public void setPullUp(bool b)
        {
            PullUp = b;
        }

        #endregion

        #region exertion

        public float getExertion
        {
            get { return exertion; }
        }

        public void setExertion(int f)
        {
            exertion = f;
        }

        #endregion

        #region isExerting

        public bool getIsExerting
        {
            get { return isExerting; }
        }

        public void setIsExerting(bool b)
        {
            isExerting = b;
        }

        #endregion

        #region isRecharging

        public bool getIsRecharging
        {
            get { return IsRecharging; }
        }

        public void setIsRecharging(bool b)
        {
            IsRecharging = b;
        }

        #endregion

        public Vector2 getClimberCentre
        {
            get { return climberCentre; }
        }

        #endregion

        public void RHandRelease()
        {
            RHandReleaseComplete = TimeManager.CurrentTime + 2.0;
            RHandGrip = false;
        }

        public void LHandRelease()
        {
            LHandReleaseComplete = TimeManager.CurrentTime + 2.0;
            LHandGrip = false;
        }

        #endregion

        #region Private Methods


        private void CreateRagdoll()
        {
            //
            Vector2[] torsoVerts = {
            new Vector2(startPos.X-1f, startPos.Y-3.5f),
            new Vector2(startPos.X+1f, startPos.Y-3.5f),
            new Vector2(startPos.X+1f, startPos.Y+0.5f),
            new Vector2(startPos.X+-1f, startPos.Y+0.5f) };
            Vertices farTorsoVerts;
            //
            //
            Vector2[] rLowerArmVerts = {
            new Vector2(startPos.X+1f, startPos.Y+0f),
            new Vector2(startPos.X+5f, startPos.Y+0.25f),
            new Vector2(startPos.X+1f, startPos.Y+0.5f) };
            Vertices farRLowerArmVerts;
            //
            Vector2[] lLowerArmVerts = {
            new Vector2(startPos.X-5f, startPos.Y+0.25f),
            new Vector2(startPos.X-1f, startPos.Y+0f),
            new Vector2(startPos.X-1f, startPos.Y+0.5f) };
            Vertices farLLowerArmVerts;
            //
            //
            Vector2[] rUpperArmVerts = {
            new Vector2(startPos.X+5f, startPos.Y+0.25f),
            new Vector2(startPos.X+9f, startPos.Y+0f),
            new Vector2(startPos.X+9f, startPos.Y+0.5f) };
            Vertices farRUpperArmVerts;
            //
            Vector2[] lUpperArmVerts = {
            new Vector2(startPos.X-9f, startPos.Y+0f),
            new Vector2(startPos.X-5f, startPos.Y+0.25f),
            new Vector2(startPos.X-9f, startPos.Y+0.5f) };
            Vertices farLUpperArmVerts;
            //
            //
            Vector2[] rUpperLegVerts = {
            new Vector2(startPos.X-0.75f, startPos.Y-7.5f),
            new Vector2(startPos.X+1f, startPos.Y-3.5f),
            new Vector2(startPos.X+0.5f, startPos.Y-3.5f) };
            Vertices farRUpperLegVerts;
            //
            Vector2[] lUpperLegVerts = {
            new Vector2(startPos.X-0.75f, startPos.Y-7.5f),
            new Vector2(startPos.X-0.5f, startPos.Y-3.5f),
            new Vector2(startPos.X-1f, startPos.Y-3.5f) };
            Vertices farLUpperLegVerts;
            //
            //
            Vector2[] rLowerLegVerts = {
            new Vector2(startPos.X+0.5f, startPos.Y-11.5f),
            new Vector2(startPos.X+1f, startPos.Y-11.5f),
            new Vector2(startPos.X+0.75f, startPos.Y-7.5f) };
            Vertices farRLowerLegVerts;
            //
            Vector2[] lLowerLegVerts = {
            new Vector2(startPos.X-1f, startPos.Y-11.5f),
            new Vector2(startPos.X-0.5f, startPos.Y-11.5f),
            new Vector2(startPos.X-0.75f, startPos.Y-7.5f) };
            Vertices farLLowerLegVerts;
            //
            //
            //
            //
            FlatRedBall.Math.Geometry.Point[] frbTorsoPolygonPointArray = {
                new FlatRedBall.Math.Geometry.Point(startPos.X-1f, startPos.Y-3.5f),
                new FlatRedBall.Math.Geometry.Point(startPos.X+1f, startPos.Y-3.5f),
                new FlatRedBall.Math.Geometry.Point(startPos.X+1f, startPos.Y+0.5f),
                new FlatRedBall.Math.Geometry.Point(startPos.X-1f, startPos.Y+0.5f),
                new FlatRedBall.Math.Geometry.Point(startPos.X-1f, startPos.Y-3.5f) };
            //
            FlatRedBall.Math.Geometry.Point[] frbRLowerArmPolygonPointArray = {
                new FlatRedBall.Math.Geometry.Point(startPos.X+1f, startPos.Y+0f),
                new FlatRedBall.Math.Geometry.Point(startPos.X+5f, startPos.Y+0.25f),
                new FlatRedBall.Math.Geometry.Point(startPos.X+1f, startPos.Y+0.5f),
                new FlatRedBall.Math.Geometry.Point(startPos.X+1f, startPos.Y+0f) };
            //
            FlatRedBall.Math.Geometry.Point[] frbLLowerArmPolygonPointArray = {
                new FlatRedBall.Math.Geometry.Point(startPos.X-5f, startPos.Y+0.25f),
                new FlatRedBall.Math.Geometry.Point(startPos.X-1f, startPos.Y+0f),
                new FlatRedBall.Math.Geometry.Point(startPos.X-1f, startPos.Y+0.5f),
                new FlatRedBall.Math.Geometry.Point(startPos.X-5f, startPos.Y+0.25f) };
            //
            FlatRedBall.Math.Geometry.Point[] frbRUpperArmPolygonPointArray = {
                new FlatRedBall.Math.Geometry.Point(startPos.X+5f, startPos.Y+0.25f),
                new FlatRedBall.Math.Geometry.Point(startPos.X+9f, startPos.Y+0f),
                new FlatRedBall.Math.Geometry.Point(startPos.X+9f, startPos.Y+0.5f),
                new FlatRedBall.Math.Geometry.Point(startPos.X+5f, startPos.Y+0.25f) };
            //
            FlatRedBall.Math.Geometry.Point[] frbLUpperArmPolygonPointArray = {
                
                new FlatRedBall.Math.Geometry.Point(startPos.X-9f, startPos.Y+0f),
                new FlatRedBall.Math.Geometry.Point(startPos.X-5f, startPos.Y+0.25f),
                new FlatRedBall.Math.Geometry.Point(startPos.X-9f, startPos.Y+0.5f),
                new FlatRedBall.Math.Geometry.Point(startPos.X-9f, startPos.Y+0f) };
            //
            FlatRedBall.Math.Geometry.Point[] frbRUpperLegPolygonPointArray = {
                new FlatRedBall.Math.Geometry.Point(startPos.X+0.75f, startPos.Y-7.5f),
                new FlatRedBall.Math.Geometry.Point(startPos.X+1f, startPos.Y-3.5f),
                new FlatRedBall.Math.Geometry.Point(startPos.X+0.5f, startPos.Y-3.5f),
                new FlatRedBall.Math.Geometry.Point(startPos.X+0.75f, startPos.Y-7.5f) };
            //
            FlatRedBall.Math.Geometry.Point[] frbLUpperLegPolygonPointArray = {
                new FlatRedBall.Math.Geometry.Point(startPos.X-0.75f, startPos.Y-7.5f),
                new FlatRedBall.Math.Geometry.Point(startPos.X-0.5f, startPos.Y-3.5f),
                new FlatRedBall.Math.Geometry.Point(startPos.X-1f, startPos.Y-3.5f),
                new FlatRedBall.Math.Geometry.Point(startPos.X-0.75f, startPos.Y-7.5f) };
            //
            FlatRedBall.Math.Geometry.Point[] frbRLowerLegPolygonPointArray = {
                new FlatRedBall.Math.Geometry.Point(startPos.X+0.5f, startPos.Y-11.5f),
                new FlatRedBall.Math.Geometry.Point(startPos.X+1f, startPos.Y-11.5f),
                new FlatRedBall.Math.Geometry.Point(startPos.X+0.75f, startPos.Y-7.5f),
                new FlatRedBall.Math.Geometry.Point(startPos.X+0.5f, startPos.Y-11.5f) };
            //
            FlatRedBall.Math.Geometry.Point[] frbLLowerLegPolygonPointArray = {
                
                new FlatRedBall.Math.Geometry.Point(startPos.X-1f, startPos.Y-11.5f),
                new FlatRedBall.Math.Geometry.Point(startPos.X-0.5f, startPos.Y-11.5f),
                new FlatRedBall.Math.Geometry.Point(startPos.X-0.75f, startPos.Y-7.5f),
                new FlatRedBall.Math.Geometry.Point(startPos.X-1f, startPos.Y-11.5f) };
            //
            //
            //
            //
            // Vector2 arrays converted to vertices for use as param of BodyFactory.CreatePolygon.
            //
            farTorsoVerts = new Vertices(torsoVerts);
            farTorsoBody = BodyFactory.CreatePolygon(farWorld, farTorsoVerts, 1);
            farTorsoBody.Restitution = 1f;
            // Dynamic objects respond to gravity.
            farTorsoBody.BodyType = BodyType.Dynamic;
            farTorsoBody.Mass = 5f;
            //
            farHeadBody = BodyFactory.CreateCircle(farWorld, 1f, 1f, new Vector2(startPos.X + 0f, startPos.Y + 1.5f));
            farHeadBody.Restitution = 1f;
            farHeadBody.BodyType = BodyType.Dynamic;
            farHeadBody.Mass = 0.15f;
            //
            farRHand = BodyFactory.CreateCircle(farWorld, 0.5f, 1f, new Vector2(startPos.X + 9f, startPos.Y + 0.25f));
            farRHand.Restitution = 1f;
            farRHand.BodyType = BodyType.Dynamic;
            farRHand.Mass = 0.015f;
            //
            farLHand = BodyFactory.CreateCircle(farWorld, 0.5f, 1f, new Vector2(startPos.X - 9f, startPos.Y + 0.25f));
            farLHand.Restitution = 1f;
            farLHand.BodyType = BodyType.Dynamic;
            farLHand.Mass = 0.015f;
            //
            farRLowerArmVerts = new Vertices(rLowerArmVerts);
            farRLowerArmBody = BodyFactory.CreatePolygon(farWorld, farRLowerArmVerts, 1);
            farRLowerArmBody.Restitution = 1f;
            farRLowerArmBody.BodyType = BodyType.Dynamic;
            farRLowerArmBody.Mass = 0.05f;
            //
            farLLowerArmVerts = new Vertices(lLowerArmVerts);
            farLLowerArmBody = BodyFactory.CreatePolygon(farWorld, farLLowerArmVerts, 1);
            farLLowerArmBody.Restitution = 1f;
            farLLowerArmBody.BodyType = BodyType.Dynamic;
            farLLowerArmBody.Mass = 0.05f;
            //
            farRUpperArmVerts = new Vertices(rUpperArmVerts);
            farRUpperArmBody = BodyFactory.CreatePolygon(farWorld, farRUpperArmVerts, 1);
            farRUpperArmBody.Restitution = 1f;
            farRUpperArmBody.BodyType = BodyType.Dynamic;
            farRUpperArmBody.Mass = 0.1f;
            //
            farLUpperArmVerts = new Vertices(lUpperArmVerts);
            farLUpperArmBody = BodyFactory.CreatePolygon(farWorld, farLUpperArmVerts, 1);
            farLUpperArmBody.Restitution = 1f;
            farLUpperArmBody.BodyType = BodyType.Dynamic;
            farLUpperArmBody.Mass = 0.1f;
            //
            //
            farRUpperLegVerts = new Vertices(rUpperLegVerts);
            farRUpperLegBody = BodyFactory.CreatePolygon(farWorld, farRUpperLegVerts, 1);
            farRUpperLegBody.Restitution = 1f;
            farRUpperLegBody.BodyType = BodyType.Dynamic;
            farRUpperLegBody.Mass = 1f;
            //
            farLUpperLegVerts = new Vertices(lUpperLegVerts);
            farLUpperLegBody = BodyFactory.CreatePolygon(farWorld, farLUpperLegVerts, 1);
            farLUpperLegBody.Restitution = 1f;
            farLUpperLegBody.BodyType = BodyType.Dynamic;
            farLUpperLegBody.Mass = 1f;
            //
            farRLowerLegVerts = new Vertices(rLowerLegVerts);
            farRLowerLegBody = BodyFactory.CreatePolygon(farWorld, farRLowerLegVerts, 1);
            farRLowerLegBody.Restitution = 1f;
            farRLowerLegBody.BodyType = BodyType.Dynamic;
            farRLowerLegBody.Mass = 0.5f;
            //
            farLLowerLegVerts = new Vertices(lLowerLegVerts);
            farLLowerLegBody = BodyFactory.CreatePolygon(farWorld, farLLowerLegVerts, 1);
            farLLowerLegBody.Restitution = 1f;
            farLLowerLegBody.BodyType = BodyType.Dynamic;
            farLLowerLegBody.Mass = 0.5f;
            //
            //
            //
            //
            // Creates a joint between the two bodies in the parameter at the specified anchor point.

            //
            jNeck = JointFactory.CreateRevoluteJoint(farWorld, farTorsoBody, farHeadBody, new Vector2(startPos.X + 0f, startPos.Y + 0.5f));
            jNeck.Enabled = true;
            // Limiting angle of RevoluteJoint mimics human elbow joint; cannot rotate beyond 180deg.
            jNeck.LimitEnabled = true;
            jNeck.LowerLimit = MathHelper.ToRadians(-4f);
            jNeck.UpperLimit = MathHelper.ToRadians(4f);
            // jElbow.MotorEnabled = true;
            // jElbow.MaxMotorTorque = 3;
            jNeck.CollideConnected = true;
            // var jNeckDist = JointFactory.CreateDistanceJoint(farWorld, farTorsoBody, farHeadBody, new Vector2(startPos.X + 0f, startPos.Y + 0.5f), new Vector2(startPos.X + 0f, startPos.Y + 0.5f));
            //
            //
            jRShoulder = JointFactory.CreateRevoluteJoint(farWorld, farTorsoBody, farRLowerArmBody, new Vector2(startPos.X + 1f, startPos.Y + 0.25f));
            jRShoulder.Enabled = true;
            // Limiting angle of RevoluteJoint mimics human elbow joint; cannot rotate beyond 180deg.
            jRShoulder.LimitEnabled = true;
            jRShoulder.LowerLimit = MathHelper.ToRadians(-90f);
            jRShoulder.UpperLimit = MathHelper.ToRadians(180f);
            // jElbow.MotorEnabled = true;
            // jElbow.MaxMotorTorque = 3;
            jRShoulder.CollideConnected = false;
            //
            jLShoulder = JointFactory.CreateRevoluteJoint(farWorld, farTorsoBody, farLLowerArmBody, new Vector2(startPos.X - 1f, startPos.Y + 0.25f));
            jLShoulder.Enabled = true;
            // Limiting angle of RevoluteJoint mimics human elbow joint; cannot rotate beyond 180deg.
            jLShoulder.LimitEnabled = true;
            jLShoulder.LowerLimit = MathHelper.ToRadians(-180f);
            jLShoulder.UpperLimit = MathHelper.ToRadians(90f);
            // jElbow.MotorEnabled = true;
            // jElbow.MaxMotorTorque = 3;
            jLShoulder.CollideConnected = false;
            //
            jRElbow = JointFactory.CreateRevoluteJoint(farWorld, farRLowerArmBody, farRUpperArmBody, new Vector2(startPos.X + 5f, startPos.Y + 0.25f));
            jRElbow.Enabled = true;
            // Limiting angle of RevoluteJoint mimics human elbow joint; cannot rotate beyond 180deg.
            jRElbow.LimitEnabled = true;
            jRElbow.LowerLimit = MathHelper.ToRadians(0f);
            jRElbow.UpperLimit = MathHelper.ToRadians(180f);
            // jElbow.MotorEnabled = true;
            // jElbow.MaxMotorTorque = 3;
            jRElbow.CollideConnected = true;
            //
            jLElbow = JointFactory.CreateRevoluteJoint(farWorld, farLLowerArmBody, farLUpperArmBody, new Vector2(startPos.X - 5f, startPos.Y + 0.25f));
            jLElbow.Enabled = true;
            // Limiting angle of RevoluteJoint mimics human elbow joint; cannot rotate beyond 180deg.
            jLElbow.LimitEnabled = true;
            jLElbow.LowerLimit = MathHelper.ToRadians(-180f);
            jLElbow.UpperLimit = MathHelper.ToRadians(0f);
            // jElbow.MotorEnabled = true;
            // jElbow.MaxMotorTorque = 3;
            jLElbow.CollideConnected = true;
            //
            //
            jRHip = JointFactory.CreateRevoluteJoint(farWorld, farTorsoBody, farRUpperLegBody, new Vector2(startPos.X + 0.75f, startPos.Y - 3.5f));
            jRHip.Enabled = true;
            // Limiting angle of RevoluteJoint mimics human elbow joint; cannot rotate beyond 180deg.
            jRHip.LimitEnabled = true;
            jRHip.LowerLimit = MathHelper.ToRadians(-90f);
            jRHip.UpperLimit = MathHelper.ToRadians(90f);
            // jElbow.MotorEnabled = true;
            // jElbow.MaxMotorTorque = 3;
            jRHip.CollideConnected = false;
            //
            jLHip = JointFactory.CreateRevoluteJoint(farWorld, farTorsoBody, farLUpperLegBody, new Vector2(startPos.X - 0.75f, startPos.Y - 3.5f));
            jLHip.Enabled = true;
            // Limiting angle of RevoluteJoint mimics human elbow joint; cannot rotate beyond 180deg.
            jLHip.LimitEnabled = true;
            jLHip.LowerLimit = MathHelper.ToRadians(-90f);
            jLHip.UpperLimit = MathHelper.ToRadians(90f);
            // jElbow.MotorEnabled = true;
            // jElbow.MaxMotorTorque = 3;
            jLHip.CollideConnected = false;
            //
            //
            jRKnee = JointFactory.CreateRevoluteJoint(farWorld, farRUpperLegBody, farRLowerLegBody, new Vector2(startPos.X + 0.75f, startPos.Y - 7.5f));
            jRKnee.Enabled = true;
            // Limiting angle of RevoluteJoint mimics human elbow joint; cannot rotate beyond 180deg.
            jRKnee.LimitEnabled = true;
            jRKnee.LowerLimit = MathHelper.ToRadians(-90f);
            jRKnee.UpperLimit = MathHelper.ToRadians(0f);
            // jElbow.MotorEnabled = true;
            // jElbow.MaxMotorTorque = 3;
            jRKnee.CollideConnected = true;
            //
            jLKnee = JointFactory.CreateRevoluteJoint(farWorld, farLUpperLegBody, farLLowerLegBody, new Vector2(startPos.X - 0.75f, startPos.Y - 7.5f));
            jLKnee.Enabled = true;
            // Limiting angle of RevoluteJoint mimics human elbow joint; cannot rotate beyond 180deg.
            jLKnee.LimitEnabled = true;
            jLKnee.LowerLimit = MathHelper.ToRadians(0f);
            jLKnee.UpperLimit = MathHelper.ToRadians(90f);
            // jElbow.MotorEnabled = true;
            // jElbow.MaxMotorTorque = 3;
            jLKnee.CollideConnected = true;
            //
            jRWrist = JointFactory.CreateRevoluteJoint(farWorld, farRHand, farRUpperArmBody, new Vector2(startPos.X + 9f, startPos.Y + 0.25f));
            jRWrist.Enabled = true;
            jRWrist.LimitEnabled = false;
            jRWrist.CollideConnected = false;
            //
            jLWrist = JointFactory.CreateRevoluteJoint(farWorld, farLHand, farLUpperArmBody, new Vector2(startPos.X - 9f, startPos.Y + 0.25f));
            jLWrist.Enabled = true;
            jLWrist.LimitEnabled = false;
            jLWrist.CollideConnected = false;
            //
            jRHandGrip = JointFactory.CreateFixedRevoluteJoint(farWorld, farRHand, new Vector2(0f, 0f), farRHand.Position);
            jRHandGrip.Enabled = false;
            jRHandGrip.LimitEnabled = false;
            //
            jLHandGrip = JointFactory.CreateFixedRevoluteJoint(farWorld, farLHand, new Vector2(0f, 0f), farLHand.Position);
            jLHandGrip.Enabled = false;
            jLHandGrip.LimitEnabled = false;

            // FixtureFactory.AttachPolygon(mUpperArmVertices, 1, mUpperArmBody);




            // Initialise FRB variables

            // Draw visual representation of Farseer bodies and add to ShapeManager
            //
            //
            frbTorsoPolygon = new Polygon();
            frbTorsoPolygon.Points = frbTorsoPolygonPointArray;
            ShapeManager.AddPolygon(frbTorsoPolygon);
            frbBodyPartsPolygonList.Add(frbTorsoPolygon);
            //
            frbHeadCircle = new Circle();
            frbHeadCircle.Radius = 1f;
            frbHeadCircle.X = startPos.X + 0f;
            frbHeadCircle.Y = startPos.Y + 1.5f;
            ShapeManager.AddCircle(frbHeadCircle);
            frbBodyPartsCircleList.Add(frbHeadCircle);
            //
            frbRHandCircle = new Circle();
            frbRHandCircle.Radius = 0.5f;
            frbRHandCircle.X = startPos.X + 9f;
            frbRHandCircle.Y = startPos.Y + 0.25f;
            ShapeManager.AddCircle(frbRHandCircle);
            frbBodyPartsCircleList.Add(frbRHandCircle);
            //
            frbLHandCircle = new Circle();
            frbLHandCircle.Radius = 0.5f;
            frbLHandCircle.X = startPos.X - 9f;
            frbLHandCircle.Y = startPos.Y + 0.25f;
            ShapeManager.AddCircle(frbLHandCircle);
            frbBodyPartsCircleList.Add(frbLHandCircle);
            //
            frbRLowerArmPolygon = new Polygon();
            frbRLowerArmPolygon.Points = frbRLowerArmPolygonPointArray;
            ShapeManager.AddPolygon(frbRLowerArmPolygon);
            frbBodyPartsPolygonList.Add(frbRLowerArmPolygon);
            //
            frbLLowerArmPolygon = new Polygon();
            frbLLowerArmPolygon.Points = frbLLowerArmPolygonPointArray;
            ShapeManager.AddPolygon(frbLLowerArmPolygon);
            frbBodyPartsPolygonList.Add(frbLLowerArmPolygon);
            //
            //
            frbRUpperArmPolygon = new Polygon();
            frbRUpperArmPolygon.Points = frbRUpperArmPolygonPointArray;
            ShapeManager.AddPolygon(frbRUpperArmPolygon);
            frbBodyPartsPolygonList.Add(frbRUpperArmPolygon);
            //
            frbLUpperArmPolygon = new Polygon();
            frbLUpperArmPolygon.Points = frbLUpperArmPolygonPointArray;
            ShapeManager.AddPolygon(frbLUpperArmPolygon);
            frbBodyPartsPolygonList.Add(frbLUpperArmPolygon);
            //
            //
            frbRUpperLegPolygon = new Polygon();
            frbRUpperLegPolygon.Points = frbRUpperLegPolygonPointArray;
            ShapeManager.AddPolygon(frbRUpperLegPolygon);
            frbBodyPartsPolygonList.Add(frbRUpperLegPolygon);
            //
            frbLUpperLegPolygon = new Polygon();
            frbLUpperLegPolygon.Points = frbLUpperLegPolygonPointArray;
            ShapeManager.AddPolygon(frbLUpperLegPolygon);
            frbBodyPartsPolygonList.Add(frbLUpperLegPolygon);
            //
            //
            frbRLowerLegPolygon = new Polygon();
            frbRLowerLegPolygon.Points = frbRLowerLegPolygonPointArray;
            ShapeManager.AddPolygon(frbRLowerLegPolygon);
            frbBodyPartsPolygonList.Add(frbRLowerLegPolygon);
            //
            frbLLowerLegPolygon = new Polygon();
            frbLLowerLegPolygon.Points = frbLLowerLegPolygonPointArray;
            ShapeManager.AddPolygon(frbLLowerLegPolygon);
            frbBodyPartsPolygonList.Add(frbLLowerLegPolygon);
            //
            //


        }



        #endregion
    }
}
