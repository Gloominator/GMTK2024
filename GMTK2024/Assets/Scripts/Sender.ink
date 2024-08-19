
-> main

=== main ===
Hello, welcome to Sender.
* [How do I play this?]
    You will be deciding people's fates between Heaven and Hell.
    ** [Cool, send me my first person!]
    -> satan
* [I know how to play already.]
    -> carlos

-> DONE 

VAR satanName = ""
// satanName is empty
=== satan === 
Hello, my name is S... Uh, John. Better be quick, there's a few hundred people behind me that you have to get through.
    ~ satanName = "John"
    // changes satanName to John on dialogue box

 * [What's your actual name?]
    You got me! I'm SATAN. Now, can you stop messing around and send me to heaven? I'm trying to TERRORIZE some angels! How? Just press the "Judge" button and send me to heaven. In the future, if you think anyone's lying, just click whatever lines pop up on the left and fact check them with the scale on the right.
    
        ~ satanName = "Satan"
        // changes satanName to Satan on dialogue box
        ** [...]
            What are you waiting for? Hurry up and Judge me! By the way, you don't have to wait for me, or anyone for that matter, to finish talking before you Judge... Wink.
            *** [Judge]
                -> judgeSatan
    

 * [How did you lead your life]
        I spend my life punishing the wicked. Haha, you believe me so easily, don't you?
        ** [How so?]
        Anyway, send me to Heaven, don't be messing around with me. 
         *** [What's your actual name?]
            You got me! I'm SATAN. Now, can you stop messing around and send me to heaven? I'm trying to TERRORIZE some angels! How? Just press the "Judge" button and send me to heaven. In the future, if you think anyone's lying, just click whatever lines pop up on the left and fact check them with the scale on the right.
    
            -> nothingToSay
        -> judgeSatan
        -> newStory
        
=== nothingToSay ===
    I've got nothing else to say to you
    -> judgeCarlos
    
=== judgeCarlos ===
* [I'm sending you to Heaven!]
-> newClientBad
* [I'm sending you to Hell!]
-> newClientGood

=== newClientGood ===
Good job sending them where they belong. I'll send the next one your way.

=== newClientBad ===
Bad! You sent them to the wrong place. I'm sending the next one your way. Don't mess this up!
-> nextClient

=== nextClient ===
-> mathUgh

=== susan ===
Hey, I´m Susan.
-> DONE

=== mathUgh ===
Here's your next client.
+ [nextClient]
    {RANDOM(1,3)}
    -> susan


-> main
->DONE
=== newStory ===

Congrats, you got through Satan. We always vet all of our senders through him to make sure you're up to the task ahead. Now, here's your first real client. # Next client comes to the table.

    -> carlos
    
=== carlos ===
Hey, I'm Carlos. Not gonna lie, I don't really get what's going on but this place is mad cool.
    * [Thank you.]
        Oh, no problem, I'm just saying, this whole place is sick as hell. I mean, wait. Where am I, again?
    ** [You are being Judged for your life.]
        Cool, I mean, that's kinda scary I guess. So how do y'all do that? Do I gotta fill out a survey, maybe there's a QR code I could scan?
        *** [How did you lead your life, Carlos?]
            Yeah I’d say I lived a pretty nice life. I don’t think I should go to hell for anything, maybe that one time I killed a bug but you know, that thing was big and I was really scared and my girlfriend would’ve cried if she had to.
            **** [Where is your girlfriend right now?]
                She's probably at home, wondering when I'll get back from the store. Man, that car crash was wild.
                ***** [Car crash?]
                    Sick, I mean, in that case, no I did die in a car crash. Six truck pile-up. Very tragic.
                    ****** [Are you lying to me, Carlos?] 
                        No, of course not? I mean, no?
                        -> judgeCarlos
            **** [Are you lying to me, Carlos?] 
                No, of course not? I mean, no?
                -> judgeCarlos
            **** [Have you ever harmed your fellow man?]
                No, my mom would kill me if I ever did anything like that.
                ***** [Are you lying to me, Carlos?]
                        No, of course not? I mean, no?
                        -> judgeCarlos
                

=== judgeSatan ===
Hey, that's not very nice of you.
* [I'm sending you to Hell!]
    Alright, alright, enough tricks, I'll stop holding up the line for you. Next guy's the real deal. Promise. Or ia he? Muahaha. # ANGEL GUIDE comes up to you.
    -> newStory
* [I'm sending you to Heaven!]
    Perfect, perfect. Just perfect!
    -> immediateFail

=== immediateFail ===
You failed your job already? How'd you do that?
-> main