from datetime import datetime

import naoqi
from naoqi import ALBroker
from naoqi import ALModule
from naoqi import ALProxy

instance = None

SoundLocator = None

def log(text):
    instance.nao.log("SoundLocationModule: " + text)
    return;

class SoundLocationModule(object):
    nao = None
    proxy = None
    lastDetectionTime = datetime.now()

    def __init__(self, naoServer):
        global instance
        instance = self
        self.nao = naoServer
        return

    def start(self):
        log("Connecting AudioSourceLocalizationProxy...")
        try:
            global myBroker
            myBroker = ALBroker("soundLocalizationBroker", "0.0.0.0", 0, self.nao.brokerIp(), self.nao.brokerPort())

            self.proxy = ALProxy("ALAudioSourceLocalization")
            log("Version " + self.proxy.version())

            self.nao.memory.proxy.subscribeToEvent("ALAudioSourceLocalization/SoundLocated","SoundLocator","onSoundLocated")
        except Exception, Argument:
            log("Failed: " + str(Argument))
            self.proxy = None
            return False
        return True

    def dispose(self):
        if (self.proxy==None): return False
        log("Disconnect AudioSourceLocalizationProxy...")
        try:
            if (self.nao.memory.proxy!=None):
                self.nao.memory.proxy.unsubscribeToEvent("ALAudioSourceLocalization/SoundLocated","SoundLocator")
        except Exception, Argument:
            log("Failed: " + str(Argument))
        return

        #try:
        #    global myBroker
        #    myBroker.shutdown()
        #except Exception, Argument:
        #    log("Failed: " + str(Argument))
        return

class SoundLocatorModule(ALModule):
    def onSoundLocated(self, *_args):
        """ does this and that """
        global instance
        #print ("sound located")
        try:
            if (instance != None and instance.nao != None and not instance.nao.speech.isSpeaking):
                currentTime = datetime.now()
                deltaTime = currentTime - instance.lastDetectionTime
                if (deltaTime.seconds > 1):
                    instance.lastDetectionTime = currentTime
                    s = ""
                    for field in _args[1][1]:
                        s = s + " " + str(field)
                    if (abs(_args[1][1][0])>0):
                        if (instance != None and instance.nao != None):
                            instance.nao.NotifySoundLocated(_args[1][1][0], _args[1][1][1], _args[1][1][2])
                else:
                    #print("SoundLocator: skipped " + str(deltaTime.seconds))
                    pass
        except Exception, Argument:
            log("SoundLocator Failed: " + str(Argument))
