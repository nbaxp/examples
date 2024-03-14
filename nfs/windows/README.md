# Windows NFS Server

## exports.txt

```txt
D:\github\examples\nfs\windows\temp\folder1 > /folder1
D:\github\examples\nfs\windows\temp\folder2 > /folder2
```

## start.cmd

```cmd
WinNFSd.exe -pathFile D:\github\examples\nfs\windows\exports.txt
```

## 访问共享文件夹

`\\localhost\folder1`
