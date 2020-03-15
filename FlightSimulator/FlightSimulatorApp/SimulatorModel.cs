using System;
using System.ComponentModel;
using System.Net.Sockets;

public class SimulatorModel : ISimulatorModel
{
	private TcpClient getter_client;
	private TcpClient setter_client;

	public SimulatorModel()
	{
	}

	public double Rudder { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
	public double Elevator { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
	public double Aileron { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
	public double Throttle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
	public double Heading_Degree { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
	public double Vertical_Speed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
	public double Ground_Speed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
	public double Air_Speed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
	public double Altitude_FT { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
	public double Roll_Degree { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
	public double Pitch_Degree { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
	public double Altimeter_FT { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

	public event PropertyChangedEventHandler PropertyChanged;

	public void Connect(string ip, int port)
	{
		getter_client.Connect(ip, port);
		setter_client.Connect(ip, port);
	}

	public void Disconnect()
	{
		getter_client.Close();
		setter_client.Close();
	}

	public void Start()
	{
		//NetworkStream stream = setter_client.GetStream();
	
	}
}
