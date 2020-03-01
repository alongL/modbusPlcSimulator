# modbusPlcSimulator
multi modbus slave devices simulator (modbus server, works as  PLC devices)

[Chinese version](https://github.com/alongL/modbusPlcSimulator/blob/master/README_CN.md "chinese doc")

##  1.purpose
+ This program is mainly used to simulate MODBUS devices. Through configuration files, multiple devices can be simulated at once, starting and stopping with one key. 
+ Support to view the internal value of register and MODBUS client access log.
+ Support automatic data refresh by providing * _data.csv file.


Program run window 
![screenshot.png](https://raw.githubusercontent.com/alongL/modbusPlcSimulator/master/imgs/screenshot.png "window UI")

Register display window 
![screenshot-register.png](https://raw.githubusercontent.com/alongL/modbusPlcSimulator/master/imgs/screenshot-register.png "register window")



##  2. How to use
+ After compiling, copy the config directory to the same directory as the executable exe. After configuring the CSV file correctly, you can use it.
+ Click the play button in the play window to refresh the data according to the CSV. When refreshing, select a row from the [data. CSV] to refresh the data.
+ Do not close the play window. Currently, closing the play window will end the data refresh thread.
+ You can view the underlying access of Modbus Protocol in the log window, and the Modbus reading and writing are recorded.
+ If you don't want to comile it. You just need download and run it. Go directly to [Release Page] (https://github.com/alongl/modbusplcsimulator/releases) to download this program


##  3.  Principle
+ PLC provides Modbus protocol, which can be simulated by providing MODBUS service. In the past, modsim was mainly used for simulation. In this way, only one device can be simulated at a time, and the operation is relatively troublesome. This program is easy to operate and use. It only needs to run, that is, it can simulate multiple devices at the same time, and it is convenient to debug the Modbus acquisition software.
+ In the future, more functions can be added, such as automatic simulation of device operation.

## 4. Profile introduction
+ App.cfg specifies the device configuration file loaded when the program starts. The default is default.csv, which is in the config directory.
+ Default.csv configures all types in a farm and the ports that MODBUS listens to locally. The port is generally 15011502... Etc. to avoid conflict with the port used by other programs in the system.
+ Pcs.csv is configured to simulate the type and value of Modbus registers used by a PCS device.
+ Pcs_data.csv is the data to refresh per second.



## 5. Precautions
+ 1. address
Modbus protocol uses 1 more address than the actual address. The address displayed when viewing with modscan + 1. It is recommended to set offsetaddone = 1 in the configuration file app.cfg
+2. Value type
At present, the int type is processed as a 16 bit 1 register.
Use DWORD for 32-bit int
Float is treated as 32-bit.
Real is treated as a 32-bit floating-point number.


## 6. Todo
+ 1. The value of floating-point number displayed in register viewer
+ 2. Set the value of variable bit by bit

