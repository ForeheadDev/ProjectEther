using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Core;
using Urho;
using Urho.Actions;
using Urho.SharpReality;
using Urho.Shapes;
using Urho.Resources;
using Urho.Gui;

namespace ProjectEther
{
    internal class Program
    {
        [MTAThread]
        static void Main()
        {
            var appViewSource = new UrhoAppViewSource<HelloWorldApplication>(new ApplicationOptions("Data"));
            appViewSource.UrhoAppViewCreated += OnViewCreated;
            CoreApplication.Run(appViewSource);
        }

        static void OnViewCreated(UrhoAppView view)
        {
            view.WindowIsSet += View_WindowIsSet;
        }

        static void View_WindowIsSet(Windows.UI.Core.CoreWindow coreWindow)
        {
            // you can subscribe to CoreWindow events here
        }
    }

    public class HelloWorldApplication : StereoApplication
    {
        Node DynamoNode;
        Node LumixNode;
        Node UINode;

        AnimationController DCLanim; // dynamo case left anim
        AnimationController DCRanim; // dyanamo case Right anim
        AnimationController pipeanim;
        AnimationController fananim;
        AnimationController holderanim;
        AnimationController brushanim;

        //AnimationController anim;
        //AnimationController anim2;

        const String DCLtextBDCmd = "//Models//Dynamo//dynamoCaseLBreakdown.ani"; // Dynamo case left text breakdown command
        const String DCRtextBDCmd = "//Models//Dynamo//dynamoCaseRBreakdown.ani";
        const String pipetextBDCmd = "//Models//Dynamo//pipeBreakdown.ani";
        const String fantextBDCmd = "//Models//Dynamo//fanBreakdown.ani";
        const String holdertextBDCmd = "//Models//Dynamo//holderBreakdown.ani";
        const String brushtextBDCmd = "//Models//Dynamo//brushBreakdown.ani";
        //const String light2Anim = "//Models//Lumix//light2.ani";
        //const String tutupLensAnim = "//Models//Lumix//Armature.ani";

        const String DCLtextRACmd = "//Models//Dynamo//dynamoCaseLReAssemble.ani";
        const String DCRtextRACmd = "//Models//Dynamo//dynamoCaseRReAssemble.ani";
        const String pipetextRACmd = "//Models//Dynamo//pipeReAssemble.ani";
        const String fantextRACmd = "//Models//Dynamo//fanReAssemble.ani";
        const String holdertextRACmd = "//Models//Dynamo//holderReAssemble.ani";
        const String brushtextRACmd = "//Models//Dynamo//brushReAssemble.ani";

        bool inBrowseObj = false;

        bool inDynamoSpawn = false;
        bool inDynamoCmd = false;

        bool inCameraSpawn = false;
        bool inCameraCmd = false;

        bool toDynamo = false;
        bool toCamera = false;

        public HelloWorldApplication(ApplicationOptions opts) : base(opts) { }

        protected override async void Start()
        {
            // Create a basic scene, see StereoApplication
            base.Start();

            // Create 2d Crosshair
            Sprite crosshair = new Sprite(Context);
            crosshair.Texture = ResourceCache.GetTexture2D(@"Data/Textures/Crosshair.png");
            crosshair.SetAlignment(HorizontalAlignment.Center,
            VerticalAlignment.Center);
            crosshair.Position = new IntVector2(0, 0);
            crosshair.SetSize(35, 25);
            UI.Root.AddChild(crosshair);

            // Enable input
            EnableGestureManipulation = true;
            EnableGestureTapped = true;

            // Create a node for Dynamo
            DynamoNode = Scene.CreateChild();
            DynamoNode.Name = "DynamoNode";

            // Create a node for Lumix
            LumixNode = Scene.CreateChild();
            LumixNode.Name = "LummixNode";

            //Create  a node for UI
            UINode = Scene.CreateChild();
            UINode.Name = "UINode";
            
            
            //Manipulation a Node
            //DynamoNode.Position = new Vector3(0, -0.1f, 1.5f); //1.5m away
            DynamoNode.Position = new Vector3(0.3f, 0, 2.5f); //1.5m away
            DynamoNode.SetScale(0.1f);
            //DynamoNode.Rotation = new Quaternion(0, 90, 90);

            LumixNode.Position = new Vector3(0.3f, 0, 2.5f);
            LumixNode.SetScale(0.1f);

            UINode.Position = new Vector3(-0.1f, 0, 2f);
            UINode.SetScale(0.1f);
            UINode.Rotation = new Quaternion(0, 60, 0);

            // Scene has a lot of pre-configured components, such as Cameras (eyes), Lights, etc.
            DirectionalLight.Brightness = 1f;
            DirectionalLight.Node.SetDirection(new Vector3(-1, 0, 0.5f));

            //Crearing dynamo case
            var dynamoCaseNode = DynamoNode.CreateChild();
            dynamoCaseNode.Name = "dynamoCaseNode";
            /*
            var dynamo = dynamoCaseNode.CreateComponent<StaticModel>();
            dynamo.Model = ResourceCache.GetModel(@"Data\Models\Dynamo\dynamoCase.mdl");
            dynamo.Material = Material.FromImage(@"Data\Models\Dynamo\dynamoTexture.jpg");
            */

            //Creating dynamo component
            var dynamoCaseLNode = DynamoNode.CreateChild();
            dynamoCaseLNode.Name = "dynamoLNode";
            var dynamoCaseL = dynamoCaseLNode.CreateComponent<AnimatedModel>();
            dynamoCaseL.Model = ResourceCache.GetModel(@"Data\Models\Dynamo\dynamoCaseL.mdl");
            dynamoCaseL.Material = Material.FromImage(@"Data\Models\Dynamo\dynamoTexture.jpg");

            DCLanim = dynamoCaseLNode.CreateComponent<AnimationController>();

            var dynamoCaseRNode = DynamoNode.CreateChild();
            dynamoCaseRNode.Name = "dynamoRNode";
            var dynamoCaseR = dynamoCaseRNode.CreateComponent<AnimatedModel>();
            dynamoCaseR.Model = ResourceCache.GetModel(@"Data\Models\Dynamo\dynamoCaseR.mdl");
            dynamoCaseR.Material = Material.FromImage(@"Data\Models\Dynamo\dynamoTexture.jpg");

            DCRanim = dynamoCaseRNode.CreateComponent<AnimationController>();

            var dynamoEngineNode = DynamoNode.CreateChild();
            dynamoEngineNode.Name = "dynamoEngineNode";
            var dynamoEngine = dynamoEngineNode.CreateComponent<StaticModel>();
            dynamoEngine.Model = ResourceCache.GetModel(@"Data\Models\Dynamo\Engine.mdl");
            dynamoEngine.Material = Material.FromImage(@"Data\Models\Dynamo\dynamoTexture.jpg");

            var dynamoFanNode = DynamoNode.CreateChild();
            dynamoFanNode.Name = "dynamoFanNode";
            var dynamoFan = dynamoFanNode.CreateComponent<AnimatedModel>();
            dynamoFan.Model = ResourceCache.GetModel(@"Data\Models\Dynamo\Fan.mdl");
            dynamoFan.Material = Material.FromImage(@"Data\Models\Dynamo\dynamoTexture.jpg");

            fananim = dynamoFanNode.CreateComponent<AnimationController>();
            

            var dynamoPipeNode = DynamoNode.CreateChild();
            dynamoPipeNode.Name = "dynamoPipeNode";
            var dynamoPipe = dynamoPipeNode.CreateComponent<AnimatedModel>();
            dynamoPipe.Model = ResourceCache.GetModel(@"Data\Models\Dynamo\Pipe.mdl");
            dynamoPipe.Material = Material.FromImage(@"Data\Models\Dynamo\dynamoTexture.jpg");

            pipeanim = dynamoPipeNode.CreateComponent<AnimationController>();

            var dynamoBrushNode = DynamoNode.CreateChild();
            dynamoBrushNode.Name = "dynamoBrushNode";
            var dynamoBrush = dynamoBrushNode.CreateComponent<AnimatedModel>();
            dynamoBrush.Model = ResourceCache.GetModel(@"Data\Models\Dynamo\brush.mdl");
            dynamoBrush.Material = Material.FromImage(@"Data\Models\Dynamo\dynamoTexture.jpg");

            brushanim = dynamoBrushNode.CreateComponent<AnimationController>();

            var dynamoHolderNode = DynamoNode.CreateChild();
            dynamoHolderNode.Name = "dynamoHolderNode";
            var dynamoHolder = dynamoHolderNode.CreateComponent<AnimatedModel>();
            dynamoHolder.Model = ResourceCache.GetModel(@"Data\Models\Dynamo\holder.mdl");
            dynamoHolder.Material = Material.FromImage(@"Data\Models\Dynamo\dynamoTexture.jpg");

            holderanim = dynamoHolderNode.CreateComponent<AnimationController>();

            // trun off unvisible model
            dynamoCaseNode.Enabled = false;
            dynamoCaseLNode.Enabled = false;
            dynamoCaseRNode.Enabled = false;
            dynamoEngineNode.Enabled = false;
            dynamoFanNode.Enabled = false;
            dynamoPipeNode.Enabled = false;
            dynamoBrushNode.Enabled = false;
            dynamoHolderNode.Enabled = false;

            // creating lumix model
            var lumixCameraNode = LumixNode.CreateChild(); //0
            lumixCameraNode.Name = "LumixCameraNode";
            var lumixCamera = lumixCameraNode.CreateComponent<StaticModel>();
            lumixCamera.Model = ResourceCache.GetModel(@"Data\Models\Lumix\CameraLumix.mdl");
            lumixCamera.Material = Material.FromImage(@"Data\Models\Lumix\LumixTexture-01.jpg");

            var bodyBottomNode = LumixNode.CreateChild(); // 1
            bodyBottomNode.Name = "bodyBottomNode";
            var bodyBottom = bodyBottomNode.CreateComponent<StaticModel>();
            bodyBottom.Model = ResourceCache.GetModel(@"Data\Models\Lumix\BodyBottom.mdl");
            bodyBottom.Material = Material.FromImage(@"Data\Models\Lumix\LumixTexture-01.jpg");

            var bodyTopNode = LumixNode.CreateChild(); //2
            bodyTopNode.Name = "bodyTopNode";
            var bodyTop = bodyTopNode.CreateComponent<StaticModel>();
            bodyTop.Model = ResourceCache.GetModel(@"Data\Models\Lumix\BodyTop.mdl");
            bodyTop.Material = Material.FromImage(@"Data\Models\Lumix\LumixTexture-01.jpg");

            var flashNode = LumixNode.CreateChild(); //3
            flashNode.Name = "flashNode";
            var flash = flashNode.CreateComponent<StaticModel>();
            flash.Model = ResourceCache.GetModel(@"Data\Models\Lumix\flash.mdl");
            flash.Material = Material.FromImage(@"Data\Models\Lumix\LumixTexture-01.jpg");

            var lensLNode = LumixNode.CreateChild(); // 4
            lensLNode.Name = "lensLNode";
            var lensL = lensLNode.CreateComponent<StaticModel>();
            lensL.Model = ResourceCache.GetModel(@"Data\Models\Lumix\lensL.mdl");
            lensL.Material = Material.FromImage(@"Data\Models\Lumix\LumixTexture-01.jpg");

            var lensMNode = LumixNode.CreateChild(); //5
            lensMNode.Name = "lensMNode";
            var lensM = lensMNode.CreateComponent<StaticModel>();
            lensM.Model = ResourceCache.GetModel(@"Data\Models\Lumix\lensM.mdl");
            lensM.Material = Material.FromImage(@"Data\Models\Lumix\LumixTexture-01.jpg");

            var lensSNode = LumixNode.CreateChild(); //6
            lensSNode.Name = "lensSNode";
            var lensS = lensSNode.CreateComponent<StaticModel>();
            lensS.Model = ResourceCache.GetModel(@"Data\Models\Lumix\lensS.mdl");
            lensS.Material = Material.FromImage(@"Data\Models\Lumix\LumixTexture-01.jpg");

            var imageSensorNode = LumixNode.CreateChild(); //7
            imageSensorNode.Name = "imageSensorNode";
            var imageSensor = imageSensorNode.CreateComponent<StaticModel>();
            imageSensor.Model = ResourceCache.GetModel(@"Data\Models\Lumix\imageSensor.mdl");
            imageSensor.Material = Material.FromImage(@"Data\Models\Lumix\LumixTexture-01.jpg");

            var buttonLumixNode = LumixNode.CreateChild(); //8
            buttonLumixNode.Name = "buttonLumixNode";
            var buttonLumix = buttonLumixNode.CreateComponent<StaticModel>();
            buttonLumix.Model = ResourceCache.GetModel(@"Data\Models\Lumix\button.mdl");
            buttonLumix.Material = Material.FromImage(@"Data\Models\Lumix\LumixTexture-01.jpg");

            var lcdHolderNode = LumixNode.CreateChild(); //9
            lcdHolderNode.Name = "lcdHolderNode";
            var lcdHolder = lcdHolderNode.CreateComponent<StaticModel>();
            lcdHolder.Model = ResourceCache.GetModel(@"Data\Models\Lumix\lcdHolder.mdl");
            lcdHolder.Material = Material.FromImage(@"Data\Models\Lumix\LumixTexture-01.jpg");

            var LCDLumixNode = LumixNode.CreateChild(); //10
            LCDLumixNode.Name = "LCDLumixNode";
            var LCDLumix = LCDLumixNode.CreateComponent<StaticModel>();
            LCDLumix.Model = ResourceCache.GetModel(@"Data\Models\Lumix\LCD.mdl");
            LCDLumix.Material = Material.FromImage(@"Data\Models\Lumix\LumixTexture-01.jpg");

            var pcbLumixNode = LumixNode.CreateChild(); //11
            pcbLumixNode.Name = "pcbLumixNode";
            var pcbLumix = pcbLumixNode.CreateComponent<StaticModel>();
            pcbLumix.Model = ResourceCache.GetModel(@"Data\Models\Lumix\pcb.mdl");
            pcbLumix.Material = Material.FromImage(@"Data\Models\Lumix\pcbTexture-01.jpg");

            /*
            var light2Node = LumixNode.CreateChild();
            light2Node.Name = "light2Node";
            var light2 = light2Node.CreateComponent<AnimatedModel>();
            light2.Model = ResourceCache.GetModel(@"Data\Models\Lumix\light2.mdl");
            light2.Material = Material.FromImage(@"Data\Models\Lumix\LumixTexture-01.jpg");

            var tutupLensNode = LumixNode.CreateChild();
            tutupLensNode.Name = "tutupLensNode";
            var tutupLens = tutupLensNode.CreateComponent<AnimatedModel>();
            tutupLens.Model = ResourceCache.GetModel(@"Data\Models\Lumix\tutupLensa.mdl");
            light2.Material = Material.FromImage(@"Data\Models\Lumix\tutupLensa-01.jpg");

            anim = light2Node.CreateComponent<AnimationController>();
            anim2 = tutupLensNode.CreateComponent<AnimationController>();
            */

            //enable unvisible lumix model
            lumixCameraNode.Enabled = false;
            bodyBottomNode.Enabled = false;
            bodyTopNode.Enabled = false;
            flashNode.Enabled = false;
            lensLNode.Enabled = false;
            lensMNode.Enabled = false;
            lensSNode.Enabled = false;
            imageSensorNode.Enabled = false;
            buttonLumixNode.Enabled = false;
            lcdHolderNode.Enabled = false;
            LCDLumixNode.Enabled = false;
            pcbLumixNode.Enabled = false;
           

            //UI background
            var bgUINode = UINode.CreateChild(); //0
            bgUINode.Name = "bgUINode";
            var bgUI = bgUINode.CreateComponent<StaticModel>();
            bgUI.Model = ResourceCache.GetModel(@"Data\assetUI\bgUI.mdl");
            bgUI.Material = Material.FromImage(@"Data\assetUI\bg.jpg");

            // UI Non Interactive
            var logoNode = UINode.CreateChild(); //1
            var logo = logoNode.CreateComponent<StaticModel>();
            logo.Model = ResourceCache.GetModel(@"Data\assetUI\logo.mdl");
            logo.Material = Material.FromImage(@"Data\assetUI\home.jpg");

            var cmdListNode = UINode.CreateChild(); //2
            var cmdList = cmdListNode.CreateComponent<StaticModel>();
            cmdList.Model = ResourceCache.GetModel(@"Data\assetUI\cmdList.mdl");
            cmdList.Material = Material.FromImage(@"Data\assetUI\ButtonAndText-01.jpg");

            //UI button

            var browseObjBtnNode = UINode.CreateChild(); //3
            browseObjBtnNode.Name = "browseObjBtnNode";
            var browseObjBtn = browseObjBtnNode.CreateComponent<StaticModel>();
            browseObjBtn.Model = ResourceCache.GetModel(@"Data\assetUI\browseObj.mdl");
            browseObjBtn.Material = Material.FromImage(@"Data\assetUI\ButtonAndText-01.jpg");

            var aboutBtnNode = UINode.CreateChild(); //4
            aboutBtnNode.Name = "aboutBtnNode";
            var aboutBtn = aboutBtnNode.CreateComponent<StaticModel>();
            aboutBtn.Model = ResourceCache.GetModel(@"Data\assetUI\About.mdl");
            aboutBtn.Material = Material.FromImage(@"Data\assetUI\ButtonAndText-01.jpg");

            var exitBtnNode = UINode.CreateChild(); //5
            exitBtnNode.Name = "exitBtnNode";
            var exitBtn = exitBtnNode.CreateComponent<StaticModel>();
            exitBtn.Model = ResourceCache.GetModel(@"Data\assetUI\Exit.mdl");
            exitBtn.Material = Material.FromImage(@"Data\assetUI\ButtonAndText-01.jpg");

            var backBtnNode = UINode.CreateChild(); //6
            backBtnNode.Name = "backBtnNode";
            var backBtn = backBtnNode.CreateComponent<StaticModel>();
            backBtn.Model = ResourceCache.GetModel(@"Data\assetUI\back.mdl");
            backBtn.Material = Material.FromImage(@"Data\assetUI\ButtonAndText-01.jpg");

            var dynamoBtnNode = UINode.CreateChild(); //7
            dynamoBtnNode.Name = "dynamoBtnNode";
            var dynamoBtn = dynamoBtnNode.CreateComponent<StaticModel>();
            dynamoBtn.Model = ResourceCache.GetModel(@"Data\assetUI\DynamoBtn.mdl");
            dynamoBtn.Material = Material.FromImage(@"Data\assetUI\ButtonAndText-01.jpg");

            var cameraBtnNode = UINode.CreateChild(); //8
            cameraBtnNode.Name = "cameraBtnNode";
            var cameraBtn = cameraBtnNode.CreateComponent<StaticModel>();
            cameraBtn.Model = ResourceCache.GetModel(@"Data\assetUI\CameraBtn.mdl");
            cameraBtn.Material = Material.FromImage(@"Data\assetUI\ButtonAndText-01.jpg");

            var batteryBtnNode = UINode.CreateChild(); //9
            batteryBtnNode.Name = "batteryBtnNode";
            var batteryBtn = batteryBtnNode.CreateComponent<StaticModel>();
            batteryBtn.Model = ResourceCache.GetModel(@"Data\assetUI\BatteryButton.mdl");
            batteryBtn.Material = Material.FromImage(@"Data\assetUI\comingSoonObj-01.jpg");

            var breakdownBtnNode = UINode.CreateChild(); //10
            breakdownBtnNode.Name = "breakdownBtnNode";
            var breakdownBtn = breakdownBtnNode.CreateComponent<StaticModel>();
            breakdownBtn.Model = ResourceCache.GetModel(@"Data\assetUI\breakdownBtn.mdl");
            breakdownBtn.Material = Material.FromImage(@"Data\assetUI\ButtonAndText-01.jpg");

            var reassembleBtnNode = UINode.CreateChild(); //11
            reassembleBtnNode.Name = "reassembleBtnNode";
            var reassembleBtn = reassembleBtnNode.CreateComponent<StaticModel>();
            reassembleBtn.Model = ResourceCache.GetModel(@"Data\assetUI\reassembleBtn.mdl");
            reassembleBtn.Material = Material.FromImage(@"Data\assetUI\ButtonAndText-01.jpg");

            var spawnBtnNode = UINode.CreateChild(); //12
            spawnBtnNode.Name = "spawnBtnNode";
            var spawnBtn = spawnBtnNode.CreateComponent<StaticModel>();
            spawnBtn.Model = ResourceCache.GetModel(@"Data\assetUI\spawn.mdl");
            spawnBtn.Material = Material.FromImage(@"Data\assetUI\ButtonAndText-01.jpg");

            // create UI Text 

            var dynamoTextNode = UINode.CreateChild(); // 13
            dynamoTextNode.Name = "dynamoTextBtnNode";
            var dynamoText = dynamoTextNode.CreateComponent<StaticModel>();
            dynamoText.Model = ResourceCache.GetModel(@"Data\assetUI\dynamoText.mdl");
            dynamoText.Material = Material.FromImage(@"Data\assetUI\ButtonAndText-01.jpg");

            var dynamoThumbNode = UINode.CreateChild(); // 14
            dynamoThumbNode.Name = "dynamoThumbBtnNode";
            var dynamoThumb = dynamoThumbNode.CreateComponent<StaticModel>();
            dynamoThumb.Model = ResourceCache.GetModel(@"Data\assetUI\dynamoThumb.mdl");
            dynamoThumb.Material = Material.FromImage(@"Data\assetUI\dynamoThumb.jpg");

            var lumixTextNode = UINode.CreateChild(); // 15
            lumixTextNode.Name = "lumixTextBtnNode";
            var lumixText = lumixTextNode.CreateComponent<StaticModel>();
            lumixText.Model = ResourceCache.GetModel(@"Data\assetUI\lumixText.mdl");
            lumixText.Material = Material.FromImage(@"Data\assetUI\lumixText-01.jpg");

            var lumixThumbNode = UINode.CreateChild(); // 16
            lumixThumbNode.Name = "lumixThumbBtnNode";
            var lumixThumb = lumixThumbNode.CreateComponent<StaticModel>();
            lumixThumb.Model = ResourceCache.GetModel(@"Data\assetUI\lumixThumb.mdl");
            lumixThumb.Material = Material.FromImage(@"Data\assetUI\lumixTrace.png");


            // disabled default unvisible UI obj
            
            dynamoThumbNode.Enabled = false;
            backBtnNode.Enabled = false;
            dynamoBtnNode.Enabled = false;
            cameraBtnNode.Enabled = false;
            batteryBtnNode.Enabled = false;
            cmdListNode.Enabled = false;
            dynamoTextNode.Enabled = false;
            spawnBtnNode.Enabled = false;
            breakdownBtnNode.Enabled = false;
            reassembleBtnNode.Enabled = false;
            lumixTextNode.Enabled = false;
            lumixThumbNode.Enabled = false;





            await TextToSpeech("Project Ether Start!");
        }

        // For HL optical stabilization (optional)
        public override Vector3 FocusWorldPoint => DynamoNode.WorldPosition;


        //Handle input:

        Vector3 earthPosBeforeManipulations;
        public override void OnGestureManipulationStarted() => earthPosBeforeManipulations = DynamoNode.Position;
        public override void OnGestureManipulationUpdated(Vector3 relativeHandPosition) =>
            DynamoNode.Position = relativeHandPosition + earthPosBeforeManipulations;

        public override void OnGestureTapped() {
            base.OnGestureTapped();

            Ray cameraRay = RightCamera.GetScreenRay(0.5f, 0.5f);
            var result = Scene.GetComponent<Octree>().RaycastSingle(cameraRay, RayQueryLevel.Triangle, 100, DrawableFlags.Geometry, 0x70000000);



            // go to browse obj
            if (result != null && result.Value.Node.Name == "browseObjBtnNode")
            {
                UINode.GetChild(1).Enabled = false;
                UINode.GetChild(3).Enabled = false;
                UINode.GetChild(4).Enabled = false;
                UINode.GetChild(5).Enabled = false;
                UINode.GetChild(6).Enabled = true;
                UINode.GetChild(7).Enabled = true;
                UINode.GetChild(8).Enabled = true;
                UINode.GetChild(9).Enabled = true;
                inBrowseObj = true;

            }


            //go to to dynamo
            if (result != null && result.Value.Node.Name == "dynamoBtnNode")
            {              
                UINode.GetChild(6).Enabled = true;
                UINode.GetChild(7).Enabled = false;
                UINode.GetChild(8).Enabled = false;
                UINode.GetChild(9).Enabled = false;
                UINode.GetChild(12).Enabled = true;
                UINode.GetChild(13).Enabled = true;
                UINode.GetChild(14).Enabled = true;
                toDynamo = true;
                inDynamoSpawn = true;
                inBrowseObj = false;
            }

            if (result != null && result.Value.Node.Name == "cameraBtnNode")
            {
                UINode.GetChild(6).Enabled = true;
                UINode.GetChild(7).Enabled = false;
                UINode.GetChild(8).Enabled = false;
                UINode.GetChild(9).Enabled = false;
                UINode.GetChild(12).Enabled = true;
                UINode.GetChild(15).Enabled = true;
                UINode.GetChild(16).Enabled = true;
                toCamera = true;
                inCameraSpawn = true;
                inBrowseObj = false;
            }

            //Spawn Dynamo
            if (result != null && result.Value.Node.Name == "spawnBtnNode" && toDynamo == true)
            {
                UINode.GetChild(2).Enabled = true;
                UINode.GetChild(10).Enabled = true;
                UINode.GetChild(11).Enabled = true;
                UINode.GetChild(12).Enabled = false;
                UINode.GetChild(13).Enabled = false;
                UINode.GetChild(14).Enabled = false;

                DynamoNode.GetChild(1).Enabled = true;
                DynamoNode.GetChild(2).Enabled = true;
                DynamoNode.GetChild(3).Enabled = true;
                DynamoNode.GetChild(4).Enabled = true;
                DynamoNode.GetChild(5).Enabled = true;
                DynamoNode.GetChild(6).Enabled = true;
                DynamoNode.GetChild(7).Enabled = true;

                toDynamo = true;
                inDynamoSpawn = false;
                inDynamoCmd = true;
            }
            else if (result != null && result.Value.Node.Name == "spawnBtnNode" && toCamera == true) //spawn camera
            {
                UINode.GetChild(2).Enabled = true;
                UINode.GetChild(10).Enabled = true;
                UINode.GetChild(11).Enabled = true;
                UINode.GetChild(12).Enabled = false;
                UINode.GetChild(15).Enabled = false;
                UINode.GetChild(16).Enabled = false;              
                LumixNode.GetChild(0).Enabled = true;

                /*
                anim.Play(light2Anim, 1, false, 0f);
                anim2.Play(tutupLensAnim, 1, false, 0f);
                */
                toCamera = true;
                inCameraSpawn = false;
                inCameraCmd = true;
            }

                // check condition before breakdown or reassemble
                
                if (result != null && result.Value.Node.Name == "breakdownBtnNode" && toDynamo == true )
            {

                DCLanim.Play(DCLtextBDCmd, 1, false, 0f);
                DCRanim.Play(DCRtextBDCmd, 1, false, 0f);
                pipeanim.Play(pipetextBDCmd, 1, false, 0f);
                fananim.Play(fantextBDCmd, 1, false, 0f);
                holderanim.Play(holdertextBDCmd, 1, false, 0f);
                brushanim.Play(brushtextBDCmd, 1, false, 0f);

                DCLanim.Stop(DCLtextRACmd, 0f);
                DCRanim.Stop(DCRtextRACmd, 0f);
                pipeanim.Stop(pipetextRACmd, 0f);
                fananim.Stop(fantextRACmd, 0f);
                holderanim.Stop(holdertextRACmd, 0f);
                brushanim.Stop(brushtextRACmd, 0f);


            }
            else if(result != null && result.Value.Node.Name == "reassembleBtnNode" && toDynamo ==true)
            {

                DCLanim.Play(DCLtextRACmd, 1, false, 0f);
                DCRanim.Play(DCRtextRACmd, 1, false, 0f);
                pipeanim.Play(pipetextRACmd, 1, false, 0f);
                fananim.Play(fantextRACmd, 1, false, 0f);
                holderanim.Play(holdertextRACmd, 1, false, 0f);
                brushanim.Play(brushtextRACmd, 1, false, 0f);

                DCLanim.Stop(DCLtextBDCmd,0f);
                DCRanim.Stop(DCRtextBDCmd, 0f);
                pipeanim.Stop(pipetextBDCmd, 0f);
                fananim.Stop(fantextBDCmd, 0f);
                holderanim.Stop(holdertextBDCmd, 0f);
                brushanim.Stop(brushtextBDCmd, 0f);


            }
            else if (result != null && result.Value.Node.Name == "breakdownBtnNode" && toCamera == true)
            {
                LumixNode.GetChild(0).Enabled = false;
                LumixNode.GetChild(1).Enabled = true;
                LumixNode.GetChild(2).Enabled = true;
                LumixNode.GetChild(3).Enabled = true;
                LumixNode.GetChild(4).Enabled = true;
                LumixNode.GetChild(5).Enabled = true;
                LumixNode.GetChild(6).Enabled = true;
                LumixNode.GetChild(7).Enabled = true;
                LumixNode.GetChild(8).Enabled = true;
                LumixNode.GetChild(9).Enabled = true;
                LumixNode.GetChild(10).Enabled = true;
                LumixNode.GetChild(11).Enabled = true;

                
            }
            else if(result != null && result.Value.Node.Name == "reassembleBtnNode" && toCamera ==true)
            {
                LumixNode.GetChild(0).Enabled = true;
                LumixNode.GetChild(1).Enabled = false;
                LumixNode.GetChild(2).Enabled = false;
                LumixNode.GetChild(3).Enabled = false;
                LumixNode.GetChild(4).Enabled = false;
                LumixNode.GetChild(5).Enabled = false;
                LumixNode.GetChild(6).Enabled = false;
                LumixNode.GetChild(7).Enabled = false;
                LumixNode.GetChild(8).Enabled = false;
                LumixNode.GetChild(9).Enabled = false;
                LumixNode.GetChild(10).Enabled = false;
                LumixNode.GetChild(11).Enabled = false;
            }
            
            // Check Object before Narration
            if (result != null && result.Value.Node.Name == "dynamoCaseNode")
            {
                TextToSpeech("This is dynamo, Dynamo is the first electric generator capable, of delivering power to industry, ");
            } else if (result != null && result.Value.Node.Name == "dynamoLNode" || result != null && result.Value.Node.Name == "dynamoRNode")
            {
                TextToSpeech("this is Motor Housting, The Motor Housting serves to protect the inside of the dynamo");
            } else if (result != null && result.Value.Node.Name == "dynamoEngineNode")
            {
                TextToSpeech("this is Stator, in the Stator there is a coil and armature. the coil functions to increase the voltage from the battery. Armature serves to drain the starter electricity.");
            } else if (result != null && result.Value.Node.Name == "dynamoFanNode")
            {
                TextToSpeech("this is Cooling Fan, the cooling fan serves to cool the dynamo component");
            } else if(result != null && result.Value.Node.Name == "dynamoPipeNode")
            {
                TextToSpeech("this is Main Shaft, The Main shaft acts as a shaft on which various equipment must be moved.");
            } else if (result != null && result.Value.Node.Name == "dynamoBrushNode")
            {
                TextToSpeech("this is Cooper Brush, The Cooper brush whose function is to connect an electric current to the rotor.");
            } else if (result != null && result.Value.Node.Name == "dynamoHolderNode")
            {
                TextToSpeech("this isField Electromagnetic, The part that will produce Electromagnetic Field");
            }

            // Check Camera before narration
            if (result != null && result.Value.Node.Name == "LumixCameraNode")
            {
                TextToSpeech("This is Mirrorless camera, a camera that doesn't use a mirror");
            } else if (result != null && result.Value.Node.Name == "bodyBottomNode" || result.Value.Node.Name == "bodyTopNode")
            {
                TextToSpeech("This is a part of body, body serves to protect the inside of the camera.");
            } else if (result != null && result.Value.Node.Name == "flashNode")
            {
                TextToSpeech("This is flash, a flash serves to make artificial light");
            } else if (result != null && result.Value.Node.Name == "lensLNode" || result.Value.Node.Name == "lensMNode" || result.Value.Node.Name == "lensSNode")
            {
                TextToSpeech("This is a part of lens, the lens serves as a place for the entry of light");
            } else if (result != null && result.Value.Node.Name == "imageSensorNode")
            {
                TextToSpeech("This is Image Sensor, a sensor that detects and conveys information used to make an image");
            } else if (result != null && result.Value.Node.Name == "buttonLumixNode")
            {
                TextToSpeech("This is a Holder button, a part to hold several button");
            }
            else if (result != null && result.Value.Node.Name == "lcdHolderNode")
            {
                TextToSpeech("This a part of LCD, the back holder of lcd");
            }
            else if (result != null && result.Value.Node.Name == "LCDLumixNode")
            {
                TextToSpeech("This is LCD, LCD is a type of display media that uses liquid crystal as the main viewer.");
            }
            else if (result != null && result.Value.Node.Name == "pcbLumixNode")
            {
                TextToSpeech("This is PCB, PCB is a board full of metal circuits that connect different electronic components of one type or another without wires");
            }

                // go back
                if (result != null && result.Value.Node.Name == "backBtnNode" && inBrowseObj == true)
            {
                UINode.GetChild(1).Enabled = true;
                UINode.GetChild(3).Enabled = true;
                UINode.GetChild(4).Enabled = true;
                UINode.GetChild(5).Enabled = true;
                UINode.GetChild(6).Enabled = false;
                UINode.GetChild(7).Enabled = false;
                UINode.GetChild(8).Enabled = false;
                UINode.GetChild(9).Enabled = false;
                inBrowseObj = false;
                inDynamoSpawn = false;
                inDynamoCmd = false;
                inCameraCmd = false;
                inCameraSpawn = false;
           

            }

            if (result != null && result.Value.Node.Name == "backBtnNode" && inDynamoSpawn == true)
            {
                UINode.GetChild(1).Enabled = false;
                UINode.GetChild(3).Enabled = false;
                UINode.GetChild(4).Enabled = false;
                UINode.GetChild(5).Enabled = false;
                UINode.GetChild(6).Enabled = true;
                UINode.GetChild(7).Enabled = true;
                UINode.GetChild(8).Enabled = true;
                UINode.GetChild(9).Enabled = true;
                UINode.GetChild(12).Enabled = false;
                UINode.GetChild(13).Enabled = false;
                UINode.GetChild(14).Enabled = false;
                
                inBrowseObj = true;
                //inCameraCmd = false;              
                inDynamoSpawn = false;
                //inDynamoCmd = false;
                //toCamera = false;
                toDynamo = false;

            } else if (result != null && result.Value.Node.Name == "backBtnNode" && inCameraSpawn == true)
            {
                UINode.GetChild(1).Enabled = false;
                UINode.GetChild(3).Enabled = false;
                UINode.GetChild(4).Enabled = false;
                UINode.GetChild(5).Enabled = false;
                UINode.GetChild(6).Enabled = true;
                UINode.GetChild(7).Enabled = true;
                UINode.GetChild(8).Enabled = true;
                UINode.GetChild(9).Enabled = true;
                UINode.GetChild(12).Enabled = false;
                UINode.GetChild(15).Enabled = false;
                UINode.GetChild(16).Enabled = false;

                inBrowseObj = true;
                //inCameraCmd = false;
                inDynamoSpawn = false;
                //inDynamoCmd = false;
                toCamera = false;
                //toDynamo = false;
            }


            if (result != null && result.Value.Node.Name == "backBtnNode" && inDynamoCmd == true)
            {
               
                // enabled unwanted obj
                UINode.GetChild(2).Enabled = false;
                UINode.GetChild(10).Enabled = false;
                UINode.GetChild(11).Enabled = false;

                UINode.GetChild(7).Enabled = false;
                UINode.GetChild(8).Enabled = false;
                UINode.GetChild(9).Enabled = false;

                UINode.GetChild(12).Enabled = true;
                UINode.GetChild(13).Enabled = true;
                UINode.GetChild(14).Enabled = true;
               
                DynamoNode.GetChild(1).Enabled = false;
                DynamoNode.GetChild(2).Enabled = false;
                DynamoNode.GetChild(3).Enabled = false;
                DynamoNode.GetChild(4).Enabled = false;
                DynamoNode.GetChild(5).Enabled = false;
                DynamoNode.GetChild(6).Enabled = false;
                DynamoNode.GetChild(7).Enabled = false;

                // stop animation obj before back
                DCLanim.Stop(DCLtextBDCmd, 0f);
                DCRanim.Stop(DCRtextBDCmd, 0f);
                pipeanim.Stop(pipetextBDCmd, 0f);
                fananim.Stop(fantextBDCmd, 0f);
                holderanim.Stop(holdertextBDCmd, 0f);
                brushanim.Stop(brushtextBDCmd, 0f);

                DCLanim.Stop(DCLtextRACmd, 0f);
                DCRanim.Stop(DCRtextRACmd, 0f);
                pipeanim.Stop(pipetextRACmd, 0f);
                fananim.Stop(fantextRACmd, 0f);
                holderanim.Stop(holdertextRACmd, 0f);
                brushanim.Stop(brushtextRACmd, 0f);

                //toDynamo = false;
                //toCamera = false;
                inDynamoSpawn = true;
                inBrowseObj = false;
                inDynamoCmd = false;
            } else if (result != null && result.Value.Node.Name == "backBtnNode" && inCameraCmd == true)
            {
                UINode.GetChild(2).Enabled = false;
                UINode.GetChild(10).Enabled = false;
                UINode.GetChild(11).Enabled = false;

                UINode.GetChild(7).Enabled = false;
                UINode.GetChild(8).Enabled = false;
                UINode.GetChild(9).Enabled = false;

                UINode.GetChild(12).Enabled = true;
                UINode.GetChild(15).Enabled = true;
                UINode.GetChild(16).Enabled = true;

                LumixNode.GetChild(0).Enabled = false;
                LumixNode.GetChild(1).Enabled = false;
                LumixNode.GetChild(2).Enabled = false;
                LumixNode.GetChild(3).Enabled = false;
                LumixNode.GetChild(4).Enabled = false;
                LumixNode.GetChild(5).Enabled = false;
                LumixNode.GetChild(6).Enabled = false;
                LumixNode.GetChild(7).Enabled = false;
                LumixNode.GetChild(8).Enabled = false;
                LumixNode.GetChild(9).Enabled = false;
                LumixNode.GetChild(10).Enabled = false;
                LumixNode.GetChild(11).Enabled = false;

                //toDynamo = false;
                //toCamera = false;
                inCameraSpawn = true;
                inCameraCmd = false;
                inBrowseObj = false;
            }
        }
        public override void OnGestureDoubleTapped() { }
    }
}