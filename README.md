# FlightGearDesktopUI

A Desktop app which connects to FlightGear simulator and controls the aircrafts.
GitHub repository link: [Here](https://github.com/ori294/FlightGearDesktopUI)

## General Description

This program controls an aircraft with in the "[_FlightGear_]" (https://www.flightgear.org/) flight simulator. 
The program connects to the flight simulator both as client.
The client will send the simulator instructions That allows us to control our flight vessel from the UI.

## Collaborators

This program was developed by Gadi Didi and Ori Levy, CS students from Bar-Ilan university, Israel.


## General Description

##### User Interface
- The program open at the log in page and ask from the user to enter an IP and Port.
- once the user clicking on "Fly" the model will start a connection eith the simulator for recieve and send information.
- The simulator page will be open and the user will be able to watch the movment of the plain, the dashboard and to be able to move the joistick,
sliders provided to the user.

FlightSimulatorApp has been programed by the mvvm design.
The viewModels and the model are initialized at the startup of the program at App.xaml
and are independence.
#### The view:
The view is a observer and get information from 3 independence view model that we will describe later.
In addition, the view is also listen to updates of the "Navigator" control and update the relevant view Model about the changes that have been made.

#### The view Models:
There are three view models, each one is responsible to different matter.
- Errors_VM - is responsible to update the view about errors occur at the model sector.
- get_VM - is responsible to update the view about information we received from the simulator server related to the Dashboard.
- set_VM - is responsible to update the model about information that have been change in the view by actions of the user.

#### The Model:
The model is responsible to make a connection with the simulator server and get information about the Dashboard and update him if its necessary about
actions of the user.

## Features
#### Map:
Using Bing Maps SDK, the map shows the current location of the flight vessel. You can travel around the world and explore the map as you please. In order to certerilize the map around the flight vessel, just click on the "Center me" button.

#### Controller:
Using a joystick and two sliders, you can control the flight vessel. The joystick controls the Rudder and Elevator while the sliders control the Throttle and Aileron.

#### Real-Time flight data:
During the flight, you'll recieve real-time flight data, such as Longtitude, Latitude, Air Speed, Pitch, Roll, Heading and more.

#### Connection Status and error log indicator:
Just under the map, there's an indicator which shows the connection status and whether an error had occured or not.


