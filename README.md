# Ruler - Yet Another Ruler

## Purpose

This application creates a Ruler on your screen which you can move around and use it to measure pixels. You can drag it around, size it, display additional lines or change its transparency.

![Image of Ruler with lines](https://praschl.github.io/Ruler/withLines.png)

## Actions

Move the window by dragging it around, and size it by dragging the borders.
You can also move it with Cursor-Keys; holding Shift moves faster, and holding Control even faster.

Clicking the Window adds a marker line at the mouse Position.

Right click for context menu.

Mousewheel to change transparency.

## Keyboard

### Ruler window
C ... to clear the lines.
O ... to toggle between horizontal and vertical orientation
H ... switch to horizontal orientation
V ... switch to vertical orientation

Alt-A ... About Window

Alt-F4 ... quit.

### About window
ESC ... Close about window
Alt-A ... Close about window

## Not implemented yet, but planned

Lots of settings

## Download

Download [Ruler v0.2](https://praschl.github.com/Ruler/Ruler.zip)

Some Virus Scanners currently seem to detect the "HEUR/QVM03.0.0000.Malware.Gen" on the executable in the zip file. However, http://stackoverflow.com/questions/33998715/visual-studio-2015-community-trojan-heur-qvm03-0-malware-gen states, that this scanner just flags even the most simple windows forms applications with this malware, all being false positives.

Also, Google Chrome identifies it as "suspicious download".

If you are afraid of viruses, you are free to review and compile the source yourself. In the meantime I'll look into how the false positive can be avoided.
