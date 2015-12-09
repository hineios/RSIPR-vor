# coding=ISO8859-1

import naoqi
from naoqi import ALBroker
from naoqi import ALModule
from naoqi import ALProxy
import os
import math
from threading import Thread
import thread
from threading import Lock
import time

instance = None

def log(text):
    instance.nao.log("SpeechRecognitionModule: " + text)
    return;

class SpeechRecognitionModule(object):
    nao = None
    proxy = None

    bShutdown = False

    def __init__(self, naoServer):
        global instance
        instance = self
        self.nao = naoServer
        return

    def start(self):

        log("Connecting SpeechRecognitionProxy...")
        try:
            self.proxy = ALProxy("ALSpeechRecognition")
            log(self.proxy.version())
        except Exception, Argument:
            log("Failed: " + str(Argument))
            self.proxy = None
            return False

        self.proxy.setVisualExpression(False)
        self.proxy.setAudioExpression(False)
        self.proxy.setLanguage("Portuguese")
        self.proxy.setVocabulary(["robou", "energia", "dinheiro", "ambiente", "felicidade", "sorriso", "bem estar", "electricidade", "oleo", u"petróleo", u"não renovável", "economia", "farol", "casas", u"população", "passar", u"nível", u"avançar", "solares", "melhorias", "melhora", "melhorar", "aplicar", u"política", "construir"], True) #"mar", "rio", 

        self.nao.memory.proxy.subscribeToEvent("WordRecognized","SpeechRecoWatcher","onWordRecognized")
        #volumeth = Thread(target=self.volumeCtrlCycle)
        #volumeth.start()
        #print "DEBUG>> Started volume control cycle"

        return True

    def ExecuteAction(self, p, action):
        log("execute action " + str(action) + ", " + str(p))
        return

    def dispose(self):
        try:
            self.nao.memory.proxy.unsubscribeToEvent("WordRecognized","SpeechRecoWatcher")
            self.proxy.unsubscribe("SpeechRecoWatcher")
        except Exception, Argument:
            log("Failed: " + str(Argument))
        #self.proxy.stop()
        self.bShutdown = True
        return

class SpeechRecoWatcherModule(ALModule):

    def onWordRecognized(self, *_args):
        """ does this and that """
        global instance
        count = int(math.ceil(len(_args[1])/2))
        log(str(_args))
        log(str(count))
        acceptedWords = []
        for i in range(0, count):
            if (_args[1][i*2+1]>0.5): 
                log(str(_args[1][i*2]) + ": " + str(_args[1][i*2+1]))
                acceptedWords.append(str(_args[1][i*2]))
        if (len(acceptedWords)>0):
            instance.nao.NotifyWordDetected(acceptedWords)
        return