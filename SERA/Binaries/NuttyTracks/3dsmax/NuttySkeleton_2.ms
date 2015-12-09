global skeletonNodes = #()
global skeletonNodeOffset = dotNetObject "System.Collections.Hashtable"
global skeletonNodesCount = 0
global skeletonNodeAxis = dotNetObject "System.Collections.Hashtable"
global skeletonNodeInitialValue = dotNetObject "System.Collections.Hashtable"

global skeletonObjects = #()

SkeletonAnimate = undefined
SkeletonReady = false

function SkeletonFinished =
(
	for i = 1 to skeletonNodesCount do (
		axis = skeletonNodeAxis.Item[skeletonNodes[i]]
		format "%: %\n" skeletonNodes[i] skeletonNodeInitialValue.Item[skeletonNodes[i]]
		if ((getnodebyname skeletonNodes[i]).rotation.controller.keys.count == 0 and (axis>=1 and axis <= 3)) do (getnodebyname skeletonNodes[i]).rotation.controller[axis].value = skeletonNodeInitialValue.Item[skeletonNodes[i]]
	)
	SkeletonReady = true
	print "Skeleton created"
)

function setNodeRotationOffset n axis rotationOffset =
(
	if (axis==4) then (
		n.rotation.x_rotation = rotationOffset
	) else if (axis==5) then (
		n.rotation.y_rotation = rotationOffset
	)
	else (
		n.rotation.z_rotation = rotationOffset
	)
	--ResetTransform n
)

function configureSkeletonNodeGizmo gizmo x_rotation y_rotation z_rotation axis rotationOffset =
(
	format "transform '%' '%' '%'\n" x_rotation y_rotation z_rotation
	gizmo.rotation.x_rotation = x_rotation
	gizmo.rotation.y_rotation = y_rotation
	gizmo.rotation.z_rotation = z_rotation
	ResetTransform gizmo
	setNodeRotationOffset gizmo axis rotationOffset 
	ResetTransform gizmo
	gizmo.displayByLayer = true
	--freeze gizmo
	gizmo.position.controller = position_constraint()
	gizmo.rotation.controller = orientation_constraint()
	gizmo.rotation.controller.relative = true
	return gizmo
)



function CreateRotationalSkeletonNode nodeName parent pX pY pZ rotationOffset minAngle maxAngle axis =
(
	if (axis>=4 and axis <= 6) then (
		--radius of needle
		local bigSize = 20
		--radius of cylinder of 
		local smallSize = 10
		
		/*if node already exists, just connect it, dont re-create it*/
		if ((getnodebyname nodeName)!=undefined) then 
		(
			format "Connect to Rotational Node '%', parent '%'\n" nodeName parent
			 skeletonNodeInitialValue.Add nodeName rotationOffset
			 return (getnodebyname nodeName)
		)
		else
		(
			format "Create Rotational Node '%', parent '%', axis = '%', offset = '%'\n" nodeName parent axis rotationOffset
			local x_rotation = 0
			local y_rotation = 0
			local z_rotation = 0
			if (axis == 4) then (
				x_rotation = 90
			) else if (axis == 5) then (
				z_rotation = 90
			) else if (axis == 6) then (
				y_rotation = -90
				z_rotation = 90
			)
			if ((pY < 0) or (parent!="" and (getnodebyname parent).position.y < 0)) then (
				if (axis == 4) then (
					z_rotation = 180
				) else if (axis == 5) then (
					x_rotation = 90
				)
			)
						
			--first create and orient the needle
			local p = pyramid()
			p.width = 5
			p.depth = 5
			p.height = bigSize
			p = configureSkeletonNodeGizmo p x_rotation y_rotation z_rotation axis rotationOffset
			
			--create the angle limit slice cylinder
			local slice = Cylinder()
			slice.height = 3
			slice.radius = (smallSize+bigSize)/2
			slice.slice = on
			slice.sliceFrom = maxAngle
			slice.sliceTo = minAngle
			slice.wirecolor = color 200 200 200
			slice.position.z = -slice.height/2
			slice.pivot = [0,0,0]
			slice.rotation.y_rotation = -90
			ResetTransform slice
			slice = configureSkeletonNodeGizmo slice x_rotation y_rotation z_rotation axis rotationOffset

			local newNode = Cylinder()		
			newNode.radius = smallSize
			newNode.height = 5
			newNode.pivot = [0,0,newNode.height/2]
			newNode.position.z = 12.5
			newnode.rotation.y_rotation = -90
			ResetTransform newnode
			newnode.rotation.x_rotation = x_rotation
			newnode.rotation.y_rotation = y_rotation
			newnode.rotation.z_rotation = z_rotation
			p.wirecolor = newnode.wirecolor
			ResetTransform newNode
			
			setNodeRotationOffset newNode axis rotationOffset

			if (parent!="") do
			(
				newNode.transform = (getnodebyname parent).transform
				newNode.parent = (getnodebyname parent)
			) 
			
			
			p.position.controller.appendTarget newNode 100
			p.rotation.controller.appendTarget newNode 100
			slice.position.controller.appendTarget newNode 100
			--slice.rotation.controller.appendTarget newNode 100
			
			join skeletonObjects #(slice,p, newNode)
			
			BuildSkeletonNode newNode nodeName parent pX pY pZ rotationOffset minAngle maxAngle axis 
			
			return newNode
		)
	)
)

function CreateTranslationalSkeletonNode nodeName parent pX pY pZ rotationOffset minT maxT axis =
(
)

function CreateStaticSkeletonNode nodeName parent pX pY pZ =
(
	local staticNode = box()
	join skeletonObjects #(staticNode)
	freeze staticNode
	skeletonNodeInitialValue.Add nodeName 0
	return staticNode
)

function BuildSkeletonNode newNode nodeName parent pX pY pZ offset minValue maxValue axis  = 
(

	newNode.name = nodeName
	
	local transformlocks = #{}
	for i = 1 to 10 do if (axis!=i) do append transformlocks i
	setTransformLockFlags newNode transformlocks
	
	
	
	newNode.pos.controller[1].value = pX
	newNode.pos.controller[2].value = pY
	newNode.pos.controller[3].value = pZ

	tIndex = axis/4
	
	newNode.transform.controller[tIndex+1][axis-tIndex*3].controller = float_limit ()
	newNode.transform.controller[tIndex+1][axis-tIndex*3].controller.upper_limit = degToRad maxValue
	newNode.transform.controller[tIndex+1][axis-tIndex*3].controller.lower_limit = degToRad minValue
	
	if (parent !="") do
	(
		local s = splineShape()
		addNewSpline s
		addKnot s 1 #corner #line newNode.position
		addKnot s 1 #corner #line newNode.parent.position
		s.render_displayRenderMesh = true
		s.render_thickness = 5
		updateshape s
		freeze s
		s.parent = newNode.parent
	)
	
	skeletonNodeInitialValue.Add nodeName zeroValue
	skeletonNodesCount = skeletonNodesCount+1
	skeletonNodes[skeletonNodesCount] = nodeName
	skeletonNodeOffset.Add nodeName offset
	skeletonNodeAxis.Add nodeName axis
)


function CreateSkeletonNode nodeName parent pX pY pZ offset axis =
(
	SkeletonReady = false
	if (parent == "" and nodeName != "") do 
	(
		print "New skeleton"
		local layer = LayerManager.getLayerFromName "NuttyMax"
		if (layer == undefined) do layer = LayerManager.newLayerFromName  "NuttyMax"
		layer.current = true
		layer.showFrozenInGray = false
		skeletonNodes = #()
		skeletonNodeOffset = dotNetObject "System.Collections.Hashtable"
		skeletonObjects = #()
		skeletonNodeAxis = dotNetObject "System.Collections.Hashtable"
		skeletonNodeInitialValue = dotNetObject "System.Collections.Hashtable"
		skeletonNodesCount = 0
	)
	
	local maxAngle = 40
	local minAngle = -40
	local minT = 0
	local maxT = 15
	local newNode
	
	if (axis>=4 and axis <= 6) then newNode = CreateRotationalSkeletonNode nodeName parent pX pY pZ offset minAngle maxAngle axis
	else if (axis>=1 and axis <= 3) then newNode = CreateTranslationalSkeletonNode nodeName parent pX pY pZ offset minT maxT axis
	else newNode = CreateStaticSkeletonNode nodeName parent pX pY pZ

	nodeName
)

function sk =
(
	CreateSkeletonNode "Pan" "" 0 0 0 30 6
	CreateSkeletonNode "Tilt" "Pan" 0 0 20 80 4
)

function SkeletonAnimate frame =
(
	if (SkeletonReady) do (
		with redraw off (
			try (
				it = frame.GetEnumerator()
				while it.MoveNext() do
				(
					channelName = it.current.key
					
					currentNode = (getnodebyname channelName)
					if (currentNode != $) and (currentNode!=undefined) do (
						angleValue = it.current.value
						if (skeletonNodeAxis.ContainsKey channelName) do
						(
							axis = skeletonNodeAxis.Item[channelName]
							if (axis>=1 and axis <=3) do
							(
								currentNode.rotation.controller[axis].value = angleValue
							)
						)
					)
				)
			)
			catch
			(
				print "Failed to animate skeleton."
			)
		)
	)
)


function GetSkeletonPose =
(

	dict = dotnetobject "System.Collections.Generic.Dictionary`2[System.String,System.Double]"
	for i = 1 to skeletonNodesCount do
	(
		axis = skeletonNodeAxis.Item[skeletonNodes[i]]
		if (axis>=1 and axis <=3) do (
			dict.add	skeletonNodes[i] (getnodebyname skeletonNodes[i]).rotation.controller[skeletonNodeAxis.Item[skeletonNodes[i]]].value
		)
	)
	
-- 	it = dict.GetEnumerator()
-- 	while it.MoveNext() do
-- 	(
-- 		channelName = it.current.key
-- 		angleValue = it.current.value as double
-- 	)
	
	return dict
)
