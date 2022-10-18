# Finder
### Fast multi-thread find in files system for unity
Finder helps you finding those implicit method usages that IDEs such as rider cannot find by file indexing

## Problem
Unity allow us to assign public instance methods invokation to UnityEvents, EventTriggers, Buttons, Animation Events and more.
And some of those implicit usages cannot be found by JetBrains Rider.
So when refactoring legacy code, its a pain to know if a apparently non-used method is truly not being used, or just being invoked implicitly.

## Solution
In the past I was using notepad++ to find those usages, but for larger projects, as notepad++ find in files is single threaded, it just takes to much.
So I decided to design a robust finder that could help in those cases.
