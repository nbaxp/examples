namespace Wta.Infrastructure.Common;

[Flags]
public enum Platform
{
    FreeBSD = 1,
    Linux = 2,
    OSX = 4,
    Windows = 8,
    All = FreeBSD | Linux | OSX | Windows
}
