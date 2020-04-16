using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class SimulatorModel : ISimulatorModel
{
    // ShouldStop for ShouldStop the thread in the staet method
    // tcpclient for the comunication with the server

    volatile Boolean ShouldStop;
    private TcpClient TCPClient;
    private NetworkStream TCPStream;
    private List<Byte[]> DataToSend = new List<Byte[]>();
    private readonly object bBalanceLock = new object();
    private bool ShouldAdd;

    //CTOR 
    public SimulatorModel(TcpClient T)
    {
        this.ShouldStop = false;
        this.ShouldAdd = true;
        this.TCPClient = T;
        this.PropLocation = new Location(0, 0);
        //Sets the error log to default value
        this.Errlog = "Status: Connected";
        // Sets the receive time out using the ReceiveTimeout public property.
        TCPClient.ReceiveTimeout = 10000;

        // Gets the receive time out using the ReceiveTimeout public property.
        if (TCPClient.ReceiveTimeout == 10000)
            Debug.WriteLine("The receive time out limit was successfully set " + TCPClient.ReceiveTimeout.ToString());
    }

    // event
    public event PropertyChangedEventHandler PropertyChanged;

    // 8 members for dashboard
    private double PropHeadingDegree;
    private double PropVerticalSpeed;
    private double PropGroundSpeed;
    private double PropAirSpeed;
    private double PropAltitudeFT;
    private double PropRollDegree;
    private double PropPitchDegree;
    private double PropAltimeterFT;

    //members for the map
    private double PropLongitude;
    private double PropLatitude;
    private Location PropLocation;
    private string PropLongAndLat;

    //member Error Log
    private string errlog;

    //here 8 property for dashboard
    // more three for pin in map
    // Errlog property for sign kinds of err to the view by binding
    public string LongtitudeLatitude
    {
        get { return PropLongAndLat; }
        set
        {
            PropLongAndLat = value;
            NotifyPropertyChanged("LongtitudeLatitude");
        }
    }

    public string Errlog
    {
        get { return errlog; }
        set
        {
            errlog = value;
            NotifyPropertyChanged("Errlog");
        }
    }

    public Location Location
    {
        get { return PropLocation; }
        set
        {
            PropLocation = value;
            NotifyPropertyChanged("Location");
        }
    }

    public double LongitudeDegree
    {
        get { return PropLongitude; }
        set
        {
            PropLongitude = value;
            NotifyPropertyChanged("LongitudeDegree");
        }
    }

    public double LatitudeDegree
    {
        get { return PropLatitude; }
        set
        {
            PropLatitude = value;
            NotifyPropertyChanged("LatitudeDegree");
        }
    }

    public double HeadingDegree
    {
        get { return PropHeadingDegree; }
        set
        {
            PropHeadingDegree = value;
            NotifyPropertyChanged("HeadingDegree");
        }
    }
    public double VerticalSpeed
    {
        get { return PropVerticalSpeed; }
        set
        {
            PropVerticalSpeed = value;
            NotifyPropertyChanged("VerticalSpeed");
        }
    }
    public double GroundSpeed
    {
        get { return PropGroundSpeed; }
        set
        {
            PropGroundSpeed = value;
            NotifyPropertyChanged("GroundSpeed");
        }
    }
    public double AirSpeed
    {
        get { return PropAirSpeed; }
        set
        {
            PropAirSpeed = value;
            NotifyPropertyChanged("AirSpeed");
        }
    }
    public double AltitudeFT
    {
        get { return PropAltitudeFT; }
        set
        {
            PropAltitudeFT = value;
            NotifyPropertyChanged("AltitudeFT");
        }
    }
    public double RollDegree
    {
        get { return PropRollDegree; }
        set
        {
            PropRollDegree = value;
            NotifyPropertyChanged("RollDegree");
        }
    }
    public double PitchDegree
    {
        get { return PropPitchDegree; }
        set
        {
            PropPitchDegree = value;
            NotifyPropertyChanged("PitchDegree");
        }
    }
    public double AltimeterFT
    {
        get { return PropAltimeterFT; }
        set
        {
            PropAltimeterFT = value;
            NotifyPropertyChanged("AltimeterFT");
        }
    }

    // commands for set the value of aileron ,add the path to the send list
    public void SetAileron(double aileron)
    {
        double value_to_send;

        //checking the range value and change it according to aileron range
        if (aileron < 1) 
        {
            if (aileron > -1)
            {
                value_to_send = aileron;
            }
            else
            {
                value_to_send = -1;
            }
        }
        else
        {
            value_to_send = 1;
        }
        string msg = "set /controls/flight/aileron " + value_to_send.ToString() + "\n";

        Byte[] bytes = System.Text.Encoding.ASCII.GetBytes(msg);
        lock (bBalanceLock)
        {
            if (this.ShouldAdd)
            {
                this.DataToSend.Add(bytes);
            }
        }
    }

    // commands for set the value of throttle ,add the path to the send list
    public void SetThrottle(double throttle)
    {
        double value_to_send;

        //checking the range value and change it according to throttle range
        if (throttle < 1)
        {
            if (throttle > 0)
            {
                value_to_send = throttle;
            }
            else
            {
                value_to_send = 0;
            }
        }
        else
        {
            value_to_send = 1;
        }

        string msg = "set /controls/engines/current-engine/throttle " + value_to_send.ToString() + "\n";
        Byte[] bytes = System.Text.Encoding.ASCII.GetBytes(msg);
        lock (bBalanceLock)
        {
            if (this.ShouldAdd)
            {
                this.DataToSend.Add(bytes);
            }

        }
    }

    // commands for set the value of x_rudder and  y_elevator,add the path to the send list
    public void SetDirection(double x_rudder, double y_elevator)
    {
        double value_to_send;

        //checking the range value and change it according to x_rudder range
        if (x_rudder < 1)
        {
            if (x_rudder > -1)
            {
                value_to_send = x_rudder;
            }
            else
            {
                value_to_send = -1;
            }
        }
        else
        {
            value_to_send = 1;
        }

        string msg = "set /controls/flight/rudder " + value_to_send.ToString() + "\n";
        Byte[] bytes = System.Text.Encoding.ASCII.GetBytes(msg);
        lock (bBalanceLock)
        {
            if (this.ShouldAdd)
            {
                this.DataToSend.Add(bytes);

            }
        }

        //checking the range value and change it according to y_elevator range
        if (y_elevator < 1)
        {
            if (y_elevator > -1)
            {
                value_to_send = y_elevator;
            }
            else
            {
                value_to_send = -1;
            }
        }
        else
        {
            value_to_send = 1;
        }
        msg = "set /controls/flight/elevator " + value_to_send.ToString() + "\n";
        Byte[]  bytes2 = System.Text.Encoding.ASCII.GetBytes(msg);
        lock (bBalanceLock)
        {
            if (this.ShouldAdd)
            {
                this.DataToSend.Add(bytes2);
            }
        }
    }

    //create connection with the server(simulator)
    // catch the exp if the server ip or port does not exsit 
    public void Connect(string ip, int port)
    {
        TCPClient.Connect(ip, port);
        this.Start();

    }

    // ShouldStop the thread and log out
    public void Disconnect()
    {
        TCPClient.Close();
        this.ShouldStop = true;
    }

    //this method sample 10 value from the server-simulator in another thread and update the properties 
    // moreover this method first of all send the commands in the "list to send" member (setter commands)
    // we write the 10 getter properrty without little for\while loop becouse we found out that deployed the code
    // faster than write in mini loop. So we coded like that.
    public void Start()
    {
        Thread T = new Thread(delegate ()
        {
            TCPStream = TCPClient.GetStream();
            List<Byte[]> list_data = new List<Byte[]>();
            list_data.Add(System.Text.Encoding.ASCII.GetBytes("get /instrumentation/heading-indicator/indicated-heading-deg\n"));
            list_data.Add(System.Text.Encoding.ASCII.GetBytes("get /instrumentation/gps/indicated-vertical-speed\n"));
            list_data.Add(System.Text.Encoding.ASCII.GetBytes("get /instrumentation/gps/indicated-ground-speed-kt\n"));
            list_data.Add(System.Text.Encoding.ASCII.GetBytes("get /instrumentation/airspeed-indicator/indicated-speed-kt\n"));
            list_data.Add(System.Text.Encoding.ASCII.GetBytes("get /instrumentation/gps/indicated-altitude-ft\n"));
            list_data.Add(System.Text.Encoding.ASCII.GetBytes("get /instrumentation/attitude-indicator/internal-roll-deg\n"));
            list_data.Add(System.Text.Encoding.ASCII.GetBytes("get /instrumentation/attitude-indicator/internal-pitch-deg\n"));
            list_data.Add(System.Text.Encoding.ASCII.GetBytes("get /instrumentation/altimeter/indicated-altitude-ft\n"));
            list_data.Add(System.Text.Encoding.ASCII.GetBytes("get /position/latitude-deg\n"));
            list_data.Add(System.Text.Encoding.ASCII.GetBytes("get /position/longitude-deg\n"));
            Byte[] messageReceived = new byte[256];
            // String to store the response ASCII representation.
            String responseData = String.Empty;
            bool sreverErr = false;
            while (!this.ShouldStop)
            {

                int i = 0;
                Int32 bytes;
                string value;
                // every property we write the protocol to the server ,read the answer ,encoding ,parse it and update.
                try
                {
                    if (sreverErr)
                    {
                        bytes = TCPStream.Read(messageReceived, 0, messageReceived.Length);
                        Errlog = "Status: Connected";
                        sreverErr = false;
                    }
                    try
                    {
                        TCPStream.Write(list_data[i], 0, list_data[i++].Length);
                        bytes = TCPStream.Read(messageReceived, 0, messageReceived.Length);
                        responseData = System.Text.Encoding.ASCII.GetString(messageReceived, 0, bytes);
                        value = System.Text.RegularExpressions.Regex.Match(responseData, @"\d+[.]\d+").Value;
                        HeadingDegree = Double.Parse(value);
                        responseData = String.Empty;
                    }
                    catch (FormatException)
                    {
                        Errlog = "Bad Input: HeadingDegree";
                    }
                    try
                    {
                        TCPStream.Write(list_data[i], 0, list_data[i++].Length);
                        bytes = TCPStream.Read(messageReceived, 0, messageReceived.Length);
                        responseData = System.Text.Encoding.ASCII.GetString(messageReceived, 0, bytes);
                        value = System.Text.RegularExpressions.Regex.Match(responseData, @"\d+[.]\d+").Value;
                        VerticalSpeed = Double.Parse(value);
                        responseData = String.Empty;
                    }
                    catch (FormatException)
                    {
                        Errlog = "Bad Input: VerticalSpeed";
                    }
                    try
                    {
                        TCPStream.Write(list_data[i], 0, list_data[i++].Length);
                        bytes = TCPStream.Read(messageReceived, 0, messageReceived.Length);
                        responseData = System.Text.Encoding.ASCII.GetString(messageReceived, 0, bytes);
                        value = System.Text.RegularExpressions.Regex.Match(responseData, @"\d+[.]\d+").Value;
                        GroundSpeed = Double.Parse(value);
                        responseData = String.Empty;
                    }
                    catch (FormatException)
                    {
                        Errlog = "Bad Input: GroundSpeed";
                    }

                    try
                    {
                        TCPStream.Write(list_data[i], 0, list_data[i++].Length);
                        bytes = TCPStream.Read(messageReceived, 0, messageReceived.Length);
                        responseData = System.Text.Encoding.ASCII.GetString(messageReceived, 0, bytes);
                        value = System.Text.RegularExpressions.Regex.Match(responseData, @"\d+[.]\d+").Value;
                        AirSpeed = Double.Parse(value);
                        responseData = String.Empty;
                    }
                    catch (FormatException)
                    {
                        Errlog = "Bad Input: AirSpeed";
                    }

                    try
                    {
                        TCPStream.Write(list_data[i], 0, list_data[i++].Length);
                        bytes = TCPStream.Read(messageReceived, 0, messageReceived.Length);
                        responseData = System.Text.Encoding.ASCII.GetString(messageReceived, 0, bytes);
                        value = System.Text.RegularExpressions.Regex.Match(responseData, @"\d+[.]\d+").Value;
                        AltitudeFT = Double.Parse(value);
                        responseData = String.Empty;
                    }
                    catch (FormatException)
                    {
                        Errlog = "Bad Input: AltitudeFT";
                    }

                    try
                    {
                        TCPStream.Write(list_data[i], 0, list_data[i++].Length);
                        bytes = TCPStream.Read(messageReceived, 0, messageReceived.Length);
                        responseData = System.Text.Encoding.ASCII.GetString(messageReceived, 0, bytes);
                        value = System.Text.RegularExpressions.Regex.Match(responseData, @"\d+[.]\d+").Value;
                        RollDegree = Double.Parse(value);
                        responseData = String.Empty;
                    }
                    catch (FormatException)
                    {
                        Errlog = "Bad Input: RollDegree";
                    }

                    try
                    {
                        TCPStream.Write(list_data[i], 0, list_data[i++].Length);
                        bytes = TCPStream.Read(messageReceived, 0, messageReceived.Length);
                        responseData = System.Text.Encoding.ASCII.GetString(messageReceived, 0, bytes);
                        value = System.Text.RegularExpressions.Regex.Match(responseData, @"\d+.\d+").Value;
                        PitchDegree = Double.Parse(value);
                        responseData = String.Empty;
                    }
                    catch (FormatException)
                    {
                        Errlog = "Bad Input: PitchDegree";
                    }

                    try
                    {
                        TCPStream.Write(list_data[i], 0, list_data[i++].Length);
                        bytes = TCPStream.Read(messageReceived, 0, messageReceived.Length);
                        responseData = System.Text.Encoding.ASCII.GetString(messageReceived, 0, bytes);
                        value = System.Text.RegularExpressions.Regex.Match(responseData, @"\d+[.]\d+").Value;
                        AltimeterFT = Double.Parse(value);
                        responseData = String.Empty;
                    }
                    catch (FormatException)
                    {
                        Errlog = "Bad Input: AltimeterFT";
                    }

                    try
                    {
                        TCPStream.Write(list_data[i], 0, list_data[i++].Length);
                        bytes = TCPStream.Read(messageReceived, 0, messageReceived.Length);
                        responseData = System.Text.Encoding.ASCII.GetString(messageReceived, 0, bytes);
                        value = System.Text.RegularExpressions.Regex.Match(responseData, @"\d+.\d+").Value;
                        LatitudeDegree = Double.Parse(value);
                        responseData = String.Empty;
                    }
                    catch (FormatException)
                    {
                        Errlog = "Bad Input: Latitude";
                    }

                    try
                    {
                        TCPStream.Write(list_data[i], 0, list_data[i].Length);
                        bytes = TCPStream.Read(messageReceived, 0, messageReceived.Length);
                        responseData = System.Text.Encoding.ASCII.GetString(messageReceived, 0, bytes);
                        value = System.Text.RegularExpressions.Regex.Match(responseData, @"\d+[.]\d+").Value;
                        LongitudeDegree = Double.Parse(value);
                    }
                    catch (FormatException)
                    {
                        Errlog = "Bad Input: Longitude";
                    }

                    //if the long or lat out of range update the err log !!
                    if (LatitudeDegree > 90 || LatitudeDegree < -90)
                    {
                        LongtitudeLatitude = "Wrong: out of range";
                    }

                    else if (LongitudeDegree > 180 || LongitudeDegree < -180)
                    {
                        LongtitudeLatitude = "Wrong: out of range";
                    }
                    else
                    {
                        LongtitudeLatitude = LatitudeDegree + "\n" + LongitudeDegree;
                    }
                    responseData = String.Empty;

                    Location = new Location(LatitudeDegree, LongitudeDegree);
                }
                // catch if passed 10 sec and the server did not return answer 'update the errLog 
                catch (IOException e)
                {
                    Debug.WriteLine(e);
                    if (e.ToString().Contains("connected party did not properly respond after a period of time"))
                    {
                        Errlog = "Conection Timeout";
                        sreverErr = true;
                    } else
                    {
                        Errlog = "The connection was forcibly closed by the remote host, please go back to the main menu and try again";
                        Thread.Sleep(250);
                        Thread.CurrentThread.Abort();
                    }
                    this.DataToSend.Clear();
                }
                //lock this becouse we touch the list to send in 2 places
                lock (bBalanceLock)
                {
                    try
                    {
                        if (this.DataToSend.Count != 0)
                        {
                            int j = 0;
                            do
                            {

                                TCPStream.Write(DataToSend[j], 0, DataToSend[j].Length);
                                Int32 bytes32 = TCPStream.Read(DataToSend[j], 0, DataToSend[j].Length);
                                DataToSend.Remove(DataToSend[j]);

                            } while (this.DataToSend.Count != 0);
                        }
                    }
                    // catch if passed 10 sec and the server did not return answer 'update the errLog 
                    catch (IOException e)
                    {
                        Debug.WriteLine(e);
                        if (e.ToString().Contains("connected party did not properly respond after a period of time"))
                        {
                            Errlog = "Conection Timeout";
                        }
                        else
                        {
                            Errlog = "The connection was forcibly closed by the remote host, please go back to the main menu and try again";
                            Thread.Sleep(250);
                            Thread.CurrentThread.Abort();
                        }
                        this.DataToSend.Clear();
                    }
                    catch
                    {
                        Errlog = "No connection";
                    }
                }
                i = 0;
                Thread.Sleep(250); // read the data in 4Hz
            }
        });
        T.Start();

    }

    // this method activate the observable DP 
    public void NotifyPropertyChanged(string propName)
    {
        if (this.PropertyChanged != null)
            this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
    }

}
