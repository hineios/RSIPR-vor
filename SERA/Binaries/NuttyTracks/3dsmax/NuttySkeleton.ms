global skeletonNodes = #()
global skeletonNodesCount = 0
global skeletonNodeAxis = dotNetObject "System.Collections.Hashtable"
global skeletonNodeStartAngle = dotNetObject "System.Collections.Hashtable"
global newSkeletonNodesCount = 0
global newSkeletonNodes = #()

global skeletonObjects = #()

SkeletonAnimate = undefined
SkeletonReady = false

function SkeletonFinished =
(
	for i = 1 to skeletonNodesCount do (
		axis = skeletonNodeAxis.Item[skeletonNodes[i]]
		--format "%: %\n" skeletonNodes[i] skeletonNodeStartAngle.Item[skeletonNodes[i]]
		if ((getnodebyname skeletonNodes[i]).rotation.controller.keys.count == 0 and (axis>=1 and axis <= 3)) do (getnodebyname skeletonNodes[i]).rotation.controller[axis].value = skeletonNodeStartAngle.Item[skeletonNodes[i]]
	)
	SkeletonReady = true
	print "Skeleton created"
)

function CreateSkeletonNode nodeName parent pX pY pZ rotationOffset axis =
(
	SkeletonReady = false
	if (parent == "" and nodeName != "") do 
	(
		print "New skeleton"
		layer = LayerManager.getLayerFromName "NuttyMax"
		if (layer == undefined) do layer = LayerManager.newLayerFromName  "NuttyMax"
		layer.current = true
		layer.showFrozenInGray = false
		skeletonNodes = #()
		skeletonObjects = #()
		newSkeletonNodes = #()
		skeletonNodeAxis = dotNetObject "System.Collections.Hashtable"
		skeletonNodeStartAngle = dotNetObject "System.Collections.Hashtable"
		newSkeletonNodesCount = 0
		skeletonNodesCount = 0
	)
	
	if ((getnodebyname nodeName)!=undefined) then 
	(
		format "Connect to Node '%', parent '%'\n" nodeName parent
		if (axis>=1 and axis <= 3) then skeletonNodeStartAngle.Add nodeName rotationOffset
	)
	else
	(
		bigSize = 40
		smallSize = 25
		format "Create Node '%', parent '%'\n" nodeName parent
		if (axis>=1 and axis<=3) then (
			b = box()
			p = pyramid()
			p.width = b.width
			p.depth = b.length
			p.height = bigSize

			if (axis == 1) then (
				b.width = bigSize
				p.rotation.x_rotation = 90
			) else if (axis == 2) then (
				b.length = bigSize
				p.rotation.z_rotation = 90
			) else (
				b.height = bigSize
				p.rotation.y_rotation = 90
			)
			
			if ((pY < 0) or (parent!="" and (getnodebyname parent).position.y < 0)) then (
				if (axis == 1) then (
					p.rotation.z_rotation = 180
				) else if (axis == 2) then (
					p.rotation.x_rotation = 90
				)
			) else if (axis == 2) then (
				p.rotation.x_rotation = 90
			)
			
			CenterPivot b
			b.displayByLayer = true
			p.displayByLayer = true
			freeze b
			freeze p
			
			
			p.position.controller = position_constraint()
			p.rotation.controller = orientation_constraint()
			p.rotation.controller.relative = true
			p.position.controller.appendTarget b 100
			p.rotation.controller.appendTarget b 100
			
			
			b.position.controller = position_constraint()
			b.rotation.controller = orientation_constraint()
			
			newNode = Cylinder()
			b.position.controller.appendTarget newNode 100
			b.rotation.controller.appendTarget newNode 100
			
			newNode.radius = 10
			newNode.height = 60
			newNode.pivot = [0,0,newNode.height/2]
			newNode.position.z = 12.5
			if (axis == 1) do newnode.rotation.y_rotation = 90
			if (axis == 2) do newnode.rotation.x_rotation = 90
			
			join skeletonObjects #(b,p, newNode)
			
		) 
		else 
		(
			newNode = box()
			join skeletonObjects #(newNode)
			freeze newNode
		)
		ResetTransform newNode 
		newNode.name = nodeName
		
		transformlocks = #{1,2,3}
		if (axis!=1) do append transformlocks 4
		if (axis!=2) do append transformlocks 5
		if (axis!=3) do append transformlocks 6
		
		setTransformLockFlags newNode transformlocks
		
		if (parent!="") do
		(
			newNode.transform = (getnodebyname parent).transform
			newNode.parent = (getnodebyname parent)
		) 
		
		newNode.pos.controller[1].value = pX
		newNode.pos.controller[2].value = pY
		newNode.pos.controller[3].value = pZ
		
		if (parent !="") do
		(
			s = splineShape()
			addNewSpline s
			addKnot s 1 #corner #line newNode.position
			addKnot s 1 #corner #line newNode.parent.position
			s.render_displayRenderMesh = true
			s.render_thickness = 5
			updateshape s
			freeze s
			s.parent = newNode.parent
		)
		newSkeletonNodesCount = newSkeletonNodesCount+1
		newSkeletonNodes[newSkeletonNodesCount] = nodeName
		if (axis>=1 and axis <= 3) then skeletonNodeStartAngle.Add nodeName rotationOffset
		/*ResetTransform newNode*/
	)
	
	skeletonNodesCount = skeletonNodesCount+1
	skeletonNodes[skeletonNodesCount] = nodeName

	if (axis<1 or axis>3) then skeletonNodeStartAngle.Add nodeName 0
	skeletonNodeAxis.Add nodeName axis
	nodeName
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
							if (axis>=1 and axis <=3) then
							(
								currentNode.rotation.controller[axis].value = angleValue
							)
							else if (axis == -1) then 
							(
								currentNode.pos.controller.Path_Constraint.controller.PERCENT = angleValue
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
		
		if (axis==-1) then
		(
			dict.add	skeletonNodes[i] (getnodebyname skeletonNodes[i]).pos.controller.Path_Constraint.controller.PERCENT
		)
		else if (axis>=1 and axis <=3) do
		(
			if ((getnodebyname skeletonNodes[i]).rotation.controller as String == "Controller:Rotation_List") then
			(
				ctrlIndex = -1
				for j = 1 to (getnodebyname skeletonNodes[i]).rotation.controller.count do
				(
					if ((getnodebyname skeletonNodes[i]).rotation.controller[j] as String == "SubAnim:Euler_XYZ") then
					(
						ctrlIndex = j
						exit
					) 
				)
				if (ctrlIndex != -1) then
				(
					val = (getnodebyname skeletonNodes[i]).rotation.controller[ctrlIndex][axis].value
					--format "'%': '%' '%' '%'\n" skeletonNodes[i] (getnodebyname skeletonNodes[i]).rotation.controller[ctrlIndex] axis val
					dict.add	skeletonNodes[i] val
				)
				else
				(
					euler = quatToEuler((getnodebyname skeletonNodes[i]).transform.rotation)
					val = 0
					if (axis==1) then val = euler.x
					else if (axis==2) then val = euler.y
					else val = euler.z
					--format "'%': '%'\n" skeletonNodes[i] val
					dict.add	skeletonNodes[i] val
				)
			)
			else
			(
				--format "'%': '%'\n" skeletonNodes[i] (getnodebyname skeletonNodes[i]).rotation.controller[axis].value
				dict.add	skeletonNodes[i] (getnodebyname skeletonNodes[i]).rotation.controller[axis].value
			)
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
