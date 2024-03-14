# WPF

<https://github.com/microsoft/WPF-Samples>

1. 项目配置
1. UI 组成
1. 窗体设计
    1. 自定义窗体和控件
1. 内置控件、组件和对话框
    1. 内置控件
    1. 内置组件
    1. 内置对话框

## 一、项目配置

```mermaid
graph LR
csproj-->Microsoft.NET.Sdk
csproj-->WinExe
csproj-->net7.0-windows
csproj-->UseWPF
```

## 二、UI 组成

由多个继承自 System.Windows.Window 的窗体组成,每个窗体包含多个继承自 System.Windows.UIElement 的 WPF 控件;使用 xaml 描述界面

```mermaid
graph LR
App-->App.xaml.cs-->System.Windows.Application
App-->App.xaml-->MainWindow(StartupUri=MainWindow.xaml)
MainWindow-->MainWindow.xaml
MainWindow-->MainWindow.xaml.cs-->System.Windows.Window
```

### XAML

[标记扩展](https://learn.microsoft.com/zh-cn/dotnet/desktop/wpf/advanced/wpf-xaml-extensions?view=netframeworkdesktop-4.8&viewFallbackFrom=netdesktop-7.0)

```mermaid
graph LR
XAML-->XML-->XML命名空间(命名空间)-->C#命名空间
XML-->标记-->v1("C#命名空间中的应用、窗体、控件等Class类型名称")
XML-->属性-->v2("C#对应Class类型的特性")
XAML-->根元素-->根元素类型-->Application-->应用
XAML-->标记扩展-->v3(属性值使用大括号)-->StaticResource
v3-->DynamicResource
v3-->Binding
根元素类型-->Window-->窗体
根元素类型-->Page-->页面
根元素-->命名空间-->xmlns-->StartupUri-->应用程序启动时自动打开的UI
命名空间-->xmlns:x-->x:Class-->关联的类类型完全限定名
命名空间-->xmlns:d-->d:DesignHeight
xmlns:d-->d:DesignWidth
命名空间-->xmlns:mc-->mc:Ignorable-->仅在XAML设计器中显示
命名空间-->xmlns:local-->将命名空间映射到前缀local
```

## 三、窗体设计

### 自定义窗体和控件

```mermaid
graph LR
自定义窗体-->MainWindow-->MainWindow.xaml
MainWindow-->MainWindow.xaml.cs-->System.Windows.Window
自定义控件-->UserControl1-->UserControl1.xaml
UserControl1-->UserControl1.cs-->System.Windows.Controls.UserControl
```

### 布局



## 四、 控件、组件和对话框

### 1. 自定义窗体和控件

```mermaid
graph LR
System.Windows.UIElement-->FrameworkElement-->Control-->ContentControl-->Window-->自定义窗体
ContentControl-->UserControl-->自定义控件
FrameworkElement-->Page-->自定义页
```

### 内置控件
