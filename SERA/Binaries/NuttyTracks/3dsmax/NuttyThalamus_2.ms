NUTTYTHALAMUSPATH = systemTools.getEnvVariable("NUTTYTHALAMUSPATH")+@"\"
sysinfo.currentdir = NUTTYTHALAMUSPATH + "..\Plugins"

dll = NUTTYTHALAMUSPATH + "..\Plugins\RiverFlow.dll"
assembly = dotnet.loadAssembly(dll)

dll = NUTTYTHALAMUSPATH + "..\Plugins\Thalamus.dll"
assembly = dotnet.loadAssembly(dll)

dll = NUTTYTHALAMUSPATH + "..\Plugins\NuttyTracks.dll"
assembly2 = dotnet.loadAssembly(dll)

dll = NUTTYTHALAMUSPATH + "..\Plugins\NuttyThalamus.dll"
assembly4 = dotnet.loadAssembly(dll)

MaxAddSkeletonNode = undefined
MaxSkeletonFinished = undefined
GetPosture = undefined
MaxAnimate = undefined
GetAnimation = undefined
WindowStateChanged = undefined
ChangeControlBody = undefined
gc()

timeConfiguration.realTimePlayback = false

fileIn (NUTTYTHALAMUSPATH+"3dsmax\NuttySkeleton.ms")

--if (nutty != undefined and showMethods( nutty.Shutingdown) then (
	nutty = undefined
--)
if (nutty==undefined) then (
	global nutty = (dotNetObject "NuttyThalamus.NuttyThalamusPlugin"("tiago"))
	dotNet.setLifetimeControl nutty #dotnet
	sleep 0.5
	callbacks.addScript #preSystemShutdown "nutty.Dispose()" id:#disposeNutty
) 
else 
(
	dotNet.removeAllEventHandlers nutty
)

function MaxAddSkeletonNode dn_nodeName dn_nodeParent dn_pX dn_pY dn_pZ dn_rotationOffset dn_axis =
(
	nodeName = dn_nodeName.ToString()
	nodeParent = dn_nodeParent.ToString()
	pX = dn_pX.ToString() as Float
	pY = dn_pY.ToString() as Float
	pZ = dn_pZ.ToString() as Float
	rotationOffset = dn_rotationOffset.ToString() as Float
	case of
	(
		"RotX": axis = 4
		"RotY": axis = 5
		"RotZ": axis = 6
		"TransX": axis = 1
		"TransY": axis = 2
		"TransZ": axis = 3
		default: axis = 0
	)
	axis = dn_axis.ToString() as Integer
	format "MaxAddSkeletonNode %, %, [% % %] % %\n" nodeName nodeParent pX pY pZ rotationOffset axis
	with redraw off CreateSkeletonNode nodeName nodeParent pX pY pZ rotationOffset axis
)

function MaxSkeletonFinished =
(
	print "skeleton finished"
	with redraw off SkeletonFinished
)

function GetPosture =
(
	nutty.SavePosture (GetSkeletonPose())
)

function GetAnimation =
(
	i = 0
	sliderTime = animationRange.start
	nutty.SaveAnimationStart (dotNetobject "System.Int32"(animationRange.end-animationRange.start)) (dotNetobject "System.Int32"(frameRate))
	for f = animationRange.start to animationRange.end do
	(
		pose = GetSkeletonPose()
		frameNum = (dotNetObject "System.Int32"(sliderTime - animationRange.start))
		fr = dotNetobject "System.Double"(frameRate)
 		nutty.SaveAnimationFrame frameNum pose fr
		sliderTime = sliderTime+1
		redrawViews()
	)
	nutty.SaveAnimationEnd()
)

function ChangeControlBody controlBody = 
(
	global NuttyMaxControlBody
	print (controlBody.ToString())
	if (controlBody.ToString()=="False") then NuttyMaxControlBody = false
	else NuttyMaxControlBody = true
	print NuttyMaxControlBody
)

function Debug txt = 
(
	print(txt.ToString())
)

global NuttyMaxControlBody = false

global queuedFrame = undefined 
fn QueueSkeletonAnimate frame = (
	global windowActive
	try (
		if (queuedFrame==undefined) then (
			queuedFrame = frame
			SkeletonAnimate(queuedFrame)
		)
	)
	catch
	(
		print "failed to queue animate"
	)
)

dotnet.addeventhandler nutty "GetAnimationEvent" GetAnimation
dotnet.addeventhandler nutty "ConnectSkeletonNodeEvent" MaxAddSkeletonNode
dotnet.addeventhandler nutty "SkeletonFinishedEvent" SkeletonFinished
--dotnet.addeventhandler nutty "AnimateEvent" QueueSkeletonAnimate
dotnet.addeventhandler nutty "GetPostureEvent" GetPosture
dotnet.addeventhandler nutty "ChangeControlBodyEvent" ChangeControlBody
dotnet.addeventhandler nutty "PrintEvent" Debug

global windowActive = true

global nuttymax_redrawviews_callback_function
unregisterRedrawViewsCallback nuttymax_redrawviews_callback_function
fn nuttymax_redrawviews_callback_function = (

	if (NuttyMaxControlBody) then
	(
		pose = GetSkeletonPose()
		fr = dotNetobject "System.Double"(frameRate)
		nutty.AnimateBody pose fr
	) else if (queuedFrame != undefined) do (
		queuedFrame = undefined
	)
)
registerRedrawViewsCallback nuttymax_redrawviews_callback_function

function WindowFocus state = (
	global windowActive
	windowActive = state
	format "windowActiveChanged: %\n" windowActive
)

callbacks.addScript #mainWindowEnabled "WindowFocus((callbacks.notificationParam()))"

nutty.ShowForm()
print "Running NuttyThalamus"