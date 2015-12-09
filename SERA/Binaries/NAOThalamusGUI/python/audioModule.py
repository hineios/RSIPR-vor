import naoqi
from naoqi import ALBroker
from naoqi import ALModule
from naoqi import ALProxy
import os

from threading import Thread
import thread
from threading import Lock
import time

instance = None

def log(text):
    instance.nao.log("AudioModule: " + text)
    return;

def isWavFile(f):
        return f.find(".wav") != -1

class AudioModule(object):
    nao = None
    proxy = None
    masterDeviceProxy = None

    bShutdown = False
    _targetVolume = 0

    _preloadedAudioFiles = dict()
    _playingSound = None

    _volumeIncrement = 5

    def __init__(self, naoServer):
        global instance
        instance = self
        self.nao = naoServer
        return

    def start(self):

        log("Connecting AudioDeviceProxy...")
        try:
            self.masterDeviceProxy = ALProxy("ALAudioDevice")
            log(self.masterDeviceProxy.version())
            self._targetVolume = self.masterDeviceProxy.getOutputVolume()
            log("Current Master Volume: {0}".format(self._targetVolume))
        except Exception, Argument:
            log("Failed: " + str(Argument))
            self.masterDeviceProxy = None
            return False

        log("Connecting AudioPlayerProxy...")
        try:
            self.proxy = ALProxy("ALAudioPlayer")
            log(self.proxy.version())
        except Exception, Argument:
            log("Failed: " + str(Argument))
            self.proxy = None
            return False
        try:
            for file in filter(isWavFile,os.listdir("/home/nao/audio")):
                try:
                    self._preloadedAudioFiles[file]=self.proxy.loadFile("/home/nao/audio/" + file)
                    log("Preloaded audio file '" + str(file) + "'.")
                except Exception, Argument:
                    log("Failed to preload audio file: " + str(Argument))
        except Exception, Argument:
            log("Failed to get audio directory: /home/nao/audio")

        volumeth = Thread(target=self.volumeCtrlCycle)
        volumeth.start()
        print "DEBUG>> Started volume control cycle"

        return True

    def ExecuteAction(self, p, action):
        log("execute action " + str(action) + ", " + str(p))
        return

    def dispose(self):
        self.bShutdown = True
        return

    def volumeCtrlCycle(self):
        log("Running Volume Control cycle")
        while(not self.bShutdown):
            try:
                currentVolume = self.masterDeviceProxy.getOutputVolume()
                if (currentVolume==100): self.masterDeviceProxy.setOutputVolume(95)
                #log("Current: {0}, Target: {1}, abs: {2}".format(currentVolume, self._targetVolume, abs(currentVolume-self._targetVolume)))
                if (abs(currentVolume-self._targetVolume)>=self._volumeIncrement):
                    if (currentVolume < self._targetVolume):
                        #log("Increase: {0} < {1}".format(currentVolume, self._targetVolume))
                        self.masterDeviceProxy.setOutputVolume(currentVolume+self._volumeIncrement)
                    elif (currentVolume > self._targetVolume):
                        #log("Decrease: {0} > {1} -> {2}".format(currentVolume, self._targetVolume, currentVolume-self._volumeIncrement))
                        self.masterDeviceProxy.setOutputVolume(currentVolume-self._volumeIncrement)
                #else:
                    #log("Hold volume")

            except Exception, Argument:
                if (not Argument == None): log("Volume Control Cycle Exception: " + str(Argument))
                else: log("Volume Control Cycle Exception.")
            time.sleep(0.3)
        log("Exiting Volume Control cycle")
        return

    def SetVolume(self, d_value):
        #log("setVolume value:{0}".format(d_value))
        d_value = min(1, max(0, d_value))
        self._targetVolume = int(d_value*100)
        #self.masterDeviceProxy.setOutputVolume(int(d_value*100))
        return ""

    def PlaySound(self, s_id, s_soundName, s_volume, d_pitch):
        log("playSound id:{0}, soundName:{1}, volume:{2}, pitch:{3}".format(s_id, s_soundName, s_volume, s_pitch))
        if (self.proxy==None): return False
        try:
            self._playingSound = s_id
            self.nao.NotifySoundStart(s_id)
            if (soundName in self._preloadedAudioFiles):
                id = self.proxy.play(self._preloadedAudioFiles[soundName])
            else:
                id = self.proxy.playFile("/var/persistent/home/nao/audio/"+str(soundName))
            self._playingSound = None
            self.nao.NotifySoundEnd(s_id)
        except Exception, Argument:
            log("Failed: " + str(Argument))
            return False
        return True
    
    def PlaySoundLoop(self, s_id, s_soundName, s_volume, s_pitch):
        log("playSoundLoop id:{0}, soundName:{1}, volume:{2}, pitch:{3}".format(s_id, s_soundName, s_volume, s_pitch))
        if (self.proxy==None): return False
        try:
            self._playingSound = s_id
            self.nao.NotifySoundStart(s_id)
            if (soundName in self._preloadedAudioFiles):
                id = self.proxy.playInLoop(self._preloadedAudioFiles[soundName])
            else:
                id = self.proxy.playFileInLoop("/var/persistent/home/nao/audio/"+str(soundName))
            self._playingSound = None
            self.nao.NotifySoundEnd(s_id)
        except Exception, Argument:
            log("Failed: " + str(Argument))
            return False
        return True
    
    def StopSound(self, s_id):
        log("stopSound")
        if (self.proxy==None): return False
        try:
            #if (self._playingSound!=None):
                #self.nao.self.nao.NotifySoundEnd(self._playingSound)
            self.proxy.stopAll()
        except Exception, Argument:
            log("Failed: " + str(Argument))
            return False
        return True
