using Microsoft.Maps.MapControl.WPF;
using System;
using System.ComponentModel;
using System.Net.Sockets;

public interface ISimulatorModel : INotifyPropertyChanged
{

	public void Connect(string ip, int port);
	public void Disconnect();
	public void Start();

	public string Long_lat { get; set; }
	//airplane properties
	public double Heading_Degree { get; set; }
	public double Vertical_Speed { get; set; }
	public double Ground_Speed { get; set; }
	public double Air_Speed { get; set; }
	public double Altitude_FT { get; set; }
	public double Roll_Degree { get; set; }
	public double Pitch_Degree { get; set; }
	public double Altimeter_FT { get; set; }

	public double Latitude_deg { get; set; }

	public double Longitude_deg { get; set; }

	public Location Location { get; set; }

	//err log property to recognize things that happen in the communications
	public string Errlog { get; set; }

	//three commands to send to the server(simulator)
	public void setAileron(double aileron);

	public void setThrottle(double throttle);

	public void setDirection(double x_rudder, double y_elevator);

}
