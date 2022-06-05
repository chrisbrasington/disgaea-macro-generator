# disgaea 1 (or probably anything) macro generator

## define commands

define commands within input.txt

```
; attack only
STARTCOMMAND ^SPACE::
ATTACK
ENDCOMMAND
```
which map a keyword to multiple keystrokes to the mapping dictionary
```
mappings.Add("ATTACK", new List<string>() { "ENTER", "S", "ENTER", "ENTER" });
```
which in turns generates out the auto-hot-key script with delays during the key press
```
^SPACE::
Send, {ENTER down}
sleep, 100
Send, {ENTER up}
sleep, 100
Send, {S down}
sleep, 100
Send, {S up}
sleep, 100
Send, {ENTER down}
sleep, 100
Send, {ENTER up}
sleep, 100
Send, {ENTER down}
sleep, 100
Send, {ENTER up}
sleep, 100
return

```
and in action...

![](.img/readme1.gif)

## define complex commands

let's you string together repetitive tasks like..
```
; left rotation
STARTCOMMAND ^A::
UP
ATTACK
DOWN
RIGHT
ATTACK
ENDTURN
ENDCOMMAND
```
![](.img/readme2.gif)