RoxioGameCap
============

A no-frills recording tool for the Roxio GameCap HD PRO.

[Download the latest release](https://github.com/warrenseymour/RoxioGameCap/raw/master/Releases/RoxioGameCap-latest.zip)

About
-----

The Roxio GameCap HD PRO is a great bit of kit, capable of taking an HDMI or Component input (up to 1080p30), performing H264 encoding in hardware, and passing the encoded stream to a PC via USB for writing to disk and/or livestreaming to Twitch.tv, all at a very reasonable price.

Unfortunately the device is only compatible with the bundled software, which is a crash-prone resource hog. This has left myself and many other owners feeling like they've bought a turkey.

This tool aims to be a lightweight and reliable replacement for the bundled software.

Current Features
----------------

- Preview decoded capture stream
- Record capture stream to an M2TS file on disk

Current Limitations
-------------------

- Only supports HDMI 1080p input
- Manual configuration of recording directory

Planned Features
----------------

- User-friendly recording configuration
- Livestream to Twitch.tv
- Disable preview during record/livestream (to save CPU)
- Record and livestream simultaneously
- Livestream at 1080p30

Requirements
------------

- .NET Framework version 4.0
- The bundled software and drivers must be installed, but it is *not* necessary to have the bundled capture software running whilst using this tool.
- RoxioGameCap.exe.config must be configured manually. Change the contents of the `<value>` node under `<RoxioGameCap.Properties.Settings>` to the directory you wish to save your recordings to. Remember to use double-slashes in the path as well as a trailing double-slash.

Building
--------

This project was written in VS2010 and requires the [DirectShowNet](http://directshownet.sourceforge.net/) library to be available on your system. Update the reference to DirectShowLib-2005 to where `DirectShowLib-2005.dll` resides.

Reporting issues
----------------

This software is offered without warranty, but if you have any problems with this software I will do whatever I can to fix them. Please [raise a ticket](https://github.com/warrenseymour/RoxioGameCap/issues) and include as much information as possible so that I can help you.
