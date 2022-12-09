# RevitApi
The purpose of this project is getting sensor data from a online database and link them to revit model.
## Overview
This Code contains below classes:
* AdskApplication and AdskCommand: these classes are for creating ribbon tab with related buttons and defining the usercontrol as dockable pane, respectively.
* Userontrol: In the code behind the usercontrol the function of each button include filling variables like Sdate,Edate and Request_Type and
calling the "ExternalEventHandlerGetData" class.
* ExternalEventHandlerGetData: There are all API functions and operations in this class. In fact the requests come from usercontrol and based on Request_Type
the related sub or function will be called.
* Object classes: These classes include SensorItem (for defining the sensor elements that exist in the model), SensorData (for defining all sensor types data
except Particle sensor because its structure is different) and ParticleData.
## Installation and execute
Before running the code, the addresses in the Build Events (Compile) and Revit.exe address in debug tab in the solution properties
should be edited accoriding to your paths.
After running the code, at first related dll files will be copy automatically in target folder and Revit application will be started.
Now you should follow belew procedure:

At first open Revit 2023 and push the “Arrow” button shown in below picture:
![alt text](https://github.com/Ali-Mansoori/RevitApi2/blob/master/install1.JPG?raw=true)
Then push the register button shown in below picture:
![alt text](https://github.com/Ali-Mansoori/RevitApi2/blob/master/install2.JPG?raw=true)
Now open your revit file.
![alt text](https://github.com/Ali-Mansoori/RevitApi2/blob/master/install3.JPG?raw=true)

* Get last status: 
Select a sensor element and click the “Get Sensor Data”
The last value of parameters will be shown in textboxes.
* Heat Map:
1-	Select all sensor elements (You can select all elemnts, the code recognize the sensor elements) and click the “Get Sensor Elements”. The sensor element now defined and their name list will be shown in a messagebox.
2-	Choose the parameter type, Dates and the Interval and then click “Get” to request the related data. Wait seconds and now model prepared for heatmap animation.
3-	You can play, pause, go to the next and previous date and reset by related buttons.
* IFC:
Select the folder path to export model in IFC format.
