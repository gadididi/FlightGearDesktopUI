using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class SimulatorModel : ISimulatorModel
{
    volatile Boolean stop;
    private TcpClient getter_client;
    

    public SimulatorModel(TcpClient T)
    {
        this.stop = false;
        this.getter_client = T;
    }

    // event
    public event PropertyChangedEventHandler PropertyChanged;

    private double heading_Degree;
    private double vertical_Speed;
    private double ground_Speed;
    private double air_Speed;
    private double altitude_FT;
    private double roll_Degree;
    private double pitch_Degree;
    private double altimeter_FT;
    
    //for the map
    private double longitude_deg;
    private double latitude_deg;

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

    public void setAileron(double aileron)
    {
        double value_to_send;

        if (aileron < 1)
        {
            if (aileron > 0)
            {
                value_to_send = aileron;
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
        NetworkStream stream = getter_client.GetStream();
        string msg = "set /controls/engines/current-engine/throttle " + value_to_send.ToString() + "\r\n";
        Byte[] bytes = System.Text.Encoding.ASCII.GetBytes(msg);
        bytes = new byte[getter_client.ReceiveBufferSize];
        stream.Write(bytes, 0, bytes.Length);
        //Here you send to the server "value_to_send"
    }

    public void setThrottle(double throttle)
    {
        double value_to_send;

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
        NetworkStream stream = getter_client.GetStream();
        string msg = "set /controls/engines/current-engine/throttle " + value_to_send.ToString() + "\r\n";
        Byte[] bytes = System.Text.Encoding.ASCII.GetBytes(msg);
        stream.Write(bytes, 0, bytes.Length);
        //Here you send to the server "value_to_send"
    }

    public void setDirection(double x_rudder, double y_elevator)
    {
        double value_to_send;
        NetworkStream stream = getter_client.GetStream();

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
        string msg = "set /controls/flight/rudder " + value_to_send.ToString() + "\r\n";
        Byte[] bytes = System.Text.Encoding.ASCII.GetBytes(msg);
        stream.Write(bytes, 0, bytes.Length);
        //Here you send to the server "value_to_send"

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
         msg = "set /controls/flight/elevator " + value_to_send.ToString() + "\r\n";
         bytes = System.Text.Encoding.ASCII.GetBytes(msg);
         stream.Write(bytes, 0, bytes.Length);
        //Here you send to the server "value_to_send"
    }

    public void Connect(string ip, int port)
    {
        getter_client.Connect(ip, port);
        this.Start();
    }

    public void Disconnect()
    {
        getter_client.Close();
        this.stop = true;
    }

    public void Start()
    {
        Thread T  = new Thread(delegate ()
        {

            NetworkStream stream = getter_client.GetStream();
            List<Byte[]> list_data = new List<Byte[]>();
            list_data.Add(System.Text.Encoding.ASCII.GetBytes("get /instrumentation/heading-indicator/indicated-heading-deg\r\n"));
            list_data.Add(System.Text.Encoding.ASCII.GetBytes("get /instrumentation/gps/indicated-vertical-speed\r\n"));
            list_data.Add(System.Text.Encoding.ASCII.GetBytes("get /instrumentation/gps/indicated-ground-speed-kt\r\n"));
            list_data.Add(System.Text.Encoding.ASCII.GetBytes("get /instrumentation/airspeed-indicator/indicated-speed-kt\r\n"));
            list_data.Add(System.Text.Encoding.ASCII.GetBytes("get /instrumentation/gps/indicated-altitude-ft\r\n"));
            list_data.Add(System.Text.Encoding.ASCII.GetBytes("get /instrumentation/attitude-indicator/internal-roll-deg\r\n"));
            list_data.Add(System.Text.Encoding.ASCII.GetBytes("get /instrumentation/attitude-indicator/internal-pitch-deg\r\n"));
            list_data.Add(System.Text.Encoding.ASCII.GetBytes("get /instrumentation/altimeter/indicated-altitude-ft\r\n"));
            list_data.Add(System.Text.Encoding.ASCII.GetBytes("get /position/latitude-deg\r\n"));
            list_data.Add(System.Text.Encoding.ASCII.GetBytes("get /position/longitude-deg\r\n"));
            Byte[] messageReceived = new byte[256];
            // String to store the response ASCII representation.
            String responseData = String.Empty;
            while (!this.stop)
            {

                int i = 0;
                stream.Write(list_data[i], 0, list_data[i++].Length);
                Int32 bytes = stream.Read(messageReceived, 0, messageReceived.Length);
                responseData = System.Text.Encoding.ASCII.GetString(messageReceived, 0, bytes);
                string[] words = responseData.Split(' ');
                words[2] = words[2].Replace("\r\n", string.Empty);
                words[2] = words[2].Replace("'", string.Empty);
                Heading_Degree = Double.Parse(words[2]);
                responseData = String.Empty;

                stream.Write(list_data[i], 0, list_data[i++].Length);
                bytes = stream.Read(messageReceived, 0, messageReceived.Length);
                responseData = System.Text.Encoding.ASCII.GetString(messageReceived, 0, bytes);
                words = responseData.Split(' ');
                words[2] = words[2].Replace("\r\n", string.Empty);
                words[2] = words[2].Replace("'", string.Empty);
                Vertical_Speed = Double.Parse(words[2]);
                responseData = String.Empty;

                stream.Write(list_data[i], 0, list_data[i++].Length);
                bytes = stream.Read(messageReceived, 0, messageReceived.Length);
                responseData = System.Text.Encoding.ASCII.GetString(messageReceived, 0, bytes);
                words = responseData.Split(' ');
                words[2] = words[2].Replace("\r\n", string.Empty);
                words[2] = words[2].Replace("'", string.Empty);
                Ground_Speed = Double.Parse(words[2]);
                responseData = String.Empty;

                stream.Write(list_data[i], 0, list_data[i++].Length);
                bytes = stream.Read(messageReceived, 0, messageReceived.Length);
                responseData = System.Text.Encoding.ASCII.GetString(messageReceived, 0, bytes);
                words = responseData.Split(' ');
                words[2] = words[2].Replace("\r\n", string.Empty);
                words[2] = words[2].Replace("'", string.Empty);
                Air_Speed = Double.Parse(words[2]);
                responseData = String.Empty;

                stream.Write(list_data[i], 0, list_data[i++].Length);
                bytes = stream.Read(messageReceived, 0, messageReceived.Length);
                responseData = System.Text.Encoding.ASCII.GetString(messageReceived, 0, bytes);
                words = responseData.Split(' ');
                words[2] = words[2].Replace("\r\n", string.Empty);
                words[2] = words[2].Replace("'", string.Empty);
                Altitude_FT = Double.Parse(words[2]);
                responseData = String.Empty;

                stream.Write(list_data[i], 0, list_data[i++].Length);
                bytes = stream.Read(messageReceived, 0, messageReceived.Length);
                responseData = System.Text.Encoding.ASCII.GetString(messageReceived, 0, bytes);
                words = responseData.Split(' ');
                words[2] = words[2].Replace("\r\n", string.Empty);
                words[2] = words[2].Replace("'", string.Empty);
                Roll_Degree = Double.Parse(words[2]);
                responseData = String.Empty;

                stream.Write(list_data[i], 0, list_data[i++].Length);
                bytes = stream.Read(messageReceived, 0, messageReceived.Length);
                responseData = System.Text.Encoding.ASCII.GetString(messageReceived, 0, bytes);
                words = responseData.Split(' ');
                words[2] = words[2].Replace("\r\n", string.Empty);
                words[2] = words[2].Replace("'", string.Empty);
                Pitch_Degree = Double.Parse(words[2]);
                responseData = String.Empty;

                stream.Write(list_data[i], 0, list_data[i++].Length);
                bytes = stream.Read(messageReceived, 0, messageReceived.Length);
                responseData = System.Text.Encoding.ASCII.GetString(messageReceived, 0, bytes);
                words = responseData.Split(' ');
                words[2] = words[2].Replace("\r\n", string.Empty);
                words[2] = words[2].Replace("'", string.Empty);
                Altimeter_FT = Double.Parse(words[2]);
                responseData = String.Empty;

                stream.Write(list_data[i], 0, list_data[i++].Length);
                bytes = stream.Read(messageReceived, 0, messageReceived.Length);
                responseData = System.Text.Encoding.ASCII.GetString(messageReceived, 0, bytes);
                words = responseData.Split(' ');
                words[2] = words[2].Replace("\r\n", string.Empty);
                words[2] = words[2].Replace("'", string.Empty);
                Latitude_deg = Double.Parse(words[2]);
                responseData = String.Empty;

                stream.Write(list_data[i], 0, list_data[i].Length);
                bytes = stream.Read(messageReceived, 0, messageReceived.Length);
                responseData = System.Text.Encoding.ASCII.GetString(messageReceived, 0, bytes);
                words = responseData.Split(' ');
                words[2] = words[2].Replace("\r\n", string.Empty);
                words[2] = words[2].Replace("'", string.Empty);
                Longitude_deg = Double.Parse(words[2]);
                responseData = String.Empty;
                i = 0;
                Thread.Sleep(250); // read the data in 4Hz
            }
        });
        T.Start();

    }
    public void run()
    {

    }

    public void NotifyPropertyChanged(string propName)
    {
        if (this.PropertyChanged != null)
            this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
    }

}
