# FlightGearDesktopUI

A Desktop app which connects to FlightGear simulator and controls the aircrafts.
GitHub repository link: [Here](https://github.com/ori294/FlightGearDesktopUI)

## General Description

This program controls an aircraft with in the "[_FlightGear_]" (https://www.flightgear.org/) flight simulator. 
The program connects to the flight simulator both as client.
The client will send the simulator instructions That allows us to control our flight vessel from the UI.

## Collaborators

This program was developed by Gadi Didi and Ori Levy, CS students from Bar-Ilan university, Israel.


## Code Design:
FlightSimulatorApp has been programed by the MVVM design, as we used C Sharp's data binding mechanism.
The viewModels and the model are initialized at the startup of the program at App.xaml and are completely independendent from each other.

## Features
#### Map:
Using Bing Maps SDK, the map shows the current location of the flight vessel. You can travel around the world and explore the map as you please. In order to certerilize the map around the flight vessel, just click on the "Center me" button.

#### Controller:
Using a joystick and two sliders, you can control the flight vessel. The joystick controls the Rudder and Elevator while the sliders control the Throttle and Aileron.

#### Real-Time flight data:
During the flight, you'll recieve real-time flight data, such as Longtitude, Latitude, Air Speed, Pitch, Roll, Heading and more.

#### Connection Status and error log indicator:
Just under the map, there's an indicator which shows the connection status and whether an error had occured or not.


