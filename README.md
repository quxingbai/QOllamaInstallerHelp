# Ollama工具

> Ollama安装时候 默认安装到C盘，安装到其他位置需要在命令行执行额外参数。
>
> OllamaSetup.exe /DIR=D:\Ollama
>
> 这很麻烦...
>
> 其次想要配置要去注册表一个个写，比如 改变本地监听服务地址和端口的 OLLAMA_HOST
>
> 依旧很麻烦...
>
> 所以干脆做了个可视化的小工具...方便以后用了。



## 安装

1. 从官网下载exe安装文件
2. 在【安装文件】里输入或者选择 exe 的地址（如果是 × 就代表没找到文件，它也只能判断文件在不在，无法判断exe是否完好之类的）
3. 希望ollama安装到什么未知 输入到第三个框里
4. 把安装后的地址写到注册表里，比如把它的地址放到Path中，再比如声明OLLAMA_MODELS告诉他下载的模型放到这里（默认是地址+models的文件夹）
5. 监听地址也是个比较常用的东西，听Deepseek说默认的是127，局域网无法发现 所以它推荐了0.0.0.0

<img width="984" height="568" alt="image" src="https://github.com/user-attachments/assets/17bd20fa-5766-4be6-88e6-3e73d4ce58ea" />


## 注册表

> 有些时候Ollama注册表有缓存还是啥的，导致修改了注册表也不一定会生效...

**点击右上角的按钮能切换页面**，第一页是安装 第二页是让Deepseek生成的常用注册表项目，包含官方可能的默认值 和介绍，在项目的LocalData/OLLAMA_RegistryTooltips.json里写着，一些自定义注册表也可以仍Json中。

它会查一遍 ，如果本地注册表已经有这个项了就用蓝色标注出来 扔到前排，如果没写则是黑色。

鼠标移动到指定项 右下角的按钮就可以点击，然后弹出注册表修改界面 确认/取消

<img width="996" height="574" alt="image" src="https://github.com/user-attachments/assets/61de8973-6d48-47ff-b819-9937befe0b9e" />
<img width="976" height="566" alt="image" src="https://github.com/user-attachments/assets/1dda8f23-1956-4c90-8712-332fb196d131" />

## Json
在LocalData/OLLAMA_RegistryTooltips.json
<img width="1243" height="733" alt="image" src="https://github.com/user-attachments/assets/c0857192-b1fb-49e7-98c6-c07fe30b1521" />
