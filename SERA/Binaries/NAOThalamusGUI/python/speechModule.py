import naoqi
from naoqi import ALBroker
from naoqi import ALModule
from naoqi import ALProxy


import codecs
import time
from threading import Lock

global instance
instance = None

SpeechWatcher = None

def log(text):
    global instance
    instance.nao.log("SpeechModule: " + text)
    return;

class SpeechModule(object):
    nao = None
    proxy = None
    stopProxy = None
    isSpeaking = False
    speechBookmarks = []
    speechMutex = Lock()

    def __init__(self, naoServer):
        global instance
        instance = self
        self.nao = naoServer
        return

    def start(self):
        log("Connecting SpeechProxy...")
        try:
            #global myBroker
            #myBroker = ALBroker("speechBroker", "0.0.0.0", 0, self.nao.brokerIp(), self.nao.brokerPort())

            self.proxy = ALProxy("ALTextToSpeech")
            self.stopProxy = ALProxy("ALTextToSpeech")
            log(self.proxy.version())
            log("Available languages: " + str(self.proxy.getAvailableLanguages()))
            log("Available voices: " + str(self.proxy.getAvailableVoices()))
            #self.proxy.setVoice("Celia22Enhanced");
            try:
                self.proxy.enableNotifications()
            except Exception, Argument:
                log("Notifications are already enabled")

            self.nao.memory.proxy.subscribeToEvent("ALTextToSpeech/CurrentBookMark","SpeechWatcher","onCurrentBookMark")
            self.nao.memory.proxy.subscribeToEvent("ALTextToSpeech/TextStarted","SpeechWatcher","onTextStarted")
            self.nao.memory.proxy.subscribeToEvent("ALTextToSpeech/TextDone","SpeechWatcher","onTextDone")
            self.nao.memory.proxy.subscribeToEvent("ALTextToSpeech/CurrentSentence","SpeechWatcher","onCurrentSentence")
        except Exception, Argument:
            log("Failed: " + str(Argument))
            self.proxy = None
            return False
        return True

    def ExecuteAction(self, p, action):
        #log("execute action " + str(action) + ", " + str(p))
        if (type(action).__name__ == 'SpeakStruct'):
            self.Speak(action.s_id, action.s_text)
        else:
            if (type(action).__name__ == 'SpeakBookmarksStruct'):
                self.SpeakBookmarks(action.s_id, action.a_s_text, action.a_s_bookmarks)
        return

    def dispose(self):
        log("Disconnect SpeechProxy...")
        try:
            self.nao.memory.proxy.unsubscribeToEvent("ALTextToSpeech/CurrentBookMark","SpeechWatcher")
        except Exception, Argument:
            log("Failed: " + str(Argument))
        #try:
        #    global myBroker
        #    myBroker.shutdown()
        #except Exception, Argument:
        #    log("Failed: " + str(Argument))
        return

    

    def SpeakStop(self):
        try: 
            log("going to stop")
            self.stopProxy.stopAll()
            log("stopped")
        except Exception, Argument:
            log("SpeakStop Failed: " + str(Argument))
            return False
        return ""

    def Speak(self, s_id, s_text):
        if (self.proxy==None): return False
        try: 
            self.speechMutex.acquire()
            #while (self.isSpeaking):
                #time.sleep(0.1)

            self.speechBookmarks = []
            self.speechId = s_id
            s_text = s_text.encode('utf-8')
            
            
            #id = self.proxy.post.say(s_text)
            self.requestedSpeaking = True
            self.SpeakStarted()
            self.proxy.say(s_text)
            self.SpeakDone()
            self.speechMutex.release()
        except Exception, Argument:
            log("Speak Failed: " + str(Argument))
            return False
        return ""

    def SpeakBookmarks(self, s_id, arr_s_text, arr_s_bookmarks):
        if (self.proxy==None): return False
        try: 
            self.speechMutex.acquire()
            self.speechBookmarks = arr_s_bookmarks
            text = ""
            i = 0
            for s in arr_s_text:
                s = s.encode('utf-8')
                text = text + s + " "
                if (len(arr_s_bookmarks)>i):
                    text = text + "\\mrk=" + str(i) + "\\"
                    i = i + 1
            self.speechId = s_id
            #id = self.proxy.post.say(text)
            self.requestedSpeaking = True
            self.SpeakStarted()
            self.proxy.say(text)
            self.SpeakDone()
            self.speechMutex.release()
        except Exception, Argument:
            log("SpeakBookmarks Failed: " + str(Argument))
            return False
        return ""

    def SpeakStarted(self):
        self.isSpeaking = True
        self.requestedSpeaking = False
        self.nao.NotifySpeakStart(self.speechId)
        return ""

    def SpeakDone(self):
        self.isSpeaking = False
        self.nao.NotifySpeakEnd(self.speechId)
        self.speechId = ""
        return ""

class SpeechWatcherModule(ALModule):

    def onCurrentBookMark(self, *_args):
        """ does this and that """
        global instance
        if (len(instance.speechBookmarks)>=_args[1]+1):
            instance.nao.NotifySpeakBookmark(instance.speechBookmarks[_args[1]])
        return

    def onTextStarted(self, *_args):
        """ does this and that """
        global instance
        #if (not instance.isSpeaking and instance.requestedSpeaking and _args[1]==1): instance.SpeakStarted()
        return

    def onCurrentSentence(self, *_args):
        """ does this and that """
        global instance
        log(str(_args))
        #if (instance.isSpeaking and len(_args[1])==0): instance.SpeakDone()
        return