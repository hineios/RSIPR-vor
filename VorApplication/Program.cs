using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace VorApplication
{
    public class GameConnection
    {
        public string ip { get; set; }
        public int port { get; set; }
        public List<Word> words { get; set; }

        public GameConnection(string ip, int port, List<Word> words)
        {
            this.ip = ip;
            this.port = port;
            this.words = words;
        }

        static public GameConnection Deserialize(string str)
        {
            return JsonConvert.DeserializeObject<GameConnection>(str);
        }
        static public string Serialize(GameConnection gc)
        {
            return JsonConvert.SerializeObject(gc);
        }
    }
    public class Word
    {
        public string id { get; set; }
        public string value { get; set; }

        public Word(string id, string value)
        {
            this.id = id;
            this.value = value;
        }
        static public Word Deserialize(string str)
        {
            return JsonConvert.DeserializeObject<Word>(str);
        }
        static public string Serialize(Word w)
        {
            return JsonConvert.SerializeObject(w);
        }
    }
    public class PlayerNames
    {
        public string confederate { get; set; }
        public string participant { get; set; }

        public PlayerNames(string confederate, string participant)
        {
            this.confederate = confederate;
            this.participant = participant;
        }
        static public PlayerNames Deserialize(string str)
        {
            return JsonConvert.DeserializeObject<PlayerNames>(str);
        }
        static public string Serialize(PlayerNames pn)
        {
            return JsonConvert.SerializeObject(pn);
        }
    }
    class Program
    {
        static int Main(string[] args)
        {
            HttpServer httpServer;
            if (args.GetLength(0) > 1)
            {
                httpServer = new MyHttpServer(Convert.ToInt16(args[0]), args[1]);
            }
            else if (args.GetLength(0) > 0)
            {
                httpServer = new MyHttpServer(Convert.ToInt16(args[0]));
            }
            else
            {
                httpServer = new MyHttpServer();
            }
            Thread thread = new Thread(new ThreadStart(httpServer.listen));
            thread.Start();
            return 0;
        }
    }

    // offered to the public domain for any use with no restriction
    // and also with no warranty of any kind, please enjoy. - David Jeske. 

    // simple HTTP explanation
    // http://www.jmarshall.com/easy/http/
    public class HttpProcessor
    {
        public TcpClient socket;
        public HttpServer srv;

        private Stream inputStream;
        public StreamWriter outputStream;

        public String http_method;
        public String http_url;
        public String http_protocol_versionstring;
        public Hashtable httpHeaders = new Hashtable();


        private static int MAX_POST_SIZE = 10 * 1024 * 1024; // 10MB

        public HttpProcessor(TcpClient s, HttpServer srv)
        {
            this.socket = s;
            this.srv = srv;
        }
        private string streamReadLine(Stream inputStream)
        {
            int next_char;
            string data = "";
            while (true)
            {
                next_char = inputStream.ReadByte();
                if (next_char == '\n') { break; }
                if (next_char == '\r') { continue; }
                if (next_char == -1) { Thread.Sleep(1); continue; };
                data += Convert.ToChar(next_char);
            }
            return data;
        }
        public void process()
        {
            // we can't use a StreamReader for input, because it buffers up extra data on us inside it's
            // "processed" view of the world, and we want the data raw after the headers
            inputStream = new BufferedStream(socket.GetStream());

            // we probably shouldn't be using a streamwriter for all output from handlers either
            outputStream = new StreamWriter(new BufferedStream(socket.GetStream()));
            try
            {
                parseRequest();
                readHeaders();
                if (http_method.Equals("GET"))
                {
                    handleGETRequest();
                }
                else if (http_method.Equals("POST"))
                {
                    handlePOSTRequest();
                }else if (http_method.Equals("OPTIONS"))
                {
                    handlePOSTRequest();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.ToString());
                writeFailure();
            }
            outputStream.Flush();
            // bs.Flush(); // flush any remaining output
            inputStream = null; outputStream = null; // bs = null;            
            socket.Close();
        }
        public void parseRequest()
        {
            String request = streamReadLine(inputStream);
            string[] tokens = request.Split(' ');
            if (tokens.Length != 3)
            {
                throw new Exception("invalid http request line");
            }
            http_method = tokens[0].ToUpper();
            http_url = tokens[1];
            http_protocol_versionstring = tokens[2];

            //Console.WriteLine("starting: " + request);
        }
        public void readHeaders()
        {
            //Console.WriteLine("readHeaders()");
            String line;
            while ((line = streamReadLine(inputStream)) != null)
            {
                if (line.Equals(""))
                {
                    //Console.WriteLine("got headers");
                    return;
                }

                int separator = line.IndexOf(':');
                if (separator == -1)
                {
                    throw new Exception("invalid http header line: " + line);
                }
                String name = line.Substring(0, separator);
                int pos = separator + 1;
                while ((pos < line.Length) && (line[pos] == ' '))
                {
                    pos++; // strip any spaces
                }

                string value = line.Substring(pos, line.Length - pos);
                //Console.WriteLine("header: {0}:{1}", name, value);
                httpHeaders[name] = value;
            }
        }
        public void handleGETRequest()
        {
            srv.handleGETRequest(this);
        }
        private const int BUF_SIZE = 4096;
        public void handlePOSTRequest()
        {
            // this post data processing just reads everything into a memory stream.
            // this is fine for smallish things, but for large stuff we should really
            // hand an input stream to the request processor. However, the input stream 
            // we hand him needs to let him see the "end of the stream" at this content 
            // length, because otherwise he won't know when he's seen it all! 

            //Console.WriteLine("get post data start");
            int content_len = 0;
            MemoryStream ms = new MemoryStream();
            if (this.httpHeaders.ContainsKey("Content-Length"))
            {
                content_len = Convert.ToInt32(this.httpHeaders["Content-Length"]);
                if (content_len > MAX_POST_SIZE)
                {
                    throw new Exception(
                        String.Format("POST Content-Length({0}) too big for this simple server",
                            content_len));
                }
                byte[] buf = new byte[BUF_SIZE];
                int to_read = content_len;
                while (to_read > 0)
                {
                    //Console.WriteLine("starting Read, to_read={0}", to_read);

                    int numread = this.inputStream.Read(buf, 0, Math.Min(BUF_SIZE, to_read));
                    //Console.WriteLine("read finished, numread={0}", numread);
                    if (numread == 0)
                    {
                        if (to_read == 0)
                        {
                            break;
                        }
                        else
                        {
                            throw new Exception("client disconnected during post");
                        }
                    }
                    to_read -= numread;
                    ms.Write(buf, 0, numread);
                }
                ms.Seek(0, SeekOrigin.Begin);
            }
            //Console.WriteLine("get post data end");
            srv.handlePOSTRequest(this, new StreamReader(ms));

        }
        public void writeSuccess(string content_type = "text/html")
        {
            // this is the successful HTTP response line
            outputStream.WriteLine("HTTP/1.0 200 OK");
            // these are the HTTP headers...          
            outputStream.WriteLine("Content-Type: " + content_type);
            outputStream.WriteLine("Connection: close");
            // ..add your own headers here if you like
            outputStream.WriteLine("Access-Control-Allow-Origin: *");
            outputStream.WriteLine("Access-Control-Allow-Methods: POST,GET,OPTIONS");
            outputStream.WriteLine("Access-Control-Allow-Headers: Origin, X-Requested-With, Content-Type, Accept");

            outputStream.WriteLine(""); // this terminates the HTTP headers.. everything after this is HTTP body..
        }
        public void writeFailure()
        {
            // this is an http 404 failure response
            outputStream.WriteLine("HTTP/1.0 404 File not found");
            // these are the HTTP headers
            outputStream.WriteLine("Connection: close");
            // ..add your own headers here

            outputStream.WriteLine(""); // this terminates the HTTP headers.
        }
    }
    public abstract class HttpServer
    {
        protected int port;
        TcpListener listener;
        bool is_active = true;
        protected VorApplicationClient tclient;

        public HttpServer(int port)
        {
            this.port = port;
        }
        public void listen()
        {
            //listener = new TcpListener(port);
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            while (is_active)
            {
                TcpClient s = listener.AcceptTcpClient();
                HttpProcessor processor = new HttpProcessor(s, this);
                Thread thread = new Thread(new ThreadStart(processor.process));
                thread.Start();
                Thread.Sleep(1);
            }
            tclient.Dispose();
        }
        public abstract void handleGETRequest(HttpProcessor p);
        public abstract void handlePOSTRequest(HttpProcessor p, StreamReader inputData);
    }
    public class MyHttpServer : HttpServer
    {
        public MyHttpServer(int port = 8080, string master = "VorThalamusMaster") : base(port)
        {
            tclient = new VorApplicationClient(master);
        }

        public override void handleGETRequest(HttpProcessor p)
        {
            Console.WriteLine("GET request: {0}", p.http_url);
            p.writeFailure();
            p.outputStream.BaseStream.Flush();
            //p.outputStream.WriteLine("Mete-te na tua vida");
        }
        public override void handlePOSTRequest(HttpProcessor p, StreamReader inputData)
        {
            if (p.http_method.Equals("OPTIONS"))
            {
                p.writeSuccess();
            }
            else
            {
                Console.WriteLine("POST request: {0}", p.http_url);
                string data = inputData.ReadToEnd();
                //Console.WriteLine(data);
                Word w;
                GameConnection g;
                PlayerNames pn;
                switch (p.http_url.ToLower())
                {
                    //GameConnection
                    case "/gameconnection":
                        try
                        {
                            g = JsonConvert.DeserializeObject<GameConnection>(data);
                            p.writeSuccess();
                            Console.WriteLine("GameConnection: ip:{0} ; port:{1}; words:{2}", g.ip, g.port, g.words.Count);
                            p.outputStream.WriteLine("<html><body><h1>OK</h1>");
                            p.outputStream.BaseStream.Flush();
                            tclient.PassServer(g.ip, g.port);
                            //Pass information to WOZ
                            tclient.vorPublisher.ApplicationLoaded(GameConnection.Serialize(g));
                        }
                        catch (Exception e)
                        {
                            p.writeFailure();
                            Console.WriteLine("ERRO JSON: {0}", e.ToString());
                        }
                        break;
                    //PlayerNames
                    case "/playernames":
                        try {
                            pn = JsonConvert.DeserializeObject<PlayerNames>(data);
                            Console.WriteLine("Player Names: confederate:{0} ; participant:{1}", pn.confederate, pn.participant);
                            p.writeSuccess();
                            p.outputStream.WriteLine("<html><body><h1>OK</h1>");
                            p.outputStream.BaseStream.Flush();
                            //Pass information to WOZ
                            tclient.vorPublisher.ApplicationReady(pn.confederate, pn.participant);
                        }
                        catch (Exception e)
                        {
                            p.writeFailure();
                            Console.WriteLine("ERRO JSON: {0}", e.ToString());
                        }
                        break;
                    //WordAccepted
                    case "/wordaccepted":
                        try
                        {
                            w = JsonConvert.DeserializeObject<Word>(data);
                            p.writeSuccess();
                            Console.WriteLine("Word Accepted: id:{0} ; value:{1}", w.id, w.value);
                            p.outputStream.WriteLine("<html><body><h1>OK</h1>");
                            p.outputStream.BaseStream.Flush();
                            //Pass information to WOZ
                            tclient.vorPublisher.WordAccepted(w.id);
                        }
                        catch (Exception e)
                        {
                            p.writeFailure();
                            Console.WriteLine("ERRO JSON: {0}", e.ToString());
                        }   
                        break;
                    //WordDeclined
                    case "/worddeclined":
                        try
                        {
                            w = JsonConvert.DeserializeObject<Word>(data);
                            p.writeSuccess();
                            Console.WriteLine("Word Declined: id:{0} ; value:{1}", w.id, w.value);
                            p.outputStream.WriteLine("<html><body><h1>OK</h1>");
                            p.outputStream.BaseStream.Flush();
                            //Pass information to WOZ
                            tclient.vorPublisher.WordDeclined(w.id);
                        }
                        catch (Exception e)
                        {
                            p.writeFailure();
                            Console.WriteLine("ERRO JSON: {0}", e.ToString());
                        }
                        break;
                    //SelectedWord
                    case "/selectedword":
                        try
                        {
                            w = JsonConvert.DeserializeObject<Word>(data);
                            p.writeSuccess();
                            Console.WriteLine("Selected Word: id:{0} ; value:{1}", w.id, w.value);
                            p.outputStream.WriteLine("<html><body><h1>OK</h1>");
                            p.outputStream.BaseStream.Flush();
                            //Pass information to WOZ
                            tclient.vorPublisher.WordSelected(w.id);
                        }
                        catch (Exception e)
                        {
                            p.writeFailure();
                            Console.WriteLine("ERRO JSON: {0}", e.ToString());
                        }
                        break;
                    case "/tipnotselected":
                        p.writeSuccess();
                        p.outputStream.BaseStream.Flush();
                        tclient.vorPublisher.TipNotSelected();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
