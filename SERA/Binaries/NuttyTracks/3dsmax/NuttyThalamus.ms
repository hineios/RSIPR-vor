NUTTYTHALAMUSPATH = systemTools.getEnvVariable("NUTTYTHALAMUSPATH")+@"\"
sysinfo.currentdir = NUTTYTHALAMUSPATH + "..\Plugins"

dll = NUTTYTHALAMUSPATH + "..\Plugins\RiverFlow.dll"
assembly = dotnet.loadAssembly(dll)

dll = NUTTYTHALAMUSPATH + "..\Plugins\Newtonsoft.Json.dll"
assembly = dotnet.loadAssembly(dll)

dll = NUTTYTHALAMUSPATH + "..\Plugins\JsonObjectStream.dll"
assembly = dotnet.loadAssembly(dll)

dll = NUTTYTHALAMUSPATH + "..\Plugins\CookComputing.XmlRpcV2.dll"
assembly = dotnet.loadAssembly(dll)

dll = NUTTYTHALAMUSPATH + "..\Plugins\MathNet.Iridium.dll"
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
global channelProxies = dotNetObject "System.Collections.Hashtable"
gc()

timeConfiguration.realTimePlayback = false

fileIn (NUTTYTHALAMUSPATH+"3dsmax\NuttySkeleton.ms")

if (nutty != undefined and nutty.ShutingDown) then (
	nutty = undefined
)
if (nutty==undefined) then (
	local p = dotNetObject "System.String"(NUTTYTHALAMUSPATH + "..\\")
	local char = dotNetObject "System.String"("")
	global nutty = dotNetObject "NuttyThalamus.NuttyThalamusPlugin" p char
	dotNet.setLifetimeControl nutty #dotnet
	sleep 0.5
	callbacks.addScript #preSystemShutdown "nutty.Dispose()" id:#disposeNutty
) 
else 
(
	dotNet.removeAllEventHandlers nutty
)

global newSkeletonNodes = #()
global newSkeletonNodesCount = 0
function MaxAddSkeletonNode dn_nodeName dn_nodeParent dn_pX dn_pY dn_pZ dn_rotationOffset dn_axis =
(
	nodeName = dn_nodeName.ToString()
	nodeParent = dn_nodeParent.ToString()
	channelNamespace = nutty.ChannelNamespace(nodeName)
	pX = dn_pX.ToString() as Float
	pY = dn_pY.ToString() as Float
	pZ = dn_pZ.ToString() as Float
	rotationOffset = dn_rotationOffset.ToString() as Float
	axis = 0
	if (dn_axis.ToString() == "RotX") then axis = 1
	else if (dn_axis.ToString() == "RotY") then axis = 2
	else if (dn_axis.ToString() == "RotZ") then axis = 3
	else if (dn_axis.ToString() == "Translate") then axis = -1
	channelsProxy = (getnodebyname channelNamespace)
	isChannelProxy = false
	if (axis == 0 and (channelsProxy!=undefined) and (custAttributes.count channelsProxy)>0) then
	(
		channelName = nutty.ChannelName(nodeName)
		channelsProxy = (custAttributes.get channelsProxy 1)
		if (hasProperty channelsProxy channelName) then
		(
			global channelProxies
			channelProxies.add channelName channelNamespace 
			format "Set ChannelProxy % for %\n" channelName channelNamespace
			isChannelProxy = true
		)
	)
	if (not isChannelProxy) do
	(
		format "AddSkeletonNode %, %, [% % %] % %\n" nodeName nodeParent pX pY pZ rotationOffset axis
		with redraw off CreateSkeletonNode nodeName nodeParent pX pY pZ rotationOffset axis
	)
)

function GetChannelsFromProxies =
(
	proxyAttributes = undefined
	proxyName = ""
	global channelProxies
	it = channelProxies.GetEnumerator()
	dict = dotnetobject "System.Collections.Generic.Dictionary`2[System.String,System.Double]"
	while it.MoveNext() do
	(
		if (proxyName != it.current.value) then
		(
			proxyName = it.current.value
			proxyAttributes = (custAttributes.get (getnodebyname proxyName) 1)
		)
		dict.add (proxyName + "." + it.current.key) (getProperty proxyAttributes it.current.key)
		--format "% %" proxyName + "." + it.current.key (getProperty proxyAttributes it.current.key)
	)
	return dict
)

function GetAnimationFrame =
(
	skp = GetSkeletonPose()
	chp = GetChannelsFromProxies()
	it = chp.GetEnumerator()
	while it.MoveNext() do
	(
		skp.add it.current.key it.current.value
	)
	skp
)


function VerifyOutput =
(
	if (nutty.NuttyManager.Output.ToString() == "NuttyJsonStreamOutput") then
	(
		if (not nutty.NuttyManager.Output.JsonOut.Running) then nutty.NuttyManager.Output.JsonOut.Start()
		if (not nutty.NuttyManager.Output.JsonIn.Listening) then nutty.NuttyManager.Output.JsonIn.Start()
	)
)

function MaxSkeletonFinished =
(
	SkeletonFinished()
	af = GetAnimationFrame()
	it = af.GetEnumerator()
	print "\nCollected Animation Channels:\n"
	while it.MoveNext() do
	(
		format "% = %\n" it.current.key it.current.value
	)
)

function GetPosture =
(
	nutty.SavePosture (GetAnimationFrame())
)

function GetAnimation =
(
	i = 0
	sliderTime = animationRange.start
	nutty.SaveAnimationStart (dotNetobject "System.Int32"(animationRange.end-animationRange.start)) (dotNetobject "System.Int32"(frameRate))
	for f = animationRange.start to animationRange.end do
	(
		pose = GetAnimationFrame()
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
			queuedFrame = undefined
		)
	)
	catch
	(
		print "failed to queue animate"
	)
)

dotnet.addeventhandler nutty "GetAnimationEvent" GetAnimation
dotnet.addeventhandler nutty "ConnectSkeletonNodeEvent" MaxAddSkeletonNode
dotnet.addeventhandler nutty "SkeletonFinishedEvent" MaxSkeletonFinished
dotnet.addeventhandler nutty "AnimateEvent" QueueSkeletonAnimate
dotnet.addeventhandler nutty "GetPostureEvent" GetPosture
dotnet.addeventhandler nutty "ChangeControlBodyEvent" ChangeControlBody
dotnet.addeventhandler nutty "PrintEvent" Debug

global windowActive = true

global nuttymax_redrawviews_callback_function
unregisterRedrawViewsCallback nuttymax_redrawviews_callback_function
fn nuttymax_redrawviews_callback_function = (

	if (NuttyMaxControlBody) then
	(
		pose = GetAnimationFrame()
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