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

## Collaborators

This program was developed by [Eliadsellem](https://github.com/Eliadsellem) and [shirgoldberger](https://github.com/shirgoldberger), CS students from Bar-Ilan university, Israel.


## Features
#### Exit button:
The "exit" button is shown for all the time the program is running and give an option to the user finish with the program.

#### Back button:
The "back" button is shown after we successively made a log in and give the possibility to go back to the log in page and reconnect.

#### Map:
The Map has many features:
- first of all we have a focus button that allowing to focus at our plain and move the map so the plain will be at the center of it.

- when the plain is exceed the map from any direction, the map make an update so that the plain will still be shown and the plain will be shown like its continues from the other side.
for example: 
if the plain is exceed the map from above with a positive Slope, if its exceeding from the right/left side so it'll look like the plain continues flight from the left side of the bottom.
but if it was exceeding from above with negative Slope, if its exceeding from the right/left side so it'll look like the plain continues flight from the right side of the bottom.

- you have the possibility to play with the map and move it and focus, and if the plain is still on the map the functionality above will still work,
else it makes you at a free zone and you can continue playing until you will re-find the plain or click on the focus button.

#### Connection Status Window:
There is a window that display to the user about the connection status.

#### Errors:

Errors massages are displays on the UI if there is a connection problem with the simulator, invalid inputs given from the simulator, 10 sec timeout for reading a massage, or any other exception problems.
