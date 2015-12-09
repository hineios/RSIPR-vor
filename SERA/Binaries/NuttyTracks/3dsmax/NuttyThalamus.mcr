macroScript NuttyThalamus
	enabledIn:#("max", "viz", "vizr")
	category:"Nutty"
	internalCategory:"NuttyTracks"
	toolTip:"NuttyThalamus"
	buttontext:"NuttyThalamus"
(
	NUTTYTHALAMUSPATH = systemTools.getEnvVariable("NUTTYTHALAMUSPATH")+@"\"
	fileIn (NUTTYTHALAMUSPATH+"3dsmax\NuttyThalamus.ms")
)