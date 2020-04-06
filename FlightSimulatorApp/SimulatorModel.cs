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
    // stop for stop the thread in the staet method
    // tcpclient for the comunication with the server

    volatile Boolean stop;
    private TcpClient getter_client;
    private NetworkStream stream;
    private List<Byte[]> To_send = new List<Byte[]>();
    private readonly object balanceLock = new object();

    //CTOR 
    public SimulatorModel(TcpClient T)
    {
        this.stop = false;
        this.getter_client = T;
        this.location = new Location(0, 0);
        //Sets the error log to default value
        this.Errlog = "Status: Connected";
        // Sets the receive time out using the ReceiveTimeout public property.
        getter_client.ReceiveTimeout = 10000;

        // Gets the receive time out using the ReceiveTimeout public property.
        if (getter_client.ReceiveTimeout == 10000)
            Debug.WriteLine("The receive time out limit was successfully set " + getter_client.ReceiveTimeout.ToString());
    }

    // event
    public event PropertyChangedEventHandler PropertyChanged;

    // 8 members for dashboard
    private double heading_Degree;
    private double vertical_Speed;
    private double ground_Speed;
    private double air_Speed;
    private double altitude_FT;
    private double roll_Degree;
    private double pitch_Degree;
    private double altimeter_FT;

    //members for the map
    private double longitude_deg;
    private double latitude_deg;
    private Location location;
    private string long_lat;

    //member Error Log
    private string errlog;

    //here 8 property for dashboard
    // more three for pin in map
    // Errlog property for sign kinds of err to the view by binding
    public string Long_lat
    {
        get { return long_lat; }
        set
        {
            long_lat = value;
            NotifyPropertyChanged("Long_lat");
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
        get { return location; }
        set
        {
            location = value;
            NotifyPropertyChanged("Location");
        }
    }

    public double Longitude_deg
    {
        get { return longitude_deg; }
        set
        {
            longitude_deg = value;
            NotifyPropertyChanged("Longitude_deg");
        }
    }

    public double Latitude_deg
    {
        get { return latitude_deg; }
        set
        {
            latitude_deg = value;
            NotifyPropertyChanged("Latitude_deg");
        }
    }

    public double Heading_Degree
    {
        get { return heading_Degree; }
        set
        {
            heading_Degree = value;
            NotifyPropertyChanged("Heading_Degree");
        }
    }
    public double Vertical_Speed
    {
        get { return vertical_Speed; }
        set
        {
            vertical_Speed = value;
            NotifyPropertyChanged("Vertical_Speed");
        }
    }
    public double Ground_Speed
    {
        get { return ground_Speed; }
        set
        {
            ground_Speed = value;
            NotifyPropertyChanged("Ground_Speed");
        }
    }
    public double Air_Speed
    {
        get { return air_Speed; }
        set
        {
            air_Speed = value;
            NotifyPropertyChanged("Air_Speed");
        }
    }
    public double Altitude_FT
    {
        get { return altitude_FT; }
        set
        {
            altitude_FT = value;
            NotifyPropertyChanged("Altitude_FT");
        }
    }
    public double Roll_Degree
    {
        get { return roll_Degree; }
        set
        {
            roll_Degree = value;
            NotifyPropertyChanged("Roll_Degree");
        }
    }
    public double Pitch_Degree
    {
        get { return pitch_Degree; }
        set
        {
            pitch_Degree = value;
            NotifyPropertyChanged("Pitch_Degree");
        }
    }
    public double Altimeter_FT
    {
        get { return altimeter_FT; }
        set
        {
            altimeter_FT = value;
            NotifyPropertyChanged("Altimeter_FT");
        }
    }

    // commands for set the value of aileron ,add the path to the send list
    public void setAileron(double aileron)
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
        lock (balanceLock)
        {
            this.To_send.Add(bytes);
        }
    }

    // commands for set the value of throttle ,add the path to the send list
    public void setThrottle(double throttle)
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
        lock (balanceLock)
        {
            this.To_send.Add(bytes);
        }
    }

    // commands for set the value of x_rudder and  y_elevator,add the path to the send list
    public void setDirection(double x_rudder, double y_elevator)
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
        lock (balanceLock)
        {
            this.To_send.Add(bytes);
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
        lock (balanceLock)
        {
            this.To_send.Add(bytes2);
        }
    }

    //create connection with the server(simulator)
    // catch the exp if the server ip or port does not exsit 
    public void Connect(string ip, int port)
    {
        getter_client.Connect(ip, port);
        this.Start();

    }

    // stop the thread and log out
    public void Disconnect()
    {
        getter_client.Close();
        this.stop = true;
    }

    //this method sample 10 value from the server-simulator in another thread and update the properties 
    // moreover this method first of all send the commands in the "list to send" member (setter commands)
    // we write the 10 getter properrty without little for\while loop becouse we found out that deployed the code
    // faster than write in mini loop. So we coded like that.
    public void Start()
    {
        Thread T = new Thread(delegate ()
        {
            stream = getter_client.GetStream();
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
            while (!this.stop)
            {
                //lock this becouse we touch the list to send in 2 places
                lock (balanceLock)
                {
                    try
                    {
                        if (this.To_send.Count != 0)
                        {
                            int j = 0;
                            do
                            {

                                stream.Write(To_send[j], 0, To_send[j].Length);
                                Int32 bytes32 = stream.Read(To_send[j], 0, To_send[j].Length);
                                To_send.Remove(To_send[j]);

                            } while (this.To_send.Count != 0);
                        }
                    } catch
                    {
                        Errlog = "No connection";
                    }
                }

                int i = 0;
                Int32 bytes;
                string value;
                // every property we write the protocol to the server ,read the answer ,encoding ,parse it and update.
                try
                {
                    try
                    {
                        Errlog = "Status: Connected";
                        stream.Write(list_data[i], 0, list_data[i++].Length);
                        bytes = stream.Read(messageReceived, 0, messageReceived.Length);
                        responseData = System.Text.Encoding.ASCII.GetString(messageReceived, 0, bytes);
                        value = System.Text.RegularExpressions.Regex.Match(responseData, @"\d+[.]\d+").Value;
                        Heading_Degree = Double.Parse(value);
                        responseData = String.Empty;
                    }
                    catch (FormatException)
                    {
                        Errlog = "Bad Input: Heading_Degree";
                    }
                    try
                    {
                        stream.Write(list_data[i], 0, list_data[i++].Length);
                        bytes = stream.Read(messageReceived, 0, messageReceived.Length);
                        responseData = System.Text.Encoding.ASCII.GetString(messageReceived, 0, bytes);
                        value = System.Text.RegularExpressions.Regex.Match(responseData, @"\d+[.]\d+").Value;
                        Vertical_Speed = Double.Parse(value);
                        responseData = String.Empty;
                    }
                    catch (FormatException)
                    {
                        Errlog = "Bad Input: Vertical_Speed";
                    }
                    try
                    {
                        stream.Write(list_data[i], 0, list_data[i++].Length);
                        bytes = stream.Read(messageReceived, 0, messageReceived.Length);
                        responseData = System.Text.Encoding.ASCII.GetString(messageReceived, 0, bytes);
                        value = System.Text.RegularExpressions.Regex.Match(responseData, @"\d+[.]\d+").Value;
                        Ground_Speed = Double.Parse(value);
                        responseData = String.Empty;
                    }
                    catch (FormatException)
                    {
                        Errlog = "Bad Input: Ground_Speed";
                    }

                    try
                    {
                        stream.Write(list_data[i], 0, list_data[i++].Length);
                        bytes = stream.Read(messageReceived, 0, messageReceived.Length);
                        responseData = System.Text.Encoding.ASCII.GetString(messageReceived, 0, bytes);
                        value = System.Text.RegularExpressions.Regex.Match(responseData, @"\d+[.]\d+").Value;
                        Air_Speed = Double.Parse(value);
                        responseData = String.Empty;
                    }
                    catch (FormatException)
                    {
                        Errlog = "Bad Input: Air_Speed";
                    }

                    try
                    {
                        stream.Write(list_data[i], 0, list_data[i++].Length);
                        bytes = stream.Read(messageReceived, 0, messageReceived.Length);
                        responseData = System.Text.Encoding.ASCII.GetString(messageReceived, 0, bytes);
                        value = System.Text.RegularExpressions.Regex.Match(responseData, @"\d+[.]\d+").Value;
                        Altitude_FT = Double.Parse(value);
                        responseData = String.Empty;
                    }
                    catch (FormatException)
                    {
                        Errlog = "Bad Input: Altitude_FT";
                    }

                    try
                    {
                        stream.Write(list_data[i], 0, list_data[i++].Length);
                        bytes = stream.Read(messageReceived, 0, messageReceived.Length);
                        responseData = System.Text.Encoding.ASCII.GetString(messageReceived, 0, bytes);
                        value = System.Text.RegularExpressions.Regex.Match(responseData, @"\d+[.]\d+").Value;
                        Roll_Degree = Double.Parse(value);
                        responseData = String.Empty;
                    }
                    catch (FormatException)
                    {
                        Errlog = "Bad Input: Roll_Degree";
                    }

                    try
                    {
                        stream.Write(list_data[i], 0, list_data[i++].Length);
                        bytes = stream.Read(messageReceived, 0, messageReceived.Length);
                        responseData = System.Text.Encoding.ASCII.GetString(messageReceived, 0, bytes);
                        value = System.Text.RegularExpressions.Regex.Match(responseData, @"\d+.\d+").Value;
                        Pitch_Degree = Double.Parse(value);
                        responseData = String.Empty;
                    }
                    catch (FormatException)
                    {
                        Errlog = "Bad Input: Pitch_Degree";
                    }

                    try
                    {
                        stream.Write(list_data[i], 0, list_data[i++].Length);
                        bytes = stream.Read(messageReceived, 0, messageReceived.Length);
                        responseData = System.Text.Encoding.ASCII.GetString(messageReceived, 0, bytes);
                        value = System.Text.RegularExpressions.Regex.Match(responseData, @"\d+[.]\d+").Value;
                        Altimeter_FT = Double.Parse(value);
                        responseData = String.Empty;
                    }
                    catch (FormatException)
                    {
                        Errlog = "Bad Input: Altimeter_FT";
                    }

                    try
                    {
                        stream.Write(list_data[i], 0, list_data[i++].Length);
                        bytes = stream.Read(messageReceived, 0, messageReceived.Length);
                        responseData = System.Text.Encoding.ASCII.GetString(messageReceived, 0, bytes);
                        value = System.Text.RegularExpressions.Regex.Match(responseData, @"\d+.\d+").Value;
                        Latitude_deg = Double.Parse(value);
                        responseData = String.Empty;
                    }
                    catch (FormatException)
                    {
                        Errlog = "Bad Input: Latitude_deg";
                    }

                    try
                    {
                        stream.Write(list_data[i], 0, list_data[i].Length);
                        bytes = stream.Read(messageReceived, 0, messageReceived.Length);
                        responseData = System.Text.Encoding.ASCII.GetString(messageReceived, 0, bytes);
                        value = System.Text.RegularExpressions.Regex.Match(responseData, @"\d+[.]\d+").Value;
                        Longitude_deg = Double.Parse(value);
                    }
                    catch (FormatException)
                    {
                        Errlog = "Bad Input: Longitude_deg";
                    }

                    //if the long or lat out of range update the err log !!
                    if (Latitude_deg > 90 || Latitude_deg < -90)
                    {
                        Long_lat = "Wrong: out of range";
                    }

                    else if (Longitude_deg > 180 || Longitude_deg < -180)
                    {
                        Long_lat = "Wrong: out of range";
                    }
                    else
                    {
                        Long_lat = Latitude_deg.ToString().Substring(0, 6) + ", " + Longitude_deg.ToString().Substring(0, 6);
                    }
                    responseData = String.Empty;

                    Location = new Location(Latitude_deg, Longitude_deg);
                }
                // catch if passed 10 sec and the server did not return answer 'update the errLog 
                catch (IOException)
                {
                    Errlog = "Conection Timeout";
                    Thread.Sleep(1000);
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
