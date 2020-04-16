using Microsoft.Maps.MapControl.WPF;
using System.ComponentModel;

public interface ISimulatorModel : INotifyPropertyChanged
{

	void Connect(string ip, int port);
	void Disconnect();
	void Start();

	string LongtitudeLatitude { get; set; }
	//airplane properties
	double HeadingDegree { get; set; }
	double VerticalSpeed { get; set; }
	double GroundSpeed { get; set; }
	 double AirSpeed { get; set; }
	 double AltitudeFT { get; set; }
	 double RollDegree { get; set; }
	 double PitchDegree { get; set; }
	 double AltimeterFT { get; set; }

	 double LatitudeDegree { get; set; }

	 double LongitudeDegree { get; set; }

	 Location Location { get; set; }

	//err log property to recognize things that happen in the communications
	 string Errlog { get; set; }

	//three commands to send to the server(simulator)
	 void SetAileron(double aileron);

	 void SetThrottle(double throttle);

	 void SetDirection(double rudder, double elevator);

}
