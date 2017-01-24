import naoqi
from naoqi import ALBroker
from naoqi import ALModule
from naoqi import ALProxy

import time

from threading import Thread

instance = None

def log(text):
    if (instance!=None): instance.nao.log("EyesModule: " + text)
    else: print text
    return;

EyeRot_None = 0
EyeRot_In = 1
EyeRot_Out = 2

global ledsNames
ledsNames = dict()

class EyeState(object):
    leftEye = True
    rightEye = True
    leftUpperLid = False
    leftLowerLid = False
    rightUpperLid = False
    rightLowerLid = False
    leftRotation = EyeRot_None
    rightRotation = EyeRot_None

    def __str__(self):
     return "EyeState[LE:{0} RE:{1} LUL:{2} LLL:{3} RUL:{4} RLL:{5} LRot:{6} RRot:{7}".format(self.leftEye, self.rightEye, self.leftUpperLid, self.leftLowerLid, self.rightUpperLid, self.rightLowerLid, self.leftRotation, self.rightRotation)

    def setLeftEye(self, state):
        self.leftEye = state
        if (not state):
            self.leftUpperLid = False
            self.leftLowerLid = False

    def setRightEye(self, state):
        self.rightEye = state
        if (not state):
            self.rightUpperLid = False
            self.rightLowerLid = False

    def setLeftUpperLid(self, state):
        if (not self.leftEye and not state):
            self.leftEye = True
            self.leftUpperLid = False
            self.leftLowerLid = True
        else:
            self.leftUpperLid = state

    def setLeftLowerLid(self, state):
        if (not self.leftEye and not state):
            self.leftEye = True
            self.leftUpperLid = True
            self.leftLowerLid = False
        else:
            self.leftLowerLid = state

    def setRightUpperLid(self, state):
        if (not self.rightEye and not state):
            self.rightEye = True
            self.rightUpperLid = False
            self.rightLowerLid = True
        else:
            self.rightUpperLid = state

    def setRightLowerLid(self, state):
        if (not self.rightEye and not state):
            self.rightEye = True
            self.rightUpperLid = True
            self.rightLowerLid = False
        else:
            self.rightLowerLid = state

    def setLeftRotation(self, rot):
        self.leftRotation = rot

    def setRightRotation(self, rot):
        self.rightRotation = rot


class EyesModule(object):

    ExpressionDefaultTime = 500
    DefaultExpression = "neutral"
    BlinkExpression = "shut"

    nao = None
    proxy = None
    bShutdown = False
    expressionTime = 0

    eyesIntensity = 1.0
    lastIntensity = 0
    
    currentExpression = ""
    savedStates = dict()

    def __init__(self, naoServer):
        global instance
        instance = self
        self.nao = naoServer
        return

    def start(self):
        log("Connecting LedsProxy...")
        try:
            self.proxy = ALProxy("ALLeds")
            log(self.proxy.version())

            #print str(eyes.proxy.listLEDs())

            self.lastLeftleds = set()
            self.lastRightleds = set()

            #self.leds[1]=("LeftFaceLed1", "RightFaceLed8")
            #self.leds[2]=("LeftFaceLed8", "RightFaceLed7")
            #self.leds[3]=("LeftFaceLed7", "RightFaceLed6")
            #self.leds[4]=("LeftFaceLed6", "RightFaceLed5")
            #self.leds[5]=("LeftFaceLed5", "RightFaceLed4")
            #self.leds[6]=("LeftFaceLed4", "RightFaceLed3")
            #self.leds[7]=("LeftFaceLed3", "RightFaceLed2")
            #self.leds[8]=("LeftFaceLed2", "RightFaceLed1")
            global ledsNames

            ledsNames["eyeRight1"]="RightFaceLed8"
            ledsNames["eyeRight2"]="RightFaceLed1"
            ledsNames["eyeRight3"]="RightFaceLed2"
            ledsNames["eyeRight4"]="RightFaceLed3"
            ledsNames["eyeRight5"]="RightFaceLed4"
            ledsNames["eyeRight6"]="RightFaceLed5"
            ledsNames["eyeRight7"]="RightFaceLed6"
            ledsNames["eyeRight8"]="RightFaceLed7"

            ledsNames["eyeLeft1"]="LeftFaceLed1"
            ledsNames["eyeLeft2"]="LeftFaceLed8"
            ledsNames["eyeLeft3"]="LeftFaceLed7"
            ledsNames["eyeLeft4"]="LeftFaceLed6"
            ledsNames["eyeLeft5"]="LeftFaceLed5"
            ledsNames["eyeLeft6"]="LeftFaceLed4"
            ledsNames["eyeLeft7"]="LeftFaceLed3"
            ledsNames["eyeLeft8"]="LeftFaceLed2"

            self.CreateBaseEyeStates()

            th = Thread(target=self.eyesCycle)
            th.start()

            #self.nao.NotifyEyeShapeList(self.savedStates.keys())

        except Exception, Argument:
            log("Failed: " + str(Argument))
            self.proxy = None
            return False
        return True

    def dispose(self):
        log("Disconnect LedsProxy...")
        self.bShutdown = True

    def eyesCycle(self):
        while(not self.bShutdown):
            if (self.expressionTime>=100):
                self.expressionTime = self.expressionTime-100
            else:
                if (self.currentExpression != ""):
                    self.RenderEyeState(self.savedStates[self.DefaultExpression])
                    
                    if (self.currentExpression==self.BlinkExpression): self.expressionTime = self.ExpressionDefaultTime*6
                    else: self.expressionTime = self.ExpressionDefaultTime
                    self.currentExpression = ""
                else:
                    self.ExpressState(self.BlinkExpression, 100)
            time.sleep(0.1)
        log("Exited eye cycle")

    def SetIntensity(self, d_value):
        self.eyesIntensity = d_value
        if (self.currentExpression == ""): self.RenderEyeState(self.savedStates[self.DefaultExpression])
        else: self.RenderEyeState(self.savedStates[self.currentExpression])

    def ShiftState(self, stateName):
        self.DefaultExpression = stateName
        self.ExpressState(stateName)

    def ExpressState(self, stateName, duration=0):
        if (stateName!=self.BlinkExpression): log("ExpressState {0}, t={1}".format(stateName, duration))
        if (duration<=0): duration = self.ExpressionDefaultTime
        if (stateName in self.savedStates):
            self.currentExpression = stateName
            self.RenderEyeState(self.savedStates[stateName])
            self.expressionTime = duration

    def CreateBaseEyeStates(self):
        state = EyeState()
        state.setLeftUpperLid(True)
        state.setRightUpperLid(True)
        self.savedStates["neutral"] = state
        self.savedStates[""] = state

        state = EyeState()
        state.setLeftEye(False)
        state.setRightEye(False)
        self.savedStates["shut"] = state

        state = EyeState()
        state.setRightLowerLid(True)
        state.setRightRotation(EyeRot_In)
        state.setLeftRotation(EyeRot_In)
        state.setLeftLowerLid(True)
        self.savedStates["happiness"] = state

        state = EyeState()
        state.setRightLowerLid(True)
        state.setRightUpperLid(True)
        state.setRightRotation(EyeRot_In)
        state.setLeftRotation(EyeRot_In)
        state.setLeftUpperLid(True)
        state.setLeftLowerLid(True)
        self.savedStates["anger"] = state

        state = EyeState()
        #state.setRightLowerLid(True)
        state.setRightUpperLid(True)
        state.setRightRotation(EyeRot_Out)
        state.setLeftRotation(EyeRot_Out)
        state.setLeftUpperLid(True)
        #state.setLeftLowerLid(True)
        self.savedStates["sadness"] = state

        state = EyeState()
        state.setRightLowerLid(True)
        state.setRightRotation(EyeRot_Out)
        state.setLeftRotation(EyeRot_Out)
        state.setLeftLowerLid(True)
        self.savedStates["fear"] = state

        state = EyeState()
        state.setLeftEye(True)
        state.setRightEye(True)
        self.savedStates["surprise"] = state

        state = EyeState()
        state.setRightLowerLid(True)
        state.setLeftRotation(EyeRot_In)
        state.setLeftUpperLid(True)
        state.setLeftLowerLid(True)
        self.savedStates["disgust"] = state

    def DemoEmotions(self):
        self.RenderEyeState(self.savedStates["neutral"])
        time.sleep(1)
        self.RenderEyeState(self.savedStates["anger"])
        time.sleep(2)
        self.RenderEyeState(self.savedStates["disgust"])
        time.sleep(2)
        self.RenderEyeState(self.savedStates["fear"])
        time.sleep(2)
        self.RenderEyeState(self.savedStates["happiness"])
        time.sleep(2)
        self.RenderEyeState(self.savedStates["sadness"])
        time.sleep(2)
        self.RenderEyeState(self.savedStates["surprise"])
        time.sleep(2)
        self.RenderEyeState(self.savedStates["neutral"])

    def RenderEyeState(self, state):
        
        #log("RenderEyeState " + str(state))
        global ledsNames
        self.rightleds = []
        self.leftleds = []
        if (state.rightEye == False):
            self.proxy.off("RightFaceLeds")
            self.lastRightleds = set()
        else:
            if (state.rightRotation == EyeRot_In):
                rightUpperLid = [ledsNames["eyeRight1"], ledsNames["eyeRight8"], ledsNames["eyeRight7"]]
                rightLowerLid = [ledsNames["eyeRight3"], ledsNames["eyeRight4"], ledsNames["eyeRight5"]]
            elif (state.rightRotation == EyeRot_Out):
                rightUpperLid = [ledsNames["eyeRight1"], ledsNames["eyeRight2"], ledsNames["eyeRight3"]]
                rightLowerLid = [ledsNames["eyeRight5"], ledsNames["eyeRight6"], ledsNames["eyeRight7"]]
            else:
                rightUpperLid = [ledsNames["eyeRight8"], ledsNames["eyeRight1"], ledsNames["eyeRight2"]]
                rightLowerLid = [ledsNames["eyeRight4"], ledsNames["eyeRight5"], ledsNames["eyeRight6"]]
            if (state.rightUpperLid):
                self.rightleds = self.rightleds + rightUpperLid
            if (state.rightLowerLid):
                self.rightleds = self.rightleds + rightLowerLid
        
            s = set(self.rightleds)
            if (not (s.issubset(self.lastRightleds) and self.lastRightleds.issubset(s) and (self.lastIntensity==self.eyesIntensity))):
                self.lastRightleds = s
                self.proxy.setIntensity("RightFaceLeds", self.eyesIntensity)
                if(len(self.rightleds)>0):
                    self.proxy.createGroup("rightEyeMask", self.rightleds)
                    self.proxy.off("rightEyeMask")

        if (state.leftEye == False):
            self.proxy.off("LeftFaceLeds")
            self.lastLeftleds = set()
        else:
            if (state.leftRotation == EyeRot_In):
                leftUpperLid = [ledsNames["eyeLeft8"], ledsNames["eyeLeft1"], ledsNames["eyeLeft7"]]
                leftLowerLid = [ledsNames["eyeLeft4"], ledsNames["eyeLeft5"], ledsNames["eyeLeft3"]]
            elif (state.leftRotation == EyeRot_Out):
                leftUpperLid = [ledsNames["eyeLeft3"], ledsNames["eyeLeft1"], ledsNames["eyeLeft2"]]
                leftLowerLid = [ledsNames["eyeLeft7"], ledsNames["eyeLeft5"], ledsNames["eyeLeft6"]]
            else:
                leftUpperLid = [ledsNames["eyeLeft8"], ledsNames["eyeLeft1"], ledsNames["eyeLeft2"]]
                leftLowerLid = [ledsNames["eyeLeft4"], ledsNames["eyeLeft5"], ledsNames["eyeLeft6"]]
            if (state.leftUpperLid):
                self.leftleds = self.leftleds + leftUpperLid
            if (state.leftLowerLid):
                self.leftleds = self.leftleds + leftLowerLid

            s = set(self.leftleds)
            if (not (s.issubset(self.lastLeftleds) and self.lastLeftleds.issubset(s) and (self.lastIntensity==self.eyesIntensity))):
                self.lastLeftleds = s
                self.proxy.setIntensity("LeftFaceLeds", self.eyesIntensity)
                if(len(self.leftleds)>0):
                    self.proxy.createGroup("leftEyeMask", self.leftleds)
                    self.proxy.off("leftEyeMask")
        self.lastIntensity = self.eyesIntensity

    def demo(self):
        state = EyeState()
        eyes.RenderEyeState(state)
        time.sleep(1)

        state.setLeftLowerLid(True)
        state.setRightLowerLid(True)
        eyes.RenderEyeState(state)
        time.sleep(1)

        state.setLeftRotation(EyeRot_In)
        eyes.RenderEyeState(state)
        time.sleep(1)

        state.setLeftRotation(EyeRot_Out)
        eyes.RenderEyeState(state)
        time.sleep(1)

        state.setRightRotation(EyeRot_In)
        eyes.RenderEyeState(state)
        time.sleep(1)

        state.setRightRotation(EyeRot_Out)
        eyes.RenderEyeState(state)
        time.sleep(1)

        state.setRightUpperLid(True)
        eyes.RenderEyeState(state)
        time.sleep(1)

        state = EyeState()
        state.setLeftUpperLid(True)
        state.setRightUpperLid(True)
        eyes.RenderEyeState(state)
        time.sleep(1)

#BROKER_IP = "localhost"
#BROKER_PORT = 9559

#myBroker = ALBroker("eyesBroker", "0.0.0.0", 0, BROKER_IP, BROKER_PORT)
#eyes = EyesModule(None)
#eyes.start()
#eyes.DemoEmotions()



