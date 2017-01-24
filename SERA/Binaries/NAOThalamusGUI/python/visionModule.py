import string

import naoqi
from naoqi import ALBroker
from naoqi import ALModule
from naoqi import ALProxy

instance = None

VisionRecognizer = None

def log(text):
    instance.nao.log("VisionModule: " + text)
    return;

class VisionModule(object):
    nao = None
    proxy = None

    def __init__(self, naoServer):
        global instance
        instance = self
        self.nao = naoServer
        return

    def start(self):
        log("Connecting VisionRecognitionProxy...")
        try:
            #global myBroker
            #myBroker = ALBroker("visionBroker", "0.0.0.0", 0, self.nao.brokerIp(), self.nao.brokerPort())

            self.proxy = ALProxy("ALVisionRecognition")
            log(self.proxy.version())

            self.nao.memory.proxy.subscribeToEvent("PictureDetected","VisionRecognizer","onPictureDetected")
        except Exception, Argument:
            log("Failed: " + str(Argument))
            self.proxy = None
            return False
        return True

    def dispose(self):
        if (self.proxy==None): return False
        log("Disconnect VisionRecognitionProxy...")
        try:
            if (self.proxy!=None):
                self.nao.memory.proxy.unsubscribeToEvent("PictureDetected","VisionRecognizer")
        except Exception, Argument:
            log("Failed: " + str(Argument))
        #try:
        #    global myBroker
        #    myBroker.shutdown()
        #except Exception, Argument:
        #    log("Failed: " + str(Argument))
        return

class VisionRecognizerModule(ALModule):
    def onPictureDetected(self, *_args):
        """ does this and that """
        global instance
        if (instance != None and instance.nao != None and (not _args[1] == [])):
            print _args
            for picture in _args[1][1]:
                print (str(picture))
                s = str(picture[0][len(picture[0])-1])
                log("PictureRecognition: " + s)
                s = string.lstrip(s)
                instance.nao.NotifyPictureDetected(s, picture[2]);
