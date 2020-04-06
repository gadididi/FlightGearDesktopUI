using Microsoft.Maps.MapControl.WPF;
using System;
using System.ComponentModel;
using System.Net.Sockets;

public interface ISimulatorModel : INotifyPropertyChanged
{

	void Connect(string ip, int port);
	void Disconnect();
	void Start();

	string Long_lat { get; set; }
	//airplane properties
	double Heading_Degree { get; set; }
	double Vertical_Speed { get; set; }
	double Ground_Speed { get; set; }
	 double Air_Speed { get; set; }
	 double Altitude_FT { get; set; }
	 double Roll_Degree { get; set; }
	 double Pitch_Degree { get; set; }
	 double Altimeter_FT { get; set; }

	 double Latitude_deg { get; set; }

	 double Longitude_deg { get; set; }

	 Location Location { get; set; }

	//err log property to recognize things that happen in the communications
	 string Errlog { get; set; }

	//three commands to send to the server(simulator)
	 void setAileron(double aileron);

	 void setThrottle(double throttle);

	 void setDirection(double x_rudder, double y_elevator);

}
