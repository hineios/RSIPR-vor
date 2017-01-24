function getSkeletonPose =
(
	channels = dotnetobject "System.String[]" newSkeletonNodesCount
	values = dotnetobject "System.String[]" newSkeletonNodesCount
	for i = 1 to newSkeletonNodesCount do
	(
		channels.SetValue i skeletonNodes[i]
		values.SetValue i ((getnodebyname skeletonNodes[i]).rotation.controller[skeletonNodeAxis.Item[skeletonNodes[i]]].value as string)
	)
)