# modbusPlcSimulator
modbus服务器模拟程序

## 一、用途
+ 此程序主要用来模拟modbus设备，通过配置文件，可以一次模拟多台设备，一键启机和停机。
+ 支持查看寄存器内部的值和modbus客户端访问日志。
+ 通过提供*_data.csv文件，支持数据自动刷新。

程序运行窗口
![screenshot.png](https://raw.githubusercontent.com/alongL/modbusPlcSimulator/master/imgs/screenshot.png "运行窗口")
寄存器显示
![screenshot-register.png](https://raw.githubusercontent.com/alongL/modbusPlcSimulator/master/imgs/screenshot-register.png "寄存器窗口")



## 二、使用方法
+ 编译后将config目录复制到可执行文件exe同一个目录下，正确配置csv文件后，即可使用
+ 点击播放窗口的播放按钮后数据才会根据csv进行刷新，刷新时数据是从 _data.csv中选取一行进行刷新。
+ 不要关闭播放窗口，目前关闭播放窗口会结束数据刷新线程。
+ 可以在日志窗口查看modbus协议底层访问情况，modbus读写都有记录。
+ 直接前往[Release页面](https://github.com/alongL/modbusPlcSimulator/releases)下载此程序

## 三、原理
+ plc对外提供的是modbus协议，可以通过提供modbus服务模拟。以前主要是用modsim进行模拟，这种方式一次只能模拟一台设备，且操作起来相对比较麻烦。此程序操作简单，使用方便，只需运行，即可以同时模拟多台设备，方便进行modbus采集软件调试。
+ 以后还可以添加更多的功能，比如自动模拟设备运行等。

## 四、配置文件介绍 
+ app.cfg 指定程序启动时加载的设备配置文件，默认是default.csv，在config目录下。
+ default.csv配置一个风场中所有的类型和在本地模拟modbus所监听的端口。端口一般用1501,1502 ...等等，避免与系统中其他程序所使用的端口相冲突。
+ PCS.csv 配置模拟一台PCS设备所用的modbus寄存器的类型和值。
+ PCS_data.csv 是每秒要刷新的数据。

## 五、注意事项
+ 1.地址
 modbus协议所用的地址比实际地址多1。 用modscan查看时显示的地址+1，建议在配置文件app.cfg中设置 offsetAddOne=1

+ 2.值类型
 目前INT型是作为16位1个寄存器处理。
 32位INT请用DWORD
 FLOAT当作32位处理。
 REAL作为32位浮点数处理。


## 六、TODO 
+ 1.寄存器察看器中显示浮点数的数值
+ 2.按位设置变量的值



