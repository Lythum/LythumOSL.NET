What is this?
==============================

LythumOSL - Lythum Open Source Library is used in many our commercial, non-commercial & freeware projects. 

It had several versions since begin, around 2005 year. A lot of various and different development concepts in long run. In begin it was named as Alpha library (Alpha.Core, Alpha.UI, Alpha.IO and etc...) And a lot of our free and commercial software still using it. 


This library at the end of development will have all features of all previous versions of this family libraries. Just namespaces changed a lot of times, dll structure and etc. But will be possible to port all software, which is using this family libraries to this version. And objective is to port everything, with time, only on this open source library. To prevent usage and support of several versions of this family libraries at once.



Why i did it as an opensource?
==============================

Main problem was that a lot of various free and commercial projects in long run started to use those libraries. I wanted to avoid any possible inconvenience, that some customer can understand those libraries as part of their commercial software as in some cases (eg. government) they're buying software with source code. And i should include all source code. So i had only one choice, to make core libraries as opensource. In this way everybody can access them who needs it. That's why i am choosed Apache 2.00 license, as this code can be used in commercial and non-commercial projects. 

If you see that those libraries are good for you. You can use them as well. Just you're unable to sell those libraries, but you can sell software which using those libraries. While libraries itself are free and opensource. That is my point. As well as you can use portions of this code on your own.

In long run, developing those libraries i've used a lot of various information from internet, stackechange, code project and etc... And they include a lot of working solutions, where i often used knowledge of other developers. 


Concept of development using LythumOSL
==============================

I always had oposite opinion regarding some popular technical ideologies, concepts. As in today world we need more and more developers and in many cases technologies starting to be more and more complex. Requires more and more code to achieve solution. My concept is similar which is used in Army. As better man is armed as more he can do alone. Eg. if to give to soldier assault rifle, he can fight against several enemies. If to give to him machinegun, he can fight against much more enemies rather than with assault rifle. 

Similar concept is here. To make strong system core, which solve most of technical stuff and you don't need eg. every time to initialize SQLConnections or SQLCommands when you access database and etc.. To write your software business logic parts with as possible less lines of code. That code was as possible smaller, where all functionality is hidden in core libraries.

Better all bugs will be in libraries and will be fixed at once with some fix, rather than copy-pasted in whole solution million of times... 

And this concept required to do solutions which in some cases are against current popular or best practices software engineering solutions and concepts. 

I don't care what do they think. As for me priority is to not waste to much of time on achievement of solution. To make, that single developer was able to work efficient and even to rival solo with software development companies. That he not needed to waste to much of time on technologies or their implementation. But it required much more of work in libraries side.

TODO:
=======
Still strong code review is needed. As some classes was researched and not used at the end at ll. As i've found eg. different solution.  And some portions of code are commented instead of removal. Maybe should be good to cleanup them.

P.S.

Main rule! Less code = Less bugs! 
