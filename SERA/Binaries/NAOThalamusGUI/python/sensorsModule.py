import naoqi
from naoqi import ALBroker
from naoqi import ALModule
from naoqi import ALProxy
import time
import threading
from threading import Thread

import memoryModule

instance = None

SensorsWatcher = None

BROKER_IP = "localhost"
BROKER_PORT = 9559

def log(text):
    instance.nao.log("SensorsModule: " + str(text))
    return;

class SensorsModule(object):
    nao = None
    proxy = None

    def __init__(self, naoServer):
        global instance
        instance = self
        self.nao = naoServer
        return

    def start(self):
        log("Connecting SensorsProxy...")
        try:
            #global myBroker
            #myBroker = ALBroker("sensorsBroker", "0.0.0.0", 0, self.nao.brokerIp(), self.nao.brokerPort())

            self.proxy = ALProxy("ALSensors")
            log(self.proxy.version())


            self.nao.memory.proxy.subscribeToEvent("ChestButtonPressed","SensorsWatcher","onSensorTouched")
            self.nao.memory.proxy.subscribeToEvent("FrontTactilTouched","SensorsWatcher","onSensorTouched")
            self.nao.memory.proxy.subscribeToEvent("HandLeftBackTouched","SensorsWatcher","onSensorTouched")
            self.nao.memory.proxy.subscribeToEvent("HandLeftLeftTouched","SensorsWatcher","onSensorTouched")
            self.nao.memory.proxy.subscribeToEvent("HandLeftRightTouched","SensorsWatcher","onSensorTouched")
            self.nao.memory.proxy.subscribeToEvent("HandRightBackTouched","SensorsWatcher","onSensorTouched")
            self.nao.memory.proxy.subscribeToEvent("HandRightLeftTouched","SensorsWatcher","onSensorTouched")
            self.nao.memory.proxy.subscribeToEvent("HandRightRightTouched","SensorsWatcher","onSensorTouched")
            self.nao.memory.proxy.subscribeToEvent("HotJoinDetected","SensorsWatcher","onSensorTouched")
            self.nao.memory.proxy.subscribeToEvent("LeftBumperPressed","SensorsWatcher","onSensorTouched")
            self.nao.memory.proxy.subscribeToEvent("MiddleTactilTouched","SensorsWatcher","onSensorTouched")
            self.nao.memory.proxy.subscribeToEvent("RearTactilTouched","SensorsWatcher","onSensorTouched")
            self.nao.memory.proxy.subscribeToEvent("RightBumperPressed","SensorsWatcher","onSensorTouched")
            self.lastTouch = time.time()
            
            global instance
            instance.stopTouch = False
        except Exception, Argument:
            log("Failed: " + str(Argument))
            self.proxy = None
            return False
        return True

    def dispose(self):
        if (self.proxy==None): return False
        log("Disconnect SensorsProxy...")
        try:
            if (self.proxy!=None):
                self.nao.memory.proxy.unsubscribeToEvent("ChestButtonPressed","SensorsWatcher")
                self.nao.memory.proxy.unsubscribeToEvent("FrontTactilTouched","SensorsWatcher")
                self.nao.memory.proxy.unsubscribeToEvent("HandLeftBackTouched","SensorsWatcher")
                self.nao.memory.proxy.unsubscribeToEvent("HandLeftLeftTouched","SensorsWatcher")
                self.nao.memory.proxy.unsubscribeToEvent("HandLeftRightTouched","SensorsWatcher")
                self.nao.memory.proxy.unsubscribeToEvent("HandRightBackTouched","SensorsWatcher")
                self.nao.memory.proxy.unsubscribeToEvent("HandRightLeftTouched","SensorsWatcher")
                self.nao.memory.proxy.unsubscribeToEvent("HandRightRightTouched","SensorsWatcher")
                self.nao.memory.proxy.unsubscribeToEvent("HotJoinDetected","SensorsWatcher")
                self.nao.memory.proxy.unsubscribeToEvent("LeftBumperPressed","SensorsWatcher")
                self.nao.memory.proxy.unsubscribeToEvent("MiddleTactilTouched","SensorsWatcher")
                self.nao.memory.proxy.unsubscribeToEvent("RearTactilTouched","SensorsWatcher")
                self.nao.memory.proxy.unsubscribeToEvent("RightBumperPressed","SensorsWatcher")
        except Exception, Argument:
            log("Failed: " + str(Argument))
        
        #try:
        #    global myBroker
        #    myBroker.shutdown()
        #except Exception, Argument:
        #    log("Failed: " + str(Argument))
        return


class SensorsWatcherModule(ALModule):

    def onSensorTouched(self, *_args):
        """ does this and that """
        global instance
        if (instance != None and instance.nao != None):
            try:
                if ((time.time()-instance.lastTouch) < 0.05):
                    instance.stopTouch = True
                    if (not self.notifyThread==None):
                        self.notifyThread._Thread__stop()
                    instance.lastTouch = time.time()
                else:
                    if (_args[1]==1): 
                        instance.stopTouch = False
                        instance.lastTouch = time.time()
                        self.notifyThread = SensorNotifier(_args[0], True)
                    else: 
                        instance.nao.NotifySensorTouched(_args[0], False)
                        self.notifyThread = None
                        instance.lastTouch = time.time()
            except Exception, Argument:
                log("SensorWatch Failed: " + str(Argument))

                

class SensorNotifier(threading.Thread):
    def __init__(self, sensor, value):
        threading.Thread.__init__(self)
        time.sleep(0.05)
        global instance
        if (not instance.stopTouch):
            instance.nao.NotifySensorTouched(sensor, value)

