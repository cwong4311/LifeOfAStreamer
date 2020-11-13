VAR game_type = "mem"
VAR viewers = 1

Man, haven't seen this game in a while.
How are things going?
* Alright
    That's good. 
    -> Streaming
* \[Ignore\] 
    -> Streaming

== Streaming ==
L_D
Btw, don't think I've seen you before
Are u new to streaming?
* Yeah
    Cool. Yea, streaming's really something.
    Takes a while to get rolling.
    Good luck dude.
    ** Thanks 
        -> GameTalk
    ** \[Ignore\] 
        -> GameTalk
* Nah
    Ah rip, soz.
    Good luck dude, gettin your name out there can be pretty hard.
    ** Thanks 
        -> GameTalk
    ** \[Ignore\] 
        -> GameTalk
    
== GameTalk ==
L_D
How you finding the game?
* Not too bad
    Haha, that's cool.
    I've been looking for a fun timekiller.
    Would you recommend this?
    ** Yeah
        Nice!
        -> Compliment
    ** Not really
        Haha, nws.
        -> Compliment
* Not that good
    Oh, that's a shame..
    Why'd you stream this then?
    ** Just cuz
        I see..
        -> Compliment
    ** Dunno
        I see..
        -> Compliment
    ** \[Ignore\]
        -> Compliment

== Compliment ==
L_D
{game_type == "plat":
    Nice Jump!
    L_D
    This game looks hard lmao
}
{game_type == "inv":
    Gotta keep dodging
    L_D
    Nice shot!
}
{game_type == "mem":
    Remembering all the cards is so hard
    L_D
    Nice matching kek
}
-> Conclusion

== Conclusion ==
L_D
Aight, gotta hop off now.
You stream often?
* Yeah
    cool
    I'll catch you next stream then!
    -> Fin
* Not really
    rip
    Well, maybe next time you're on.
    -> Fin
* Dunno
    I see
    Well, whenever you stream again.
    -> Fin
    
== Fin ==
cya
S_D
I_M_That was a pretty cool guy
S_D
I_M_I think that's good enough for one day
S_D
E_C
-> END

