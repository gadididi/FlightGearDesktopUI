using System;
using System.ComponentModel;
using System.Net.Sockets;

public interface ISimulatorModel : INotifyPropertyChanged
{

	public void Connect(string ip, int port);
	public void Disconnect();
	public void Start();


	//airplane properties
	public double Heading_Degree { get; set; }
	public double Vertical_Speed { get; set; }
	public double Ground_Speed { get; set; }
	public double Air_Speed { get; set; }
	public double Altitude_FT { get; set; }
	public double Roll_Degree { get; set; }
	public double Pitch_Degree { get; set; }
	public double Altimeter_FT { get; set; }

}
