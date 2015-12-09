using UnityEngine;
using System.Collections;
using CookComputing.XmlRpc;
using LPSClientMessages;
using System.Net;
using System.Threading;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Globalization;


/*
 * This class shares a lot of similarities with the one that is already explained in the ThalamusUnityClient solution.
 * The main difference here is that there is no implementation of a ThalamusClient.
 * However, all the code related to the XmlRpc client and server are nearly the same.
 * Incomming messages from thalamus will be routed to the corresponding methods in the ThalamusUnity class.
 * */


/*
* This interface will be used by XmlRpc client proxy to send messages to the Thalamus client.
* It is the same as ITUCEvents (which will be the ones received in the other end), but extends it by adding the IXmlRpcProxy interface
* */
public interface ILPSRPC : ILPSEventsRPC, ILibraryActionsRPC, IXmlRpcProxy { }


/*
 * This is a normal Unity MonoBehaviour class, but it extends the ITUCActions because it will be receiving the messages defined in it
 * */
public class LPS : MonoBehaviour, ILPSActionsRPC, ILibraryEventsRPC
{

    #region fields and properties

    public GameObject lstWorksContent;
    public GameObject lstAuthorsContent;
    public GameObject buttonPrefab;


    [SerializeField]
    static Button btnPerform = null;
    [SerializeField]
    static Button btnStop = null;

    LibraryInfo library = null;

    List<string> ignoreCategories = new List<string>() { "interaction" };
    Dictionary<string, Dictionary<string, List<int>>> authorsWorks = new Dictionary<string, Dictionary<string, List<int>>>();

    List<GameObject> authorButtons = new List<GameObject>();
    List<GameObject> worksButtons = new List<GameObject>();

    private static string selectedAuthor = "";
    private static string selectedWork = "";

    private static List<Action> externalActions = new List<Action>();
    private bool loadingLibraries = true;
    private Queue<string> loadLibrariesLeft = new Queue<string>();

    private string currentLibrary = "";
    static private bool performing = false;
    private Dictionary<string, string> authorLibrary = new Dictionary<string, string>();


    private string hexTextDisabled = "#10101055";
    private string hexTextEnabled = "#101010FF";
    private string hexButtonUnselected = "#B75B5B20";
    private string hexButtonSelected = "#AEBFFF42";

    /*
     * Thus class just encapsulates the messages from ITUCEvents so that you have a clean way of publishing them.
     * This way, from any other script in Unity that can access this component, you can just call the "thalamusUnity_instance.Publisher.METHOD(); to get messages published
     * */
    public LPSPublisher Publisher;

    #endregion

    public enum SpeechLanguages
    {
        English,
        Portuguese
    }

    public class LPSPublisher : ILPSEventsRPC, ILibraryActionsRPC
    {
        LPS lps;
        public LPSPublisher(LPS lps)
        {
            this.lps = lps;
        }

        public void ChangeLibrary(string newLibrary)
        {
            lps.ThalamusProxy.ChangeLibrary(newLibrary);
        }

        public void GetLibraries()
        {
            lps.ThalamusProxy.GetLibraries();
        }

        public void GetUtterances(string category, string subcategory)
        {
            lps.ThalamusProxy.GetUtterances(category, subcategory);
        }

        public void Perform(string category, string subcategory)
        {
            lps.ThalamusProxy.Perform(category, subcategory);
        }

        public void Stop()
        {
            lps.ThalamusProxy.Stop();
        }

        public void SetLanguage(string language)
        {
            lps.ThalamusProxy.SetLanguage(language);
        }
    }

    #region start/update
    // Use this for initialization
	void Start () {

        ReloadLibraries();

        btnPerform = GameObject.Find("btnPerform").GetComponent<Button>();
        btnStop = GameObject.Find("btnStop").GetComponent<Button>();
        Debug.Log("Start");
        Debug.Log(btnPerform);
        Debug.Log(btnStop);

        btnPerform.onClick.AddListener(
            () =>
            {
                if (!performing)
                {
                    Debug.Log("Perform " + selectedAuthor + ":" + selectedWork);
                    if (selectedAuthor != "" && selectedWork != "")
                    {
                        DisableButton(btnPerform);
                        performing = true;
                        PerformWork(selectedAuthor, selectedWork);
                        EnableButton(btnStop);
                    }
                }
            });

        btnStop.onClick.AddListener(
            () =>
            {
                if (performing)
                {
                    DisableButton(btnStop);
                    EnableButton(btnPerform);
                    performing = false;
                    Publisher.Stop();
                }
            });

        DisableButton(btnStop);
        DisableButton(btnPerform);
	}
	
	// Update is called once per frame
	void Update () {
        if (externalActions.Count > 0)
        {
            List<Action> actions;
            lock (externalActions)
            {
                actions = new List<Action>(externalActions);
                externalActions.Clear();
            }
            foreach (Action a in actions) a();
        }
	}

    #endregion

    #region aux

    private void EnableButton(Button btn)
    {
        if (btn == null) return;
        btn.GetComponentInChildren<Text>().color = HexToColor(hexTextEnabled);
        btn.interactable = true;
    }

    private void DisableButton(Button btn)
    {
        btn.GetComponentInChildren<Text>().color = HexToColor(hexTextDisabled);
        btn.interactable = false;
    }

    public void ExternalAction(Action a)
    {
        lock (externalActions)
        {
            externalActions.Add(a);
        }
    }

    private string LanguageForLibrary(string library)
    {
        SpeechLanguages lang;
        string shortlanguage = "en";
        if (library.Contains("_")) shortlanguage = library.Split(new char[] { '_' }, 2)[1];
        else shortlanguage = library;
        switch (shortlanguage)
        {
            case "pt": lang = SpeechLanguages.Portuguese; break;
            default:
                lang = SpeechLanguages.English; break;
        }
        return lang.ToString();
    }

    public static Color HexToColor(string aStr)
    {
        Color clr = new Color(0, 0, 0);
        if (aStr != null && aStr.Length > 0)
        {
            try
            {
                string str = aStr.Substring(1, aStr.Length - 1);
                clr.r = (float)System.Int32.Parse(str.Substring(0, 2),
                    NumberStyles.AllowHexSpecifier) / 255.0f;
                clr.g = (float)System.Int32.Parse(str.Substring(2, 2),
                    NumberStyles.AllowHexSpecifier) / 255.0f;
                clr.b = (float)System.Int32.Parse(str.Substring(4, 2),
                    NumberStyles.AllowHexSpecifier) / 255.0f;
                if (str.Length == 8) clr.a = System.Int32.Parse(str.Substring(6, 2),
                       NumberStyles.AllowHexSpecifier) / 255.0f;
                else clr.a = 1.0f;
            }
            catch (Exception e)
            {
                Debug.Log("Could not convert " + aStr + " to Color. " + e);
                return new Color(0, 0, 0, 0);
            }
        }
        return clr;
    }

    #endregion

    #region Perform

    public void PerformWork(string author, string work)
    {
        Publisher.Perform(author + "_" + work.Replace(' ', '_'), "1");
    }

    public void FinishedPerforming(string category, string subcategory)
    {
        if (!performing) return;
        string[] split = category.Split(new char[] { '_' }, 2);
        string author = split[0];
        string work = split[1];
        if (authorsWorks.ContainsKey(author) && authorsWorks[author].ContainsKey(work))
        {
            int num = -1;
            if (int.TryParse(subcategory, out num))
            {
                if (authorsWorks[author][work].Contains(num + 1))
                {
                    Debug.Log("Verse " + num + " of " + authorsWorks[author][work].Count);
                    Publisher.Perform(category, (num + 1).ToString());
                }
                else
                {
                    FinishedWork();
                }
            }
        }
    }

    public void FinishedWork()
    {
        Debug.Log("Finished.");
        performing = false;
        DisableButton(btnStop);
        EnableButton(btnPerform);
    }

    #endregion

    #region UI

    public void SelectAuthor(string author, GameObject button)
    {
        foreach (GameObject go in authorButtons)
        {
            go.GetComponent<Image>().color = HexToColor(hexButtonUnselected);
        }
        button.GetComponent<Image>().color = HexToColor(hexButtonSelected);
        selectedAuthor = author;
        if (authorLibrary.ContainsKey(author) && currentLibrary != authorLibrary[author])
        {
            currentLibrary = authorLibrary[author];
            Publisher.SetLanguage(LanguageForLibrary(authorLibrary[author]));
            Publisher.ChangeLibrary(authorLibrary[author]);
        }
        ShowWorks(author);
        DisableButton(btnPerform);
    }

    public void SelectWork(string work, GameObject button)
    {
        foreach (GameObject go in worksButtons)
        {
            go.GetComponent<Image>().color = HexToColor(hexButtonUnselected);
        }
        button.GetComponent<Image>().color = HexToColor(hexButtonSelected);
        selectedWork = work;
        EnableButton(btnPerform);
    }

    private void ClearWorks()
    {
        var children = new List<GameObject>();
        foreach (Transform child in lstWorksContent.transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
        worksButtons.Clear();
    }

    private void ClearAuthors()
    {
        var children = new List<GameObject>();
        foreach (Transform child in lstAuthorsContent.transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
        authorButtons.Clear();
    }

    private void AddWork(string work)
    {
        GameObject myButton = Instantiate(buttonPrefab) as GameObject;
        myButton.GetComponentInChildren<Text>().text = work.Replace('_', ' ');
        myButton.transform.parent = lstWorksContent.transform;
        myButton.transform.localScale = Vector3.one;
        string w = work;
        myButton.GetComponent<Button>().onClick.AddListener(
            delegate
            {
                SelectWork(work, myButton);
            });
        worksButtons.Add(myButton);
    }

    private void AddAuthor(string author)
    {
        GameObject myButton = Instantiate(buttonPrefab) as GameObject;
        string language = "";
        if (authorLibrary.ContainsKey(author))
        {
            if (authorLibrary[author].Contains("_")) language = " (" + authorLibrary[author].Split(new char[] { '_' }, 2)[1] + ")";
            else language = " (" + authorLibrary[author] + ")";
        }
        myButton.GetComponentInChildren<Text>().text = author + language;
        //myButton.transform.SetParent(lstAuthorsContent.transform);
        myButton.transform.parent = lstAuthorsContent.transform;
        myButton.transform.localScale = Vector3.one;
        string a = author;
        myButton.GetComponent<Button>().onClick.AddListener(
            delegate
            {
                SelectAuthor(a, myButton);
            });
        authorButtons.Add(myButton);
    }

    private void ShowWorks(string author)
    {
        selectedWork = "";
        if (lstWorksContent != null)
        {
            ClearWorks();
            foreach (KeyValuePair<string, List<int>> work in authorsWorks[author])
            {
                AddWork(work.Key);
            }
        }
    }

    private void ShowAuthors()
    {
        selectedAuthor = "";
        selectedWork = "";
        if (lstAuthorsContent != null)
        {
            ClearAuthors();
            foreach (string author in authorsWorks.Keys)
            {
                AddAuthor(author);
            }
            ClearWorks();
            AddWork("please select an author...");
        }
    }

    #endregion

    #region library mgmt

    public void ReloadLibraries()
    {
        loadingLibraries = true;
        authorsWorks = new Dictionary<string, Dictionary<string, List<int>>>();
        ClearWorks();
        ClearAuthors();
        AddWork("loading, please wait...");
        AddAuthor("loading, please wait...");
        Publisher.GetLibraries();
    }    

    public void LibraryList(string[] libraries)
    {
        Debug.Log("Got from Thalamus: LibraryList");
        foreach (string lib in libraries)
        {
            if (lib.StartsWith("LPS"))
            {
                loadLibrariesLeft.Enqueue(lib);
            }
        }
        LoadNextLibrary();
    }

    private void LoadNextLibrary()
    {
        if (loadLibrariesLeft.Count > 0)
        {
            Publisher.ChangeLibrary(loadLibrariesLeft.Dequeue());
        }
        else
        {
            loadingLibraries = false;
            ShowAuthors();
        }
    }

    public void LibraryChanged(string serialized_LibraryContents)
    {
        Debug.Log("Got from Thalamus: LibraryChanged");
        library = LibraryInfo.DeserializeFromJson(serialized_LibraryContents);
        currentLibrary = library.LibraryName;
        if (loadingLibraries)
        {
            if (library.LibraryName.StartsWith("LPS"))
            {
                foreach (string c in library.Categories.Keys)
                {
                    if (ignoreCategories.Contains(c)) continue;
                    string[] split = c.Split(new char[] { '_' }, 2);
                    string author = split[0];
                    string work = split[1];
                    if (!authorsWorks.ContainsKey(author)) authorsWorks[author] = new Dictionary<string, List<int>>();
                    authorLibrary[author] = library.LibraryName;
                    if (!authorsWorks[author].ContainsKey(work)) authorsWorks[author][work] = new List<int>();
                    foreach (string s in library.Categories[c])
                    {
                        int num = -1;
                        if (int.TryParse(s, out num)) authorsWorks[author][work].Add(num);
                    }
                }
            }
            LoadNextLibrary();
        }
    }


    public void Utterances(string library, string category, string subcategory, string[] utterances)
    {

    }

    #endregion

    #region constructor/dispose

    /*
     * We use the constructor to initialize all the XmlRpc stuff. It's fine, and that way you keep the Awake and Start methods clean for your Unity code.
     * */
    public LPS()
    {
        Publisher = new LPSPublisher(this);

        remoteUri = String.Format("http://{0}:{1}", remoteAddress, remotePort);
        thalamusProxy = XmlRpcProxyGen.Create<ILPSRPC>();
        thalamusProxy.Timeout = 1000;
        thalamusProxy.Url = remoteUri;
        thalamusProxy.Url = remoteUri;
        Debug.Log("Thalamus endpoint set to " + remoteUri);

        dispatcherThread = new Thread(new ThreadStart(DispatcherThreadThalamus));
        messageDispatcherThread = new Thread(new ThreadStart(MessageDispatcherThalamus));
        dispatcherThread.Start();
        messageDispatcherThread.Start();
    }

    public void Dispose()
    {
        shutdown = true;

        try
        {
            if (listener != null) listener.Stop();
        }
        catch { }

        try
        {
            if (dispatcherThread != null) dispatcherThread.Join();
        }
        catch { }

        try
        {
            if (messageDispatcherThread != null) messageDispatcherThread.Join();
        }
        catch { }
    }

    #endregion

    #region XmlRpc code

    // This helps on debugging. Switch it off if you'r getting your Unity debig too cluttered.
    private bool printExceptions = true;

    //make sure these correspond (in opposite) to the ports used in the ThalamusUnityClient
    private int localPort = 7001;
    private string remoteAddress = "localhost";
    private int remotePort = 7000;
        
    private HttpListener listener;
    private bool serviceRunning;
    private bool shutdown;
    List<HttpListenerContext> httpRequestsQueue = new List<HttpListenerContext>();
    private Thread dispatcherThread;
    private Thread messageDispatcherThread;
	private string remoteUri = "";
    //XmLRpc client through which we send messages back to the Thalamus client
    ILPSRPC thalamusProxy;
    public ILPSRPC ThalamusProxy
    {
        get { return thalamusProxy; }
    }


    public void ProcessMessageThalamus(object oContext)
    {
        try
        {
            XmlRpcListenerService svc = new LPSService(this);
            svc.ProcessRequest((HttpListenerContext)oContext);
        }
        catch (Exception e)
        {
            if (printExceptions) Debug.Log("Exception in ProcessMessageThalamus: " + e.ToString());
        }
    }

    public void DispatcherThreadThalamus()
    {
        while (!serviceRunning)
        {
            try
            {
                Debug.Log("Attempt to start service on port '" + localPort + "'");
                listener = new HttpListener();
                listener.Prefixes.Add(string.Format("http://*:{0}/", localPort));
                listener.Start();
                Debug.Log("XMLRPC Listening on " + string.Format("http://*:{0}/", localPort));
                serviceRunning = true;
            }
            catch
            {
                localPort++;
                Debug.Log("Port unavaliable.");
                serviceRunning = false;
            }
        }

        while (!shutdown)
        {
            try
            {
                HttpListenerContext context = listener.GetContext();
                lock (httpRequestsQueue)
                {
                    httpRequestsQueue.Add(context);
                }
            }
            catch (Exception e)
            {
                if (printExceptions) Debug.Log("Exception in DispatcherThreadThalamus: " + e.ToString());
                serviceRunning = false;
                if (listener != null)
                    listener.Close();
            }
        }
        Debug.Log("Terminated DispatcherThreadThalamus");
        listener.Close();
    }

    public void MessageDispatcherThalamus()
    {
        while (!shutdown)
        {
            bool performSleep = true;
            try
            {
                if (httpRequestsQueue.Count > 0)
                {
                    performSleep = false;
                    List<HttpListenerContext> httpRequests;
                    lock (httpRequestsQueue)
                    {
                        httpRequests = new List<HttpListenerContext>(httpRequestsQueue);
                        httpRequestsQueue.Clear();
                    }
                    foreach (HttpListenerContext r in httpRequests)
                    {
                        //ProcessRequest(r);
                        (new Thread(new ParameterizedThreadStart(ProcessMessageThalamus))).Start(r);
                        performSleep = false;
                    }
                }


            }
            catch (Exception e)
            {
                if (printExceptions) Debug.Log("Exception in MessageDispatcherThalamus: " + e.ToString());
            }
            if (performSleep) Thread.Sleep(10);
        }
        Debug.Log("Terminated MessageDispatcherThalamus");
    }
    #endregion

    
}


