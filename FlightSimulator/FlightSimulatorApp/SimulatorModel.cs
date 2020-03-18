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
        string msg = "set/controls/engines/current-engine/throttle " + throttle.ToString() + "\n";
        Byte[] bytes = System.Text.Encoding.ASCII.GetBytes(msg);
        bytes = new byte[getter_client.ReceiveBufferSize];
        stream.Write(bytes, 0, bytes.Length);
        //Here you send to the server "value_to_send"
    }

    public void setDirection(double x_rudder, double y_elevator)
    {
        double value_to_send;

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
            list_data.Add(System.Text.Encoding.ASCII.GetBytes("get/instrumentation/heading-indicator/indicated-heading-deg"));
            list_data.Add(System.Text.Encoding.ASCII.GetBytes("get/instrumentation/gps/indicated-vertical-speed"));
            list_data.Add(System.Text.Encoding.ASCII.GetBytes("get/instrumentation/gps/indicated-ground-speed-kt"));
            list_data.Add(System.Text.Encoding.ASCII.GetBytes("get/instrumentation/airspeed-indicator/indicated-speed-kt"));
            list_data.Add(System.Text.Encoding.ASCII.GetBytes("get/instrumentation/gps/indicated-altitude-ft"));
            list_data.Add(System.Text.Encoding.ASCII.GetBytes("get/instrumentation/attitude-indicator/internal-roll-deg"));
            list_data.Add(System.Text.Encoding.ASCII.GetBytes("get/instrumentation/attitude-indicator/internal-pitch-deg"));
            list_data.Add(System.Text.Encoding.ASCII.GetBytes("get/instrumentation/altimeter/indicated-altitude-ft"));
            list_data.Add(System.Text.Encoding.ASCII.GetBytes("get/position/latitude-deg"));
            list_data.Add(System.Text.Encoding.ASCII.GetBytes("get/position/longitude-deg"));
            Byte[] bytes;
            bytes = new byte[getter_client.ReceiveBufferSize];
            while (!this.stop)
            {

                int i = 0;
                stream.Write(list_data[i], 0, list_data[i++].Length);
                stream.Read(bytes, 0, getter_client.ReceiveBufferSize);
                Heading_Degree = Double.Parse(Encoding.UTF8.GetString(bytes));
                stream.Write(list_data[i], 0, list_data[i++].Length);
                stream.Read(bytes, 0, getter_client.ReceiveBufferSize);
                Vertical_Speed = Double.Parse(Encoding.UTF8.GetString(bytes));
                stream.Write(list_data[i], 0, list_data[i++].Length);
                stream.Read(bytes, 0, getter_client.ReceiveBufferSize);
                Ground_Speed = Double.Parse(Encoding.UTF8.GetString(bytes));
                stream.Write(list_data[i], 0, list_data[i++].Length);
                stream.Read(bytes, 0, getter_client.ReceiveBufferSize);

                Air_Speed = Double.Parse(Encoding.UTF8.GetString(bytes));
                stream.Write(list_data[i], 0, list_data[i++].Length);
                stream.Read(bytes, 0, getter_client.ReceiveBufferSize);

                Altitude_FT = Double.Parse(Encoding.UTF8.GetString(bytes));
                stream.Write(list_data[i], 0, list_data[i++].Length);
                stream.Read(bytes, 0, getter_client.ReceiveBufferSize);

                Roll_Degree = Double.Parse(Encoding.UTF8.GetString(bytes));
                stream.Write(list_data[i], 0, list_data[i++].Length);
                stream.Read(bytes, 0, getter_client.ReceiveBufferSize);

                Pitch_Degree = Double.Parse(Encoding.UTF8.GetString(bytes));
                stream.Write(list_data[i], 0, list_data[i++].Length);
                stream.Read(bytes, 0, getter_client.ReceiveBufferSize);

                Altimeter_FT = Double.Parse(Encoding.UTF8.GetString(bytes));
                stream.Write(list_data[i], 0, list_data[i++].Length);
                stream.Read(bytes, 0, getter_client.ReceiveBufferSize);

                Latitude_deg = Double.Parse(Encoding.UTF8.GetString(bytes));
                stream.Write(list_data[i], 0, list_data[i].Length);
                stream.Read(bytes, 0, getter_client.ReceiveBufferSize);

                Longitude_deg = Double.Parse(Encoding.UTF8.GetString(bytes));
                i = 0;
                Thread.Sleep(250); // read the data in 4Hz
            }
        });
        T.Start();

    }
    public void run()
    {

    }

    //#### NEED TO FILL IN !! ####
    // activate actuators


    public void NotifyPropertyChanged(string propName)
    {
        if (this.PropertyChanged != null)
            this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
    }

}
