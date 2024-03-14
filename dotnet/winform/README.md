# winform

<https://github.com/dotnet/winforms>

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
csproj-->UseWindowsForms
```

## 二、UI 组成

由多个继承自 System.Windows.Forms.Form 的窗体组成,每个窗体包含多个继承自 System.Windows.Forms.Control 的控件

```mermaid
graph LR
Main-->Application.Run-->System.Windows.Forms.Form
```

## 三、窗体设计

```mermaid
graph LR
Form1-->Form1.cs-->手动维护
Form1-->Form1.Designer.cs-->自动生成
Form1-->Form1.resx-->资源文件
```

## 四、 控件、组件和对话框

### 1. 自定义窗体和控件

```mermaid
graph LR
System.Windows.Forms.Control-->ScrollableControl-->ContainerControl-->Form-->自定义窗体
ContainerControl-->UserControl-->自定义控件
```

### 2. 内置控件

```mermaid
graph LR
Control(System.Windows.Forms.Control)-->ScrollableControl-->ToolStrip
Control-->DataGridView
ToolStrip-->BindingNavigator-->配合BindingSource一起使用
Control-->TextBoxBase-->TextBox
TextBoxBase-->RichTextBox
TextBoxBase-->MaskedTextBox
Control-->Label-->LinkLabel
ToolStrip-->StatusStrip
Control-->ProgressBar
Control-->WebBrowserBase-->WebBrowser
Control-->ListControl-->ListBox-->CheckedListBox
ListControl-->ComboBox
ScrollableControl-->ContainerControl-->UpDownBase-->DomainUpDown
Control-->ListView
UpDownBase-->NumericUpDown
Control-->TreeView
Control-->PictureBox
Control-->ButtonBase-->CheckBox
ButtonBase-->RadioButton
Control-->TrackBar
Control-->DateTimePicker
Control-->MonthCalendar
ToolStrip-->MenuStrip
ToolStrip-->ToolStripDropDown-->ToolStripDropDownMenu-->ContextMenuStrip
ButtonBase-->Button
ScrollableControl-->Panel
Control-->GroupBox
Control-->TabControl
ContainerControl-->SplitContainer
Panel-->TableLayoutPanel
Panel-->FlowLayoutPanel
```

### 4. 内置组件

```mermaid
graph LR
Component(System.ComponentModel.Component)-->BindingSource
Component-->imageList
Component-->NotifyIcon
Component-->HelpProvider
Component-->ToolTip
Component-->SoundPlayer
Component-->BackgroundWorker
```

### 3. 内置对话框

```mermaid
graph LR
CommonDialog(System.Windows.Forms.CommonDialog)-->ColorDialog
CommonDialog-->FontDialog
CommonDialog-->FileDialog-->OpenFileDialog
CommonDialog-->PrintDialog
System.Windows.Forms.Control-->ScrollableControl-->ContainerControl-->Form-->PrintPreviewDialog
CommonDialog-->FolderBrowserDialog
FileDialog-->SaveFileDialog
```
