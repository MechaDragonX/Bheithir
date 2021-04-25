# Bheithir

## What is Bheithir?
Bheithir is a program that sets Discord Rich Presence (RPC) status for various emulators. 

## What emulators are supported right now?
- [DOSBox](https://www.dosbox.com/)
    - [MS-DOS](https://en.wikipedia.org/wiki/MS-DOS) emulator primarilly designed for playing games
- [FCEUX](http://www.fceux.com/web/home.html)
    - [NES](https://en.wikipedia.org/wiki/Nintendo_Entertainment_System)
- [SNES9X](http://www.snes9x.com/)
    - [SNES](https://en.wikipedia.org/wiki/Super_Nintendo_Entertainment_System)
- [Fusion](https://segaretro.org/Kega_Fusion)
    - SEGA 8 and 16-bit systems including the [Master System / Mark III](https://en.wikipedia.org/wiki/Super_Nintendo_Entertainment_System) and [Genesis / Mega Drive](https://en.wikipedia.org/wiki/Sega_Genesis)
- [VBA-M](https://github.com/visualboyadvance-m/visualboyadvance-m)
    - [Game Boy](https://en.wikipedia.org/wiki/Game_Boy), [Game Boy Color](https://en.wikipedia.org/wiki/Game_Boy_Color), [Game Boy Advance](https://en.wikipedia.org/wiki/Game_Boy_Advance)

## What emulators will be supported in the future?
- [PCSX2](https://pcsx2.net/)
    - [PlayStation 2](https://en.wikipedia.org/wiki/PlayStation_2)
- [DeSmuME](http://desmume.org/) and [melonDS](http://melonds.kuribo64.net/)
    - [Nintendo DS](https://en.wikipedia.org/wiki/Nintendo_DS)
- [ePSXe](https://www.epsxe.com/)
    - [PlayStation 1](https://en.wikipedia.org/wiki/PlayStation_(console))
- [MAME](https://www.mamedev.org/)
    - Emulates a variety of arcade systems and vintage computers
    - [List of supported systems](https://emulation.gametechwiki.com/index.php/MAME_compatibility_list)
- Other popular emulators for the consoles supported and consoles planned to be supported
- [RetroArch](https://www.retroarch.com/)
    - The most popular and widely supported multi-system emulator, as it has been ported to everything imaginable
    - [List of supported systems](https://emulation.gametechwiki.com/index.php/Libretro#Cores)
- [BizHawk](http://tasvideos.org/BizHawk.html)
    - Multi-system, focused on TAS creation, supports [Libretro](https://docs.libretro.com/) cores (less accurately, but more popularly, referred to as [RetroArch](https://www.retroarch.com/) cores)
    - [List of supported systems](https://emulation.gametechwiki.com/index.php/BizHawk#Supported_systems)

## How do I run it?
- You can get the latest binary from the [releases](https://github.com/MechaDragonX/Bheithir/releases) pages.
- **All builds are framework-dependent!** Please follow the instructions for your OS to download .NET [here](https://dotnet.microsoft.com/download/dotnet/).
- Bheithir runs on it's own and is entirely seperate from all emulators supported. **This is not a plugin!**
- Bheithir works with all versions of these emulators after the first release where support was added.
    - Since this program is dependant on the text in the window title, it is possible earlier or future releases are not supported.
    - If the presence does not look right, please leave an [issue](https://github.com/MechaDragonX/Bheithir/issues)!

### Earliest Known Supported Versions
- OS
    - Windows 7 x86 (32-bit)
        - x86_64 (64-bit) and ARM versions of Windows are supported as well, but builds target x86.
    - Anything older is not supported whatsoever.
- .NET
    - v5.0
    - Anything older is not supported whatsover.
- DOSBox
    - x86 (32-bit)
    - v0.74-3
- FCEUX
    - x86 (32-bit)
    - v2.2.3
- Snes9x
    - x86 (32-bit), x86_64 (64-bit)
    - v1.60
- Fusion
    - x86 (32-bit)
    - v3.64
- VBA-M
    - x86 (32-bit), x86_64 (64-bit)
    - v2.1.4

## Is it functional?
At the moment it should be... Check the [issues](https://github.com/MechaDragonX/Bheithir/issues) tab for any issues.

## What the hell is this name?
Since the first emulator I supported, DOSBox, is an emulator for MS-DOS, an American operating system, I decided to go with a name influenced by British mythology and folklore, as America shares a lot with Britian culturally. Bheithir is a dragon who was though to be a representation of the [Celtic goddess of storms, winter, death, and darkness](https://books.google.com/books?id=XPoRSLTkhtsC&pg=PA179&lpg=PA179&dq=bheithir+scotland&source=bl&ots=h2FaVFtlKN&sig=ACfU3U0fPo-tvlrPfv6CW9OVvdrHlOE66g&hl=en&ppis=_e&sa=X&ved=2ahUKEwjKiInks77oAhWuGTQIHZbbAToQ6AEwCXoECB4QAQ#v=onepage&q=bheithir%20scotland&f=false). It is said it once resided in the Scottish mountain, [Beinn a' Bheithir](https://en.wikipedia.org/wiki/Beinn_a%27_Bheithir), the Mountain of the Thunderbolt, where it was tricked and killed after terrorizing the villagers at the bottom of the mountain for a long time.

## When will this be done?
Well...it's functional, but it doesn't look the way I want it to or have all the features I want it to...so let's say it's *in progress*.

## Upcoming Features
- A program icon
- Running as a system tray application so that it can update the background and you don't need to worry about it

## Special Thanks
Special thanks to [BloodDragooner](https://github.com/BloodDragooner) for giving me the idea to do this sort of thing with [SumatraPDF](https://github.com/sumatrapdfreader/sumatrapdf)! A lot of code was borrowed from that project, Chilong, so check it out [here](https://github.com/MechaDragonX/Chilong)!
