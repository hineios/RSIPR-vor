import naoqi
from naoqi import ALBroker
from naoqi import ALModule
from naoqi import ALProxy

import math
import time
import thread
from threading import Timer

instance = None
import threading
from threading import Lock

import json

mutex = Lock()
hBmutex = Lock()

def log(text):
    instance.nao.log("MotionModule: " + text)
    return

class HeartbeatEcho:
    def __init__(self, ticks, joints, values):
        self.Ticks = ticks
        self.Joints = joints
        self.Values = values
    def to_JSON(self):
        return json.dumps(self, default=lambda o: o.__dict__, sort_keys=True)

class MotionModule(object):

    nao = None
    proxy = None
    joints = set()
    bodyNames = []
    bodyLimitsDict = dict()
    heartbeatTick = 0
    lastTickTime = time.clock()

    def __init__(self, naoServer):
        global instance
        instance = self
        self.nao = naoServer
        return

    def start(self):
        log("Connecting MotionProxy...")
        try:
            global myBroker
            myBroker = ALBroker("motionBroker", "0.0.0.0", 0, self.nao.brokerIp(), self.nao.brokerPort())

            self.performingGazeId = None
            self.frameCount = 0
            self.heartbeatCount = 0
            self.fps = 0
            self.hbFps = 0
            self.fpsTimer = Timer(1, self.fpsCounter)

            self.proxy = ALProxy("ALMotion")
            log(self.proxy.version())
            self.bodyNames = self.proxy.getBodyNames("Body")
            bodyLimits = self.proxy.getLimits("Body")

            for i in range(0, len(self.bodyNames)):
                self.joints.add(self.bodyNames[i])
                self.bodyLimitsDict[self.bodyNames[i]]=bodyLimits[i]
            log("Robot DoFs: " + str(self.joints))
            
            #self.proxy.stiffnessInterpolation("Body", 1.0, 2.0)
            
            try:
                self.proxy.setSmartStiffnessEnabled(True)
            except Exception, Argument:
                log("Robot does not support smart stiffness.")

            self.proxy.setCollisionProtectionEnabled("Arms", False)
            
            self.fpsTimer.start()
        except Exception, Argument:
            log("Failed: " + str(Argument))
            self.proxy = None
            return False
        return True

    def fpsCounter(self):
        self.fps = self.frameCount
        self.hbFps = self.heartbeatCount
        self.frameCount = 0
        self.heartbeatCount = 0
        if (self.fps > 0):
            log("FPS: " + str(self.fps))
        if (self.hbFps > 0):
            log("Heartbeat: " + str(self.hbFps))
        self.fpsTimer = Timer(1, self.fpsCounter)
        self.fpsTimer.start()

    def ExecuteAction(self, p, action):
        #log("execute action " + str(action) + ", " + str(action.s_ticks))
        if (type(action).__name__ == 'HeartbeatStruct'):
            thread.start_new_thread(self.Heartbeat, (action.d_ticks,0))
        elif (type(action).__name__ == 'GazeStruct'):
            thread.start_new_thread(self.Gaze, (action.s_id, action.d_horizontal, action.d_vertical, action.d_speed))
        elif (type(action).__name__ == 'PlayAnimationBufferStruct'):
            self.PlayAnimationBuffer(action.a_s_joints, action.a_d_angles, action.d_deltaTime)
        elif (type(action).__name__ == 'PointStruct'):
            thread.start_new_thread(self.Pointing, (action.s_id, action.d_horizontal, action.d_vertical, action.d_speed, action.s_mode))
        elif (type(action).__name__ == 'WavingStruct'):
            thread.start_new_thread(self.Waving, (action.s_id, action.d_horizontal, action.d_vertical, action.d_frequency, action.d_amplitude, action.d_duration, action.s_mode))
        elif (type(action).__name__ == 'HeadStruct'):
            thread.start_new_thread(self.Head, (action.s_id, action.s_lexeme, action.i_repetitions, action.d_amplitude, action.d_frequency))
        return

    def MakeHeartbeatEcho(self):
        if (hBmutex.acquire(False)):
            try:
                values = self.proxy.getAngles("Body", True)
                degreeAngles = []
                for i in range(0,len(values)):
                    degreeAngles.append(math.degrees(values[i]))
                self.heartbeatCount = self.heartbeatCount + 1
                return HeartbeatEcho(self.heartbeatTick + (time.time()-self.lastTickTime), self.bodyNames, degreeAngles)
                #self.nao.NotifyHeartbeatEcho(d_ticks, self.bodyNames, degreeAngles)
                #log("HB: " + str(self.heartbeatCount) + " time: " + str(time.time()-t))
            except Exception, Argument:
                log("[ERROR] MakeHeartbeatEcho: " + str(Argument))
            finally:
                hBmutex.release()
        return

    def Heartbeat(self, d_ticks, i_nada):
        if (hBmutex.acquire(False)):
            try:
                t = time.time()
                values = self.proxy.getAngles("Body", True)
                degreeAngles = []
                for i in range(0,len(values)):
                    degreeAngles.append(math.degrees(values[i]))
                self.heartbeatCount = self.heartbeatCount + 1
                self.nao.NotifyHeartbeatEcho(d_ticks, self.bodyNames, degreeAngles)
                #log("HB: " + str(self.heartbeatCount) + " time: " + str(time.time()-t))
            except Exception, Argument:
                log("[ERROR] Heartbeat: " + str(Argument))
            finally:
                hBmutex.release()
        return

    def Gaze(self, s_id, d_horizontal, d_vertical, d_speed):
        #autoIdle = self.nao.behavior._autoIdle
        #if (autoIdle):
        #   try:
        #        self.nao.behavior.switchIdle(False)
        #    except Exception, Argument:
        #        log("[ERROR] Gaze: " + str(Argument))
        try:
            jointList = ["HeadYaw", "HeadPitch"]
            angleList = [-d_horizontal * (math.pi / 180), -d_vertical * (math.pi / 180)]
            
            self.nao.NotifyGazeStart(s_id)
            self.proxy.angleInterpolationWithSpeed(jointList, angleList, min(1.0, d_speed * 1.0))
            self.nao.NotifyGazeEnd(s_id)
        except Exception, Argument:
            log("[ERROR] Gaze: " + str(Argument))

        #if (autoIdle):
        #    try:
        #        self.nao.behavior.switchIdle(True)
        #    except Exception, Argument:
        #        log("[ERROR] Gaze: " + str(Argument))
        return

    def PointArm(self, d_horizontal, d_vertical, s_mode, d_speed = 0.5):
        if (s_mode=="LeftHand"):
            jointList = ["LShoulderRoll", "LShoulderPitch", "LElbowRoll", "LElbowYaw", "LWristYaw", "LHand"]
        else:
            jointList = ["RShoulderRoll", "RShoulderPitch", "RElbowRoll", "RElbowYaw", "RWristYaw", "RHand"]
        angleList = [-d_horizontal * (math.pi / 180), -d_vertical * (math.pi / 180),0, 0, 0, 1]
        self.proxy.angleInterpolationWithSpeed(jointList, angleList, min(1.0, d_speed * 1.0))
        return angleList

    def Pointing(self, s_id, d_horizontal, d_vertical, d_speed, s_mode):
        mutex.acquire()
        log("Pointing")
        autoIdle = self.nao.behavior._autoIdle
        if (autoIdle):
            try:
                self.nao.behavior.switchIdle(False)
            except Exception, Argument:
                log("[ERROR] Point: " + str(Argument))
        try:
            self.nao.NotifyPointingStart(s_id)
            self.PointArm(d_horizontal, d_vertical, s_mode, d_speed)
            self.nao.NotifyPointingEnd(s_id)
        except Exception, Argument:
            log("[ERROR] Point: " + str(Argument))

        if (autoIdle):
            try:
                self.nao.behavior.switchIdle(True)
            except Exception, Argument:
                log("[ERROR] Point: " + str(Argument))

        mutex.release()
        return

    def Waving(self, s_id, d_horizontal, d_vertical, d_frequency, d_amplitude, d_duration, s_mode):
        mutex.acquire()
        log("Pointing")
        horizontal = -d_horizontal * (math.pi / 180)
        vertical = -d_vertical * (math.pi / 180)
        autoIdle = self.nao.behavior._autoIdle
        log("Waving started " + str(d_frequency) + " " + str(d_amplitude) + " " + str(d_duration))
        if (autoIdle):
            try:
                self.nao.behavior.switchIdle(False)
            except Exception, Argument:
                log("[ERROR] Waving: " + str(Argument))
        try:
            if (s_mode=="LeftHand"):
                jointList = ["LShoulderRoll", "LShoulderPitch", "LElbowRoll", "LElbowYaw", "LWristYaw", "LHand"]
            else:
                jointList = ["RShoulderRoll", "RShoulderPitch", "RElbowRoll", "RElbowYaw", "RWristYaw", "RHand"]

            
            #directing the arm
            angleList = self.PointArm(d_horizontal, d_vertical, s_mode)

            amplitudeRad = d_amplitude * (math.pi / 180)
            fps = 20
            t = 0.1
            X = 0
            angles = [[]] * 6                       #python access an array of arrays in a weird way, so for now this is done in this "simple" way 
            times = [[]] * 6
            j0a = []
            j1a = []
            j2a = []
            j3a = []
            j4a = []
            j5a = []
            j0t = []
            j1t = []
            j2t = []
            j3t = []
            j4t = []
            j5t = []
            while ((t) <= d_duration):
                print ("Angle: "+str(X/(math.pi/180))+" t: "+str(t)+" sin: "+str(math.sin(math.pi*t*d_frequency)))
                j0a.append(X)
                j1a.append(0)
                j2a.append(0)
                j3a.append(0)
                j4a.append(0)
                j5a.append(1)
                j0t.append(t)
                j1t.append(t)
                j2t.append(t)
                j3t.append(t)
                j4t.append(t)
                j5t.append(t)
                X = amplitudeRad * math.sin(math.pi*t*d_frequency)
                t = t + (1.0/fps)

            angles[0] = j0a
            angles[1] = j1a
            angles[2] = j2a
            angles[3] = j3a
            angles[4] = j4a
            angles[5] = j5a
            times[0] = j0t
            times[1] = j1t
            times[2] = j2t
            times[3] = j3t
            times[4] = j4t
            times[5] = j5t

                
            self.nao.NotifyWavingStart(s_id)
            self.proxy.angleInterpolation(jointList,angles,times, False)
            self.nao.NotifyWavingEnd(s_id)
        except Exception, Argument:
            log("[ERROR] Point: " + str(Argument))

        if (autoIdle):
            try:
                self.nao.behavior.switchIdle(True)
            except Exception, Argument:
                log("[ERROR] Point: " + str(Argument))

        mutex.release()
        return

    def Head(self, s_id, s_lexeme, i_repetitions, d_amplitude, d_frequency):
        if (s_lexeme == "NOD"):
            jointList = ["HeadPitch"]
        elif (s_lexeme == "SHAKE"):
            jointList = ["HeadYaw"]
        else:
            log("[ERROR] Head: wrong lexeme: "+s_lexeme)
            return

        amplitudeRad = d_amplitude * (math.pi / 180)

        duration = math.pow(d_frequency/i_repetitions,-1)
        fps = 20
        t = 0.1
        X = 0
        angles = []
        times = []
        while ((t) <= duration):
            angles.append(X)
            times.append(t)
            X = amplitudeRad * math.sin(math.pi*t*d_frequency)
            t = t+(1.0/fps)
            log("X: "+str(X/(math.pi / 180))+" t: "+str(t)) #+" t*freq: "+str(t*d_frequency)+" X: "+str(math.sin(t*d_frequency*90)))
            
    

        self.nao.NotifyHeadStart(s_id)
        self.proxy.angleInterpolation(jointList,angles,times, False)
        self.nao.NotifyHeadEnd(s_id)
        return

    def PlayAnimationBuffer(self, ticks, a_s_joints, a_d_angles, a_d_speed, d_deltaTime):
        if (mutex.acquire(False)):
            try:
                jointList = []
                angleList = []
                speedsList = []
                minSpeed = 1.0;
                valid = False
                for i in range(0,len(a_s_joints)):
                    jointName = a_s_joints[i]
                    
                    if (len(jointName) > 1):
                        jointNames = jointName.split('.')
                        if (jointNames[0] == "NAOTorsoBodySet" or jointNames[0] == "NAOLegsBodySet"):
                            jointName = jointNames[1]

                    if (jointName in self.joints):
                        if (True): #Animate joints together
                            angle = math.radians(a_d_angles[i])
                            if (angle > self.bodyLimitsDict[jointName][1] or angle < self.bodyLimitsDict[jointName][0]):
                                #log(str(angle) + " > " + str(self.bodyLimitsDict[jointName][1]) + " or < " + str(self.bodyLimitsDict[jointName][0]))
                                continue
                            #jointList.append(jointName.encode('ascii', 'ignore'))
                            jointList.append(jointName)
                            if (jointName == "RHand" or jointName == "LHand"): angleList.append(angle / 10)
                            else: angleList.append(angle)
                            if (minSpeed>a_d_speed[i]): minSpeed=a_d_speed[i]
                            speedsList.append(a_d_speed[i])
                            valid = True
                        else:
                            angle = math.radians(a_d_angles[i])
                            if (angle > self.bodyLimitsDict[jointName][1] or angle < self.bodyLimitsDict[jointName][0]):
                                continue
                            self.proxy.setAngles(jointName, angle, a_d_speed[i])
                            valid = False

                if (valid):
                    #self.proxy.angleInterpolationWithSpeed(jointList, angleList, minSpeed)
                    #Timer(0.3, self.proxy.setAngles, (jointList, angleList, minSpeed)).start()
                    self.proxy.setAngles(jointList, angleList, minSpeed)
                    self.heartbeatTick = ticks
                    self.lastTickTime = time.time()
                self.frameCount = self.frameCount + 1
            except Exception, Argument:
                log("[ERROR] PlayAnimationBuffer: " + str(Argument))
            finally:
                mutex.release()
        else:
            log("Skipped Animate mutex")
        return

    def dispose(self):
        self.fpsTimer.cancel()
        global myBroker
        myBroker.shutdown()
        return