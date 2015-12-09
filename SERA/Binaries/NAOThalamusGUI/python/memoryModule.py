import naoqi
from naoqi import ALBroker
from naoqi import ALModule
from naoqi import ALProxy

instance = None

def log(text):
    instance.nao.log("MemoryModule: " + text)
    return;

class MemoryModule(object):
    nao = None
    proxy = None

    def __init__(self, naoServer):
        global instance
        instance = self
        self.nao = naoServer
        return

    def start(self):
        log("Connecting MemoryProxy...")
        try:
            self.proxy = ALProxy("ALMemory")
            log(self.proxy.version())
        except Exception, Argument:
            log("Failed: " + str(Argument))
            self.proxy = None
            return False
        return True

    def dispose(self):
        return